namespace GenshinDiscordBotUI
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.simpleLogTextBox1 = new Serilog.Sinks.WinForms.Core.SimpleLogTextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(12, 415);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 1;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(93, 415);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(75, 23);
            this.button_Stop.TabIndex = 2;
            this.button_Stop.Text = "Stop";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // simpleLogTextBox1
            // 
            this.simpleLogTextBox1.ForContext = "";
            this.simpleLogTextBox1.Location = new System.Drawing.Point(13, 12);
            this.simpleLogTextBox1.LogBorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.simpleLogTextBox1.LogPadding = new System.Windows.Forms.Padding(3);
            this.simpleLogTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.simpleLogTextBox1.Name = "simpleLogTextBox1";
            this.simpleLogTextBox1.ReadOnly = false;
            this.simpleLogTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.simpleLogTextBox1.Size = new System.Drawing.Size(774, 397);
            this.simpleLogTextBox1.TabIndex = 3;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipTitle = "Genshin Discord Bot";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Genshin Discord Bot";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.simpleLogTextBox1);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.button_Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Genshin Discord Bot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Stop;
        private Serilog.Sinks.WinForms.Core.SimpleLogTextBox simpleLogTextBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}