using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SPLConqueror_Core;

namespace VariabilitModel_GUI
{
    public partial class EditOptionDialog : Form
    {
        private const string NO_DATA = "None";
        private const string DESCRIPTION_CHANGE_NAME = "Please enter a new feature name:";
        private const string DESCRIPTION_SET_PARENT = "Choose the new parent of this feature:";
        private const string DESCRIPTION_CHANGE_RANGE = "Please enter a new range of values for this feature:";
        private const string DESCRIPTION_CHANGE_STEP_SIZE = "Please enter a new step function:";

        VariabilityModel_Form parent = null;
        ConfigurationOption currentOption = null;

        public EditOptionDialog(VariabilityModel_Form parentForm, ConfigurationOption selectedOption)
        {
            parent = parentForm;
            InitializeComponent();
            
            initializeComponents();
        
            if (selectedOption != null)
                selectOptionComboBox.SelectedIndex = selectOptionComboBox.Items.IndexOf(selectedOption);
        }

        /// <summary>
        /// Initializes all components within the form.
        /// </summary>
        private void initializeComponents()
        {
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();

            optionalCheckBox.Visible = false;

            renameOptionButton.Enabled = false;
            setParentButton.Enabled = false;
            numericSettingsGroupBox.Enabled = false;

            foreach (ConfigurationOption opt in options)
                selectOptionComboBox.Items.Add(opt);

        }

        /// <summary>
        /// Invokes if the selected index of the corresponding combo box chnaged.
        /// 
        /// This method will update the parent label, the settings of the option and the lists
        /// of the excluded and required combinations of features.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void selectOptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentOption = (ConfigurationOption)selectOptionComboBox.SelectedItem;

            parentLabel.Text = currentOption.Parent == null ? NO_DATA : currentOption.Parent.Name;
            setParentButton.Enabled = currentOption != GlobalState.varModel.Root;
            renameOptionButton.Enabled = true;

            if (currentOption is BinaryOption)
            {
                optionalCheckBox.Visible = true;
                optionalCheckBox.Checked = ((BinaryOption)currentOption).Optional;
                numericSettingsGroupBox.Enabled = false;
                rangeLabel.Text = NO_DATA;
                stepSizeLabel.Text = NO_DATA;
            } else if (currentOption is NumericOption)
            {
                optionalCheckBox.Visible = false;
                numericSettingsGroupBox.Enabled = true;
                rangeLabel.Text = "( " + ((NumericOption)currentOption).Min_value + ", "
                    + ((NumericOption)currentOption).Max_value + " )";
                stepSizeLabel.Text = ((NumericOption)currentOption).StepFunction.ToString();
            }

            prefixTextBox.Text = currentOption.Prefix;
            postfixTextBox.Text = currentOption.Postfix;

            excludesCheckedListBox.Items.Clear();
            requiresCheckedListBox.Items.Clear();

            foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
            {
                if (opt != currentOption)
                {
                    excludesCheckedListBox.Items.Add(opt);
                    requiresCheckedListBox.Items.Add(opt);
                }
            }
            
            excludesOverviewListBox.Items.Clear();
            requiresOverviewListBox.Items.Clear();

            foreach (List<ConfigurationOption> excludeList in currentOption.Excluded_Options)
                excludesOverviewListBox.Items.Add(String.Join(" | ", excludeList));

            foreach (List<ConfigurationOption> requireList in currentOption.Implied_Options)
                requiresOverviewListBox.Items.Add(String.Join(" | ", requireList));

            excludesDelButton.Enabled = excludesOverviewListBox.Items.Count > 0;
            requiresDelButton.Enabled = requiresOverviewListBox.Items.Count > 0;
            excludesAddButton.Enabled = false;
            requiresAddButton.Enabled = false;
        }

        /// <summary>
        /// Invokes if the check state of the corresponding check box has changed.
        /// 
        /// This method will set the 'Optional'-value of the current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void optionalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ((BinaryOption)currentOption).Optional = optionalCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the button for renaming options has been pressed.
        /// 
        /// This method will open a new dialog for entering the new name of the current
        /// option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void renameOptionButton_Click(object sender, EventArgs e)
        {
            Tuple<DialogResult, string> result = RenameDialog();

            if (result.Item1 == DialogResult.OK) {
                List<string> editedBooleanConstraints = new List<string>();
                List<NonBooleanConstraint> editedNonBooleanConstraints = new List<NonBooleanConstraint>();

                foreach (string boolConst in GlobalState.varModel.BooleanConstraints)
                {
                    string[] constParts = boolConst.Split(' ');

                    for (int i = 0; i < constParts.Length; i++)
                        constParts[i] = constParts[i] == currentOption.Name ? result.Item2 : constParts[i];

                    editedBooleanConstraints.Add(String.Join(" ", constParts));
                }

                foreach (NonBooleanConstraint nbConst in GlobalState.varModel.NonBooleanConstraints)
                {
                    string[] constParts = nbConst.ToString().Split(' ');

                    for (int i = 0; i < constParts.Length; i++)
                        constParts[i] = constParts[i] == currentOption.Name ? result.Item2 : constParts[i];

                    editedNonBooleanConstraints.Add(new NonBooleanConstraint(String.Join(" ", constParts), GlobalState.varModel));
                }

                GlobalState.varModel.BooleanConstraints = editedBooleanConstraints;
                GlobalState.varModel.NonBooleanConstraints = editedNonBooleanConstraints;
                currentOption.Name = result.Item2;

                selectOptionComboBox.Items.Clear();

                foreach (ConfigurationOption opt in GlobalState.varModel.getOptions())
                    selectOptionComboBox.Items.Add(opt);
                
                selectOptionComboBox.SelectedIndex = selectOptionComboBox.Items.IndexOf(currentOption);
            }
        }

