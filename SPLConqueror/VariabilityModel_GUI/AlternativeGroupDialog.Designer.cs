namespace VariabilitModel_GUI
{
    partial class AlternativeGroupDialog
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
            this.selectedOptionLabel = new System.Windows.Forms.Label();
            this.selectedOptionComboBox = new System.Windows.Forms.ComboBox();
            this.selectedOptionAddButton = new System.Windows.Forms.Button();
            this.currAltGroupsListBox = new System.Windows.Forms.ListBox();
            this.currAltGroupsDeleteButton = new System.Windows.Forms.Button();
            this.currAltGroupsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectedOptionLabel
            // 
            this.selectedOptionLabel.AutoSize = true;
            this.selectedOptionLabel.Location = new System.Drawing.Point(12, 9);
            this.selectedOptionLabel.Name = "selectedOptionLabel";
            this.selectedOptionLabel.Size = new System.Drawing.Size(84, 13);
            this.selectedOptionLabel.TabIndex = 0;
            this.selectedOptionLabel.Text = "Selected option:";
            // 
            // selectedOptionComboBox
            // 
            this.selectedOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectedOptionComboBox.FormattingEnabled = true;
            this.selectedOptionComboBox.Location = new System.Drawing.Point(118, 6);
            this.selectedOptionComboBox.Name = "selectedOptionComboBox";
            this.selectedOptionComboBox.Size = new System.Drawing.Size(121, 21);
            this.selectedOptionComboBox.TabIndex = 1;
            this.selectedOptionComboBox.SelectedIndexChanged += selectedOptionComboBox_SelectedIndexChanged;
            // 
            // selectedOptionAddButton
            // 
            this.selectedOptionAddButton.Location = new System.Drawing.Point(164, 33);
            this.selectedOptionAddButton.Name = "selectedOptionAddButton";
            this.selectedOptionAddButton.Size = new System.Drawing.Size(75, 23);
            this.selectedOptionAddButton.TabIndex = 2;
            this.selectedOptionAddButton.Text = "Add option";
            this.selectedOptionAddButton.UseVisualStyleBackColor = true;
            this.selectedOptionAddButton.Click += new System.EventHandler(this.selectedOptionAddButton_Click);
            // 
            // currAltGroupsListBox
            // 
            this.currAltGroupsListBox.FormattingEnabled = true;
            this.currAltGroupsListBox.Location = new System.Drawing.Point(15, 93);
            this.currAltGroupsListBox.Name = "currAltGroupsListBox";
            this.currAltGroupsListBox.Size = new System.Drawing.Size(224, 173);
            this.currAltGroupsListBox.TabIndex = 3;
            // 
            // currAltGroupsDeleteButton
            // 
            this.currAltGroupsDeleteButton.Location = new System.Drawing.Point(164, 272);
            this.currAltGroupsDeleteButton.Name = "currAltGroupsDeleteButton";
            this.currAltGroupsDeleteButton.Size = new System.Drawing.Size(75, 23);
            this.currAltGroupsDeleteButton.TabIndex = 4;
            this.currAltGroupsDeleteButton.Text = "Delete";
            this.currAltGroupsDeleteButton.UseVisualStyleBackColor = true;
            this.currAltGroupsDeleteButton.Click += new System.EventHandler(this.currAltGroupsDeleteButton_Click);
            // 
            // currAltGroupsLabel
            // 
            this.currAltGroupsLabel.AutoSize = true;
            this.currAltGroupsLabel.Location = new System.Drawing.Point(12, 64);
            this.currAltGroupsLabel.Name = "currAltGroupsLabel";
            this.currAltGroupsLabel.Size = new System.Drawing.Size(135, 13);
            this.currAltGroupsLabel.TabIndex = 5;
            this.currAltGroupsLabel.Text = "Current alternate groups at:";
            // 
            // AlternativeGroupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 307);
            this.Controls.Add(this.currAltGroupsLabel);
            this.Controls.Add(this.currAltGroupsDeleteButton);
            this.Controls.Add(this.currAltGroupsListBox);
            this.Controls.Add(this.selectedOptionAddButton);
            this.Controls.Add(this.selectedOptionComboBox);
            this.Controls.Add(this.selectedOptionLabel);
            this.Name = "AlternativeGroupDialog";
            this.Text = "Creating groups...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectedOptionLabel;
        private System.Windows.Forms.ComboBox selectedOptionComboBox;
        private System.Windows.Forms.Button selectedOptionAddButton;
        private System.Windows.Forms.ListBox currAltGroupsListBox;
        private System.Windows.Forms.Button currAltGroupsDeleteButton;
        private System.Windows.Forms.Label currAltGroupsLabel;
    }
}