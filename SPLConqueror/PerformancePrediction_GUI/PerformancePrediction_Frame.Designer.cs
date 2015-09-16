namespace PerformancePrediction_GUI
{
    partial class PerformancePrediction_Frame
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
            this.BinarySamplingGroup = new System.Windows.Forms.GroupBox();
            this.binWholePop = new System.Windows.Forms.CheckBox();
            this.negOW = new System.Windows.Forms.CheckBox();
            this.PW = new System.Windows.Forms.CheckBox();
            this.OW = new System.Windows.Forms.CheckBox();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.LogGroup = new System.Windows.Forms.GroupBox();
            this.StartLearningButton = new System.Windows.Forms.Button();
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
            this.perfInfGridView = new System.Windows.Forms.DataGridView();
            this.pim_group = new System.Windows.Forms.GroupBox();
            this.nfpSelection = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MlSettings_Box = new System.Windows.Forms.GroupBox();
            this.mlSettingsPanel = new System.Windows.Forms.Panel();
            this.readMeasurements = new System.Windows.Forms.Button();
            this.cleanButton = new System.Windows.Forms.Button();
            this.readVarModel = new System.Windows.Forms.Button();
            this.BinarySamplingGroup.SuspendLayout();
            this.LogGroup.SuspendLayout();
            this.expDasign_group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.perfInfGridView)).BeginInit();
            this.pim_group.SuspendLayout();
            this.MlSettings_Box.SuspendLayout();
            this.SuspendLayout();
            // 
            // BinarySamplingGroup
            // 
            this.BinarySamplingGroup.Controls.Add(this.binWholePop);
            this.BinarySamplingGroup.Controls.Add(this.negOW);
            this.BinarySamplingGroup.Controls.Add(this.PW);
            this.BinarySamplingGroup.Controls.Add(this.OW);
            this.BinarySamplingGroup.Location = new System.Drawing.Point(113, 7);
            this.BinarySamplingGroup.Name = "BinarySamplingGroup";
            this.BinarySamplingGroup.Size = new System.Drawing.Size(461, 50);
            this.BinarySamplingGroup.TabIndex = 0;
            this.BinarySamplingGroup.TabStop = false;
            this.BinarySamplingGroup.Text = "Binary Options: Sampling Heuristics ";
            // 
            // binWholePop
            // 
            this.binWholePop.AutoSize = true;
            this.binWholePop.Location = new System.Drawing.Point(309, 19);
            this.binWholePop.Name = "binWholePop";
            this.binWholePop.Size = new System.Drawing.Size(110, 17);
            this.binWholePop.TabIndex = 3;
            this.binWholePop.Text = "Whole Population";
            this.binWholePop.UseVisualStyleBackColor = true;
            // 
            // negOW
            // 
            this.negOW.AutoSize = true;
            this.negOW.Location = new System.Drawing.Point(175, 19);
            this.negOW.Name = "negOW";
            this.negOW.Size = new System.Drawing.Size(128, 17);
            this.negOW.TabIndex = 2;
            this.negOW.Text = "negative Option-Wise";
            this.negOW.UseVisualStyleBackColor = true;
            // 
            // PW
            // 
            this.PW.AutoSize = true;
            this.PW.Location = new System.Drawing.Point(98, 20);
            this.PW.Name = "PW";
            this.PW.Size = new System.Drawing.Size(71, 17);
            this.PW.TabIndex = 1;
            this.PW.Text = "Pair-Wise";
            this.PW.UseVisualStyleBackColor = true;
            // 
            // OW
            // 
            this.OW.AutoSize = true;
            this.OW.Location = new System.Drawing.Point(7, 20);
            this.OW.Name = "OW";
            this.OW.Size = new System.Drawing.Size(84, 17);
            this.OW.TabIndex = 0;
            this.OW.Text = "Option-Wise";
            this.OW.UseVisualStyleBackColor = true;
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(7, 20);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogBox.Size = new System.Drawing.Size(1048, 70);
            this.LogBox.TabIndex = 2;
            // 
            // LogGroup
            // 
            this.LogGroup.Controls.Add(this.LogBox);
            this.LogGroup.Location = new System.Drawing.Point(13, 708);
            this.LogGroup.Name = "LogGroup";
            this.LogGroup.Size = new System.Drawing.Size(1061, 95);
            this.LogGroup.TabIndex = 3;
            this.LogGroup.TabStop = false;
            this.LogGroup.Text = "Log";
            // 
            // StartLearningButton
            // 
            this.StartLearningButton.Location = new System.Drawing.Point(639, 298);
            this.StartLearningButton.Name = "StartLearningButton";
            this.StartLearningButton.Size = new System.Drawing.Size(93, 23);
            this.StartLearningButton.TabIndex = 4;
            this.StartLearningButton.Text = "Start Learning";
            this.StartLearningButton.UseVisualStyleBackColor = true;
            this.StartLearningButton.Click += new System.EventHandler(this.StartLearningButton_Click);
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
            this.expDasign_group.Location = new System.Drawing.Point(113, 63);
            this.expDasign_group.Name = "expDasign_group";
            this.expDasign_group.Size = new System.Drawing.Size(461, 160);
            this.expDasign_group.TabIndex = 8;
            this.expDasign_group.TabStop = false;
            this.expDasign_group.Text = "Numeric Options: Experimental Designs";
            // 
            // num_oneFactorAtATime_num_Text
            // 
            this.num_oneFactorAtATime_num_Text.Location = new System.Drawing.Point(205, 134);
            this.num_oneFactorAtATime_num_Text.Name = "num_oneFactorAtATime_num_Text";
            this.num_oneFactorAtATime_num_Text.Size = new System.Drawing.Size(36, 20);
            this.num_oneFactorAtATime_num_Text.TabIndex = 34;
            this.num_oneFactorAtATime_num_Text.Text = "5";
            this.num_oneFactorAtATime_num_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_oneFactorAtATime_num_Label
            // 
            this.num_oneFactorAtATime_num_Label.AutoSize = true;
            this.num_oneFactorAtATime_num_Label.Location = new System.Drawing.Point(156, 136);
            this.num_oneFactorAtATime_num_Label.Name = "num_oneFactorAtATime_num_Label";
            this.num_oneFactorAtATime_num_Label.Size = new System.Drawing.Size(41, 13);
            this.num_oneFactorAtATime_num_Label.TabIndex = 33;
            this.num_oneFactorAtATime_num_Label.Text = "values:";
            // 
            // num_oneFactorAtATime_Box
            // 
            this.num_oneFactorAtATime_Box.AutoSize = true;
            this.num_oneFactorAtATime_Box.Location = new System.Drawing.Point(7, 136);
            this.num_oneFactorAtATime_Box.Name = "num_oneFactorAtATime_Box";
            this.num_oneFactorAtATime_Box.Size = new System.Drawing.Size(126, 17);
            this.num_oneFactorAtATime_Box.TabIndex = 32;
            this.num_oneFactorAtATime_Box.Text = "One Factor at a Time";
            this.num_oneFactorAtATime_Box.UseVisualStyleBackColor = true;
            // 
            // num_rand_seed_Text
            // 
            this.num_rand_seed_Text.Location = new System.Drawing.Point(283, 92);
            this.num_rand_seed_Text.Name = "num_rand_seed_Text";
            this.num_rand_seed_Text.Size = new System.Drawing.Size(33, 20);
            this.num_rand_seed_Text.TabIndex = 31;
            this.num_rand_seed_Text.Text = "0";
            this.num_rand_seed_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_rand_seed_Label
            // 
            this.num_rand_seed_Label.AutoSize = true;
            this.num_rand_seed_Label.Location = new System.Drawing.Point(251, 92);
            this.num_rand_seed_Label.Name = "num_rand_seed_Label";
            this.num_rand_seed_Label.Size = new System.Drawing.Size(33, 13);
            this.num_rand_seed_Label.TabIndex = 30;
            this.num_rand_seed_Label.Text = "seed:";
            // 
            // num_Plackett_n_Box
            // 
            this.num_Plackett_n_Box.Location = new System.Drawing.Point(283, 66);
            this.num_Plackett_n_Box.Name = "num_Plackett_n_Box";
            this.num_Plackett_n_Box.Size = new System.Drawing.Size(33, 20);
            this.num_Plackett_n_Box.TabIndex = 29;
            this.num_Plackett_n_Box.Text = "9";
            this.num_Plackett_n_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_Plackett_n_Label
            // 
            this.num_Plackett_n_Label.AutoSize = true;
            this.num_Plackett_n_Label.Location = new System.Drawing.Point(251, 68);
            this.num_Plackett_n_Label.Name = "num_Plackett_n_Label";
            this.num_Plackett_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_Plackett_n_Label.TabIndex = 28;
            this.num_Plackett_n_Label.Text = "n:";
            // 
            // num_Plackett_Level_Box
            // 
            this.num_Plackett_Level_Box.Location = new System.Drawing.Point(205, 66);
            this.num_Plackett_Level_Box.Name = "num_Plackett_Level_Box";
            this.num_Plackett_Level_Box.Size = new System.Drawing.Size(36, 20);
            this.num_Plackett_Level_Box.TabIndex = 27;
            this.num_Plackett_Level_Box.Text = "3";
            this.num_Plackett_Level_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_Plackett_Level_Label
            // 
            this.num_Plackett_Level_Label.AutoSize = true;
            this.num_Plackett_Level_Label.Location = new System.Drawing.Point(156, 68);
            this.num_Plackett_Level_Label.Name = "num_Plackett_Level_Label";
            this.num_Plackett_Level_Label.Size = new System.Drawing.Size(32, 13);
            this.num_Plackett_Level_Label.TabIndex = 26;
            this.num_Plackett_Level_Label.Text = "level:";
            // 
            // num_hyper_percent_text
            // 
            this.num_hyper_percent_text.Location = new System.Drawing.Point(205, 110);
            this.num_hyper_percent_text.Name = "num_hyper_percent_text";
            this.num_hyper_percent_text.Size = new System.Drawing.Size(36, 20);
            this.num_hyper_percent_text.TabIndex = 25;
            this.num_hyper_percent_text.Text = "10";
            this.num_hyper_percent_text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_hyper_percent_Label
            // 
            this.num_hyper_percent_Label.AutoSize = true;
            this.num_hyper_percent_Label.Location = new System.Drawing.Point(156, 113);
            this.num_hyper_percent_Label.Name = "num_hyper_percent_Label";
            this.num_hyper_percent_Label.Size = new System.Drawing.Size(46, 13);
            this.num_hyper_percent_Label.TabIndex = 24;
            this.num_hyper_percent_Label.Text = "percent:";
            // 
            // num_random_n_Text
            // 
            this.num_random_n_Text.Location = new System.Drawing.Point(205, 89);
            this.num_random_n_Text.Name = "num_random_n_Text";
            this.num_random_n_Text.Size = new System.Drawing.Size(36, 20);
            this.num_random_n_Text.TabIndex = 23;
            this.num_random_n_Text.Text = "100";
            this.num_random_n_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_rand_n_Label
            // 
            this.num_rand_n_Label.AutoSize = true;
            this.num_rand_n_Label.Location = new System.Drawing.Point(156, 92);
            this.num_rand_n_Label.Name = "num_rand_n_Label";
            this.num_rand_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_rand_n_Label.TabIndex = 22;
            this.num_rand_n_Label.Text = "n:";
            // 
            // num_kEx_k_Box
            // 
            this.num_kEx_k_Box.Location = new System.Drawing.Point(283, 41);
            this.num_kEx_k_Box.Name = "num_kEx_k_Box";
            this.num_kEx_k_Box.Size = new System.Drawing.Size(33, 20);
            this.num_kEx_k_Box.TabIndex = 21;
            this.num_kEx_k_Box.Text = "5";
            this.num_kEx_k_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_kEx_k_Label
            // 
            this.num_kEx_k_Label.AutoSize = true;
            this.num_kEx_k_Label.Location = new System.Drawing.Point(251, 44);
            this.num_kEx_k_Label.Name = "num_kEx_k_Label";
            this.num_kEx_k_Label.Size = new System.Drawing.Size(16, 13);
            this.num_kEx_k_Label.TabIndex = 20;
            this.num_kEx_k_Label.Text = "k:";
            // 
            // num_kEx_n_Box
            // 
            this.num_kEx_n_Box.Location = new System.Drawing.Point(205, 41);
            this.num_kEx_n_Box.Name = "num_kEx_n_Box";
            this.num_kEx_n_Box.Size = new System.Drawing.Size(36, 20);
            this.num_kEx_n_Box.TabIndex = 19;
            this.num_kEx_n_Box.Text = "50";
            this.num_kEx_n_Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // num_kEx_n_Label
            // 
            this.num_kEx_n_Label.AutoSize = true;
            this.num_kEx_n_Label.Location = new System.Drawing.Point(156, 43);
            this.num_kEx_n_Label.Name = "num_kEx_n_Label";
            this.num_kEx_n_Label.Size = new System.Drawing.Size(16, 13);
            this.num_kEx_n_Label.TabIndex = 18;
            this.num_kEx_n_Label.Text = "n:";
            // 
            // num_hyperSampling_check
            // 
            this.num_hyperSampling_check.AutoSize = true;
            this.num_hyperSampling_check.Location = new System.Drawing.Point(7, 113);
            this.num_hyperSampling_check.Name = "num_hyperSampling_check";
            this.num_hyperSampling_check.Size = new System.Drawing.Size(98, 17);
            this.num_hyperSampling_check.TabIndex = 15;
            this.num_hyperSampling_check.Text = "Hyper sampling";
            this.num_hyperSampling_check.UseVisualStyleBackColor = true;
            // 
            // num_randomSampling_num
            // 
            this.num_randomSampling_num.AutoSize = true;
            this.num_randomSampling_num.Location = new System.Drawing.Point(7, 90);
            this.num_randomSampling_num.Name = "num_randomSampling_num";
            this.num_randomSampling_num.Size = new System.Drawing.Size(110, 17);
            this.num_randomSampling_num.TabIndex = 14;
            this.num_randomSampling_num.Text = "Random sampling";
            this.num_randomSampling_num.UseVisualStyleBackColor = true;
            // 
            // num_PlackettBurman_check
            // 
            this.num_PlackettBurman_check.AutoSize = true;
            this.num_PlackettBurman_check.Location = new System.Drawing.Point(7, 66);
            this.num_PlackettBurman_check.Name = "num_PlackettBurman_check";
            this.num_PlackettBurman_check.Size = new System.Drawing.Size(104, 17);
            this.num_PlackettBurman_check.TabIndex = 13;
            this.num_PlackettBurman_check.Text = "Plackett-Burman";
            this.num_PlackettBurman_check.UseVisualStyleBackColor = true;
            // 
            // num_kEx_check
            // 
            this.num_kEx_check.AutoSize = true;
            this.num_kEx_check.Location = new System.Drawing.Point(7, 43);
            this.num_kEx_check.Name = "num_kEx_check";
            this.num_kEx_check.Size = new System.Drawing.Size(138, 17);
            this.num_kEx_check.TabIndex = 12;
            this.num_kEx_check.Text = "D-Optimal (k-Exchange)";
            this.num_kEx_check.UseVisualStyleBackColor = true;
            // 
            // num_FullFactorial_check
            // 
            this.num_FullFactorial_check.AutoSize = true;
            this.num_FullFactorial_check.Location = new System.Drawing.Point(300, 18);
            this.num_FullFactorial_check.Name = "num_FullFactorial_check";
            this.num_FullFactorial_check.Size = new System.Drawing.Size(85, 17);
            this.num_FullFactorial_check.TabIndex = 11;
            this.num_FullFactorial_check.Text = "Full-Factorial";
            this.num_FullFactorial_check.UseVisualStyleBackColor = true;
            // 
            // num_CentralComposite_check
            // 
            this.num_CentralComposite_check.AutoSize = true;
            this.num_CentralComposite_check.Location = new System.Drawing.Point(153, 18);
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
            // perfInfGridView
            // 
            this.perfInfGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.perfInfGridView.Location = new System.Drawing.Point(6, 20);
            this.perfInfGridView.Name = "perfInfGridView";
            this.perfInfGridView.Size = new System.Drawing.Size(1049, 356);
            this.perfInfGridView.TabIndex = 9;
            // 
            // pim_group
            // 
            this.pim_group.Controls.Add(this.perfInfGridView);
            this.pim_group.Location = new System.Drawing.Point(13, 321);
            this.pim_group.Name = "pim_group";
            this.pim_group.Size = new System.Drawing.Size(1061, 381);
            this.pim_group.TabIndex = 4;
            this.pim_group.TabStop = false;
            this.pim_group.Text = "Performance-Influence Model";
            // 
            // nfpSelection
            // 
            this.nfpSelection.FormattingEnabled = true;
            this.nfpSelection.Location = new System.Drawing.Point(13, 126);
            this.nfpSelection.Name = "nfpSelection";
            this.nfpSelection.Size = new System.Drawing.Size(95, 49);
            this.nfpSelection.TabIndex = 9;
            this.nfpSelection.SelectedIndexChanged += new System.EventHandler(this.nfpSelection_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "NFP:";
            // 
            // MlSettings_Box
            // 
            this.MlSettings_Box.Controls.Add(this.mlSettingsPanel);
            this.MlSettings_Box.Location = new System.Drawing.Point(738, 7);
            this.MlSettings_Box.Name = "MlSettings_Box";
            this.MlSettings_Box.Size = new System.Drawing.Size(336, 314);
            this.MlSettings_Box.TabIndex = 11;
            this.MlSettings_Box.TabStop = false;
            this.MlSettings_Box.Text = "Machine-Learning Settings";
            // 
            // mlSettingsPanel
            // 
            this.mlSettingsPanel.Location = new System.Drawing.Point(7, 19);
            this.mlSettingsPanel.Name = "mlSettingsPanel";
            this.mlSettingsPanel.Size = new System.Drawing.Size(323, 289);
            this.mlSettingsPanel.TabIndex = 0;
            // 
            // readMeasurements
            // 
            this.readMeasurements.Location = new System.Drawing.Point(13, 65);
            this.readMeasurements.Name = "readMeasurements";
            this.readMeasurements.Size = new System.Drawing.Size(95, 36);
            this.readMeasurements.TabIndex = 12;
            this.readMeasurements.Text = "Read Measurements";
            this.readMeasurements.UseVisualStyleBackColor = true;
            this.readMeasurements.Click += new System.EventHandler(this.readMeasurements_Click);
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(12, 181);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(75, 36);
            this.cleanButton.TabIndex = 13;
            this.cleanButton.Text = "clean Sampling";
            this.cleanButton.UseVisualStyleBackColor = true;
            this.cleanButton.Click += new System.EventHandler(this.cleanButton_Click);
            // 
            // readVarModel
            // 
            this.readVarModel.Location = new System.Drawing.Point(13, 7);
            this.readVarModel.Name = "readVarModel";
            this.readVarModel.Size = new System.Drawing.Size(95, 36);
            this.readVarModel.TabIndex = 14;
            this.readVarModel.Text = "Read Variability Model";
            this.readVarModel.UseVisualStyleBackColor = true;
            this.readVarModel.Click += new System.EventHandler(this.readVarModel_Click);
            // 
            // PerformancePrediction_Frame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 815);
            this.Controls.Add(this.readVarModel);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.readMeasurements);
            this.Controls.Add(this.MlSettings_Box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nfpSelection);
            this.Controls.Add(this.pim_group);
            this.Controls.Add(this.expDasign_group);
            this.Controls.Add(this.StartLearningButton);
            this.Controls.Add(this.LogGroup);
            this.Controls.Add(this.BinarySamplingGroup);
            this.Name = "PerformancePrediction_Frame";
            this.Text = "SPL Conqueror: Performance Prediction";
            this.BinarySamplingGroup.ResumeLayout(false);
            this.BinarySamplingGroup.PerformLayout();
            this.LogGroup.ResumeLayout(false);
            this.LogGroup.PerformLayout();
            this.expDasign_group.ResumeLayout(false);
            this.expDasign_group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.perfInfGridView)).EndInit();
            this.pim_group.ResumeLayout(false);
            this.MlSettings_Box.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox BinarySamplingGroup;
        private System.Windows.Forms.CheckBox negOW;
        private System.Windows.Forms.CheckBox PW;
        private System.Windows.Forms.CheckBox OW;
        private System.Windows.Forms.GroupBox LogGroup;
        private System.Windows.Forms.Button StartLearningButton;
        private System.Windows.Forms.GroupBox expDasign_group;
        private System.Windows.Forms.TextBox num_oneFactorAtATime_num_Text;
        private System.Windows.Forms.Label num_oneFactorAtATime_num_Label;
        private System.Windows.Forms.CheckBox num_oneFactorAtATime_Box;
        private System.Windows.Forms.TextBox num_rand_seed_Text;
        private System.Windows.Forms.Label num_rand_seed_Label;
        private System.Windows.Forms.TextBox num_Plackett_n_Box;
        private System.Windows.Forms.Label num_Plackett_n_Label;
        private System.Windows.Forms.TextBox num_Plackett_Level_Box;
        private System.Windows.Forms.Label num_Plackett_Level_Label;
        private System.Windows.Forms.TextBox num_hyper_percent_text;
        private System.Windows.Forms.Label num_hyper_percent_Label;
        private System.Windows.Forms.TextBox num_random_n_Text;
        private System.Windows.Forms.Label num_rand_n_Label;
        private System.Windows.Forms.TextBox num_kEx_k_Box;
        private System.Windows.Forms.Label num_kEx_k_Label;
        private System.Windows.Forms.TextBox num_kEx_n_Box;
        private System.Windows.Forms.Label num_kEx_n_Label;
        private System.Windows.Forms.CheckBox num_hyperSampling_check;
        private System.Windows.Forms.CheckBox num_randomSampling_num;
        private System.Windows.Forms.CheckBox num_PlackettBurman_check;
        private System.Windows.Forms.CheckBox num_kEx_check;
        private System.Windows.Forms.CheckBox num_FullFactorial_check;
        private System.Windows.Forms.CheckBox num_CentralComposite_check;
        private System.Windows.Forms.CheckBox num_BoxBehnken_check;
        private System.Windows.Forms.DataGridView perfInfGridView;
        private System.Windows.Forms.GroupBox pim_group;
        private System.Windows.Forms.CheckedListBox nfpSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox MlSettings_Box;
        private System.Windows.Forms.Panel mlSettingsPanel;
        private System.Windows.Forms.Button readMeasurements;
        private System.Windows.Forms.Button cleanButton;
        public System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Button readVarModel;
        private System.Windows.Forms.CheckBox binWholePop;
    }
}