        /// <summary>
        /// Invokes if the button for setting the parent has been pressed.
        /// 
        /// This method will open a new dialog for selecting the new parent of the
        /// current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void setParentButton_Click(object sender, EventArgs e)
        {
            Tuple<DialogResult, ConfigurationOption> result = SetParentDialog();

            if (result.Item1 == DialogResult.OK)
            {
                currentOption.Parent.Children.Remove(currentOption);

                currentOption.Parent = result.Item2;
                currentOption.Parent.Children.Add(currentOption);
            }
        }

        /// <summary>
        /// Invokes if the button for changing the range of values has been pressed.
        /// 
        /// This method will open a dialog to change the range of the selected option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void changeRangeButton_Click(object sender, EventArgs e)
        {
            Tuple<DialogResult, string, string> result = ChangeRangeDialog();

            if (result.Item1 == DialogResult.OK)
            {
                rangeLabel.Text = "( " + result.Item2 + ", " + result.Item3 + " )";

                ((NumericOption)currentOption).Min_value = Convert.ToDouble(result.Item2);
                ((NumericOption)currentOption).Max_value = Convert.ToDouble(result.Item3);
            }
        }

        /// <summary>
        /// Invokes if the button for changing the step size has been pressed.
        /// 
        /// This method will open a dialog to change the step size of the selected
        /// option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void changeStepSizeButton_Click(object sender, EventArgs e)
        {
            Tuple<DialogResult, string> result = ChangeStepSizeDialog();

            if (result.Item1 == DialogResult.OK)
            {
                stepSizeLabel.Text = result.Item2;
                ((NumericOption)currentOption).StepFunction = new InfluenceFunction(result.Item2);
            }
        }

        /// <summary>
        /// Invokes if the text of the prefix textbox has changed.
        /// 
        /// This method will set the prefix of the selected option to the text of the
        /// corresponding textbox.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void prefixTextBox_TextChanged(object sender, EventArgs e)
        {
            currentOption.Prefix = prefixTextBox.Text;
        }

        /// <summary>
        /// Invokes if the text of the postfix textbox has changed.
        /// 
        /// This method will set the postfix of the selected option to the text of the
        /// corresponding textbox.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void postfixTextBox_TextChanged(object sender, EventArgs e)
        {
            currentOption.Postfix = postfixTextBox.Text;
        }

