using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Eryan
{

    /// <summary>
    /// The Eryan Client window, handles the adding and removing of bot windows/loading scripts/etc.
    /// </summary>
    public class ClientWindow : Utils
    {
        public TabPage tabPage1;
        public TabControl tabControl1;

        private List<Bot> bots = new List<Bot>();
        private delegate void formDelegate(Utils form);
        private delegate Bot createBotDelegate();


        /// <summary>
        /// Thread-safe way to add a WindowHandler to its controltab
        /// </summary>
        /// <param name="form">The instance of WindowHandler to add</param>
        public void addControlToTab(Utils form)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new formDelegate(addControlToTab), new object[] { form });
                return;
            }


            this.tabControl1.TabPages[0].Controls.Add(form);
            //this.tabControl1.Size = new Size(800, 900);
            //this.tabControl1.TabPages[0].Size = new Size(800, 900);
        }


        /// <summary>
        /// Getter for the Bot list.
        /// </summary>
        /// <returns>Returns a List of Bot</returns>
        public List<Bot> getBots()
        {
            return bots;
        }

        /// <summary>
        /// Thread-safe Bot creator
        /// </summary>
        /// <returns>The reference to the bot created</returns>
        public Bot createBot()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new createBotDelegate(createBot), new object[] { });
                return null;
            }

            bots.Add(new Bot());
            Bot bot = bots[0];
            bot.initializeBot(this);
            return bot;
        }

        public ClientWindow()
        {
            
            InitializeComponent();
            this.Size = new Size(700, 800);
            this.AutoSize = true;
            this.tabControl1.TabPages[0].AutoScroll = true;
        }

        /// <summary>
        /// Updates the location of the drawable area
        /// </summary>
        /// <param name="e">EventArgs for the event</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (bots.Count > 0)
            {
                WindowHandler tmp = bots[0].getHandle();
                if (tmp != null)
                {
                    Utils util = DrawAbleScreenFetcher.fetch(tmp.getPid());
                    if(util != null)
                        util.setLocation(new Point(this.Location.X + 5, this.Location.Y + 50));
                }
            }
            this.tabControl1.Size = this.Size;
            base.OnVisibleChanged(e);
        }

        /// <summary>
        /// Updates the location of the drawable area
        /// </summary>
        /// <param name="e">EventArgs for the event</param>
        protected override void OnMove(EventArgs e)
        {
            
            if (bots.Count > 0)
            {
                WindowHandler tmp = bots[0].getHandle();
                if (tmp != null)
                {
                    Utils util = DrawAbleScreenFetcher.fetch(tmp.getPid());
                    if(util != null)
                        util.setLocation(new Point(this.Location.X + 5, this.Location.Y + 50));
                }
            }

            this.tabControl1.Size = this.Size;
            base.OnMove(e);
        }
        /// <summary>
        /// Updates the location of the drawable area
        /// </summary>
        /// <param name="e">EventArgs for the event</param>
        protected override void OnResize(EventArgs e)
        {
         
            if (bots.Count > 0)
            {
                WindowHandler tmp = bots[0].getHandle();
                if (tmp != null)
                {
                    Utils util = DrawAbleScreenFetcher.fetch(tmp.getPid());
                    if(util != null)
                        util.setLocation(new Point(this.Location.X + 5, this.Location.Y + 50));
                }
            }

            this.tabControl1.Size = this.Size;
            base.OnResize(e);
        }

        private void InitializeComponent()
        {
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(283, 246);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(291, 272);
            this.tabControl1.TabIndex = 0;
            // 
            // ClientWindow
            // 
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.tabControl1);
            this.Name = "ClientWindow";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
    }
}
