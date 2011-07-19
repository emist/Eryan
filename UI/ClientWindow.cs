using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Eryan.Singleton;

namespace Eryan.UI
{
    ///RUN button needs to be updated when mulitple instance support gets added


    /// <summary>
    /// The Eryan Client window, handles the adding and removing of bot windows/loading scripts/etc.
    /// </summary>
    public class ClientWindow : Utils
    {

        private List<Bot> bots = new List<Bot>();
        private TableLayoutPanel tableLayoutPanel1;
        public TabControl tabControl1;
        public TabPage tabPage1;
        private Button runButton;
        private Button mouseInput;
        private Boolean isMouse = false;
        private Button loadScriptBtn;
        private Button stopScriptBtn;
        private OpenFileDialog sDialog;


        private delegate void formDelegate(Utils form);
        private delegate Bot createBotDelegate();
        public Boolean running = false;
        


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


        private void stopScriptBtn_Click(object sender, System.EventArgs e)
        {
            if (!bots[0].paused)
            {
                Console.WriteLine("Pausing script");
                stopScriptBtn.Text = "Resume Script";
                bots[0].paused = true;
                //Add Loading script stuff here
            }
            else
            {
                Console.WriteLine("Resuming Script");
                stopScriptBtn.Text = "Pause Script";
                bots[0].paused = false;
                //add unloading script code here
            }
        }

        /// <summary>
        /// Handles the Run/Stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void runButton_Click(object sender, System.EventArgs e)
        {
            if (bots[0].running)
            {
                Console.WriteLine("Stopping Script");
                bots[0].scriptName = null;
                runButton.Text = "Run Script";
                bots[0].running = false;
            }
            else
            {
                Console.WriteLine("Runing Script");
                runButton.Text = "Stop Script";
                bots[0].running = true;
            }
            running = !running;
        }

        /// <summary>
        /// Handles the input button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void mouseInput_Click(object sender, System.EventArgs e)
        {
            if (isMouse)
            {
                Console.WriteLine("Stopping Input");
                mouseInput.Text = "Input";
                
                this.Invalidate();
            }
            else
            {
                Console.WriteLine("Starting Input");
                mouseInput.Text = "No Input";
                
                this.Invalidate();
            }
            isMouse = !isMouse;
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
            
            bots[0].initializeBot(this);
            return bots[0];
        }

        public ClientWindow()
        {
            
            InitializeComponent();

            //this.Size = new Size(700, 800);
            //this.AutoSize = true;

            
            this.tabControl1.TabPages[0].AutoScroll = true;
            runButton.Click += runButton_Click;
            mouseInput.Click += mouseInput_Click;
            stopScriptBtn.Click += stopScriptBtn_Click;
            sDialog = new OpenFileDialog();

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


        /// <summary>
        /// Returns if user input is enabled
        /// </summary>
        public Boolean AllowInput
        {
            get
            {
                return isMouse;
            }
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mouseInput = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.loadScriptBtn = new System.Windows.Forms.Button();
            this.stopScriptBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mouseInput, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.052503F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.94749F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1032, 849);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1032, 824);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1024, 798);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // mouseInput
            // 
            this.mouseInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mouseInput.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.mouseInput.Location = new System.Drawing.Point(478, 3);
            this.mouseInput.Name = "mouseInput";
            this.mouseInput.Size = new System.Drawing.Size(75, 19);
            this.mouseInput.TabIndex = 1;
            this.mouseInput.Text = "Input";
            this.mouseInput.UseVisualStyleBackColor = true;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(968, 1);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(63, 26);
            this.runButton.TabIndex = 1;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            // 
            // loadScriptBtn
            // 
            this.loadScriptBtn.Location = new System.Drawing.Point(3, 0);
            this.loadScriptBtn.Name = "loadScriptBtn";
            this.loadScriptBtn.Size = new System.Drawing.Size(75, 23);
            this.loadScriptBtn.TabIndex = 2;
            this.loadScriptBtn.Text = "Load Script";
            this.loadScriptBtn.UseVisualStyleBackColor = true;
            this.loadScriptBtn.Click += new System.EventHandler(this.loadScriptBtn_Click);
            // 
            // stopScriptBtn
            // 
            this.stopScriptBtn.Location = new System.Drawing.Point(92, 0);
            this.stopScriptBtn.Name = "stopScriptBtn";
            this.stopScriptBtn.Size = new System.Drawing.Size(95, 23);
            this.stopScriptBtn.TabIndex = 3;
            this.stopScriptBtn.Text = "Resume Script";
            this.stopScriptBtn.UseVisualStyleBackColor = true;
            // 
            // ClientWindow
            // 
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(1034, 852);
            this.Controls.Add(this.stopScriptBtn);
            this.Controls.Add(this.loadScriptBtn);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(900, 890);
            this.Name = "ClientWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Eryan v. 2.0";
            this.Load += new System.EventHandler(this.ClientWindow_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }

        private void loadScriptBtn_Click(object sender, EventArgs e)
        {
            if (sDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bots[0].scriptName = sDialog.FileName;
                Console.WriteLine(bots[0].scriptName);
            }
        }

        
    }
}
