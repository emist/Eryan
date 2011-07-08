using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Eryan
{
    public partial class Bot : Form
    {

        Thread botThread;
        WindowHandler bot; 

        //Thread[] botWindowThreads = new Thread[20];
        //WindowHandler[] bots = new WindowHandler[20];

        public Bot()
        {
       //     InitializeComponent();

//            Load += new EventHandler(Program_Load);
            
        }


        private void Program_Load(object sender, EventArgs e)
        {
            //CreateBot();
        }




        public Thread CreateBot()
        {
            botThread = new Thread(new ParameterizedThreadStart(ShowForm));
            bot = new WindowHandler();
            botThread.Start(bot);
            return botThread;
        }

        public WindowHandler getBot()
        {
            return bot;
        }

        public uint getPid()
        {
            return bot.getPid();
        }

        public void ShowForm(object firm)
        {
            Application.Run(firm as Form);
        }

        public void update()
        {
  
        }

        //Destroy Bot
        public Boolean DestroyBot()
        {
            return true;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 3);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(294, 266);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(286, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(286, 240);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Client
            // 
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.tabControl1);
            this.Name = "Client";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
