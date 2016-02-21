namespace VariabilitModel_GUI
{
    partial class EditOptionDialog
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectOptionLabel = new System.Windows.Forms.Label();
            this.optionalCheckBox = new System.Windows.Forms.CheckBox();
            this.currentParentLabel = new System.Windows.Forms.Label();
            this.constraintsGroupBox = new System.Windows.Forms.GroupBox();
            this.requiresDelButton = new System.Windows.Forms.Button();
            this.requiresOverviewListBox = new System.Windows.Forms.ListBox();
            this.excludesDelButton = new System.Windows.Forms.Button();
            this.excludesOverviewListBox = new System.Windows.Forms.ListBox();
            this.requiresAddButton = new System.Windows.Forms.Button();
            this.excludesAddButton = new System.Windows.Forms.Button();
            this.requiresLabel = new System.Windows.Forms.Label();
            this.excludesLabel = new System.Windows.Forms.Label();
            this.requiresCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.excludesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.optionTypeNumericRadioButton = new System.Windows.Forms.RadioButton();
            this.optionTypeBinaryRadioButton = new System.Windows.Forms.RadioButton();
            this.currentOptionTypeLabel = new System.Windows.Forms.Label();
            this.parentLabel = new System.Windows.Forms.Label();
            this.setParentButton = new System.Windows.Forms.Button();
            this.renameOptionButton = new System.Windows.Forms.Button();
            this.selectOptionComboBox = new System.Windows.Forms.ComboBox();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.postfixTextBox = new System.Windows.Forms.TextBox();
            this.prefixTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.outputStringTextBox = new System.Windows.Forms.TextBox();
            this.outputStringLabel = new System.Windows.Forms.Label();
            this.numericSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.changeStepSizeButton = new System.Windows.Forms.Button();
            this.changeRangeButton = new System.Windows.Forms.Button();
            this.stepSizeLabel = new System.Windows.Forms.Label();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.currentStepSizeLabel = new System.Windows.Forms.Label();
            this.currentRangeOfValuesLabel = new System.Windows.Forms.Label();
            this.constraintsGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.settingsGroupBox.SuspendLayout();
            this.numericSettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectOptionLabel
            // 
            this.selectOptionLabel.AutoSize = true;
            this.selectOptionLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectOptionLabel.Location = new System.Drawing.Point(10, 19);
            this.selectOptionLabel.Name = "selectOptionLabel";
            this.selectOptionLabel.Size = new System.Drawing.Size(82, 15);
            this.selectOptionLabel.TabIndex = 0;
            this.selectOptionLabel.Text = "Select Option:";
            // 
            // optionalCheckBox
            // 
            this.optionalCheckBox.AutoSize = true;
            this.optionalCheckBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionalCheckBox.Location = new System.Drawing.Point(330, 18);
            this.optionalCheckBox.Name = "optionalCheckBox";
            this.optionalCheckBox.Size = new System.Drawing.Size(74, 19);
            this.optionalCheckBox.TabIndex = 10;
            this.optionalCheckBox.Text = "Optional";
            this.optionalCheckBox.UseVisualStyleBackColor = true;
            this.optionalCheckBox.Visible = false;
            this.optionalCheckBox.CheckedChanged += new System.EventHandler(this.optionalCheckBox_CheckedChanged);
            // 
            // currentParentLabel
            // 
            this.currentParentLabel.AutoSize = true;
            this.currentParentLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentParentLabel.Location = new System.Drawing.Point(10, 50);
            this.currentParentLabel.Name = "currentParentLabel";
            this.currentParentLabel.Size = new System.Drawing.Size(90, 15);
            this.currentParentLabel.TabIndex = 23;
            this.currentParentLabel.Text = "Current parent:";
            // 
            // constraintsGroupBox
            // 
            this.constraintsGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this.constraintsGroupBox.Controls.Add(this.requiresDelButton);
            this.constraintsGroupBox.Controls.Add(this.requiresOverviewListBox);
            this.constraintsGroupBox.Controls.Add(this.excludesDelButton);
            this.constraintsGroupBox.Controls.Add(this.excludesOverviewListBox);
            this.constraintsGroupBox.Controls.Add(this.requiresAddButton);
            this.constraintsGroupBox.Controls.Add(this.excludesAddButton);
            this.constraintsGroupBox.Controls.Add(this.requiresLabel);
            this.constraintsGroupBox.Controls.Add(this.excludesLabel);
            this.constraintsGroupBox.Controls.Add(this.requiresCheckedListBox);
            this.constraintsGroupBox.Controls.Add(this.excludesCheckedListBox);
            this.constraintsGroupBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constraintsGroupBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.constraintsGroupBox.Location = new System.Drawing.Point(12, 300);
            this.constraintsGroupBox.Name = "constraintsGroupBox";
            this.constraintsGroupBox.Size = new System.Drawing.Size(600, 316);
            this.constraintsGroupBox.TabIndex = 50;
            this.constraintsGroupBox.TabStop = false;
            this.constraintsGroupBox.Text = "Constraints";
            // 
            // requiresDelButton
            // 
            this.requiresDelButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresDelButton.Location = new System.Drawing.Point(315, 137);
            this.requiresDelButton.Name = "requiresDelButton";
            this.requiresDelButton.Size = new System.Drawing.Size(56, 23);
            this.requiresDelButton.TabIndex = 75;
            this.requiresDelButton.Text = "Del";
            this.requiresDelButton.UseVisualStyleBackColor = true;
            this.requiresDelButton.Click += new System.EventHandler(this.requiresDelButton_Click);
            // 
            // requiresOverviewListBox
            // 
            this.requiresOverviewListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresOverviewListBox.FormattingEnabled = true;
            this.requiresOverviewListBox.ItemHeight = 15;
            this.requiresOverviewListBox.Location = new System.Drawing.Point(315, 37);
            this.requiresOverviewListBox.Name = "requiresOverviewListBox";
            this.requiresOverviewListBox.Size = new System.Drawing.Size(279, 94);
            this.requiresOverviewListBox.TabIndex = 74;
            // 
            // excludesDelButton
            // 
            this.excludesDelButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesDelButton.Location = new System.Drawing.Point(9, 137);
            this.excludesDelButton.Name = "excludesDelButton";
            this.excludesDelButton.Size = new System.Drawing.Size(56, 23);
            this.excludesDelButton.TabIndex = 73;
            this.excludesDelButton.Text = "Del";
            this.excludesDelButton.UseVisualStyleBackColor = true;
            this.excludesDelButton.Click += new System.EventHandler(this.excludesDelButton_Click);
            // 
            // excludesOverviewListBox
            // 
            this.excludesOverviewListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesOverviewListBox.FormattingEnabled = true;
            this.excludesOverviewListBox.ItemHeight = 15;
            this.excludesOverviewListBox.Location = new System.Drawing.Point(9, 37);
            this.excludesOverviewListBox.Name = "excludesOverviewListBox";
            this.excludesOverviewListBox.Size = new System.Drawing.Size(269, 94);
            this.excludesOverviewListBox.TabIndex = 72;
            // 
            // requiresAddButton
            // 
            this.requiresAddButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresAddButton.Location = new System.Drawing.Point(315, 286);
            this.requiresAddButton.Name = "requiresAddButton";
            this.requiresAddButton.Size = new System.Drawing.Size(56, 23);
            this.requiresAddButton.TabIndex = 70;
            this.requiresAddButton.Text = "Add";
            this.requiresAddButton.UseVisualStyleBackColor = true;
            this.requiresAddButton.Click += new System.EventHandler(this.requiresAddButton_Click);
            // 
            // excludesAddButton
            // 
            this.excludesAddButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesAddButton.Location = new System.Drawing.Point(9, 284);
            this.excludesAddButton.Name = "excludesAddButton";
            this.excludesAddButton.Size = new System.Drawing.Size(56, 23);
            this.excludesAddButton.TabIndex = 68;
            this.excludesAddButton.Text = "Add";
            this.excludesAddButton.UseVisualStyleBackColor = true;
            this.excludesAddButton.Click += new System.EventHandler(this.excludesAddButton_Click);
            // 
            // requiresLabel
            // 
            this.requiresLabel.AutoSize = true;
            this.requiresLabel.Location = new System.Drawing.Point(312, 19);
            this.requiresLabel.Name = "requiresLabel";
            this.requiresLabel.Size = new System.Drawing.Size(59, 15);
            this.requiresLabel.TabIndex = 67;
            this.requiresLabel.Text = "Requires:";
            // 
            // excludesLabel
            // 
            this.excludesLabel.AutoSize = true;
            this.excludesLabel.Location = new System.Drawing.Point(6, 19);
            this.excludesLabel.Name = "excludesLabel";
            this.excludesLabel.Size = new System.Drawing.Size(57, 15);
            this.excludesLabel.TabIndex = 66;
            this.excludesLabel.Text = "Excludes:";
            // 
            // requiresCheckedListBox
            // 
            this.requiresCheckedListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresCheckedListBox.FormattingEnabled = true;
            this.requiresCheckedListBox.Location = new System.Drawing.Point(315, 166);
            this.requiresCheckedListBox.Name = "requiresCheckedListBox";
            this.requiresCheckedListBox.Size = new System.Drawing.Size(279, 112);
            this.requiresCheckedListBox.Sorted = true;
            this.requiresCheckedListBox.TabIndex = 65;
            this.requiresCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.requiresCheckedListBox_SelectedIndexChanged);
            // 
            // excludesCheckedListBox
            // 
            this.excludesCheckedListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesCheckedListBox.FormattingEnabled = true;
            this.excludesCheckedListBox.Location = new System.Drawing.Point(9, 166);
            this.excludesCheckedListBox.Name = "excludesCheckedListBox";
            this.excludesCheckedListBox.Size = new System.Drawing.Size(267, 112);
            this.excludesCheckedListBox.Sorted = true;
            this.excludesCheckedListBox.TabIndex = 64;
            this.excludesCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.excludesCheckedListBox_SelectedIndexChanged);
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.optionTypeNumericRadioButton);
            this.generalGroupBox.Controls.Add(this.optionTypeBinaryRadioButton);
            this.generalGroupBox.Controls.Add(this.currentOptionTypeLabel);
            this.generalGroupBox.Controls.Add(this.parentLabel);
            this.generalGroupBox.Controls.Add(this.setParentButton);
            this.generalGroupBox.Controls.Add(this.renameOptionButton);
            this.generalGroupBox.Controls.Add(this.selectOptionLabel);
            this.generalGroupBox.Controls.Add(this.selectOptionComboBox);
            this.generalGroupBox.Controls.Add(this.optionalCheckBox);
            this.generalGroupBox.Controls.Add(this.currentParentLabel);
            this.generalGroupBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(531, 108);
            this.generalGroupBox.TabIndex = 52;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General Properties";
            // 
            // optionTypeNumericRadioButton
            // 
            this.optionTypeNumericRadioButton.AutoSize = true;
            this.optionTypeNumericRadioButton.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.optionTypeNumericRadioButton.Location = new System.Drawing.Point(224, 79);
            this.optionTypeNumericRadioButton.Name = "optionTypeNumericRadioButton";
            this.optionTypeNumericRadioButton.Size = new System.Drawing.Size(71, 19);
            this.optionTypeNumericRadioButton.TabIndex = 61;
            this.optionTypeNumericRadioButton.TabStop = true;
            this.optionTypeNumericRadioButton.Text = "Numeric";
            this.optionTypeNumericRadioButton.UseVisualStyleBackColor = true;
            // 
            // optionTypeBinaryRadioButton
            // 
            this.optionTypeBinaryRadioButton.AutoSize = true;
            this.optionTypeBinaryRadioButton.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.optionTypeBinaryRadioButton.Location = new System.Drawing.Point(148, 79);
            this.optionTypeBinaryRadioButton.Name = "optionTypeBinaryRadioButton";
            this.optionTypeBinaryRadioButton.Size = new System.Drawing.Size(61, 19);
            this.optionTypeBinaryRadioButton.TabIndex = 60;
            this.optionTypeBinaryRadioButton.TabStop = true;
            this.optionTypeBinaryRadioButton.Text = "Binary";
            this.optionTypeBinaryRadioButton.UseVisualStyleBackColor = true;
            // 
            // currentOptionTypeLabel
            // 
            this.currentOptionTypeLabel.AutoSize = true;
            this.currentOptionTypeLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentOptionTypeLabel.Location = new System.Drawing.Point(10, 81);
            this.currentOptionTypeLabel.Name = "currentOptionTypeLabel";
            this.currentOptionTypeLabel.Size = new System.Drawing.Size(115, 15);
            this.currentOptionTypeLabel.TabIndex = 59;
            this.currentOptionTypeLabel.Text = "Current option type:";
            // 
            // parentLabel
            // 
            this.parentLabel.AutoSize = true;
            this.parentLabel.Location = new System.Drawing.Point(145, 50);
            this.parentLabel.Name = "parentLabel";
            this.parentLabel.Size = new System.Drawing.Size(40, 15);
            this.parentLabel.TabIndex = 58;
            this.parentLabel.Text = "label1";
            // 
            // setParentButton
            // 
            this.setParentButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setParentButton.Location = new System.Drawing.Point(420, 42);
            this.setParentButton.Name = "setParentButton";
            this.setParentButton.Size = new System.Drawing.Size(101, 23);
            this.setParentButton.TabIndex = 57;
            this.setParentButton.Text = "Set new parent";
            this.setParentButton.UseVisualStyleBackColor = true;
            this.setParentButton.Click += new System.EventHandler(this.setParentButton_Click);
            // 
            // renameOptionButton
            // 
            this.renameOptionButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renameOptionButton.Location = new System.Drawing.Point(420, 15);
            this.renameOptionButton.Name = "renameOptionButton";
            this.renameOptionButton.Size = new System.Drawing.Size(100, 23);
            this.renameOptionButton.TabIndex = 52;
            this.renameOptionButton.Text = "Rename Option";
            this.renameOptionButton.UseVisualStyleBackColor = true;
            this.renameOptionButton.Click += new System.EventHandler(this.renameOptionButton_Click);
            // 
            // selectOptionComboBox
            // 
            this.selectOptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectOptionComboBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectOptionComboBox.FormattingEnabled = true;
            this.selectOptionComboBox.Location = new System.Drawing.Point(148, 16);
            this.selectOptionComboBox.Name = "selectOptionComboBox";
            this.selectOptionComboBox.Size = new System.Drawing.Size(167, 23);
            this.selectOptionComboBox.Sorted = true;
            this.selectOptionComboBox.TabIndex = 1;
            this.selectOptionComboBox.SelectedIndexChanged += new System.EventHandler(this.selectOptionComboBox_SelectedIndexChanged);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.postfixTextBox);
            this.settingsGroupBox.Controls.Add(this.prefixTextBox);
            this.settingsGroupBox.Controls.Add(this.label6);
            this.settingsGroupBox.Controls.Add(this.label5);
            this.settingsGroupBox.Controls.Add(this.outputStringTextBox);
            this.settingsGroupBox.Controls.Add(this.outputStringLabel);
            this.settingsGroupBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsGroupBox.Location = new System.Drawing.Point(12, 216);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(600, 78);
            this.settingsGroupBox.TabIndex = 53;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Other Settings";
            // 
            // postfixTextBox
            // 
            this.postfixTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postfixTextBox.Location = new System.Drawing.Point(231, 16);
            this.postfixTextBox.Name = "postfixTextBox";
            this.postfixTextBox.Size = new System.Drawing.Size(100, 23);
            this.postfixTextBox.TabIndex = 28;
            this.postfixTextBox.TextChanged += new System.EventHandler(this.postfixTextBox_TextChanged);
            // 
            // prefixTextBox
            // 
            this.prefixTextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefixTextBox.Location = new System.Drawing.Point(53, 16);
            this.prefixTextBox.Name = "prefixTextBox";
            this.prefixTextBox.Size = new System.Drawing.Size(100, 23);
            this.prefixTextBox.TabIndex = 27;
            this.prefixTextBox.TextChanged += new System.EventHandler(this.prefixTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(177, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 15);
            this.label6.TabIndex = 26;
            this.label6.Text = "Postfix:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Prefix:";
            // 
            // outputStringTextBox
            // 
            this.outputStringTextBox.Location = new System.Drawing.Point(95, 46);
            this.outputStringTextBox.Name = "outputStringTextBox";
            this.outputStringTextBox.Size = new System.Drawing.Size(172, 23);
            this.outputStringTextBox.TabIndex = 0;
            this.outputStringTextBox.TextChanged += new System.EventHandler(this.outputStringTextBox_TextChanged);
            // 
            // outputStringLabel
            // 
            this.outputStringLabel.AutoSize = true;
            this.outputStringLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputStringLabel.Location = new System.Drawing.Point(6, 49);
            this.outputStringLabel.Name = "outputStringLabel";
            this.outputStringLabel.Size = new System.Drawing.Size(83, 15);
            this.outputStringLabel.TabIndex = 23;
            this.outputStringLabel.Text = "Output string:";
            // 
            // numericSettingsGroupBox
            // 
            this.numericSettingsGroupBox.Controls.Add(this.changeStepSizeButton);
            this.numericSettingsGroupBox.Controls.Add(this.changeRangeButton);
            this.numericSettingsGroupBox.Controls.Add(this.stepSizeLabel);
            this.numericSettingsGroupBox.Controls.Add(this.rangeLabel);
            this.numericSettingsGroupBox.Controls.Add(this.currentStepSizeLabel);
            this.numericSettingsGroupBox.Controls.Add(this.currentRangeOfValuesLabel);
            this.numericSettingsGroupBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericSettingsGroupBox.Location = new System.Drawing.Point(12, 126);
            this.numericSettingsGroupBox.Name = "numericSettingsGroupBox";
            this.numericSettingsGroupBox.Size = new System.Drawing.Size(531, 84);
            this.numericSettingsGroupBox.TabIndex = 54;
            this.numericSettingsGroupBox.TabStop = false;
            this.numericSettingsGroupBox.Text = "Numeric Settings";
            // 
            // changeStepSizeButton
            // 
            this.changeStepSizeButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeStepSizeButton.Location = new System.Drawing.Point(420, 44);
            this.changeStepSizeButton.Name = "changeStepSizeButton";
            this.changeStepSizeButton.Size = new System.Drawing.Size(100, 23);
            this.changeStepSizeButton.TabIndex = 5;
            this.changeStepSizeButton.Text = "New step size";
            this.changeStepSizeButton.UseVisualStyleBackColor = true;
            this.changeStepSizeButton.Click += new System.EventHandler(this.changeStepSizeButton_Click);
            // 
            // changeRangeButton
            // 
            this.changeRangeButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeRangeButton.Location = new System.Drawing.Point(420, 15);
            this.changeRangeButton.Name = "changeRangeButton";
            this.changeRangeButton.Size = new System.Drawing.Size(100, 23);
            this.changeRangeButton.TabIndex = 4;
            this.changeRangeButton.Text = "New range";
            this.changeRangeButton.UseVisualStyleBackColor = true;
            this.changeRangeButton.Click += new System.EventHandler(this.changeRangeButton_Click);
            // 
            // stepSizeLabel
            // 
            this.stepSizeLabel.AutoSize = true;
            this.stepSizeLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepSizeLabel.Location = new System.Drawing.Point(145, 48);
            this.stepSizeLabel.Name = "stepSizeLabel";
            this.stepSizeLabel.Size = new System.Drawing.Size(35, 15);
            this.stepSizeLabel.TabIndex = 3;
            this.stepSizeLabel.Text = "None";
            // 
            // rangeLabel
            // 
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rangeLabel.Location = new System.Drawing.Point(146, 19);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(35, 15);
            this.rangeLabel.TabIndex = 2;
            this.rangeLabel.Text = "None";
            // 
            // currentStepSizeLabel
            // 
            this.currentStepSizeLabel.AutoSize = true;
            this.currentStepSizeLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentStepSizeLabel.Location = new System.Drawing.Point(6, 48);
            this.currentStepSizeLabel.Name = "currentStepSizeLabel";
            this.currentStepSizeLabel.Size = new System.Drawing.Size(101, 15);
            this.currentStepSizeLabel.TabIndex = 1;
            this.currentStepSizeLabel.Text = "Current step size:";
            // 
            // currentRangeOfValuesLabel
            // 
            this.currentRangeOfValuesLabel.AutoSize = true;
            this.currentRangeOfValuesLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentRangeOfValuesLabel.Location = new System.Drawing.Point(6, 19);
            this.currentRangeOfValuesLabel.Name = "currentRangeOfValuesLabel";
            this.currentRangeOfValuesLabel.Size = new System.Drawing.Size(138, 15);
            this.currentRangeOfValuesLabel.TabIndex = 0;
            this.currentRangeOfValuesLabel.Text = "Current range of values:";
            // 
            // EditOptionDialog
            // 
            this.ClientSize = new System.Drawing.Size(619, 624);
            this.Controls.Add(this.numericSettingsGroupBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.constraintsGroupBox);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditOptionDialog";
            this.Text = "Editing options...";
            this.constraintsGroupBox.ResumeLayout(false);
            this.constraintsGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.numericSettingsGroupBox.ResumeLayout(false);
            this.numericSettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label selectOptionLabel;
        private System.Windows.Forms.CheckBox optionalCheckBox;
        private System.Windows.Forms.Label currentParentLabel;
        private System.Windows.Forms.GroupBox constraintsGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.Button renameOptionButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.TextBox outputStringTextBox;
        private System.Windows.Forms.Label outputStringLabel;
        private System.Windows.Forms.Button setParentButton;
        private System.Windows.Forms.ComboBox selectOptionComboBox;
        private System.Windows.Forms.CheckedListBox excludesCheckedListBox;
        private System.Windows.Forms.CheckedListBox requiresCheckedListBox;
        private System.Windows.Forms.Label requiresLabel;
        private System.Windows.Forms.Label excludesLabel;
        private System.Windows.Forms.Label parentLabel;
        private System.Windows.Forms.Button requiresDelButton;
        private System.Windows.Forms.ListBox requiresOverviewListBox;
        private System.Windows.Forms.Button excludesDelButton;
        private System.Windows.Forms.ListBox excludesOverviewListBox;
        private System.Windows.Forms.Button requiresAddButton;
        private System.Windows.Forms.Button excludesAddButton;
        private System.Windows.Forms.TextBox postfixTextBox;
        private System.Windows.Forms.TextBox prefixTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox numericSettingsGroupBox;
        private System.Windows.Forms.Button changeStepSizeButton;
        private System.Windows.Forms.Button changeRangeButton;
        private System.Windows.Forms.Label stepSizeLabel;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.Label currentStepSizeLabel;
        private System.Windows.Forms.Label currentRangeOfValuesLabel;
        private System.Windows.Forms.RadioButton optionTypeNumericRadioButton;
        private System.Windows.Forms.RadioButton optionTypeBinaryRadioButton;
        private System.Windows.Forms.Label currentOptionTypeLabel;
    }
}