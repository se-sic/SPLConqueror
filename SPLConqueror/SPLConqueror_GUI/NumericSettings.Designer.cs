namespace SPLConqueror_GUI
{
    partial class NumericSettings
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
            this.okButton = new System.Windows.Forms.Button();
            this.optionPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(239, 353);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // optionPanel
            // 
            this.optionPanel.AutoScroll = true;
            this.optionPanel.Location = new System.Drawing.Point(0, 12);
            this.optionPanel.Name = "optionPanel";
            this.optionPanel.Size = new System.Drawing.Size(326, 330);
            this.optionPanel.TabIndex = 2;
            // 
            // NumericSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 384);
            this.Controls.Add(this.optionPanel);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NumericSettings";
            this.Text = "Configuration Options";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel optionPanel;
    }
}