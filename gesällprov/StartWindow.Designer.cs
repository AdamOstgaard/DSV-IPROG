namespace IPROG.Uppgifter.gesallprov
{
    partial class StartWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.watcherButton = new System.Windows.Forms.Button();
            this.shareButton = new System.Windows.Forms.Button();
            this.FrameRateCounter = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // watcherButton
            // 
            this.watcherButton.Location = new System.Drawing.Point(40, 76);
            this.watcherButton.Name = "watcherButton";
            this.watcherButton.Size = new System.Drawing.Size(75, 23);
            this.watcherButton.TabIndex = 0;
            this.watcherButton.Text = "Watch";
            this.watcherButton.UseVisualStyleBackColor = true;
            this.watcherButton.Click += new System.EventHandler(this.WatchButtonClick);
            // 
            // shareButton
            // 
            this.shareButton.Location = new System.Drawing.Point(175, 78);
            this.shareButton.Name = "shareButton";
            this.shareButton.Size = new System.Drawing.Size(75, 23);
            this.shareButton.TabIndex = 1;
            this.shareButton.Text = "Share";
            this.shareButton.UseVisualStyleBackColor = true;
            this.shareButton.Click += new System.EventHandler(this.ShareButton_Click);
            // 
            // FrameRateCounter
            // 
            this.FrameRateCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameRateCounter.AutoSize = true;
            this.FrameRateCounter.Location = new System.Drawing.Point(211, 9);
            this.FrameRateCounter.Name = "FrameRateCounter";
            this.FrameRateCounter.Size = new System.Drawing.Size(69, 15);
            this.FrameRateCounter.TabIndex = 2;
            this.FrameRateCounter.Text = "Not sharing";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(40, 33);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(210, 23);
            this.serverTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server";
            // 
            // StartWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 132);
            this.Controls.Add(this.watcherButton);
            this.Controls.Add(this.shareButton);
            this.Controls.Add(this.FrameRateCounter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.serverTextBox);
            this.Name = "StartWindow";
            this.Text = "StartWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button watcherButton;
        private System.Windows.Forms.Button shareButton;
        private System.Windows.Forms.Label FrameRateCounter;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label label2;
    }
}

