using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        private iXML.Service iXMLSrv = new iXML.Service();

        public Form1()
        {
            InitializeComponent();
            frmLogin loginDiag = new frmLogin();
            DialogResult result = loginDiag.ShowDialog();

            switch (result)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    Environment.Exit(0);
                    break;
            }
        }

        private void btnClTest_Click(object sender, EventArgs e)
        {
            bool rtn = false;
            try
            {
                btnClTest.Enabled = false;

                iXMLSrv.SetBaseURL(LoginInfo.URL);
                iXMLSrv.SetUserInfo(LoginInfo.User, LoginInfo.GetPassword());
                iXMLSrv.SetIpcInfo("/tmp/xmlservicei");

                rtn = iXMLSrv.ExecuteCommand("SNDMSG MSG('TEST XMLSERVICE') TOUSR(" + LoginInfo.User + ")");

                if (rtn)
                {
                    txtOutput.Text = "Command Was Successful. XML Response: " + Environment.NewLine + iXMLSrv.GetLastXmlResponse();
                }
                else
                {
                    txtOutput.Text = "Command Failed. XML Response: " + Environment.NewLine + iXMLSrv.GetLastXmlResponse();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Error: " + ex.Message;
            }
            finally
            {
                btnClTest.Enabled = true;
            }

        }

        private void btnKillService_Click(object sender, EventArgs e)
        {
            bool rtn = false;
            try
            {
                btnKillService.Enabled = false;

                iXMLSrv.SetBaseURL(LoginInfo.URL);
                iXMLSrv.SetUserInfo(LoginInfo.User, LoginInfo.GetPassword());
                iXMLSrv.SetIpcInfo("/tmp/xmlservicei");

                rtn = iXMLSrv.KillService();

                if (rtn)
                {
                    txtOutput.Text = "Service shutdown was successful. XML Response: " + Environment.NewLine + iXMLSrv.GetLastXmlResponse();
                }
                else
                {
                    txtOutput.Text = "Service shutdown failed. XML Response: " + Environment.NewLine + iXMLSrv.GetLastXmlResponse();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Error: " + ex.Message;
            }
            finally
            {
                btnKillService.Enabled = true;
            }
        }
    }
}
