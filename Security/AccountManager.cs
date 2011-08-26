using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eryan.Security
{
    public partial class AccountManager : Form
    {
        
        public AccountManager()
        {
            InitializeComponent();
        }

        public string Username
        {
            get
            {
                return username.Text;
            }
        }

        public string Password
        {
            get
            {
                return password.Text;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
