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
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // selectedOptionLabel
            // 
            this.selectedOptionLabel.AutoSize = true;
            this.selectedOptionLabel.Location = new System.Drawing.Point(12, 9);
            this.selectedOptionLabel.Name = "selectedOptionLabel";
            this.selectedOptionLabel.Size = new System.Drawing.Size(110, 17);
            this.selectedOptionLabel.TabIndex = 0;
            this.selectedOptionLabel.Text = "Selected option:";
            // 
            // selectedOptionComboBox
            // 
            this.selectedOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectedOptionComboBox.FormattingEnabled = true;
            this.helpProvider1.SetHelpString(this.selectedOptionComboBox, "List of all binary features that have at least two children and therefore qualify" +
        " as a parent of an alternative group..");
            this.selectedOptionComboBox.Location = new System.Drawing.Point(128, 6);
            this.selectedOptionComboBox.Name = "selectedOptionComboBox";
            this.helpProvider1.SetShowHelp(this.selectedOptionComboBox, true);
            this.selectedOptionComboBox.Size = new System.Drawing.Size(121, 24);
            this.selectedOptionComboBox.TabIndex = 1;
            // 
            // selectedOptionAddButton
            // 
            this.helpProvider1.SetHelpString(this.selectedOptionAddButton, "All binary children of the currently selected feature will be added to an alterna" +
        "tive group.");
            this.selectedOptionAddButton.Location = new System.Drawing.Point(219, 41);
            this.selectedOptionAddButton.Margin = new System.Windows.Forms.Padding(4);
            this.selectedOptionAddButton.Name = "selectedOptionAddButton";
            this.helpProvider1.SetShowHelp(this.selectedOptionAddButton, true);
            this.selectedOptionAddButton.Size = new System.Drawing.Size(100, 28);
            this.selectedOptionAddButton.TabIndex = 2;
            this.selectedOptionAddButton.Text = "Add option";
            this.selectedOptionAddButton.UseVisualStyleBackColor = true;
            this.selectedOptionAddButton.Click += new System.EventHandler(this.selectedOptionAddButton_Click);
            // 
            // currAltGroupsListBox
            // 
            this.currAltGroupsListBox.FormattingEnabled = true;
            this.helpProvider1.SetHelpString(this.currAltGroupsListBox, "The children of these configurations are in an alternative group.");
            this.currAltGroupsListBox.ItemHeight = 16;
            this.currAltGroupsListBox.Location = new System.Drawing.Point(20, 114);
            this.currAltGroupsListBox.Margin = new System.Windows.Forms.Padding(4);
            this.currAltGroupsListBox.Name = "currAltGroupsListBox";
            this.helpProvider1.SetShowHelp(this.currAltGroupsListBox, true);
            this.currAltGroupsListBox.Size = new System.Drawing.Size(297, 212);
            this.currAltGroupsListBox.TabIndex = 3;
            // 
            // currAltGroupsDeleteButton
            // 
            this.helpProvider1.SetHelpString(this.currAltGroupsDeleteButton, "Remove the alternative group at the feature, that is selected in the list above.");
            this.currAltGroupsDeleteButton.Location = new System.Drawing.Point(219, 335);
            this.currAltGroupsDeleteButton.Margin = new System.Windows.Forms.Padding(4);
            this.currAltGroupsDeleteButton.Name = "currAltGroupsDeleteButton";
            this.helpProvider1.SetShowHelp(this.currAltGroupsDeleteButton, true);
            this.currAltGroupsDeleteButton.Size = new System.Drawing.Size(100, 28);
            this.currAltGroupsDeleteButton.TabIndex = 4;
            this.currAltGroupsDeleteButton.Text = "Delete";
            this.currAltGroupsDeleteButton.UseVisualStyleBackColor = true;
            this.currAltGroupsDeleteButton.Click += new System.EventHandler(this.currAltGroupsDeleteButton_Click);
            // 
            // currAltGroupsLabel
            // 
            this.currAltGroupsLabel.AutoSize = true;
            this.currAltGroupsLabel.Location = new System.Drawing.Point(16, 79);
            this.currAltGroupsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currAltGroupsLabel.Name = "currAltGroupsLabel";
            this.currAltGroupsLabel.Size = new System.Drawing.Size(183, 17);
            this.currAltGroupsLabel.TabIndex = 5;
            this.currAltGroupsLabel.Text = "Current alternate groups at:";
            // 
            // AlternativeGroupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 378);
            this.Controls.Add(this.currAltGroupsLabel);
            this.Controls.Add(this.currAltGroupsDeleteButton);
            this.Controls.Add(this.currAltGroupsListBox);
            this.Controls.Add(this.selectedOptionAddButton);
            this.Controls.Add(this.selectedOptionComboBox);
            this.Controls.Add(this.selectedOptionLabel);
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}