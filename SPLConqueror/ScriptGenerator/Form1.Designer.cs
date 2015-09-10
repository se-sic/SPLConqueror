namespace ScriptGenerator
{
    partial class Form1
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
            this.GenerateScript = new System.Windows.Forms.Button();
            this.MlSettings_Box = new System.Windows.Forms.GroupBox();
            this.AddMlSetting = new System.Windows.Forms.Button();
            this.mlSettingsPanel = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.addedElementsBox = new System.Windows.Forms.GroupBox();
            this.removeElement = new System.Windows.Forms.Button();
            this.addedElementsList = new System.Windows.Forms.ListBox();
            this.varModel_button = new System.Windows.Forms.Button();
            this.addResultFile_Group = new System.Windows.Forms.GroupBox();
            this.addResultFile = new System.Windows.Forms.Button();
            this.defineVMforResultcheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.locateResultFile_button = new System.Windows.Forms.Button();
            this.informatioLabel = new System.Windows.Forms.Label();
            this.bsamp_group = new System.Windows.Forms.GroupBox();
            this.bsamp_random__modulo_textBox = new System.Windows.Forms.TextBox();
            this.bsamp_random_textBox = new System.Windows.Forms.TextBox();
            this.bsamp_random_box = new System.Windows.Forms.CheckBox();
            this.bsamp_all_box = new System.Windows.Forms.CheckBox();
            this.bsamp_negFW_box = new System.Windows.Forms.CheckBox();
            this.bsamp_PW_box = new System.Windows.Forms.CheckBox();
            this.bsamp_FW_box = new System.Windows.Forms.CheckBox();
            this.bsamp_ForValidation = new System.Windows.Forms.CheckBox();
            this.bsamp_addButton = new System.Windows.Forms.Button();
            this.expDasign_group = new System.Windows.Forms.GroupBox();
            this.num_oneFactorAtATime_num_Text = new System.Windows.Forms.TextBox();
            this.num_oneFactorAtATime_num_Label = new System.Windows.Forms.Label();
            this.num_oneFactorAtATime_Box = new System.Windows.Forms.CheckBox();
            this.num_rand_seed_Text = new System.Windows.Forms.TextBox();
            this.num_rand_seed_Label = new System.Windows.Forms.Label();
            this.num_Plackett_n_Box = new System.Windows.Forms.TextBox();
            this.num_Plackett_n_Label = new System.Windows.Forms.Label();
            this.num_Plackett_Level_Box = new System.Windows.Forms.TextBox();
            this.num_Plackett_Level_Label = new System.Windows.Forms.Label();
            this.num_hyper_percent_text = new System.Windows.Forms.TextBox();
            this.num_hyper_percent_Label = new System.Windows.Forms.Label();
            this.num_random_n_Text = new System.Windows.Forms.TextBox();
            this.num_rand_n_Label = new System.Windows.Forms.Label();
            this.num_kEx_k_Box = new System.Windows.Forms.TextBox();
            this.num_kEx_k_Label = new System.Windows.Forms.Label();
            this.num_kEx_n_Box = new System.Windows.Forms.TextBox();
            this.num_kEx_n_Label = new System.Windows.Forms.Label();
            this.num_hyperSampling_check = new System.Windows.Forms.CheckBox();
            this.num_randomSampling_num = new System.Windows.Forms.CheckBox();
            this.num_PlackettBurman_check = new System.Windows.Forms.CheckBox();
            this.num_kEx_check = new System.Windows.Forms.CheckBox();
            this.num_FullFactorial_check = new System.Windows.Forms.CheckBox();
            this.num_CentralComposite_check = new System.Windows.Forms.CheckBox();
            this.num_BoxBehnken_check = new System.Windows.Forms.CheckBox();
            this.num_forValidationCheckBox = new System.Windows.Forms.CheckBox();
            this.expDesign_addButton = new System.Windows.Forms.Button();
            this.logFile_Button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.learn_button = new System.Windows.Forms.RadioButton();
            this.print_button = new System.Windows.Forms.RadioButton();
            this.MlSettings_Box.SuspendLayout();
            this.mlSettingsPanel.SuspendLayout();
            this.addedElementsBox.SuspendLayout();
            this.addResultFile_Group.SuspendLayout();
            this.bsamp_group.SuspendLayout();
            this.expDasign_group.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GenerateScript
            // 
            this.GenerateScript.Location = new System.Drawing.Point(703, 367);
            this.GenerateScript.Name = "GenerateScript";
            this.GenerateScript.Size = new System.Drawing.Size(75, 23);
            this.GenerateScript.TabIndex = 0;
            this.GenerateScript.Text = "generate";
            this.GenerateScript.UseVisualStyleBackColor = true;
            this.GenerateScript.Click += new System.EventHandler(this.GenerateScript_Click);
            // 
            // MlSettings_Box
            // 
            this.MlSettings_Box.Controls.Add(this.AddMlSetting);
            this.MlSettings_Box.Controls.Add(this.mlSettingsPanel);
            this.MlSettings_Box.Location = new System.Drawing.Point(503, 12);
            this.MlSettings_Box.Name = "MlSettings_Box";
            this.MlSettings_Box.Size = new System.Drawing.Size(292, 221);
            this.MlSettings_Box.TabIndex = 1;
            this.MlSettings_Box.TabStop = false;
            this.MlSettings_Box.Text = "mlSettings";
            // 
            // AddMlSetting
            // 
            this.AddMlSetting.Location = new System.Drawing.Point(200, 193);
            this.AddMlSetting.Name = "AddMlSetting";
            this.AddMlSetting.Size = new System.Drawing.Size(75, 23);
            this.AddMlSetting.TabIndex = 1;
            this.AddMlSetting.Text = "add";
            this.AddMlSetting.UseVisualStyleBackColor = true;
            this.AddMlSetting.Click += new System.EventHandler(this.AddMlSetting_Click);
            // 
            // mlSettingsPanel
            // 
            this.mlSettingsPanel.Controls.Add(this.vScrollBar1);
            this.mlSettingsPanel.Location = new System.Drawing.Point(7, 20);
            this.mlSettingsPanel.Name = "mlSettingsPanel";
            this.mlSettingsPanel.Size = new System.Drawing.Size(279, 166);
            this.mlSettingsPanel.TabIndex = 0;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(262, 1);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 165);
            this.vScrollBar1.TabIndex = 0;
            // 
            // addedElementsBox
            // 
            this.addedElementsBox.Controls.Add(this.removeElement);
            this.addedElementsBox.Controls.Add(this.addedElementsList);
            this.addedElementsBox.Location = new System.Drawing.Point(503, 258);
            this.addedElementsBox.Name = "addedElementsBox";
            this.addedElementsBox.Size = new System.Drawing.Size(194, 153);
            this.addedElementsBox.TabIndex = 2;
            this.addedElementsBox.TabStop = false;
            this.addedElementsBox.Text = "elements";
            // 
            // removeElement
            // 
            this.removeElement.Location = new System.Drawing.Point(7, 122);
            this.removeElement.Name = "removeElement";
            this.removeElement.Size = new System.Drawing.Size(75, 23);
            this.removeElement.TabIndex = 1;
            this.removeElement.Text = "remove";
            this.removeElement.UseVisualStyleBackColor = true;
            this.removeElement.Click += new System.EventHandler(this.removeElement_Click);
            // 
            // addedElementsList
            // 
            this.addedElementsList.FormattingEnabled = true;
            this.addedElementsList.Location = new System.Drawing.Point(7, 20);
            this.addedElementsList.Name = "addedElementsList";
            this.addedElementsList.Size = new System.Drawing.Size(181, 95);
            this.addedElementsList.TabIndex = 0;
            // 
            // varModel_button
            // 
            this.varModel_button.Location = new System.Drawing.Point(13, 13);
            this.varModel_button.Name = "varModel_button";
            this.varModel_button.Size = new System.Drawing.Size(106, 23);
            this.varModel_button.TabIndex = 3;
            this.varModel_button.Text = "addVariablityModel";
            this.varModel_button.UseVisualStyleBackColor = true;
            this.varModel_button.Click += new System.EventHandler(this.addVariabilityModel_Click);
            // 
            // addResultFile_Group
            // 
            this.addResultFile_Group.Controls.Add(this.addResultFile);
            this.addResultFile_Group.Controls.Add(this.defineVMforResultcheckedListBox);
            this.addResultFile_Group.Controls.Add(this.locateResultFile_button);
            this.addResultFile_Group.Location = new System.Drawing.Point(13, 43);
            this.addResultFile_Group.Name = "addResultFile_Group";
            this.addResultFile_Group.Size = new System.Drawing.Size(200, 190);
            this.addResultFile_Group.TabIndex = 4;
            this.addResultFile_Group.TabStop = false;
            this.addResultFile_Group.Text = "add result file";
            // 
            // addResultFile
            // 
            this.addResultFile.Location = new System.Drawing.Point(57, 161);
            this.addResultFile.Name = "addResultFile";
            this.addResultFile.Size = new System.Drawing.Size(75, 23);
            this.addResultFile.TabIndex = 2;
            this.addResultFile.Text = "add";
            this.addResultFile.UseVisualStyleBackColor = true;
            this.addResultFile.Click += new System.EventHandler(this.addResultFile_Click);
            // 
            // defineVMforResultcheckedListBox
            // 
            this.defineVMforResultcheckedListBox.FormattingEnabled = true;
            this.defineVMforResultcheckedListBox.Location = new System.Drawing.Point(7, 50);
            this.defineVMforResultcheckedListBox.Name = "defineVMforResultcheckedListBox";
            this.defineVMforResultcheckedListBox.Size = new System.Drawing.Size(173, 34);
            this.defineVMforResultcheckedListBox.TabIndex = 1;
            // 
            // locateResultFile_button
            // 
            this.locateResultFile_button.Location = new System.Drawing.Point(7, 20);
            this.locateResultFile_button.Name = "locateResultFile_button";
            this.locateResultFile_button.Size = new System.Drawing.Size(75, 23);
            this.locateResultFile_button.TabIndex = 0;
            this.locateResultFile_button.Text = "define file";
            this.locateResultFile_button.UseVisualStyleBackColor = true;
            this.locateResultFile_button.Click += new System.EventHandler(this.LocateResultFile_button_Click);
            // 
            // informatioLabel
            // 
            this.informatioLabel.AutoSize = true;
            this.informatioLabel.Location = new System.Drawing.Point(17, 390);
            this.informatioLabel.Name = "informatioLabel";
            this.informatioLabel.Size = new System.Drawing.Size(58, 13);
            this.informatioLabel.TabIndex = 5;
            this.informatioLabel.Text = "information";
            // 
            // bsamp_group
            // 
            this.bsamp_group.Controls.Add(this.bsamp_random__modulo_textBox);
            this.bsamp_group.Controls.Add(this.bsamp_random_textBox);
            this.bsamp_group.Controls.Add(this.bsamp_random_box);
            this.bsamp_group.Controls.Add(this.bsamp_all_box);
            this.bsamp_group.Controls.Add(this.bsamp_negFW_box);
            this.bsamp_group.Controls.Add(this.bsamp_PW_box);
            this.bsamp_group.Controls.Add(this.bsamp_FW_box);
            this.bsamp_group.Controls.Add(this.bsamp_ForValidation);
            this.bsamp_group.Controls.Add(this.bsamp_addButton);
            this.bsamp_group.Location = new System.Drawing.Point(13, 239);
            this.bsamp_group.Name = "bsamp_group";
            this.bsamp_group.Size = new System.Drawing.Size(200, 142);
            this.bsamp_group.TabIndex = 6;
            this.bsamp_group.TabStop = false;
            this.bsamp_group.Text = "binary sampling heuristics";
            // 
            // bsamp_random__modulo_textBox
            // 
            this.bsamp_random__modulo_textBox.Location = new System.Drawing.Point(138, 73);
            this.bsamp_random__modulo_textBox.Name = "bsamp_random__modulo_textBox";
            this.bsamp_random__modulo_textBox.ShortcutsEnabled = false;
            this.bsamp_random__modulo_textBox.Size = new System.Drawing.Size(32, 20);
            this.bsamp_random__modulo_textBox.TabIndex = 8;
            this.bsamp_random__modulo_textBox.Text = "5";
            // 
            // bsamp_random_textBox
            // 
            this.bsamp_random_textBox.Location = new System.Drawing.Point(100, 73);
            this.bsamp_random_textBox.Name = "bsamp_random_textBox";
            this.bsamp_random_textBox.Size = new System.Drawing.Size(32, 20);
            this.bsamp_random_textBox.TabIndex = 7;
            this.bsamp_random_textBox.Text = "100";
            // 
            // bsamp_random_box
            // 
            this.bsamp_random_box.AutoSize = true;
            this.bsamp_random_box.Location = new System.Drawing.Point(7, 75);
            this.bsamp_random_box.Name = "bsamp_random_box";
            this.bsamp_random_box.Size = new System.Drawing.Size(61, 17);
            this.bsamp_random_box.TabIndex = 6;
            this.bsamp_random_box.Text = "random";
            this.bsamp_random_box.UseVisualStyleBackColor = true;
            // 
            // bsamp_all_box
            // 
            this.bsamp_all_box.AutoSize = true;
            this.bsamp_all_box.Location = new System.Drawing.Point(100, 28);
            this.bsamp_all_box.Name = "bsamp_all_box";
            this.bsamp_all_box.Size = new System.Drawing.Size(36, 17);
            this.bsamp_all_box.TabIndex = 5;
            this.bsamp_all_box.Text = "all";
            this.bsamp_all_box.UseVisualStyleBackColor = true;
            // 
            // bsamp_negFW_box
            // 
            this.bsamp_negFW_box.AutoSize = true;
            this.bsamp_negFW_box.Location = new System.Drawing.Point(100, 52);
            this.bsamp_negFW_box.Name = "bsamp_negFW_box";
            this.bsamp_negFW_box.Size = new System.Drawing.Size(61, 17);
            this.bsamp_negFW_box.TabIndex = 4;
            this.bsamp_negFW_box.Text = "negFW";
            this.bsamp_negFW_box.UseVisualStyleBackColor = true;
            // 
            // bsamp_PW_box
            // 
            this.bsamp_PW_box.AutoSize = true;
            this.bsamp_PW_box.Location = new System.Drawing.Point(7, 52);
            this.bsamp_PW_box.Name = "bsamp_PW_box";
            this.bsamp_PW_box.Size = new System.Drawing.Size(44, 17);
            this.bsamp_PW_box.TabIndex = 3;
            this.bsamp_PW_box.Text = "PW";
            this.bsamp_PW_box.UseVisualStyleBackColor = true;
            // 
            // bsamp_FW_box
            // 
            this.bsamp_FW_box.AutoSize = true;
            this.bsamp_FW_box.Location = new System.Drawing.Point(7, 28);
            this.bsamp_FW_box.Name = "bsamp_FW_box";
            this.bsamp_FW_box.Size = new System.Drawing.Size(43, 17);
            this.bsamp_FW_box.TabIndex = 2;
            this.bsamp_FW_box.Text = "FW";
            this.bsamp_FW_box.UseVisualStyleBackColor = true;
            // 
            // bsamp_ForValidation
            // 
            this.bsamp_ForValidation.AutoSize = true;
            this.bsamp_ForValidation.Location = new System.Drawing.Point(94, 115);
            this.bsamp_ForValidation.Name = "bsamp_ForValidation";
            this.bsamp_ForValidation.Size = new System.Drawing.Size(86, 17);
            this.bsamp_ForValidation.TabIndex = 1;
            this.bsamp_ForValidation.Text = "for validation";
            this.bsamp_ForValidation.UseVisualStyleBackColor = true;
            // 
            // bsamp_addButton
            // 
            this.bsamp_addButton.Location = new System.Drawing.Point(7, 111);
            this.bsamp_addButton.Name = "bsamp_addButton";
            this.bsamp_addButton.Size = new System.Drawing.Size(75, 23);
            this.bsamp_addButton.TabIndex = 0;
            this.bsamp_addButton.Text = "add";
            this.bsamp_addButton.UseVisualStyleBackColor = true;
            this.bsamp_addButton.Click += new System.EventHandler(this.bsamp_addButton_Click);
            // 
            // expDasign_group
            // 
            this.expDasign_group.Controls.Add(this.num_oneFactorAtATime_num_Text);
            this.expDasign_group.Controls.Add(this.num_oneFactorAtATime_num_Label);
            this.expDasign_group.Controls.Add(this.num_oneFactorAtATime_Box);
            this.expDasign_group.Controls.Add(this.num_rand_seed_Text);
            this.expDasign_group.Controls.Add(this.num_rand_seed_Label);
            this.expDasign_group.Controls.Add(this.num_Plackett_n_Box);
            this.expDasign_group.Controls.Add(this.num_Plackett_n_Label);
            this.expDasign_group.Controls.Add(this.num_Plackett_Level_Box);
            this.expDasign_group.Controls.Add(this.num_Plackett_Level_Label);
            this.expDasign_group.Controls.Add(this.num_hyper_percent_text);
            this.expDasign_group.Controls.Add(this.num_hyper_percent_Label);
            this.expDasign_group.Controls.Add(this.num_random_n_Text);
            this.expDasign_group.Controls.Add(this.num_rand_n_Label);
            this.expDasign_group.Controls.Add(this.num_kEx_k_Box);
            this.expDasign_group.Controls.Add(this.num_kEx_k_Label);
            this.expDasign_group.Controls.Add(this.num_kEx_n_Box);
            this.expDasign_group.Controls.Add(this.num_kEx_n_Label);
            this.expDasign_group.Controls.Add(this.num_hyperSampling_check);
            this.expDasign_group.Controls.Add(this.num_randomSampling_num);
            this.expDasign_group.Controls.Add(this.num_PlackettBurman_check);
            this.expDasign_group.Controls.Add(this.num_kEx_check);
            this.expDasign_group.Controls.Add(this.num_FullFactorial_check);
            this.expDasign_group.Controls.Add(this.num_CentralComposite_check);
            this.expDasign_group.Controls.Add(this.num_BoxBehnken_check);
            this.expDasign_group.Controls.Add(this.num_forValidationCheckBox);
            this.expDasign_group.Controls.Add(this.expDesign_addButton);
            this.expDasign_group.Location = new System.Drawing.Point(219, 13);
            this.expDasign_group.Name = "expDasign_group";
            this.expDasign_group.Size = new System.Drawing.Size(278, 251);
            this.expDasign_group.TabIndex = 7;
            this.expDasign_group.TabStop = false;
            this.expDasign_group.Text = "Experimental design";
            // 
            // num_oneFactorAtATime_num_Text
            // 
            this.num_oneFactorAtATime_num_Text.Location = new System.Drawing.Point(199, 179);
            this.num_oneFactorAtATime_num_Text.Name = "num_oneFactorAtATime_num_Text";
            this.num_oneFactorAtATime_num_Text.Size = new System.Drawing.Size(38, 20);
            this.num_oneFactorAtATime_num_Text.TabIndex = 34;
            this.num_oneFactorAtATime_num_Text.Text = "5";
            this.num_oneFactorAtATime_num_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_oneFactorAtATime_num_Label
            // 
            this.num_oneFactorAtATime_num_Label.AutoSize = true;
            this.num_oneFactorAtATime_num_Label.Location = new System.Drawing.Point(152, 181);
            this.num_oneFactorAtATime_num_Label.Name = "num_oneFactorAtATime_num_Label";
            this.num_oneFactorAtATime_num_Label.Size = new System.Drawing.Size(41, 13);
            this.num_oneFactorAtATime_num_Label.TabIndex = 33;
            this.num_oneFactorAtATime_num_Label.Text = "values:";
            // 
            // num_oneFactorAtATime_Box
            // 
            this.num_oneFactorAtATime_Box.AutoSize = true;
            this.num_oneFactorAtATime_Box.Location = new System.Drawing.Point(7, 181);
            this.num_oneFactorAtATime_Box.Name = "num_oneFactorAtATime_Box";
            this.num_oneFactorAtATime_Box.Size = new System.Drawing.Size(126, 17);
            this.num_oneFactorAtATime_Box.TabIndex = 32;
            this.num_oneFactorAtATime_Box.Text = "One Factor at a Time";
            this.num_oneFactorAtATime_Box.UseVisualStyleBackColor = true;
            // 
            // num_rand_seed_Text
            // 
            this.num_rand_seed_Text.Location = new System.Drawing.Point(238, 137);
            this.num_rand_seed_Text.Name = "num_rand_seed_Text";
            this.num_rand_seed_Text.Size = new System.Drawing.Size(29, 20);
            this.num_rand_seed_Text.TabIndex = 31;
            this.num_rand_seed_Text.Text = "0";
            this.num_rand_seed_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_rand_seed_Label
            // 
            this.num_rand_seed_Label.AutoSize = true;
            this.num_rand_seed_Label.Location = new System.Drawing.Point(205, 137);
            this.num_rand_seed_Label.Name = "num_rand_seed_Label";
            this.num_rand_seed_Label.Size = new System.Drawing.Size(33, 13);
            this.num_rand_seed_Label.TabIndex = 30;
            this.num_rand_seed_Label.Text = "seed:";
            // 
            // num_Plackett_n_Box
            // 
            this.num_Plackett_n_Box.Location = new System.Drawing.Point(238, 110);
            this.num_Plackett_n_Box.Name = "num_Plackett_n_Box";
            this.num_Plackett_n_Box.Size = new System.Drawing.Size(29, 20);
            this.num_Plackett_n_Box.TabIndex = 29;
            this.num_Plackett_n_Box.Text = "9";
            this.num_Plackett_n_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_Plackett_n_Label
            // 
            this.num_Plackett_n_Label.AutoSize = true;
            this.num_Plackett_n_Label.Location = new System.Drawing.Point(221, 112);
            this.num_Plackett_n_Label.Name = "num_Plackett_n_Label";
            this.num_Plackett_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_Plackett_n_Label.TabIndex = 28;
            this.num_Plackett_n_Label.Text = "n:";
            // 
            // num_Plackett_Level_Box
            // 
            this.num_Plackett_Level_Box.Location = new System.Drawing.Point(185, 110);
            this.num_Plackett_Level_Box.Name = "num_Plackett_Level_Box";
            this.num_Plackett_Level_Box.Size = new System.Drawing.Size(31, 20);
            this.num_Plackett_Level_Box.TabIndex = 27;
            this.num_Plackett_Level_Box.Text = "3";
            this.num_Plackett_Level_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_Plackett_Level_Label
            // 
            this.num_Plackett_Level_Label.AutoSize = true;
            this.num_Plackett_Level_Label.Location = new System.Drawing.Point(152, 112);
            this.num_Plackett_Level_Label.Name = "num_Plackett_Level_Label";
            this.num_Plackett_Level_Label.Size = new System.Drawing.Size(32, 13);
            this.num_Plackett_Level_Label.TabIndex = 26;
            this.num_Plackett_Level_Label.Text = "level:";
            // 
            // num_hyper_percent_text
            // 
            this.num_hyper_percent_text.Location = new System.Drawing.Point(202, 155);
            this.num_hyper_percent_text.Name = "num_hyper_percent_text";
            this.num_hyper_percent_text.Size = new System.Drawing.Size(36, 20);
            this.num_hyper_percent_text.TabIndex = 25;
            this.num_hyper_percent_text.Text = "10";
            this.num_hyper_percent_text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_hyper_percent_Label
            // 
            this.num_hyper_percent_Label.AutoSize = true;
            this.num_hyper_percent_Label.Location = new System.Drawing.Point(152, 158);
            this.num_hyper_percent_Label.Name = "num_hyper_percent_Label";
            this.num_hyper_percent_Label.Size = new System.Drawing.Size(46, 13);
            this.num_hyper_percent_Label.TabIndex = 24;
            this.num_hyper_percent_Label.Text = "percent:";
            // 
            // num_random_n_Text
            // 
            this.num_random_n_Text.Location = new System.Drawing.Point(172, 134);
            this.num_random_n_Text.Name = "num_random_n_Text";
            this.num_random_n_Text.Size = new System.Drawing.Size(30, 20);
            this.num_random_n_Text.TabIndex = 23;
            this.num_random_n_Text.Text = "100";
            this.num_random_n_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_rand_n_Label
            // 
            this.num_rand_n_Label.AutoSize = true;
            this.num_rand_n_Label.Location = new System.Drawing.Point(152, 137);
            this.num_rand_n_Label.Name = "num_rand_n_Label";
            this.num_rand_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_rand_n_Label.TabIndex = 22;
            this.num_rand_n_Label.Text = "n:";
            // 
            // num_kEx_k_Box
            // 
            this.num_kEx_k_Box.Location = new System.Drawing.Point(224, 87);
            this.num_kEx_k_Box.Name = "num_kEx_k_Box";
            this.num_kEx_k_Box.Size = new System.Drawing.Size(33, 20);
            this.num_kEx_k_Box.TabIndex = 21;
            this.num_kEx_k_Box.Text = "5";
            this.num_kEx_k_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_kEx_k_Label
            // 
            this.num_kEx_k_Label.AutoSize = true;
            this.num_kEx_k_Label.Location = new System.Drawing.Point(211, 90);
            this.num_kEx_k_Label.Name = "num_kEx_k_Label";
            this.num_kEx_k_Label.Size = new System.Drawing.Size(16, 13);
            this.num_kEx_k_Label.TabIndex = 20;
            this.num_kEx_k_Label.Text = "k:";
            // 
            // num_kEx_n_Box
            // 
            this.num_kEx_n_Box.Location = new System.Drawing.Point(166, 87);
            this.num_kEx_n_Box.Name = "num_kEx_n_Box";
            this.num_kEx_n_Box.Size = new System.Drawing.Size(39, 20);
            this.num_kEx_n_Box.TabIndex = 19;
            this.num_kEx_n_Box.Text = "50";
            this.num_kEx_n_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_kEx_n_Label
            // 
            this.num_kEx_n_Label.AutoSize = true;
            this.num_kEx_n_Label.Location = new System.Drawing.Point(152, 89);
            this.num_kEx_n_Label.Name = "num_kEx_n_Label";
            this.num_kEx_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_kEx_n_Label.TabIndex = 18;
            this.num_kEx_n_Label.Text = "n:";
            // 
            // num_hyperSampling_check
            // 
            this.num_hyperSampling_check.AutoSize = true;
            this.num_hyperSampling_check.Location = new System.Drawing.Point(7, 158);
            this.num_hyperSampling_check.Name = "num_hyperSampling_check";
            this.num_hyperSampling_check.Size = new System.Drawing.Size(98, 17);
            this.num_hyperSampling_check.TabIndex = 15;
            this.num_hyperSampling_check.Text = "Hyper sampling";
            this.num_hyperSampling_check.UseVisualStyleBackColor = true;
            // 
            // num_randomSampling_num
            // 
            this.num_randomSampling_num.AutoSize = true;
            this.num_randomSampling_num.Location = new System.Drawing.Point(7, 135);
            this.num_randomSampling_num.Name = "num_randomSampling_num";
            this.num_randomSampling_num.Size = new System.Drawing.Size(110, 17);
            this.num_randomSampling_num.TabIndex = 14;
            this.num_randomSampling_num.Text = "Random sampling";
            this.num_randomSampling_num.UseVisualStyleBackColor = true;
            // 
            // num_PlackettBurman_check
            // 
            this.num_PlackettBurman_check.AutoSize = true;
            this.num_PlackettBurman_check.Location = new System.Drawing.Point(7, 112);
            this.num_PlackettBurman_check.Name = "num_PlackettBurman_check";
            this.num_PlackettBurman_check.Size = new System.Drawing.Size(101, 17);
            this.num_PlackettBurman_check.TabIndex = 13;
            this.num_PlackettBurman_check.Text = "PlackettBurman";
            this.num_PlackettBurman_check.UseVisualStyleBackColor = true;
            // 
            // num_kEx_check
            // 
            this.num_kEx_check.AutoSize = true;
            this.num_kEx_check.Location = new System.Drawing.Point(7, 89);
            this.num_kEx_check.Name = "num_kEx_check";
            this.num_kEx_check.Size = new System.Drawing.Size(128, 17);
            this.num_kEx_check.TabIndex = 12;
            this.num_kEx_check.Text = "k-Exchange algorithm";
            this.num_kEx_check.UseVisualStyleBackColor = true;
            // 
            // num_FullFactorial_check
            // 
            this.num_FullFactorial_check.AutoSize = true;
            this.num_FullFactorial_check.Location = new System.Drawing.Point(7, 66);
            this.num_FullFactorial_check.Name = "num_FullFactorial_check";
            this.num_FullFactorial_check.Size = new System.Drawing.Size(85, 17);
            this.num_FullFactorial_check.TabIndex = 11;
            this.num_FullFactorial_check.Text = "Full-Factorial";
            this.num_FullFactorial_check.UseVisualStyleBackColor = true;
            // 
            // num_CentralComposite_check
            // 
            this.num_CentralComposite_check.AutoSize = true;
            this.num_CentralComposite_check.Location = new System.Drawing.Point(7, 43);
            this.num_CentralComposite_check.Name = "num_CentralComposite_check";
            this.num_CentralComposite_check.Size = new System.Drawing.Size(108, 17);
            this.num_CentralComposite_check.TabIndex = 10;
            this.num_CentralComposite_check.Text = "CentralComposite";
            this.num_CentralComposite_check.UseVisualStyleBackColor = true;
            // 
            // num_BoxBehnken_check
            // 
            this.num_BoxBehnken_check.AutoSize = true;
            this.num_BoxBehnken_check.Location = new System.Drawing.Point(7, 20);
            this.num_BoxBehnken_check.Name = "num_BoxBehnken_check";
            this.num_BoxBehnken_check.Size = new System.Drawing.Size(87, 17);
            this.num_BoxBehnken_check.TabIndex = 9;
            this.num_BoxBehnken_check.Text = "BoxBehnken";
            this.num_BoxBehnken_check.UseVisualStyleBackColor = true;
            // 
            // num_forValidationCheckBox
            // 
            this.num_forValidationCheckBox.AutoSize = true;
            this.num_forValidationCheckBox.Location = new System.Drawing.Point(151, 224);
            this.num_forValidationCheckBox.Name = "num_forValidationCheckBox";
            this.num_forValidationCheckBox.Size = new System.Drawing.Size(86, 17);
            this.num_forValidationCheckBox.TabIndex = 1;
            this.num_forValidationCheckBox.Text = "for validation";
            this.num_forValidationCheckBox.UseVisualStyleBackColor = true;
            // 
            // expDesign_addButton
            // 
            this.expDesign_addButton.Location = new System.Drawing.Point(7, 220);
            this.expDesign_addButton.Name = "expDesign_addButton";
            this.expDesign_addButton.Size = new System.Drawing.Size(75, 23);
            this.expDesign_addButton.TabIndex = 0;
            this.expDesign_addButton.Text = "add";
            this.expDesign_addButton.UseVisualStyleBackColor = true;
            this.expDesign_addButton.Click += new System.EventHandler(this.expDesign_addButton_Click);
            // 
            // logFile_Button
            // 
            this.logFile_Button.Location = new System.Drawing.Point(125, 14);
            this.logFile_Button.Name = "logFile_Button";
            this.logFile_Button.Size = new System.Drawing.Size(88, 23);
            this.logFile_Button.TabIndex = 8;
            this.logFile_Button.Text = "log-File";
            this.logFile_Button.UseVisualStyleBackColor = true;
            this.logFile_Button.Click += new System.EventHandler(this.logFile_Button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.learn_button);
            this.groupBox1.Controls.Add(this.print_button);
            this.groupBox1.Location = new System.Drawing.Point(703, 258);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(92, 74);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // learn_button
            // 
            this.learn_button.Location = new System.Drawing.Point(7, 44);
            this.learn_button.Name = "learn_button";
            this.learn_button.Size = new System.Drawing.Size(52, 24);
            this.learn_button.TabIndex = 1;
            this.learn_button.TabStop = true;
            this.learn_button.Text = "learn";
            this.learn_button.UseVisualStyleBackColor = true;
            // 
            // print_button
            // 
            this.print_button.AutoSize = true;
            this.print_button.Location = new System.Drawing.Point(7, 20);
            this.print_button.Name = "print_button";
            this.print_button.Size = new System.Drawing.Size(45, 17);
            this.print_button.TabIndex = 0;
            this.print_button.TabStop = true;
            this.print_button.Text = "print";
            this.print_button.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 415);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.logFile_Button);
            this.Controls.Add(this.expDasign_group);
            this.Controls.Add(this.bsamp_group);
            this.Controls.Add(this.informatioLabel);
            this.Controls.Add(this.addResultFile_Group);
            this.Controls.Add(this.varModel_button);
            this.Controls.Add(this.addedElementsBox);
            this.Controls.Add(this.MlSettings_Box);
            this.Controls.Add(this.GenerateScript);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MlSettings_Box.ResumeLayout(false);
            this.mlSettingsPanel.ResumeLayout(false);
            this.addedElementsBox.ResumeLayout(false);
            this.addResultFile_Group.ResumeLayout(false);
            this.bsamp_group.ResumeLayout(false);
            this.bsamp_group.PerformLayout();
            this.expDasign_group.ResumeLayout(false);
            this.expDasign_group.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateScript;
        private System.Windows.Forms.GroupBox MlSettings_Box;
        private System.Windows.Forms.Panel mlSettingsPanel;
        private System.Windows.Forms.Button AddMlSetting;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.GroupBox addedElementsBox;
        private System.Windows.Forms.Button removeElement;
        private System.Windows.Forms.ListBox addedElementsList;
        private System.Windows.Forms.Button varModel_button;
        private System.Windows.Forms.GroupBox addResultFile_Group;
        private System.Windows.Forms.Button locateResultFile_button;
        private System.Windows.Forms.CheckedListBox defineVMforResultcheckedListBox;
        private System.Windows.Forms.Button addResultFile;
        private System.Windows.Forms.Label informatioLabel;
        private System.Windows.Forms.GroupBox bsamp_group;
        private System.Windows.Forms.Button bsamp_addButton;
        private System.Windows.Forms.CheckBox bsamp_ForValidation;
        private System.Windows.Forms.CheckBox bsamp_PW_box;
        private System.Windows.Forms.CheckBox bsamp_FW_box;
        private System.Windows.Forms.CheckBox bsamp_negFW_box;
        private System.Windows.Forms.CheckBox bsamp_all_box;
        private System.Windows.Forms.CheckBox bsamp_random_box;
        private System.Windows.Forms.TextBox bsamp_random_textBox;
        private System.Windows.Forms.GroupBox expDasign_group;
        private System.Windows.Forms.CheckBox num_forValidationCheckBox;
        private System.Windows.Forms.Button expDesign_addButton;
        private System.Windows.Forms.CheckBox num_hyperSampling_check;
        private System.Windows.Forms.CheckBox num_randomSampling_num;
        private System.Windows.Forms.CheckBox num_PlackettBurman_check;
        private System.Windows.Forms.CheckBox num_kEx_check;
        private System.Windows.Forms.CheckBox num_FullFactorial_check;
        private System.Windows.Forms.CheckBox num_CentralComposite_check;
        private System.Windows.Forms.CheckBox num_BoxBehnken_check;
        private System.Windows.Forms.Label num_kEx_n_Label;
        private System.Windows.Forms.TextBox num_kEx_n_Box;
        private System.Windows.Forms.Label num_kEx_k_Label;
        private System.Windows.Forms.TextBox num_kEx_k_Box;
        private System.Windows.Forms.Label num_rand_n_Label;
        private System.Windows.Forms.TextBox num_random_n_Text;
        private System.Windows.Forms.Label num_hyper_percent_Label;
        private System.Windows.Forms.TextBox num_hyper_percent_text;
        private System.Windows.Forms.TextBox num_Plackett_Level_Box;
        private System.Windows.Forms.Label num_Plackett_Level_Label;
        private System.Windows.Forms.Label num_Plackett_n_Label;
        private System.Windows.Forms.TextBox num_Plackett_n_Box;
        private System.Windows.Forms.Label num_rand_seed_Label;
        private System.Windows.Forms.TextBox num_rand_seed_Text;
        private System.Windows.Forms.TextBox bsamp_random__modulo_textBox;
        private System.Windows.Forms.CheckBox num_oneFactorAtATime_Box;
        private System.Windows.Forms.Label num_oneFactorAtATime_num_Label;
        private System.Windows.Forms.TextBox num_oneFactorAtATime_num_Text;
        private System.Windows.Forms.Button logFile_Button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton learn_button;
        private System.Windows.Forms.RadioButton print_button;
    }
}

