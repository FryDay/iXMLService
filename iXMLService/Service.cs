using System.Net;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool SetHttpTimeout(int timeout)
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
    }
}
