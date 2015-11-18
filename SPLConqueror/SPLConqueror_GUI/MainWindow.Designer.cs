namespace SPLConqueror_GUI
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.evaluationGroupBox = new System.Windows.Forms.GroupBox();
            this.evaluationFunctionPanel = new System.Windows.Forms.Panel();
            this.defaultNumericOptionLabel = new System.Windows.Forms.Label();
            this.numericDefaultPanel = new System.Windows.Forms.Panel();
            this.failureLabel = new System.Windows.Forms.Label();
            this.generateFunctionButton = new System.Windows.Forms.Button();
            this.secondAxisCombobox = new System.Windows.Forms.ComboBox();
            this.secondAxisLabel = new System.Windows.Forms.Label();
            this.firstAxisLabel = new System.Windows.Forms.Label();
            this.firstAxisCombobox = new System.Windows.Forms.ComboBox();
            this.noNumericOptionPanel = new System.Windows.Forms.Panel();
            this.calculationResultLabel = new System.Windows.Forms.Label();
            this.calculatePerformanceButton = new System.Windows.Forms.Button();
            this.calculatedPerformanceLabel = new System.Windows.Forms.Label();
            this.noNumericOptionsLabel = new System.Windows.Forms.Label();
            this.variableConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.regexTextbox = new System.Windows.Forms.TextBox();
            this.filterRegexCheckBox = new System.Windows.Forms.CheckBox();
            this.filterOptionCombobox = new System.Windows.Forms.ComboBox();
            this.variableTreeView = new System.Windows.Forms.TreeView();
            this.variableListBox = new System.Windows.Forms.CheckedListBox();
            this.filterVariablesCheckbox = new System.Windows.Forms.CheckBox();
            this.constantConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            this.constantRelativeValueSlider = new System.Windows.Forms.TrackBar();
            this.constantDecimalCheckBox = new System.Windows.Forms.CheckBox();
            this.constantsDigitsUpDown = new System.Windows.Forms.NumericUpDown();
            this.constantFilteringCheckbox = new System.Windows.Forms.CheckBox();
            this.functionGroupBox = new System.Windows.Forms.GroupBox();
            this.loadExpOnlyButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.originalFunctionTextBox = new System.Windows.Forms.RichTextBox();
            this.originalFunctionLabel = new System.Windows.Forms.Label();
            this.factorRadioButton = new System.Windows.Forms.RadioButton();
            this.normalRadioButton = new System.Windows.Forms.RadioButton();
            this.adjustedTextBox = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.functionGraphTabPage = new System.Windows.Forms.TabPage();
            this.pointPositionLabel = new System.Windows.Forms.Label();
            this.ilFunctionPanel = new ILNumerics.Drawing.ILPanel();
            this.interactionsInfluencesTabPage = new System.Windows.Forms.TabPage();
            this.rangeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.interactionsLabel = new System.Windows.Forms.Label();
            this.chartDescriptionLabel = new System.Windows.Forms.Label();
            this.maxOccuranceChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.maxChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pieOptionLabel = new System.Windows.Forms.Label();
            this.constantChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartComboBox = new System.Windows.Forms.ComboBox();
            this.interactionChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.interactionTextBox = new System.Windows.Forms.RichTextBox();
            this.measurementsTabPage = new System.Windows.Forms.TabPage();
            this.overviewPanel = new System.Windows.Forms.Panel();
            this.overviewRelativeDifferencePanel = new System.Windows.Forms.Panel();
            this.overviewRelativeDifferenceIlPanel = new ILNumerics.Drawing.ILPanel();
            this.overviewAbsoluteDifferencePanel = new System.Windows.Forms.Panel();
            this.overviewAbsoluteDifferenceIlPanel = new ILNumerics.Drawing.ILPanel();
            this.overviewMeasurementPanel = new System.Windows.Forms.Panel();
            this.overviewMeasurementIlPanel = new ILNumerics.Drawing.ILPanel();
            this.overviewPerformancePanel = new System.Windows.Forms.Panel();
            this.overviewPerformanceIlPanel = new ILNumerics.Drawing.ILPanel();
            this.relativeDifferenceLabel = new System.Windows.Forms.Label();
            this.absoluteDifferenceLabel = new System.Windows.Forms.Label();
            this.measurementsLabel = new System.Windows.Forms.Label();
            this.calculatedPerformancesLabel = new System.Windows.Forms.Label();
            this.relativeDifferencePanel = new System.Windows.Forms.Panel();
            this.relativeDifferenceIlPanel = new ILNumerics.Drawing.ILPanel();
            this.measurementPointLabel = new System.Windows.Forms.Label();
            this.absoluteDifferencePanel = new System.Windows.Forms.Panel();
            this.absoluteDifferenceIlPanel = new ILNumerics.Drawing.ILPanel();
            this.measurementsOnlyPanel = new System.Windows.Forms.Panel();
            this.measurementsOnlyIlPanel = new ILNumerics.Drawing.ILPanel();
            this.measurementViewCombobox = new System.Windows.Forms.ComboBox();
            this.measurementErrorLabel = new System.Windows.Forms.Label();
            this.bothGraphsPanel = new System.Windows.Forms.Panel();
            this.bothGraphsIlPanel = new ILNumerics.Drawing.ILPanel();
            this.nfpValueCombobox = new System.Windows.Forms.ComboBox();
            this.loadMeasurementButton = new System.Windows.Forms.Button();
            this.helpTabPage = new System.Windows.Forms.TabPage();
            this.helpTextBox = new System.Windows.Forms.RichTextBox();
            this.constraintTextbox = new System.Windows.Forms.RichTextBox();
            this.factorizationSettingsButton = new System.Windows.Forms.Button();
            this.adjustedFunctionGroupBox = new System.Windows.Forms.GroupBox();
            this.resetFactorizationButton = new System.Windows.Forms.Button();
            this.constraintsGroupBox = new System.Windows.Forms.GroupBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.evaluationGroupBox.SuspendLayout();
            this.evaluationFunctionPanel.SuspendLayout();
            this.noNumericOptionPanel.SuspendLayout();
            this.variableConfigurationGroupBox.SuspendLayout();
            this.constantConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.constantRelativeValueSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantsDigitsUpDown)).BeginInit();
            this.functionGroupBox.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.functionGraphTabPage.SuspendLayout();
            this.interactionsInfluencesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxOccuranceChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.interactionChart)).BeginInit();
            this.measurementsTabPage.SuspendLayout();
            this.overviewPanel.SuspendLayout();
            this.overviewRelativeDifferencePanel.SuspendLayout();
            this.overviewAbsoluteDifferencePanel.SuspendLayout();
            this.overviewMeasurementPanel.SuspendLayout();
            this.overviewPerformancePanel.SuspendLayout();
            this.relativeDifferencePanel.SuspendLayout();
            this.absoluteDifferencePanel.SuspendLayout();
            this.measurementsOnlyPanel.SuspendLayout();
            this.bothGraphsPanel.SuspendLayout();
            this.helpTabPage.SuspendLayout();
            this.adjustedFunctionGroupBox.SuspendLayout();
            this.constraintsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // evaluationGroupBox
            // 
            this.evaluationGroupBox.Controls.Add(this.evaluationFunctionPanel);
            this.evaluationGroupBox.Controls.Add(this.noNumericOptionPanel);
            this.evaluationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.evaluationGroupBox.Location = new System.Drawing.Point(1005, 469);
            this.evaluationGroupBox.Name = "evaluationGroupBox";
            this.evaluationGroupBox.Size = new System.Drawing.Size(316, 274);
            this.evaluationGroupBox.TabIndex = 17;
            this.evaluationGroupBox.TabStop = false;
            this.evaluationGroupBox.Text = "Evaluation configuration";
            // 
            // evaluationFunctionPanel
            // 
            this.evaluationFunctionPanel.Controls.Add(this.defaultNumericOptionLabel);
            this.evaluationFunctionPanel.Controls.Add(this.numericDefaultPanel);
            this.evaluationFunctionPanel.Controls.Add(this.failureLabel);
            this.evaluationFunctionPanel.Controls.Add(this.generateFunctionButton);
            this.evaluationFunctionPanel.Controls.Add(this.secondAxisCombobox);
            this.evaluationFunctionPanel.Controls.Add(this.secondAxisLabel);
            this.evaluationFunctionPanel.Controls.Add(this.firstAxisLabel);
            this.evaluationFunctionPanel.Controls.Add(this.firstAxisCombobox);
            this.evaluationFunctionPanel.Location = new System.Drawing.Point(6, 18);
            this.evaluationFunctionPanel.Name = "evaluationFunctionPanel";
            this.evaluationFunctionPanel.Size = new System.Drawing.Size(301, 250);
            this.evaluationFunctionPanel.TabIndex = 1;
            // 
            // defaultNumericOptionLabel
            // 
            this.defaultNumericOptionLabel.AutoSize = true;
            this.defaultNumericOptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultNumericOptionLabel.Location = new System.Drawing.Point(3, 81);
            this.defaultNumericOptionLabel.Name = "defaultNumericOptionLabel";
            this.defaultNumericOptionLabel.Size = new System.Drawing.Size(205, 13);
            this.defaultNumericOptionLabel.TabIndex = 16;
            this.defaultNumericOptionLabel.Text = "Default values for numeric options:";
            // 
            // numericDefaultPanel
            // 
            this.numericDefaultPanel.AutoScroll = true;
            this.numericDefaultPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericDefaultPanel.Location = new System.Drawing.Point(0, 96);
            this.numericDefaultPanel.Name = "numericDefaultPanel";
            this.numericDefaultPanel.Size = new System.Drawing.Size(301, 154);
            this.numericDefaultPanel.TabIndex = 15;
            // 
            // failureLabel
            // 
            this.failureLabel.AutoSize = true;
            this.failureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failureLabel.ForeColor = System.Drawing.Color.Crimson;
            this.failureLabel.Location = new System.Drawing.Point(3, 60);
            this.failureLabel.Name = "failureLabel";
            this.failureLabel.Size = new System.Drawing.Size(35, 13);
            this.failureLabel.TabIndex = 14;
            this.failureLabel.Text = "label5";
            this.failureLabel.Visible = false;
            // 
            // generateFunctionButton
            // 
            this.generateFunctionButton.Enabled = false;
            this.generateFunctionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateFunctionButton.Location = new System.Drawing.Point(241, 0);
            this.generateFunctionButton.Name = "generateFunctionButton";
            this.generateFunctionButton.Size = new System.Drawing.Size(60, 54);
            this.generateFunctionButton.TabIndex = 13;
            this.generateFunctionButton.Text = "Generate       Function";
            this.toolTip.SetToolTip(this.generateFunctionButton, "Generates the function graph");
            this.generateFunctionButton.UseVisualStyleBackColor = true;
            this.generateFunctionButton.Click += new System.EventHandler(this.generateFunctionButton_Click);
            // 
            // secondAxisCombobox
            // 
            this.secondAxisCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.secondAxisCombobox.Enabled = false;
            this.secondAxisCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondAxisCombobox.FormattingEnabled = true;
            this.secondAxisCombobox.Location = new System.Drawing.Point(76, 32);
            this.secondAxisCombobox.Name = "secondAxisCombobox";
            this.secondAxisCombobox.Size = new System.Drawing.Size(159, 21);
            this.secondAxisCombobox.Sorted = true;
            this.secondAxisCombobox.TabIndex = 12;
            this.secondAxisCombobox.SelectedIndexChanged += new System.EventHandler(this.secondAxisCombobox_SelectedIndexChanged);
            // 
            // secondAxisLabel
            // 
            this.secondAxisLabel.AutoSize = true;
            this.secondAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondAxisLabel.Location = new System.Drawing.Point(3, 35);
            this.secondAxisLabel.Name = "secondAxisLabel";
            this.secondAxisLabel.Size = new System.Drawing.Size(68, 13);
            this.secondAxisLabel.TabIndex = 11;
            this.secondAxisLabel.Text = "Second axis:";
            // 
            // firstAxisLabel
            // 
            this.firstAxisLabel.AutoSize = true;
            this.firstAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstAxisLabel.Location = new System.Drawing.Point(3, 4);
            this.firstAxisLabel.Name = "firstAxisLabel";
            this.firstAxisLabel.Size = new System.Drawing.Size(50, 13);
            this.firstAxisLabel.TabIndex = 10;
            this.firstAxisLabel.Text = "First axis:";
            // 
            // firstAxisCombobox
            // 
            this.firstAxisCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firstAxisCombobox.Enabled = false;
            this.firstAxisCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstAxisCombobox.Location = new System.Drawing.Point(76, 1);
            this.firstAxisCombobox.Name = "firstAxisCombobox";
            this.firstAxisCombobox.Size = new System.Drawing.Size(159, 21);
            this.firstAxisCombobox.Sorted = true;
            this.firstAxisCombobox.TabIndex = 9;
            this.firstAxisCombobox.SelectedIndexChanged += new System.EventHandler(this.firstAxisCombobox_SelectedIndexChanged);
            // 
            // noNumericOptionPanel
            // 
            this.noNumericOptionPanel.Controls.Add(this.calculationResultLabel);
            this.noNumericOptionPanel.Controls.Add(this.calculatePerformanceButton);
            this.noNumericOptionPanel.Controls.Add(this.calculatedPerformanceLabel);
            this.noNumericOptionPanel.Controls.Add(this.noNumericOptionsLabel);
            this.noNumericOptionPanel.Location = new System.Drawing.Point(6, 18);
            this.noNumericOptionPanel.Name = "noNumericOptionPanel";
            this.noNumericOptionPanel.Size = new System.Drawing.Size(301, 104);
            this.noNumericOptionPanel.TabIndex = 0;
            // 
            // calculationResultLabel
            // 
            this.calculationResultLabel.AutoSize = true;
            this.calculationResultLabel.Location = new System.Drawing.Point(-3, 68);
            this.calculationResultLabel.Name = "calculationResultLabel";
            this.calculationResultLabel.Size = new System.Drawing.Size(145, 13);
            this.calculationResultLabel.TabIndex = 3;
            this.calculationResultLabel.Text = "Please press the button.";
            // 
            // calculatePerformanceButton
            // 
            this.calculatePerformanceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatePerformanceButton.Location = new System.Drawing.Point(218, 42);
            this.calculatePerformanceButton.Name = "calculatePerformanceButton";
            this.calculatePerformanceButton.Size = new System.Drawing.Size(80, 23);
            this.calculatePerformanceButton.TabIndex = 2;
            this.calculatePerformanceButton.Text = "Calculate";
            this.calculatePerformanceButton.UseVisualStyleBackColor = true;
            this.calculatePerformanceButton.Click += new System.EventHandler(this.calculatePerformanceButton_Click);
            // 
            // calculatedPerformanceLabel
            // 
            this.calculatedPerformanceLabel.AutoSize = true;
            this.calculatedPerformanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatedPerformanceLabel.Location = new System.Drawing.Point(-3, 47);
            this.calculatedPerformanceLabel.Name = "calculatedPerformanceLabel";
            this.calculatedPerformanceLabel.Size = new System.Drawing.Size(122, 13);
            this.calculatedPerformanceLabel.TabIndex = 1;
            this.calculatedPerformanceLabel.Text = "Calculated performance:";
            // 
            // noNumericOptionsLabel
            // 
            this.noNumericOptionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noNumericOptionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noNumericOptionsLabel.ForeColor = System.Drawing.Color.Red;
            this.noNumericOptionsLabel.Location = new System.Drawing.Point(0, 0);
            this.noNumericOptionsLabel.Name = "noNumericOptionsLabel";
            this.noNumericOptionsLabel.Size = new System.Drawing.Size(301, 104);
            this.noNumericOptionsLabel.TabIndex = 0;
            this.noNumericOptionsLabel.Text = "Currently, there are no numeric options to choose. Now you can calculate the valu" +
    "e with your current settings.";
            // 
            // variableConfigurationGroupBox
            // 
            this.variableConfigurationGroupBox.Controls.Add(this.regexTextbox);
            this.variableConfigurationGroupBox.Controls.Add(this.filterRegexCheckBox);
            this.variableConfigurationGroupBox.Controls.Add(this.filterOptionCombobox);
            this.variableConfigurationGroupBox.Controls.Add(this.variableTreeView);
            this.variableConfigurationGroupBox.Controls.Add(this.variableListBox);
            this.variableConfigurationGroupBox.Controls.Add(this.filterVariablesCheckbox);
            this.variableConfigurationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableConfigurationGroupBox.Location = new System.Drawing.Point(10, 415);
            this.variableConfigurationGroupBox.Name = "variableConfigurationGroupBox";
            this.variableConfigurationGroupBox.Size = new System.Drawing.Size(316, 203);
            this.variableConfigurationGroupBox.TabIndex = 16;
            this.variableConfigurationGroupBox.TabStop = false;
            this.variableConfigurationGroupBox.Text = "Variable configuration";
            // 
            // regexTextbox
            // 
            this.regexTextbox.Enabled = false;
            this.regexTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regexTextbox.Location = new System.Drawing.Point(6, 177);
            this.regexTextbox.Name = "regexTextbox";
            this.regexTextbox.Size = new System.Drawing.Size(301, 20);
            this.regexTextbox.TabIndex = 5;
            this.regexTextbox.TextChanged += new System.EventHandler(this.regexTextbox_TextChanged);
            // 
            // filterRegexCheckBox
            // 
            this.filterRegexCheckBox.AutoSize = true;
            this.filterRegexCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.filterRegexCheckBox.Enabled = false;
            this.filterRegexCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterRegexCheckBox.Location = new System.Drawing.Point(9, 157);
            this.filterRegexCheckBox.Name = "filterRegexCheckBox";
            this.filterRegexCheckBox.Size = new System.Drawing.Size(172, 17);
            this.filterRegexCheckBox.TabIndex = 4;
            this.filterRegexCheckBox.Text = "Search for variables containing";
            this.filterRegexCheckBox.UseVisualStyleBackColor = false;
            this.filterRegexCheckBox.CheckedChanged += new System.EventHandler(this.filterRegexCheckBox_CheckedChanged);
            // 
            // filterOptionCombobox
            // 
            this.filterOptionCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterOptionCombobox.Enabled = false;
            this.filterOptionCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterOptionCombobox.FormattingEnabled = true;
            this.filterOptionCombobox.Location = new System.Drawing.Point(170, 15);
            this.filterOptionCombobox.Name = "filterOptionCombobox";
            this.filterOptionCombobox.Size = new System.Drawing.Size(137, 21);
            this.filterOptionCombobox.TabIndex = 3;
            this.filterOptionCombobox.SelectedIndexChanged += new System.EventHandler(this.filterOptionCombobox_SelectedIndexChanged);
            // 
            // variableTreeView
            // 
            this.variableTreeView.CheckBoxes = true;
            this.variableTreeView.Enabled = false;
            this.variableTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableTreeView.Location = new System.Drawing.Point(6, 42);
            this.variableTreeView.Name = "variableTreeView";
            this.variableTreeView.Size = new System.Drawing.Size(301, 109);
            this.variableTreeView.TabIndex = 2;
            this.variableTreeView.Visible = false;
            // 
            // variableListBox
            // 
            this.variableListBox.CheckOnClick = true;
            this.variableListBox.Enabled = false;
            this.variableListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableListBox.FormattingEnabled = true;
            this.variableListBox.Location = new System.Drawing.Point(6, 42);
            this.variableListBox.Name = "variableListBox";
            this.variableListBox.Size = new System.Drawing.Size(301, 109);
            this.variableListBox.Sorted = true;
            this.variableListBox.TabIndex = 1;
            this.variableListBox.SelectedIndexChanged += new System.EventHandler(this.variableListBox_SelectedIndexChanged);
            // 
            // filterVariablesCheckbox
            // 
            this.filterVariablesCheckbox.AutoSize = true;
            this.filterVariablesCheckbox.Enabled = false;
            this.filterVariablesCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterVariablesCheckbox.Location = new System.Drawing.Point(9, 19);
            this.filterVariablesCheckbox.Name = "filterVariablesCheckbox";
            this.filterVariablesCheckbox.Size = new System.Drawing.Size(93, 17);
            this.filterVariablesCheckbox.TabIndex = 0;
            this.filterVariablesCheckbox.Text = "Filter variables";
            this.filterVariablesCheckbox.UseVisualStyleBackColor = true;
            this.filterVariablesCheckbox.CheckedChanged += new System.EventHandler(this.filterVariablesCheckbox_CheckedChanged);
            // 
            // constantConfigurationGroupBox
            // 
            this.constantConfigurationGroupBox.Controls.Add(this.constantRelativeValueSlider);
            this.constantConfigurationGroupBox.Controls.Add(this.constantDecimalCheckBox);
            this.constantConfigurationGroupBox.Controls.Add(this.constantsDigitsUpDown);
            this.constantConfigurationGroupBox.Controls.Add(this.constantFilteringCheckbox);
            this.constantConfigurationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantConfigurationGroupBox.Location = new System.Drawing.Point(12, 318);
            this.constantConfigurationGroupBox.Name = "constantConfigurationGroupBox";
            this.constantConfigurationGroupBox.Size = new System.Drawing.Size(316, 91);
            this.constantConfigurationGroupBox.TabIndex = 15;
            this.constantConfigurationGroupBox.TabStop = false;
            this.constantConfigurationGroupBox.Text = "Constant configuration";
            // 
            // constantRelativeValueSlider
            // 
            this.constantRelativeValueSlider.Enabled = false;
            this.constantRelativeValueSlider.Location = new System.Drawing.Point(170, 44);
            this.constantRelativeValueSlider.Maximum = 100;
            this.constantRelativeValueSlider.Name = "constantRelativeValueSlider";
            this.constantRelativeValueSlider.Size = new System.Drawing.Size(137, 45);
            this.constantRelativeValueSlider.TabIndex = 5;
            this.constantRelativeValueSlider.Scroll += new System.EventHandler(this.constantRelativeValueSlider_Scroll);
            // 
            // constantDecimalCheckBox
            // 
            this.constantDecimalCheckBox.AutoSize = true;
            this.constantDecimalCheckBox.Enabled = false;
            this.constantDecimalCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantDecimalCheckBox.Location = new System.Drawing.Point(9, 19);
            this.constantDecimalCheckBox.Name = "constantDecimalCheckBox";
            this.constantDecimalCheckBox.Size = new System.Drawing.Size(163, 17);
            this.constantDecimalCheckBox.TabIndex = 11;
            this.constantDecimalCheckBox.Text = "Fractional digits of constants:";
            this.constantDecimalCheckBox.UseVisualStyleBackColor = true;
            this.constantDecimalCheckBox.CheckedChanged += new System.EventHandler(this.constantDecimalCheckBox_CheckedChanged);
            // 
            // constantsDigitsUpDown
            // 
            this.constantsDigitsUpDown.Enabled = false;
            this.constantsDigitsUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantsDigitsUpDown.Location = new System.Drawing.Point(181, 18);
            this.constantsDigitsUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.constantsDigitsUpDown.Name = "constantsDigitsUpDown";
            this.constantsDigitsUpDown.Size = new System.Drawing.Size(37, 20);
            this.constantsDigitsUpDown.TabIndex = 8;
            this.constantsDigitsUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.constantsDigitsUpDown.ValueChanged += new System.EventHandler(this.constantsDigitsUpDown_ValueChanged);
            // 
            // constantFilteringCheckbox
            // 
            this.constantFilteringCheckbox.AutoSize = true;
            this.constantFilteringCheckbox.Enabled = false;
            this.constantFilteringCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantFilteringCheckbox.Location = new System.Drawing.Point(9, 46);
            this.constantFilteringCheckbox.Name = "constantFilteringCheckbox";
            this.constantFilteringCheckbox.Size = new System.Drawing.Size(155, 17);
            this.constantFilteringCheckbox.TabIndex = 9;
            this.constantFilteringCheckbox.Text = "Relative value of constants";
            this.constantFilteringCheckbox.UseVisualStyleBackColor = true;
            this.constantFilteringCheckbox.CheckedChanged += new System.EventHandler(this.constantFilteringCheckbox_CheckedChanged);
            // 
            // functionGroupBox
            // 
            this.functionGroupBox.Controls.Add(this.loadExpOnlyButton);
            this.functionGroupBox.Controls.Add(this.loadButton);
            this.functionGroupBox.Controls.Add(this.originalFunctionTextBox);
            this.functionGroupBox.Controls.Add(this.originalFunctionLabel);
            this.functionGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.functionGroupBox.Location = new System.Drawing.Point(12, 8);
            this.functionGroupBox.Name = "functionGroupBox";
            this.functionGroupBox.Size = new System.Drawing.Size(316, 304);
            this.functionGroupBox.TabIndex = 14;
            this.functionGroupBox.TabStop = false;
            this.functionGroupBox.Text = "Function";
            // 
            // loadExpOnlyButton
            // 
            this.loadExpOnlyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadExpOnlyButton.Location = new System.Drawing.Point(181, 16);
            this.loadExpOnlyButton.Name = "loadExpOnlyButton";
            this.loadExpOnlyButton.Size = new System.Drawing.Size(126, 23);
            this.loadExpOnlyButton.TabIndex = 9;
            this.loadExpOnlyButton.Text = "Load Expression Only";
            this.toolTip.SetToolTip(this.loadExpOnlyButton, "Loads only the expression");
            this.loadExpOnlyButton.UseVisualStyleBackColor = true;
            this.loadExpOnlyButton.Click += new System.EventHandler(this.loadExpOnlyButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadButton.Location = new System.Drawing.Point(100, 16);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 6;
            this.loadButton.Text = "Load";
            this.toolTip.SetToolTip(this.loadButton, "Loads the expression and the model");
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // originalFunctionTextBox
            // 
            this.originalFunctionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.originalFunctionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalFunctionTextBox.Location = new System.Drawing.Point(6, 45);
            this.originalFunctionTextBox.Name = "originalFunctionTextBox";
            this.originalFunctionTextBox.ReadOnly = true;
            this.originalFunctionTextBox.Size = new System.Drawing.Size(301, 253);
            this.originalFunctionTextBox.TabIndex = 1;
            this.originalFunctionTextBox.Text = "";
            // 
            // originalFunctionLabel
            // 
            this.originalFunctionLabel.AutoSize = true;
            this.originalFunctionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalFunctionLabel.Location = new System.Drawing.Point(6, 21);
            this.originalFunctionLabel.Name = "originalFunctionLabel";
            this.originalFunctionLabel.Size = new System.Drawing.Size(86, 13);
            this.originalFunctionLabel.TabIndex = 0;
            this.originalFunctionLabel.Text = "Original function:";
            // 
            // factorRadioButton
            // 
            this.factorRadioButton.AutoSize = true;
            this.factorRadioButton.Enabled = false;
            this.factorRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.factorRadioButton.Location = new System.Drawing.Point(6, 423);
            this.factorRadioButton.Name = "factorRadioButton";
            this.factorRadioButton.Size = new System.Drawing.Size(85, 17);
            this.factorRadioButton.TabIndex = 8;
            this.factorRadioButton.Text = "Factorization";
            this.factorRadioButton.UseVisualStyleBackColor = true;
            this.factorRadioButton.CheckedChanged += new System.EventHandler(this.factorRadioButton_CheckedChanged);
            // 
            // normalRadioButton
            // 
            this.normalRadioButton.AutoSize = true;
            this.normalRadioButton.Checked = true;
            this.normalRadioButton.Enabled = false;
            this.normalRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.normalRadioButton.Location = new System.Drawing.Point(6, 400);
            this.normalRadioButton.Name = "normalRadioButton";
            this.normalRadioButton.Size = new System.Drawing.Size(58, 17);
            this.normalRadioButton.TabIndex = 7;
            this.normalRadioButton.TabStop = true;
            this.normalRadioButton.Text = "Normal";
            this.normalRadioButton.UseVisualStyleBackColor = true;
            this.normalRadioButton.CheckedChanged += new System.EventHandler(this.normalRadioButton_CheckedChanged);
            // 
            // adjustedTextBox
            // 
            this.adjustedTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.adjustedTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.adjustedTextBox.Location = new System.Drawing.Point(6, 18);
            this.adjustedTextBox.Name = "adjustedTextBox";
            this.adjustedTextBox.ReadOnly = true;
            this.adjustedTextBox.Size = new System.Drawing.Size(301, 376);
            this.adjustedTextBox.TabIndex = 5;
            this.adjustedTextBox.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.functionGraphTabPage);
            this.tabControl.Controls.Add(this.interactionsInfluencesTabPage);
            this.tabControl.Controls.Add(this.measurementsTabPage);
            this.tabControl.Controls.Add(this.helpTabPage);
            this.tabControl.Location = new System.Drawing.Point(332, 1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(664, 742);
            this.tabControl.TabIndex = 18;
            // 
            // functionGraphTabPage
            // 
            this.functionGraphTabPage.Controls.Add(this.pointPositionLabel);
            this.functionGraphTabPage.Controls.Add(this.ilFunctionPanel);
            this.functionGraphTabPage.Location = new System.Drawing.Point(4, 22);
            this.functionGraphTabPage.Name = "functionGraphTabPage";
            this.functionGraphTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.functionGraphTabPage.Size = new System.Drawing.Size(656, 716);
            this.functionGraphTabPage.TabIndex = 0;
            this.functionGraphTabPage.Text = "Function Graph";
            this.functionGraphTabPage.UseVisualStyleBackColor = true;
            // 
            // pointPositionLabel
            // 
            this.pointPositionLabel.AutoSize = true;
            this.pointPositionLabel.ForeColor = System.Drawing.Color.Green;
            this.pointPositionLabel.Location = new System.Drawing.Point(6, 698);
            this.pointPositionLabel.Name = "pointPositionLabel";
            this.pointPositionLabel.Size = new System.Drawing.Size(93, 13);
            this.pointPositionLabel.TabIndex = 1;
            this.pointPositionLabel.Text = "pointPositionLabel";
            this.pointPositionLabel.Visible = false;
            // 
            // ilFunctionPanel
            // 
            this.ilFunctionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilFunctionPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilFunctionPanel.Editor = null;
            this.ilFunctionPanel.Location = new System.Drawing.Point(3, 3);
            this.ilFunctionPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ilFunctionPanel.Name = "ilFunctionPanel";
            this.ilFunctionPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilFunctionPanel.Rectangle")));
            this.ilFunctionPanel.ShowUIControls = false;
            this.ilFunctionPanel.Size = new System.Drawing.Size(650, 710);
            this.ilFunctionPanel.TabIndex = 0;
            // 
            // interactionsInfluencesTabPage
            // 
            this.interactionsInfluencesTabPage.Controls.Add(this.rangeChart);
            this.interactionsInfluencesTabPage.Controls.Add(this.interactionsLabel);
            this.interactionsInfluencesTabPage.Controls.Add(this.chartDescriptionLabel);
            this.interactionsInfluencesTabPage.Controls.Add(this.maxOccuranceChart);
            this.interactionsInfluencesTabPage.Controls.Add(this.maxChart);
            this.interactionsInfluencesTabPage.Controls.Add(this.pieOptionLabel);
            this.interactionsInfluencesTabPage.Controls.Add(this.constantChart);
            this.interactionsInfluencesTabPage.Controls.Add(this.chartComboBox);
            this.interactionsInfluencesTabPage.Controls.Add(this.interactionChart);
            this.interactionsInfluencesTabPage.Controls.Add(this.interactionTextBox);
            this.interactionsInfluencesTabPage.Location = new System.Drawing.Point(4, 22);
            this.interactionsInfluencesTabPage.Name = "interactionsInfluencesTabPage";
            this.interactionsInfluencesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.interactionsInfluencesTabPage.Size = new System.Drawing.Size(656, 716);
            this.interactionsInfluencesTabPage.TabIndex = 1;
            this.interactionsInfluencesTabPage.Text = "Interactions and Influences";
            this.interactionsInfluencesTabPage.UseVisualStyleBackColor = true;
            // 
            // rangeChart
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.Name = "ChartArea1";
            this.rangeChart.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.rangeChart.Legends.Add(legend1);
            this.rangeChart.Location = new System.Drawing.Point(0, 380);
            this.rangeChart.Name = "rangeChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.rangeChart.Series.Add(series1);
            this.rangeChart.Size = new System.Drawing.Size(490, 336);
            this.rangeChart.TabIndex = 9;
            this.rangeChart.Text = "chart1";
            this.rangeChart.Visible = false;
            // 
            // interactionsLabel
            // 
            this.interactionsLabel.AutoSize = true;
            this.interactionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.interactionsLabel.Location = new System.Drawing.Point(-2, 6);
            this.interactionsLabel.Name = "interactionsLabel";
            this.interactionsLabel.Size = new System.Drawing.Size(78, 13);
            this.interactionsLabel.TabIndex = 8;
            this.interactionsLabel.Text = "Interactions:";
            // 
            // chartDescriptionLabel
            // 
            this.chartDescriptionLabel.AutoSize = true;
            this.chartDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartDescriptionLabel.Location = new System.Drawing.Point(6, 366);
            this.chartDescriptionLabel.Name = "chartDescriptionLabel";
            this.chartDescriptionLabel.Size = new System.Drawing.Size(41, 13);
            this.chartDescriptionLabel.TabIndex = 7;
            this.chartDescriptionLabel.Text = "label2";
            this.chartDescriptionLabel.Visible = false;
            // 
            // maxOccuranceChart
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.IsLabelAutoFit = false;
            chartArea2.AxisX.LabelStyle.Angle = -45;
            chartArea2.Name = "ChartArea1";
            this.maxOccuranceChart.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.maxOccuranceChart.Legends.Add(legend2);
            this.maxOccuranceChart.Location = new System.Drawing.Point(0, 380);
            this.maxOccuranceChart.Name = "maxOccuranceChart";
            this.maxOccuranceChart.Size = new System.Drawing.Size(490, 336);
            this.maxOccuranceChart.TabIndex = 6;
            this.maxOccuranceChart.Text = "chart1";
            // 
            // maxChart
            // 
            chartArea3.AxisX.Interval = 1D;
            chartArea3.AxisX.IsLabelAutoFit = false;
            chartArea3.AxisX.LabelStyle.Angle = -45;
            chartArea3.Name = "ChartArea1";
            this.maxChart.ChartAreas.Add(chartArea3);
            legend3.Enabled = false;
            legend3.Name = "Legend1";
            this.maxChart.Legends.Add(legend3);
            this.maxChart.Location = new System.Drawing.Point(0, 380);
            this.maxChart.Name = "maxChart";
            this.maxChart.Size = new System.Drawing.Size(490, 336);
            this.maxChart.TabIndex = 5;
            this.maxChart.Text = "chart1";
            // 
            // pieOptionLabel
            // 
            this.pieOptionLabel.AutoSize = true;
            this.pieOptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pieOptionLabel.Location = new System.Drawing.Point(499, 419);
            this.pieOptionLabel.Name = "pieOptionLabel";
            this.pieOptionLabel.Size = new System.Drawing.Size(68, 13);
            this.pieOptionLabel.TabIndex = 4;
            this.pieOptionLabel.Text = "Pie option:";
            // 
            // constantChart
            // 
            chartArea4.AxisX.Interval = 1D;
            chartArea4.AxisX.IsLabelAutoFit = false;
            chartArea4.AxisX.LabelStyle.Angle = -45;
            chartArea4.Name = "ChartArea1";
            this.constantChart.ChartAreas.Add(chartArea4);
            legend4.Enabled = false;
            legend4.Name = "Legend1";
            this.constantChart.Legends.Add(legend4);
            this.constantChart.Location = new System.Drawing.Point(0, 380);
            this.constantChart.Name = "constantChart";
            this.constantChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.constantChart.Size = new System.Drawing.Size(490, 336);
            this.constantChart.TabIndex = 3;
            this.constantChart.Text = "chart1";
            // 
            // chartComboBox
            // 
            this.chartComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chartComboBox.Enabled = false;
            this.chartComboBox.FormattingEnabled = true;
            this.chartComboBox.Location = new System.Drawing.Point(499, 445);
            this.chartComboBox.Name = "chartComboBox";
            this.chartComboBox.Size = new System.Drawing.Size(150, 21);
            this.chartComboBox.Sorted = true;
            this.chartComboBox.TabIndex = 2;
            this.chartComboBox.SelectedIndexChanged += new System.EventHandler(this.chartComboBox_SelectedIndexChanged);
            // 
            // interactionChart
            // 
            chartArea5.AxisX.Interval = 1D;
            chartArea5.AxisX.IsLabelAutoFit = false;
            chartArea5.AxisX.LabelStyle.Angle = -45;
            chartArea5.Name = "ChartArea1";
            this.interactionChart.ChartAreas.Add(chartArea5);
            legend5.Enabled = false;
            legend5.Name = "Legend1";
            this.interactionChart.Legends.Add(legend5);
            this.interactionChart.Location = new System.Drawing.Point(0, 380);
            this.interactionChart.Name = "interactionChart";
            this.interactionChart.Size = new System.Drawing.Size(490, 336);
            this.interactionChart.TabIndex = 1;
            this.interactionChart.Text = "chart1";
            // 
            // interactionTextBox
            // 
            this.interactionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.interactionTextBox.Location = new System.Drawing.Point(0, 24);
            this.interactionTextBox.Name = "interactionTextBox";
            this.interactionTextBox.ReadOnly = true;
            this.interactionTextBox.Size = new System.Drawing.Size(653, 331);
            this.interactionTextBox.TabIndex = 0;
            this.interactionTextBox.Text = "";
            // 
            // measurementsTabPage
            // 
            this.measurementsTabPage.Controls.Add(this.overviewPanel);
            this.measurementsTabPage.Controls.Add(this.relativeDifferencePanel);
            this.measurementsTabPage.Controls.Add(this.measurementPointLabel);
            this.measurementsTabPage.Controls.Add(this.absoluteDifferencePanel);
            this.measurementsTabPage.Controls.Add(this.measurementsOnlyPanel);
            this.measurementsTabPage.Controls.Add(this.measurementViewCombobox);
            this.measurementsTabPage.Controls.Add(this.measurementErrorLabel);
            this.measurementsTabPage.Controls.Add(this.bothGraphsPanel);
            this.measurementsTabPage.Controls.Add(this.nfpValueCombobox);
            this.measurementsTabPage.Controls.Add(this.loadMeasurementButton);
            this.measurementsTabPage.Location = new System.Drawing.Point(4, 22);
            this.measurementsTabPage.Name = "measurementsTabPage";
            this.measurementsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.measurementsTabPage.Size = new System.Drawing.Size(656, 716);
            this.measurementsTabPage.TabIndex = 2;
            this.measurementsTabPage.Text = "Difference with other measurements";
            this.measurementsTabPage.UseVisualStyleBackColor = true;
            // 
            // overviewPanel
            // 
            this.overviewPanel.Controls.Add(this.overviewRelativeDifferencePanel);
            this.overviewPanel.Controls.Add(this.overviewAbsoluteDifferencePanel);
            this.overviewPanel.Controls.Add(this.overviewMeasurementPanel);
            this.overviewPanel.Controls.Add(this.overviewPerformancePanel);
            this.overviewPanel.Controls.Add(this.relativeDifferenceLabel);
            this.overviewPanel.Controls.Add(this.absoluteDifferenceLabel);
            this.overviewPanel.Controls.Add(this.measurementsLabel);
            this.overviewPanel.Controls.Add(this.calculatedPerformancesLabel);
            this.overviewPanel.Location = new System.Drawing.Point(0, 30);
            this.overviewPanel.Name = "overviewPanel";
            this.overviewPanel.Size = new System.Drawing.Size(656, 665);
            this.overviewPanel.TabIndex = 8;
            this.overviewPanel.Visible = false;
            // 
            // overviewRelativeDifferencePanel
            // 
            this.overviewRelativeDifferencePanel.Controls.Add(this.overviewRelativeDifferenceIlPanel);
            this.overviewRelativeDifferencePanel.Location = new System.Drawing.Point(354, 335);
            this.overviewRelativeDifferencePanel.Name = "overviewRelativeDifferencePanel";
            this.overviewRelativeDifferencePanel.Size = new System.Drawing.Size(294, 276);
            this.overviewRelativeDifferencePanel.TabIndex = 7;
            // 
            // overviewRelativeDifferenceIlPanel
            // 
            this.overviewRelativeDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewRelativeDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewRelativeDifferenceIlPanel.Editor = null;
            this.overviewRelativeDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewRelativeDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.overviewRelativeDifferenceIlPanel.Name = "overviewRelativeDifferenceIlPanel";
            this.overviewRelativeDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewRelativeDifferenceIlPanel.Rectangle")));
            this.overviewRelativeDifferenceIlPanel.ShowUIControls = false;
            this.overviewRelativeDifferenceIlPanel.Size = new System.Drawing.Size(294, 276);
            this.overviewRelativeDifferenceIlPanel.TabIndex = 0;
            // 
            // overviewAbsoluteDifferencePanel
            // 
            this.overviewAbsoluteDifferencePanel.Controls.Add(this.overviewAbsoluteDifferenceIlPanel);
            this.overviewAbsoluteDifferencePanel.Location = new System.Drawing.Point(9, 335);
            this.overviewAbsoluteDifferencePanel.Name = "overviewAbsoluteDifferencePanel";
            this.overviewAbsoluteDifferencePanel.Size = new System.Drawing.Size(294, 276);
            this.overviewAbsoluteDifferencePanel.TabIndex = 6;
            // 
            // overviewAbsoluteDifferenceIlPanel
            // 
            this.overviewAbsoluteDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewAbsoluteDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewAbsoluteDifferenceIlPanel.Editor = null;
            this.overviewAbsoluteDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewAbsoluteDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.overviewAbsoluteDifferenceIlPanel.Name = "overviewAbsoluteDifferenceIlPanel";
            this.overviewAbsoluteDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewAbsoluteDifferenceIlPanel.Rectangle")));
            this.overviewAbsoluteDifferenceIlPanel.ShowUIControls = false;
            this.overviewAbsoluteDifferenceIlPanel.Size = new System.Drawing.Size(294, 276);
            this.overviewAbsoluteDifferenceIlPanel.TabIndex = 0;
            // 
            // overviewMeasurementPanel
            // 
            this.overviewMeasurementPanel.Controls.Add(this.overviewMeasurementIlPanel);
            this.overviewMeasurementPanel.Location = new System.Drawing.Point(354, 33);
            this.overviewMeasurementPanel.Name = "overviewMeasurementPanel";
            this.overviewMeasurementPanel.Size = new System.Drawing.Size(294, 276);
            this.overviewMeasurementPanel.TabIndex = 5;
            // 
            // overviewMeasurementIlPanel
            // 
            this.overviewMeasurementIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewMeasurementIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewMeasurementIlPanel.Editor = null;
            this.overviewMeasurementIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewMeasurementIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.overviewMeasurementIlPanel.Name = "overviewMeasurementIlPanel";
            this.overviewMeasurementIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewMeasurementIlPanel.Rectangle")));
            this.overviewMeasurementIlPanel.ShowUIControls = false;
            this.overviewMeasurementIlPanel.Size = new System.Drawing.Size(294, 276);
            this.overviewMeasurementIlPanel.TabIndex = 0;
            // 
            // overviewPerformancePanel
            // 
            this.overviewPerformancePanel.Controls.Add(this.overviewPerformanceIlPanel);
            this.overviewPerformancePanel.Location = new System.Drawing.Point(9, 33);
            this.overviewPerformancePanel.Name = "overviewPerformancePanel";
            this.overviewPerformancePanel.Size = new System.Drawing.Size(294, 276);
            this.overviewPerformancePanel.TabIndex = 4;
            // 
            // overviewPerformanceIlPanel
            // 
            this.overviewPerformanceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewPerformanceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewPerformanceIlPanel.Editor = null;
            this.overviewPerformanceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewPerformanceIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.overviewPerformanceIlPanel.Name = "overviewPerformanceIlPanel";
            this.overviewPerformanceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewPerformanceIlPanel.Rectangle")));
            this.overviewPerformanceIlPanel.ShowUIControls = false;
            this.overviewPerformanceIlPanel.Size = new System.Drawing.Size(294, 276);
            this.overviewPerformanceIlPanel.TabIndex = 0;
            // 
            // relativeDifferenceLabel
            // 
            this.relativeDifferenceLabel.AutoSize = true;
            this.relativeDifferenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.relativeDifferenceLabel.Location = new System.Drawing.Point(351, 319);
            this.relativeDifferenceLabel.Name = "relativeDifferenceLabel";
            this.relativeDifferenceLabel.Size = new System.Drawing.Size(117, 13);
            this.relativeDifferenceLabel.TabIndex = 3;
            this.relativeDifferenceLabel.Text = "Relative Difference";
            // 
            // absoluteDifferenceLabel
            // 
            this.absoluteDifferenceLabel.AutoSize = true;
            this.absoluteDifferenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.absoluteDifferenceLabel.Location = new System.Drawing.Point(6, 319);
            this.absoluteDifferenceLabel.Name = "absoluteDifferenceLabel";
            this.absoluteDifferenceLabel.Size = new System.Drawing.Size(119, 13);
            this.absoluteDifferenceLabel.TabIndex = 2;
            this.absoluteDifferenceLabel.Text = "Absolute Difference";
            // 
            // measurementsLabel
            // 
            this.measurementsLabel.AutoSize = true;
            this.measurementsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.measurementsLabel.Location = new System.Drawing.Point(351, 13);
            this.measurementsLabel.Name = "measurementsLabel";
            this.measurementsLabel.Size = new System.Drawing.Size(88, 13);
            this.measurementsLabel.TabIndex = 1;
            this.measurementsLabel.Text = "Measurements";
            // 
            // calculatedPerformancesLabel
            // 
            this.calculatedPerformancesLabel.AutoSize = true;
            this.calculatedPerformancesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatedPerformancesLabel.Location = new System.Drawing.Point(6, 13);
            this.calculatedPerformancesLabel.Name = "calculatedPerformancesLabel";
            this.calculatedPerformancesLabel.Size = new System.Drawing.Size(148, 13);
            this.calculatedPerformancesLabel.TabIndex = 0;
            this.calculatedPerformancesLabel.Text = "Calculated Performances";
            // 
            // relativeDifferencePanel
            // 
            this.relativeDifferencePanel.Controls.Add(this.relativeDifferenceIlPanel);
            this.relativeDifferencePanel.Location = new System.Drawing.Point(0, 30);
            this.relativeDifferencePanel.Name = "relativeDifferencePanel";
            this.relativeDifferencePanel.Size = new System.Drawing.Size(656, 665);
            this.relativeDifferencePanel.TabIndex = 7;
            this.relativeDifferencePanel.Visible = false;
            // 
            // relativeDifferenceIlPanel
            // 
            this.relativeDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.relativeDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.relativeDifferenceIlPanel.Editor = null;
            this.relativeDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.relativeDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.relativeDifferenceIlPanel.Name = "relativeDifferenceIlPanel";
            this.relativeDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("relativeDifferenceIlPanel.Rectangle")));
            this.relativeDifferenceIlPanel.ShowUIControls = false;
            this.relativeDifferenceIlPanel.Size = new System.Drawing.Size(656, 665);
            this.relativeDifferenceIlPanel.TabIndex = 0;
            // 
            // measurementPointLabel
            // 
            this.measurementPointLabel.AutoSize = true;
            this.measurementPointLabel.ForeColor = System.Drawing.Color.Green;
            this.measurementPointLabel.Location = new System.Drawing.Point(3, 698);
            this.measurementPointLabel.Name = "measurementPointLabel";
            this.measurementPointLabel.Size = new System.Drawing.Size(35, 13);
            this.measurementPointLabel.TabIndex = 1;
            this.measurementPointLabel.Text = "label8";
            this.measurementPointLabel.Visible = false;
            // 
            // absoluteDifferencePanel
            // 
            this.absoluteDifferencePanel.Controls.Add(this.absoluteDifferenceIlPanel);
            this.absoluteDifferencePanel.Location = new System.Drawing.Point(0, 30);
            this.absoluteDifferencePanel.Name = "absoluteDifferencePanel";
            this.absoluteDifferencePanel.Size = new System.Drawing.Size(656, 665);
            this.absoluteDifferencePanel.TabIndex = 6;
            this.absoluteDifferencePanel.Visible = false;
            // 
            // absoluteDifferenceIlPanel
            // 
            this.absoluteDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.absoluteDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.absoluteDifferenceIlPanel.Editor = null;
            this.absoluteDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.absoluteDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.absoluteDifferenceIlPanel.Name = "absoluteDifferenceIlPanel";
            this.absoluteDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("absoluteDifferenceIlPanel.Rectangle")));
            this.absoluteDifferenceIlPanel.ShowUIControls = false;
            this.absoluteDifferenceIlPanel.Size = new System.Drawing.Size(656, 665);
            this.absoluteDifferenceIlPanel.TabIndex = 0;
            // 
            // measurementsOnlyPanel
            // 
            this.measurementsOnlyPanel.Controls.Add(this.measurementsOnlyIlPanel);
            this.measurementsOnlyPanel.Location = new System.Drawing.Point(0, 30);
            this.measurementsOnlyPanel.Name = "measurementsOnlyPanel";
            this.measurementsOnlyPanel.Size = new System.Drawing.Size(656, 665);
            this.measurementsOnlyPanel.TabIndex = 5;
            this.measurementsOnlyPanel.Visible = false;
            // 
            // measurementsOnlyIlPanel
            // 
            this.measurementsOnlyIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measurementsOnlyIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.measurementsOnlyIlPanel.Editor = null;
            this.measurementsOnlyIlPanel.Location = new System.Drawing.Point(0, 0);
            this.measurementsOnlyIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.measurementsOnlyIlPanel.Name = "measurementsOnlyIlPanel";
            this.measurementsOnlyIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("measurementsOnlyIlPanel.Rectangle")));
            this.measurementsOnlyIlPanel.ShowUIControls = false;
            this.measurementsOnlyIlPanel.Size = new System.Drawing.Size(656, 665);
            this.measurementsOnlyIlPanel.TabIndex = 0;
            // 
            // measurementViewCombobox
            // 
            this.measurementViewCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementViewCombobox.Enabled = false;
            this.measurementViewCombobox.FormattingEnabled = true;
            this.measurementViewCombobox.Location = new System.Drawing.Point(383, 3);
            this.measurementViewCombobox.Name = "measurementViewCombobox";
            this.measurementViewCombobox.Size = new System.Drawing.Size(123, 21);
            this.measurementViewCombobox.TabIndex = 4;
            this.toolTip.SetToolTip(this.measurementViewCombobox, "Selected view of measurements");
            this.measurementViewCombobox.SelectedIndexChanged += new System.EventHandler(this.measurementViewCombobox_SelectedIndexChanged);
            // 
            // measurementErrorLabel
            // 
            this.measurementErrorLabel.AutoSize = true;
            this.measurementErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.measurementErrorLabel.Location = new System.Drawing.Point(130, 6);
            this.measurementErrorLabel.Name = "measurementErrorLabel";
            this.measurementErrorLabel.Size = new System.Drawing.Size(164, 13);
            this.measurementErrorLabel.TabIndex = 3;
            this.measurementErrorLabel.Text = "Please load some measurements.";
            // 
            // bothGraphsPanel
            // 
            this.bothGraphsPanel.Controls.Add(this.bothGraphsIlPanel);
            this.bothGraphsPanel.Location = new System.Drawing.Point(0, 30);
            this.bothGraphsPanel.Name = "bothGraphsPanel";
            this.bothGraphsPanel.Size = new System.Drawing.Size(656, 665);
            this.bothGraphsPanel.TabIndex = 2;
            this.bothGraphsPanel.TabStop = true;
            // 
            // bothGraphsIlPanel
            // 
            this.bothGraphsIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bothGraphsIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.bothGraphsIlPanel.Editor = null;
            this.bothGraphsIlPanel.Location = new System.Drawing.Point(0, 0);
            this.bothGraphsIlPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bothGraphsIlPanel.Name = "bothGraphsIlPanel";
            this.bothGraphsIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("bothGraphsIlPanel.Rectangle")));
            this.bothGraphsIlPanel.ShowUIControls = false;
            this.bothGraphsIlPanel.Size = new System.Drawing.Size(656, 665);
            this.bothGraphsIlPanel.TabIndex = 0;
            // 
            // nfpValueCombobox
            // 
            this.nfpValueCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nfpValueCombobox.Enabled = false;
            this.nfpValueCombobox.FormattingEnabled = true;
            this.nfpValueCombobox.Location = new System.Drawing.Point(512, 3);
            this.nfpValueCombobox.Name = "nfpValueCombobox";
            this.nfpValueCombobox.Size = new System.Drawing.Size(137, 21);
            this.nfpValueCombobox.TabIndex = 1;
            this.toolTip.SetToolTip(this.nfpValueCombobox, "Selected shown measurement value");
            this.nfpValueCombobox.SelectedIndexChanged += new System.EventHandler(this.nfpValueCombobox_SelectedIndexChanged);
            // 
            // loadMeasurementButton
            // 
            this.loadMeasurementButton.Location = new System.Drawing.Point(6, 1);
            this.loadMeasurementButton.Name = "loadMeasurementButton";
            this.loadMeasurementButton.Size = new System.Drawing.Size(118, 23);
            this.loadMeasurementButton.TabIndex = 0;
            this.loadMeasurementButton.Text = "Load measurements";
            this.toolTip.SetToolTip(this.loadMeasurementButton, "Loads valid measurements");
            this.loadMeasurementButton.UseVisualStyleBackColor = true;
            this.loadMeasurementButton.Click += new System.EventHandler(this.loadMeasurementButton_Click);
            // 
            // helpTabPage
            // 
            this.helpTabPage.Controls.Add(this.helpTextBox);
            this.helpTabPage.Location = new System.Drawing.Point(4, 22);
            this.helpTabPage.Name = "helpTabPage";
            this.helpTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.helpTabPage.Size = new System.Drawing.Size(656, 716);
            this.helpTabPage.TabIndex = 4;
            this.helpTabPage.Text = "Help";
            this.helpTabPage.UseVisualStyleBackColor = true;
            // 
            // helpTextBox
            // 
            this.helpTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.helpTextBox.Location = new System.Drawing.Point(0, 0);
            this.helpTextBox.Name = "helpTextBox";
            this.helpTextBox.ReadOnly = true;
            this.helpTextBox.Size = new System.Drawing.Size(656, 716);
            this.helpTextBox.TabIndex = 0;
            this.helpTextBox.Text = "";
            // 
            // constraintTextbox
            // 
            this.constraintTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.constraintTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constraintTextbox.Location = new System.Drawing.Point(6, 19);
            this.constraintTextbox.Name = "constraintTextbox";
            this.constraintTextbox.ReadOnly = true;
            this.constraintTextbox.Size = new System.Drawing.Size(301, 91);
            this.constraintTextbox.TabIndex = 12;
            this.constraintTextbox.Text = "";
            // 
            // factorizationSettingsButton
            // 
            this.factorizationSettingsButton.Enabled = false;
            this.factorizationSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.factorizationSettingsButton.Location = new System.Drawing.Point(109, 420);
            this.factorizationSettingsButton.Name = "factorizationSettingsButton";
            this.factorizationSettingsButton.Size = new System.Drawing.Size(120, 23);
            this.factorizationSettingsButton.TabIndex = 11;
            this.factorizationSettingsButton.Text = "Factorization Settings";
            this.toolTip.SetToolTip(this.factorizationSettingsButton, "Set your own factorization priorities");
            this.factorizationSettingsButton.UseVisualStyleBackColor = true;
            this.factorizationSettingsButton.Click += new System.EventHandler(this.factorizationSettingsButton_Click);
            // 
            // adjustedFunctionGroupBox
            // 
            this.adjustedFunctionGroupBox.Controls.Add(this.resetFactorizationButton);
            this.adjustedFunctionGroupBox.Controls.Add(this.adjustedTextBox);
            this.adjustedFunctionGroupBox.Controls.Add(this.factorizationSettingsButton);
            this.adjustedFunctionGroupBox.Controls.Add(this.normalRadioButton);
            this.adjustedFunctionGroupBox.Controls.Add(this.factorRadioButton);
            this.adjustedFunctionGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.adjustedFunctionGroupBox.Location = new System.Drawing.Point(1005, 8);
            this.adjustedFunctionGroupBox.Name = "adjustedFunctionGroupBox";
            this.adjustedFunctionGroupBox.Size = new System.Drawing.Size(316, 455);
            this.adjustedFunctionGroupBox.TabIndex = 19;
            this.adjustedFunctionGroupBox.TabStop = false;
            this.adjustedFunctionGroupBox.Text = "Adjusted function";
            // 
            // resetFactorizationButton
            // 
            this.resetFactorizationButton.Enabled = false;
            this.resetFactorizationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetFactorizationButton.Location = new System.Drawing.Point(235, 420);
            this.resetFactorizationButton.Name = "resetFactorizationButton";
            this.resetFactorizationButton.Size = new System.Drawing.Size(72, 23);
            this.resetFactorizationButton.TabIndex = 12;
            this.resetFactorizationButton.Text = "Reset";
            this.toolTip.SetToolTip(this.resetFactorizationButton, "Resets the factorization priorities");
            this.resetFactorizationButton.UseVisualStyleBackColor = true;
            this.resetFactorizationButton.Click += new System.EventHandler(this.resetFactorizationButton_Click);
            // 
            // constraintsGroupBox
            // 
            this.constraintsGroupBox.Controls.Add(this.constraintTextbox);
            this.constraintsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constraintsGroupBox.Location = new System.Drawing.Point(10, 624);
            this.constraintsGroupBox.Name = "constraintsGroupBox";
            this.constraintsGroupBox.Size = new System.Drawing.Size(316, 119);
            this.constraintsGroupBox.TabIndex = 20;
            this.constraintsGroupBox.TabStop = false;
            this.constraintsGroupBox.Text = "Constraints";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 746);
            this.Controls.Add(this.constraintsGroupBox);
            this.Controls.Add(this.adjustedFunctionGroupBox);
            this.Controls.Add(this.evaluationGroupBox);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.variableConfigurationGroupBox);
            this.Controls.Add(this.constantConfigurationGroupBox);
            this.Controls.Add(this.functionGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "SPLConqueror";
            this.evaluationGroupBox.ResumeLayout(false);
            this.evaluationFunctionPanel.ResumeLayout(false);
            this.evaluationFunctionPanel.PerformLayout();
            this.noNumericOptionPanel.ResumeLayout(false);
            this.noNumericOptionPanel.PerformLayout();
            this.variableConfigurationGroupBox.ResumeLayout(false);
            this.variableConfigurationGroupBox.PerformLayout();
            this.constantConfigurationGroupBox.ResumeLayout(false);
            this.constantConfigurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.constantRelativeValueSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantsDigitsUpDown)).EndInit();
            this.functionGroupBox.ResumeLayout(false);
            this.functionGroupBox.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.functionGraphTabPage.ResumeLayout(false);
            this.functionGraphTabPage.PerformLayout();
            this.interactionsInfluencesTabPage.ResumeLayout(false);
            this.interactionsInfluencesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangeChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxOccuranceChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.constantChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.interactionChart)).EndInit();
            this.measurementsTabPage.ResumeLayout(false);
            this.measurementsTabPage.PerformLayout();
            this.overviewPanel.ResumeLayout(false);
            this.overviewPanel.PerformLayout();
            this.overviewRelativeDifferencePanel.ResumeLayout(false);
            this.overviewAbsoluteDifferencePanel.ResumeLayout(false);
            this.overviewMeasurementPanel.ResumeLayout(false);
            this.overviewPerformancePanel.ResumeLayout(false);
            this.relativeDifferencePanel.ResumeLayout(false);
            this.absoluteDifferencePanel.ResumeLayout(false);
            this.measurementsOnlyPanel.ResumeLayout(false);
            this.bothGraphsPanel.ResumeLayout(false);
            this.helpTabPage.ResumeLayout(false);
            this.adjustedFunctionGroupBox.ResumeLayout(false);
            this.adjustedFunctionGroupBox.PerformLayout();
            this.constraintsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox evaluationGroupBox;
        private System.Windows.Forms.GroupBox variableConfigurationGroupBox;
        private System.Windows.Forms.CheckedListBox variableListBox;
        private System.Windows.Forms.CheckBox filterVariablesCheckbox;
        private System.Windows.Forms.GroupBox constantConfigurationGroupBox;
        private System.Windows.Forms.TrackBar constantRelativeValueSlider;
        private System.Windows.Forms.CheckBox constantDecimalCheckBox;
        private System.Windows.Forms.NumericUpDown constantsDigitsUpDown;
        private System.Windows.Forms.CheckBox constantFilteringCheckbox;
        private System.Windows.Forms.GroupBox functionGroupBox;
        private System.Windows.Forms.Button loadExpOnlyButton;
        private System.Windows.Forms.RadioButton factorRadioButton;
        private System.Windows.Forms.RadioButton normalRadioButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.RichTextBox originalFunctionTextBox;
        private System.Windows.Forms.Label originalFunctionLabel;
        private System.Windows.Forms.RichTextBox adjustedTextBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage functionGraphTabPage;
        private System.Windows.Forms.TabPage interactionsInfluencesTabPage;
        private System.Windows.Forms.RichTextBox interactionTextBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart interactionChart;
        private System.Windows.Forms.TabPage measurementsTabPage;
        private System.Windows.Forms.DataVisualization.Charting.Chart constantChart;
        private System.Windows.Forms.ComboBox chartComboBox;
        private System.Windows.Forms.Label pieOptionLabel;
        private System.Windows.Forms.Button factorizationSettingsButton;
        private System.Windows.Forms.ComboBox filterOptionCombobox;
        private System.Windows.Forms.TreeView variableTreeView;
        private System.Windows.Forms.Button loadMeasurementButton;
        private System.Windows.Forms.Panel evaluationFunctionPanel;
        private System.Windows.Forms.Label failureLabel;
        private System.Windows.Forms.Button generateFunctionButton;
        private System.Windows.Forms.ComboBox secondAxisCombobox;
        private System.Windows.Forms.Label secondAxisLabel;
        private System.Windows.Forms.Label firstAxisLabel;
        private System.Windows.Forms.ComboBox firstAxisCombobox;
        private System.Windows.Forms.Panel noNumericOptionPanel;
        private System.Windows.Forms.Label noNumericOptionsLabel;
        private System.Windows.Forms.Label calculationResultLabel;
        private System.Windows.Forms.Button calculatePerformanceButton;
        private System.Windows.Forms.Label calculatedPerformanceLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart maxOccuranceChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart maxChart;
        private System.Windows.Forms.Panel bothGraphsPanel;
        private ILNumerics.Drawing.ILPanel bothGraphsIlPanel;
        private System.Windows.Forms.ComboBox nfpValueCombobox;
        private System.Windows.Forms.Label measurementErrorLabel;
        private System.Windows.Forms.Label measurementPointLabel;
        private System.Windows.Forms.RichTextBox constraintTextbox;
        private System.Windows.Forms.TabPage helpTabPage;
        private System.Windows.Forms.ComboBox measurementViewCombobox;
        private System.Windows.Forms.Panel measurementsOnlyPanel;
        private System.Windows.Forms.Panel absoluteDifferencePanel;
        private ILNumerics.Drawing.ILPanel absoluteDifferenceIlPanel;
        private ILNumerics.Drawing.ILPanel measurementsOnlyIlPanel;
        private System.Windows.Forms.Label pointPositionLabel;
        private ILNumerics.Drawing.ILPanel ilFunctionPanel;
        private System.Windows.Forms.GroupBox adjustedFunctionGroupBox;
        private System.Windows.Forms.GroupBox constraintsGroupBox;
        private System.Windows.Forms.Button resetFactorizationButton;
        private System.Windows.Forms.RichTextBox helpTextBox;
        private System.Windows.Forms.Panel relativeDifferencePanel;
        private ILNumerics.Drawing.ILPanel relativeDifferenceIlPanel;
        private System.Windows.Forms.CheckBox filterRegexCheckBox;
        private System.Windows.Forms.TextBox regexTextbox;
        private System.Windows.Forms.Panel numericDefaultPanel;
        private System.Windows.Forms.Label interactionsLabel;
        private System.Windows.Forms.Label chartDescriptionLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart rangeChart;
        private System.Windows.Forms.Panel overviewPanel;
        private System.Windows.Forms.Panel overviewPerformancePanel;
        private System.Windows.Forms.Label relativeDifferenceLabel;
        private System.Windows.Forms.Label absoluteDifferenceLabel;
        private System.Windows.Forms.Label measurementsLabel;
        private System.Windows.Forms.Label calculatedPerformancesLabel;
        private System.Windows.Forms.Panel overviewMeasurementPanel;
        private System.Windows.Forms.Panel overviewRelativeDifferencePanel;
        private System.Windows.Forms.Panel overviewAbsoluteDifferencePanel;
        private ILNumerics.Drawing.ILPanel overviewRelativeDifferenceIlPanel;
        private ILNumerics.Drawing.ILPanel overviewAbsoluteDifferenceIlPanel;
        private ILNumerics.Drawing.ILPanel overviewMeasurementIlPanel;
        private ILNumerics.Drawing.ILPanel overviewPerformanceIlPanel;
        private System.Windows.Forms.Label defaultNumericOptionLabel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}