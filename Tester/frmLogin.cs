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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtUrl.Text = Settings.Default.URL;
            txtUser.Text = Settings.Default.User;
            //txtPassword.Text = LoginInfo.Decrypt(Settings.Default.Password);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            LoginInfo.URL = txtUrl.Text;
            LoginInfo.User = txtUser.Text;
            LoginInfo.SetPassword(txtPassword.Text);

            if (chkRemember.Checked)
            {
                Settings.Default.URL = txtUrl.Text;
                Settings.Default.User = txtUser.Text;
                Settings.Default.Password = LoginInfo.GetEncPassword();

                Settings.Default.Save();
            }
        }
    }
}
