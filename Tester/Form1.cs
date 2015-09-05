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
        }

        private void btnClTest_Click(object sender, EventArgs e)
        {
            bool rtn = false;
            try
            {
                btnClTest.Enabled = false;

                iXMLSrv.SetBaseURL((txtUrl.Text.Trim()));
                iXMLSrv.SetUserInfo(txtUser.Text.Trim(), txtPassword.Text.Trim());
                iXMLSrv.SetIpcInfo("/tmp/xmlservicei");

                rtn = iXMLSrv.ExecuteCommand("SNDMSG MSG('TEST XMLSERVICE') TOUSR(" + txtUser.Text.Trim() + ")");

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
    }
}
