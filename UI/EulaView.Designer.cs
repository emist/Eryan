namespace Eryan.UI
{
    partial class EulaView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.agreeButton = new System.Windows.Forms.Button();
            this.declineButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(2, 10);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(784, 469);
            this.textBox1.TabIndex = 0;
            // 
            // agreeButton
            // 
            this.agreeButton.Location = new System.Drawing.Point(227, 496);
            this.agreeButton.Name = "agreeButton";
            this.agreeButton.Size = new System.Drawing.Size(148, 29);
            this.agreeButton.TabIndex = 1;
            this.agreeButton.Text = "Agree";
            this.agreeButton.UseVisualStyleBackColor = true;
            this.agreeButton.Click += new System.EventHandler(this.agreeButton_Click);
            // 
            // declineButton
            // 
            this.declineButton.Location = new System.Drawing.Point(457, 493);
            this.declineButton.Name = "declineButton";
            this.declineButton.Size = new System.Drawing.Size(158, 32);
            this.declineButton.TabIndex = 2;
            this.declineButton.Text = "Decline";
            this.declineButton.UseVisualStyleBackColor = true;
            this.declineButton.Click += new System.EventHandler(this.declineButton_Click);
            // 
            // EulaView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 534);
            this.Controls.Add(this.declineButton);
            this.Controls.Add(this.agreeButton);
            this.Controls.Add(this.textBox1);
            this.Name = "EulaView";
            this.Text = "EulaView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button agreeButton;
        private System.Windows.Forms.Button declineButton;

    }
}