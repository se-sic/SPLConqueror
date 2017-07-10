using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VariabilitModel_GUI
{
    public partial class AlternativeGroupDialog : Form
    {
        private List<ConfigurationOption> currAltGroups = new List<ConfigurationOption>();

        /// <summary>
        ///  Constructor of this class.
        ///  
        /// Initializes all necessary information of this form. The combo box of
        /// the form will be set the specified option. If the specified option is
        /// null or not selectable, the combo box will start at the first element.
        /// </summary>
        /// <param name="opt">Initial selected option</param>
        public AlternativeGroupDialog(ConfigurationOption opt)
        {
            InitializeComponent();
            initializeForm();

            if (selectedOptionComboBox.Items.Count > 0)
            {
                if (opt != null && selectedOptionComboBox.Items.Contains(opt))
                    selectedOptionComboBox.SelectedItem = opt;
                else
                    selectedOptionComboBox.SelectedIndex = 0;

                selectedOptionAddButton.Enabled = !currAltGroups.Contains(
                    (ConfigurationOption) selectedOptionComboBox.SelectedItem);
            } else
                selectedOptionAddButton.Enabled = false;
        }

        /// <summary>
        /// Initializes the form and its data.
        /// Therefore, this method checks which configuration options are alternative groups or not.
        /// </summary>
        private void initializeForm()
        {
            // This list contains the options that are potential candidates as alternative groups
            List<ConfigurationOption> remainingOptions = new List<ConfigurationOption>();

            // If all children do exclude each other, the current option has to be an alternative option
            List<ConfigurationOption> innerOpts = GlobalState.varModel.getOptions().Where(o => o.Children.Count > 0 && o is BinaryOption).ToList();

            // Only inner nodes are potential alternative groups
            foreach (ConfigurationOption innerOpt in innerOpts)
            {
                List<ConfigurationOption> childOpts = innerOpt.Children;

                bool addToSelection = true;
                foreach (ConfigurationOption childOpt in childOpts)
                {
                    // This list contains the other options. If at least one feature misses one of the other options, the parent feature is
                    // not declared as alternative group.
                    List<ConfigurationOption> otherOpts = childOpts.Where(o => o != childOpt).ToList();
                    foreach (List<ConfigurationOption> excludedOptions in childOpt.Excluded_Options)
                    {
                        if (excludedOptions.Count == 1 && otherOpts.Contains(excludedOptions[0]))
                        {
                            otherOpts.Remove(excludedOptions[0]);
                        }
                    }

                    if (otherOpts.Count > 0)
                    {
                        addToSelection = false;
                        break;
                    }
                }

                if (addToSelection)
                {
                    currAltGroups.Add(innerOpt);
                    currAltGroupsListBox.Items.Add(innerOpt);
                } else
                {
                    remainingOptions.Add(innerOpt);
                }
            }

            // Add the remaining options to the combo-box
            selectedOptionComboBox.Items.AddRange(remainingOptions.ToArray());
            if (selectedOptionComboBox.Items.Count > 0) {
                selectedOptionComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Invokes if the selected index of the corresponding combo box changed.
        /// 
        /// This will change the status of the adding button. It will be enabled if the
        /// selected option is not an alternative group yet. If it is, the button will
        /// be disabled.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void selectedOptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedOptionAddButton.Enabled = !currAltGroups.Contains(
                (ConfigurationOption)selectedOptionComboBox.SelectedItem);
        }

        /// <summary>
        /// Invokes if the button for adding the selected option to the alternative groups
        /// was pressed.
        /// 
        /// This will add the selected option to the alternative groups of the current
        /// variability model by adding a new constraint and modifying the excluded options.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void selectedOptionAddButton_Click(object sender, EventArgs e)
        {
            ConfigurationOption selectedOption = (ConfigurationOption)selectedOptionComboBox.SelectedItem;
            List<ConfigurationOption> opts = selectedOption.Children;
            List<ConfigurationOption> temp = new List<ConfigurationOption>();
            foreach (ConfigurationOption opt in opts)
            {
                if (opt is BinaryOption)
                {
                    temp.Add(opt);
                }
            }
            foreach (ConfigurationOption thisOpt in opts)
            {
                foreach (ConfigurationOption otherOpt in opts)
                {
                    if (!thisOpt.Equals(otherOpt))
                    {
                        List<ConfigurationOption> newContstraint = new List<ConfigurationOption>() { otherOpt };
                        if (!ConfigurationOption.hasConstraint(thisOpt.Excluded_Options, newContstraint))
                        {
                            thisOpt.Excluded_Options.Add(new List<ConfigurationOption>() { otherOpt });
                        }

                    }
                }                
            }

            currAltGroups.Add(selectedOption);
            currAltGroupsListBox.Items.Add(selectedOption);
            selectedOptionAddButton.Enabled = false;
        }

        /// <summary>
        /// Invokes if the button for deleting a selected alternative group was pressed.
        /// 
        /// This will remove the selected option from the alternative groupsof the current
        /// variability model by removing the specific constraint and modifying the
        /// exclude options.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        private void currAltGroupsDeleteButton_Click(object sender, EventArgs e)
        {
            if (currAltGroupsListBox.SelectedIndex > -1)
            {
                ConfigurationOption selectedOption = (ConfigurationOption)currAltGroupsListBox.SelectedItem;
                List<ConfigurationOption> opts = selectedOption.Children;

                foreach (ConfigurationOption o1 in opts)
                {
                    List<ConfigurationOption> list = new List<ConfigurationOption>();
                    list.AddRange(opts);
                    list.Remove(o1);

                    o1.Excluded_Options.RemoveAll(l => l.Count == list.Count
                                && new HashSet<ConfigurationOption>(l).SetEquals(list));
                }

                currAltGroups.Remove(selectedOption);
                currAltGroupsListBox.Items.Remove(selectedOption);

                if (selectedOptionComboBox.SelectedItem == selectedOption)
                    selectedOptionAddButton.Enabled = true;
            }

        }

        /// <summary>
        /// Returns the option that is described by the specified constraint.
        /// If the constraint does not descibe an alternative group, the returned
        /// value will be null.
        /// </summary>
        /// <param name="constraint">Constraint with described alternative group</param>
        /// <returns>Option described by the specified constraint</returns>
        private ConfigurationOption getAltGroup(string constraint)
        {
            ConfigurationOption altCandidate = null;
            string[] array = constraint.Split(new string[] { "=>" }, StringSplitOptions.None);
            string[] combinations;
            
            // Get the candidate option for the alternative group
            switch(array.Length)
            {
                case 1:
                    combinations = constraint.Split('|');

                    foreach (string word in constraint.Split(' '))
                    {
                        string option = word.Contains("!") ? word.Replace("!", "") : word;

                        if (word == "&" || word == "|")
                            continue;

                        ConfigurationOption o = GlobalState.varModel.getOption(option);

                        if (altCandidate == null)
                            altCandidate = o.Parent;
                        else if (altCandidate != o.Parent)
                            return null;
                    }
                    break;
                case 2:
                    string[] split = array[0].TrimEnd().Split(' ');

                    if (split.Length > 1 || split[0].Contains('!'))
                        return null;

                    altCandidate = GlobalState.varModel.getOption(split[0]);

                    if (altCandidate == null)
                        return null;

                    combinations = array[1].TrimStart().Split('|');
                    break;
                default:
                    return null;
            }

            if (combinations.Length != altCandidate.Children.Count)
                return null;

            // Check if the constraint contains all necessary combinations
            foreach (ConfigurationOption o1 in altCandidate.Children)
            {
                bool combinationFound = false;

                for (int i = 0; i < combinations.Length && !combinationFound; i++)
                {
                    bool stillOk = true;

                    for (int j = 0; j < altCandidate.Children.Count && stillOk; j++)
                    {
                        ConfigurationOption o2 = altCandidate.Children[j];
                        string[] split = combinations[i].Split(' ');
                        bool optionFound = false;
                        
                        for (int k = 0; k < split.Length && !optionFound; k++)
                        {
                            if (o1 != o2)
                                optionFound = split[k] == "!" + o2.Name;
                            else
                                optionFound = split[k] == o2.Name;
                        }

                        stillOk = optionFound;
                    }

                    combinationFound = stillOk;
                }

                if (!combinationFound)
                    return null;
            }

            return altCandidate;
        }
    }
}
