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
            this.interactionChartRepl = new OxyPlot.WindowsForms.PlotView();
            this.constantChartRepl = new OxyPlot.WindowsForms.PlotView();
            this.maxChartRepl = new OxyPlot.WindowsForms.PlotView();
            this.maxOccChartRepl = new OxyPlot.WindowsForms.PlotView();
            this.rangeChartRepl = new OxyPlot.WindowsForms.PlotView();
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
            this.chartPanel = new System.Windows.Forms.Panel();
            this.chartComboBox = new System.Windows.Forms.ComboBox();
            this.pieOptionLabel = new System.Windows.Forms.Label();
            this.chartDescriptionLabel = new System.Windows.Forms.Label();
            this.interactionsLabel = new System.Windows.Forms.Label();
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
            this.vipeTab = new System.Windows.Forms.TabPage();
            this.vipeSettingsPanel = new System.Windows.Forms.Panel();
            this.plotButton = new System.Windows.Forms.Button();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.data2Button = new System.Windows.Forms.Button();
            this.data1Button = new System.Windows.Forms.Button();
            this.data2TextBox = new System.Windows.Forms.TextBox();
            this.data1TextBox = new System.Windows.Forms.TextBox();
            this.data2Label = new System.Windows.Forms.Label();
            this.data1Label = new System.Windows.Forms.Label();
            this.vipeRSettings = new System.Windows.Forms.Panel();
            this.initializationCheckBox = new System.Windows.Forms.CheckBox();
            this.pathToExeButton = new System.Windows.Forms.Button();
            this.pathToLibButton = new System.Windows.Forms.Button();
            this.pathToRExe = new System.Windows.Forms.TextBox();
            this.performInitialization = new System.Windows.Forms.Label();
            this.pathToRExeLabel = new System.Windows.Forms.Label();
            this.pathToRLibLabel = new System.Windows.Forms.Label();
            this.pathToRLib = new System.Windows.Forms.TextBox();
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
            this.chartPanel.SuspendLayout();
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
            this.vipeTab.SuspendLayout();
            this.vipeSettingsPanel.SuspendLayout();
            this.dataPanel.SuspendLayout();
            this.vipeRSettings.SuspendLayout();
            this.adjustedFunctionGroupBox.SuspendLayout();
            this.constraintsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // interactionChartRepl
            // 
            this.interactionChartRepl.Location = new System.Drawing.Point(3, 57);
            this.interactionChartRepl.Name = "interactionChartRepl";
            this.interactionChartRepl.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.interactionChartRepl.Size = new System.Drawing.Size(855, 322);
            this.interactionChartRepl.TabIndex = 8;
            this.interactionChartRepl.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.interactionChartRepl.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.interactionChartRepl.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // constantChartRepl
            // 
            this.constantChartRepl.Location = new System.Drawing.Point(3, 57);
            this.constantChartRepl.Name = "constantChartRepl";
            this.constantChartRepl.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.constantChartRepl.Size = new System.Drawing.Size(855, 322);
            this.constantChartRepl.TabIndex = 8;
            this.constantChartRepl.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.constantChartRepl.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.constantChartRepl.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // maxChartRepl
            // 
            this.maxChartRepl.Location = new System.Drawing.Point(3, 57);
            this.maxChartRepl.Name = "maxChartRepl";
            this.maxChartRepl.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.maxChartRepl.Size = new System.Drawing.Size(855, 322);
            this.maxChartRepl.TabIndex = 8;
            this.maxChartRepl.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.maxChartRepl.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.maxChartRepl.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // maxOccChartRepl
            // 
            this.maxOccChartRepl.Location = new System.Drawing.Point(3, 57);
            this.maxOccChartRepl.Name = "maxOccChartRepl";
            this.maxOccChartRepl.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.maxOccChartRepl.Size = new System.Drawing.Size(855, 322);
            this.maxOccChartRepl.TabIndex = 8;
            this.maxOccChartRepl.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.maxOccChartRepl.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.maxOccChartRepl.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // rangeChartRepl
            // 
            this.rangeChartRepl.Location = new System.Drawing.Point(3, 57);
            this.rangeChartRepl.Name = "rangeChartRepl";
            this.rangeChartRepl.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.rangeChartRepl.Size = new System.Drawing.Size(855, 322);
            this.rangeChartRepl.TabIndex = 8;
            this.rangeChartRepl.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.rangeChartRepl.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.rangeChartRepl.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // evaluationGroupBox
            // 
            this.evaluationGroupBox.Controls.Add(this.evaluationFunctionPanel);
            this.evaluationGroupBox.Controls.Add(this.noNumericOptionPanel);
            this.evaluationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.evaluationGroupBox.Location = new System.Drawing.Point(1340, 577);
            this.evaluationGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.evaluationGroupBox.Name = "evaluationGroupBox";
            this.evaluationGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.evaluationGroupBox.Size = new System.Drawing.Size(421, 337);
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
            this.evaluationFunctionPanel.Location = new System.Drawing.Point(8, 22);
            this.evaluationFunctionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.evaluationFunctionPanel.Name = "evaluationFunctionPanel";
            this.evaluationFunctionPanel.Size = new System.Drawing.Size(401, 308);
            this.evaluationFunctionPanel.TabIndex = 1;
            // 
            // defaultNumericOptionLabel
            // 
            this.defaultNumericOptionLabel.AutoSize = true;
            this.defaultNumericOptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultNumericOptionLabel.Location = new System.Drawing.Point(4, 100);
            this.defaultNumericOptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.defaultNumericOptionLabel.Name = "defaultNumericOptionLabel";
            this.defaultNumericOptionLabel.Size = new System.Drawing.Size(262, 17);
            this.defaultNumericOptionLabel.TabIndex = 16;
            this.defaultNumericOptionLabel.Text = "Default values for numeric options:";
            // 
            // numericDefaultPanel
            // 
            this.numericDefaultPanel.AutoScroll = true;
            this.numericDefaultPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericDefaultPanel.Location = new System.Drawing.Point(0, 118);
            this.numericDefaultPanel.Margin = new System.Windows.Forms.Padding(4);
            this.numericDefaultPanel.Name = "numericDefaultPanel";
            this.numericDefaultPanel.Size = new System.Drawing.Size(401, 190);
            this.numericDefaultPanel.TabIndex = 15;
            // 
            // failureLabel
            // 
            this.failureLabel.AutoSize = true;
            this.failureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failureLabel.ForeColor = System.Drawing.Color.Crimson;
            this.failureLabel.Location = new System.Drawing.Point(4, 74);
            this.failureLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.failureLabel.Name = "failureLabel";
            this.failureLabel.Size = new System.Drawing.Size(46, 17);
            this.failureLabel.TabIndex = 14;
            this.failureLabel.Text = "label5";
            this.failureLabel.Visible = false;
            // 
            // generateFunctionButton
            // 
            this.generateFunctionButton.Enabled = false;
            this.generateFunctionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generateFunctionButton.Location = new System.Drawing.Point(321, 0);
            this.generateFunctionButton.Margin = new System.Windows.Forms.Padding(4);
            this.generateFunctionButton.Name = "generateFunctionButton";
            this.generateFunctionButton.Size = new System.Drawing.Size(80, 66);
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
            this.secondAxisCombobox.Location = new System.Drawing.Point(101, 39);
            this.secondAxisCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.secondAxisCombobox.Name = "secondAxisCombobox";
            this.secondAxisCombobox.Size = new System.Drawing.Size(211, 25);
            this.secondAxisCombobox.Sorted = true;
            this.secondAxisCombobox.TabIndex = 12;
            this.secondAxisCombobox.SelectedIndexChanged += new System.EventHandler(this.secondAxisCombobox_SelectedIndexChanged);
            // 
            // secondAxisLabel
            // 
            this.secondAxisLabel.AutoSize = true;
            this.secondAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondAxisLabel.Location = new System.Drawing.Point(4, 43);
            this.secondAxisLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.secondAxisLabel.Name = "secondAxisLabel";
            this.secondAxisLabel.Size = new System.Drawing.Size(88, 17);
            this.secondAxisLabel.TabIndex = 11;
            this.secondAxisLabel.Text = "Second axis:";
            // 
            // firstAxisLabel
            // 
            this.firstAxisLabel.AutoSize = true;
            this.firstAxisLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstAxisLabel.Location = new System.Drawing.Point(4, 5);
            this.firstAxisLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.firstAxisLabel.Name = "firstAxisLabel";
            this.firstAxisLabel.Size = new System.Drawing.Size(67, 17);
            this.firstAxisLabel.TabIndex = 10;
            this.firstAxisLabel.Text = "First axis:";
            // 
            // firstAxisCombobox
            // 
            this.firstAxisCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firstAxisCombobox.Enabled = false;
            this.firstAxisCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstAxisCombobox.Location = new System.Drawing.Point(101, 1);
            this.firstAxisCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.firstAxisCombobox.Name = "firstAxisCombobox";
            this.firstAxisCombobox.Size = new System.Drawing.Size(211, 25);
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
            this.noNumericOptionPanel.Location = new System.Drawing.Point(8, 22);
            this.noNumericOptionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.noNumericOptionPanel.Name = "noNumericOptionPanel";
            this.noNumericOptionPanel.Size = new System.Drawing.Size(401, 128);
            this.noNumericOptionPanel.TabIndex = 0;
            // 
            // calculationResultLabel
            // 
            this.calculationResultLabel.AutoSize = true;
            this.calculationResultLabel.Location = new System.Drawing.Point(-4, 84);
            this.calculationResultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.calculationResultLabel.Name = "calculationResultLabel";
            this.calculationResultLabel.Size = new System.Drawing.Size(186, 17);
            this.calculationResultLabel.TabIndex = 3;
            this.calculationResultLabel.Text = "Please press the button.";
            // 
            // calculatePerformanceButton
            // 
            this.calculatePerformanceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatePerformanceButton.Location = new System.Drawing.Point(291, 52);
            this.calculatePerformanceButton.Margin = new System.Windows.Forms.Padding(4);
            this.calculatePerformanceButton.Name = "calculatePerformanceButton";
            this.calculatePerformanceButton.Size = new System.Drawing.Size(107, 28);
            this.calculatePerformanceButton.TabIndex = 2;
            this.calculatePerformanceButton.Text = "Calculate";
            this.calculatePerformanceButton.UseVisualStyleBackColor = true;
            this.calculatePerformanceButton.Click += new System.EventHandler(this.calculatePerformanceButton_Click);
            // 
            // calculatedPerformanceLabel
            // 
            this.calculatedPerformanceLabel.AutoSize = true;
            this.calculatedPerformanceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatedPerformanceLabel.Location = new System.Drawing.Point(-4, 58);
            this.calculatedPerformanceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.calculatedPerformanceLabel.Name = "calculatedPerformanceLabel";
            this.calculatedPerformanceLabel.Size = new System.Drawing.Size(162, 17);
            this.calculatedPerformanceLabel.TabIndex = 1;
            this.calculatedPerformanceLabel.Text = "Calculated performance:";
            // 
            // noNumericOptionsLabel
            // 
            this.noNumericOptionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noNumericOptionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noNumericOptionsLabel.ForeColor = System.Drawing.Color.Red;
            this.noNumericOptionsLabel.Location = new System.Drawing.Point(0, 0);
            this.noNumericOptionsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.noNumericOptionsLabel.Name = "noNumericOptionsLabel";
            this.noNumericOptionsLabel.Size = new System.Drawing.Size(401, 128);
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
            this.variableConfigurationGroupBox.Location = new System.Drawing.Point(13, 511);
            this.variableConfigurationGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.variableConfigurationGroupBox.Name = "variableConfigurationGroupBox";
            this.variableConfigurationGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.variableConfigurationGroupBox.Size = new System.Drawing.Size(421, 250);
            this.variableConfigurationGroupBox.TabIndex = 16;
            this.variableConfigurationGroupBox.TabStop = false;
            this.variableConfigurationGroupBox.Text = "Variable configuration";
            // 
            // regexTextbox
            // 
            this.regexTextbox.Enabled = false;
            this.regexTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regexTextbox.Location = new System.Drawing.Point(8, 218);
            this.regexTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.regexTextbox.Name = "regexTextbox";
            this.regexTextbox.Size = new System.Drawing.Size(400, 23);
            this.regexTextbox.TabIndex = 5;
            this.regexTextbox.TextChanged += new System.EventHandler(this.regexTextbox_TextChanged);
            // 
            // filterRegexCheckBox
            // 
            this.filterRegexCheckBox.AutoSize = true;
            this.filterRegexCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.filterRegexCheckBox.Enabled = false;
            this.filterRegexCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterRegexCheckBox.Location = new System.Drawing.Point(12, 193);
            this.filterRegexCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.filterRegexCheckBox.Name = "filterRegexCheckBox";
            this.filterRegexCheckBox.Size = new System.Drawing.Size(226, 21);
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
            this.filterOptionCombobox.Location = new System.Drawing.Point(227, 18);
            this.filterOptionCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.filterOptionCombobox.Name = "filterOptionCombobox";
            this.filterOptionCombobox.Size = new System.Drawing.Size(181, 25);
            this.filterOptionCombobox.TabIndex = 3;
            this.filterOptionCombobox.SelectedIndexChanged += new System.EventHandler(this.filterOptionCombobox_SelectedIndexChanged);
            // 
            // variableTreeView
            // 
            this.variableTreeView.CheckBoxes = true;
            this.variableTreeView.Enabled = false;
            this.variableTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableTreeView.Location = new System.Drawing.Point(8, 52);
            this.variableTreeView.Margin = new System.Windows.Forms.Padding(4);
            this.variableTreeView.Name = "variableTreeView";
            this.variableTreeView.Size = new System.Drawing.Size(400, 133);
            this.variableTreeView.TabIndex = 2;
            this.variableTreeView.Visible = false;
            // 
            // variableListBox
            // 
            this.variableListBox.CheckOnClick = true;
            this.variableListBox.Enabled = false;
            this.variableListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableListBox.FormattingEnabled = true;
            this.variableListBox.Location = new System.Drawing.Point(8, 52);
            this.variableListBox.Margin = new System.Windows.Forms.Padding(4);
            this.variableListBox.Name = "variableListBox";
            this.variableListBox.Size = new System.Drawing.Size(400, 130);
            this.variableListBox.Sorted = true;
            this.variableListBox.TabIndex = 1;
            this.variableListBox.SelectedIndexChanged += new System.EventHandler(this.variableListBox_SelectedIndexChanged);
            // 
            // filterVariablesCheckbox
            // 
            this.filterVariablesCheckbox.AutoSize = true;
            this.filterVariablesCheckbox.Enabled = false;
            this.filterVariablesCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterVariablesCheckbox.Location = new System.Drawing.Point(12, 23);
            this.filterVariablesCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.filterVariablesCheckbox.Name = "filterVariablesCheckbox";
            this.filterVariablesCheckbox.Size = new System.Drawing.Size(122, 21);
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
            this.constantConfigurationGroupBox.Location = new System.Drawing.Point(16, 391);
            this.constantConfigurationGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.constantConfigurationGroupBox.Name = "constantConfigurationGroupBox";
            this.constantConfigurationGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.constantConfigurationGroupBox.Size = new System.Drawing.Size(421, 112);
            this.constantConfigurationGroupBox.TabIndex = 15;
            this.constantConfigurationGroupBox.TabStop = false;
            this.constantConfigurationGroupBox.Text = "Constant configuration";
            // 
            // constantRelativeValueSlider
            // 
            this.constantRelativeValueSlider.Enabled = false;
            this.constantRelativeValueSlider.Location = new System.Drawing.Point(227, 54);
            this.constantRelativeValueSlider.Margin = new System.Windows.Forms.Padding(4);
            this.constantRelativeValueSlider.Maximum = 100;
            this.constantRelativeValueSlider.Name = "constantRelativeValueSlider";
            this.constantRelativeValueSlider.Size = new System.Drawing.Size(183, 56);
            this.constantRelativeValueSlider.TabIndex = 5;
            this.constantRelativeValueSlider.Scroll += new System.EventHandler(this.constantRelativeValueSlider_Scroll);
            // 
            // constantDecimalCheckBox
            // 
            this.constantDecimalCheckBox.AutoSize = true;
            this.constantDecimalCheckBox.Enabled = false;
            this.constantDecimalCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantDecimalCheckBox.Location = new System.Drawing.Point(12, 23);
            this.constantDecimalCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.constantDecimalCheckBox.Name = "constantDecimalCheckBox";
            this.constantDecimalCheckBox.Size = new System.Drawing.Size(214, 21);
            this.constantDecimalCheckBox.TabIndex = 11;
            this.constantDecimalCheckBox.Text = "Fractional digits of constants:";
            this.constantDecimalCheckBox.UseVisualStyleBackColor = true;
            this.constantDecimalCheckBox.CheckedChanged += new System.EventHandler(this.constantDecimalCheckBox_CheckedChanged);
            // 
            // constantsDigitsUpDown
            // 
            this.constantsDigitsUpDown.Enabled = false;
            this.constantsDigitsUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantsDigitsUpDown.Location = new System.Drawing.Point(241, 22);
            this.constantsDigitsUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.constantsDigitsUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.constantsDigitsUpDown.Name = "constantsDigitsUpDown";
            this.constantsDigitsUpDown.Size = new System.Drawing.Size(49, 23);
            this.constantsDigitsUpDown.TabIndex = 8;
            this.constantsDigitsUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.constantsDigitsUpDown.ValueChanged += new System.EventHandler(this.constantsDigitsUpDown_ValueChanged);
            // 
            // constantFilteringCheckbox
            // 
            this.constantFilteringCheckbox.AutoSize = true;
            this.constantFilteringCheckbox.Enabled = false;
            this.constantFilteringCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constantFilteringCheckbox.Location = new System.Drawing.Point(12, 57);
            this.constantFilteringCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.constantFilteringCheckbox.Name = "constantFilteringCheckbox";
            this.constantFilteringCheckbox.Size = new System.Drawing.Size(200, 21);
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
            this.functionGroupBox.Location = new System.Drawing.Point(16, 10);
            this.functionGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.functionGroupBox.Name = "functionGroupBox";
            this.functionGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.functionGroupBox.Size = new System.Drawing.Size(421, 374);
            this.functionGroupBox.TabIndex = 14;
            this.functionGroupBox.TabStop = false;
            this.functionGroupBox.Text = "Function";
            // 
            // loadExpOnlyButton
            // 
            this.loadExpOnlyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadExpOnlyButton.Location = new System.Drawing.Point(241, 20);
            this.loadExpOnlyButton.Margin = new System.Windows.Forms.Padding(4);
            this.loadExpOnlyButton.Name = "loadExpOnlyButton";
            this.loadExpOnlyButton.Size = new System.Drawing.Size(168, 28);
            this.loadExpOnlyButton.TabIndex = 9;
            this.loadExpOnlyButton.Text = "Load Expression Only";
            this.toolTip.SetToolTip(this.loadExpOnlyButton, "Loads only the expression");
            this.loadExpOnlyButton.UseVisualStyleBackColor = true;
            this.loadExpOnlyButton.Click += new System.EventHandler(this.loadExpOnlyButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadButton.Location = new System.Drawing.Point(133, 20);
            this.loadButton.Margin = new System.Windows.Forms.Padding(4);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(100, 28);
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
            this.originalFunctionTextBox.Location = new System.Drawing.Point(8, 55);
            this.originalFunctionTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.originalFunctionTextBox.Name = "originalFunctionTextBox";
            this.originalFunctionTextBox.ReadOnly = true;
            this.originalFunctionTextBox.Size = new System.Drawing.Size(400, 310);
            this.originalFunctionTextBox.TabIndex = 1;
            this.originalFunctionTextBox.Text = "";
            // 
            // originalFunctionLabel
            // 
            this.originalFunctionLabel.AutoSize = true;
            this.originalFunctionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalFunctionLabel.Location = new System.Drawing.Point(8, 26);
            this.originalFunctionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.originalFunctionLabel.Name = "originalFunctionLabel";
            this.originalFunctionLabel.Size = new System.Drawing.Size(115, 17);
            this.originalFunctionLabel.TabIndex = 0;
            this.originalFunctionLabel.Text = "Original function:";
            // 
            // factorRadioButton
            // 
            this.factorRadioButton.AutoSize = true;
            this.factorRadioButton.Enabled = false;
            this.factorRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.factorRadioButton.Location = new System.Drawing.Point(8, 521);
            this.factorRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.factorRadioButton.Name = "factorRadioButton";
            this.factorRadioButton.Size = new System.Drawing.Size(110, 21);
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
            this.normalRadioButton.Location = new System.Drawing.Point(8, 492);
            this.normalRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.normalRadioButton.Name = "normalRadioButton";
            this.normalRadioButton.Size = new System.Drawing.Size(74, 21);
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
            this.adjustedTextBox.Location = new System.Drawing.Point(8, 22);
            this.adjustedTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.adjustedTextBox.Name = "adjustedTextBox";
            this.adjustedTextBox.ReadOnly = true;
            this.adjustedTextBox.Size = new System.Drawing.Size(400, 462);
            this.adjustedTextBox.TabIndex = 5;
            this.adjustedTextBox.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.functionGraphTabPage);
            this.tabControl.Controls.Add(this.interactionsInfluencesTabPage);
            this.tabControl.Controls.Add(this.measurementsTabPage);
            this.tabControl.Controls.Add(this.helpTabPage);
            this.tabControl.Controls.Add(this.vipeTab);
            this.tabControl.Location = new System.Drawing.Point(443, 1);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(885, 913);
            this.tabControl.TabIndex = 18;
            // 
            // functionGraphTabPage
            // 
            this.functionGraphTabPage.Controls.Add(this.pointPositionLabel);
            this.functionGraphTabPage.Controls.Add(this.ilFunctionPanel);
            this.functionGraphTabPage.Location = new System.Drawing.Point(4, 25);
            this.functionGraphTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.functionGraphTabPage.Name = "functionGraphTabPage";
            this.functionGraphTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.functionGraphTabPage.Size = new System.Drawing.Size(877, 884);
            this.functionGraphTabPage.TabIndex = 0;
            this.functionGraphTabPage.Text = "Function Graph";
            this.functionGraphTabPage.UseVisualStyleBackColor = true;
            // 
            // pointPositionLabel
            // 
            this.pointPositionLabel.AutoSize = true;
            this.pointPositionLabel.ForeColor = System.Drawing.Color.Green;
            this.pointPositionLabel.Location = new System.Drawing.Point(8, 859);
            this.pointPositionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pointPositionLabel.Name = "pointPositionLabel";
            this.pointPositionLabel.Size = new System.Drawing.Size(124, 17);
            this.pointPositionLabel.TabIndex = 1;
            this.pointPositionLabel.Text = "pointPositionLabel";
            this.pointPositionLabel.Visible = false;
            // 
            // ilFunctionPanel
            // 
            this.ilFunctionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilFunctionPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilFunctionPanel.Editor = null;
            this.ilFunctionPanel.Location = new System.Drawing.Point(4, 4);
            this.ilFunctionPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ilFunctionPanel.Name = "ilFunctionPanel";
            this.ilFunctionPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilFunctionPanel.Rectangle")));
            this.ilFunctionPanel.ShowUIControls = false;
            this.ilFunctionPanel.Size = new System.Drawing.Size(869, 876);
            this.ilFunctionPanel.TabIndex = 0;
            // 
            // interactionsInfluencesTabPage
            // 
            this.interactionsInfluencesTabPage.Controls.Add(this.chartPanel);
            this.interactionsInfluencesTabPage.Controls.Add(this.interactionsLabel);
            this.interactionsInfluencesTabPage.Controls.Add(this.interactionTextBox);
            this.interactionsInfluencesTabPage.Location = new System.Drawing.Point(4, 25);
            this.interactionsInfluencesTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.interactionsInfluencesTabPage.Name = "interactionsInfluencesTabPage";
            this.interactionsInfluencesTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.interactionsInfluencesTabPage.Size = new System.Drawing.Size(877, 884);
            this.interactionsInfluencesTabPage.TabIndex = 1;
            this.interactionsInfluencesTabPage.Text = "Interactions and Influences";
            this.interactionsInfluencesTabPage.UseVisualStyleBackColor = true;
            // 
            // chartPanel
            // 
            this.chartPanel.Controls.Add(this.chartComboBox);
            this.chartPanel.Controls.Add(this.pieOptionLabel);
            this.chartPanel.Controls.Add(this.chartDescriptionLabel);
            this.chartPanel.Controls.Add(this.interactionChartRepl);
            this.chartPanel.Controls.Add(this.constantChartRepl);
            this.chartPanel.Controls.Add(this.maxChartRepl);
            this.chartPanel.Controls.Add(this.maxOccChartRepl);
            this.chartPanel.Controls.Add(this.rangeChartRepl);
            this.chartPanel.Location = new System.Drawing.Point(7, 443);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Size = new System.Drawing.Size(862, 433);
            this.chartPanel.TabIndex = 10;
            // 
            // chartComboBox
            // 
            this.chartComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chartComboBox.Enabled = false;
            this.chartComboBox.FormattingEnabled = true;
            this.chartComboBox.Location = new System.Drawing.Point(659, 30);
            this.chartComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.chartComboBox.Name = "chartComboBox";
            this.chartComboBox.Size = new System.Drawing.Size(199, 24);
            this.chartComboBox.Sorted = true;
            this.chartComboBox.TabIndex = 2;
            this.chartComboBox.SelectedIndexChanged += new System.EventHandler(this.chartComboBox_SelectedIndexChanged);
            // 
            // pieOptionLabel
            // 
            this.pieOptionLabel.AutoSize = true;
            this.pieOptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pieOptionLabel.Location = new System.Drawing.Point(656, 8);
            this.pieOptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pieOptionLabel.Name = "pieOptionLabel";
            this.pieOptionLabel.Size = new System.Drawing.Size(86, 17);
            this.pieOptionLabel.TabIndex = 4;
            this.pieOptionLabel.Text = "Pie option:";
            // 
            // chartDescriptionLabel
            // 
            this.chartDescriptionLabel.AutoSize = true;
            this.chartDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartDescriptionLabel.Location = new System.Drawing.Point(4, 9);
            this.chartDescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.chartDescriptionLabel.Name = "chartDescriptionLabel";
            this.chartDescriptionLabel.Size = new System.Drawing.Size(52, 17);
            this.chartDescriptionLabel.TabIndex = 7;
            this.chartDescriptionLabel.Text = "label2";
            this.chartDescriptionLabel.Visible = false;
            // 
            // interactionsLabel
            // 
            this.interactionsLabel.AutoSize = true;
            this.interactionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.interactionsLabel.Location = new System.Drawing.Point(-3, 7);
            this.interactionsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.interactionsLabel.Name = "interactionsLabel";
            this.interactionsLabel.Size = new System.Drawing.Size(98, 17);
            this.interactionsLabel.TabIndex = 8;
            this.interactionsLabel.Text = "Interactions:";
            // 
            // interactionTextBox
            // 
            this.interactionTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.interactionTextBox.Location = new System.Drawing.Point(4, 30);
            this.interactionTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.interactionTextBox.Name = "interactionTextBox";
            this.interactionTextBox.ReadOnly = true;
            this.interactionTextBox.Size = new System.Drawing.Size(865, 406);
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
            this.measurementsTabPage.Location = new System.Drawing.Point(4, 25);
            this.measurementsTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.measurementsTabPage.Name = "measurementsTabPage";
            this.measurementsTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.measurementsTabPage.Size = new System.Drawing.Size(877, 884);
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
            this.overviewPanel.Location = new System.Drawing.Point(0, 37);
            this.overviewPanel.Margin = new System.Windows.Forms.Padding(4);
            this.overviewPanel.Name = "overviewPanel";
            this.overviewPanel.Size = new System.Drawing.Size(875, 818);
            this.overviewPanel.TabIndex = 8;
            this.overviewPanel.Visible = false;
            // 
            // overviewRelativeDifferencePanel
            // 
            this.overviewRelativeDifferencePanel.Controls.Add(this.overviewRelativeDifferenceIlPanel);
            this.overviewRelativeDifferencePanel.Location = new System.Drawing.Point(472, 412);
            this.overviewRelativeDifferencePanel.Margin = new System.Windows.Forms.Padding(4);
            this.overviewRelativeDifferencePanel.Name = "overviewRelativeDifferencePanel";
            this.overviewRelativeDifferencePanel.Size = new System.Drawing.Size(392, 340);
            this.overviewRelativeDifferencePanel.TabIndex = 7;
            // 
            // overviewRelativeDifferenceIlPanel
            // 
            this.overviewRelativeDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewRelativeDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewRelativeDifferenceIlPanel.Editor = null;
            this.overviewRelativeDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewRelativeDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overviewRelativeDifferenceIlPanel.Name = "overviewRelativeDifferenceIlPanel";
            this.overviewRelativeDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewRelativeDifferenceIlPanel.Rectangle")));
            this.overviewRelativeDifferenceIlPanel.ShowUIControls = false;
            this.overviewRelativeDifferenceIlPanel.Size = new System.Drawing.Size(392, 340);
            this.overviewRelativeDifferenceIlPanel.TabIndex = 0;
            // 
            // overviewAbsoluteDifferencePanel
            // 
            this.overviewAbsoluteDifferencePanel.Controls.Add(this.overviewAbsoluteDifferenceIlPanel);
            this.overviewAbsoluteDifferencePanel.Location = new System.Drawing.Point(12, 412);
            this.overviewAbsoluteDifferencePanel.Margin = new System.Windows.Forms.Padding(4);
            this.overviewAbsoluteDifferencePanel.Name = "overviewAbsoluteDifferencePanel";
            this.overviewAbsoluteDifferencePanel.Size = new System.Drawing.Size(392, 340);
            this.overviewAbsoluteDifferencePanel.TabIndex = 6;
            // 
            // overviewAbsoluteDifferenceIlPanel
            // 
            this.overviewAbsoluteDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewAbsoluteDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewAbsoluteDifferenceIlPanel.Editor = null;
            this.overviewAbsoluteDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewAbsoluteDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overviewAbsoluteDifferenceIlPanel.Name = "overviewAbsoluteDifferenceIlPanel";
            this.overviewAbsoluteDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewAbsoluteDifferenceIlPanel.Rectangle")));
            this.overviewAbsoluteDifferenceIlPanel.ShowUIControls = false;
            this.overviewAbsoluteDifferenceIlPanel.Size = new System.Drawing.Size(392, 340);
            this.overviewAbsoluteDifferenceIlPanel.TabIndex = 0;
            // 
            // overviewMeasurementPanel
            // 
            this.overviewMeasurementPanel.Controls.Add(this.overviewMeasurementIlPanel);
            this.overviewMeasurementPanel.Location = new System.Drawing.Point(472, 41);
            this.overviewMeasurementPanel.Margin = new System.Windows.Forms.Padding(4);
            this.overviewMeasurementPanel.Name = "overviewMeasurementPanel";
            this.overviewMeasurementPanel.Size = new System.Drawing.Size(392, 340);
            this.overviewMeasurementPanel.TabIndex = 5;
            // 
            // overviewMeasurementIlPanel
            // 
            this.overviewMeasurementIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewMeasurementIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewMeasurementIlPanel.Editor = null;
            this.overviewMeasurementIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewMeasurementIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overviewMeasurementIlPanel.Name = "overviewMeasurementIlPanel";
            this.overviewMeasurementIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewMeasurementIlPanel.Rectangle")));
            this.overviewMeasurementIlPanel.ShowUIControls = false;
            this.overviewMeasurementIlPanel.Size = new System.Drawing.Size(392, 340);
            this.overviewMeasurementIlPanel.TabIndex = 0;
            // 
            // overviewPerformancePanel
            // 
            this.overviewPerformancePanel.Controls.Add(this.overviewPerformanceIlPanel);
            this.overviewPerformancePanel.Location = new System.Drawing.Point(12, 41);
            this.overviewPerformancePanel.Margin = new System.Windows.Forms.Padding(4);
            this.overviewPerformancePanel.Name = "overviewPerformancePanel";
            this.overviewPerformancePanel.Size = new System.Drawing.Size(392, 340);
            this.overviewPerformancePanel.TabIndex = 4;
            // 
            // overviewPerformanceIlPanel
            // 
            this.overviewPerformanceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overviewPerformanceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.overviewPerformanceIlPanel.Editor = null;
            this.overviewPerformanceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.overviewPerformanceIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.overviewPerformanceIlPanel.Name = "overviewPerformanceIlPanel";
            this.overviewPerformanceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("overviewPerformanceIlPanel.Rectangle")));
            this.overviewPerformanceIlPanel.ShowUIControls = false;
            this.overviewPerformanceIlPanel.Size = new System.Drawing.Size(392, 340);
            this.overviewPerformanceIlPanel.TabIndex = 0;
            // 
            // relativeDifferenceLabel
            // 
            this.relativeDifferenceLabel.AutoSize = true;
            this.relativeDifferenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.relativeDifferenceLabel.Location = new System.Drawing.Point(468, 393);
            this.relativeDifferenceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.relativeDifferenceLabel.Name = "relativeDifferenceLabel";
            this.relativeDifferenceLabel.Size = new System.Drawing.Size(147, 17);
            this.relativeDifferenceLabel.TabIndex = 3;
            this.relativeDifferenceLabel.Text = "Relative Difference";
            // 
            // absoluteDifferenceLabel
            // 
            this.absoluteDifferenceLabel.AutoSize = true;
            this.absoluteDifferenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.absoluteDifferenceLabel.Location = new System.Drawing.Point(8, 393);
            this.absoluteDifferenceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.absoluteDifferenceLabel.Name = "absoluteDifferenceLabel";
            this.absoluteDifferenceLabel.Size = new System.Drawing.Size(151, 17);
            this.absoluteDifferenceLabel.TabIndex = 2;
            this.absoluteDifferenceLabel.Text = "Absolute Difference";
            // 
            // measurementsLabel
            // 
            this.measurementsLabel.AutoSize = true;
            this.measurementsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.measurementsLabel.Location = new System.Drawing.Point(468, 16);
            this.measurementsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.measurementsLabel.Name = "measurementsLabel";
            this.measurementsLabel.Size = new System.Drawing.Size(113, 17);
            this.measurementsLabel.TabIndex = 1;
            this.measurementsLabel.Text = "Measurements";
            // 
            // calculatedPerformancesLabel
            // 
            this.calculatedPerformancesLabel.AutoSize = true;
            this.calculatedPerformancesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calculatedPerformancesLabel.Location = new System.Drawing.Point(8, 16);
            this.calculatedPerformancesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.calculatedPerformancesLabel.Name = "calculatedPerformancesLabel";
            this.calculatedPerformancesLabel.Size = new System.Drawing.Size(189, 17);
            this.calculatedPerformancesLabel.TabIndex = 0;
            this.calculatedPerformancesLabel.Text = "Calculated Performances";
            // 
            // relativeDifferencePanel
            // 
            this.relativeDifferencePanel.Controls.Add(this.relativeDifferenceIlPanel);
            this.relativeDifferencePanel.Location = new System.Drawing.Point(0, 37);
            this.relativeDifferencePanel.Margin = new System.Windows.Forms.Padding(4);
            this.relativeDifferencePanel.Name = "relativeDifferencePanel";
            this.relativeDifferencePanel.Size = new System.Drawing.Size(875, 818);
            this.relativeDifferencePanel.TabIndex = 7;
            this.relativeDifferencePanel.Visible = false;
            // 
            // relativeDifferenceIlPanel
            // 
            this.relativeDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.relativeDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.relativeDifferenceIlPanel.Editor = null;
            this.relativeDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.relativeDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.relativeDifferenceIlPanel.Name = "relativeDifferenceIlPanel";
            this.relativeDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("relativeDifferenceIlPanel.Rectangle")));
            this.relativeDifferenceIlPanel.ShowUIControls = false;
            this.relativeDifferenceIlPanel.Size = new System.Drawing.Size(875, 818);
            this.relativeDifferenceIlPanel.TabIndex = 0;
            // 
            // measurementPointLabel
            // 
            this.measurementPointLabel.AutoSize = true;
            this.measurementPointLabel.ForeColor = System.Drawing.Color.Green;
            this.measurementPointLabel.Location = new System.Drawing.Point(4, 859);
            this.measurementPointLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.measurementPointLabel.Name = "measurementPointLabel";
            this.measurementPointLabel.Size = new System.Drawing.Size(46, 17);
            this.measurementPointLabel.TabIndex = 1;
            this.measurementPointLabel.Text = "label8";
            this.measurementPointLabel.Visible = false;
            // 
            // absoluteDifferencePanel
            // 
            this.absoluteDifferencePanel.Controls.Add(this.absoluteDifferenceIlPanel);
            this.absoluteDifferencePanel.Location = new System.Drawing.Point(0, 37);
            this.absoluteDifferencePanel.Margin = new System.Windows.Forms.Padding(4);
            this.absoluteDifferencePanel.Name = "absoluteDifferencePanel";
            this.absoluteDifferencePanel.Size = new System.Drawing.Size(875, 818);
            this.absoluteDifferencePanel.TabIndex = 6;
            this.absoluteDifferencePanel.Visible = false;
            // 
            // absoluteDifferenceIlPanel
            // 
            this.absoluteDifferenceIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.absoluteDifferenceIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.absoluteDifferenceIlPanel.Editor = null;
            this.absoluteDifferenceIlPanel.Location = new System.Drawing.Point(0, 0);
            this.absoluteDifferenceIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.absoluteDifferenceIlPanel.Name = "absoluteDifferenceIlPanel";
            this.absoluteDifferenceIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("absoluteDifferenceIlPanel.Rectangle")));
            this.absoluteDifferenceIlPanel.ShowUIControls = false;
            this.absoluteDifferenceIlPanel.Size = new System.Drawing.Size(875, 818);
            this.absoluteDifferenceIlPanel.TabIndex = 0;
            // 
            // measurementsOnlyPanel
            // 
            this.measurementsOnlyPanel.Controls.Add(this.measurementsOnlyIlPanel);
            this.measurementsOnlyPanel.Location = new System.Drawing.Point(0, 37);
            this.measurementsOnlyPanel.Margin = new System.Windows.Forms.Padding(4);
            this.measurementsOnlyPanel.Name = "measurementsOnlyPanel";
            this.measurementsOnlyPanel.Size = new System.Drawing.Size(875, 818);
            this.measurementsOnlyPanel.TabIndex = 5;
            this.measurementsOnlyPanel.Visible = false;
            // 
            // measurementsOnlyIlPanel
            // 
            this.measurementsOnlyIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.measurementsOnlyIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.measurementsOnlyIlPanel.Editor = null;
            this.measurementsOnlyIlPanel.Location = new System.Drawing.Point(0, 0);
            this.measurementsOnlyIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.measurementsOnlyIlPanel.Name = "measurementsOnlyIlPanel";
            this.measurementsOnlyIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("measurementsOnlyIlPanel.Rectangle")));
            this.measurementsOnlyIlPanel.ShowUIControls = false;
            this.measurementsOnlyIlPanel.Size = new System.Drawing.Size(875, 818);
            this.measurementsOnlyIlPanel.TabIndex = 0;
            // 
            // measurementViewCombobox
            // 
            this.measurementViewCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measurementViewCombobox.Enabled = false;
            this.measurementViewCombobox.FormattingEnabled = true;
            this.measurementViewCombobox.Location = new System.Drawing.Point(511, 4);
            this.measurementViewCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.measurementViewCombobox.Name = "measurementViewCombobox";
            this.measurementViewCombobox.Size = new System.Drawing.Size(163, 24);
            this.measurementViewCombobox.TabIndex = 4;
            this.toolTip.SetToolTip(this.measurementViewCombobox, "Selected view of measurements");
            this.measurementViewCombobox.SelectedIndexChanged += new System.EventHandler(this.measurementViewCombobox_SelectedIndexChanged);
            // 
            // measurementErrorLabel
            // 
            this.measurementErrorLabel.AutoSize = true;
            this.measurementErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.measurementErrorLabel.Location = new System.Drawing.Point(173, 7);
            this.measurementErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.measurementErrorLabel.Name = "measurementErrorLabel";
            this.measurementErrorLabel.Size = new System.Drawing.Size(221, 17);
            this.measurementErrorLabel.TabIndex = 3;
            this.measurementErrorLabel.Text = "Please load some measurements.";
            // 
            // bothGraphsPanel
            // 
            this.bothGraphsPanel.Controls.Add(this.bothGraphsIlPanel);
            this.bothGraphsPanel.Location = new System.Drawing.Point(0, 37);
            this.bothGraphsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.bothGraphsPanel.Name = "bothGraphsPanel";
            this.bothGraphsPanel.Size = new System.Drawing.Size(875, 818);
            this.bothGraphsPanel.TabIndex = 2;
            this.bothGraphsPanel.TabStop = true;
            // 
            // bothGraphsIlPanel
            // 
            this.bothGraphsIlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bothGraphsIlPanel.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.bothGraphsIlPanel.Editor = null;
            this.bothGraphsIlPanel.Location = new System.Drawing.Point(0, 0);
            this.bothGraphsIlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bothGraphsIlPanel.Name = "bothGraphsIlPanel";
            this.bothGraphsIlPanel.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("bothGraphsIlPanel.Rectangle")));
            this.bothGraphsIlPanel.ShowUIControls = false;
            this.bothGraphsIlPanel.Size = new System.Drawing.Size(875, 818);
            this.bothGraphsIlPanel.TabIndex = 0;
            // 
            // nfpValueCombobox
            // 
            this.nfpValueCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nfpValueCombobox.Enabled = false;
            this.nfpValueCombobox.FormattingEnabled = true;
            this.nfpValueCombobox.Location = new System.Drawing.Point(683, 4);
            this.nfpValueCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.nfpValueCombobox.Name = "nfpValueCombobox";
            this.nfpValueCombobox.Size = new System.Drawing.Size(181, 24);
            this.nfpValueCombobox.TabIndex = 1;
            this.toolTip.SetToolTip(this.nfpValueCombobox, "Selected shown measurement value");
            this.nfpValueCombobox.SelectedIndexChanged += new System.EventHandler(this.nfpValueCombobox_SelectedIndexChanged);
            // 
            // loadMeasurementButton
            // 
            this.loadMeasurementButton.Location = new System.Drawing.Point(8, 1);
            this.loadMeasurementButton.Margin = new System.Windows.Forms.Padding(4);
            this.loadMeasurementButton.Name = "loadMeasurementButton";
            this.loadMeasurementButton.Size = new System.Drawing.Size(157, 28);
            this.loadMeasurementButton.TabIndex = 0;
            this.loadMeasurementButton.Text = "Load measurements";
            this.toolTip.SetToolTip(this.loadMeasurementButton, "Loads valid measurements");
            this.loadMeasurementButton.UseVisualStyleBackColor = true;
            this.loadMeasurementButton.Click += new System.EventHandler(this.loadMeasurementButton_Click);
            // 
            // helpTabPage
            // 
            this.helpTabPage.Controls.Add(this.helpTextBox);
            this.helpTabPage.Location = new System.Drawing.Point(4, 25);
            this.helpTabPage.Margin = new System.Windows.Forms.Padding(4);
            this.helpTabPage.Name = "helpTabPage";
            this.helpTabPage.Padding = new System.Windows.Forms.Padding(4);
            this.helpTabPage.Size = new System.Drawing.Size(877, 884);
            this.helpTabPage.TabIndex = 4;
            this.helpTabPage.Text = "Help";
            this.helpTabPage.UseVisualStyleBackColor = true;
            // 
            // helpTextBox
            // 
            this.helpTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.helpTextBox.Location = new System.Drawing.Point(0, 0);
            this.helpTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.helpTextBox.Name = "helpTextBox";
            this.helpTextBox.ReadOnly = true;
            this.helpTextBox.Size = new System.Drawing.Size(873, 880);
            this.helpTextBox.TabIndex = 0;
            this.helpTextBox.Text = "";
            // 
            // vipeTab
            // 
            this.vipeTab.Controls.Add(this.vipeSettingsPanel);
            this.vipeTab.Location = new System.Drawing.Point(4, 25);
            this.vipeTab.Name = "vipeTab";
            this.vipeTab.Padding = new System.Windows.Forms.Padding(3);
            this.vipeTab.Size = new System.Drawing.Size(877, 884);
            this.vipeTab.TabIndex = 5;
            this.vipeTab.Text = "ViPe";
            this.vipeTab.UseVisualStyleBackColor = true;
            // 
            // vipeSettingsPanel
            // 
            this.vipeSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vipeSettingsPanel.Controls.Add(this.plotButton);
            this.vipeSettingsPanel.Controls.Add(this.dataPanel);
            this.vipeSettingsPanel.Controls.Add(this.vipeRSettings);
            this.vipeSettingsPanel.Location = new System.Drawing.Point(3, 6);
            this.vipeSettingsPanel.Name = "vipeSettingsPanel";
            this.vipeSettingsPanel.Size = new System.Drawing.Size(871, 163);
            this.vipeSettingsPanel.TabIndex = 1;
            // 
            // plotButton
            // 
            this.plotButton.Location = new System.Drawing.Point(744, 101);
            this.plotButton.Name = "plotButton";
            this.plotButton.Size = new System.Drawing.Size(119, 57);
            this.plotButton.TabIndex = 2;
            this.plotButton.Text = "Plot";
            this.plotButton.UseVisualStyleBackColor = true;
            this.plotButton.Click += new System.EventHandler(this.plotButton_Click);
            // 
            // dataPanel
            // 
            this.dataPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataPanel.Controls.Add(this.data2Button);
            this.dataPanel.Controls.Add(this.data1Button);
            this.dataPanel.Controls.Add(this.data2TextBox);
            this.dataPanel.Controls.Add(this.data1TextBox);
            this.dataPanel.Controls.Add(this.data2Label);
            this.dataPanel.Controls.Add(this.data1Label);
            this.dataPanel.Location = new System.Drawing.Point(490, 3);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(373, 92);
            this.dataPanel.TabIndex = 1;
            // 
            // data2Button
            // 
            this.data2Button.Location = new System.Drawing.Point(292, 33);
            this.data2Button.Name = "data2Button";
            this.data2Button.Size = new System.Drawing.Size(75, 26);
            this.data2Button.TabIndex = 5;
            this.data2Button.Text = "set";
            this.data2Button.UseVisualStyleBackColor = true;
            this.data2Button.Click += new System.EventHandler(this.data2Button_Click);
            // 
            // data1Button
            // 
            this.data1Button.Location = new System.Drawing.Point(293, 1);
            this.data1Button.Name = "data1Button";
            this.data1Button.Size = new System.Drawing.Size(75, 26);
            this.data1Button.TabIndex = 4;
            this.data1Button.Text = "set";
            this.data1Button.UseVisualStyleBackColor = true;
            this.data1Button.Click += new System.EventHandler(this.data1Button_Click);
            // 
            // data2TextBox
            // 
            this.data2TextBox.Location = new System.Drawing.Point(53, 32);
            this.data2TextBox.Name = "data2TextBox";
            this.data2TextBox.Size = new System.Drawing.Size(233, 22);
            this.data2TextBox.TabIndex = 3;
            // 
            // data1TextBox
            // 
            this.data1TextBox.Location = new System.Drawing.Point(53, 3);
            this.data1TextBox.Name = "data1TextBox";
            this.data1TextBox.Size = new System.Drawing.Size(233, 22);
            this.data1TextBox.TabIndex = 2;
            // 
            // data2Label
            // 
            this.data2Label.AutoSize = true;
            this.data2Label.Location = new System.Drawing.Point(5, 32);
            this.data2Label.Name = "data2Label";
            this.data2Label.Size = new System.Drawing.Size(42, 17);
            this.data2Label.TabIndex = 1;
            this.data2Label.Text = "Data:";
            // 
            // data1Label
            // 
            this.data1Label.AutoSize = true;
            this.data1Label.Location = new System.Drawing.Point(5, 4);
            this.data1Label.Name = "data1Label";
            this.data1Label.Size = new System.Drawing.Size(42, 17);
            this.data1Label.TabIndex = 0;
            this.data1Label.Text = "Data:";
            // 
            // vipeRSettings
            // 
            this.vipeRSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vipeRSettings.Controls.Add(this.initializationCheckBox);
            this.vipeRSettings.Controls.Add(this.pathToExeButton);
            this.vipeRSettings.Controls.Add(this.pathToLibButton);
            this.vipeRSettings.Controls.Add(this.pathToRExe);
            this.vipeRSettings.Controls.Add(this.performInitialization);
            this.vipeRSettings.Controls.Add(this.pathToRExeLabel);
            this.vipeRSettings.Controls.Add(this.pathToRLibLabel);
            this.vipeRSettings.Controls.Add(this.pathToRLib);
            this.vipeRSettings.Location = new System.Drawing.Point(3, 3);
            this.vipeRSettings.Name = "vipeRSettings";
            this.vipeRSettings.Size = new System.Drawing.Size(480, 92);
            this.vipeRSettings.TabIndex = 0;
            // 
            // initializationCheckBox
            // 
            this.initializationCheckBox.AutoSize = true;
            this.initializationCheckBox.Location = new System.Drawing.Point(201, 59);
            this.initializationCheckBox.Name = "initializationCheckBox";
            this.initializationCheckBox.Size = new System.Drawing.Size(18, 17);
            this.initializationCheckBox.TabIndex = 8;
            this.initializationCheckBox.UseVisualStyleBackColor = true;
            this.initializationCheckBox.CheckedChanged += new System.EventHandler(this.initializationCheckBox_CheckedChanged);
            // 
            // pathToExeButton
            // 
            this.pathToExeButton.Location = new System.Drawing.Point(379, 29);
            this.pathToExeButton.Enabled = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToExeButton.Visible = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToExeButton.Name = "pathToExeButton";
            this.pathToExeButton.Size = new System.Drawing.Size(75, 26);
            this.pathToExeButton.TabIndex = 7;
            this.pathToExeButton.Text = "set";
            this.pathToExeButton.UseVisualStyleBackColor = true;
            this.pathToExeButton.Click += new System.EventHandler(this.pathToExeButton_Click);
            // 
            // pathToLibButton
            // 
            this.pathToLibButton.Location = new System.Drawing.Point(379, 1);
            this.pathToLibButton.Name = "pathToLibButton";
            this.pathToLibButton.Size = new System.Drawing.Size(75, 26);
            this.pathToLibButton.TabIndex = 6;
            this.pathToLibButton.Text = "set";
            this.pathToLibButton.UseVisualStyleBackColor = true;
            this.pathToLibButton.Click += new System.EventHandler(this.pathToLibButton_Click);
            // 
            // pathToRExe
            // 
            this.pathToRExe.Location = new System.Drawing.Point(201, 31);
            this.pathToRExe.Name = "pathToRExe";
            this.pathToRExe.ReadOnly = true;
            this.pathToRExe.Enabled = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToRExe.Visible = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToRExe.Size = new System.Drawing.Size(172, 22);
            this.pathToRExe.TabIndex = 4;
            // 
            // performInitialization
            // 
            this.performInitialization.AutoSize = true;
            this.performInitialization.Location = new System.Drawing.Point(0, 60);
            this.performInitialization.Name = "performInitialization";
            this.performInitialization.Size = new System.Drawing.Size(176, 17);
            this.performInitialization.TabIndex = 3;
            this.performInitialization.Text = "Perform initialization tasks:";
            // 
            // pathToRExeLabel
            // 
            this.pathToRExeLabel.AutoSize = true;
            this.pathToRExeLabel.Location = new System.Drawing.Point(0, 32);
            this.pathToRExeLabel.Name = "pathToRExeLabel";
            this.pathToRExeLabel.Enabled = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToRExeLabel.Visible = System.Environment.OSVersion.ToString().Contains("Windows");
            this.pathToRExeLabel.Size = new System.Drawing.Size(135, 17);
            this.pathToRExeLabel.TabIndex = 2;
            this.pathToRExeLabel.Text = "Path to R execution:";
            // 
            // pathToRLibLabel
            // 
            this.pathToRLibLabel.AutoSize = true;
            this.pathToRLibLabel.Location = new System.Drawing.Point(0, 3);
            this.pathToRLibLabel.Name = "pathToRLibLabel";
            this.pathToRLibLabel.Size = new System.Drawing.Size(173, 17);
            this.pathToRLibLabel.TabIndex = 1;
            this.pathToRLibLabel.Text = "Path to R library directory:";
            // 
            // pathToRLib
            // 
            this.pathToRLib.Location = new System.Drawing.Point(201, 3);
            this.pathToRLib.Name = "pathToRLib";
            this.pathToRLib.ReadOnly = true;
            this.pathToRLib.Size = new System.Drawing.Size(172, 22);
            this.pathToRLib.TabIndex = 0;
            // 
            // constraintTextbox
            // 
            this.constraintTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.constraintTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constraintTextbox.Location = new System.Drawing.Point(8, 23);
            this.constraintTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.constraintTextbox.Name = "constraintTextbox";
            this.constraintTextbox.ReadOnly = true;
            this.constraintTextbox.Size = new System.Drawing.Size(400, 111);
            this.constraintTextbox.TabIndex = 12;
            this.constraintTextbox.Text = "";
            // 
            // factorizationSettingsButton
            // 
            this.factorizationSettingsButton.Enabled = false;
            this.factorizationSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.factorizationSettingsButton.Location = new System.Drawing.Point(145, 517);
            this.factorizationSettingsButton.Margin = new System.Windows.Forms.Padding(4);
            this.factorizationSettingsButton.Name = "factorizationSettingsButton";
            this.factorizationSettingsButton.Size = new System.Drawing.Size(160, 28);
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
            this.adjustedFunctionGroupBox.Location = new System.Drawing.Point(1340, 10);
            this.adjustedFunctionGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.adjustedFunctionGroupBox.Name = "adjustedFunctionGroupBox";
            this.adjustedFunctionGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.adjustedFunctionGroupBox.Size = new System.Drawing.Size(421, 560);
            this.adjustedFunctionGroupBox.TabIndex = 19;
            this.adjustedFunctionGroupBox.TabStop = false;
            this.adjustedFunctionGroupBox.Text = "Adjusted function";
            // 
            // resetFactorizationButton
            // 
            this.resetFactorizationButton.Enabled = false;
            this.resetFactorizationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetFactorizationButton.Location = new System.Drawing.Point(313, 517);
            this.resetFactorizationButton.Margin = new System.Windows.Forms.Padding(4);
            this.resetFactorizationButton.Name = "resetFactorizationButton";
            this.resetFactorizationButton.Size = new System.Drawing.Size(96, 28);
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
            this.constraintsGroupBox.Location = new System.Drawing.Point(13, 768);
            this.constraintsGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.constraintsGroupBox.Name = "constraintsGroupBox";
            this.constraintsGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.constraintsGroupBox.Size = new System.Drawing.Size(421, 146);
            this.constraintsGroupBox.TabIndex = 20;
            this.constraintsGroupBox.TabStop = false;
            this.constraintsGroupBox.Text = "Constraints";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1767, 918);
            this.Controls.Add(this.constraintsGroupBox);
            this.Controls.Add(this.adjustedFunctionGroupBox);
            this.Controls.Add(this.evaluationGroupBox);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.variableConfigurationGroupBox);
            this.Controls.Add(this.constantConfigurationGroupBox);
            this.Controls.Add(this.functionGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
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
            this.chartPanel.ResumeLayout(false);
            this.chartPanel.PerformLayout();
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
            this.vipeTab.ResumeLayout(false);
            this.vipeSettingsPanel.ResumeLayout(false);
            this.dataPanel.ResumeLayout(false);
            this.dataPanel.PerformLayout();
            this.vipeRSettings.ResumeLayout(false);
            this.vipeRSettings.PerformLayout();
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
        private System.Windows.Forms.TabPage measurementsTabPage;
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
        private System.Windows.Forms.Panel chartPanel;
        private OxyPlot.WindowsForms.PlotView interactionChartRepl;
        private OxyPlot.WindowsForms.PlotView constantChartRepl;
        private OxyPlot.WindowsForms.PlotView maxChartRepl;
        private OxyPlot.WindowsForms.PlotView maxOccChartRepl;
        private OxyPlot.WindowsForms.PlotView rangeChartRepl;
        private System.Windows.Forms.TabPage vipeTab;
        private System.Windows.Forms.Panel vipeSettingsPanel;
        private System.Windows.Forms.Panel vipeRSettings;
        private System.Windows.Forms.Label pathToRLibLabel;
        private System.Windows.Forms.TextBox pathToRLib;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.Button data2Button;
        private System.Windows.Forms.Button data1Button;
        private System.Windows.Forms.TextBox data2TextBox;
        private System.Windows.Forms.TextBox data1TextBox;
        private System.Windows.Forms.Label data2Label;
        private System.Windows.Forms.Label data1Label;
        private System.Windows.Forms.Button pathToExeButton;
        private System.Windows.Forms.Button pathToLibButton;
        private System.Windows.Forms.TextBox pathToRExe;
        private System.Windows.Forms.Label performInitialization;
        private System.Windows.Forms.Label pathToRExeLabel;
        private System.Windows.Forms.CheckBox initializationCheckBox;
        private System.Windows.Forms.Button plotButton;
    }
}