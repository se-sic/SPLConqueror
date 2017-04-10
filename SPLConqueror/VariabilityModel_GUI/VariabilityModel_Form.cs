using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPLConqueror_Core;
using System.IO;

namespace VariabilitModel_GUI
{
    public partial class VariabilityModel_Form : Form
    {
        private const string TITLE = "Model Creator";
        private const string CREATING_MODEL_TITLE = "Creating new model";
        private const string CREATING_MODEL_DESCRIPTION = "Enter the name of the new model.";
        private const string DATA_NOT_SAVED = "There is still unsaved content. Do you want to save it?";
        private const string REMOVE_WARNING = "Are you sure about removing this feature?\n"
            + "All children features will be deleted as well.";

        private TreeNode currentNode = null;
        private string currentFilePath = "";
        private bool dataSaved = true;

        public VariabilityModel_Form()
        {
            InitializeComponent();

            this.Text = TITLE;
            this.currentNode = null;
            this.saveModelToolStripMenuItem.Enabled = false;
            this.saveModelAsToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Enabled = false;
            this.addAlternativeGroupToolStripMenuItem.Enabled = false;
        }

        protected void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.currentNode = e.Node;
                removeFeatureToolStripMenuItem.Enabled = e.Node != null && e.Node.Text != "root";

