using System.Net;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iXML
{
    /// <summary>
    /// This class is used to call the XMLSERVICE program on an AS/400 - IBM i system to query a database or call a command / 
    /// program and return the data in a .Net usable format such as a DataSet or DataTable. 
    /// This class should work with V5R4 and above. 
    /// </summary>
    public class Service
    {
        private string LastError;
        private DataSet DS1;
        private DataTable ColumnDefinitions;
        private int IColumnCount;
        private int IRowCount;
        private DataTable ReturnDataTable;
        private bool XMLIsLoaded = false;
        private string BaseURL = "";
        private string DB2Parm = "db2=@@db2value&uid=@@uidvalue&pwd=@@pwdvalue&ipc=@@ipcvalue&ctl=@@ctlvalue&xmlin=@@xmlinvalue&xmlout=@@xmloutvalue";
        private DataTable ProgramResponse;
        private string User = "";
        private string Password = "";
        private string IPCInfo = "";
        private string DB2Info = "*LOCAL";
        private string LastHTTPResponse = "";
        private string LastXMLResponse = "";
        private int HttpTimeout = 60000;

        /// <summary>
        /// Program call parameter structure
        /// </summary>
        public struct PgmParm
        {
            public string type;
            public string value;

            public PgmParm(string type, string value)
            {
                this.type = type;
                this.value = value;
            }
        }

        /// <summary>
        /// Program call return structure
        /// </summary>
        public struct RtnPgmCall
        {
            public bool success;
            public List<PgmParm> parms;
        }

        /// <summary>
        /// Stored procedure paramter list
        /// </summary>
        public struct ProcedureParm
        {
            public string type;
            public string value;
            public string name;

            public ProcedureParm(string type, string value, string name)
            {
                this.type = type;
                this.value = value;
                this.name = name;
            }
        }

        /// <summary>
        /// Stored procedure return structure
        /// </summary>
        public struct RtnProcedureCall
        {
            public bool success;
            public List<PgmParm> parms;
        }

        /// <summary>
        /// Set HTTP timeout for XMLSERVICE requests
        /// </summary>
        /// <param name="iTimeout">HTTP timeout in milliseconds</param>
        /// <returns>True == Success, False == Fail</returns>
        public bool SetHttpTimeout(int timeout = 60000)
        {
            try
            {
                this.LastError = string.Empty;
                this.HttpTimeout = timeout;
                return true;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Set base URL to XMLCGI program.
        /// Example: http://1.1.1.1:30000/cgi-bin/xmlcgi.pgm
        /// Set this value one time each time the class is instantiated.
        /// </summary>        
        /// <param name="url"></param>
        /// <returns>True == Success, False == Fail</returns>
        public bool SetBaseURL(string url)
        {
            try
            {
                this.LastError = string.Empty;
                this.BaseURL = url;
                return true;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Set base user info for XMLCGI program calls.
        /// Set this value one time each time the class is instantiated.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>True == Success, False == Fail</returns>
        public bool SetUserInfo(string user, string password)
        {
            try
            {
                this.LastError = string.Empty;
                this.User = user;
                this.Password = password;
                return true;             
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Defaults to /tmp/rjsxmlservice so this call is not needed unless you want to use custom IPC info
        /// </summary>
        /// <param name="ipcInfo"></param>
        /// <returns>True == Success, False == Fail</returns>
        public bool SetIpcInfo(string ipcInfo = "/tmp/rjsxmlservice")
        {
            try
            {
                this.LastError = string.Empty;
                if (ipcInfo != default(string) && ipcInfo != string.Empty)
                {
                    this.IPCInfo = ipcInfo;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Set DB2 system info. Defaults to *LOCAL to access local system database
        /// </summary>
        /// <param name="db2Info"></param>
        /// <returns>True == Success, False == Fail</returns>
        public bool SetDb2Info(string db2Info = "*LOCAL")
        {
            try
            {
                this.LastError = string.Empty;
                this.DB2Info = db2Info;
                return true;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// This function gets the XML DataSet of data that was loaded with LoadDataSetFromXMLFile
        /// </summary>
        /// <returns>Dataset of data or null if no data set</returns>
        public DataSet GetDataSet()
        {
            try
            {
                this.LastError = string.Empty;

                if (!this.XMLIsLoaded)
                {
                    throw new Exception("No XML data is currently loaded. Call LoadDataSetFromXMLFile to load an XML file first.");
                }

                return this.DS1;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Returns last error message
        /// </summary>
        /// <returns>Last error message</returns>
        public string GetLastError()
        {
            try
            {
                return this.LastError;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Return last XML message
        /// </summary>
        /// <returns>Last XML message</returns>
        public string GetLastXmlResponse()
        {
            try
            {
                return this.LastXMLResponse;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This function gets the DataTable of data loaded from XML with LoadDataSetFromXMLFile
        /// </summary>
        /// <returns>Data table of data or null if no data set</returns>
        public DataTable GetDataTable()
        {
            try
            {
                this.LastError = string.Empty;

                if (!this.XMLIsLoaded)
                {
                    throw new Exception("No XML data is currently loaded. Call LoadDataSetFromXMLFile to load an XML file first.");
                }

                return this.ReturnDataTable;
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// This kills the currently active IPC service instance on IBM i system.
        /// These jobs are all marked as XTOOLKIT and can be seen by running WRKACTJOB JOB(XTOOLKIT).
        /// Note: If you will be doing a lot of work, you can leave the job instantiated. Otherwise kill the XTOOLKIT job
        /// if you're done using it. 
        /// </summary>
        /// <returns>True - kill succeeded. Return XML will always be blank so we will always return true most likely.</returns>
        public bool KillService()
        {
            string db2Parm = this.DB2Parm;
            string rtnXml = string.Empty;

            try
            {
                string xmlIn = "<?xml version='1.0'?>" +
                               "<?xml-stylesheet type='text/xsl' href='/DemoXslt.xsl'?>" +
                               "<script></script>";
                string xmlOut = "500000";

                this.LastError = string.Empty;

                db2Parm = SetDb2Parm(xmlIn, xmlOut);
                db2Parm = db2Parm.Replace("@@ctlvalue", "*immed");
                db2Parm = db2Parm + "&submit=*immed end (kill job)"; //TODO: Probably could be done better.

                rtnXml = HttpRequest(this.BaseURL, "POST", db2Parm);

                if (rtnXml == string.Empty)
                {
                    LastXMLResponse = rtnXml;
                    return true;                   
                }
                else
                {
                    throw new Exception(rtnXml);
                }
            }
            catch (Exception ex)
            {
                this.LastXMLResponse = rtnXml;
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// This function runs the specified IBM i CL command. The CL command can be a regular call or a SBMJOB command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>True == Success, False == Fail</returns>
        public bool ExecuteCommand(string command)
        {
            string db2Parm = this.DB2Parm;
            string rtnXml = string.Empty;

            try
            {
                string xmlIn = "<?xml version='1.0'?>" +
                               "<?xml-stylesheet type='text/xsl' href='/DemoXslt.xsl'?>" +
                               "<script><cmd>" + command + "</cmd></script>";
                string xmlOut = "32768";

                this.LastHTTPResponse = string.Empty;
                this.LastXMLResponse = string.Empty;
                this.LastError = string.Empty;

                db2Parm = SetDb2Parm(xmlIn, xmlOut);
                db2Parm = db2Parm.Replace("@@ctlvalue", "*sbmjob");

                rtnXml = HttpRequest(this.BaseURL, "POST", db2Parm);

                if (rtnXml.Contains("+++ success")) // TODO: Hardcoded string for measuring success? o_o
                {
                    this.LastXMLResponse = rtnXml;
                    return true;
                }
                else
                {
                    throw new Exception(rtnXml);
                }
            }
            catch (Exception ex)
            {
                this.LastXMLResponse = rtnXml;
                this.LastError = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Sets non-changing db2 parms.
        /// </summary>
        /// <param name="xmlIn"></param>
        /// <param name="xmlOut"></param>
        /// <returns>DB2 parm</returns>
        private string SetDb2Parm(string xmlIn, string xmlOut)
        {
            string db2Parm = this.DB2Parm;

            db2Parm = db2Parm.Replace("@@db2value", this.DB2Info);
            db2Parm = db2Parm.Replace("@@uidvalue", this.User);
            db2Parm = db2Parm.Replace("@@pwdvalue", this.Password);
            db2Parm = db2Parm.Replace("@@ipcvalue", this.IPCInfo);
            db2Parm = db2Parm.Replace("@@xmlinvalue", xmlIn);
            db2Parm = db2Parm.Replace("@@xmloutvalue", xmlOut);

            return db2Parm;
        }
    
        /// <summary>
        /// Make an HTTP request with selected URL and get response
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="timeout"></param>
        /// <returns>XML response or error string starting with "ERROR"</returns>
        private string HttpRequest(string url, string method, string data)
        {
            string responseData = string.Empty;

            try
            {
                this.LastHTTPResponse = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*";
                request.AllowAutoRedirect = true;
                request.Credentials = new NetworkCredential(this.User, this.Password);
                request.UserAgent = "iXMLService/1.0";
                request.Timeout = this.HttpTimeout;
                request.Method = method;

                if (request.Method == "POST")
                {                    
                    Byte[] postBytes = new ASCIIEncoding().GetBytes(data);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = postBytes.Length;

                    Stream post = request.GetRequestStream();
                    post.Write(postBytes, 0, postBytes.Length);
                    post.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                this.LastHTTPResponse = response.StatusCode + " " + response.StatusDescription;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader responseStream = new StreamReader(response.GetResponseStream());
                    responseData = responseStream.ReadToEnd();
                }

                response.Close();
            }
            catch (WebException ex)
            {
                responseData = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();                
            }

            return responseData;
        }
    }
}