        /// <summary>
        /// Invokes if the text of the corresponding text box has been changed.
        /// 
        /// ATTENTION: This will be open for an extension if needed.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void variantGenerationTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO: EXTENSION
        }

        /// <summary>
        /// Invokes if the number of selected items in the list check box for excluded
        /// combinations has changed.
        /// 
        /// This method will activate/deactivate the corresponding 'Add'-button
        /// depending on the amount of checked items.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void excludesCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            excludesAddButton.Enabled = excludesCheckedListBox.CheckedIndices.Count > 0;
        }

        /// <summary>
        /// Invokes if the button for deleting excluded combinations has been pressed.
        /// 
        /// This method will delete the currently selected combination of the corresponding
        /// list box from the list box and the current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void excludesDelButton_Click(object sender, EventArgs e)
        {
            int selIndex = excludesOverviewListBox.SelectedIndex;

            if (selIndex > -1)
            {
                currentOption.Excluded_Options.RemoveAt(selIndex);
                excludesOverviewListBox.Items.RemoveAt(selIndex);

                excludesDelButton.Enabled = excludesOverviewListBox.Items.Count > 0;
            }
        }

        /// <summary>
        /// Invokes if the button for adding excluded combinations has been pressed.
        /// 
        /// This method will add the current combination of the corresponding checked
        /// list box to the list box and the current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void excludesAddButton_Click(object sender, EventArgs e)
        {
            List<ConfigurationOption> excludeCombination = new List<ConfigurationOption>();

            foreach (ConfigurationOption opt in excludesCheckedListBox.CheckedItems)
                excludeCombination.Add(opt);

            if (excludeCombination.Count > 0)
            {
                currentOption.Excluded_Options.Add(excludeCombination);
                excludesOverviewListBox.Items.Add(String.Join(" | ", excludeCombination));

                excludesDelButton.Enabled = true;
            }
        }

        /// <summary>
        /// Invokes if the number of selected items in the list check box for required
        /// combinations has changed.
        /// 
        /// This method will activate/deactivate the corresponding 'Add'-button
        /// depending on the amount of checked items.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void requiresCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            requiresAddButton.Enabled = requiresCheckedListBox.CheckedIndices.Count > 0;
        }

        /// <summary>
        /// Invokes if the button for deleting required combinations has been pressed.
        /// 
        /// This method will delete the currently selected combination of the corresponding
        /// list box from the list box and the current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void requiresDelButton_Click(object sender, EventArgs e)
        {
            int selIndex = requiresOverviewListBox.SelectedIndex;

            if (selIndex > -1)
            {
                currentOption.Implied_Options.RemoveAt(selIndex);
                requiresOverviewListBox.Items.RemoveAt(selIndex);

                requiresDelButton.Enabled = requiresOverviewListBox.Items.Count > 0;
            }
        }

        /// <summary>
        /// Invokes if the button for adding required combinations has been pressed.
        /// 
        /// This method will add the current combination of the corresponding checked
        /// list box to the list box and the current option.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void requiresAddButton_Click(object sender, EventArgs e)
        {
            List<ConfigurationOption> requireCombination = new List<ConfigurationOption>();

            foreach (ConfigurationOption opt in requiresCheckedListBox.CheckedItems)
                requireCombination.Add(opt);

            if (requireCombination.Count > 0)
            {
                currentOption.Implied_Options.Add(requireCombination);
                requiresOverviewListBox.Items.Add(String.Join(" | ", requireCombination));

                requiresDelButton.Enabled = true;
            }
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //string path = fm.getFeatureModelPath(this.fm.getElementByNameUnsafe(this.selectBox.Text).getName(), "");
                //if (path.Contains("/" + otherBox.Text) || path.Contains(otherBox.Text + "/"))
                //    MessageBox.Show("Loop in Parent Child Relationship detected!");
                //else
                //    this.listBox1.Items.Add(otherBox.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //this.excludeSingleListBox.Items.Add(otherOptionComboBox.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //this.requiresSingleListBox.Items.Add(otherOptionComboBox.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //if (this.fm.getElementByNameUnsafe(this.otherBox.Text).isOptional() == true)
                //{
                //    State f1 = new State(this.otherBox.Text, "mandatory");
                //    if (f1.ShowDialog() == DialogResult.OK)
                //    {
                //        this.fm.getElementByNameUnsafe(this.otherBox.Text).setOptional(false);
                //        this.listBox5.Items.Add(otherBox.Text);
                //    }
                //    else
                //        return;
                //}
                //else
                //    this.listBox4.Items.Add(otherBox.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //if (this.fm.getElementByNameUnsafe(this.otherBox.Text).isOptional() == false)
                //{
                //    State f1 = new State(this.otherBox.Text, "optional");
                //    if (f1.ShowDialog() == DialogResult.OK)
                //    {
                //        this.fm.getElementByNameUnsafe(this.otherBox.Text).setOptional(true);
                //        this.listBox5.Items.Add(otherBox.Text);
                //    }
                //    else
                //        return;
                //}
                //else
                //    this.listBox5.Items.Add(otherBox.Text);
            }
        }

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    if (otherBox.Text != "")
        //    {
        //        this.listBox6.Items.Add(otherBox.Text);
        //    }
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    if (otherBox.Text != "")
        //    {
        //        this.listBox7.Items.Add(otherBox.Text);
        //    }
        //}

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.nbConstraintBox.SelectedItem != null)
            {
                //this.parents.Add(this.nbConstraintBox.SelectedItem.ToString());
                this.nbConstraintBox.Items.Remove(this.nbConstraintBox.SelectedItem);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            /*if (this.excludeSingleListBox.SelectedItem != null)
            {
                //this.exc.Add(this.excludeSingleListBox.SelectedItem.ToString());
                this.excludeSingleListBox.Items.Remove(this.excludeSingleListBox.SelectedItem);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            /*if (this.requiresSingleListBox.SelectedItem != null)
            {
                //this.req.Add(this.requiresSingleListBox.SelectedItem.ToString());
                this.requiresSingleListBox.Items.Remove(this.requiresSingleListBox.SelectedItem);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.otherOptionComboBox.Items.Remove(this.selectOptionComboBox.Text);
            
            BinaryOption temp = GlobalState.varModel.getBinaryOption(this.selectOptionComboBox.Text);

            if (temp == null)
                return;
            
            if (temp.Optional)
                this.optionalCheckBox.Checked = true;
            else
                this.optionalCheckBox.Checked = false;
            //fill all boxes
            this.nbConstraintBox.Items.Clear();

            excludesOverview.Items.Clear();
            //excludeSingleListBox.Items.Clear();
            for (int i = 0; i < temp.Excluded_Options.Count; i++)
            {
                List<ConfigurationOption> currExludes = temp.Excluded_Options[i];
                StringBuilder currExString = new StringBuilder();
                for (int j = 0; j < currExludes.Count-1; j++)
                {
                    currExString.Append(currExludes[j].Name+ " | ");
                }
                currExString.Append(currExludes[currExludes.Count - 1].Name);
                excludesOverview.Items.Add(currExString.ToString());
            }

            requiresOverview.Items.Clear();
            //requiresSingleListBox.Items.Clear();
            for (int i = 0; i < temp.Implied_Options.Count; i++)
            {
                List<ConfigurationOption> currImplied = temp.Implied_Options[i];
                StringBuilder currImpString = new StringBuilder();
                for (int j = 0; j < currImplied.Count - 1; j++)
                {
                    currImpString.Append(currImplied[j].Name + " | ");
                }
                currImpString.Append(currImplied[currImplied.Count - 1].Name);
                excludesOverview.Items.Add(currImpString.ToString());
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //private void insertParent()
        //{
        //    int id;
        //    if (this.nbConstraintBox.Items.Count == 0)
        //        return;
        //    if (this.comboBox3.SelectedItem.ToString() == "feature")
        //        {
        //            //id = this.fm.getElementByNameUnsafe(this.listBox1.Items[0].ToString()).getID();
        //            //if (!this.fm.getElementByNameUnsafe(this.selectBox.Text).isParent(id))
        //            //{
        //            //    this.fm.getElementByNameUnsafe(this.selectBox.Text).setParent(id);//me
        //            //    this.fm.getElementById(id).addChild(this.fm.getElementByNameUnsafe(this.selectBox.Text).getID());//target
        //            //}
        //        }
        //    else
        //    {
        //        //Element current = fm.getElementByNameUnsafe(this.selectBox.Text);
        //        //for (int i = 0; i < this.listBox1.Items.Count; i++)
        //        //{
        //        //    Element temp = this.fm.getElementByNameUnsafe(this.listBox1.Items[i].ToString());
        //        //    current.addDerivativeParent(temp);
        //        //}
        //    }
           
        //}


        private void button18_Click(object sender, EventArgs e)
        {
            if (otherOptionComboBox.Text != "")
            {
                //string path = this.fm.getFeatureModelPath(this.fm.getElementByNameUnsafe(this.selectBox.Text).getName(), "");
                //if (path.Contains("/" + otherBox.Text) || path.Contains(otherBox.Text + "/"))
                //    MessageBox.Show("Loop in Parent Child Relationship detected!");
                //else
                //    this.listBox8.Items.Add(otherBox.Text);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //this.fm.updateCodeUnits();
            //GeneratingInteractions.automaticGenerationOfDerivatives(fm);
        }

        private void renameOption_Click(object sender, EventArgs e)
        {
            /*if (this.renameOption_TextBox.Text.Length > 0)
            {
                if (this.renameOption_TextBox.Text.Contains("-"))
                {
                    WarningLabel.Text = ("No '-' in option name!");
                    return;
                }
                GlobalState.varModel.getOption(this.selectOptionComboBox.Text).Name = renameOption_TextBox.Text;
                updateGUI();
            }
        }

        private void button21_Click(object sender, EventArgs ea)
        {
            //List<Element> derivatives = new List<Element>();
            //List<Element> dependenciesToRemove = new List<Element>();
            //foreach (Element e in fm.getAllElements())
            //{
            //    if (e.getType() == "derivative")
            //        derivatives.Add(e);
            //}

            //foreach (Element deriv in derivatives)
            //{
            //    foreach (Element deriv2 in derivatives)
            //    {
            //        if (deriv != deriv2 && deriv.getDerivativeParents().Count == deriv2.getDerivativeParents().Count)
            //        {
            //            bool sameDependency = true;
            //            foreach (Element parent in deriv2.getDerivativeParents())
            //            {
            //                if (!deriv.getDerivativeParents().Contains(parent))
            //                {
            //                    sameDependency = false;
            //                    break;
            //                }
            //            }
            //            foreach (Element parent in deriv.getDerivativeParents())
            //            {
            //                if (!deriv2.getDerivativeParents().Contains(parent))
            //                {
            //                    sameDependency = false;
            //                    break;
            //                }
            //            }
            //            if (sameDependency)
            //            {
            //                if (!dependenciesToRemove.Contains(deriv) && !dependenciesToRemove.Contains(deriv2))
            //                    dependenciesToRemove.Add(deriv);
            //            }
            //        }
            //    }
            //}

            //foreach (Element e in dependenciesToRemove)
            //{
            //    fm.removeElement(e);
            //}
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //List<Element> depedenciesToAdd = new List<Element>();
            //foreach (Element current in fm.getAllElements())
            //{
            //    if (current.getType() != "feature")
            //        continue;
            //    if (current.isOptional() == false && current.getAlternativeIDs().Count == 0 && current.getCommulativeIDs().Count == 0)
            //        continue;
            //    foreach (Element second in fm.getAllElements())
            //    {
            //        if (second.getType() != "feature")
            //            continue;
            //        if (second == current)
            //            continue;
            //        if (second.isOptional() == false && second.getAlternativeIDs().Count == 0 && second.getCommulativeIDs().Count == 0)
            //            continue;
            //        if (current.getAlternatives().Contains(second))
            //            continue;
            //        string featureModelpath1 = fm.getFeatureModelPath(current.getName(), "");
            //        if (featureModelpath1.Contains(second.getName()))
            //            continue;
            //        featureModelpath1 = fm.getFeatureModelPath(second.getName(), "");
            //        if (featureModelpath1.Contains(current.getName()))
            //            continue;
            //        List<Element> tempConfig = new List<Element>();
            //        tempConfig.Add(current);
            //        tempConfig.Add(second);

            //        if (vg.minimizeConfig(tempConfig, fm, false, null, false).Count == 0)
            //            continue;
            //        Element pairWiseDepedency = new Element("dPair_" + current.getName() + "_" + second.getName(), fm.createID(), fm);
            //        pairWiseDepedency.setType("derivative");
            //        pairWiseDepedency.addDerivativeParent(current);
            //        pairWiseDepedency.addDerivativeParent(second);
            //        if (testDependencyOccurance(depedenciesToAdd,pairWiseDepedency))
            //            depedenciesToAdd.Add(pairWiseDepedency);
            //    }
            //}
            //int addedDeps = 0;
            //foreach (Element current in depedenciesToAdd)
            //{
            //    if (isAlreadyExistingDependencies(current) == false)
            //    {
            //        addedDeps++;
            //        fm.addElement(current);
            //    }
            //}
            //MessageBox.Show("Added " + addedDeps.ToString() + " pair-wise depedencies to the model.");
        }

        //Checks if there is already an existing dependencies with the same parents
        private bool isAlreadyExistingDependencies(ConfigurationOption current)
        {
            //foreach (Element dependency in fm.getAllElements())
            //{
            //    if (dependency.getType() == "derivative" && dependency.getDerivativeParents().Count == current.getDerivativeParents().Count)
            //    {
            //        bool sameElement = true;
            //        foreach (Element parent in current.getDerivativeParents())
            //        {
            //            if (!dependency.getDerivativeParents().Contains(parent))
            //            {
            //                sameElement = false;
            //                break;
            //            }
            //        }
            //        if (sameElement)
            //            return true;
            //    }
            //}
            return false;
        }

        //checks if the element is already present in the list
        private bool testDependencyOccurance(List<ConfigurationOption> depedenciesToAdd, ConfigurationOption pairWiseDepedency)
        {
            //List<Element> pairWiseParents = pairWiseDepedency.getDerivativeParents();
            //foreach (Element current in depedenciesToAdd)
            //{
            //    if (pairWiseDepedency.getDerivativeParents().Count != current.getDerivativeParents().Count)
            //        continue;
            //    bool sameElement = true;
            //    foreach (Element parent in current.getDerivativeParents())//should be enough, because both elements (current and pairWise) have the same number of parents
            //    {
            //        if (!pairWiseParents.Contains(parent))
            //        {
            //            sameElement = false;
            //            break;
            //        }
            //    }
            //    if (sameElement)
            //        return false;
            //}
            return true;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //DependencyEditor de = new DependencyEditor(this.fm);
            //de.Show();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            ConfigurationOption toDelete = GlobalState.varModel.getOption(this.selectOptionComboBox.Text);
            GlobalState.varModel.deleteOption(toDelete);
            
            //fm.removeElement(toDelete);
            //removeParent();
            //removeAlternative();
            //removeCommulative();
            //removeMandatory();
            //removeOptional();
            //removeRequires();
            //removeExcludes();
            //removeOrder();
            //this.parents = new List<string>();
            //this.manda = new List<string>();
            //this.opt = new List<string>();
            //this.alter = new List<string>();
            //this.comm = new List<string>();
            //this.exc = new List<string>();
            //this.req = new List<string>();
            //this.order = new List<string>();
            //this.selectBox.Items.Clear();
            //this.otherBox.Items.Clear();
            //for (int i = 0; i < fm.getAllElements().Count; i++)
            //{
            //    this.otherBox.Items.Add(fm.getAllElements()[i].getName());
            //    this.selectBox.Items.Add(fm.getAllElements()[i].getName());
            //}
            throw new NotImplementedException();
        }

        private void WorkloadConstraint_button_Click(object sender, EventArgs e)
        {
            //VariableFeatureModellConstrainCreatorDlg variableWorkloadConstraint = new VariableFeatureModellConstrainCreatorDlg(ref fm);
            //variableWorkloadConstraint.Show();
        }

        private void UpdateCurrEquation()
        {
            nonBoolConstraint.Text = "";
            for (int i = 0; i < currentEquation.Count; i++)
            {
                nonBoolConstraint.Text += currentEquation[i] + "";
            }
        }

        private void num7_Click(object sender, EventArgs e)
        {
            currentEquation.Add("7");
            UpdateCurrEquation();
        }

        private void num8_Click(object sender, EventArgs e)
        {
            currentEquation.Add("8");
            UpdateCurrEquation();
        }

        private void num9_Click(object sender, EventArgs e)
        {
            currentEquation.Add("9");
            UpdateCurrEquation();
        }

        private void num1_Click(object sender, EventArgs e)
        {
            currentEquation.Add("1");
            UpdateCurrEquation();
        }

        private void num5_Click(object sender, EventArgs e)
        {
            currentEquation.Add("5");
            UpdateCurrEquation();
        }

        private void num6_Click(object sender, EventArgs e)
        {
            currentEquation.Add("6");
            UpdateCurrEquation();
        }

        private void num3_Click(object sender, EventArgs e)
        {
            currentEquation.Add("3");
            UpdateCurrEquation();
        }

        private void num2_Click(object sender, EventArgs e)
        {
            currentEquation.Add("2");
            UpdateCurrEquation();
        }

        private void num4_Click(object sender, EventArgs e)
        {
            currentEquation.Add("4");
            UpdateCurrEquation();
        }

        private void num0_Click(object sender, EventArgs e)
        {
            currentEquation.Add("0");
            UpdateCurrEquation();
        }

        private void numPlus_Click(object sender, EventArgs e)
        {
            currentEquation.Add(" + ");
            UpdateCurrEquation();
        }

        private void numTimes_Click(object sender, EventArgs e)
        {
            currentEquation.Add(" * ");
            UpdateCurrEquation();
        }

        private void numEq_Click(object sender, EventArgs e)
        {
            currentEquation.Add("=");
            UpdateCurrEquation();
        }

        private void numGre_Click(object sender, EventArgs e)
        {
            currentEquation.Add(">");
            UpdateCurrEquation();
        }

        private void numDif_Click(object sender, EventArgs e)
        {
            currentEquation.Add(" - ");
            UpdateCurrEquation();
        }


        private void deleteNBConst_Click(object sender, EventArgs e)
        {
            if (nbConstraintBox.SelectedIndex != -1)
            {
                GlobalState.varModel.NonBooleanConstraints.Remove((NonBooleanConstraint)nbConstraintBox.SelectedItem);
                nbConstraintBox.Items.RemoveAt(nbConstraintBox.SelectedIndex);

            }
        }

        private void removeNBConst_Click(object sender, EventArgs e)
        {
            if (currentEquation.Count > 0)
            {
                currentEquation.RemoveAt(currentEquation.Count - 1);
                UpdateCurrEquation();
            }
        }

        private void addOption_Click(object sender, EventArgs e)
        {
            if (nbConstOptions.Text.Length != 0)
            {
                currentEquation.Add(nbConstOptions.Text);
                UpdateCurrEquation();
            }
        }

        private void addNBConst_Click(object sender, EventArgs e)
        {
            StringBuilder currConst = new StringBuilder();
            for (int i = 0; i < currentEquation.Count; i++)
            {
                currConst.Append(currentEquation[i]);
            }
            currentEquation = new List<string>();
            UpdateCurrEquation();
            nbConstraintBox.Items.Add(currConst);
            GlobalState.varModel.NonBooleanConstraints.Add(new NonBooleanConstraint(currConst.ToString(), GlobalState.varModel));
        }

        private void finExclude_Click(object sender, EventArgs e)
        {
            /*
            StringBuilder sb = new StringBuilder();
            List<ConfigurationOption> newExcludeList = new List<ConfigurationOption>();
            for (int i = 0; i < excludeSingleListBox.Items.Count-2; i++)
            {
                sb.Append(excludeSingleListBox.Items[i].ToString()+" | ");
                newExcludeList.Add(GlobalState.varModel.getBinaryOption(excludeSingleListBox.Items[i].ToString()));
            }
            sb.Append(excludeSingleListBox.Items[excludeSingleListBox.Items.Count-1].ToString());
            newExcludeList.Add(GlobalState.varModel.getBinaryOption(excludeSingleListBox.Items[excludeSingleListBox.Items.Count - 1].ToString()));
            excludeSingleListBox.Items.Clear();

            excludesOverview.Items.Add(sb.ToString());
            BinaryOption curr = GlobalState.varModel.getBinaryOption(selectOptionComboBox.Text);
            curr.Excluded_Options.Add(newExcludeList);
            
        }

        private void finRequire_Click(object sender, EventArgs e)
        {
            /*
            StringBuilder sb = new StringBuilder();
            List<ConfigurationOption> newRequireList = new List<ConfigurationOption>();
            for (int i = 0; i < requiresSingleListBox.Items.Count - 2; i++)
            {
                sb.Append(requiresSingleListBox.Items[i].ToString() + " | ");
                newRequireList.Add(GlobalState.varModel.getBinaryOption(requiresSingleListBox.Items[i].ToString()));
            }
            sb.Append(excludeSingleListBox.Items[requiresSingleListBox.Items.Count - 1].ToString());
            newRequireList.Add(GlobalState.varModel.getBinaryOption(requiresSingleListBox.Items[requiresSingleListBox.Items.Count - 1].ToString()));
            requiresSingleListBox.Items.Clear();

            requiresOverview.Items.Add(sb.ToString());
            BinaryOption curr = GlobalState.varModel.getBinaryOption(selectOptionComboBox.Text);
            curr.Excluded_Options.Add(newRequireList);
        }

        private void excludesOverview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (excludesOverview.SelectedIndex == -1)
                return;

            //excludeSingleListBox.Items.Clear();
            String currEx = excludesOverview.Items[excludesOverview.SelectedIndex].ToString();
            String[] currExParts = currEx.Split('|');
            for(int i = 0; i < currExParts.Length;i++)
            {
                //excludeSingleListBox.Items.Add(currExParts[i].Trim());
            }
        }

        private void excludesOverviewDelButton_Click(object sender, EventArgs e)
        {
            if (excludesOverview.SelectedIndex != -1)
            {
                ConfigurationOption curr = GlobalState.varModel.getBinaryOption(selectOptionComboBox.SelectedItem.ToString());
                curr.Excluded_Options.RemoveAt(excludesOverview.SelectedIndex);
            }
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void requiresOverviewDelButton_Click(object sender, EventArgs e)
        {
            if (requiresOverview.SelectedIndex != -1)
            {
                ConfigurationOption curr = GlobalState.varModel.getBinaryOption(selectOptionComboBox.SelectedItem.ToString());
                curr.Implied_Options.RemoveAt(requiresOverview.SelectedIndex);
            }
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void setParent_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditOptionDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.InitTreeView();
        }*/

        private Tuple<DialogResult, string> RenameDialog()
        {
            Form form = new Form();
            Label pleaseEnterLabel = new Label();
            TextBox featureNameTextBox = new TextBox();
            Button okButton = new Button();
            Button cancelButton = new Button();
            
            // 
            // pleaseEnterLabel
            // 
            pleaseEnterLabel.Location = new Point(9, 9);
            pleaseEnterLabel.Name = "pleaseEnterLabel";
            pleaseEnterLabel.Size = new Size(200, 13);
            pleaseEnterLabel.TabIndex = 0;
            pleaseEnterLabel.Text = DESCRIPTION_CHANGE_NAME;
            // 
            // featureNameTextBox
            // 
            featureNameTextBox.Location = new Point(12, 25);
            featureNameTextBox.Name = "featureNameTextBox";
            featureNameTextBox.Size = new Size(197, 20);
            featureNameTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(12, 85);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 3;
            okButton.Text = "Ok";
            okButton.Enabled = false;
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(134, 85);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            //
            // form
            //
            form.AutoScaleDimensions = new SizeF(6F, 13F);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.ClientSize = new Size(215, 114);
            form.Controls.Add(cancelButton);
            form.Controls.Add(okButton);
            form.Controls.Add(featureNameTextBox);
            form.Controls.Add(pleaseEnterLabel);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;
            form.Name = "Test";

            featureNameTextBox.TextChanged += (s, e) =>
            {
                okButton.Enabled = featureNameTextBox.Text != ""
                    && GlobalState.varModel.getOption(featureNameTextBox.Text) == null
                    && featureNameTextBox.Text == ConfigurationOption.removeInvalidCharsFromName(featureNameTextBox.Text);
            };

            DialogResult result = form.ShowDialog();
            return new Tuple<DialogResult, string>(result, featureNameTextBox.Text);
        }

        private Tuple<DialogResult, ConfigurationOption> SetParentDialog()
        {
            Form form = new Form();
            Label pleaseEnterLabel = new Label();
            Button okButton = new Button();
            Button cancelButton = new Button();
            ComboBox nextParentComboBox = new ComboBox();
            // 
            // pleaseEnterLabel
            // 
            pleaseEnterLabel.AutoSize = true;
            pleaseEnterLabel.Location = new Point(9, 9);
            pleaseEnterLabel.Name = "pleaseEnterLabel";
            pleaseEnterLabel.Size = new Size(187, 13);
            pleaseEnterLabel.TabIndex = 0;
            pleaseEnterLabel.Text = DESCRIPTION_SET_PARENT;
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(12, 52);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 3;
            okButton.Text = "Ok";
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(128, 52);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // nextParentComboBox
            // 
            nextParentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            nextParentComboBox.FormattingEnabled = true;
            nextParentComboBox.Location = new Point(12, 25);
            nextParentComboBox.Name = "nextParentComboBox";
            nextParentComboBox.Size = new Size(191, 21);
            nextParentComboBox.TabIndex = 5;
            // 
            // form
            // 
            form.AcceptButton = okButton;
            form.AutoScaleDimensions = new SizeF(6F, 13F);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.CancelButton = cancelButton;
            form.ClientSize = new Size(215, 82);
            form.Controls.Add(nextParentComboBox);
            form.Controls.Add(cancelButton);
            form.Controls.Add(okButton);
            form.Controls.Add(pleaseEnterLabel);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;

            foreach (ConfigurationOption opt in getNonChildrenOptions())
                nextParentComboBox.Items.Add(opt);

            nextParentComboBox.SelectedIndex = nextParentComboBox.Items.IndexOf(currentOption.Parent);

            DialogResult result = form.ShowDialog();
            return  new Tuple<DialogResult, ConfigurationOption>(result, (ConfigurationOption)nextParentComboBox.SelectedItem);
        }

        private List<ConfigurationOption> getNonChildrenOptions()
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

        private Tuple<DialogResult, string, string> ChangeRangeDialog()
        {
            Form form = new Form();
            Button okButton = new Button();
            Button cancelButton = new Button();
            Label pleaseEnterLabel = new Label();
            Label minLabel = new Label();
            Label maxLabel = new Label();
            TextBox minTextBox = new TextBox();
            TextBox maxTextBox = new TextBox();
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 57);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 0;
            okButton.Text = "Ok";
            okButton.DialogResult = DialogResult.OK;
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(223, 57);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Cancel";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // pleaseEnterLabel
            // 
            pleaseEnterLabel.AutoSize = true;
            pleaseEnterLabel.Location = new Point(9, 9);
            pleaseEnterLabel.Name = "pleaseEnterLabel";
            pleaseEnterLabel.Size = new Size(179, 13);
            pleaseEnterLabel.TabIndex = 2;
            pleaseEnterLabel.Text = DESCRIPTION_CHANGE_RANGE;
            // 
            // minLabel
            // 
            minLabel.AutoSize = true;
            minLabel.Location = new Point(9, 34);
            minLabel.Name = "minLabel";
            minLabel.Size = new Size(27, 13);
            minLabel.TabIndex = 3;
            minLabel.Text = "Min:";
            // 
            // maxLabel
            // 
            maxLabel.AutoSize = true;
            maxLabel.Location = new Point(167, 34);
            maxLabel.Name = "maxLabel";
            maxLabel.Size = new Size(30, 13);
            maxLabel.TabIndex = 4;
            maxLabel.Text = "Max:";
            // 
            // minTextBox
            // 
            minTextBox.Location = new Point(50, 31);
            minTextBox.Name = "minTextBox";
            minTextBox.Size = new Size(90, 20);
            minTextBox.Text = ((NumericOption)currentOption).Min_value.ToString();
            minTextBox.TabIndex = 5;
            // 
            // maxTextBox
            // 
            maxTextBox.Location = new Point(208, 31);
            maxTextBox.Name = "maxTextBox";
            maxTextBox.Size = new Size(90, 20);
            maxTextBox.Text = ((NumericOption)currentOption).Max_value.ToString();
            maxTextBox.TabIndex = 6;
            //
            // form
            //
            form.AutoScaleDimensions = new SizeF(6F, 13F);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.ClientSize = new Size(maxTextBox.Right + 10, okButton.Bottom + 10);
            form.Controls.Add(cancelButton);
            form.Controls.Add(okButton);
            form.Controls.Add(minLabel);
            form.Controls.Add(minTextBox);
            form.Controls.Add(maxLabel);
            form.Controls.Add(maxTextBox);
            form.Controls.Add(pleaseEnterLabel);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            minTextBox.TextChanged += (s, e) =>
            {
                bool stillOk = true;

                // Check if min value is a number
                foreach (char c in minTextBox.Text)
                    stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

                stillOk &= minTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;

                // Check if max value is a number
                foreach (char c in maxTextBox.Text)
                    stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

                stillOk &= maxTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;
                

                // Check if range is valid
                double min;
                double max;

                Double.TryParse(minTextBox.Text, out min);
                Double.TryParse(maxTextBox.Text, out max);

                stillOk &= min <= max;

                okButton.Enabled = stillOk;
            };

            maxTextBox.TextChanged += (s, e) =>
            {
                bool stillOk = true;

                // Check if min value is a number
                foreach (char c in minTextBox.Text)
                    stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

                stillOk &= minTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;

                // Check if max value is a number
                foreach (char c in maxTextBox.Text)
                    stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

                stillOk &= maxTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;


                // Check if range is valid
                double min;
                double max;

                Double.TryParse(minTextBox.Text, out min);
                Double.TryParse(maxTextBox.Text, out max);

                stillOk &= min <= max;

                okButton.Enabled = stillOk;
            };

            DialogResult result = form.ShowDialog();
            return new Tuple<DialogResult, string, string>(result, minTextBox.Text, maxTextBox.Text);
        }

        private Tuple<DialogResult, string> ChangeStepSizeDialog()
        {
            Form form = new Form();
            Label pleaseEnterLabel = new Label();
            TextBox stepSizeTextBox = new TextBox();
            Label errorLabel = new Label();
            Button okButton = new Button();
            Button cancelButton = new Button();

            // 
            // pleaseEnterLabel
            // 
            pleaseEnterLabel.Location = new Point(9, 9);
            pleaseEnterLabel.Name = "pleaseEnterLabel";
            pleaseEnterLabel.Size = new Size(200, 13);
            pleaseEnterLabel.TabIndex = 0;
            pleaseEnterLabel.Text = DESCRIPTION_CHANGE_STEP_SIZE;
            // 
            // featureNameTextBox
            // 
            stepSizeTextBox.Location = new Point(12, 25);
            stepSizeTextBox.Name = "stepSizeTextBox";
            stepSizeTextBox.Size = new Size(197, 20);
            stepSizeTextBox.TabIndex = 1;
            // 
            // errorLabel
            // 
            errorLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            errorLabel.ForeColor = Color.Red;
            errorLabel.Location = new Point(12, 58);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(0, 13);
            errorLabel.TabIndex = 2;
            errorLabel.Visible = false;
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(12, 85);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 3;
            okButton.Text = "Ok";
            okButton.Enabled = false;
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(134, 85);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            //
            // form
            //
            form.AutoScaleDimensions = new SizeF(6F, 13F);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.ClientSize = new Size(215, 114);
            form.Controls.Add(cancelButton);
            form.Controls.Add(okButton);
            form.Controls.Add(errorLabel);
            form.Controls.Add(stepSizeTextBox);
            form.Controls.Add(pleaseEnterLabel);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = okButton;
            form.CancelButton = cancelButton;

            stepSizeTextBox.TextChanged += (s, e) =>
            {
                bool everythingCorrect = false;

                try
                {
                    new InfluenceFunction(stepSizeTextBox.Text);
                    everythingCorrect = true;
                }
                catch{}

                okButton.Enabled = everythingCorrect;
                errorLabel.Visible = !everythingCorrect;
            };

            DialogResult result = form.ShowDialog();
            return new Tuple<DialogResult, string>(result, stepSizeTextBox.Text);
        }

        /// <summary>
        /// This class is needed to deactivate the double click of the checked list box
        /// due to a bug (Checked state inconsistent)
        /// </summary>
        private class NoClickListBox : CheckedListBox
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