namespace VariabilitModel_GUI
{
    partial class NewFeatureDialog
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.variantGeneratorTextBox = new System.Windows.Forms.TextBox();
            this.optionalCheckBox = new System.Windows.Forms.CheckBox();
            this.parentComboBox = new System.Windows.Forms.ComboBox();
            this.featureNameTextBox = new System.Windows.Forms.TextBox();
            this.featureNameLabel = new System.Windows.Forms.Label();
            this.variantGenerationLabel = new System.Windows.Forms.Label();
            this.parentLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addOptionButton = new System.Windows.Forms.Button();
            this.binaryRadioButton = new System.Windows.Forms.RadioButton();
            this.numericRadioButton = new System.Windows.Forms.RadioButton();
            this.postfixTextBox = new System.Windows.Forms.TextBox();
            this.numericSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.prePostCheckBox = new System.Windows.Forms.CheckBox();
            this.stepSizeCheckBox = new System.Windows.Forms.CheckBox();
            this.postfixLabel = new System.Windows.Forms.Label();
            this.valueRangeLabel = new System.Windows.Forms.Label();
            this.minLabel = new System.Windows.Forms.Label();
            this.maxLabel = new System.Windows.Forms.Label();
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.stepSizeExampleLabel = new System.Windows.Forms.Label();
            this.prefixLabel = new System.Windows.Forms.Label();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.prefixTextBox = new System.Windows.Forms.TextBox();
            this.stepSizeTextBox = new System.Windows.Forms.TextBox();
            this.optionTypeLabel = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.numericSettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // variantGeneratorTextBox
            // 
            this.variantGeneratorTextBox.Location = new System.Drawing.Point(195, 205);
            this.variantGeneratorTextBox.Name = "variantGeneratorTextBox";
            this.variantGeneratorTextBox.Size = new System.Drawing.Size(206, 20);
            this.variantGeneratorTextBox.TabIndex = 15;
            // 
            // optionalCheckBox
            // 
            this.optionalCheckBox.AutoSize = true;
            this.optionalCheckBox.Checked = true;
            this.optionalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optionalCheckBox.Location = new System.Drawing.Point(258, 6);
            this.optionalCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.optionalCheckBox.Name = "optionalCheckBox";
            this.optionalCheckBox.Size = new System.Drawing.Size(65, 17);
            this.optionalCheckBox.TabIndex = 25;
            this.optionalCheckBox.Text = "Optional";
            this.optionalCheckBox.UseVisualStyleBackColor = true;
            // 
            // parentComboBox
            // 
            this.parentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parentComboBox.FormattingEnabled = true;
            this.parentComboBox.Location = new System.Drawing.Point(128, 30);
            this.parentComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.parentComboBox.Name = "parentComboBox";
            this.parentComboBox.Size = new System.Drawing.Size(120, 21);
            this.parentComboBox.Sorted = true;
            this.parentComboBox.TabIndex = 24;
            // 
            // featureNameTextBox
            // 
            this.featureNameTextBox.Location = new System.Drawing.Point(128, 4);
            this.featureNameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.featureNameTextBox.Name = "featureNameTextBox";
            this.featureNameTextBox.Size = new System.Drawing.Size(120, 20);
            this.featureNameTextBox.TabIndex = 14;
            this.featureNameTextBox.TextChanged += new System.EventHandler(this.featureNameTextBox_TextChanged);
            // 
            // featureNameLabel
            // 
            this.featureNameLabel.AutoSize = true;
            this.featureNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.featureNameLabel.Location = new System.Drawing.Point(13, 7);
            this.featureNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.featureNameLabel.Name = "featureNameLabel";
            this.featureNameLabel.Size = new System.Drawing.Size(77, 13);
            this.featureNameLabel.TabIndex = 22;
            this.featureNameLabel.Text = "Feature Name:";
            // 
            // variantGenerationLabel
            // 
            this.variantGenerationLabel.AutoSize = true;
            this.variantGenerationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variantGenerationLabel.Location = new System.Drawing.Point(13, 208);
            this.variantGenerationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.variantGenerationLabel.Name = "variantGenerationLabel";
            this.variantGenerationLabel.Size = new System.Drawing.Size(149, 13);
            this.variantGenerationLabel.TabIndex = 19;
            this.variantGenerationLabel.Text = "Variant Generation Parameter:";
            // 
            // parentLabel
            // 
            this.parentLabel.AutoSize = true;
            this.parentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parentLabel.Location = new System.Drawing.Point(13, 33);
            this.parentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.parentLabel.Name = "parentLabel";
            this.parentLabel.Size = new System.Drawing.Size(80, 13);
            this.parentLabel.TabIndex = 20;
            this.parentLabel.Text = "Parent Feature:";
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(313, 269);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 27);
            this.cancelButton.TabIndex = 23;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // addOptionButton
            // 
            this.addOptionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.addOptionButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addOptionButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addOptionButton.Location = new System.Drawing.Point(16, 269);
            this.addOptionButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.addOptionButton.Name = "addOptionButton";
            this.addOptionButton.Size = new System.Drawing.Size(99, 27);
            this.addOptionButton.TabIndex = 21;
            this.addOptionButton.Text = "Add Option";
            this.addOptionButton.UseVisualStyleBackColor = false;
            this.addOptionButton.Click += new System.EventHandler(this.addOptionButton_Click);
            // 
            // binaryRadioButton
            // 
            this.binaryRadioButton.AutoSize = true;
            this.binaryRadioButton.Checked = true;
            this.binaryRadioButton.Location = new System.Drawing.Point(128, 59);
            this.binaryRadioButton.Name = "binaryRadioButton";
            this.binaryRadioButton.Size = new System.Drawing.Size(54, 17);
            this.binaryRadioButton.TabIndex = 1;
            this.binaryRadioButton.TabStop = true;
            this.binaryRadioButton.Text = "Binary";
            this.binaryRadioButton.UseVisualStyleBackColor = true;
            // 
            // numericRadioButton
            // 
            this.numericRadioButton.AutoSize = true;
            this.numericRadioButton.Location = new System.Drawing.Point(188, 59);
            this.numericRadioButton.Name = "numericRadioButton";
            this.numericRadioButton.Size = new System.Drawing.Size(64, 17);
            this.numericRadioButton.TabIndex = 0;
            this.numericRadioButton.Text = "Numeric";
            this.numericRadioButton.UseVisualStyleBackColor = true;
            this.numericRadioButton.CheckedChanged += new System.EventHandler(this.numericRadioButton_CheckedChanged);
            // 
            // postfixTextBox
            // 
            this.postfixTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postfixTextBox.Location = new System.Drawing.Point(258, 168);
            this.postfixTextBox.Name = "postfixTextBox";
            this.postfixTextBox.Size = new System.Drawing.Size(82, 22);
            this.postfixTextBox.TabIndex = 23;
            // 
            // numericSettingsGroupBox
            // 
            this.numericSettingsGroupBox.Controls.Add(this.stepSizeCheckBox);
            this.numericSettingsGroupBox.Controls.Add(this.valueRangeLabel);
            this.numericSettingsGroupBox.Controls.Add(this.minLabel);
            this.numericSettingsGroupBox.Controls.Add(this.maxLabel);
            this.numericSettingsGroupBox.Controls.Add(this.minValueTextBox);
            this.numericSettingsGroupBox.Controls.Add(this.maxValueTextBox);
            this.numericSettingsGroupBox.Controls.Add(this.stepSizeExampleLabel);
            this.numericSettingsGroupBox.Controls.Add(this.stepSizeLabel);
            this.numericSettingsGroupBox.Controls.Add(this.stepSizeTextBox);
            this.numericSettingsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericSettingsGroupBox.Location = new System.Drawing.Point(16, 82);
            this.numericSettingsGroupBox.Name = "numericSettingsGroupBox";
            this.numericSettingsGroupBox.Size = new System.Drawing.Size(390, 82);
            this.numericSettingsGroupBox.TabIndex = 42;
            this.numericSettingsGroupBox.TabStop = false;
            this.numericSettingsGroupBox.Text = "Numeric Option Settings";
            // 
            // prePostCheckBox
            // 
            this.prePostCheckBox.AutoSize = true;
            this.prePostCheckBox.Location = new System.Drawing.Point(25, 171);
            this.prePostCheckBox.Name = "prePostCheckBox";
            this.prePostCheckBox.Size = new System.Drawing.Size(15, 14);
            this.prePostCheckBox.TabIndex = 25;
            this.prePostCheckBox.UseVisualStyleBackColor = true;
            this.prePostCheckBox.CheckedChanged += new System.EventHandler(this.prePostCheckBox_CheckedChanged);
            // 
            // stepSizeCheckBox
            // 
            this.stepSizeCheckBox.AutoSize = true;
            this.stepSizeCheckBox.Location = new System.Drawing.Point(9, 56);
            this.stepSizeCheckBox.Name = "stepSizeCheckBox";
            this.stepSizeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.stepSizeCheckBox.TabIndex = 24;
            this.stepSizeCheckBox.UseVisualStyleBackColor = true;
            this.stepSizeCheckBox.CheckedChanged += new System.EventHandler(this.stepSizeCheckBox_CheckedChanged);
            // 
            // postfixLabel
            // 
            this.postfixLabel.AutoSize = true;
            this.postfixLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postfixLabel.Location = new System.Drawing.Point(207, 171);
            this.postfixLabel.Name = "postfixLabel";
            this.postfixLabel.Size = new System.Drawing.Size(45, 14);
            this.postfixLabel.TabIndex = 22;
            this.postfixLabel.Text = "Postfix:";
            // 
            // valueRangeLabel
            // 
            this.valueRangeLabel.AutoSize = true;
            this.valueRangeLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valueRangeLabel.Location = new System.Drawing.Point(6, 27);
            this.valueRangeLabel.Name = "valueRangeLabel";
            this.valueRangeLabel.Size = new System.Drawing.Size(97, 14);
            this.valueRangeLabel.TabIndex = 14;
            this.valueRangeLabel.Text = "Range of values:";
            // 
            // minLabel
            // 
            this.minLabel.AutoSize = true;
            this.minLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minLabel.Location = new System.Drawing.Point(118, 27);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(28, 14);
            this.minLabel.TabIndex = 15;
            this.minLabel.Text = "Min";
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLabel.Location = new System.Drawing.Point(230, 27);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(29, 14);
            this.maxLabel.TabIndex = 16;
            this.maxLabel.Text = "Max";
            // 
            // minValueTextBox
            // 
            this.minValueTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minValueTextBox.Location = new System.Drawing.Point(152, 24);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(59, 22);
            this.minValueTextBox.TabIndex = 17;
            this.minValueTextBox.TextChanged += new System.EventHandler(this.minValueTextBox_TextChanged);
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxValueTextBox.Location = new System.Drawing.Point(265, 24);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(59, 22);
            this.maxValueTextBox.TabIndex = 18;
            this.maxValueTextBox.TextChanged += new System.EventHandler(this.maxValueTextBox_TextChanged);
            // 
            // stepSizeExampleLabel
            // 
            this.stepSizeExampleLabel.AutoSize = true;
            this.stepSizeExampleLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeExampleLabel.Location = new System.Drawing.Point(265, 56);
            this.stepSizeExampleLabel.Name = "stepSizeExampleLabel";
            this.stepSizeExampleLabel.Size = new System.Drawing.Size(68, 14);
            this.stepSizeExampleLabel.TabIndex = 21;
            this.stepSizeExampleLabel.Text = "(e.g., n + 1 )";
            // 
            // prefixLabel
            // 
            this.prefixLabel.AutoSize = true;
            this.prefixLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixLabel.Location = new System.Drawing.Point(46, 171);
            this.prefixLabel.Name = "prefixLabel";
            this.prefixLabel.Size = new System.Drawing.Size(38, 14);
            this.prefixLabel.TabIndex = 19;
            this.prefixLabel.Text = "Prefix:";
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeLabel.Location = new System.Drawing.Point(30, 56);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(59, 14);
            this.stepSizeLabel.TabIndex = 21;
            this.stepSizeLabel.Text = "Step size:";
            // 
            // prefixTextBox
            // 
            this.prefixTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixTextBox.Location = new System.Drawing.Point(90, 169);
            this.prefixTextBox.Name = "prefixTextBox";
            this.prefixTextBox.Size = new System.Drawing.Size(87, 22);
            this.prefixTextBox.TabIndex = 20;
            // 
            // stepSizeTextBox
            // 
            this.stepSizeTextBox.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeTextBox.Location = new System.Drawing.Point(98, 53);
            this.stepSizeTextBox.Name = "stepSizeTextBox";
            this.stepSizeTextBox.Size = new System.Drawing.Size(161, 22);
            this.stepSizeTextBox.TabIndex = 20;
            this.stepSizeTextBox.TextChanged += new System.EventHandler(this.stepSizeTextBox_TextChanged);
            // 
            // optionTypeLabel
            // 
            this.optionTypeLabel.AutoSize = true;
            this.optionTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionTypeLabel.Location = new System.Drawing.Point(13, 59);
            this.optionTypeLabel.Name = "optionTypeLabel";
            this.optionTypeLabel.Size = new System.Drawing.Size(64, 13);
            this.optionTypeLabel.TabIndex = 44;
            this.optionTypeLabel.Text = "Option type:";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(13, 236);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(48, 13);
            this.errorLabel.TabIndex = 45;
            this.errorLabel.Text = "label18";
            this.errorLabel.Visible = false;
            // 
            // NewFeatureDialog
            // 
            this.ClientSize = new System.Drawing.Size(425, 308);
            this.Controls.Add(this.prePostCheckBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.binaryRadioButton);
            this.Controls.Add(this.postfixTextBox);
            this.Controls.Add(this.optionTypeLabel);
            this.Controls.Add(this.postfixLabel);
            this.Controls.Add(this.numericRadioButton);
            this.Controls.Add(this.numericSettingsGroupBox);
            this.Controls.Add(this.variantGeneratorTextBox);
            this.Controls.Add(this.optionalCheckBox);
            this.Controls.Add(this.parentComboBox);
            this.Controls.Add(this.featureNameTextBox);
            this.Controls.Add(this.featureNameLabel);
            this.Controls.Add(this.prefixLabel);
            this.Controls.Add(this.variantGenerationLabel);
            this.Controls.Add(this.parentLabel);
            this.Controls.Add(this.prefixTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addOptionButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewFeatureDialog";
            this.Text = "Creating new option...";
            this.numericSettingsGroupBox.ResumeLayout(false);
            this.numericSettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.TextBox variantGeneratorTextBox;
        private System.Windows.Forms.CheckBox optionalCheckBox;
        private System.Windows.Forms.ComboBox parentComboBox;
        private System.Windows.Forms.TextBox featureNameTextBox;
        private System.Windows.Forms.Label featureNameLabel;
        private System.Windows.Forms.Label variantGenerationLabel;
        private System.Windows.Forms.Label parentLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button addOptionButton;
        private System.Windows.Forms.RadioButton binaryRadioButton;
        private System.Windows.Forms.RadioButton numericRadioButton;
        private System.Windows.Forms.TextBox postfixTextBox;
        private System.Windows.Forms.GroupBox numericSettingsGroupBox;
        private System.Windows.Forms.Label postfixLabel;
        private System.Windows.Forms.Label valueRangeLabel;
        private System.Windows.Forms.Label minLabel;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.TextBox minValueTextBox;
        private System.Windows.Forms.TextBox maxValueTextBox;
        private System.Windows.Forms.Label stepSizeExampleLabel;
        private System.Windows.Forms.Label prefixLabel;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.TextBox prefixTextBox;
        private System.Windows.Forms.TextBox stepSizeTextBox;
        private System.Windows.Forms.CheckBox prePostCheckBox;
        private System.Windows.Forms.CheckBox stepSizeCheckBox;
        private System.Windows.Forms.Label optionTypeLabel;
        private System.Windows.Forms.Label errorLabel;
    }
}