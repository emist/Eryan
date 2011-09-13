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
    public partial class LogViewer : Form
    {
        public LogViewer()
        {
            InitializeComponent();
            logbox.BackColor = Color.White;
            logbox.ForeColor = Color.Black;
        }

        public RichTextBox LogBox
        {
            get
            {
                return logbox;
            }
        }
    }
}
