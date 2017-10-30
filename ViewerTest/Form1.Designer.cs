namespace ViewerTest
{
    partial class Form1
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
            this.btnTest = new System.Windows.Forms.Button();
            this.nLogViewer1 = new NLogViewer.NLogViewer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.plUserControl = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.plUserControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(382, 9);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "&Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.button1_Click);
            // 
            // nLogViewer1
            // 
            this.nLogViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nLogViewer1.ExceptionWidth = 0D;
            this.nLogViewer1.LevelWidth = 0D;
            this.nLogViewer1.Location = new System.Drawing.Point(0, 0);
            this.nLogViewer1.LoggerName = "Standard";
            this.nLogViewer1.LoggerNameWidth = 0D;
            this.nLogViewer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nLogViewer1.MessageWidth = 0D;
            this.nLogViewer1.Name = "nLogViewer1";
            this.nLogViewer1.NumberOfEntries = 70;
            this.nLogViewer1.Size = new System.Drawing.Size(466, 406);
            this.nLogViewer1.TabIndex = 2;
            this.nLogViewer1.TimeWidth = 0D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 406);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(466, 42);
            this.panel1.TabIndex = 3;
            // 
            // plUserControl
            // 
            this.plUserControl.Controls.Add(this.nLogViewer1);
            this.plUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plUserControl.Location = new System.Drawing.Point(0, 0);
            this.plUserControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.plUserControl.Name = "plUserControl";
            this.plUserControl.Size = new System.Drawing.Size(466, 406);
            this.plUserControl.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 448);
            this.Controls.Add(this.plUserControl);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.plUserControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.Button btnTest;
        private NLogViewer.NLogViewer nLogViewer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel plUserControl;
    }
}

