using CommandLine;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using MachineLearning;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SPLConqueror_GUI
{
    public partial class MainWindow : Form
    {
        // All string constants for this class
        private const string bracketTab = "     ";
        private const string filteringListBox = "Free filtering";
        private const string filteringTreeView = "Configuration filtering";
        private const string secondEmptyOption = "---------------";
        private const string buttonPressRequest = "Please press the button.";
        private const string performanceAxisLabel = "Calculated Performance";
        private const string correspondingValuesLabel = "Corresponding values";
        private const string measuredValueLabel = "Measured Values";
        private const string absoluteDifferenceLabel = "Absolute Difference";
        private const string relativeDifferenceLabel = "Relative Difference in %";
        private const string failureNoModel = "You have to load a model to generate a graph!";
        private const string failureDoubleOption = "You may not choose the same option twice!";
        private const string comboboxInteractionsOption = "Interactions";
        private const string comboboxConstantOption = "Constants";
        private const string comboboxRangeOption = "Constants Range";
        private const string comboboxRangeOccuranceOption = "Constants Range Occurance";
        private const string comboboxBothOption = "Both";
        private const string comboboxMeasurementsOption = "Measurements only";
        private const string comboboxAbsoluteDifferenceOption = "Absolute Difference";
        private const string comboboxRelativeDifferenceOption = "Relative Difference";
        private const string errorNoMeasurmentsLoaded = "Please load some measurements.";
        private const string errorNoPerformances = "Please calculate a graph first.";
        private const string errorIllegalConfiguration = "The current configuration is not valid.";
        private const string errorNoMeasurementsAvailable = "There are no measurements for the current settings.";

        // Colors for the Configuration filtering
        private Color normalColor = Color.White;
        private Color deactivatedColor = Color.SlateGray;

        private InfluenceFunction originalFunction;
        private VariabilityModel currentModel;
        private string[] adjustedExpressionTree;
        private bool modelLoaded = false;
        private double maxAbstractConstant = 0.0;
        private Dictionary<string, int> occuranceOfOptions = new Dictionary<string, int>();
        private Dictionary<NumericOption, float> numericSettings = new Dictionary<NumericOption, float>();
        private Dictionary<ConfigurationOption, double> factorizationPriorities =
            new Dictionary<ConfigurationOption, double>();
        private MachineLearning.Solver.ICheckConfigSAT sat = new MachineLearning.Solver.CheckConfigSAT(
            Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
            System.IO.Directory.GetCurrentDirectory()))) + "\\dll\\Microsoft.Solver.Foundation.dll");

        // Everything for the measurements
        private Configuration configurationForCalculation;
        private Tuple<NumericOption, NumericOption> chosenOptions;
        private ILArray<float> drawnPerformances;
        private ILArray<float> calculatedPerformances;
        private bool measurementsLoaded = false;
        private Color measurementColor = Color.Green;
        private string adjustedMeasurementFunction;

        // For reading in the right numbers
        private NumberStyles style = NumberStyles.Number;
        private CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        /// <summary>
        /// Constructor of this class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
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

                if (!isOperator(parts[1]) && parts[0] != "log10(")
                    expression = String.Join(" ", parts, 1, parts.Length - 1);
                else
                    expression = String.Join(" ", parts, 0, parts.Length);
            }

            return expression;
        }

        /// <summary>
        /// Opens a dialog to load the desired variability model.
        /// 
        /// If the user enters an inavlid XML-file that is not a variability model, an error message will be shown.
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
                    MessageBox.Show("The entered variability model is not valid.");
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// One window will be opened to load the expression. By doing this action, the user
        /// will not be able to generate a function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadMeasurementButton_Click(object sender, EventArgs e)
        {
            if (!modelLoaded)
            {
                MessageBox.Show("Please load a function with its variability model first!");
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
                MessageBox.Show("The file doesn't match with the current variability model!");
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
                MessageBox.Show("The read expression does not work with the loaded variability model!");
                return;
            }

            String optExpression = Regex.Replace(exp, @"\r\n?|\n", "");

            // Test if the loaded expression can be used
            try
            {
                new InfluenceFunction(optExpression);
            } catch
            {
                MessageBox.Show("The read expression is in an invalid form.");
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
            getMaxAbstractConstant();
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
                    && !double.TryParse(prt, style, culture, out d)
                    && !(prt.Contains('.') && (prt.Contains('E') || prt.Contains('e')))
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

            MachineLearning.Solver.VariantGenerator varGen = new MachineLearning.Solver.VariantGenerator(
                Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))
                + "\\dll\\Microsoft.Solver.Foundation.dll");

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
        /// Initializes all comonents that need to be initialized only once the programm started.
        /// </summary>
        private void initializeOnce()
        {
            constantDecimalCheckBox.Enabled = true;
            constantFilteringCheckbox.Enabled = true;
            filterVariablesCheckbox.Enabled = true;
            firstAxisCombobox.Enabled = true;
            secondAxisCombobox.Enabled = true;
            settingsButton.Enabled = true;
            normalRadioButton.Enabled = true;
            factorRadioButton.Enabled = true;

            chartComboBox.Enabled = true;
            chartComboBox.Items.Add(comboboxInteractionsOption);
            chartComboBox.Items.Add(comboboxConstantOption);
            chartComboBox.Items.Add(comboboxRangeOption);
            chartComboBox.Items.Add(comboboxRangeOccuranceOption);

            measurementViewCombobox.Items.Add(comboboxBothOption);
            measurementViewCombobox.Items.Add(comboboxMeasurementsOption);
            measurementViewCombobox.Items.Add(comboboxAbsoluteDifferenceOption);
            measurementViewCombobox.Items.Add(comboboxRelativeDifferenceOption);
            measurementViewCombobox.SelectedIndex = 0;

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

            foreach (NumericOption option in originalFunction.participatingNumOptions)
                numericSettings.Add(option, (float) option.DefaultValue);

            // Evaluation configuration
            calculationResultLabel.Text = buttonPressRequest;
            updateEvaluationConfiguration();

            // Measurement view
            initializeMeasurementView();
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

            bothGraphsIlPanel.Scene = new ILScene();
            measurementsOnlyIlPanel.Scene = new ILScene();
            absoluteDifferenceIlPanel.Scene = new ILScene();
            relativeDifferenceIlPanel.Scene = new ILScene();
            bothGraphsIlPanel.Refresh();
            measurementsOnlyIlPanel.Refresh();
            absoluteDifferenceIlPanel.Refresh();
            relativeDifferenceIlPanel.Refresh();
        }

        /// <summary>
        /// Initializes the constraintTextbox with the current constraints.
        /// </summary>
        private void initializeConstraintView()
        {
            constraintTextbox.Clear();

            if (currentModel.BooleanConstraints.Count != 0)
            {
                constraintTextbox.AppendText("Boolean constraints:\n\n");

                foreach (string constraint in currentModel.BooleanConstraints)
                    constraintTextbox.AppendText(constraint + "\n");

                constraintTextbox.AppendText("\n\n");
            }

            if (currentModel.NonBooleanConstraints.Count != 0)
            {
                constraintTextbox.AppendText("Non-Boolean constraints:\n\n");

                foreach (NonBooleanConstraint constraint in currentModel.NonBooleanConstraints)
                    constraintTextbox.AppendText(constraint.ToString() + "\n");

                constraintTextbox.AppendText("\n\n");
            }

            if (constraintTextbox.Text == "")
                constraintTextbox.AppendText("No constraints for this model...");
        }

        /// <summary>
        /// Initializes all possible options to configurate the adjusted function.
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
            // Initializing combobox
            filterOptionCombobox.Items.Clear();
            filterOptionCombobox.Items.Add(filteringTreeView);
            filterOptionCombobox.Items.Add(filteringListBox);
            filterOptionCombobox.SelectedIndex = 0;
            filterVariablesCheckbox.Checked = false;

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
        }

        /// <summary>
        /// Invokes after the check state of a tree node has changed.
        /// 
        /// While working through this method, this handler will be deactivated. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ownAfterCheck(object sender, TreeViewEventArgs e)
        {
            variableTreeView.AfterCheck -= ownAfterCheck;

            setChildrenChecked(e.Node);
            updateTreeView();
            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();

            variableTreeView.AfterCheck += ownAfterCheck;
        }

        /// <summary>
        /// Creates a tree node out of the specified option.
        /// 
        /// While doing that all children of this option (according to the variability model) will be
        /// added as children of this node.
        /// </summary>
        /// <param name="val">Option that is about to be inserted into the tree view. Must not be null.</param>
        /// <returns>The corresponding tree node ready for insertion.</returns>
        private TreeNode insertIntoTreeView(ConfigurationOption val)
        {
            if (val == null)
                throw new ArgumentException("Parameter val must not be null!");

            List<TreeNode> functionChildren = new List<TreeNode>();
            List<ConfigurationOption> allChildren;

            if (currentModel.parentChildRelationships.TryGetValue(val, out allChildren))
            {
                foreach (ConfigurationOption child in allChildren)
                    functionChildren.Add(insertIntoTreeView(child));
            }

            TreeNode current = new TreeNode(val.Name, functionChildren.ToArray());

            if (currentModel.BinaryOptions.Contains(val) && !((BinaryOption)val).Optional && !((BinaryOption)val).hasAlternatives())
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
        /// loaded InfluenceFunction.
        /// </summary>
        /// <returns>Maximum amount of used digits</returns>
        private decimal getMaxDigits()
        {
            int maxDigits = 0;

            foreach (string prt in originalFunction.getExpressionTree())
            {
                double i;

                if (double.TryParse(prt, style, culture, out i))
                {
                    int j = prt.Split(new char[] {'.', ',' })[1].Length;

                    if (maxDigits < j)
                        maxDigits = j;
                }
            }

            return maxDigits;
        }

        /// <summary>
        /// Calculates and returns all interactions of the adjusted function.
        /// </summary>
        /// <returns>Dictionary with all interactions of each option</returns>
        private Dictionary<string, Dictionary<string, int>> getInteractions()
        {
            Dictionary<string, Dictionary<string, int>> currInteractions =
                new Dictionary<string, Dictionary<string, int>>();

            foreach (string component in adjustedMeasurementFunction.ToString().Split('+'))
            {
                //Getting a list of all options in a component
                List<ConfigurationOption> componentOptions = new List<ConfigurationOption>();

                foreach (string part in component.Split(' '))
                {
                    ConfigurationOption option = currentModel.getOption(part);

                    if (option != null && !componentOptions.Contains(option))
                        componentOptions.Add(option);
                }

                ConfigurationOption[] options = componentOptions.ToArray();

                // Calculating all interactions for the list of found options
                for (int i = 0; i < options.Length; i++)
                {
                    Dictionary<string, int> optionInteractions;

                    if (!currInteractions.TryGetValue(options[i].Name, out optionInteractions))
                        optionInteractions = new Dictionary<string, int>();

                    for (int j = 0; j < options.Length; j++)
                    {
                        if (i != j)
                        {
                            int count;

                            if (optionInteractions.TryGetValue(options[j].Name, out count))
                            {
                                optionInteractions.Remove(options[j].Name);
                                optionInteractions.Add(options[j].Name, count + 1);
                            }
                            else
                                optionInteractions.Add(options[j].Name, 1);
                        }
                    }

                    currInteractions.Remove(options[i].Name);
                    currInteractions.Add(options[i].Name, optionInteractions);
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

            if (!filterVariablesCheckbox.Checked)
            {
                foreach (ConfigurationOption opt in currentModel.getOptions())
                    legalOptions.Add(opt.ToString());

                return legalOptions;
            }

            switch (filterOptionCombobox.SelectedItem.ToString())
            {
                case filteringListBox:
                    foreach (string s in variableListBox.Items)
                    {
                        if (!variableListBox.CheckedItems.Contains(s))
                            legalOptions.Add(s);
                    }
                    break;

                case filteringTreeView:
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

            return legalOptions;
        }

        /// <summary>
        /// Displays all interactions in the corresponding tab.
        /// </summary>
        private void displayInteractions()
        {
            interactionTextBox.Clear();

            foreach (KeyValuePair<string, Dictionary<string, int>> entry in getInteractions())
            {
                // Appending text to display all interactions
                interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Bold);
                interactionTextBox.AppendText(entry.Key);
                interactionTextBox.SelectionFont = new Font(interactionTextBox.Font, FontStyle.Regular);
                interactionTextBox.AppendText("\ninteracts with: \t");

                if (entry.Value.Count == 0)
                    interactionTextBox.AppendText("nothing...\n");
                else
                {
                    KeyValuePair<string, int>[] values = entry.Value.ToArray();

                    for (int i = 0; i < values.Length; i++)
                        interactionTextBox.AppendText("- " + values[i].Key + " (" + values[i].Value + ")\n\t\t");
                }

                interactionTextBox.AppendText("\n");
            }
        }

        /// <summary>
        /// Extracts the maximum abstract constant of the original Influence_Function.
        /// </summary>
        private void getMaxAbstractConstant()
        {
            List<double> constants = new List<double>();

            foreach (string part in originalFunction.ToString().Split(' '))
            {
                double i = 0.0;

                if (double.TryParse(part, style, culture, out i))
                    constants.Add(Math.Abs(i));
            }

            maxAbstractConstant = constants.Count == 0 ? 1.0 : constants.Max();
        }

        /// <summary>
        /// Invokes if the corresponding button (settingsButton) was pressed.
        /// 
        /// This opens a new form for the custom setting of default values of the NumericOptions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsButton_Click(object sender, EventArgs e)
        {
            Form form = new NumericSettings(numericSettings);
            form.ShowDialog();
        }

        /// <summary>
        /// Invokes if the corresponding button (generaterFunctionButton) has been pressed.
        /// 
        /// By clicking on the button the ilFunctionPanel will draw the adjusted function.
        /// Additionally, all information about the created function will be saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// and the constants of the Influence_Function can be adjusted. If the amount of digits
        /// have been changed in a previous adjustment, it will be considered.
        /// If the checkbox is deselected, the corresponding NumericUpDown-element will be deactivated
        /// and the adjusted function will get the original constants.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void constantDecimalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            constantsDigitsUpDown.Enabled = constantDecimalCheckBox.Checked;

            updateAdjustedFunction();
            updateCharts();
        }

        /// <summary>
        /// Invokes if the value has changed.
        /// 
        /// The counter handles the amount of digits of the constant values in the Influence_Function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void constantsDigitsUpDown_ValueChanged(object sender, EventArgs e)
        {
            updateAdjustedFunction();
            updateCharts();
        }

        /// <summary>
        /// Invokes if the mouse wheel is used on the constantsDigitsUpDown.
        /// 
        /// This method prevents that the mousewheel is used such that it will not
        /// throw an InvalidRangeException.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void constantFilteringCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            constantRelativeValueSlider.Enabled = constantFilteringCheckbox.Checked;

            updateAdjustedFunction();
            updateCharts();
        }

        /// <summary>
        /// Invokes if the specified trackbar was moved.
        /// 
        /// The trackbar indicates how much the abstract value of the constants will be filtered.
        /// If the trackbar is at the minimum value, there will be no filtering.
        /// If the trackbar is at the maximum value, only the biggest abstract will be shown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void constantRelativeValueSlider_Scroll(object sender, EventArgs e)
        {
            updateAdjustedFunction();
            updateCharts();
        }

        /// <summary>
        /// Invokes if the status of the specified checkbox changes.
        /// 
        /// If the checkbox is selected, the filtering of the variables is activated.
        /// All already checked and unchecked variables from a previous adjustment will be considered.
        /// If the checkbox is deselected, the filtering will be deactivated and all variables will be
        /// used in the Influence_Function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// view will be shown and enabled. After choosing a new index the adjusted function and configuration
       /// options will be updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterOptionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            variableListBox.Visible = false;
            variableListBox.Enabled = false;
            variableTreeView.Visible = false;
            variableTreeView.Enabled = false;

            switch (filterOptionCombobox.SelectedItem.ToString())
            {
                case filteringListBox:
                    variableListBox.Visible = true;
                    variableListBox.Enabled = true;
                    break;

                case filteringTreeView:
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void variableListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateEvaluationConfiguration();
            updateAdjustedFunction();
            updateInteractionsTab();
        }

        /// <summary>
        /// Invokes if the first axis has changed.
        /// 
        /// If both axes are the same, the generateFunction-button will be disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void firstAxisCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGenerateButton();
        }

        /// <summary>
        /// Invokes if the second axis has changed.
        /// 
        /// If both axes are the same, the generateFunction-button will be disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secondAxisCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateGenerateButton();
        }

        /// <summary>
        /// Invokes if the calculatePerformanceButton has been pressed.
        /// 
        /// This will calculate the performance of the function if there are no numeric
        /// options in the function itself. All currently available binary options
        /// will be set to 1.0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (double.TryParse(part, style, culture, out d)
                    || part.Contains(".") && (part.Contains("E") || part.Contains("e"))
                    || isOperator(part))
                    expressionParts[i] = part;
                else
                    expressionParts[i] = "1.0";
            }

            calculationResultLabel.Text = calculateFunctionExpression(expressionParts);
        }

        /// <summary>
        /// Invokes if the check state of the corresponding radiobutton (normalRadioButton) has changed.
        /// 
        /// If checked, the adjustedTextbox will display the adjusted function without any
        /// special form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            influenceChart.Visible = false;
            constantChart.Visible = false;
            rangeChart.Visible = false;
            rangeOccuranceChart.Visible = false;

            String s = chartComboBox.SelectedItem.ToString();

            switch (chartComboBox.SelectedItem.ToString())
            {
                case comboboxInteractionsOption:
                    influenceChart.Visible = true;
                    break;
                case comboboxConstantOption:
                    constantChart.Visible = true;
                    break;
                case comboboxRangeOption:
                    rangeChart.Visible = true;
                    break;
                case comboboxRangeOccuranceOption:
                    rangeOccuranceChart.Visible = true;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void measurementViewCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMeasurementPanel();
        }

        /// <summary>
        /// Invokes if another element of the corresponding combobox (nfpValueCombobox) was selected.
        /// 
        /// It will switch the NFProperty that will be read from the measurements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nfpValueCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateMeasurementTab();
        }

        /// <summary>
        /// Updates the variable configuration.
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
                    case filteringListBox:
                        variableListBox.Enabled = true;
                        break;
                    case filteringTreeView:
                        variableTreeView.Enabled = true;
                        break;
                }
            }
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
                    if (!filterVariablesCheckbox.Checked || legalOptions.Contains(option.Name))
                    {
                        firstAxisCombobox.Items.Add(option.Name);
                        secondAxisCombobox.Items.Add(option.Name);
                    }
                }

                if (firstAxisCombobox.Items.Count != 0)
                {
                    secondAxisCombobox.Items.Add(secondEmptyOption);

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
        /// Configurates the label for the error message of the function configuration.
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
                    failureLabel.Text = failureNoModel;
                else if (firstAxisCombobox.SelectedItem != null
                    && firstAxisCombobox.SelectedItem.Equals(secondAxisCombobox.SelectedItem))
                    failureLabel.Text = failureDoubleOption;

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
        /// </summary>
        private void updateCharts()
        {
            List<string> legalOptions = getLegalOptions();

            // Update influence chart
            influenceChart.Series.Clear();
            influenceChart.Series.Add("Series1");
            influenceChart.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            foreach (KeyValuePair<string, Dictionary<string, int>> entry in getInteractions())
            {
                int value = 0;

                foreach (KeyValuePair<string, int> pair in entry.Value)
                    value += pair.Value;

                if (value != 0)
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point.Label = entry.Key + " (#VALY)";
                    point.SetValueY(value);

                    influenceChart.Series["Series1"].Points.Insert(0, point);
                }
            }

            // Calculate the constant influences
            Dictionary<string, double> constantInfluences = new Dictionary<string, double>();
            Dictionary<string, double> constantRangeInfluences = new Dictionary<string, double>();
            
            foreach (string component in adjustedMeasurementFunction.ToString().Split('+'))
            {
                double constant = 1.0;
                double currConstantRange = 0;
                List<string> variables = new List<string>();

                foreach (string part in component.Split(' '))
                {
                    double num = 0.0;
                    bool isConstant = double.TryParse(part, style, culture, out num);

                    constant = isConstant ? num : constant;

                    if (!isConstant && part != "")
                    {
                        if (part.Contains(".") && (part.Contains("E") || part.Contains("e")))
                        {
                            string[] parts = part.Split(new char[] { 'E', 'e' });
                            double numFirst = 0.0;
                            double numSecond = 0.0;

                            double.TryParse(parts[0], style, culture, out numFirst);
                            double.TryParse(parts[1], style, culture, out numSecond);

                            constant = numFirst * Math.Pow(10, numSecond);
                        }
                        else if (!isOperator(part))
                            variables.Add(part);
                    }
                }

                currConstantRange = constant;

                foreach (string var in variables)
                {
                    // Calculating constant influences
                    double oldValue = 0.0;
                    
                    if (constantInfluences.TryGetValue(var, out oldValue))
                        constantInfluences.Remove(var);

                    constantInfluences.Add(var, oldValue + Math.Abs(constant));

                    // Calculating range influences
                    NumericOption opt = currentModel.getNumericOption(var);

                    if (opt != null)
                        currConstantRange = currConstantRange * opt.Max_value;                        
                }

                foreach (string var in variables)
                {
                    double oldValue = 0.0;

                    if (constantRangeInfluences.TryGetValue(var, out oldValue))
                        constantRangeInfluences.Remove(var);

                    constantRangeInfluences.Add(var, oldValue + Math.Abs(currConstantRange));
                }

            }
            
            // Update other charts
            constantChart.Series.Clear();
            constantChart.Series.Add("Series1");
            constantChart.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            rangeChart.Series.Clear();
            rangeChart.Series.Add("Series1");
            rangeChart.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            rangeOccuranceChart.Series.Clear();
            rangeOccuranceChart.Series.Add("Series1");
            rangeOccuranceChart.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            
            // Update constant chart
            foreach (KeyValuePair<string, double> entry in constantInfluences)
            {
                if (legalOptions.Contains(entry.Key))
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point.Label = entry.Key;
                    point.SetValueY(entry.Value);

                    constantChart.Series["Series1"].Points.Insert(0, point);
                }
            }

            // Update range and range occurance chart
            foreach (KeyValuePair<string, double> entry in constantRangeInfluences)
            {
                if (legalOptions.Contains(entry.Key))
                {
                    System.Windows.Forms.DataVisualization.Charting.DataPoint point1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point1.Label = entry.Key;
                    point1.SetValueY(entry.Value);

                    rangeChart.Series["Series1"].Points.Insert(0, point1);

                    int i = 0;

                    if (!occuranceOfOptions.TryGetValue(entry.Key, out i))
                        i = 1;

                    System.Windows.Forms.DataVisualization.Charting.DataPoint point2 = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                    point2.Label = entry.Key;
                    point2.SetValueY(entry.Value / i);

                    rangeOccuranceChart.Series["Series1"].Points.Insert(0, point2);
                }
            }
        }

        /// <summary>
        /// Updates the adjusted function.
        /// 
        /// This is done by looking which options have been selected and by adjusting the
        /// original Influence_Function.
        /// </summary>
        private void updateAdjustedFunction()
        {
            string[] expressionParts = new string[originalFunction.getExpressionTree().Length];

            for (int i = 0; i < expressionParts.Length; i++)
                expressionParts[i] = String.Copy(originalFunction.getExpressionTree()[i]);

            // Adjusting constants
            if (constantDecimalCheckBox.Checked || constantFilteringCheckbox.Checked)
            {
                int maxDigits = decimal.ToInt32(constantsDigitsUpDown.Value);
                double minAbstract = maxAbstractConstant * constantRelativeValueSlider.Value / 100;

                for (int i = 0; i < expressionParts.Length; i++)
                {
                    string part = expressionParts[i];
                    double number = 0.0;

                    if (double.TryParse(part, style, culture, out number))
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
                    else if (part.Contains(".") && (part.Contains("E") || part.Contains("e")))
                    {
                        string[] numParts = part.Split(new char[] {'E', 'e' });
                        double numFst = 0.0;
                        double numSnd = 0.0;

                        double.TryParse(numParts[0], style, culture, out numFst);
                        double.TryParse(numParts[1], style, culture, out numSnd);

                        double constant = numFst * Math.Pow(10, numSnd);

                        if (Math.Abs(constant) < minAbstract)
                            part = "0.0";

                        if (constantDecimalCheckBox.Checked && part != "0.0")
                        {
                            string[] parts;

                            if (part.Contains("."))
                                parts = part.Split(new char[] { 'E', 'e', '.' });
                            else
                                throw new Exception("Unknown style found! Use . or , for your constants!");

                            if (maxDigits == 0)
                                part = parts[0] + ".0E" + parts[2];
                            else if (parts[1].Length > maxDigits)
                                part = parts[0] + "." + parts[1].Substring(0, maxDigits) + "E" + parts[2];
                        }
                    }

                    expressionParts[i] = part;
                }
            }

            // Adjusting variables
            if (filterVariablesCheckbox.Checked)
                expressionParts = filterVariables(expressionParts);

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
                if (prt.Equals("+") || prt.Equals("*"))
                {
                    Tuple<string, string> second = stack.Pop();
                    Tuple<string, string> first = stack.Pop();
                    double numFst = -1.0;
                    double numSnd = -1.0;

                    bool firstSuccess = double.TryParse(first.Item1, style, culture, out numFst);
                    bool secondSuccess = double.TryParse(second.Item1, style, culture, out numSnd);

                    // Check if the first number is an E-number
                    if (first.Item1.Contains(".") && (first.Item1.Contains("E") || first.Item1.Contains("e")))
                    {
                        string[] parts = first.Item1.Split(new char[] { 'E', 'e' });
                        double exponent;

                        double.TryParse(parts[0], style, culture, out numFst);
                        double.TryParse(parts[1], style, culture, out exponent);

                        numFst = numFst * Math.Pow(10, exponent);
                        firstSuccess = true;
                    }

                    // Check if the second number is an E-number
                    if (second.Item1.Contains(".") && (second.Item1.Contains("E") || second.Item1.Contains("e")))
                    {
                        string[] parts = second.Item1.Split(new char[] { 'E', 'e' });
                        double exponent;

                        double.TryParse(parts[0], style, culture, out numSnd);
                        double.TryParse(parts[1], style, culture, out exponent);

                        numSnd = numSnd * Math.Pow(10, exponent);
                        secondSuccess = true;
                    }

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

                                    temp = first.Item2 == "+" ? "( " + first.Item1 + " )" : first.Item1;
                                    temp = temp + " * ";
                                    temp = temp + (second.Item2 == "+" ? "( " + second.Item1 + " )" : second.Item1);

                                    simple = Tuple.Create<string, string>(temp, "*");
                                }
                                break;
                            default:
                                throw new Exception("Unknown operation found!");
                        }

                        stack.Push(simple);

                    }
                    else
                    {
                        if (prt == "*")
                        {
                            string temp = "";

                            temp = first.Item2 == "+" ? "( " + first.Item1 + " )" : first.Item1;
                            temp = temp + " * ";
                            temp = temp + (second.Item2 == "+" ? "( " + second.Item1 + " )" : second.Item1);

                            stack.Push(Tuple.Create<string, string>(temp, "*"));
                        }
                        else
                            stack.Push(Tuple.Create<string, string>(first.Item1 + " " + prt + " " + second.Item1, prt));
                    }
                }
                else if (prt.Equals("[") || prt.Equals("]"))
                {
                    // If there is a log10 in the expression, calculate the inner value of the
                    // logarithm first.

                    switch (prt) {

                    case "[":
                                bool done = false;
                    int pos = i;
                    int counter = 0;

                    while (!done)
                    {
                        pos++;

                        switch (expParts[pos])
                        {
                            case "[":
                                counter++;
                                break;
                            case "]":
                                if (counter == 0)
                                    done = true;
                                else
                                    counter--;
                                break;
                        }
                    }

                    string[] innerLog = new string[pos - i - 1];
                    Array.Copy(expParts, i + 1, innerLog, 0, pos - i - 1);

                    string result = calculateFunctionExpression(innerLog);

                    stack.Push(Tuple.Create<string, string>("log10( " + result + " )", "log10"));

                    i = pos;
                    break;
                        case "]":
                            throw new Exception("The inner notation consists of ']' in unexpected places.");
                    }
                }
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
        /// </summary>
        private void updateMeasurementTab()
        {
            // Check if measurements are loaded
            if (!measurementsLoaded)
            {
                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = errorNoMeasurmentsLoaded;
                return;
            }

            // Check if a graph has been calculated before
            if (configurationForCalculation == null)
            {
                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = errorNoPerformances;
                return;
            }

            // Check if the current configuration is possible
            if (!sat.checkConfigurationSAT(configurationForCalculation.BinaryOptions.Keys.ToList(), currentModel, true))
            {
                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = errorIllegalConfiguration;
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

            // Check if there are any measurements for the current settings
            if (neededConfigurations.Count == 0)
            {
                nfpValueCombobox.Enabled = false;
                measurementViewCombobox.Enabled = false;
                measurementErrorLabel.Visible = true;
                measurementErrorLabel.Text = errorNoMeasurementsAvailable;
                return;
            }

            measurementErrorLabel.Visible = false;
            measurementViewCombobox.Enabled = true;
            nfpValueCombobox.Enabled = true;

            if (nfpValueCombobox.Items.Count == 0)
            {
                foreach (KeyValuePair<string, NFProperty> entry in GlobalState.nfProperties.ToList())
                    nfpValueCombobox.Items.Add(entry.Key);

                nfpValueCombobox.SelectedIndex = 0;
            }

            ILPlotCube bothGraphsCube, measurementsOnlyCube, absoluteDifferenceCube, relativeDifferenceCube;
            NFProperty prop = new NFProperty(nfpValueCombobox.SelectedItem.ToString());

            // Decide if there has to be a 2D or 3D shape
            if (chosenOptions.Item2 == null)
            {
                // Define plot cubes
                bothGraphsCube = new ILPlotCube(twoDMode: true);
                measurementsOnlyCube = new ILPlotCube(twoDMode: true);
                absoluteDifferenceCube = new ILPlotCube(twoDMode: true);
                relativeDifferenceCube = new ILPlotCube(twoDMode: true);

                bothGraphsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                bothGraphsCube.Axes.YAxis.Label.Text = correspondingValuesLabel;
                measurementsOnlyCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                measurementsOnlyCube.Axes.YAxis.Label.Text = measuredValueLabel;
                absoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                absoluteDifferenceCube.Axes.YAxis.Label.Text = absoluteDifferenceLabel;
                relativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                relativeDifferenceCube.Axes.YAxis.Label.Text = relativeDifferenceLabel;

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

                    for (int j = 0; j < neededConfigurations.Count && c == null; j++)
                    {
                        neededConfigurations[j].NumericOptions.TryGetValue(chosenOptions.Item1, out d);

                        if (d == value)
                            c = neededConfigurations[j];
                    }

                    if (c == null)
                    {
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

                            XY = ILMath.zeros<float>(0, 0);
                            absoluteDifferences = ILMath.zeros<float>(0, 0);
                            relativeDifferences = ILMath.zeros<float>(0, 0);
                            pos = 0;
                        }
                    }
                    else
                    {
                        c.nfpValues.TryGetValue(prop, out d);
                        XY[0, pos] = (float)value;
                        XY[1, pos] = (float)d;
                        absoluteDifferences[0, pos] = (float)value;
                        absoluteDifferences[1, pos] = ILMath.abs((float)d - calculatedPerformances[1, values.IndexOf(value)]);
                        relativeDifferences[0, pos] = (float)value;
                        relativeDifferences[1, pos] = absoluteDifferences[1, pos] >= 1 ? absoluteDifferences[1, pos]/XY[1, pos] * 100 : 0;

                        ILPoints point = createPoint(XY[0, pos], XY[1, pos], 0, measurementPointLabel);

                        // Adding events to the point to display its coordinates on the screen
                        point.MouseMove += (s, a) =>
                        {
                            Vector3 coor = point.GetPosition();

                            measurementPointLabel.Text = chosenOptions.Item1.Name + ": " + coor.X.ToString() + ", " + performanceAxisLabel + ": " + coor.Y.ToString();
                            measurementPointLabel.Visible = true;
                        };

                        pos++;

                        bothGraphsCube.Add(point);
                        measurementsOnlyCube.Add(point);
                    }
                }

                bothGraphsCube.Add(new ILLinePlot(XY)
                {
                    ColorOverride = measurementColor
                });
                bothGraphsCube.Add(new ILLinePlot(calculatedPerformances));
                measurementsOnlyCube.Add(new ILLinePlot(XY)
                {
                    ColorOverride = measurementColor
                });
                absoluteDifferenceCube.Add(new ILLinePlot(absoluteDifferences));
                absoluteDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));
                relativeDifferenceCube.Add(new ILLinePlot(relativeDifferences));
                relativeDifferenceCube.Add(new ILLinePlot(ILMath.zeros<float>(1, 1)));
            }
            else
            {
                ILArray<float> A, X, Y, absoluteDifferences, relativeDifferences;

                bothGraphsCube = new ILPlotCube(twoDMode: false);
                measurementsOnlyCube = new ILPlotCube(twoDMode: false);
                absoluteDifferenceCube = new ILPlotCube(twoDMode: false);
                relativeDifferenceCube = new ILPlotCube(twoDMode: false);

                bothGraphsCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                bothGraphsCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                bothGraphsCube.Axes.ZAxis.Label.Text = correspondingValuesLabel;
                measurementsOnlyCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                measurementsOnlyCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                measurementsOnlyCube.Axes.ZAxis.Label.Text = measuredValueLabel;
                absoluteDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                absoluteDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                absoluteDifferenceCube.Axes.ZAxis.Label.Text = absoluteDifferenceLabel;
                relativeDifferenceCube.Axes.XAxis.Label.Text = chosenOptions.Item1.Name;
                relativeDifferenceCube.Axes.YAxis.Label.Text = chosenOptions.Item2.Name;
                relativeDifferenceCube.Axes.ZAxis.Label.Text = relativeDifferenceLabel;

                X = Array.ConvertAll(chosenOptions.Item1.getAllValues().ToArray(), x => (float)x);
                Y = Array.ConvertAll(chosenOptions.Item2.getAllValues().ToArray(), y => (float)y);

                ILArray<float> XMat = 1;
                ILArray<float> YMat = ILMath.meshgrid(Y, X, XMat);

                A = ILMath.zeros<float>(X.Length, Y.Length, 3);
                absoluteDifferences = ILMath.zeros<float>(X.Length, Y.Length, 3);
                relativeDifferences = ILMath.zeros<float>(X.Length, Y.Length, 3);

                // Fill array with values
                A[":;:;1"] = XMat;
                A[":;:;2"] = YMat;
                
                List<double> valuesX = chosenOptions.Item1.getAllValues();
                List<double> valuesY = chosenOptions.Item2.getAllValues();
                valuesX.Sort();
                valuesY.Sort();

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

                        if (c == null)
                            A[i, j, 0] = float.NegativeInfinity;
                        else
                        {
                            c.nfpValues.TryGetValue(prop, out d1);
                            A[i, j, 0] = (float)d1;

                            ILPoints point = createPoint(A[i, j, 1], A[i, j, 2], A[i, j, 0], measurementPointLabel);

                            // Adding events to the point to display its coordinates on the screen
                            point.MouseMove += (s, a) =>
                            {
                                Vector3 coor = point.GetPosition();

                                measurementPointLabel.Text = chosenOptions.Item1.Name + ": " + coor.X.ToString() + ", " + chosenOptions.Item2.Name + ": " + coor.Y.ToString() + ", " + performanceAxisLabel + ": " + coor.Z.ToString();
                                measurementPointLabel.Visible = true;
                            };

                            bothGraphsCube.Add(point);
                            measurementsOnlyCube.Add(point);
                        }
                    }
                }

                for (int i = 0; i < A.Size[0]; i++)
                {
                    for (int j = 0; j < A.Size[1]; j++)
                    {
                        absoluteDifferences[i, j, 0] = A[i, j, 0] == float.NegativeInfinity
                            ? float.NegativeInfinity : Math.Abs(A[i, j, 0].GetArrayForRead()[0] - calculatedPerformances[i, j, 0].GetArrayForRead()[0]);
                        absoluteDifferences[i, j, 1] = A[i, j, 1].GetArrayForRead()[0];
                        absoluteDifferences[i, j, 2] = A[i, j, 2].GetArrayForRead()[0];
                        relativeDifferences[i, j, 0] = A[i, j, 0] == float.NegativeInfinity
                            ? float.NegativeInfinity : (absoluteDifferences[i, j, 0] >= 1 ? absoluteDifferences[i, j, 0]/A[i, j, 0] * 100 : 0);
                        relativeDifferences[i, j, 1] = A[i, j, 1].GetArrayForRead()[0];
                        relativeDifferences[i, j, 2] = A[i, j, 2].GetArrayForRead()[0];
                    }
                }

                bothGraphsCube.Add(new ILSurface(A)
                    {
                        ColorMode = ILSurface.ColorModes.Solid
                    }
                );
                bothGraphsCube.Add(new ILSurface(calculatedPerformances));
                measurementsOnlyCube.Add(new ILSurface(A)
                    {
                        ColorMode = ILSurface.ColorModes.Solid
                    }
                );

                absoluteDifferenceCube.Add(new ILSurface(absoluteDifferences));
                absoluteDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3,3,3)));
                relativeDifferenceCube.Add(new ILSurface(relativeDifferences));
                relativeDifferenceCube.Add(new ILSurface(ILMath.zeros<float>(3, 3, 3)));
            }

            bothGraphsIlPanel.Scene = new ILScene { bothGraphsCube };
            measurementsOnlyIlPanel.Scene = new ILScene { measurementsOnlyCube };
            absoluteDifferenceIlPanel.Scene = new ILScene { absoluteDifferenceCube };
            relativeDifferenceIlPanel.Scene = new ILScene { relativeDifferenceCube };

            bothGraphsIlPanel.Refresh();
            measurementsOnlyIlPanel.Refresh();
            absoluteDifferenceIlPanel.Refresh();
            relativeDifferenceIlPanel.Refresh();
            
            updateMeasurementPanel();
        }

        /// <summary>
        /// Updates which IlPanel for the measurement is visible.
        /// 
        /// This is depending on the selected item in the corresponding combo box (measurementViewCombobox).
        /// </summary>
        private void updateMeasurementPanel()
        {
            bothGraphsPanel.Visible = false;
            measurementsOnlyPanel.Visible = false;
            absoluteDifferencePanel.Visible = false;
            relativeDifferencePanel.Visible = false;
            
            switch (measurementViewCombobox.SelectedItem.ToString())
            {
                case comboboxBothOption:
                    bothGraphsPanel.Visible = true;
                    break;
                case comboboxMeasurementsOption:
                    measurementsOnlyPanel.Visible = true;
                    break;
                case comboboxAbsoluteDifferenceOption:
                    absoluteDifferencePanel.Visible = true;
                    break;
                case comboboxRelativeDifferenceOption:
                    relativeDifferencePanel.Visible = true;
                    break;
                default:
                    throw new Exception("This should not happen. An unknown option was found.");
            }
        }

        /// <summary>
        /// Clears the specified RichTextBox and appends the specified function in a wellformed way.
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

            string[] expressionParts = function.Split('+');
            string tabs = "";

            textbox.Clear();

            for (int i = 0; i < expressionParts.Length; i++)
            {
                string[] partComponents = expressionParts[i].Split(' ');

                foreach (string component in partComponents)
                {
                    double d;
                    string addingComponent = component;

                    if (double.TryParse(component, style, culture, out d))
                    {
                        // Adds color for the constants
                        textbox.SelectionBackColor = Color.FromArgb((int) (255 * Math.Abs(d) / maxAbstractConstant),
                            (int) (255 * (maxAbstractConstant - Math.Abs(d)) / maxAbstractConstant), 0);
                    }
                    else if (component.Contains(".") && (component.Contains("E") || component.Contains("e")))
                    {
                        // Adds color for constants with *10^
                        string[] parts = component.Split(new char[] {'E', 'e' });
                        double exponent;

                        double.TryParse(parts[0], style, culture, out d);
                        double.TryParse(parts[1], style, culture, out exponent);

                        d = d * Math.Pow(10, exponent);
                        textbox.SelectionBackColor = Color.FromArgb((int)(255 * Math.Abs(d) / maxAbstractConstant),
                            (int)(255 * (maxAbstractConstant - Math.Abs(d)) / maxAbstractConstant), 0);
                    }
                    else if (addingComponent == "(" || addingComponent == "log10(")
                    {
                        // Adds one tab if there is an opening bracket
                        tabs += bracketTab;
                        addingComponent = tabs + addingComponent;

                        if (textbox.Text != "")
                            addingComponent = "\n" + addingComponent;
                    }
                    else if (addingComponent == ")")
                    {
                        // Removes one tab if there is a closing bracket
                        tabs = tabs.Remove(tabs.Length - bracketTab.Length);
                        addingComponent = addingComponent + tabs;
                    }

                    textbox.AppendText(addingComponent);
                    textbox.SelectionBackColor = Color.White;
                    textbox.AppendText(" ");
                }

                if (i < expressionParts.Length - 1)
                    textbox.AppendText("\n" + tabs + "+");
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
            if (secondAxisCombobox.SelectedItem.Equals(secondEmptyOption))
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
            cube.Axes.YAxis.Label.Text = performanceAxisLabel;

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

                    pointPositionLabel.Text = option.Name + ": " + coor.X.ToString() + ", " + performanceAxisLabel + ": " + coor.Y.ToString();
                    pointPositionLabel.Visible = true;
                };

                cube.Add(point);
            }

            // Adding a lineplot to the cube
            ILLinePlot linePlot = new ILLinePlot(XY);
            
            cube.Add(linePlot);

            ilFunctionPanel.Scene = new ILScene()
                {
                    cube
                };

            // Saving the coordinates/values of the points for the measurements
            calculatedPerformances = XY;
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
            cube.Axes.ZAxis.Label.Text = performanceAxisLabel;

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

                    pointPositionLabel.Text = firstOption.Name + ": " + coor.X.ToString() + ", " + secondOption.Name + ": " + coor.Y.ToString() + ", " + performanceAxisLabel + ": " + coor.Z.ToString();
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
                        stack.Push(stack.Pop() + stack.Pop());
                        done = true;
                        break;
                    case "*":
                        stack.Push(stack.Pop() * stack.Pop());
                        done = true;
                        break;
                    case "[":
                        switch (prt)
                        {
                            case "[":
                                bool innerDone = false;
                                int pos = i;
                                int counter = 0;

                                while (!innerDone)
                                {
                                    pos++;

                                    switch (expParts[pos])
                                    {
                                        case "[":
                                            counter++;
                                            break;
                                        case "]":
                                            if (counter == 0)
                                                innerDone = true;
                                            else
                                                counter--;
                                            break;
                                    }
                                }

                                string[] innerLog = new string[pos - i - 1];
                                Array.Copy(expParts, i + 1, innerLog, 0, pos - i - 1);

                                ILArray<float> calcResult = calculateFunction(innerLog, optNames, vars);
                                ILArray<float> logResult = ILMath.zeros<float>(1, 1);

                                // Be careful! Here: log(0) = 0
                                for (int j = 0; j < calcResult.Size[0]; j++)
                                    for (int k = 0; k < calcResult.Size[1]; k++)
                                        logResult[j, k] = calcResult[j, k] <= 0 ? ILMath.zeros<float>(1) : ILMath.log10(calcResult[j, k]);

                                stack.Push(logResult);

                                i = pos;
                                break;
                            case "]":
                                throw new Exception("The inner notation consists of ']' in unexpected places.");
                        }
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

                    if (float.TryParse(prt, style, culture, out num))
                        stack.Push(num);
                    else if (prt.Contains(".") && (prt.Contains("E") || prt.Contains("e")))
                    {
                        string[] parts = prt.Split(new char[] { 'E', 'e' });
                        double numFirst = 0.0;
                        double numSecond = 0.0;

                        double.TryParse(parts[0], style, culture, out numFirst);
                        double.TryParse(parts[1], style, culture, out numSecond);

                        stack.Push((float) (numFirst * Math.Pow(10, numSecond)));
                    }
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
            return token.Equals("+") || token.Equals("*") || token.Equals("[") || token.Equals("]");
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
            string[] expParts = exp.Split('+');
            Dictionary<string, int> counting = new Dictionary<string, int>();
            bool greaterThanOne = false;

            for (int i = 0; i < expParts.Length; i++)
                expParts[i] = expParts[i].Trim();

            foreach (string section in expParts)
            {
                foreach (string prt in section.Split(' '))
                {
                    double i;

                    if (!double.TryParse(prt, style, culture, out i)
                        && !(prt.Contains(".") && (prt.Contains("E") || prt.Contains("e")))
                        && !isOperator(prt))
                    {
                        if (counting.ContainsKey(prt))
                        {
                            counting[prt] += 1;
                            greaterThanOne = true;
                        }
                        else
                            counting.Add(prt, 1);
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
                    pair.Value.Sort(delegate(ConfigurationOption x, ConfigurationOption y)
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

                List<string> containsVar = new List<string>();
                List<string> others = new List<string>();
                List<string> varRest = new List<string>();

                foreach (string prt in expParts)
                {
                    string[] parts = prt.Split(' ');
                    int varPos = -1;
                    bool containsExactVar = false;

                    for (int i = 0; i < parts.Length && !containsExactVar; i++)
                    {
                        containsExactVar = parts[i].Equals(max);
                        varPos = i;
                    }
                    
                    if (containsExactVar)
                    {
                        if (prt.Equals(max))
                            varRest.Add("1.0");
                        else if (varPos == 0)
                        {
                            parts[0] = "";
                            parts[1] = "";
                            varRest.Add(String.Join(" ",  parts).Trim());
                        }
                        else
                        {
                            string temp = "";

                            for (int i = 0; i < parts.Length; i++)
                            {
                                if (i != varPos && i != varPos - 1 && !isOperator(parts[i]))
                                    temp = temp + parts[i] + " * ";
                            }

                            varRest.Add(temp.Remove(temp.Length - 3, 3));
                        }
                    }
                    else
                        others.Add(prt);
                }

                result = max + " * ( " + String.Join(" + ", varRest) + " ) + "
                    + factorizeExpression(String.Join("+", others));

            }
            else
            {
                for(int i = 0; i < expParts.Length; i++)
                {
                    result = result + expParts[i];

                    if (i < expParts.Length - 1)
                        result = result + " + ";
                }

            }

            return result;
        }

        /// <summary>
        /// Sorts the components of the specified expression.
        /// </summary>
        /// <param name="exp">Specified expression.</param>
        /// <returns>A sorted expression</returns>
        private string sortExpression(string exp)
        {
            return exp == null ? null : String.Join("+", exp.Split('+').OrderBy(x => x.Split(' ').Length));
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
