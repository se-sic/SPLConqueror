using CommandLine;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SPLConqueror_GUI
{
    public partial class MainWindow : Form
    {
        // All string constants for this class
        private const string ABSOLUTE_DIFFERENCE_LABEL = "Absolute Difference";
        private const string BRACKET_TAB = "     ";
        private const string BUTTON_PRESS_REQUEST = "Please press the button.";
        private const string COMBOBOX_INTERACTIONS_OPTION = "Interactions";
        private const string COMBOBOX_CONSTANT_OPTION = "Constants";
        private const string COMBOBOX_MAX_OPTION = "Constants Max";
        private const string COMBOBOX_MAX_OCCURANCE_OPTION = "Constants Max Occurance";
        private const string COMBOBOX_RANGE_OPTION = "Variable Range";
        private const string COMBOBOX_OVERVIEW_OPTION = "Overview";
        private const string COMBOBOX_BOTH_OPTION = "Both Graphs";
        private const string COMBOBOX_MEASUREMENTS_OPTION = "Measurements only";
        private const string COMBOBOX_ABSOLUTE_DIFFERENCE_OPTION = "Absolute Difference";
        private const string COMBOBOX_RELATIVE_DIFFERENCE_OPTION = "Relative Difference";
        private const string CONSTANT_INFORMATION = "What is the abstract influence of each variable?";
        private const string CORRESPONDING_VALUES_LABEL = "Corresponding values";
        private const string DLL_LOCATION = "\\dll\\Microsoft.Solver.Foundation.dll";
        private const string ERROR_EXP_MODEL_INCOMPATIBLE = "The read expression does not work with the loaded variability model!";
        private const string ERROR_INVALID_MODEL = "The entered variability model is not valid.";
        private const string ERROR_INVALID_EXP = "The read expression is in an invalid form.";
        private const string ERROR_NO_MODEL = "You have to load a model to generate a graph!";
        private const string ERROR_NO_MODEL_LOADED = "Please load a function with its variability model first!";
        private const string ERROR_MEASUREMENTS_INCOMPATIBLE = "The file doesn't match with the current variability model!";
        private const string ERROR_DOUBLE_OPTION = "You may not choose the same option twice!";
        private const string ERROR_NO_MEASUREMENTS_LOADED = "Please load some measurements.";
        private const string ERROR_NO_MEASUREMENTS_NFP = "There are no measurements for this nfp value.";
        private const string ERROR_NO_PERFORMANCES = "Please calculate a graph first.";
        private const string ERROR_ILLEGAL_CONFIGURATION = "Invalid configuration. Check your constraints too.";
        private const string ERROR_NO_MEASUREMENTS_AVAILABLE = "There are no measurements for the current settings.";
        private const string FILTERING_LIST_BOX = "Free filtering";
        private const string FILTERING_TREE_VIEW = "Configuration filtering";
        private const string INTERACTION_INFORMATION = "In how many interactions do the possible variables occur?";
        private const string MAX_INFORMATION = "What is the maximum abstract influence of each variable?";
        private const string MAX_OCCURANCE_INFORMATION = "What is the maximum abstract influence of each variable depended on its usage in the configurations?";
        private const string MEASURED_VALUE_LABEL = "Measured Values";
        private const string PERFORMANCE_AXIS_LABEL = "Calculated Performance";
        private const string RANGE_INFORMATION = "What is the influence range of each variable?";
        private const string RELATIVE_DIFFERENCE_LABEL = "Relative Difference in %";
        private const string SECOND_EMPTY_OPTION = "---------------";

        // Colors for the Configuration filtering
        private Color normalColor = Color.White;
        private Color deactivatedColor = Color.SlateGray;

        private InfluenceFunction originalFunction;
        private VariabilityModel currentModel;
        private string[] adjustedExpressionTree;
        private bool modelLoaded = false;
        private double maxAbstractConstant = 0.0;
        private Dictionary<string, int> occuranceOfOptions = new Dictionary<string, int>();
        protected Dictionary<NumericOption, float> numericSettings = new Dictionary<NumericOption, float>();
        private Dictionary<ConfigurationOption, double> factorizationPriorities =
            new Dictionary<ConfigurationOption, double>();
        private MachineLearning.Solver.ICheckConfigSAT sat = new MachineLearning.Solver.CheckConfigSAT(
            Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
            System.IO.Directory.GetCurrentDirectory()))) + DLL_LOCATION);
        private MachineLearning.Solver.VariantGenerator varGen = new MachineLearning.Solver.VariantGenerator(
                Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))
                + DLL_LOCATION);

        // Everything for the measurements
        private bool measurementsLoaded = false;
        private string adjustedMeasurementFunction;
        private Configuration configurationForCalculation;
        private Tuple<NumericOption, NumericOption> chosenOptions;
        private ILArray<float> drawnPerformances;
        private ILArray<float> calculatedPerformances;
        private Color calculatedColor = Color.Red;
        private Color measurementColor = Color.Green;

        /// <summary>
        /// Constructor of this class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            initializeHelp();
        }

        private void initializeHelp()
        {
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Function:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("The currently loaded expression will be displayed here. The constants will be"
                + " a color from green to red.The color depends on the value of the maximum constant in this"
                + " expression.With the 'Load' - Button you are able to load in your expression AND your"
                + " variability model which have to be compatible with each other. The 'Load Expression Only'"
                + "- Button allows you to load in an expression only, but you won't be able to generate any"
                + " graphs.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Constant configuration:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("All constants of the loaded expression can be configured here. By checking"
                + " the first checkbox you are able to determine the amount of digits after a comma. By checking"
                + " the second checkbox you can filter the constants of the expression such that only a certain"
                + " percentage of constants will be shown depending on the value of the slider next to the"
                + " checkbox. If the slider is on the far left, there will be no filtering. A slider with a"
                + " maximum value will only show the component with the maximum constant of the current"
                + " expression.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Variable configuration:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("You are able to filter certain variables out of the expression. If the"
                + " checkbox is checked, the filtering is activated. By using the combobox you can choose which"
                + " filtering strategy you want to use. The 'Free filtering' - Option every variable is"
                + " selectable and all checked variables will be thrown out of the expression. The 'Configuration"
                + " filtering' - Option helps you to filter only valid (partial)configurations. All selectable"
                + " variables appear white, while unselectable variables appear gray. The other option for"
                + " filtering is by using the regex filtering below. Every expression in the regex will be used"
                + " for the filtering of the adjusted expression. These expression have to be separated by using"
                + " \"|\", \";\" or \",\".\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Constraints:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("All constraints which are used in the loaded variability model will be"
                + " shown here.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Adjusted function:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("After configurating theoriginal expression, the adjusted expression will be"
                + " shown here in a simplyfied form. Like before all constants will get a color from green to red."
                + " By switching between the radio buttons below you can determine if the adjusted expression"
                + " should be in a factorized form. The factorization priority of each variable can be set by"
                + " using the 'Factorization Settings' - Button. All priorities can be reset with the 'Reset'"
                + " - Buton next to it.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Evaluation configuration:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("This configuration depends on the amount of numeric options in the adjusted"
                + " expression. If there are no numeric options in the expression, you will not be able to"
                + " calculate any graph. But you can calculate the performce of the current configuration. All"
                + " variables in the adjusted expression (only binary options) will be handled as selected"
                + " options and will get the value 1. If there is at least one numeric option in the expression,"
                + " you are able to calculate a graph. You can choose which numeric options will be displayed in"
                + " the graph. All other numeric options will be set on their default value. You are able to set"
                + " this value by using the panels below. All numeric options will be shown and you can choose"
                + " which value this option should have. By clicking on the 'Generate Function' - Button you will"
                + " generate the graph of the adjusted function. If there are any problems with your input, a red"
                + " text will show you what is wrong.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Function Graph:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("If a graph was calculated, the result will be shown here. By hovering over"
                + " the black points in the graph a green text will display the values of this point.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Interactions and Influences:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("This will display all interactions which occur in the adjusted expression."
                + " Below the textbox a pie chart will show the influences of each option in the adjusted"
                + " expression. The values in the pie chart depend on the selected option on the right.\n\n");

            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Bold);
            helpTextBox.AppendText("Difference with other measurements:\n");
            helpTextBox.SelectionFont = new Font(helpTextBox.Font, FontStyle.Regular);
            helpTextBox.AppendText("You are able to load in your own measurements and compare it with the"
                + " calculated graph. To do that you need measurements which are compatible with the loaded"
                + " variability model and a calculated graph which used a valid configuration. The graph of"
                + " the measurements is displayed green, while the calculated graph will appear the same way"
                + " as calculated. If there are no measurements for the current configuration, no graph will"
                + " be shown for these settings. The first combobox will let you choose what exactly you want"
                + " to see. You can display an overview, both graphs, the measurements only or the certain"
                + " differences between the graphs. Using the second combobox you can choose which measurement"
                + " value should be shown in the graph.");
        }

        /// <summary>
        /// Opens a dialog to load the desired expression from a txt-file.
        /// </summary>
        /// <returns>The loaded expression of the function</returns>
        private string loadExpression()
        {
            String expression = "";
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(dialog.FileName);
                expression = System.IO.File.ReadAllText(fi.FullName);

                string[] parts = expression.Split(' ');

                for (int i = 0; i < parts.Length; i++)
                    parts[i] = parts[i].Replace(',', '.');

                if (parts.Length > 1 && !isOperator(parts[1]) && parts[0] != "log10(")
                    expression = String.Join(" ", parts, 1, parts.Length - 1);
                else
                    expression = String.Join(" ", parts, 0, parts.Length);
            }

            return expression;
        }

        /// <summary>
        /// Opens a dialog to load the desired variability model.
        /// 
        /// If the user enters an invalid XML-file that is not a variability model, an error message will be shown.
        /// </summary>
        /// <returns>The loaded variability model of the previously loaded expression</returns>
        private VariabilityModel loadVariabilityModel()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string filePath = "";

            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(dialog.FileName);
                filePath = fi.FullName;
            }

            if (filePath != "")
            {
                try
                {
                    return VariabilityModel.loadFromXML(filePath);
                }
                catch
                {
                    MessageBox.Show(ERROR_INVALID_MODEL);
                    return null;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Invokes if the loadButton has been pressed.
        /// 
        /// Two windows will be opened to load the expression and afterwards the corresponding
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void loadButton_Click(object sender, EventArgs e)
        {
            string expression = loadExpression();

            if (expression == "")
                return;

            VariabilityModel varModel = loadVariabilityModel();

            if (varModel == null)
                return;

            loadComponents(expression, varModel);
        }

        /// <summary>
        /// Invokes if the loadExpOnlyButton has been pressed.
        /// 
        /// One window will be opened to load the expression.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void loadExpOnlyButton_Click(object sender, EventArgs e)
        {
            string expression = loadExpression();

            if (expression == "")
                return;

            loadComponents(expression, null);
        }

        /// <summary>
        /// Invokes if the loadMeasurementButton has been pressed.
        /// 
        /// A window will be opened to load the measurements. If the XML-file does not have the
        /// appropriate form or does not fit with the current variability model, an error message
        /// will be displayed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void loadMeasurementButton_Click(object sender, EventArgs e)
        {
            if (!modelLoaded)
            {
                MessageBox.Show(ERROR_NO_MODEL_LOADED);
                return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            string filePath = "";

            dialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(dialog.FileName);
                filePath = fi.FullName;
            }

            if (filePath == "")
                return;

            try
            {
                GlobalState.clear();
                GlobalState.varModel = currentModel;

                Commands co = new Commands();
                co.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + filePath);

                measurementsLoaded = true;
                nfpValueCombobox.Items.Clear();
            }
            catch
            {
                MessageBox.Show(ERROR_MEASUREMENTS_INCOMPATIBLE);
            }

            updateMeasurementTab();
        }

        /// <summary>
        /// Sets all necessary information and initializes all components.
        /// </summary>
        /// <param name="exp">Loaded expression of the function. Must not be null.</param>
        /// <param name="model">Corresponding variability model of the expression.</param>
        private void loadComponents(string exp, VariabilityModel model)
        {
            if (exp == null)
                throw new ArgumentNullException("Parameter exp may not be null!");
            if (model != null && !checkExpressionCompatibility(exp, model))
            {
                MessageBox.Show(ERROR_EXP_MODEL_INCOMPATIBLE);
                return;
            }

            String optExpression = Regex.Replace(exp, @"\r\n?|\n", "");

            // Test if the loaded expression can be used
            try
            {
                new InfluenceFunction(optExpression);
            } catch
            {
                MessageBox.Show(ERROR_INVALID_EXP);
                return;
            }
            
            if (originalFunction == null)
                initializeOnce();
            
            modelLoaded = model != null;
            originalFunction = modelLoaded ? new InfluenceFunction(optExpression, model) : new InfluenceFunction(optExpression);
            currentModel = originalFunction.getVariabilityModel();

            adjustedExpressionTree = new string[originalFunction.getExpressionTree().Length];

            for (int i = 0; i < adjustedExpressionTree.Length; i++)
                adjustedExpressionTree[i] = string.Copy(originalFunction.getExpressionTree()[i]);

            calculateOccurances();

            // Update readFunction-textbox
            getMaxAbstractConstant(originalFunction.ToString());
            updateFunctionTextBox(originalFunctionTextBox, sortExpression(originalFunction.ToString()));
            
            // Activating all components and returning their original state
            ilFunctionPanel.Scene = new ILScene();
            ilFunctionPanel.Refresh();
            
            initializeComponents();
            updateAdjustedFunction();
        }

        /// <summary>
        /// Checks if the specified expression is compatible with the specified model.
        /// </summary>
        /// <param name="exp">Specified expression. Must not be null.</param>
        /// <param name="model">Specified variability model. Must not be null.</param>
        /// <returns>Returns true if expression and model are compatible else false</returns>
        private bool checkExpressionCompatibility(string exp, VariabilityModel model)
        {
            if (exp == null)
                throw new ArgumentNullException("Parameter exp must not be null!");
            if (model == null)
                throw new ArgumentNullException("Parameter model must not be null!");

            foreach (string prt in exp.Split(' '))
            {
                double d;

                if (!isOperator(prt) && prt != "log10(" && prt != ")"
                    && !double.TryParse(prt, out d)
                    && model.getOption(prt) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates all possible variants of the current model and checks for each binary option
        /// in how many variants it is contained.
        /// </summary>
        private void calculateOccurances()
        {
            occuranceOfOptions.Clear();

            foreach (List<BinaryOption> variant in varGen.generateAllVariantsFast(currentModel))
            {
                foreach (BinaryOption opt in variant)
                {
                    int i = 0;

                    occuranceOfOptions.TryGetValue(opt.Name, out i);

                    occuranceOfOptions.Remove(opt.Name);
                    occuranceOfOptions.Add(opt.Name, i + 1);
                }
            }
        }

        /// <summary>
        /// Initializes all components that need to be initialized only once the programm started.
        /// </summary>
        private void initializeOnce()
        {
            constantDecimalCheckBox.Enabled = true;
            constantFilteringCheckbox.Enabled = true;
            filterVariablesCheckbox.Enabled = true;
            filterRegexCheckBox.Enabled = true;
            firstAxisCombobox.Enabled = true;
            secondAxisCombobox.Enabled = true;
            numericDefaultPanel.Enabled = true;
            normalRadioButton.Enabled = true;
            factorRadioButton.Enabled = true;
            chartDescriptionLabel.Visible = true;

            // ChartComboBox
            chartComboBox.Enabled = true;
            chartComboBox.Items.Add(COMBOBOX_INTERACTIONS_OPTION);
            chartComboBox.Items.Add(COMBOBOX_CONSTANT_OPTION);
            chartComboBox.Items.Add(COMBOBOX_MAX_OPTION);
            chartComboBox.Items.Add(COMBOBOX_MAX_OCCURANCE_OPTION);
            chartComboBox.Items.Add(COMBOBOX_RANGE_OPTION);

            // MeasurementViewCombobox
            measurementViewCombobox.Items.Add(COMBOBOX_OVERVIEW_OPTION);
            measurementViewCombobox.Items.Add(COMBOBOX_BOTH_OPTION);
            measurementViewCombobox.Items.Add(COMBOBOX_MEASUREMENTS_OPTION);
            measurementViewCombobox.Items.Add(COMBOBOX_ABSOLUTE_DIFFERENCE_OPTION);
            measurementViewCombobox.Items.Add(COMBOBOX_RELATIVE_DIFFERENCE_OPTION);
            measurementViewCombobox.SelectedIndex = 0;

            // Adding events to the variableTreeView
            variableTreeView.BeforeCheck += (o, e) => {
                if (e.Node.BackColor == deactivatedColor)
                    e.Cancel = true;
            };
            variableTreeView.AfterCheck += ownAfterCheck;
        }

        /// <summary>
        /// Initializes and sets all components to their original state.
        /// </summary>
        private void initializeComponents()
        {
            // Function configuration
            initializeFunctionConfiguration();

            // Constant configuration
            constantDecimalCheckBox.Checked = false;

            constantsDigitsUpDown.Enabled = false;
            constantsDigitsUpDown.Maximum = getMaxDigits();
            constantsDigitsUpDown.Value = constantsDigitsUpDown.Maximum;

            constantFilteringCheckbox.Checked = false;

            constantRelativeValueSlider.Enabled = false;
            constantRelativeValueSlider.Value = constantRelativeValueSlider.Minimum;

            // Variable configuration
            initializeConstraintView();
            initializeVariableConfiguration();

            // Initializing the interactions tab
            updateInteractionsTab();
            chartComboBox.SelectedIndex = 0;

            // Initializing the default settings of the numeric options
            numericSettings.Clear();

            foreach (NumericOption option in currentModel.NumericOptions)
                numericSettings.Add(option, (float) option.DefaultValue);

            // Evaluation configuration
            calculationResultLabel.Text = BUTTON_PRESS_REQUEST;
            updateEvaluationConfiguration();

            // Initializing the numeric default settings panel
            if (modelLoaded)
                initializeNumericPanels();

            // Measurement view
            initializeMeasurementView();
        }

        /// <summary>
        /// Initializes the panel containing all numeric panels.
        /// </summary>
        private void initializeNumericPanels()
        {
            numericDefaultPanel.Controls.Clear();

            int i = 0;

            foreach (NumericOption option in currentModel.NumericOptions)
            {
                float val = 0;
                numericSettings.TryGetValue(option, out val);

                NumericPanel panel = new NumericPanel(option, val);
                panel.upDown.ValueChanged += (s, e) =>
                {
                    numericSettings.Remove(panel.option);
                    numericSettings.Add(panel.option, (float)panel.upDown.Value);
                };

                numericDefaultPanel.Controls.Add(panel);

                panel.Location = new System.Drawing.Point(0, i * panel.Size.Height);
                i++;
            }
        }

        /// <summary>
        /// Initializes the measurement view.
        /// </summary>
        private void initializeMeasurementView()
        {
            chosenOptions = null;
            configurationForCalculation = null;
            drawnPerformances = null;
            calculatedPerformances = null;
            measurementsLoaded = false;
            nfpValueCombobox.Items.Clear();
            nfpValueCombobox.Enabled = false;
            updateMeasurementTab();

            overviewPerformanceIlPanel.Scene = new ILScene();
            overviewMeasurementIlPanel.Scene = new ILScene();
            overviewAbsoluteDifferenceIlPanel.Scene = new ILScene();
            overviewRelativeDifferenceIlPanel.Scene = new ILScene();
            bothGraphsIlPanel.Scene = new ILScene();
            measurementsOnlyIlPanel.Scene = new ILScene();
            absoluteDifferenceIlPanel.Scene = new ILScene();
            relativeDifferenceIlPanel.Scene = new ILScene();

            overviewPerformanceIlPanel.Refresh();
            overviewMeasurementIlPanel.Refresh();
            overviewAbsoluteDifferenceIlPanel.Refresh();
            overviewRelativeDifferenceIlPanel.Refresh();
            bothGraphsIlPanel.Refresh();
            measurementsOnlyIlPanel.Refresh();
            absoluteDifferenceIlPanel.Refresh();
            relativeDifferenceIlPanel.Refresh();
        }

        /// <summary>
        /// Initializes the constraint view with the current constraints.
        /// </summary>
        private void initializeConstraintView()
        {
            constraintTextbox.Clear();

            if (currentModel.BooleanConstraints.Count != 0)
            {
                constraintTextbox.SelectionFont = new Font(constraintTextbox.Font, FontStyle.Bold);
                constraintTextbox.AppendText("Boolean constraints:\n\n");
                constraintTextbox.SelectionFont = new Font(constraintTextbox.Font, FontStyle.Regular);

                foreach (string constraint in currentModel.BooleanConstraints)
                    constraintTextbox.AppendText(constraint + "\n");

                constraintTextbox.AppendText("\n");
            }

            if (currentModel.NonBooleanConstraints.Count != 0)
            {
                constraintTextbox.SelectionFont = new Font(constraintTextbox.Font, FontStyle.Bold);
                constraintTextbox.AppendText("Non-Boolean constraints:\n\n");
                constraintTextbox.SelectionFont = new Font(constraintTextbox.Font, FontStyle.Regular);

                foreach (NonBooleanConstraint constraint in currentModel.NonBooleanConstraints)
                    constraintTextbox.AppendText(constraint.ToString() + "\n");

                constraintTextbox.AppendText("\n");
            }

            if (constraintTextbox.Text == "")
                constraintTextbox.AppendText("No constraints for this model...");
        }

        /// <summary>
        /// Initializes all options to configurate the adjusted function.
        /// </summary>
        private void initializeFunctionConfiguration()
        {
            normalRadioButton.Checked = true;
            factorRadioButton.Checked = false;

            factorizationPriorities.Clear();

            foreach (ConfigurationOption option in originalFunction.participatingNumOptions)
                factorizationPriorities.Add(option, 1.0);

            foreach (ConfigurationOption option in originalFunction.participatingBoolOptions)
                factorizationPriorities.Add(option, 1.0);
        }

        /// <summary>
        /// Initializes the variable configuration.
        /// </summary>
        private void initializeVariableConfiguration()
        {
            filterVariablesCheckbox.Checked = false;

            // Initializing combobox
            filterOptionCombobox.Items.Clear();
            filterOptionCombobox.Items.Add(FILTERING_TREE_VIEW);
            filterOptionCombobox.Items.Add(FILTERING_LIST_BOX);
            filterOptionCombobox.SelectedIndex = 0;

            // Initializing listbox
            variableListBox.Visible = false;
            variableListBox.Enabled = false;
            variableListBox.Items.Clear();

            foreach (BinaryOption option in originalFunction.participatingBoolOptions)
                variableListBox.Items.Add(option.Name, false);

            foreach (NumericOption option in originalFunction.participatingNumOptions)
                variableListBox.Items.Add(option.Name, false);

            // Initializing tree view
            variableTreeView.Visible = true;
            variableTreeView.Enabled = false;
            variableTreeView.Nodes.Clear();
            
            variableTreeView.Nodes.Add(insertIntoTreeView(currentModel.Root));

            // Initializing regex filtering
            filterRegexCheckBox.Checked = false;
            regexTextbox.Text = "";
        }

        /// <summary>
        /// Invokes after the check state of a tree node has changed.
        /// 
        /// While working through this method, this handler will be deactivated. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void ownAfterCheck(object sender, TreeViewEventArgs e)
        {
            variableTreeView.AfterCheck -= ownAfterCheck;
            
            setChildrenChecked(e.Node);
            updateTreeView();

            if (currentModel.getOption(e.Node.Text) is NumericOption)
                updateEvaluationConfiguration();

            updateAdjustedFunction();
            updateInteractionsTab();

            variableTreeView.AfterCheck += ownAfterCheck;
        }

        /// <summary>
        /// Creates a tree node out of the specified option.
        /// 
        /// While doing that, all children of this option (according to the variability model) will be
        /// added as children of this node.
        /// </summary>
        /// <param name="val">Option that is about to be inserted into the tree view. Must not be null.</param>
        /// <returns>The corresponding tree node ready for insertion.</returns>
        private TreeNode insertIntoTreeView(ConfigurationOption val)
        {
            if (val == null)
                throw new ArgumentException("Parameter val must not be null!");

            List<TreeNode> functionChildren = new List<TreeNode>();

            // Creating all nodes of the children
            foreach (ConfigurationOption child in val.Children)
                functionChildren.Add(insertIntoTreeView(child));

            // Creating this node and setting the correct state of this node
            TreeNode current = new TreeNode(val.Name, functionChildren.ToArray());

            if (currentModel.BinaryOptions.Contains(val) && !((BinaryOption)val).Optional
                && !((BinaryOption)val).hasAlternatives())
            {
                current.Checked = true;
                current.BackColor = deactivatedColor;
            }
            else if (currentModel.NumericOptions.Contains(val))
                current.Checked = true;

            return current;
        }

        /// <summary>
        /// Sets every child that represents a non-optional binary option to the checkstate of its parent.
        /// 
        /// For each of these children the method will be executed on those children too.
        /// </summary>
        /// <param name="parent">Node from which every lower node will be set to the corresponding checkstate. Must not be null.</param>
        private void setChildrenChecked(TreeNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("Parameter parent must not be null!");

            foreach (TreeNode child in parent.Nodes)
            {
                BinaryOption opt = currentModel.getBinaryOption(child.Name);

                if (opt != null && !opt.Optional && !opt.hasAlternatives())
                {
                    child.Checked = parent.Checked;
                    setChildrenChecked(child);
                }
            }
        }

        /// <summary>
        /// Calculates the maximum amount of digits used in the constants of the currently
        /// loaded original function.
        /// </summary>
        /// <returns>Maximum amount of used digits</returns>
        private decimal getMaxDigits()
        {
            int maxDigits = 0;

            foreach (string prt in originalFunction.getExpressionTree())
            {
                double i;

                if (double.TryParse(prt, out i))
                {
                    string[] splitNumber = prt.Split(new char[] { '.', ',' });

                    int j = splitNumber.Length > 1 ? splitNumber[1].Length : 0;

                    if (maxDigits < j)
                        maxDigits = j;
                }
            }

            return maxDigits;
        }

        /// <summary>
        /// Calculates and returns the components of the specified expression.
        /// 
        /// The outer additions (+) and subtractions (-) will be seen as separators
        /// of the components. Each component gets an operator '+' or '-' depending
        /// on the operator in front of this component. The first component will receive
        /// a '+'.
        /// </summary>
        /// <param name="exp">Specified expression. Must not be null.</param>
        /// <returns>List of components with their previous operations</returns>
        private List<Tuple<string, string>> getComponents(string exp)
        {
            if (exp == null)
                throw new ArgumentNullException("Parameter exp must nor be null!");

            string[] currExpression = exp.Split(' ');
            string currSign = "+";
            List<Tuple<string, string>> components = new List<Tuple<string, string>>();
            List<string> componentParts = new List<string>();
            
            for (int i = 0; i < currExpression.Length; i++)
            {
                if (currExpression[i] == "log10(")
                {
                    int bracketCount = 1;

                    while (bracketCount > 0)
                    {
                        componentParts.Add(currExpression[i]);
                        i++;

                        if (currExpression[i] == "log10(")
                            bracketCount++;

                        if (currExpression[i] == ")")
                            bracketCount--;
                    }

                    componentParts.Add(currExpression[i]);
                }
                else if (currExpression[i] == "+" || currExpression[i] == "-" || i == currExpression.Length - 1)
                {
                    if (i == currExpression.Length - 1)
                        componentParts.Add(currExpression[i]);
                    
                    if (componentParts.Count > 0)
                        components.Add(Tuple.Create<string, string>(String.Join(" ", componentParts), currSign));

                    componentParts.Clear();

                    if (currExpression[i] == "-")
                        currSign = "-";
                    else
                        currSign = "+";
                }
                else
                    componentParts.Add(currExpression[i]);
            }

            return components;
        }

        /// <summary>
        /// Calculates and returns all interactions and their degrees of the adjusted function.
        /// </summary>
        /// <returns>Sorted Dictionary with all interactions and their degrees of each option</returns>
        private SortedDictionary<string, SortedDictionary<string, List<Tuple<int, int>>>> getInteractions()
        {
            SortedDictionary<string, SortedDictionary<string, List<Tuple<int, int>>>> currInteractions =
                new SortedDictionary<string, SortedDictionary<string, List<Tuple<int, int>>>>();

            //Dictionary<string, Dictionary<string, List<Tuple<int, int>>>> currInteractions =
                //new Dictionary<string, Dictionary<string, List<Tuple<int, int>>>>();
            
            foreach (Tuple<string, string> component in getComponents(adjustedMeasurementFunction))
            {
                // Getting a list of all options in a component. Presuming that there is no
                // addition in logarithms (due to the learned function) 
                List<ConfigurationOption> componentOptions = new List<ConfigurationOption>();
                int degree = 0;

                foreach (string part in component.Item1.Split(' '))
                {
                    ConfigurationOption option = currentModel.getOption(part);
                
                    if (option != null && !componentOptions.Contains(option))
                    {
                        componentOptions.Add(option);
                        degree++;
                    }
                }

                // Calculating all interactions for the list of found options
                for (int i = 0; i < componentOptions.Count; i++)
                {
                    SortedDictionary<string, List<Tuple<int, int>>> optionInteractions;

                    if (!currInteractions.TryGetValue(componentOptions[i].Name, out optionInteractions))
                        optionInteractions = new SortedDictionary<string, List<Tuple<int,int>>>();

                    for (int j = 0; j < componentOptions.Count; j++)
                    {
                        if (i != j)
                        {
                            List<Tuple<int, int>> val;

                            if (optionInteractions.TryGetValue(componentOptions[j].Name, out val))
                            {
                                bool degreeFound = false;
                                int degreePos = 0;
                                int newValue = 1;

                                for (int k = 0; k < val.Count && !degreeFound; k++)
                                    if (val[k].Item1 == degree)
                                    {
                                        degreeFound = true;
                                        degreePos = k;
                                        newValue = val[k].Item2 + 1;
                                    }

                                if (degreeFound)
                                    val.Remove(val[degreePos]);

                                val.Add(new Tuple<int, int>(degree, newValue));
                            }
                            else
                            {
                                List<Tuple<int, int>> list = new List<Tuple<int, int>>();
                                list.Add(new Tuple<int, int>(degree, 1));

                                optionInteractions.Add(componentOptions[j].Name, list);
                            }
                        }
                    }

                    currInteractions.Remove(componentOptions[i].Name);
                    currInteractions.Add(componentOptions[i].Name, optionInteractions);
                }
            }
            
            return currInteractions;
        }

        /// Returns the current options that are allowed in the function.
        /// </summary>
        /// <returns>List of legal options</returns>
        private List<string> getLegalOptions()
        {
            List<string> legalOptions = new List<string>();

            // Variables filtering
            if (!filterVariablesCheckbox.Checked)
            {
                foreach (ConfigurationOption opt in currentModel.getOptions())
                    legalOptions.Add(opt.ToString());
            }
            else
            {
                switch (filterOptionCombobox.SelectedItem.ToString())
                {
                    case FILTERING_LIST_BOX:
                        foreach (string s in variableListBox.Items)
                        {
                            if (!variableListBox.CheckedItems.Contains(s))
                                legalOptions.Add(s);
                        }
                        break;

                    case FILTERING_TREE_VIEW:
                        if (variableTreeView.Nodes.Count > 0)
                        {
                            Stack<TreeNode> stack = new Stack<TreeNode>();
                            stack.Push(variableTreeView.Nodes[0]);

                            while (stack.Count > 0)
                            {
                                TreeNode node = stack.Pop();

                                foreach (TreeNode child in node.Nodes)
                                    stack.Push(child);

                                if (node.Checked)
                                    legalOptions.Add(node.Text);
                            }
                        }
                        break;
                }
            }

            // Regex filtering
            if (filterRegexCheckBox.Checked && regexTextbox.Text != "")
            {
                List<string> list = new List<string>();
                string[] regexes = regexTextbox.Text.Split(new char[] { ',', ';', '|' });

                for (int i = 0; i < regexes.Length; i++)
                    regexes[i] = regexes[i].Trim();

                foreach (string opt in legalOptions)
                {
                    bool containsRegex = false;

                    for (int i = 0; i < regexes.Length && !containsRegex; i++)
                    {
                        if (opt.Contains(regexes[i]) && regexes[i].Length > 0)
                        {
                            containsRegex = true;
                            list.Add(opt);
                        }
                    }
                }

                legalOptions = list;
            }

            return legalOptions;
        }

        /// <summary>
        /// Displays all interactions in the corresponding tab.
        /// </summary>
        private void displayInteractions()
        {
            interactionTextBox.Clear();

            SortedDictionary<string, SortedDictionary<string, List<Tuple<int, int>>>> interactions = getInteractions();
            Dictionary<string, Color> optionColors = new Dictionary<string, Color>();

            // Calculate colors for options
            List<string> options = interactions.Keys.ToList();
            int amountRestOptions = options.Count;
            int ceiling = (int)Math.Ceiling(Math.Pow(amountRestOptions, 1 / 3.0));
            int pos = 0;

            for (int green = 1; green <= ceiling; ++green)
            {
                for (int blue = 1; blue <= ceiling; ++blue)
                {
                    for (int red = 1; red <= ceiling; ++red)
                    {
                        if (amountRestOptions-- > 0)
                        {
                            int r = (int)(0.5 + red * 255.0 / ceiling);
                            int g = (int)(0.5 + green * 255.0 / ceiling);
                            int b = (int)(0.5 + blue * 255.0 / ceiling);

                            optionColors.Add(options[pos], Color.FromArgb(r, g, b));
                            pos++;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, SortedDictionary<string, List<Tuple<int, int>>>> entry in interactions)
            {
                int totalInteractions = 0;
                Color entryColor;
                optionColors.TryGetValue(entry.Key, out entryColor);

                // Appending text to display all interactions
                interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Bold);
                interactionTextBox.SelectionBackColor = entryColor;
                interactionTextBox.AppendText(entry.Key);
                interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Regular);
                interactionTextBox.SelectionBackColor = Color.White;
                interactionTextBox.AppendText("\ninteracts with: \t");

                if (entry.Value.Count == 0)
                    interactionTextBox.AppendText("nothing...\n");
                else
                {
                    KeyValuePair<string, List<Tuple<int, int>>>[] values = entry.Value.ToArray();

                    for (int i = 0; i < values.Length; i++)
                    {
                        Color partnerColor;
                        optionColors.TryGetValue(values[i].Key, out partnerColor);

                        interactionTextBox.AppendText("- ");
                        interactionTextBox.SelectionBackColor = partnerColor;
                        interactionTextBox.AppendText(values[i].Key);
                        interactionTextBox.SelectionBackColor = Color.White;
                        interactionTextBox.AppendText(" (");

                        for (int j = 0; j < values[i].Value.Count; j++)
                        {
                            totalInteractions += values[i].Value[j].Item2;
                            interactionTextBox.AppendText("Degree " + values[i].Value[j].Item1 + ": " + values[i].Value[j].Item2);

                            if (j < values[i].Value.Count - 1)
                                interactionTextBox.AppendText(", ");
                        }

                        interactionTextBox.AppendText(")\n\t\t");
                    }
                }

                if (entry.Value.Count > 0)
                {
                    interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Bold);
                    interactionTextBox.AppendText("Number of total interactions: " + totalInteractions + "\n");
                    interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Regular);
                }

                interactionTextBox.AppendText("\n");
            }
        }

        /// <summary>
        /// Extracts the maximum abstract constant of the specified expression.
        /// </summary>
        private void getMaxAbstractConstant(string exp)
        {
            List<double> constants = new List<double>();

            foreach (Tuple<string,string> part in getComponents(exp))
            {
                bool valid = true;
                List<double> nums = new List<double>();

                foreach (string prt in part.Item1.Split(' '))
                {
                    double i = 0.0;

                    if (double.TryParse(prt, out i))
                    {
                        if (i == 0)
                            valid = false;
                        else
                            nums.Add(Math.Abs(i));
                    }
                }

                if (valid)
                    constants.AddRange(nums);
            }

            maxAbstractConstant = constants.Count == 0 ? 1.0 : constants.Max();
        }

        /// <summary>
        /// Invokes if the corresponding button (generateFunctionButton) has been pressed.
        /// 
        /// By clicking on the button the ilFunctionPanel will draw the adjusted function.
        /// Additionally, all information about the created function will be saved.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void generateFunctionButton_Click(object sender, EventArgs e)
        {
            chosenOptions = Tuple.Create(currentModel.getNumericOption(firstAxisCombobox.SelectedItem.ToString()),
                currentModel.getNumericOption(secondAxisCombobox.SelectedItem.ToString()));

            List<BinaryOption> bins = new List<BinaryOption>();
            Dictionary<NumericOption, double> usedNumericOptions = new Dictionary<NumericOption, double>();

            foreach (string s in getLegalOptions())
            {
                BinaryOption opt = currentModel.getBinaryOption(s);
                
                if (opt != null)
                    bins.Add(opt);
            }
            
            foreach (KeyValuePair<NumericOption, float> entry in numericSettings.ToList())
            {
                if (chosenOptions.Item1 != entry.Key && chosenOptions.Item2 != entry.Key)
                    usedNumericOptions.Add(entry.Key, entry.Value);
            }

            configurationForCalculation = new Configuration(bins, usedNumericOptions);

            updateFunctionPanel();
            updateMeasurementTab();
        }

        /// <summary>
        /// Invokes if the status of the specified checkbox changes.
        /// 
        /// If the checkbox is selected, the corresponding NumericUpDown-element will be activated
        /// and the constants of the InfluenceFunction can be adjusted. If the amount of digits
        /// have been changed in a previous adjustment, this will be considered.
        /// If the checkbox is deselected, the corresponding NumericUpDown-element will be deactivated
        /// and the adjusted function will get the original constants.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void constantDecimalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            constantsDigitsUpDown.Enabled = constantDecimalCheckBox.Checked;

            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the value of the corresponding NumericUpDown-element has changed.
        /// 
        /// The counter handles the amount of digits of the constant values in the adjusted function.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void constantsDigitsUpDown_ValueChanged(object sender, EventArgs e)
        {
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the mouse wheel is used on the constantsDigitsUpDown.
        /// 
        /// This method deactivates the mouse wheel for this component.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void constantsDigitsUpDown_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        /// <summary>
        /// Invokes if the status of the specified checkbox changes.
        /// 
        /// If the checkbox is selected, the corresponding trackbar will be activated
        /// and the filtering of the constants is activated and will be executed with the already
        /// set filtering value.
        /// If the checkbox is deselected, the corresponding trackbar will be deactivated
        /// and there will be no filtering of the constants.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void constantFilteringCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            constantRelativeValueSlider.Enabled = constantFilteringCheckbox.Checked;

            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the specified trackbar was moved.
        /// 
        /// The trackbar indicates how much the abstract value of the constants will be filtered.
        /// If the trackbar is at the minimum value, there will be no filtering.
        /// If the trackbar is at the maximum value, only the biggest abstract will be shown.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void constantRelativeValueSlider_Scroll(object sender, EventArgs e)
        {
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the status of the specified checkbox changes.
        /// 
        /// If the checkbox is selected, the filtering of the variables is activated.
        /// All already checked and unchecked variables from a previous adjustment will be considered.
        /// If the checkbox is deselected, the filtering will be deactivated and all variables will be
        /// used in the Influence_Function. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void filterVariablesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            updateVariableConfiguration();
            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the index of the specified combobox changed.
        /// 
        /// It will deactivate all other views and will make them invisible to the user. Only the selected
        /// view will be shown and enabled. After choosing a new index, the adjusted function and configuration
        /// options will be updated.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void filterOptionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            variableListBox.Visible = false;
            variableListBox.Enabled = false;
            variableTreeView.Visible = false;
            variableTreeView.Enabled = false;

            switch (filterOptionCombobox.SelectedItem.ToString())
            {
                case FILTERING_LIST_BOX:
                    variableListBox.Visible = true;
                    variableListBox.Enabled = true;
                    break;

                case FILTERING_TREE_VIEW:
                    variableTreeView.Visible = true;
                    variableTreeView.Enabled = true;
                    break;
            }

            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if an item has been selected or deselected.
        /// 
        /// All selected variables will be shown in the Influence_Function.
        /// If a variable is not selected, it will not appear in the well-formed function and gets the value 0.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void variableListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentModel.getOption(variableListBox.SelectedItem.ToString()) is NumericOption)
                updateEvaluationConfiguration();

            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the first axis has changed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void firstAxisCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGenerateButton();
        }

        /// <summary>
        /// Invokes if the second axis has changed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void secondAxisCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGenerateButton();
        }

        /// <summary>
        /// Invokes if the calculatePerformanceButton has been pressed.
        /// 
        /// This will calculate the performance of the function if there are no numeric
        /// options in the adjusted function. All currently available binary options
        /// will be set to 1.0.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void calculatePerformanceButton_Click(object sender, EventArgs e)
        {
            string[] expressionParts = new string[adjustedExpressionTree.Length];

            // Copying and adjusting the current expression
            for (int i = 0; i < expressionParts.Length; i++)
            {
                double d;
                string part = String.Copy(adjustedExpressionTree[i]);

                // If the part is a number or an operator, it will be written back into the array.
                // Every remaining binary option will be set to 1.0.
                if (double.TryParse(part, out d) || isOperator(part))
                    expressionParts[i] = part;
                else
                    expressionParts[i] = "1.0";
            }

            calculationResultLabel.Text = calculateFunctionExpression(expressionParts);
        }

        /// <summary>
        /// Invokes if the status of the specified checkbox changed.
        /// 
        /// If the checkbox is selected, the regex textbox will be activated. If there already was something
        /// written in the textbox, the filtering will occur with the current text. If the checkbox is
        /// deselected, the regex textbox will be disabled and there will be no regex filtering.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void filterRegexCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            updateVariableConfiguration();
            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the text in the corresponding textbox has changed.
        /// 
        /// If the regex filtering is activated, all strings in the textbox will be used as
        /// search material of legal variables.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void regexTextbox_TextChanged(object sender, EventArgs e)
        {
            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the check state of the corresponding radiobutton (normalRadioButton) has changed.
        /// 
        /// If checked, the adjustedTextbox will display the adjusted function without any
        /// special form.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void normalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            factorizationSettingsButton.Enabled = false;
            resetFactorizationButton.Enabled = false;

            updateAdjustedFunction();
        }

        /// <summary>
        /// Invokes if the check sate of the corresponding radio button (factorRadioButton) has changed.
        /// 
        /// If checked, the adjustedTextbox will display the adjusted function with
        /// factorization of the variables.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void factorRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            factorizationSettingsButton.Enabled = true;
            resetFactorizationButton.Enabled = true;

            updateAdjustedFunction();
        }

        /// <summary>
        /// Invokes if the corresponding button (factorizationSettingsButton) was pressed.
        /// 
        /// A window will be opened such that the user is able to set the priorities of
        /// the factorization options. After closing the window, the adjusted function will
        /// be updated.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void factorizationSettingsButton_Click(object sender, EventArgs e)
        {
            Form form = new FactorizationSettings(factorizationPriorities);
            form.ShowDialog();

            updateAdjustedFunction();
        }

        /// <summary>
        /// Invokes if the corresponding button (resetFactorizationButton) was pressed.
        /// 
        /// All factorization priorities will be reset back to the value 1. After that, the
        /// adjusted function will be updated.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void resetFactorizationButton_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<ConfigurationOption, double> pair in factorizationPriorities.ToList())
            {
                factorizationPriorities.Remove(pair.Key);
                factorizationPriorities.Add(pair.Key, 1.0);
            }

            updateAdjustedFunction();
        }

        /// <summary>
        /// Invokes if another element of the corresponding combobox (chartComboBox) was selected.
        /// 
        /// Depending on the selected option, the corresponding chart will be shown.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void chartComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            interactionChart.Visible = false;
            constantChart.Visible = false;
            maxChart.Visible = false;
            maxOccuranceChart.Visible = false;
            rangeChart.Visible = false;

            switch (chartComboBox.SelectedItem.ToString())
            {
                case COMBOBOX_INTERACTIONS_OPTION:
                    interactionChart.Visible = true;
                    chartDescriptionLabel.Text = INTERACTION_INFORMATION;
                    break;
                case COMBOBOX_CONSTANT_OPTION:
                    constantChart.Visible = true;
                    chartDescriptionLabel.Text = CONSTANT_INFORMATION;
                    break;
                case COMBOBOX_MAX_OPTION:
                    maxChart.Visible = true;
                    chartDescriptionLabel.Text = MAX_INFORMATION;
                    break;
                case COMBOBOX_MAX_OCCURANCE_OPTION:
                    maxOccuranceChart.Visible = true;
                    chartDescriptionLabel.Text = MAX_OCCURANCE_INFORMATION;
                    break;
                case COMBOBOX_RANGE_OPTION:
                    rangeChart.Visible = true;
                    chartDescriptionLabel.Text = RANGE_INFORMATION;
                    break;
                default:
                    throw new Exception("This should not happen. An unknown option was selected!");
            }
        }

        /// <summary>
        /// Invokes if another index of the corresponding combobox (measurementViewCombobox) was selected.
        /// 
        /// It will change the presence of the loaded measurements depending on the selected index.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void measurementViewCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMeasurementPanel();
        }

        /// <summary>
        /// Invokes if another element of the corresponding combobox (nfpValueCombobox) was selected.
        /// 
        /// It will switch the NFProperty that will be read from the loaded measurements.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nfpValueCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMeasurementTab();
        }

        /// <summary>
        /// Updates the variable configuration.
        /// 
        /// Activates or deactivates all configuration options if the filterVariablesCheckbox
        /// is checked.
        /// </summary>
        private void updateVariableConfiguration()
        {
            filterOptionCombobox.Enabled = filterVariablesCheckbox.Checked;

            if (!filterVariablesCheckbox.Checked)
            {
                variableListBox.Enabled = false;
                variableTreeView.Enabled = false;
            }
            else
            {
                switch (filterOptionCombobox.SelectedItem.ToString())
                {
                    case FILTERING_LIST_BOX:
                        variableListBox.Enabled = true;
                        break;
                    case FILTERING_TREE_VIEW:
                        variableTreeView.Enabled = true;
                        break;
                }
            }

            regexTextbox.Enabled = filterRegexCheckBox.Checked;
        }

        /// <summary>
        /// Updates the TreeView such that only the selectable options are highlighted.
        /// </summary>
        private void updateTreeView()
        {
            List<BinaryOption> currentConfiguration = new List<BinaryOption>();
            List<Tuple<BinaryOption, TreeNode>> potentialCandidate = new List<Tuple<BinaryOption, TreeNode>>();

            List<TreeNode> allBinaryNodes = new List<TreeNode>();
            Stack<TreeNode> stack = new Stack<TreeNode>();
            stack.Push(variableTreeView.Nodes[0]);

            while(stack.Count > 0)
            {
                TreeNode node = stack.Pop();

                if (currentModel.getBinaryOption(node.Text) != null)
                {
                    allBinaryNodes.Add(node);

                    foreach (TreeNode child in node.Nodes)
                        stack.Push(child);
                }
            }

            // Numeric options will be ignored for now.
            foreach (TreeNode node in allBinaryNodes)
            {
                BinaryOption opt = currentModel.getBinaryOption(node.Text);

                if (opt != null)
                {
                    if (node.Checked)
                        currentConfiguration.Add(opt);

                    if (!opt.Optional && !opt.hasAlternatives())
                        node.BackColor = deactivatedColor;
                    else if (node.Parent != null && !node.Parent.Checked)
                        node.BackColor = deactivatedColor;
                    else
                    {
                        // Search all children of this node if there is a checked node that is optional
                        Stack<TreeNode> nodes = new Stack<TreeNode>();
                        bool done = false;

                        foreach (TreeNode child in node.Nodes)
                            nodes.Push(child);
                           
                        while (nodes.Count != 0 && !done)
                        {
                            TreeNode temp = nodes.Pop();
                            BinaryOption tempOption = currentModel.getBinaryOption(temp.Text);

                            if (temp.Checked && tempOption != null)
                            {
                                if (!tempOption.Optional && !tempOption.hasAlternatives())
                                    foreach (TreeNode tempChild in temp.Nodes)
                                        nodes.Push(tempChild);
                                else
                                    done = true;
                            }
                        }

                        if (!done)
                            potentialCandidate.Add(Tuple.Create(opt, node));
                        else
                            node.BackColor = deactivatedColor;
                    }
                }
            }
            
            // Check for each potential candidate if the configuration is still legal even
            // after its addition/removal.
            foreach (Tuple<BinaryOption, TreeNode> t in potentialCandidate)
            {
                if (!currentConfiguration.Remove(t.Item1))
                    currentConfiguration.Add(t.Item1);

                if (sat.checkConfigurationSAT(currentConfiguration, currentModel, true))
                    t.Item2.BackColor = normalColor;
                else
                    t.Item2.BackColor = deactivatedColor;

                if (!currentConfiguration.Remove(t.Item1))
                    currentConfiguration.Add(t.Item1);
            }
        }

        /// <summary>
        /// Updates the evaluation configuration.
        /// 
        /// It will update the choosable axis options such that only numeric options
        /// in the adjusted function are selectable. If there are none or there is no
        /// loaded model, an alternative panel (original groupbox panel) will be displayed.
        /// </summary>
        private void updateEvaluationConfiguration()
        {
            if (modelLoaded)
            {
                firstAxisCombobox.Items.Clear();
                secondAxisCombobox.Items.Clear();

                List<string> legalOptions = getLegalOptions();

                foreach (NumericOption option in originalFunction.participatingNumOptions)
                {
                    if (legalOptions.Contains(option.Name))
                    {
                        firstAxisCombobox.Items.Add(option.Name);
                        secondAxisCombobox.Items.Add(option.Name);
                    }
                }

                if (firstAxisCombobox.Items.Count != 0)
                {
                    secondAxisCombobox.Items.Add(SECOND_EMPTY_OPTION);

                    firstAxisCombobox.SelectedIndex = 0;
                    secondAxisCombobox.SelectedIndex = 0;
                }
            }

            evaluationFunctionPanel.Visible = modelLoaded && firstAxisCombobox.Items.Count != 0;
            evaluationFunctionPanel.Enabled = evaluationFunctionPanel.Visible;
            noNumericOptionPanel.Visible = !evaluationFunctionPanel.Visible;
            noNumericOptionPanel.Enabled = noNumericOptionPanel.Visible;

            updateGenerateButton();
        }

        /// <summary>
        /// Configurates the label for the error message of the function configuration
        /// if needed.
        /// 
        /// If the generationButton is enabled, the label will be set invisible.
        /// Otherwise the label is visible and displays a helpful message.
        /// </summary>
        private void updateGenerateButton()
        {
            generateFunctionButton.Enabled = firstAxisCombobox.Items.Count != 0 &&
                !firstAxisCombobox.SelectedItem.Equals(secondAxisCombobox.SelectedItem);

            if (generateFunctionButton.Enabled)
                failureLabel.Visible = false;
            else
            {
                if (!modelLoaded)
                    failureLabel.Text = ERROR_NO_MODEL;
                else if (firstAxisCombobox.SelectedItem != null
                    && firstAxisCombobox.SelectedItem.Equals(secondAxisCombobox.SelectedItem))
                    failureLabel.Text = ERROR_DOUBLE_OPTION;

                failureLabel.Visible = true;
            }
        }
        
        /// <summary>
        /// Updates the interaction tab.
        /// </summary>
        private void updateInteractionsTab()
        {
            // Update displayed interactions
            displayInteractions();

            // Update charts
            updateCharts();
        }

        /// <summary>
        /// Updates the charts of the interactions tab.
        /// 
        /// All charts will be recalculated.
        /// </summary>
        private void updateCharts()
        {
            List<string> legalOptions = getLegalOptions();

            // Update influence chart
            interactionChart.Series.Clear();
            interactionChart.Series.Add("Series1");

            foreach (KeyValuePair<string, SortedDictionary<string, List<Tuple<int, int>>>> entry in getInteractions())
            {
                int value = 0;

                foreach (KeyValuePair<string, List<Tuple<int, int>>> pair in entry.Value)
                    foreach (Tuple<int, int> tup in pair.Value)
                        value += tup.Item2;

                if (value != 0)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point.AxisLabel = entry.Key;
                    point.SetValueY(value);
                    point.ToolTip = "Number of interactions: #VALY";

                    interactionChart.Series["Series1"].Points.Insert(0, point);
                }
            }

            Dictionary<string, Tuple<double, double>> rangeInfluences = calculateRangeInfluences();
            Dictionary<string, double> constantInfluences = new Dictionary<string, double>();
            Dictionary<string, double> constantMaxInfluences = new Dictionary<string, double>();
            
            // Calculate the constant influences
            foreach (Tuple<string, string> component in getComponents(adjustedMeasurementFunction))
            {
                double constant = 1.0;
                double currConstantRange = 0;
                List<string> variables = new List<string>();

                foreach (string part in component.Item1.Split(' '))
                {
                    double num = 0.0;
                    bool isConstant = double.TryParse(part, out num);

                    constant = isConstant ? num : (component.Item2 == "-" ? -constant : constant);

                    if (!isConstant && part != "" && !isOperator(part))
                        variables.Add(part);
                }

                currConstantRange = constant;

                foreach (string var in variables)
                {
                    // Calculating constant influences
                    double oldValue = 0.0;
                    
                    if (constantInfluences.TryGetValue(var, out oldValue))
                        constantInfluences.Remove(var);

                    constantInfluences.Add(var, oldValue + Math.Abs(constant));

                    // Calculating max influences
                    NumericOption opt = currentModel.getNumericOption(var);

                    if (opt != null)
                        currConstantRange = currConstantRange * opt.Max_value;                        
                }

                foreach (string var in variables)
                {
                    double oldValue = 0.0;

                    if (constantMaxInfluences.TryGetValue(var, out oldValue))
                        constantMaxInfluences.Remove(var);

                    constantMaxInfluences.Add(var, oldValue + Math.Abs(currConstantRange));
                }
            }

            // Update other charts
            constantChart.Series.Clear();
            constantChart.Series.Add("Series1");

            maxChart.Series.Clear();
            maxChart.Series.Add("Series1");

            maxOccuranceChart.Series.Clear();
            maxOccuranceChart.Series.Add("Series1");

            rangeChart.Series.Clear();
            rangeChart.Series.Add("Series1");
            rangeChart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;

            // Update abstract constant chart
            foreach (KeyValuePair<string, double> entry in constantInfluences)
            {
                if (legalOptions.Contains(entry.Key))
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point.AxisLabel = entry.Key;
                    point.SetValueY(entry.Value);

                    constantChart.Series["Series1"].Points.Insert(0, point);
                }
            }

            int amountOfVariants = varGen.generateAllVariantsFast(currentModel).Count;

            // Update max and max occurance chart
            foreach (KeyValuePair<string, double> entry in constantMaxInfluences)
            {
                if (legalOptions.Contains(entry.Key))
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point1.AxisLabel = entry.Key;
                    point1.SetValueY(entry.Value);

                    maxChart.Series["Series1"].Points.Insert(0, point1);
                    
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point2.AxisLabel = entry.Key;

                    if (currentModel.getNumericOption(entry.Key) != null)
                        point2.SetValueY(entry.Value);
                    else
                    {
                        int i = 0;

                        if (!occuranceOfOptions.TryGetValue(entry.Key, out i))
                            i = 1;

                        point2.SetValueY(entry.Value * i / amountOfVariants);
                    }

                    maxOccuranceChart.Series["Series1"].Points.Insert(0, point2);
                }
            }

            foreach (KeyValuePair<string, Tuple<double, double>> entry in rangeInfluences)
            {
                System.Windows.Forms.DataVisualization.Charting.DataPoint point1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                point1.AxisLabel = entry.Key;
                point1.SetValueY(entry.Value.Item1, entry.Value.Item2);

                rangeChart.Series[0].Points.Add(point1);
            }

            // Sort data points in charts
            interactionChart.Series[0].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "AxisLabel");
            constantChart.Series[0].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "AxisLabel");
            maxChart.Series[0].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "AxisLabel");
            maxOccuranceChart.Series[0].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "AxisLabel");
            rangeChart.Series[0].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "AxisLabel");
        }

        /// <summary>
        /// Calculates and returns the range influence of each numeric option.
        /// </summary>
        /// <returns>Range influences of numeric options</returns>
        private Dictionary<string, Tuple<double, double>> calculateRangeInfluences()
        {
            Dictionary<string, Tuple<double, double>> influences = new Dictionary<string, Tuple<double, double>>();
            string[] currExpression = String.Copy(adjustedMeasurementFunction).Split(' ');
            List<string> optionList = new List<string>();

            // Remove all binary options and remember all numeric options
            for (int i = 0; i < currExpression.Length; i++)
            {
                if (currentModel.getOption(currExpression[i]) != null && !optionList.Contains(currExpression[i]))
                    optionList.Add(currExpression[i]);
            }

            // Calculate the range influence of each option
            foreach (string option in optionList)
            {
                List<string> expressions = new List<string>();
                string generalExpression = "";
                
                foreach (Tuple<string, string> comp in getComponents(String.Join(" ", currExpression)))
                {
                    string[] splitComponent = comp.Item1.Split(' ');
                    bool componentAdded = false;

                    for (int i = 0; i < splitComponent.Length && !componentAdded; i++)
                    {
                        if (splitComponent[i] == option)
                        {
                            componentAdded = true;

                            // TODO: Testen
                            if (generalExpression.Count() == 0)
                                generalExpression += "0";

                            generalExpression += " " + comp.Item2 + " " + String.Join(" ", comp.Item1);
                        }
                    }
                }

                expressions.Add(generalExpression);

                // Calculate all possible combinations of expressions
                foreach (string innerOption in optionList)
                {
                    ConfigurationOption opt = currentModel.getOption(innerOption);
                    List<string> insertedExpressions = new List<string>();

                    foreach (string exp in expressions)
                    {
                        string[] minExpression = String.Copy(exp).Split(' ');
                        string[] maxExpression = String.Copy(exp).Split(' ');

                        for (int i = 0; i < minExpression.Length; i++)
                        {
                            if (minExpression[i] == innerOption)
                            {
                                if (opt is BinaryOption)
                                {
                                    minExpression[i] = "0.0";
                                    maxExpression[i] = "1.0";
                                }
                                else if (opt is NumericOption)
                                {
                                    NumericOption numOpt = (NumericOption)opt;

                                    minExpression[i] = numOpt.Min_value.ToString().Replace(',', '.');
                                    maxExpression[i] = numOpt.Max_value.ToString().Replace(',', '.');
                                }
                                else
                                    throw new Exception("This should not happen! Error while checking types of options!");

                            }
                        }

                        insertedExpressions.Add(String.Join(" ", minExpression));
                        insertedExpressions.Add(String.Join(" ", maxExpression));
                    }

                    expressions = insertedExpressions;
                }

                List<double> values = new List<double>();

                foreach (string exp in expressions)
                    values.Add(evaluateExpression(exp));

                influences.Add(option, new Tuple<double, double>(values.Min(), values.Max()));
            }

            return influences;
        }

        /// <summary>
        /// Evaluates the specified expression.
        /// 
        /// This only works if the expression does not contain any configuration options anymore.
        /// Furthermore, the expression has to be in infix notation.
        /// </summary>
        /// <param name="exp">Specified expression with no configuration options. Must not be null.</param>
        /// <returns>Evaluated value</returns>
        private double evaluateExpression(String exp)
        {
            if (exp == null)
                throw new ArgumentNullException("Parameter exp must not be null!");

            List<Tuple<string, string>> components = getComponents(exp);
            double finalValue = 0.0;
            
            // Components only contain multiplication or logarithm
            foreach (Tuple<string, string> comp in components)
            {
                double currentValue = 1.0;
                string[] compParts = comp.Item1.Split(' ');

                for (int i = 0; i < compParts.Length && currentValue != 0; i++)
                {
                    if (compParts[i] == "log10(")
                    {
                        int bracketCounter = 1;
                        int currPos = i;

                        while (bracketCounter > 0)
                        {
                            i++;

                            if (compParts[i] == "log10(")
                                bracketCounter++;

                            if (compParts[i] == ")")
                                bracketCounter--;
                        }

                        // log(x) == 0 if x <= 0
                        string[] innerLog = new string[i - currPos - 1];
                        Array.Copy(compParts, currPos + 1, innerLog, 0, i - currPos - 1);

                        double val = evaluateExpression(String.Join(" ", innerLog));
                        currentValue *= val <= 0 ? 0 : Math.Log10(val);
                    }
                    else
                    {
                        double d;

                        if (double.TryParse(compParts[i], out d))
                            currentValue *= d;
                    }
                }

                finalValue = finalValue + (comp.Item2 == "-" ? -currentValue : currentValue);
            }

            return finalValue;
        }

        /// <summary>
        /// Updates the adjusted function.
        /// 
        /// This is done by observing which options have been selected and by adjusting the
        /// original function.
        /// </summary>
        private void updateAdjustedFunction()
        {
            string[] expressionParts = new string[originalFunction.getExpressionTree().Length];

            for (int i = 0; i < expressionParts.Length; i++)
                expressionParts[i] = String.Copy(originalFunction.getExpressionTree()[i]);

            // Adjusting variables
            if (filterVariablesCheckbox.Checked || filterRegexCheckBox.Checked)
                expressionParts = filterVariables(expressionParts);

            // Get new maximal abstract constant
            getMaxAbstractConstant(calculateFunctionExpression(expressionParts));

            // Adjusting constants
            if (constantDecimalCheckBox.Checked || constantFilteringCheckbox.Checked)
            {
                int maxDigits = decimal.ToInt32(constantsDigitsUpDown.Value);
                double minAbstract = maxAbstractConstant * constantRelativeValueSlider.Value / 100;

                for (int i = 0; i < expressionParts.Length; i++)
                {
                    string part = expressionParts[i];
                    double number = 0.0;

                    if (double.TryParse(part, out number))
                    {
                        if (Math.Abs(number) < minAbstract)
                            part = "0.0";

                        if (constantDecimalCheckBox.Checked)
                        {
                            string[] parts = part.Split('.');
                            
                            if (maxDigits == 0)
                                part = parts[0] + ".0";
                            else if (parts[1].Length > maxDigits)
                                part = parts[0] + "." + parts[1].Substring(0, maxDigits);
                        }
                    }

                    expressionParts[i] = part;
                }
            }

            // Calculate simplyfied expression in infix notation
            string adjustedExpression = sortExpression(calculateFunctionExpression(expressionParts));
            expressionParts.CopyTo(adjustedExpressionTree, 0);

            adjustedMeasurementFunction = String.Copy(adjustedExpression);

            // If needed, calculate a factorized form of the expression
            if (factorRadioButton.Checked)
                adjustedExpression = factorizeExpression(adjustedExpression);

            updateFunctionTextBox(adjustedTextBox, adjustedExpression);
        }

        /// <summary>
        /// Removes the variables from the specified expression that are not selected in the current
        /// filtering view.
        /// </summary>
        /// <param name="expressionParts">Expression with variables. Must not be null.</param>
        /// <returns>Expression without unwanted variables.</returns>
        private string[] filterVariables(string[] expressionParts)
        {
            if (expressionParts == null)
                throw new ArgumentNullException("Parameter expressionParts must not be null.");

            string[] parts = expressionParts;
            List<string> legalOptions = getLegalOptions();
            
            for (int i = 0; i < parts.Length; i++)
            {
                if (currentModel.getOption(parts[i]) != null
                    && !legalOptions.Contains(parts[i]))
                    parts[i] = "0.0";
            }

            return parts;
        }

        /// <summary>
        /// Calculates a function expression for the given expressionParts.
        /// 
        /// The method will simplyfy the expression, if this is possible.
        /// </summary>
        /// <param name="expParts">Parts of the expression. Must be given in the revert polish notation. Must not be null.</param>
        /// <returns>Simplyfied expression of the function</returns>
        private string calculateFunctionExpression(string[] expParts)
        {
            if (expParts == null)
                throw new ArgumentNullException("Parameter expParts may not be null!");

            // The stack consists of tuples that contain the value of the expression part and the last
            // operation used on this part.
            Stack<Tuple<string, string>> stack = new Stack<Tuple<string, string>>();

            for (int i = 0; i < expParts.Length; i++)
            {
                string prt = expParts[i];

                // Check if prt is an operator
                if (prt.Equals("+") || prt.Equals("-") || prt.Equals("*") || prt.Equals("/"))
                {
                    Tuple<string, string> second = stack.Pop();
                    Tuple<string, string> first = stack.Pop();
                    double numFst = -1.0;
                    double numSnd = -1.0;

                    bool firstSuccess = double.TryParse(first.Item1, out numFst);
                    bool secondSuccess = double.TryParse(second.Item1, out numSnd);

                    if (firstSuccess || secondSuccess)
                    {
                        Tuple<string, string> simple;

                        switch (prt)
                        {
                            case "+":
                                if (firstSuccess && numFst == 0.0)
                                    simple = second;
                                else if (secondSuccess && numSnd == 0.0)
                                    simple = first;
                                else if (firstSuccess && secondSuccess)
                                    simple = Tuple.Create<string, string>((numFst + numSnd).ToString().Replace(',', '.'), null);
                                else
                                    simple = Tuple.Create<string, string>(first.Item1 + " + " + second.Item1, "+");
                                break;
                            case "-":
                                if (firstSuccess && numFst == 0.0)
                                    simple = Tuple.Create(getExpression(getComponents(second.Item1)), second.Item2);
                                else if (secondSuccess && numSnd == 0.0)
                                    simple = first;
                                else if (firstSuccess && secondSuccess)
                                    simple = Tuple.Create<string, string>((numFst - numSnd).ToString().Replace(',', '.'), null);
                                else
                                    simple = Tuple.Create<string, string>(first.Item1 + " - " + second.Item1, "-");
                                break;
                            case "*":
                                if (firstSuccess && numFst == 0.0 || secondSuccess && numSnd == 0.0)
                                    simple = Tuple.Create<string, string>("0.0", null);
                                else if (numFst == 1.0)
                                    simple = second;
                                else if (numSnd == 1.0)
                                    simple = first;
                                else if (firstSuccess && secondSuccess)
                                    simple = Tuple.Create<string, string>((numFst * numSnd).ToString().Replace(',', '.'), null);
                                else
                                {
                                    string temp = "";

                                    temp = first.Item2 == "+" || first.Item2 == "-" ? "( " + first.Item1 + " )" : first.Item1;
                                    temp = temp + " * ";
                                    temp = temp + (second.Item2 == "+" || second.Item2 == "-" ? "( " + second.Item1 + " )" : second.Item1);

                                    simple = Tuple.Create<string, string>(temp, "*");
                                }
                                break;
                            case "/":
                                if (firstSuccess && numFst == 0.0 || secondSuccess && numSnd == 0.0)
                                    // CAREFUL! Here: x/0 = 0
                                    simple = Tuple.Create<string, string>("0.0", null);
                                else if (numSnd == 1.0)
                                    simple = first;
                                else if (firstSuccess && secondSuccess)
                                    simple = Tuple.Create<string, string>((numFst / numSnd).ToString().Replace(',', '.'), null);
                                else
                                {
                                    string temp = "";

                                    temp = first.Item2 == "+" || first.Item2 == "-" ? "( " + first.Item1 + " )" : first.Item1;
                                    temp = temp + " / ";
                                    temp = temp + (second.Item2 == "+" || second.Item2 == "-" ? "( " + second.Item1 + " )" : second.Item1);

                                    simple = Tuple.Create<string, string>(temp, "/");
                                }
                                break;
                            default:
                                throw new Exception("Unknown operation found!");
                        }

                        stack.Push(simple);

                    }
                    else
                    {
                        if (prt == "*" || prt == "/")
                        {
                            string temp = "";

                            temp = first.Item2 == "+" || first.Item2 == "-" ? "( " + first.Item1 + " )" : first.Item1;
                            temp = temp + " " + prt + " ";
                            temp = temp + (second.Item2 == "+" || second.Item2 == "-" ? "( " + second.Item1 + " )" : second.Item1);

                            stack.Push(Tuple.Create<string, string>(temp, prt));
                        }
                        else
                            stack.Push(Tuple.Create<string, string>(first.Item1 + " " + prt + " " + second.Item1, prt));
                    }
                }
                else if (prt.Equals("]"))
                    stack.Push(Tuple.Create<string, string>("log10( " + stack.Pop().Item1 + " )", "log10"));
                else
                {
                    // Otherwise it is a variable or a number                    
                    stack.Push(Tuple.Create<string, string>(prt, null));
                }
            }

            if(stack.Count != 1)
                throw new Exception("The entered expression is not in a valid revert polish notation!");

            return stack.Pop().Item1;
        }

        /// <summary>
        /// Updates the measurement tab of the application.
        /// 
        /// The method will check, if the preconditions are fulfilled (for example: Are there any
        /// loaded measurements?). Is not, an error message will be displayed. Is all preconditions
        /// are fulfilled, each IlPanel in the measurment tab will be recalculated and redrawn.
        /// </summary>
        private void updateMeasurementTab()
        {
            // Check if measurements are loaded
            if (!measurementsLoaded)
            {
                overviewPanel.Visible = false;
                bothGraphsPanel.Visible = false;
                measurementsOnlyPanel.Visible = false;
                absoluteDifferencePanel.Visible = false;
                relativeDifferencePanel.Visible = false;

                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = ERROR_NO_MEASUREMENTS_LOADED;
                return;
            }

            // Check if a graph has been calculated before
            if (configurationForCalculation == null)
            {
                overviewPanel.Visible = false;
                bothGraphsPanel.Visible = false;
                measurementsOnlyPanel.Visible = false;
                absoluteDifferencePanel.Visible = false;
                relativeDifferencePanel.Visible = false;

                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = ERROR_NO_PERFORMANCES;
                return;
            }

            // Check if the current configuration is possible
            if (!sat.checkConfigurationSAT(configurationForCalculation.BinaryOptions.Keys.ToList(), currentModel, true))
            {
                overviewPanel.Visible = false;
                bothGraphsPanel.Visible = false;
                measurementsOnlyPanel.Visible = false;
                absoluteDifferencePanel.Visible = false;
                relativeDifferencePanel.Visible = false;

                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = ERROR_ILLEGAL_CONFIGURATION;
                return;
            }

            // Calculating all measured configurations that can be used
            List<Configuration> neededConfigurations = new List<Configuration>();

            foreach (Configuration conf in GlobalState.allMeasurements.Configurations)
            {
                bool insert = true;

                foreach (KeyValuePair<BinaryOption, BinaryOption.BinaryValue> entry in conf.BinaryOptions.ToList())
                    insert = insert && configurationForCalculation.BinaryOptions.ContainsKey(entry.Key);

                if (insert)
                {
                    foreach (KeyValuePair<NumericOption, double> pair in configurationForCalculation.NumericOptions.ToList())
                    {
                        double val;

                        if (conf.NumericOptions.TryGetValue(pair.Key, out val))
                            insert = insert && val == pair.Value;
                    }
                }

                if (insert)
                    neededConfigurations.Add(conf);
            }

            // Check if there are no measurements for the current settings
            if (neededConfigurations.Count == 0)
            {
                overviewPerformanceIlPanel.Scene = new ILScene();
                overviewMeasurementIlPanel.Scene = new ILScene();
                overviewAbsoluteDifferenceIlPanel.Scene = new ILScene();
                overviewRelativeDifferenceIlPanel.Scene = new ILScene();
                bothGraphsIlPanel.Scene = new ILScene();
                measurementsOnlyIlPanel.Scene = new ILScene();
                absoluteDifferenceIlPanel.Scene = new ILScene();
                relativeDifferenceIlPanel.Scene = new ILScene();
                overviewPerformanceIlPanel.Refresh();
                overviewMeasurementIlPanel.Refresh();
                overviewAbsoluteDifferenceIlPanel.Refresh();
                overviewRelativeDifferenceIlPanel.Refresh();
                bothGraphsIlPanel.Refresh();
                measurementsOnlyIlPanel.Refresh();
                absoluteDifferenceIlPanel.Refresh();
                relativeDifferenceIlPanel.Refresh();

                overviewPanel.Visible = false;
                bothGraphsPanel.Visible = false;
                measurementsOnlyPanel.Visible = false;
                absoluteDifferencePanel.Visible = false;
                relativeDifferencePanel.Visible = false;

                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = ERROR_NO_MEASUREMENTS_AVAILABLE;
                return;
            }

            if (nfpValueCombobox.Items.Count == 0)
            {
                foreach (KeyValuePair<string, NFProperty> entry in GlobalState.nfProperties.ToList())
                    nfpValueCombobox.Items.Add(entry.Key);

                nfpValueCombobox.SelectedIndex = 0;
            }

            NFProperty prop = new NFProperty(nfpValueCombobox.SelectedItem.ToString());

            // Check if at least one configuration contains the current nfp value
            if(neededConfigurations.All(x => !x.nfpValues.Keys.Contains(prop)))
            {
                overviewPerformanceIlPanel.Scene = new ILScene();
                overviewMeasurementIlPanel.Scene = new ILScene();
                overviewAbsoluteDifferenceIlPanel.Scene = new ILScene();
                overviewRelativeDifferenceIlPanel.Scene = new ILScene();
                bothGraphsIlPanel.Scene = new ILScene();
                measurementsOnlyIlPanel.Scene = new ILScene();
                absoluteDifferenceIlPanel.Scene = new ILScene();
                relativeDifferenceIlPanel.Scene = new ILScene();
                overviewPerformanceIlPanel.Refresh();
                overviewMeasurementIlPanel.Refresh();
                overviewAbsoluteDifferenceIlPanel.Refresh();
                overviewRelativeDifferenceIlPanel.Refresh();
                bothGraphsIlPanel.Refresh();
                measurementsOnlyIlPanel.Refresh();
                absoluteDifferenceIlPanel.Refresh();
                relativeDifferenceIlPanel.Refresh();

                overviewPanel.Visible = false;
                bothGraphsPanel.Visible = false;
                measurementsOnlyPanel.Visible = false;
                absoluteDifferencePanel.Visible = false;
                relativeDifferencePanel.Visible = false;

                nfpValueCombobox.Enabled = true;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = ERROR_NO_MEASUREMENTS_NFP;
                return;
            }

            measurementErrorLabel.Visible = false;
            measurementViewCombobox.Enabled = true;
            nfpValueCombobox.Enabled = true;

            ILPlotCube bothGraphsCube, measurementsOnlyCube, absoluteDifferenceCube,
                relativeDifferenceCube, overviewPerformanceCube, overviewMeasurementsCube,
                overviewAbsoluteDifferenceCube, overviewRelativeDifferenceCube; 

            // Decide if there has to be a 2D or 3D shape
            if (chosenOptions.Item2 == null)
            {
                // Define plot cubes
                bothGraphsCube = new ILPlotCube(twoDMode: true);
                measurementsOnlyCube = new ILPlotCube(twoDMode: true);
                absoluteDifferenceCube = new ILPlotCube(twoDMode: true);
                relativeDifferenceCube = new ILPlotCube(twoDMode: true);
                overviewPerformanceCube = new ILPlotCube(twoDMode: true);
                overviewMeasurementsCube = new ILPlotCube(twoDMode: true);
                overviewAbsoluteDifferenceCube = new ILPlotCube(twoDMode: true);
                overviewRelativeDifferenceCube = new ILPlotCube(twoDMode: true);

                bothGraphsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                bothGraphsCube.Axes.YAxis.Label.Text = CORRESPONDING_VALUES_LABEL;
                measurementsOnlyCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                measurementsOnlyCube.Axes.YAxis.Label.Text = MEASURED_VALUE_LABEL;
                absoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                absoluteDifferenceCube.Axes.YAxis.Label.Text = ABSOLUTE_DIFFERENCE_LABEL;
                relativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                relativeDifferenceCube.Axes.YAxis.Label.Text = RELATIVE_DIFFERENCE_LABEL;
                overviewPerformanceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewPerformanceCube.Axes.YAxis.Label.Text = PERFORMANCE_AXIS_LABEL;
                overviewMeasurementsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewMeasurementsCube.Axes.YAxis.Label.Text = MEASURED_VALUE_LABEL;
                overviewAbsoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewAbsoluteDifferenceCube.Axes.YAxis.Label.Text = ABSOLUTE_DIFFERENCE_LABEL;
                overviewRelativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewRelativeDifferenceCube.Axes.YAxis.Label.Text = RELATIVE_DIFFERENCE_LABEL;

                // Add all values into the array
                ILArray<float> XY = ILMath.zeros<float>(0, 0);
                ILArray<float> absoluteDifferences = ILMath.zeros<float>(0, 0);
                ILArray<float> relativeDifferences = ILMath.zeros<float>(0, 0);
                List<double> values = chosenOptions.Item1.getAllValues();
                values.Sort();

                int pos = 0;

                foreach (double value in values)
                {
                    Configuration c = null;
                    double d;

                    // Get the measurement for the current settings
                    for (int j = 0; j < neededConfigurations.Count && c == null; j++)
                    {
                        neededConfigurations[j].NumericOptions.TryGetValue(chosenOptions.Item1, out d);

                        if (d == value)
                            c = neededConfigurations[j];
                    }

                    if (c == null || !c.nfpValues.TryGetValue(prop, out d))
                    {
                        // If there are no measurements for this specific setting, the line plots
                        // have to be drawn up to this point.
                        if (XY.Size[0] > 0)
                        {
                            bothGraphsCube.Add(new ILLinePlot(XY)
                            {
                                ColorOverride = measurementColor
                            });
                            measurementsOnlyCube.Add(new ILLinePlot(XY)
                            {
                                ColorOverride = measurementColor
                            });
                            absoluteDifferenceCube.Add(new ILLinePlot(absoluteDifferences));
                            relativeDifferenceCube.Add(new ILLinePlot(relativeDifferences));
                            overviewMeasurementsCube.Add(new ILLinePlot(XY)
                            {
                                ColorOverride = measurementColor
                            });
                            overviewAbsoluteDifferenceCube.Add(new ILLinePlot(absoluteDifferences));
                            overviewRelativeDifferenceCube.Add(new ILLinePlot(relativeDifferences));

                            XY = ILMath.zeros<float>(0, 0);
                            absoluteDifferences = ILMath.zeros<float>(0, 0);
                            relativeDifferences = ILMath.zeros<float>(0, 0);
                            pos = 0;
                        }
                    }
                    else
                    {
                        // Calculate all values for the corresponding line plots.
                        XY[0, pos] = (float)value;
                        XY[1, pos] = (float)d;
                        absoluteDifferences[0, pos] = (float)value;
                        absoluteDifferences[1, pos] = ILMath.abs((float)d - calculatedPerformances[1, values.IndexOf(value)]);
                        relativeDifferences[0, pos] = (float)value;
                        
                        if (ILMath.abs(XY[1, pos]) < 1)
                            relativeDifferences[1, pos] = absoluteDifferences[1, pos] == XY[1, pos] ? 0 : 100;
                        else
                            relativeDifferences[1, pos] = absoluteDifferences[1, pos] >= 1 ? absoluteDifferences[1, pos]/XY[1, pos] * 100 : 0;

                        ILPoints point = createPoint(XY[0, pos], XY[1, pos], 0, measurementPointLabel);

                        // Adding events to the point to display its coordinates on the screen
                        point.MouseMove += (s, a) =>
                        {
                            Vector3 coor = point.GetPosition();

                            measurementPointLabel.Text = chosenOptions.Item1.Name + ": " + coor.X.ToString() + ", " + PERFORMANCE_AXIS_LABEL + ": " + coor.Y.ToString();
                            measurementPointLabel.Visible = true;
                        };

                        pos++;

                        bothGraphsCube.Add(point);
                        measurementsOnlyCube.Add(point);
                    }
                }

                // Insert all remaining line plot parts into the corresponding cubes.
                bothGraphsCube.Add(new ILLinePlot(XY)
                {
                    ColorOverride = measurementColor
                });
                bothGraphsCube.Add(new ILLinePlot(drawnPerformances)
                {
                    Line =
                    {
                        Color = calculatedColor,
                        DashStyle = DashStyle.Dashed
                    }
                });
                measurementsOnlyCube.Add(new ILLinePlot(XY)
                {
                    ColorOverride = measurementColor
                });
                absoluteDifferenceCube.Add(new ILLinePlot(absoluteDifferences));
                absoluteDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));
                relativeDifferenceCube.Add(new ILLinePlot(relativeDifferences));
                relativeDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));

                overviewPerformanceCube.Add(new ILLinePlot(drawnPerformances)
                {
                    ColorOverride = calculatedColor
                });
                overviewMeasurementsCube.Add(new ILLinePlot(XY)
                {
                    ColorOverride = measurementColor
                });
                overviewAbsoluteDifferenceCube.Add(new ILLinePlot(absoluteDifferences));
                overviewAbsoluteDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));
                overviewRelativeDifferenceCube.Add(new ILLinePlot(relativeDifferences));
                overviewRelativeDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));
            }
            else
            {
                ILArray<float> measurements, X, Y, absoluteDifferences, relativeDifferences;

                // Define all plot cubes
                bothGraphsCube = new ILPlotCube(twoDMode: false);
                measurementsOnlyCube = new ILPlotCube(twoDMode: false);
                absoluteDifferenceCube = new ILPlotCube(twoDMode: false);
                relativeDifferenceCube = new ILPlotCube(twoDMode: false);
                overviewPerformanceCube = new ILPlotCube(twoDMode: false);
                overviewMeasurementsCube = new ILPlotCube(twoDMode: false);
                overviewAbsoluteDifferenceCube = new ILPlotCube(twoDMode: false);
                overviewRelativeDifferenceCube = new ILPlotCube(twoDMode: false);

                bothGraphsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                bothGraphsCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                bothGraphsCube.Axes.ZAxis.Label.Text = CORRESPONDING_VALUES_LABEL;
                measurementsOnlyCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                measurementsOnlyCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                measurementsOnlyCube.Axes.ZAxis.Label.Text = MEASURED_VALUE_LABEL;
                absoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                absoluteDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                absoluteDifferenceCube.Axes.ZAxis.Label.Text = ABSOLUTE_DIFFERENCE_LABEL;
                relativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                relativeDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                relativeDifferenceCube.Axes.ZAxis.Label.Text = RELATIVE_DIFFERENCE_LABEL;
                overviewPerformanceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewPerformanceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                overviewPerformanceCube.Axes.ZAxis.Label.Text = PERFORMANCE_AXIS_LABEL;
                overviewMeasurementsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewMeasurementsCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                overviewMeasurementsCube.Axes.ZAxis.Label.Text = MEASURED_VALUE_LABEL;
                overviewAbsoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewAbsoluteDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                overviewAbsoluteDifferenceCube.Axes.ZAxis.Label.Text = ABSOLUTE_DIFFERENCE_LABEL;
                overviewRelativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                overviewRelativeDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                overviewRelativeDifferenceCube.Axes.ZAxis.Label.Text = RELATIVE_DIFFERENCE_LABEL;

                // Initialize and fill all arrays
                X = Array.ConvertAll(chosenOptions.Item1.getAllValues().ToArray(), x => (float)x);
                Y = Array.ConvertAll(chosenOptions.Item2.getAllValues().ToArray(), y => (float)y);

                ILArray<float> XMat = 1;
                ILArray<float> YMat = ILMath.meshgrid(Y, X, XMat);

                measurements = ILMath.zeros<float>(X.Length, Y.Length, 3);
                absoluteDifferences = ILMath.zeros<float>(X.Length, Y.Length, 3);
                relativeDifferences = ILMath.zeros<float>(X.Length, Y.Length, 3);
                
                measurements[":;:;1"] = XMat;
                measurements[":;:;2"] = YMat;
                
                List<double> valuesX = chosenOptions.Item1.getAllValues();
                List<double> valuesY = chosenOptions.Item2.getAllValues();
                valuesX.Sort();
                valuesY.Sort();

                // Read every possible measurement value. If there is none, the corresponding value will
                /// be set to negative infinity.
                for (int i = 0; i < valuesX.Count; i++)
                {
                    for(int j = 0; j < valuesY.Count; j++)
                    {
                        Configuration c = null;
                        double d1, d2;

                        for (int k = 0; k < neededConfigurations.Count && c == null; k++)
                        {
                            neededConfigurations[k].NumericOptions.TryGetValue(chosenOptions.Item1, out d1);
                            neededConfigurations[k].NumericOptions.TryGetValue(chosenOptions.Item2, out d2);

                            if (d1 == valuesX[i] && d2 == valuesY[j])
                                c = neededConfigurations[k];
                        }

                        if (c == null || !c.nfpValues.TryGetValue(prop, out d1))
                            measurements[i, j, 0] = float.NegativeInfinity;
                        else
                        {
                            measurements[i, j, 0] = (float)d1;

                            ILPoints point = createPoint(measurements[i, j, 1], measurements[i, j, 2], measurements[i, j, 0], measurementPointLabel);

                            // Adding events to the point to display its coordinates on the screen
                            point.MouseMove += (s, a) =>
                            {
                                Vector3 coor = point.GetPosition();

                                measurementPointLabel.Text = chosenOptions.Item1.Name + ": " + coor.X.ToString() + ", " + chosenOptions.Item2.Name + ": " + coor.Y.ToString() + ", " + PERFORMANCE_AXIS_LABEL + ": " + coor.Z.ToString();
                                measurementPointLabel.Visible = true;
                            };

                            bothGraphsCube.Add(point);
                            measurementsOnlyCube.Add(point);
                        }
                    }
                }

                // Calculate all absolute and relative differences.
                for (int i = 0; i < measurements.Size[0]; i++)
                {
                    for (int j = 0; j < measurements.Size[1]; j++)
                    {
                        absoluteDifferences[i, j, 0] = measurements[i, j, 0] == float.NegativeInfinity
                            ? float.NegativeInfinity : Math.Abs(measurements[i, j, 0].GetArrayForRead()[0] - calculatedPerformances[i, j, 0].GetArrayForRead()[0]);
                        absoluteDifferences[i, j, 1] = measurements[i, j, 1].GetArrayForRead()[0];
                        absoluteDifferences[i, j, 2] = measurements[i, j, 2].GetArrayForRead()[0];

                        if (measurements[i, j, 0] == float.NegativeInfinity)
                            relativeDifferences[i, j, 0] = float.NegativeInfinity;
                        else if (measurements[i, j, 0] == 0)
                            relativeDifferences[i, j, 0] = absoluteDifferences[i, j, 0] == 0 ? 0 : 100;
                        else
                            relativeDifferences[i, j, 0] = absoluteDifferences[i, j, 0] >= 1 ? absoluteDifferences[i, j, 0] / measurements[i, j, 0] * 100 : 0;
                        
                        relativeDifferences[i, j, 1] = measurements[i, j, 1].GetArrayForRead()[0];
                        relativeDifferences[i, j, 2] = measurements[i, j, 2].GetArrayForRead()[0];
                    }
                }

                // Insert all information into the cubes
                bothGraphsCube.Add(new ILSurface(measurements)
                    {
                        ColorMode = ILSurface.ColorModes.Solid
                    }
                );
                bothGraphsCube.Add(new ILSurface(calculatedPerformances));
                measurementsOnlyCube.Add(new ILSurface(measurements)
                    {
                        ColorMode = ILSurface.ColorModes.Solid
                    }
                );
                absoluteDifferenceCube.Add(new ILSurface(absoluteDifferences)
                {
                    Children = { new ILColorbar() }
                });
                absoluteDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));
                relativeDifferenceCube.Add(new ILSurface(relativeDifferences)
                {
                    Children = { new ILColorbar() }
                });
                relativeDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));
                overviewPerformanceCube.Add(new ILSurface(calculatedPerformances));
                overviewMeasurementsCube.Add(new ILSurface(measurements)
                {
                    ColorMode = ILSurface.ColorModes.Solid
                });
                overviewMeasurementsCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));
                overviewAbsoluteDifferenceCube.Add(new ILSurface(absoluteDifferences));
                overviewAbsoluteDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));
                overviewRelativeDifferenceCube.Add(new ILSurface(relativeDifferences));
                overviewRelativeDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));

                // Adding events for a synchronized rotation of the overview cubes
                overviewPerformanceCube.MouseMove += (s, e) =>
                {
                    ILPlotCube cube = overviewPerformanceIlPanel.GetCurrentScene().First<ILPlotCube>();
                    Matrix4 matrix = cube.Rotation;

                    overviewMeasurementsCube.Rotation = matrix;
                    overviewAbsoluteDifferenceCube.Rotation = matrix;
                    overviewRelativeDifferenceCube.Rotation = matrix;

                    overviewMeasurementIlPanel.Refresh();
                    overviewAbsoluteDifferenceIlPanel.Refresh();
                    overviewRelativeDifferenceIlPanel.Refresh();
                };

                overviewMeasurementsCube.MouseMove += (s, e) =>
                {
                    ILPlotCube cube = overviewMeasurementIlPanel.GetCurrentScene().First<ILPlotCube>();
                    Matrix4 matrix = cube.Rotation;

                    overviewPerformanceCube.Rotation = matrix;
                    overviewAbsoluteDifferenceCube.Rotation = matrix;
                    overviewRelativeDifferenceCube.Rotation = matrix;

                    overviewPerformanceIlPanel.Refresh();
                    overviewAbsoluteDifferenceIlPanel.Refresh();
                    overviewRelativeDifferenceIlPanel.Refresh();
                };

                overviewAbsoluteDifferenceCube.MouseMove += (s, e) =>
                {
                    ILPlotCube cube = overviewAbsoluteDifferenceIlPanel.GetCurrentScene().First<ILPlotCube>();
                    Matrix4 matrix = cube.Rotation;

                    overviewPerformanceCube.Rotation = matrix;
                    overviewMeasurementsCube.Rotation = matrix;
                    overviewRelativeDifferenceCube.Rotation = matrix;

                    overviewPerformanceIlPanel.Refresh();
                    overviewMeasurementIlPanel.Refresh();
                    overviewRelativeDifferenceIlPanel.Refresh();
                };

                overviewRelativeDifferenceCube.MouseMove += (s, e) =>
                {
                    ILPlotCube cube = overviewRelativeDifferenceIlPanel.GetCurrentScene().First<ILPlotCube>();
                    Matrix4 matrix = cube.Rotation;

                    overviewPerformanceCube.Rotation = matrix;
                    overviewMeasurementsCube.Rotation = matrix;
                    overviewAbsoluteDifferenceCube.Rotation = matrix;

                    overviewPerformanceIlPanel.Refresh();
                    overviewMeasurementIlPanel.Refresh();
                    overviewAbsoluteDifferenceIlPanel.Refresh();
                };
            }

            overviewPerformanceIlPanel.Scene = new ILScene { overviewPerformanceCube };
            overviewMeasurementIlPanel.Scene = new ILScene { overviewMeasurementsCube };
            overviewAbsoluteDifferenceIlPanel.Scene = new ILScene { overviewAbsoluteDifferenceCube };
            overviewRelativeDifferenceIlPanel.Scene = new ILScene { overviewRelativeDifferenceCube };
            bothGraphsIlPanel.Scene = new ILScene { bothGraphsCube };
            measurementsOnlyIlPanel.Scene = new ILScene { measurementsOnlyCube };
            absoluteDifferenceIlPanel.Scene = new ILScene { absoluteDifferenceCube };
            relativeDifferenceIlPanel.Scene = new ILScene { relativeDifferenceCube };

            overviewPerformanceIlPanel.Refresh();
            overviewMeasurementIlPanel.Refresh();
            overviewAbsoluteDifferenceIlPanel.Refresh();
            overviewRelativeDifferenceIlPanel.Refresh();
            bothGraphsIlPanel.Refresh();
            measurementsOnlyIlPanel.Refresh();
            absoluteDifferenceIlPanel.Refresh();
            relativeDifferenceIlPanel.Refresh();
            
            updateMeasurementPanel();
        }

        /// <summary>
        /// Updates which IlPanel for the measurement is visible.
        /// 
        /// This is depending on the selected item in the corresponding combobox (measurementViewCombobox).
        /// </summary>
        private void updateMeasurementPanel()
        {
            overviewPanel.Visible = false;
            bothGraphsPanel.Visible = false;
            measurementsOnlyPanel.Visible = false;
            absoluteDifferencePanel.Visible = false;
            relativeDifferencePanel.Visible = false;
            
            switch (measurementViewCombobox.SelectedItem.ToString())
            {
                case COMBOBOX_OVERVIEW_OPTION:
                    overviewPanel.Visible = true;
                    break;
                case COMBOBOX_BOTH_OPTION:
                    bothGraphsPanel.Visible = true;
                    break;
                case COMBOBOX_MEASUREMENTS_OPTION:
                    measurementsOnlyPanel.Visible = true;
                    break;
                case COMBOBOX_ABSOLUTE_DIFFERENCE_OPTION:
                    absoluteDifferencePanel.Visible = true;
                    break;
                case COMBOBOX_RELATIVE_DIFFERENCE_OPTION:
                    relativeDifferencePanel.Visible = true;
                    break;
                default:
                    throw new Exception("This should not happen. An unknown option was found.");
            }
        }

        /// <summary>
        /// Clears the specified RichTextBox and appends the specified function in a well-formed way.
        /// 
        /// The color of the constants are dependent on the maximum abstract constant of the
        /// expression.
        /// </summary>
        /// <param name="textbox">RichTextBox in which the specified function will be written on. Must not be null.</param>
        /// <param name="function">Expression of the function. Must not be null.</param>
        private void updateFunctionTextBox(RichTextBox textbox, string function)
        {
            if (textbox == null)
                throw new ArgumentNullException("Parameter textbox may not be null!");
            if (function == null)
                throw new ArgumentNullException("Parameter function may not be null!");

            List<Tuple<string, string>> expressionParts = getComponents(function);
            string tabs = "";

            textbox.Clear();

            for (int i = 0; i < expressionParts.Count; i++)
            {
                string[] partComponents = expressionParts[i].Item1.Split(' ');
                int pos = 0;

                foreach (string component in partComponents)
                {
                    double d;
                    string addingComponent = component;

                    if (double.TryParse(component, out d))
                    {
                        // Adds color for the constants
                        if (maxAbstractConstant == 0)
                            textbox.SelectionBackColor = Color.LightGreen;
                        else
                            textbox.SelectionBackColor = Color.FromArgb((int) (255 * Math.Abs(d) / maxAbstractConstant),
                                (int) (255 * (maxAbstractConstant - Math.Abs(d)) / maxAbstractConstant), 0);
                    }
                    else if (addingComponent == "(" || addingComponent == "log10(")
                    {
                        // Adds one tab if there is an opening bracket
                        tabs += BRACKET_TAB;
                        addingComponent = tabs + addingComponent;

                        if (textbox.Text != "")
                            addingComponent = "\n" + addingComponent;
                    }
                    else if (addingComponent == ")")
                    {
                        // Removes one tab if there is a closing bracket
                        tabs = tabs.Remove(tabs.Length - BRACKET_TAB.Length);

                        if (pos < partComponents.Length - 2)
                            addingComponent = addingComponent + "\n" + tabs;
                    }

                    if (addingComponent != "")
                    {
                        textbox.AppendText(addingComponent);
                        textbox.SelectionBackColor = Color.White;
                        textbox.AppendText(" ");
                    }

                    pos++;
                }
                
                if (i < expressionParts.Count - 1)
                    textbox.AppendText("\n" + tabs + expressionParts[i+1].Item2 + " ");
            }
        }

        /// <summary>
        /// Updates the Function_Panel.
        /// 
        /// Draws the current adjusted Influence_function. In the process the selected
        /// axes will be considered. If only one axis has been selected, the result will
        /// be a 2D-graph of the adjusted function. Otherwise, there will be a 3D-representation
        /// of the function with the specified axes.
        /// </summary>
        private void updateFunctionPanel()
        {
            if (secondAxisCombobox.SelectedItem.Equals(SECOND_EMPTY_OPTION))
                draw2DShape();
            else
                draw3DShape();
   
            ilFunctionPanel.Refresh();
        }        

        /// <summary>
        /// Draws the adjusted function into a two-dimensional grid.
        /// </summary>
        private void draw2DShape()
        {
            var scene = new ILScene();

            // Copying all adjusted expression for further adjustments
            string[] polishAdjusted = new string[adjustedExpressionTree.Length];

            for (int i = 0; i < polishAdjusted.Length; i++)
                polishAdjusted[i] = String.Copy(adjustedExpressionTree[i]);

            NumericOption option = currentModel
                    .getNumericOption(firstAxisCombobox.SelectedItem.ToString());

            // Replace remaining options with variables and actual values
            for (int i = 0; i < polishAdjusted.Length; i++)
            {
                if (currentModel.getNumericOption(polishAdjusted[i]) != null)
                {
                    if (polishAdjusted[i].Equals(option.Name))
                        polishAdjusted[i] = "XY";
                    else
                    {
                        // All other options will be set on the value they have been set in the
                        // settings option.
                        float value = 0;
                        numericSettings.TryGetValue(currentModel.getNumericOption(polishAdjusted[i]), out value);

                        string[] parts = value.ToString().Split(new char[] {'.', ',' });

                        if (parts.Length == 1)
                            polishAdjusted[i] = parts[0];
                        else if (parts.Length == 2)
                            polishAdjusted[i] = parts[0] + "." + parts[1];
                        else
                            throw new Exception("An illegal number was found!");
                    }
                }
                else if (currentModel.getBinaryOption(polishAdjusted[i]) != null)
                    polishAdjusted[i] = "1.0";
            }
            
            // Define plot cube
            ILPlotCube cube = new ILPlotCube(twoDMode: true);
            cube.Axes.XAxis.Label.Text = option.Name;
            cube.Axes.YAxis.Label.Text = PERFORMANCE_AXIS_LABEL;

            // Calculate values for the measurements
            ILArray<float> XY = Array.ConvertAll(option.getAllValues().ToArray(), x => (float) x);
            XY = XY.T;

            XY["0;:"] = XY;
            XY["1;:"] = calculateFunction(polishAdjusted, new string[] { "XY" }, new ILArray<float>[] { XY });

            // Adding interactive points to the cube
            for (int i = 0; i < XY.Size[1]; i++)
            {
                ILPoints point = createPoint(XY[0, i], XY[1, i], 0, pointPositionLabel);

                // Adding events to the point to display its coordinates on the screen
                point.MouseMove += (s, a) =>
                {
                    Vector3 coor = point.GetPosition();

                    pointPositionLabel.Text = option.Name + ": " + coor.X.ToString() + ", " + PERFORMANCE_AXIS_LABEL + ": " + coor.Y.ToString();
                    pointPositionLabel.Visible = true;
                };

                cube.Add(point);
            }

            calculatedPerformances = ILMath.zeros<float>(0, 0);

            for (int i = 0; i < XY.Size[0]; i++)
                for (int j = 0; j < XY.Size[1]; j++)
                    calculatedPerformances[i, j] = XY[i, j];

            // Calculated values for the plot
            XY = ILMath.linspace<float>(option.Min_value, option.Max_value, 50);

            XY["0;:"] = XY;
            XY["1;:"] = calculateFunction(polishAdjusted, new string[] { "XY" }, new ILArray<float>[] { XY });

            // Adding a lineplot to the cube
            ILLinePlot linePlot = new ILLinePlot(XY)
            {
                Line =
                {
                    Color = calculatedColor
                }
            };
            
            cube.Add(linePlot);

            ilFunctionPanel.Scene = new ILScene()
                {
                    cube
                };

            // Saving the coordinates/values of the points for the measurements
            drawnPerformances = XY;
        }

        /// <summary>
        /// Draws the adjusted function into a three-dimencional grid.
        /// </summary>
        private void draw3DShape()
        {
            var scene = new ILScene();

            // Copying all adjusted expression for further adjustments
            string[] polishAdjusted = new string[adjustedExpressionTree.Length];

            for (int i = 0; i < polishAdjusted.Length; i++)
                polishAdjusted[i] = String.Copy(adjustedExpressionTree[i]);

            // Getting selected options for axes
            NumericOption firstOption = currentModel.getNumericOption(firstAxisCombobox.SelectedItem.ToString());
            NumericOption secondOption = currentModel.getNumericOption(secondAxisCombobox.SelectedItem.ToString());

            // Replace remaining options with variables and actual values
            for (int i = 0; i < polishAdjusted.Length; i++)
            {
                if (currentModel.getNumericOption(polishAdjusted[i]) != null)
                {
                    if (polishAdjusted[i].Equals(firstOption.Name))
                        polishAdjusted[i] = "XMat";
                    else if (polishAdjusted[i].Equals(secondOption.Name))
                        polishAdjusted[i] = "YMat";
                    else
                    {
                        // All other options will be set on the value they have been set in the
                        // settings option.
                        NumericOption option = currentModel.getNumericOption(polishAdjusted[i]);

                        float value = 0;
                        numericSettings.TryGetValue(option, out value);

                        string[] parts = value.ToString().Split(new char[] { '.', ',' });

                        if (parts.Length == 1)
                            polishAdjusted[i] = parts[0];
                        else if (parts.Length == 2)
                            polishAdjusted[i] = parts[0] + "." + parts[1];
                        else
                            throw new Exception("An illegal number was found!");
                    }
                }
                else if (currentModel.getBinaryOption(polishAdjusted[i]) != null)
                    polishAdjusted[i] = "1.0";
            }

            // Define the ranges of the axes
            ILArray<float> X, Y, A;
            ILPlotCube cube = new ILPlotCube(twoDMode: false);
            cube.Axes.XAxis.Label.Text = firstOption.Name;
            cube.Axes.YAxis.Label.Text = secondOption.Name;
            cube.Axes.ZAxis.Label.Text = PERFORMANCE_AXIS_LABEL;
            
            // Calculating the surface
            X = Array.ConvertAll(firstOption.getAllValues().ToArray(), x => (float)x);
            Y = Array.ConvertAll(secondOption.getAllValues().ToArray(), y => (float)y);

            ILArray<float> XMat = 1;
            ILArray<float> YMat = ILMath.meshgrid(Y, X, XMat);

            A = ILMath.zeros<float>(X.Length, Y.Length, 3);

            // Fill array with values
            A[":;:;0"] = calculateFunction(polishAdjusted,
                new string[] { "XMat", "YMat" }, new ILArray<float>[] { XMat, YMat });
            A[":;:;1"] = XMat;
            A[":;:;2"] = YMat;
            
            cube.Add(new ILSurface(A));

            calculatedPerformances = ILMath.zeros<float>(0, 0, 0);

            // Saving the coordinates/values of the points for the measurements
            for (int i = 0; i < A.Size[0]; i++)
                for (int j = 0; j < A.Size[1]; j++)
                    for (int k = 0; k < A.Size[2]; k++)
                        calculatedPerformances[i, j, k] = A[i, j, k];
            
            // Calculating the points on the surface
            X = X.T;
            Y = Y.T;

            XMat = 1;
            YMat = ILMath.meshgrid(Y, X, XMat);

            ILArray<float> resultArray = calculateFunction(polishAdjusted,
                new string[] { "XMat", "YMat" }, new ILArray<float>[] { XMat, YMat }).GetArrayForRead();
            ILArray<float> xArray = XMat.GetArrayForRead();
            ILArray<float> yArray = YMat.GetArrayForRead();

            // Fill array with values
            A = ILMath.zeros<float>(3, resultArray.Length);
            A["0;:"] = xArray.T;
            A["1;:"] = yArray.T;
            A["2;:"] = resultArray.T;

            for (int i = 0; i < A.Size[1]; i++)
            {
                ILPoints point = createPoint(A[0, i], A[1, i], A[2, i], pointPositionLabel);

                // Adding events to the point to display its coordinates on the screen
                point.MouseMove += (s, a) =>
                {
                    Vector3 coor = point.GetPosition();

                    pointPositionLabel.Text = firstOption.Name + ": " + coor.X.ToString() + ", " + secondOption.Name + ": " + coor.Y.ToString() + ", " + PERFORMANCE_AXIS_LABEL + ": " + coor.Z.ToString();
                    pointPositionLabel.Visible = true;
                };

                cube.Add(point);
            }

            ilFunctionPanel.Scene = new ILScene()
                {
                    cube
                };
        }

        /// <summary>
        /// Creates an ILPoints-object at the specified coordinates and with a handler to set the
       ///  specified label for the coordinates invisible.
        /// </summary>
        /// <param name="x">First coordinate of the point. Must be a one-dimensional array with one element and must not be null!</param>
        /// <param name="y">Second coordinate of the point. Must be a one-dimensional array with one element and must not be null!</param>
        /// <param name="z">Third coordinate of the point. Must be a one-dimensional array with one element and must not be null!</param>
        /// <param name="l">Label that will show the coordinates of the point. Must not be null.</param>
        /// <returns>An interactive point that displays its coordinates.</returns>
        private ILPoints createPoint(ILRetArray<float> x, ILRetArray<float> y, ILRetArray<float> z, Label l)
        {
            if (x == null)
                throw new ArgumentNullException("Parameter x must not be null!");
            if (y == null)
                throw new ArgumentNullException("Parameter y must not be null!");
            if (z == null)
                throw new ArgumentNullException("Parameter z must not be null!");
            if (l == null)
                throw new ArgumentNullException("Parameter l must not be null!");
            if (x.Size.NumberOfElements != 1)
                throw new Exception("Parameter x does not contain exactly one element!");
            if (y.Size.NumberOfElements != 1)
                throw new Exception("Parameter y does not contain exactly one element!");
            if (z.Size.NumberOfElements != 1)
                throw new Exception("Parameter z does not contain exactly one element!");

            ILArray<float> coors = ILMath.zeros<float>(0);

            coors[0, 0] = x;
            coors[1, 0] = y;
            coors[2, 0] = z;

            ILPoints point = new ILPoints
            {
                Positions = coors,
                Color = Color.Black
            };

            point.MouseLeave += (s, a) =>
            {
                l.Visible = false;
            };

            return point;
        }

        /// <summary>
        /// Calculates the function for the ILFunctionPanel.
        /// 
        /// There has to be the same amount of optNames as in vars.
        /// </summary>
        /// <param name="expParts">Array of the separate expression parts. Must be given in the revert polish notation. Must not be null.</param>
        /// <param name="optNames">Array of string names of the used variables. Must not be null.</param>
        /// <param name="vars">Array of used variables. Must not be null.</param>
        /// <returns>The calculated function with the specified variables</returns>
        private ILArray<float> calculateFunction(string[] expParts, string[] optNames, ILArray<float>[] vars)
        {
            if (expParts == null)
                throw new ArgumentNullException("Parameter expParts may not be null!");
            if (optNames == null)
                throw new ArgumentNullException("Parameter optNames may not be null!");
            if (vars == null)
                throw new ArgumentNullException("Parameter vars may not be null!");
            if (optNames.Length != vars.Length)
                throw new FormatException("The number of option names does not equal"
                    + "with the number of actual variables!");

            Stack<ILArray<float>> stack = new Stack<ILArray<float>>();

            for (int i = 0; i < expParts.Length; i++)
            {
                string prt = expParts[i];
                bool done = false;

                // Check if part is an operator
                switch(prt)
                {
                    case "+":
                        if (i+1 >= expParts.Length || expParts[i + 1] == "+")
                            stack.Push(stack.Pop() + stack.Pop());
                        else if (expParts[i + 1] == "-")
                        {
                            ILArray<float> secondSumElem = stack.Pop();
                            ILArray<float> firstSumElem = stack.Pop();
                            stack.Push(firstSumElem - secondSumElem);
                        }
                        done = true;
                        break;
                    case "-":
                        if (i + 1 >= expParts.Length || expParts[i + 1] == "+")
                        {
                            ILArray<float> secondDiffElem = stack.Pop();
                            ILArray<float> firstDiffElem = stack.Pop();
                            stack.Push(firstDiffElem - secondDiffElem);
                        }
                        else if (expParts[i + 1] == "-")
                            stack.Push(stack.Pop() + stack.Pop());
                        done = true;
                        break;
                    case "*":
                        stack.Push(stack.Pop() * stack.Pop());
                        done = true;
                        break;
                    case "/":
                        ILArray<float> secondDivElem = stack.Pop();
                        ILArray<float> firstDivElem = stack.Pop();
                        ILArray<float> divResult = ILMath.zeros<float>(1, 1);

                        for (int j = 0; j < firstDivElem.Size[0]; j++)
                            for (int k = 0; k < firstDivElem.Size[1]; k++)
                                divResult[j, k] = secondDivElem[j, k] == 0 ? ILMath.zeros<float>(1) : (firstDivElem[j, k] / secondDivElem[j, k]);

                        stack.Push(divResult);
                        done = true;
                        break;
                    case "]":
                        ILArray<float> calcResult = stack.Pop();
                        ILArray<float> logResult = ILMath.zeros<float>(1, 1);

                        for (int j = 0; j < calcResult.Size[0]; j++)
                            for (int k = 0; k < calcResult.Size[1]; k++)
                                logResult[j, k] = calcResult[j, k] <= 0 ? ILMath.zeros<float>(1) : ILMath.log10(calcResult[j, k]);

                        stack.Push(logResult);
                        done = true;
                        break;
                }

                // Check if part is a variable
                for (int j = 0; j < optNames.Length && !done; j++)
                {
                    if (prt == optNames[j])
                    {
                        stack.Push(vars[j]);
                        done = true;
                    }
                }

                // Otherwise it is just a number or something unknown
                if(!done)
                {
                    float num = (float) 0.0;

                    if (float.TryParse(prt, out num))
                        stack.Push(num);
                    else
                        throw new Exception("Expression contains unknown parts!");
                }
            }

            if (stack.Count != 1)
                throw new Exception("The expression is not in a valid state!");

            return stack.Pop();
        }

        /// <summary>
        /// Checks if the specified token is an operator.
        /// </summary>
        /// <param name="token">Specified token</param>
        /// <returns>True if the token is an operator else false.</returns>
        private bool isOperator(string token)
        {
            return token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/") || token.Equals("]");
        }

        /// <summary>
        /// Factorizes one variable at a time.
        /// 
        /// If there are already some brackets in the specified expression, the factorization does not
        /// work as planned.
        /// </summary>
        /// <param name="exp">The expression that gets factorized. Must not be null.</param>
        /// <returns>A factorized version of the entered expression</returns>
        private string factorizeExpression(string exp)
        {
            if (exp == null)
                throw new ArgumentNullException("Parameter exp must not be null!");

            string result = "";
            List<Tuple<string, string>> expParts = getComponents(exp);
            Dictionary<string, int> counting = new Dictionary<string, int>();
            bool greaterThanOne = false;

            for (int i = 0; i < expParts.Count; i++)
                expParts[i] = Tuple.Create<string, string>(expParts[i].Item1.Trim(), expParts[i].Item2);

            foreach (Tuple<string, string> section in expParts)
            {
                string[] sectionParts = section.Item1.Split(' ');
                List<string> usedCompOptions = new List<string>();

                for (int i = 0; i < sectionParts.Length; i++)
                {
                    double d;

                    if (sectionParts[i] == "log10(")
                    {
                        int bracketsCount = 1;

                        while (bracketsCount > 0)
                        {
                            i++;

                            if (sectionParts[i] == "log10(")
                                bracketsCount++;

                            if (sectionParts[i] == ")")
                                bracketsCount--;
                        }
                    }
                    else if (!double.TryParse(sectionParts[i], out d) && !isOperator(sectionParts[i]))
                    {
                        if (!usedCompOptions.Contains(sectionParts[i])
                            && (i <= 0 || sectionParts[i] != "/"))
                        {
                            if (counting.ContainsKey(sectionParts[i]))
                            {
                                counting[sectionParts[i]] += 1;
                                greaterThanOne = true;
                            }
                            else
                                counting.Add(sectionParts[i], 1);

                            usedCompOptions.Add(sectionParts[i]);
                        }
                    }
                }
            }

            // If there are actual possibilities to factorize the expression
            if (greaterThanOne)
            {
                SortedDictionary<double, List<ConfigurationOption>> possibleOptions = new SortedDictionary<double, List<ConfigurationOption>>();
                bool maxFound = false;
                String max = "";

                // Calculate a list of all found options sorted by their priorities
                foreach (KeyValuePair<string, int> pair in counting) {
                    ConfigurationOption option = currentModel.getOption(pair.Key);
                    List<ConfigurationOption> list;
                    double val;

                    factorizationPriorities.TryGetValue(option, out val);

                    if (possibleOptions.ContainsKey(val))
                    {
                        possibleOptions.TryGetValue(val, out list);
                        list.Add(option);
                    }
                    else
                    {
                        list = new List<ConfigurationOption>();
                        list.Add(option);
                        possibleOptions.Add(val, list);
                    }
                }

                // Sort all options
                foreach (KeyValuePair<double, List<ConfigurationOption>> pair in possibleOptions)
                {
                    pair.Value.Sort(delegate (ConfigurationOption x, ConfigurationOption y)
                    {
                        int i, j;
                        counting.TryGetValue(x.Name, out i);
                        counting.TryGetValue(y.Name, out j);

                        return Math.Sign(j - i);
                    });
                }

                // Get the best option for factorization
                for (int i = possibleOptions.Count - 1; i >= 0 && !maxFound; i--)
                {
                    List<ConfigurationOption> list = possibleOptions.ToList()[i].Value;

                    for (int j = 0; j < list.Count && !maxFound; j++)
                    {
                        int count;
                        counting.TryGetValue(list[j].Name, out count);

                        if (count > 1)
                        {
                            maxFound = true;
                            max = list[j].Name;
                        }
                    }
                }

                List<Tuple<string, string>> others = new List<Tuple<string, string>>();
                List<Tuple<string, string>> varRest = new List<Tuple<string, string>>();

                foreach (Tuple<string, string> prt in expParts)
                {
                    string[] parts = prt.Item1.Split(' ');
                    int varPos = -1;
                    bool containsExactVar = false;

                    for (int i = 0; i < parts.Length && !containsExactVar; i++)
                    {
                        if (parts[i] == "log10(")
                        {
                            int bracketsCount = 1;

                            while (bracketsCount > 0)
                            {
                                i++;

                                if (parts[i] == "log10(")
                                    bracketsCount++;

                                if (parts[i] == ")")
                                    bracketsCount--;
                            }
                        }
                        else
                        {
                            containsExactVar = parts[i].Equals(max) && (i == 0 || parts[i] != "/");
                            varPos = i;
                        }
                    }

                    if (containsExactVar)
                    {
                        if (prt.Equals(max))
                            varRest.Add(Tuple.Create<string, string>("1.0", "+"));
                        else if (varPos == 0)
                        {
                            if (parts[1] == "/")
                                // TODO: Fragen, ob so passt
                                parts[0] = "1.0";
                            else
                            {
                                parts[0] = "";
                                parts[1] = "";
                            }

                            varRest.Add(Tuple.Create<string, string>(String.Join(" ", parts).Trim(), prt.Item2));
                        }
                        else
                        {
                            string temp = "";

                            for (int i = 0; i < parts.Length; i++)
                            {
                                if (i != varPos && i != varPos - 1 && !isOperator(parts[i]))
                                    temp = temp + parts[i] + " * ";
                            }

                            varRest.Add(Tuple.Create<string, string>(temp.Remove(temp.Length - 3, 3), prt.Item2));
                        }
                    }
                    else
                        others.Add(prt);
                }
                
                string factorizedRest = factorizeExpression(getExpression(varRest));
                string[] splitRest = factorizedRest.Split(' ');

                if (splitRest[2] == "(" && splitRest[splitRest.Length - 1] == ")")
                    result = max + " * " + factorizedRest;
                else
                    result = max + " * ( " + factorizedRest + " )";

                if (others.Count > 0)
                    result = result + " + " + factorizeExpression(getExpression(others));
            }
            else
                return exp;

            return result;
        }

        /// <summary>
        /// Calculates an appropriate expression for the specified expression parts.
        /// 
        /// If there is an negative component at the beginning, the constant in the first
        /// component will be negated. If there is no constant, an sppropriate constant
        /// will be added.
        /// </summary>
        /// <param name="expParts">Parts of the expression</param>
        /// <returns>Corresponding expression</returns>
        public string getExpression(List<Tuple<string, string>> expParts)
        {
            bool foundConstant = false;
            string[] compParts = expParts[0].Item1.Split(' ');
            string adjustedExp = "";

            for (int j = 0; j < compParts.Length && !foundConstant; j++)
            {
                if (compParts[j] == "log10(")
                {
                    int bracketCount = 1;

                    while (bracketCount > 0)
                    {
                        j++;

                        if (compParts[j] == "log10(")
                            bracketCount++;

                        if (compParts[j] == ")")
                            bracketCount--;
                    }
                }
                else if (expParts[0].Item2 == "-")
                {
                    double d = 0;

                    if (double.TryParse(compParts[j], out d))
                    {
                        compParts[j] = (-d).ToString();
                        foundConstant = true;
                    }
                }
            }

            if (!foundConstant && expParts[0].Item2 == "-")
                adjustedExp = " -1.0 * ";

            adjustedExp += String.Join(" ", compParts);

            for (int i = 1; i < expParts.Count; i++)
                adjustedExp += " " + expParts[i].Item2 + " " + expParts[i].Item1;

            return adjustedExp;
        }

        /// <summary>
        /// Sorts the components of the specified expression.
        /// </summary>
        /// <param name="exp">Specified expression.</param>
        /// <returns>A sorted expression</returns>
        private string sortExpression(string exp)
        {
            List<Tuple<string, string>> sortedComponents = getComponents(exp).OrderBy(x => x.Item1.Trim().Split(' ').Length).ToList();
            
            return getExpression(sortedComponents);
        }

        /// <summary>
        /// This class represents a little panel for the setting of the numeric option.
        /// </summary>
        private class NumericPanel : Panel
        {
            public NumericOption option;
            public NumericUpDown upDown;

            private Label label = new System.Windows.Forms.Label();

            /// <summary>
            /// Constructor of this class.
            /// </summary>
            /// <param name="opt">NumericOption with information for the componentsy</param>
            public NumericPanel(NumericOption opt, double currValue)
            {
                if (opt == null)
                    throw new ArgumentNullException("Parameter option must not be null!");

                option = opt;
                upDown = new OwnNumericUpDown(option.getAllValues(), currValue);
                upDown.MouseWheel += UpDown_MouseWheel;

                initializePanel(option);
            }

            private void UpDown_MouseWheel(object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }

            /// <summary>
            /// Initializes this panel with the specified NumericOption.
            /// </summary>
            /// <param name="option">NumericOption with information for the components</param>
            private void initializePanel(NumericOption option)
            {
                // Configuring this panel
                Controls.Add(upDown);
                Controls.Add(label);
                Location = new System.Drawing.Point(0, 0);
                Name = "panel1";
                Size = new System.Drawing.Size(300, 37);
                TabIndex = 1;

                // Configuring the label
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(12, 9);
                label.Name = "label";
                label.Size = new System.Drawing.Size(35, 13);
                label.TabIndex = 0;
                label.Text = option.Name;

                // Configuring the NumericUpDown
                upDown.Location = new System.Drawing.Point(200, 7);
                upDown.Name = "upDown";
                upDown.Size = new System.Drawing.Size(75, 20);
                upDown.TabIndex = 1;
            }
        }

        /// <summary>
        /// This class is a modified NumericUpDown such that it only displays the values it is given.
        /// </summary>
        private class OwnNumericUpDown : NumericUpDown
        {
            private List<double> values;

            /// <summary>
            /// Constructor of this class.
            /// 
            /// Sets all necessary properties.
            /// </summary>
            /// <param name="l">List of all displayed values</param>
            /// <param name="currValue">Current value that is to be shown</param>
            public OwnNumericUpDown(List<double> l, double currValue)
            {
                if (l == null)
                    throw new ArgumentNullException("Parameter l must not be null!");

                if (!l.Contains(currValue))
                    throw new Exception("The list l does not contain the current value!");

                this.Minimum = (decimal)l.Min();
                this.Maximum = (decimal)l.Max();

                this.Value = (decimal)currValue;

                values = l;
            }

            /// <summary>
            /// Overrides the method such that only legal values of the option are allowed.
            /// 
            /// If the maximum is already selected, there will be no increase.
            /// </summary>
            public override void UpButton()
            {
                if (this.Value != Maximum)
                    this.Value = (decimal)values[values.IndexOf((double)this.Value) + 1];

                UpdateEditText();
            }

            /// <summary>
            /// Overrides the method such that only legal values of the option are allowed.
            /// 
            /// If the minimum is already selected, there will be no decrease.
            /// </summary>
            public override void DownButton()
            {
                if (this.Value != Minimum)
                    this.Value = (decimal)values[values.IndexOf((double)this.Value) - 1];

                UpdateEditText();
            }
        }

        /// <summary>
        /// This class is needed to deactivate the double click of the tree view
        /// due to a bug (TreeNode checked state inconsistent)
        /// </summary>
        private class NoClickTree : TreeView
        {
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x203)
                    m.Result = IntPtr.Zero;
                else
                    base.WndProc(ref m);
            }
        }
    }
}
