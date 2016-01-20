using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SPLConqueror_Core;
using System.IO;

namespace SPLConqueror_WebSite
{
    public partial class ModelEditor : System.Web.UI.Page
    {
        private const string ERROR_MODEL_NO_FILE = "There is no file to load!";
        private const string ERROR_MODEL_WRONG_FORMAT = "The loaded file is not a .xml-file";
        private const string ERROR_MODEL_PROBLEM_LOADING = "The loaded file contains errors.";
        private const string ERROR_CRE_NAME_EMPTY = "Please enter a name for the new feature.";
        private const string ERROR_CRE_NAME_INVALID = "The entered feature name contains invalid symbols.";
        private const string ERROR_CRE_NAME_EXISTS = "The entered feature name already exists.";
        private const string ERROR_EDIT_RENAME_INVALID = "The entered name for renaming contains invalid symbols.";
        private const string ERROR_EDIT_RENAME_EXISTS = "The entered name for renaming already exists.";
        private const string ERROR_INVALID_RANGE = "The entered range is invalid.";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (GlobalState.varModel == null)
                    initializeComponents();
                else
                {
                    updateModelView();
                    updateCreationView();
                    updateEditorView();
                    updateConstraintView();
                }
            }
        }

        /// <summary>
        /// Initializes all components of this site.
        /// </summary>
        protected void initializeComponents()
        {
            initializeModelViewer();
            initializeOptionCreation();
            initializeOptionEditor();
            initializeConstraintEditor();
        }

        /// <summary>
        /// Initializes the model viewer.
        /// 
        /// If there is already a model loaded, it will be displayed. If there
        /// is none, a new model will be created automaticly.
        /// </summary>
        private void initializeModelViewer()
        {
            if (GlobalState.varModel == null)
                GlobalState.varModel = new VariabilityModel("Model");

            updateModelView();
        }

        /// <summary>
        /// Initializes the option creator.
        /// 
        /// If there is already a model loaded, all information of the model will be
        /// considered.
        /// </summary>
        private void initializeOptionCreation()
        {
            if (GlobalState.varModel != null)
            {
                creFeatureNameTextBox.Enabled = true;
                creFeatureNameTextBox.Text = "";

                foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                    creParentFeatureDDList.Items.Add(opt.Name);

                sortDDList(creParentFeatureDDList);
                creParentFeatureDDList.Enabled = true;
                creParentFeatureDDList.SelectedIndex = 0;

                creBinaryRadioButton.Enabled = true;
                creBinaryRadioButton.Checked = true;
                creBinaryRadioButton_CheckedChanged(null, null);
                creNumericRadioButton.Enabled = true;
                crePrePostCheckBox.Enabled = true;
                creVarGenParameterTextBox.Enabled = true;

                creMinTextBox.Text = "";
                creMaxTextBox.Text = "";
                creStepSizeTextBox.Text = "";
                crePreTextBox.Text = "";
                crePostTextBox.Text = "";
                creVarGenParameterTextBox.Text = "";
            }
        }

        /// <summary>
        /// Initializes the editor for options.
        /// </summary>
        private void initializeOptionEditor()
        {
            if (GlobalState.varModel != null)
            {
                editFeatureNameDDList.Items.Clear();

                foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                    editFeatureNameDDList.Items.Add(opt.Name);
                
                sortDDList(editFeatureNameDDList);
                editFeatureNameDDList.Enabled = true;
                editFeatureNameDDList.SelectedIndex = 0;
                editFeatureNameDDList_SelectedIndexChanged(null, null);
                
                editRenameTextBox.Text = "";
                editMinTextBox.Text = "";
                editMaxTextBox.Text = "";
                editStepSizeTextBox.Text = "";
                editPreTextBox.Text = "";
                editPostTextBox.Text = "";
                editVarGenParameterTextBox.Text = "";
            }
        }

        /// <summary>
        /// Initializes the editor for constraints.
        /// 
        /// This method will initialize boolean and the non-booean constraints.
        /// </summary>
        private void initializeConstraintEditor()
        {
            updateConstraintView();

            boolConstraintList.Clear();
            boolConstraintTextBox.Text = "";
            updateBoolConstraintBox();

            nbConstraintList.Clear();
            nbConstraintTextBox.Text = "";
            updateNbConstraintBox();
        }

        /// <summary>
        /// Sorts the elements of the specified DropDownList.
        /// </summary>
        /// <param name="ddl">List to sort</param>
        private void sortDDList(DropDownList ddl)
        {
            List<ListItem> listCopy = new List<ListItem>();

            foreach (ListItem item in ddl.Items)
                listCopy.Add(item);

            ddl.Items.Clear();

            foreach (ListItem item in listCopy.OrderBy(item => item.Text))
                ddl.Items.Add(item);
        }

        /*
         * Model viewer.
         */

        /// <summary>
        /// Invokes if the button for a new model was pressed.
        /// 
        /// This method will create a new model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void newModelButton_Click(object sender, EventArgs e)
        {
            GlobalState.varModel = new VariabilityModel("Model");

            initializeOptionCreation();
            initializeOptionEditor();
            initializeConstraintEditor();

            updateModelView();
        }

        /// <summary>
        /// Invokes if a node in the tree view was selected.
        /// 
        /// This method will remove the selected node from the current model. All
        /// children of this node will be deleted too.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void modelTreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            string selected = modelTreeView.SelectedNode.Text;

            if (selected != GlobalState.varModel.Root.Name)
            {
                GlobalState.varModel.deleteOption(GlobalState.varModel.getOption(selected));
                updateModelView();
            }
        }

        /// <summary>
        /// Invokes if the button for loading a modl was pressed.
        /// 
        /// This method will try to load the uploaded model. If there are any
        /// problems while loading, an appropriate error message will be displayed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void loadModelButton_Click(object sender, EventArgs e)
        {
            if (!modelFileUpload.HasFile)
            {
                modelErrorLabel.Text = ERROR_MODEL_NO_FILE;
                modelErrorLabel.Visible = true;
                return;
            }

            string filename = Path.GetFileName(modelFileUpload.FileName);
            modelFileUpload.SaveAs(Server.MapPath("~/") + filename);

            if (!filename.EndsWith(".xml"))
            {
                modelErrorLabel.Text = ERROR_MODEL_WRONG_FORMAT;
                modelErrorLabel.Visible = true;
                return;
            }

            try
            {
                GlobalState.varModel = VariabilityModel.loadFromXML(filename);
            } catch
            {
                modelErrorLabel.Text = ERROR_MODEL_PROBLEM_LOADING;
                modelErrorLabel.Visible = true;
                return;
            }

            modelErrorLabel.Visible = false;

            updateModelView();
        }

        /// <summary>
        /// Updates the model view.
        /// </summary>
        private void updateModelView()
        {
            modelTreeView.Nodes.Clear();

            TreeNode root = new TreeNode("root");
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();
            for (int i = 0; i < options.Count; i++)
            {
                if ((options[i].Parent == GlobalState.varModel.Root || options[i].Parent == null)
                    && (!options[i].Name.Equals("root")))//first level element
                {
                    TreeNode tn = new TreeNode(options[i].Name);
                    root.Checked = true;
                    insertSubElements(options[i], tn);
                    // TODO: Other color possible?
                    //if (options[i] is SPLConqueror_Core.NumericOption)
                    //    tn.ForeColor = Color.Red;
                    root.ChildNodes.Add(tn);
                }
            }
            modelTreeView.Nodes.Add(root);
            modelTreeView.ExpandAll();
        }

        /// <summary>
        /// Inserts all children of the specified node into the tree view.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="t"></param>
        /// <param name="bParentChecked"></param>
        private void insertSubElements(ConfigurationOption element, TreeNode t)
        {
            t.Text = element.Name;
            //if (element is SPLConqueror_Core.NumericOption)
            //    t.ForeColor = Color.Red;

            //rekursiv die unterelemente einfügen
            element.updateChildren();
            foreach (ConfigurationOption elem in element.Children)
            {
                TreeNode tn = new TreeNode(elem.Name);
                insertSubElements(elem, tn);

                t.ChildNodes.Add(tn);
            }
        }

        /*
         * ------------------------------------------------------------
         * Creating options.
         * ------------------------------------------------------------
         */

        /// <summary>
        /// Invokes if the radio button for a binary option was selected.
        /// 
        /// This will enable all possible options for binary features and disable all
        /// options for numeric features.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void creBinaryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (creBinaryRadioButton.Checked)
            {
                creNumericRadioButton.Checked = false;

                creOptionalCheckBox.Enabled = true;
                creOptionalLabel.Enabled = true;
                creRangeLabel.Enabled = false;
                creMinLabel.Enabled = false;
                creMinTextBox.Enabled = false;
                creMaxLabel.Enabled = false;
                creMaxTextBox.Enabled = false;
                creStepSizeCheckBox.Enabled = false;
                creStepSizeLabel.Enabled = false;
                creStepSizeTextBox.Enabled = false;
                creStepExampleLabel.Enabled = false;
            }
        }

        /// <summary>
        /// Invokes if the radio button for a numeric option was selected.
        /// 
        /// This will enable all possible options for numeric features and disable all
        /// options for binary features.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void creNumericRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (creNumericRadioButton.Checked)
            {
                creBinaryRadioButton.Checked = false;

                creOptionalCheckBox.Enabled = false;
                creOptionalLabel.Enabled = false;
                creRangeLabel.Enabled = true;
                creMinLabel.Enabled = true;
                creMinTextBox.Enabled = true;
                creMaxLabel.Enabled = true;
                creMaxTextBox.Enabled = true;
                creStepSizeCheckBox.Enabled = true;
                creStepSizeLabel.Enabled = creStepSizeCheckBox.Checked;
                creStepSizeTextBox.Enabled = creStepSizeCheckBox.Checked;
                creStepExampleLabel.Enabled = creStepSizeCheckBox.Checked;
            }
        }

        /// <summary>
        /// Invokes if the check state of the check box for step functions has changed.
        /// 
        /// This method will enable or disable the possible options for editing the
        /// step function depending on the selected check state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void creStepSizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            creStepSizeLabel.Enabled = creStepSizeCheckBox.Checked;
            creStepSizeTextBox.Enabled = creStepSizeCheckBox.Checked;
            creStepExampleLabel.Enabled = creStepSizeCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the check state of the check box for prefix and postfix has
        /// changed.
        /// 
        /// This method will enable or disable the possible options for editing the
        /// prefix and postfix depending on the selected check state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void crePrePostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            crePreLabel.Enabled = crePrePostCheckBox.Checked;
            crePreTextBox.Enabled = crePrePostCheckBox.Checked;
            crePostLabel.Enabled = crePrePostCheckBox.Checked;
            crePostTextBox.Enabled = crePrePostCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the button for adding the new option was pressed.
        /// 
        /// This method will add a new option to the current model. This option has
        /// all properties that were previously selected.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void creAddOptionButton_Click(object sender, EventArgs e)
        {
            if (creFeatureNameTextBox.Text == "")
            {
                creErrorLabel.Text = ERROR_CRE_NAME_EMPTY;
                creErrorLabel.Visible = true;
                return;
            }

            if (GlobalState.varModel.getOption(creFeatureNameTextBox.Text) != null)
            {
                creErrorLabel.Text = ERROR_CRE_NAME_EXISTS;
                creErrorLabel.Visible = true;
                return;
            }

            if (creFeatureNameTextBox.Text != ConfigurationOption.removeInvalidCharsFromName(creFeatureNameTextBox.Text))
            {
                creErrorLabel.Text = ERROR_CRE_NAME_INVALID;
                creErrorLabel.Visible = true;
                return;
            }

            if (currentOption is NumericOption && !isRangeValid(editMinTextBox.Text, editMaxTextBox.Text))
            {
                creErrorLabel.Visible = true;
                creErrorLabel.Text = ERROR_INVALID_RANGE;
                return;
            }

            creErrorLabel.Visible = false;

            ConfigurationOption newOption = null;

            if (creNumericRadioButton.Checked)
            {
                newOption = new NumericOption(GlobalState.varModel, creFeatureNameTextBox.Text);
                ((NumericOption)newOption).Min_value = Convert.ToDouble(creMinTextBox.Text);
                ((NumericOption)newOption).Max_value = Convert.ToDouble(creMaxTextBox.Text);

                if (creStepSizeCheckBox.Checked)
                    ((NumericOption)newOption).StepFunction = new InfluenceFunction(
                        creStepSizeTextBox.Text == "" ? "n + 1" : creStepSizeTextBox.Text, (NumericOption)newOption);
                else
                    ((NumericOption)newOption).StepFunction = new InfluenceFunction("n + 1", (NumericOption)newOption);
            }
            else
            {
                newOption = new BinaryOption(GlobalState.varModel, creFeatureNameTextBox.Text);
                ((BinaryOption)newOption).Optional = creOptionalCheckBox.Checked;
            }

            if (crePrePostCheckBox.Checked)
            {
                newOption.Prefix = crePreTextBox.Text;
                newOption.Postfix = crePostTextBox.Text;
            }

            newOption.Parent = GlobalState.varModel.getOption(creParentFeatureDDList.Text);
            newOption.Parent.Children.Add(newOption);
            
            GlobalState.varModel.addConfigurationOption(newOption);
            updateModelView();
            updateEditorView();
            updateConstraintView();

            initializeOptionCreation();
        }

        /// <summary>
        /// Updates the view of creating a new option.
        /// </summary>
        private void updateCreationView()
        {
            creParentFeatureDDList.Items.Clear();

            foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                creParentFeatureDDList.Items.Add(opt.Name);

            sortDDList(creParentFeatureDDList);
            creParentFeatureDDList.SelectedIndex = 0;
        }

        /*
         * ------------------------------------------------------------
         * Editing options.
         * ------------------------------------------------------------
         */

        private ConfigurationOption currentOption;
        private List<List<ConfigurationOption>> excludes = new List<List<ConfigurationOption>>();
        private List<List<ConfigurationOption>> implied = new List<List<ConfigurationOption>>();

        /// <summary>
        /// Invokes if the another feature was selected.
        /// 
        /// Thsi method will change all components to show the current properties
        /// of the new selected feature.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editFeatureNameDDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentOption = GlobalState.varModel.getOption(editFeatureNameDDList.Text);

            editRenameTextBox.Enabled = currentOption != GlobalState.varModel.Root;
            editRenameTextBox.Text = "";
            editParentDDList.Enabled = currentOption != GlobalState.varModel.Root;
            editParentDDList.Items.Clear();

            foreach (ConfigurationOption opt in getPossibleParents())
                editParentDDList.Items.Add(opt.Name);

            if (editParentDDList.Items.Count > 0)
            {
                sortDDList(editParentDDList);
                editParentDDList.SelectedIndex = 0;
            }

            bool isBinary = currentOption is BinaryOption;
            bool isNumeric = currentOption is NumericOption;

            editOptionalCheckBox.Visible = isBinary;
            editOptionalCheckBox.Checked = isBinary ? ((BinaryOption)currentOption).Optional : false;
            editOptionalLabel.Visible = isBinary;

            editNumericErrorLabel.Visible = isBinary;
            editRangeLabel.Enabled = isNumeric;
            editMinLabel.Enabled = isNumeric;
            editMinTextBox.Text = isNumeric ? ((NumericOption)currentOption).Min_value.ToString() : "";
            editMaxLabel.Enabled = isNumeric;
            editMaxTextBox.Text = isNumeric ? ((NumericOption)currentOption).Max_value.ToString() : "";
            editStepSizeLabel.Enabled = isNumeric;
            editStepSizeTextBox.Enabled = isNumeric;
            editStepSizeTextBox.Text = isNumeric ? ((NumericOption)currentOption).StepFunction.ToString() : "";
            editStepExampleLabel.Enabled = isNumeric;

            editPreTextBox.Enabled = true;
            editPreTextBox.Text = currentOption.Prefix;
            editPostTextBox.Enabled = true;
            editPostTextBox.Text = currentOption.Postfix;
            editVarGenParameterTextBox.Enabled = true;

            excludes = new List<List<ConfigurationOption>>();
            implied = new List<List<ConfigurationOption>>();

            foreach (List<ConfigurationOption> list in currentOption.Excluded_Options)
                excludes.Add(list.ToList());

            foreach (List<ConfigurationOption> list in currentOption.Implied_Options)
                implied.Add(list.ToList());
            
            editExcludeCheckBoxList.Items.Clear();
            editImplyCheckBoxList.Items.Clear();

            foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
            {
                if (opt != currentOption)
                {
                    editExcludeCheckBoxList.Items.Add(opt.Name);
                    editImplyCheckBoxList.Items.Add(opt.Name);
                }
            }

            editExcludeListBox.Items.Clear();
            editImplyListBox.Items.Clear();

            foreach (List<ConfigurationOption> excludeList in currentOption.Excluded_Options)
                editExcludeListBox.Items.Add(String.Join(" | ", excludeList));

            foreach (List<ConfigurationOption> requireList in currentOption.Implied_Options)
                editImplyListBox.Items.Add(String.Join(" | ", requireList));

            editExcludeDeleteButton.Enabled = editExcludeListBox.Items.Count > 0;
            editImplyDeleteButton.Enabled = editImplyListBox.Items.Count > 0;
            editExcludeAddButton.Enabled = false;
            editImplyAddButton.Enabled = false;
        }

        /// <summary>
        /// Calculates and returns a list of features that can be selected as new parent
        /// of the currently selected feature.
        /// </summary>
        /// <returns>List of possible new parents</returns>
        private List<ConfigurationOption> getPossibleParents()
        {
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();
            List<ConfigurationOption> optionsToRemove = new List<ConfigurationOption>();

            optionsToRemove.Add(currentOption);

            while (optionsToRemove.Count > 0)
            {
                ConfigurationOption opt = optionsToRemove[0];

                options.Remove(opt);

                foreach (ConfigurationOption child in opt.Children)
                    optionsToRemove.Add(child);

                optionsToRemove.Remove(opt);
            }

            return options;
        }

        /// <summary>
        /// Checks if the specified range is valid.
        /// </summary>
        /// <param name="minString">String for minimum.</param>
        /// <param name="maxString">String for maximum.</param>
        /// <returns>True, if the range is valid, else false</returns>
        private bool isRangeValid(string minString, string maxString)
        {
            bool stillOk = true;

            // Check if min value is a number
            foreach (char c in minString)
                stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

            stillOk &= minString.Replace("-", "").Replace(".", "").Length > 0;

            // Check if max value is a number
            foreach (char c in maxString)
                stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

            stillOk &= maxString.Replace("-", "").Replace(".", "").Length > 0;

            if (!stillOk)
                return stillOk;

            // Check if range is valid
            double min, max;
            
            Double.TryParse(minString, out min);
            Double.TryParse(maxString, out max);

            stillOk &= min <= max;

            return stillOk;
        }

        /// <summary>
        /// Invokes if the selection of the excluded check box list has changed.
        /// 
        /// This method will enable or disable the button for adding the combination
        /// to the excluded list if at least one feature is selected.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editExcludeCheckBoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            editExcludeAddButton.Enabled = editExcludeCheckBoxList.SelectedIndex > -1;
        }

        /// <summary>
        /// Invokes if the button for adding the excluded combination was pressed.
        /// 
        /// This method will add the currently selected combination to the excluded list.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editExcludeAddButton_Click(object sender, EventArgs e)
        {
            List<ConfigurationOption> excludeCombination = new List<ConfigurationOption>();

            foreach (ListItem item in editExcludeListBox.Items)
                if (item.Selected)
                    excludeCombination.Add(GlobalState.varModel.getOption(item.Text));

            if (excludeCombination.Count > 0)
            {
                excludes.Add(excludeCombination);
                editExcludeListBox.Items.Add(String.Join(" | ", excludeCombination));

                editExcludeDeleteButton.Enabled = true;
            }
        }

        /// <summary>
        /// Invokes if the button for deleting an excluded combination was pressed.
        /// 
        /// This method will delete the selected excluded combination.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editExcludeDeleteButton_Click(object sender, EventArgs e)
        {
            int selIndex = editExcludeListBox.SelectedIndex;

            if (selIndex > -1)
            {
                excludes.RemoveAt(selIndex);
                editExcludeListBox.Items.RemoveAt(selIndex);

                editExcludeDeleteButton.Enabled = editExcludeListBox.Items.Count > 0;
            }
        }

        /// <summary>
        /// Invokes if the selection of the implied check box list has changed.
        /// 
        /// This method will enable or disable the button for adding the combination
        /// to the implied list if at least one feature is selected.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editImplyCheckBoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            editImplyAddButton.Enabled = editImplyCheckBoxList.SelectedIndex > -1;
        }

        /// <summary>
        /// Invokes if the button for adding the implied combination was pressed.
        /// 
        /// This method will add the currently selected combination to the implied list.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editImplyAddButton_Click(object sender, EventArgs e)
        {
            List<ConfigurationOption> implyCombination = new List<ConfigurationOption>();

            foreach (ListItem item in editImplyListBox.Items)
                if (item.Selected)
                    implyCombination.Add(GlobalState.varModel.getOption(item.Text));

            if (implyCombination.Count > 0)
            {
                implied.Add(implyCombination);
                editImplyListBox.Items.Add(String.Join(" | ", implyCombination));

                editImplyDeleteButton.Enabled = true;
            }
        }

        /// <summary>
        /// Invokes if the button for deleting an implied combination was pressed.
        /// 
        /// This method will delete the selected implied combination.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editImplyDeleteButton_Click(object sender, EventArgs e)
        {
            int selIndex = editImplyListBox.SelectedIndex;

            if (selIndex > -1)
            {
                implied.RemoveAt(selIndex);
                editImplyListBox.Items.RemoveAt(selIndex);

                editImplyDeleteButton.Enabled = editImplyListBox.Items.Count > 0;
            }
        }

        /// <summary>
        /// Invokes if the button for confirming all changes of the current feature
        /// was pressed.
        /// 
        /// This method will adopt all changes done to the current feature. This is only
        /// done if there are no errors in the changes. If there are any errors, a message
        /// will clarify what the problem is.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void editConfirmButton_Click(object sender, EventArgs e)
        {
            if (editRenameTextBox.Text != "")
            {   
                if (GlobalState.varModel.getOption(editRenameTextBox.Text) != null)
                {
                    editErrorLabel.Text = ERROR_EDIT_RENAME_EXISTS;
                    editErrorLabel.Visible = true;
                    return;
                }
                
                if (editRenameTextBox.Text != ConfigurationOption.removeInvalidCharsFromName(editRenameTextBox.Text))
                {
                    editErrorLabel.Text = ERROR_EDIT_RENAME_INVALID;
                    editErrorLabel.Visible = true;
                    return;
                }

                if (currentOption is NumericOption && !isRangeValid(editMinTextBox.Text, editMaxTextBox.Text))
                {
                    editErrorLabel.Visible = true;
                    editErrorLabel.Text = ERROR_INVALID_RANGE;
                    return;
                }

                List<string> editedBooleanConstraints = new List<string>();
                List<NonBooleanConstraint> editedNonBooleanConstraints = new List<NonBooleanConstraint>();

                foreach (string boolConst in GlobalState.varModel.BooleanConstraints)
                {
                    string[] constParts = boolConst.Split(' ');

                    for (int i = 0; i < constParts.Length; i++)
                        constParts[i] = constParts[i] == currentOption.Name ? editRenameTextBox.Text : constParts[i];

                    editedBooleanConstraints.Add(String.Join(" ", constParts));
                }

                foreach (NonBooleanConstraint nbConst in GlobalState.varModel.NonBooleanConstraints)
                {
                    string[] constParts = nbConst.ToString().Split(' ');

                    for (int i = 0; i < constParts.Length; i++)
                        constParts[i] = constParts[i] == currentOption.Name ? editRenameTextBox.Text : constParts[i];

                    editedNonBooleanConstraints.Add(new NonBooleanConstraint(String.Join(" ", constParts), GlobalState.varModel));
                }

                GlobalState.varModel.BooleanConstraints = editedBooleanConstraints;
                GlobalState.varModel.NonBooleanConstraints = editedNonBooleanConstraints;
                currentOption.Name = editRenameTextBox.Text;
            }

            if (currentOption != GlobalState.varModel.Root)
            {
                currentOption.Parent.Children.Remove(currentOption);

                currentOption.Parent = GlobalState.varModel.getOption(editParentDDList.Text);
                currentOption.Parent.Children.Add(currentOption);
            }

            if (currentOption is BinaryOption)
                ((BinaryOption)currentOption).Optional = editOptionalCheckBox.Checked;
            else
            {
                ((NumericOption)currentOption).Min_value = Convert.ToDouble(editMinTextBox.Text);
                ((NumericOption)currentOption).Min_value = Convert.ToDouble(editMinTextBox.Text);
                ((NumericOption)currentOption).StepFunction = new InfluenceFunction(editStepSizeTextBox.Text);
            }

            currentOption.Prefix = editPreTextBox.Text;
            currentOption.Postfix = editPostTextBox.Text;
            currentOption.Excluded_Options = excludes;
            currentOption.Implied_Options = implied;

            editErrorLabel.Visible = false;

            updateModelView();
            updateCreationView();
            updateConstraintView();

            initializeOptionEditor();
        }

        /// <summary>
        /// Updates the view for editing options.
        /// </summary>
        private void updateEditorView()
        {
            editFeatureNameDDList.Items.Clear();

            foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                editFeatureNameDDList.Items.Add(opt.Name);

            sortDDList(editFeatureNameDDList);
            editFeatureNameDDList.SelectedIndex = 0;
            editFeatureNameDDList_SelectedIndexChanged(null, null);
        }

        /*
         * ------------------------------------------------------------
         * Constraints.
         * ------------------------------------------------------------
         */

        /// <summary>
        /// Updates the general view of the constraint editor.
        /// </summary>
        private void updateConstraintView()
        {
            boolOptionDDList.Items.Clear();
            nbOptionDDList.Items.Clear();

            if (GlobalState.varModel != null)
            {
                foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                {
                    boolOptionDDList.Items.Add(opt.Name);

                    if (opt is NumericOption)
                        nbOptionDDList.Items.Add(opt.Name);
                }
                
                sortDDList(boolOptionDDList);
                boolOptionDDList.Enabled = true;
                boolOptionDDList.SelectedIndex = 0;
                boolAddOptionButton.Enabled = true;

                if (nbOptionDDList.Items.Count > 0)
                {
                    sortDDList(nbOptionDDList);
                    nbOptionDDList.Enabled = true;
                    nbOptionDDList.SelectedIndex = 0;
                    nbAddOptionButton.Enabled = true;
                }

                boolConstraintListBox.Items.Clear();
                nbConstraintListBox.Items.Clear();

                foreach (string boolConst in GlobalState.varModel.BooleanConstraints)
                    boolConstraintListBox.Items.Add(boolConst);

                foreach (NonBooleanConstraint nbConst in GlobalState.varModel.NonBooleanConstraints)
                    nbConstraintListBox.Items.Add(nbConst.ToString());

                boolConstraintListBox.Enabled = true;
                boolDeleteButton.Enabled = boolConstraintListBox.Items.Count > 0;
                nbConstraintListBox.Enabled = true;
                nbDeleteButton.Enabled = nbConstraintListBox.Items.Count > 0;
            }
        }

        /*
         * ------------------------------------------------------------
         * Boolean constraints.
         * ------------------------------------------------------------
         */
        private List<string> boolConstraintList = new List<string>();

        private void setBoolConstraintList()
        {
            if (boolConstraintTextBox.Text.Length > 0)
                boolConstraintList = boolConstraintTextBox.Text.Split(' ').ToList();
        }

        /// <summary>
        /// Invokes if the button for adding an option to the current boolean
        /// constraint was pressed.
        /// 
        /// This method will add the currently selected option to the current
        /// boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolAddOptionButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += boolOptionDDList.SelectedItem.ToString();
            else
                boolConstraintList.Add(boolOptionDDList.SelectedItem.ToString());

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding an AND to the current boolean
        /// constraint was pressed.
        /// 
        /// This method will add a '&' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolAndButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += "&";
            else
                boolConstraintList.Add("&");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding an OR to the current boolean
        /// constraint was pressed.
        /// 
        /// This method will add a '|' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolOrButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += "|";
            else
                boolConstraintList.Add("|");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding an implication to the current boolean
        /// constraint was pressed.
        /// 
        /// This method will add a '=>' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolImplButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            boolConstraintList.Add("=>");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a negation to the current boolean
        /// constraint was pressed.
        /// 
        /// This method will add a '!' to the current boolean constraint. If the
        /// last addition to the boolean constraint was a negation, it will be
        /// nullified.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolNegButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList.RemoveAt(boolConstraintList.Count - 1);
            else
                boolConstraintList.Add("!");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding the current boolean constraint.
        /// 
        /// This method will add the current boolean constraint to the
        /// current variability model. After adding the constraint, it will be
        /// cleared for the next constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolAddConstraintButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            boolConstraintListBox.Items.Add(String.Join(" ", boolConstraintList));
            GlobalState.varModel.BooleanConstraints.Add(String.Join(" ", boolConstraintList));

            boolConstraintList.Clear();

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for removing the last addition to the
        /// boolean constraint was pressed.
        /// 
        /// This method will remove the last addition to the boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolRemoveButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            boolConstraintList.RemoveAt(boolConstraintList.Count - 1);

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for deleting a boolean constraint was pressed.
        /// 
        /// This method will delete the selected boolean constraint from the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void boolDeleteButton_Click(object sender, EventArgs e)
        {
            setBoolConstraintList();

            int selIndex = boolConstraintListBox.SelectedIndex;

            if (selIndex > -1)
            {
                GlobalState.varModel.BooleanConstraints.RemoveAt(selIndex);
                boolConstraintListBox.Items.RemoveAt(selIndex);

                updateBoolConstraintBox();
            }
        }

        /// <summary>
        /// Updates the view for creating boolean constraints.
        /// </summary>
        private void updateBoolConstraintBox()
        {
            boolConstraintTextBox.Enabled = true;
            boolConstraintTextBox.Text = String.Join(" ", boolConstraintList);

            // Choosing which buttons should be enabled. Depending on the current
            // amount of elements.
            int offset = boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!" ? 1 : 0;

            if ((boolConstraintList.Count - offset) % 2 == 1)
            {
                // Only operators can follow
                boolAndButton.Enabled = true;
                boolOrButton.Enabled = true;
                boolImplButton.Enabled = offset == 0;

                boolAddOptionButton.Enabled = false;
            }
            else
            {
                // Only options can follow
                boolAddOptionButton.Enabled = boolOptionDDList.Items.Count > 0;

                boolAndButton.Enabled = false;
                boolOrButton.Enabled = false;
                boolImplButton.Enabled = false;
            }

            boolAddConstraintButton.Enabled = boolConstraintList.Count >= 1
                && (boolConstraintList.Count - offset) % 2 == 1
                && boolConstraintList[boolConstraintList.Count - 1] != "!";
            boolRemoveButton.Enabled = boolConstraintTextBox.Text.Length > 0;
            boolDeleteButton.Enabled = boolConstraintListBox.Items.Count > 0;
        }

        /*
         * ------------------------------------------------------------
         * Non-boolean constraints.
         * ------------------------------------------------------------
         */
        private List<string> nbConstraintList = new List<string>();

        private void setNbConstraintList()
        {
            if (nbConstraintTextBox.Text.Length > 0)
                nbConstraintList = nbConstraintTextBox.Text.Split(' ').ToList();
        }

        /// <summary>
        /// Invokes if the button for adding an option to the current non-boolean
        /// constraint was pressed.
        /// 
        /// This method will add the currently selected option to the current
        /// non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbAddOptionButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            if (nbConstraintList.Count > 0 && nbConstraintList[nbConstraintList.Count - 1] == "-")
                nbConstraintList[nbConstraintList.Count - 1] += nbOptionDDList.SelectedItem.ToString();
            else
                nbConstraintList.Add(nbOptionDDList.SelectedItem.ToString());

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '0' was pressed.
        /// 
        /// This method will add a '0' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb0Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(0);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '1' was pressed.
        /// 
        /// This method will add a '1' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb1Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(1);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '2' was pressed.
        /// 
        /// This method will add a '2' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb2Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(2);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '3' was pressed.
        /// 
        /// This method will add a '3' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb3Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(3);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '4' was pressed.
        /// 
        /// This method will add a '4' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb4Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(4);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '5' was pressed.
        /// 
        /// This method will add a '5' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb5Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(5);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '6' was pressed.
        /// 
        /// This method will add a '6' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb6Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(6);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '7' was pressed.
        /// 
        /// This method will add a '7' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb7Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(7);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '8' was pressed.
        /// 
        /// This method will add a '8' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb8Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(8);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '9' was pressed.
        /// 
        /// This method will add a '9' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nb9Button_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            addNumber(9);
            updateNbConstraintBox();
        }

        /// <summary>
        /// Adds a number to the current non-boolean constraint.
        /// </summary>
        /// <param name="n">Number that needs to be inserted</param>
        private void addNumber(int n)
        {
            double d;

            if (nbConstraintList.Count > 0 && nbConstraintList.Count % 2 == 1
                && (Double.TryParse(nbConstraintList[nbConstraintList.Count - 1], out d)
                || nbConstraintList[nbConstraintList.Count - 1] == "-"))
                nbConstraintList[nbConstraintList.Count - 1] += n.ToString();
            else
                nbConstraintList.Add(n.ToString());

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '+' was pressed.
        /// 
        /// This method will add a '+' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbPlusButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList.Add("+");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '-' was pressed.
        /// 
        /// This method will add a '-' to the end of the current non-boolean
        /// constraint. If the last addition to the non-boolean constraint already
        /// was a '-' and it is an algebraic sign, it will be nullified.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbSubButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            if (nbConstraintList.Count > 0 && nbConstraintList.Count % 2 == 1
                && nbConstraintList[nbConstraintList.Count - 1] == "-")
                nbConstraintList.RemoveAt(nbConstraintList.Count - 1);
            else
                nbConstraintList.Add("-");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '*' was pressed.
        /// 
        /// This method will add a '*' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbMulButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList.Add("*");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '.' was pressed.
        /// 
        /// This method will add a '.' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbPointButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList[nbConstraintList.Count - 1] += ".";

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '=' was pressed.
        /// 
        /// This method will add a '=' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbEqButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList.Add("=");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '>=' was pressed.
        /// 
        /// This method will add a '>=' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbGreEqButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList.Add(">=");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding a '>' was pressed.
        /// 
        /// This method will add a '>' to the end of the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbGreButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            nbConstraintList.Add(">");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding the non-boolean constraint was
        /// pressed.
        /// 
        /// This method will add the current non-boolean constraint to the
        /// current variability model. After that the current non-boolean
        /// constraint will be cleared for the next non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbAddConstraintButton_Click(object sender, EventArgs e)
        {
            nbConstraintListBox.Items.Add(nbConstraintTextBox.Text);
            GlobalState.varModel.NonBooleanConstraints.Add(
                new NonBooleanConstraint(nbConstraintTextBox.Text, GlobalState.varModel));

            nbConstraintList.Clear();

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for removing the last addition to the
        /// non-boolean constraint was pressed.
        /// 
        /// This method will remove the last addition to the non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbRemoveButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            double d;

            if (Double.TryParse(nbConstraintList[nbConstraintList.Count - 1], out d))
            {
                if (nbConstraintList[nbConstraintList.Count - 1].Length > 1)
                {
                    string lastString = nbConstraintList[nbConstraintList.Count - 1];

                    nbConstraintList[nbConstraintList.Count - 1] = lastString.Substring(0, lastString.Length - 1);
                }
                else
                    nbConstraintList.RemoveAt(nbConstraintList.Count - 1);
            }
            else
                nbConstraintList.RemoveAt(nbConstraintList.Count - 1);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for deleting a non-boolean constraint was pressed.
        /// 
        /// This method will delete the selected non-boolean constraint from the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nbDeleteButton_Click(object sender, EventArgs e)
        {
            setNbConstraintList();

            int selIndex = nbConstraintListBox.SelectedIndex;

            if (selIndex > -1)
            {
                GlobalState.varModel.NonBooleanConstraints.RemoveAt(selIndex);
                nbConstraintListBox.Items.RemoveAt(selIndex);

                updateNbConstraintBox();
            }
        }
        /// <summary>
        /// Checks if the parameter is a separator.
        /// </summary>
        /// <param name="s">Parameter that needs to be checked</param>
        /// <returns>true, if the parameter is a separator, else false</returns>
        private bool isSeparator(string s)
        {
            return s.Equals("=") || s.Equals(">=") || s.Equals(">");
        }

        /// <summary>
        /// Sets all numerical buttons to the specified status.
        /// </summary>
        /// <param name="enabled">Enabled-status of buttons</param>
        private void setNumericalButtons(bool enabled)
        {
            nb0Button.Enabled = enabled;
            nb1Button.Enabled = enabled;
            nb2Button.Enabled = enabled;
            nb3Button.Enabled = enabled;
            nb4Button.Enabled = enabled;
            nb5Button.Enabled = enabled;
            nb6Button.Enabled = enabled;
            nb7Button.Enabled = enabled;
            nb8Button.Enabled = enabled;
            nb9Button.Enabled = enabled;
        }

        /// <summary>
        /// Sets all separator buttons to the specified status.
        /// </summary>
        /// <param name="enabled">Enabled-status of buttons</param>
        private void setSeparatorButtons(bool enabled)
        {
            nbEqButton.Enabled = enabled;
            nbGreEqButton.Enabled = enabled;
            nbGreButton.Enabled = enabled;
        }

        /// <summary>
        /// Updates the view for creating non-boolean constraints.
        /// </summary>
        private void updateNbConstraintBox()
        {
            if (nbOptionDDList.Items.Count == 0)
            {
                nbOptionDDList.Enabled = false;
                nbAddOptionButton.Enabled = false;

                nbPlusButton.Enabled = false;
                nbSubButton.Enabled = false;
                nbMulButton.Enabled = false;
                nbPointButton.Enabled = false;
                nbEqButton.Enabled = false;
                nbGreEqButton.Enabled = false;
                nbGreButton.Enabled = false;
                setNumericalButtons(false);

                nbConstraintTextBox.Enabled = false;
                nbAddConstraintButton.Enabled = false;
                nbRemoveButton.Enabled = false;
                nbConstraintListBox.Enabled = false;
                nbDeleteButton.Enabled = false;
                return;
            }

            nbConstraintTextBox.Enabled = true;
            nbConstraintTextBox.Text = String.Join(" ", nbConstraintList);
            
            // Enabling the buttons depending on the current state
            int offset = nbConstraintList.Count > 0
                && nbConstraintList[nbConstraintList.Count - 1] == "-" ? 1 : 0;

            if (nbConstraintList.Count % 2 == 0)
            {
                nbAddOptionButton.Enabled = true;

                setNumericalButtons(true);

                nbPlusButton.Enabled = false;
                nbSubButton.Enabled = true;
                nbMulButton.Enabled = false;
                nbPointButton.Enabled = false;

                setSeparatorButtons(false);
            }
            else
            {
                double d;

                if (nbConstraintList[nbConstraintList.Count - 1] == "-")
                {
                    // The last element is an algebraic sign
                    nbAddOptionButton.Enabled = true;

                    setNumericalButtons(true);

                    nbPlusButton.Enabled = false;
                    nbSubButton.Enabled = true;
                    nbMulButton.Enabled = false;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(false);
                }
                else if (nbConstraintList[nbConstraintList.Count - 1].EndsWith("."))
                {
                    // There is currently a number that gets decimal digits
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(true);

                    nbPlusButton.Enabled = false;
                    nbSubButton.Enabled = false;
                    nbMulButton.Enabled = false;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(false);
                }
                else if (Double.TryParse(nbConstraintList[nbConstraintList.Count - 1], out d))
                {
                    // The last element is a (decimal) number
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(true);

                    nbPlusButton.Enabled = true;
                    nbSubButton.Enabled = true;
                    nbMulButton.Enabled = true;
                    nbPointButton.Enabled = !nbConstraintList[nbConstraintList.Count - 1].Contains(".");

                    setSeparatorButtons(!nbConstraintList.Any(x => isSeparator(x)));
                }
                else
                {
                    // Only operators and/or separators are allowed
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(false);

                    nbPlusButton.Enabled = true;
                    nbSubButton.Enabled = true;
                    nbMulButton.Enabled = true;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(!nbConstraintList.Any(x => isSeparator(x)));
                }
            }

            nbAddConstraintButton.Enabled = nbConstraintList.Any(isSeparator)
                && (nbConstraintList.Count - offset) % 2 == 1
                && !nbConstraintList[nbConstraintList.Count - 1].EndsWith(".")
                && nbConstraintList[nbConstraintList.Count - 1] != "-";
            nbRemoveButton.Enabled = nbConstraintTextBox.Text.Length > 0;
            nbDeleteButton.Enabled = nbConstraintListBox.Items.Count > 0;
        }
    }
}