using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Eryan.UI
{
    public partial class EulaView : Form
    {
        private bool accepted = false;
        public EulaView()
        {
            InitializeComponent();
            textBox1.Text = System.IO.File.ReadAllText("Eula.txt");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void agreeButton_Click(object sender, EventArgs e)
        {
            accepted = true;
            this.Close();
        }

        private void declineButton_Click(object sender, EventArgs e)
        {
            accepted = false;
            this.Close();
        }

        public bool Status
        {
            get
            {
                return accepted;
            }
        }

    }
}