                contextMenuStrip.Show(treeView, e.Location);
            }
        }

        public void InitTreeView()
        {
            this.treeView.Nodes.Clear();
            this.treeView.CheckBoxes = true;

            TreeNode root = new TreeNode("root");
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();
            for (int i = 0; i < options.Count; i++)
            {
                if ((options[i].Parent == GlobalState.varModel.Root || options[i].Parent == null) && (!options[i].Name.Equals("root")))//first level element
                {
                    TreeNode tn = new TreeNode(options[i].Name);
                    root.Checked = true;/*root is always checked*/
                    insertSubElements(options[i], tn, true /*root is always checked*/);
                    if (options[i] is SPLConqueror_Core.NumericOption)
                        tn.ForeColor = Color.Red;
                    root.Nodes.Add(tn);
                }
            }
            this.treeView.Nodes.Add(root);
            this.treeView.ExpandAll();
        }

        protected void insertSubElements(ConfigurationOption element, TreeNode t, bool bParentChecked)
        {
            bool bChecked = false;

            t.Tag = element;
            if (element is SPLConqueror_Core.NumericOption)
                t.ForeColor = Color.Red;

            //rekursiv die unterelemente einfügen
            element.updateChildren();
            foreach (ConfigurationOption elem in element.Children)
            {
                TreeNode tn = new TreeNode(elem.Name);
                insertSubElements(elem, tn, bChecked);

                t.Nodes.Add(tn);
            }
        }

        /// <summary>
        /// Invokes if the 'File -> New model'-option in the menu strip was clicked.
        /// 
        /// This will open a dialog to enter the name of the new model. If the
        /// name entering was successful, the old model will be removed and a
        /// new one will be created.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void newModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataSaved && handleUnsavedData() == DialogResult.Cancel)
                return;

            Tuple<DialogResult, string> result = CreationDialog();

            if (result.Item1 == DialogResult.Cancel)
                return;

            GlobalState.varModel = new VariabilityModel(result.Item2);
            this.saveModelToolStripMenuItem.Enabled = true;
            this.saveModelAsToolStripMenuItem.Enabled = true;
            this.editToolStripMenuItem.Enabled = true;
            this.addAlternativeGroupToolStripMenuItem.Enabled = true;
            this.Text = TITLE + ": " + result.Item2;

            currentFilePath = "";
            dataSaved = false;

            InitTreeView();
        }

        /// <summary>
        /// Invokes if the 'File -> Save model'-option in the menu strip was clicked.
        /// 
        /// This will save the curent model in the saved file path. If the saved path is empty,
        /// a dialog will open to get the new saving path.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void saveModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFilePath.Length > 0)
            {
                GlobalState.varModel.Path = currentFilePath;
                GlobalState.varModel.saveXML();

                dataSaved = true;
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    String folder = fbd.SelectedPath;
                    GlobalState.varModel.Path = folder + Path.DirectorySeparatorChar + GlobalState.varModel.Name + ".xml";
                    GlobalState.varModel.saveXML();

                    currentFilePath = GlobalState.varModel.Path;
                    dataSaved = true;
                }
            }
        }

        /// <summary>
        /// Invokes if the 'File -> Save model as'-option in the menu strip was clicked.
        /// 
        /// This will open a dialog to determine where to save the current model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void saveModelAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                String path = fbd.FileName;
                String modelName = Path.GetFileNameWithoutExtension(path);

                if (!path.EndsWith(".xml"))
                    path += ".xml";

                if (modelName.EndsWith(".xml"))
                    modelName = modelName.Remove(modelName.Length - 4);

                GlobalState.varModel.Name = modelName;
                GlobalState.varModel.Path = path;
                GlobalState.varModel.saveXML();

                dataSaved = true;
            }
        }

        /// <summary>
        /// Invokes if the 'File -> Load model'-option in the menu strip was clicked.
        /// 
        /// This will open a dialog to determine which model should be loaded.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataSaved && handleUnsavedData() == DialogResult.Cancel)
                return;

            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new FileInfo(pfd.FileName);
                GlobalState.varModel = VariabilityModel.loadFromXML(fi.FullName);
                this.saveModelToolStripMenuItem.Enabled = true;
                this.saveModelAsToolStripMenuItem.Enabled = true;
                this.editToolStripMenuItem.Enabled = true;
                this.addAlternativeGroupToolStripMenuItem.Enabled = true;

                currentFilePath = fi.FullName;
                dataSaved = true;

                InitTreeView();
            }
        }

        /// <summary>
        /// Invokes if the 'File -> Exit'-option in the menu strip was clicked.
        /// 
        /// This will simply close the form.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataSaved && handleUnsavedData() == DialogResult.Cancel)
                return;

            this.Dispose();
        }

        /// <summary>
        /// Invokes if the current form is about to be closed.
        /// 
        /// Additionally, it will check if there is any unsaved data. If that is the case,
        /// a dialog will be opened to dtermine if the data should be saved.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (!dataSaved)
            {
                DialogResult result = MessageBox.Show(DATA_NOT_SAVED, "", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    saveModelToolStripMenuItem_Click(null, null);
            }
        }

        /// <summary>
        /// Invokes if the 'Edit -> Edit Options'-option in the menu strip was clicked.
        /// 
        /// This will open the corresponding dialog for editing options.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void editOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditOptionDialog form = new EditOptionDialog(this, null);
            form.Show();

            dataSaved = false;
        }

        /// <summary>
        /// Invokes if the 'Edit -> Edit Contraints'-option in the menu strip was clicked.
        /// 
        /// This will open the corresponding dialog for editing the contraints of the current
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void editConstraintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditContraintsDialog form = new EditContraintsDialog();
            form.Show();

            dataSaved = false;
        }

        /// <summary>
        /// Invokes if the 'Edit -> Edit Alternative Groups'-option in the menu strip was
        /// clicked.
        /// 
        /// This will open the corresponding dialog for editing the alternative groups of the
        /// current variability modl.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void editAlternativeGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlternativeGroupDialog form = new AlternativeGroupDialog(null);
            form.Show();

            dataSaved = false;
        }

        /// <summary>
        /// Invokes if the '?'-option in the menu strip was clicked.
        /// 
        /// This will open a dialog with some information about this application.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// Invokes if the 'Add Feature'-option in the context menu strip was clicked.
        /// 
        /// This will open a dialog to create a new feature at the position where the
        /// context menu strip was placed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void addFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String featureName = this.currentNode == null ? null : this.currentNode.Text;

            NewFeatureDialog dlg = new NewFeatureDialog(featureName);
            dlg.ShowDialog();

            dataSaved = false;

            InitTreeView();
        }

        /// <summary>
        /// Invokes if the 'Edit Feature'-option in the context menu strip was clicked.
        /// 
        /// This will open a dialog to edit the feature at the position where the context
        /// menu strip was placed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void editFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationOption selected;

            if (this.currentNode == null)
                selected = null;
            else if (this.currentNode.Text == "root")
                selected = GlobalState.varModel.Root;
            else
                selected = GlobalState.varModel.getOption(this.currentNode.Text);

            EditOptionDialog dlg = new EditOptionDialog(this, selected);
            dlg.ShowDialog();

            dataSaved = false;

            InitTreeView();
        }

        /// <summary>
        /// Invokes if the 'Remove Feature'-option in the context menu strip was clicked.
        /// 
        /// This will open a dialog to get assured if the user really wants to remove the
        /// feature at the position where the context menu strip was placed. All children
        /// of this feature will be deleted too.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void removeFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(REMOVE_WARNING, "", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ConfigurationOption selected = GlobalState.varModel.getOption(this.currentNode.Text);

                GlobalState.varModel.deleteOption(selected);

                dataSaved = false;

                InitTreeView();
            }
        }

        /// <summary>
        /// invokes if the 'Add Alternative Group'-option in the context menu strip was clicked.
        /// 
        /// This will open a dialog to edit the alternative groupd of the current
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void addAlternativeGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationOption selected = GlobalState.varModel.getOption(this.currentNode.Text);

            AlternativeGroupDialog form = new AlternativeGroupDialog(selected);
            form.Show();

            dataSaved = false;
        }

        /// <summary>
        /// Opens a dialog to ask the user if he/she wants to save the unsaved data.
        /// 
        /// This method will also return the decision of the user.
        /// </summary>
        /// <returns>Result of the dialog</returns>
        private DialogResult handleUnsavedData()
        {
            DialogResult result = MessageBox.Show(DATA_NOT_SAVED, "", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                saveModelToolStripMenuItem_Click(null, null);
                dataSaved = true;
            }

            return result;
        }

        /// <summary>
        /// Opens a dialog to enter the name of the new model.
        /// 
        /// The DialogResult in the returned tuple can only be 'OK' if the entered text
        /// contains at least one character. If the text is empty, the OK button cannot
        /// be pressed.
        /// </summary>
        /// <returns>Result of the dialog and the entered text</returns>
        private Tuple<DialogResult, string> CreationDialog()
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = CREATING_MODEL_TITLE;
            label.Text = CREATING_MODEL_DESCRIPTION;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonOk.Enabled = false;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            textBox.TextChanged += (s, e) =>
                {
                    buttonOk.Enabled = textBox.Text.Length > 0;
                };

            DialogResult dialogResult = form.ShowDialog();
            return new Tuple<DialogResult, string>(dialogResult, textBox.Text);
        }

        private void convertNumericOptionsToBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.Title = "Save all binary variability Model";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                String path = fbd.FileName;
                System.Threading.Thread transformThread = new System.Threading.Thread(() => saveTransformedVM(path));
                transformThread.Start();
            }
        }

        private static void saveTransformedVM(string path)
        {
            VariabilityModel transformedModel = transformVarModel();
            String modelName = Path.GetFileNameWithoutExtension(path);
            if (!path.EndsWith(".xml"))
            {
                path += ".xml";
            }

            if (modelName.EndsWith(".xml"))
            {
                modelName = modelName.Remove(modelName.Length - 4);
            }

            transformedModel.Name = modelName;
            transformedModel.Path = path;
            transformedModel.saveXML();
            MessageBox.Show("Model successfully converted");
        }

        private void exportToDimacsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region parse to allbinary model
        private static VariabilityModel transformVarModel()
        {
            VariabilityModel transformedVarModel = new VariabilityModel(GlobalState.varModel.Name);

            GlobalState.varModel.BinaryOptions.ForEach(x => transformedVarModel.addConfigurationOption(x));
            GlobalState.varModel.BooleanConstraints.ForEach(constraint =>
                convertToCNF(constraint).ForEach(convertedConstraint => transformedVarModel.BooleanConstraints.Add(convertedConstraint)));

            foreach (NumericOption currNumOpt in GlobalState.varModel.NumericOptions)
            {
                // Create Binary Options for each numeric Option( #Steps)
                List<ConfigurationOption> allChildren = new List<ConfigurationOption>();
                foreach (double step in currNumOpt.getAllValues())
                {
                    BinaryOption toAdd = new BinaryOption(GlobalState.varModel, currNumOpt.Name + "_" + step);
                    toAdd.Optional = false;
                    toAdd.OutputString = currNumOpt.Prefix + step + currNumOpt.Postfix;
                    allChildren.Add(toAdd);
                    transformedVarModel.addConfigurationOption(toAdd);
                }

                // Add a exclude statement so that it isnt possible to select 2 values for a numeric option at the same time
                foreach (ConfigurationOption currentOption in allChildren)
                {
                    List<List<ConfigurationOption>> excluded = new List<List<ConfigurationOption>>();
                    List<ConfigurationOption> currentOptionWrapper = new List<ConfigurationOption>();
                    currentOptionWrapper.Add(currentOption);
                    allChildren.Except(currentOptionWrapper).ToList().ForEach(x => excluded.Add(new ConfigurationOption[] { x }.ToList()));
                    currentOption.Excluded_Options = excluded;
                }
            }
            transformNumericConstraintsToBoolean(transformedVarModel);
            return transformedVarModel;

        }

        private static void transformNumericConstraintsToBoolean(VariabilityModel newVariabilityModel)
        {
            foreach (NonBooleanConstraint constraintToTransform in GlobalState.varModel.NonBooleanConstraints)
            {
                String constraintAsExpression = constraintToTransform.ToString();

                string[] parts = constraintAsExpression.Split(new String[] { "=", "<", ">", "<=", ">=" }, StringSplitOptions.None);
                InfluenceFunction leftHandSide = new InfluenceFunction(parts[0], GlobalState.varModel);
                InfluenceFunction rightHandSide = new InfluenceFunction(parts[parts.Length - 1], GlobalState.varModel);

                // Find all possible assignments for the participating numeric options to turn it into a CNF clause
                // TODO: Maybe use a solver 
                List<NumericOption> allParticipatingNumericOptions = leftHandSide.participatingNumOptions.ToList()
                                                            .Union(rightHandSide.participatingNumOptions.ToList()).Distinct().ToList();

                List<List<double>> allPossibleValues = new List<List<double>>();
                allParticipatingNumericOptions.ForEach(x => allPossibleValues.Add(x.getAllValues()));

                var cartesianProduct = allPossibleValues.First().Select(x => x.ToString());
                foreach(List<double> possibleValue in allPossibleValues.Skip(1))
                {
                    cartesianProduct = from product in cartesianProduct
                                       from newValue in possibleValue
                                       select product + "$" + newValue;
                }

                // Find all invalid Configurations, turn each invalid Configuration into a or clause and combine them to a CNF 
                IEnumerable<string> invalidConfigs = cartesianProduct
                    .Where(x => !testIfValidConfiguration(x, allParticipatingNumericOptions, constraintToTransform));
                StringBuilder nonBooleanConstraintAsBoolean = new StringBuilder();
                foreach (string validConfig in invalidConfigs)
                {
                    List<string> binaryRepresentation = validConfig.Split(new char[] { '$' }).Zip(allParticipatingNumericOptions, (x, y) => "!" + y.Name + "_" + x)
                        .ToList();
                    nonBooleanConstraintAsBoolean.Append(binaryRepresentation.First());
                    for (int i = 1; i < binaryRepresentation.Count; i++)
                    {
                        nonBooleanConstraintAsBoolean.Append(" | " + binaryRepresentation.ElementAt(i));
                    }
                    nonBooleanConstraintAsBoolean.Append(" & ");
                }

                if (nonBooleanConstraintAsBoolean.Length > 0)
                {
                    // Remove trailing ' & '
                    nonBooleanConstraintAsBoolean.Length = nonBooleanConstraintAsBoolean.Length - 3;
                    convertToCNF(nonBooleanConstraintAsBoolean.ToString()).ForEach(clause => newVariabilityModel.BooleanConstraints.Add(clause.Trim()));
                }
            }

        }

        private static bool testIfValidConfiguration(string configuration, List<NumericOption> participatingOptions, NonBooleanConstraint constraintToTest)
        {
            Dictionary<NumericOption, double> numericSelection = new Dictionary<NumericOption, double>();
            string[] values = configuration.Split(new char[] { '$' });
            for (int i = 0; i < participatingOptions.Count; i++)
            {
                numericSelection.Add(participatingOptions.ElementAt(i), Double.Parse(values[i]));
            }
            return constraintToTest.configIsValid(numericSelection);
        }

        private static List<string> convertToCNF(string nonCNFFormula)
        {
            // Assumption no '(',')', etc are used since these arent supported by SPLConqueror either
            // and functions with both & and | are already in CNF
            if (!nonCNFFormula.Contains("|"))
            {
                return nonCNFFormula.Split(new char[] { '&' }).Select(variable => variable.Trim()).ToList();
            }
            if (!nonCNFFormula.Contains("&"))
            {
                List<string> oneClause = new List<string>();
                oneClause.Add(nonCNFFormula);
                return oneClause;
            }
            List<string> inCNF = nonCNFFormula.Split(new char[] { '&' }).ToList();
            return inCNF;

        }
        #endregion parse to allbinary model

    }
}
