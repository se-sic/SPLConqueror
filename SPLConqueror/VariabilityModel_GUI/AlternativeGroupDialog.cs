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
        private IDictionary<ConfigurationOption, string> currAltGroups = new Dictionary<ConfigurationOption, string>();

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

                selectedOptionAddButton.Enabled = !currAltGroups.ContainsKey(
                    (ConfigurationOption) selectedOptionComboBox.SelectedItem);
            } else
                selectedOptionAddButton.Enabled = false;
        }

        /// <summary>
        /// Initializes the form and its data.
        /// </summary>
        private void initializeForm()
        {
            List<ConfigurationOption> opts = new List<ConfigurationOption>();

            foreach (ConfigurationOption o in GlobalState.varModel.getOptions())
            {
                if (o.Children.Count > 1)
                    opts.Add(o);
            }

            selectedOptionComboBox.Items.AddRange(opts.ToArray());
            selectedOptionComboBox.SelectedIndex = 0;

            foreach (string c in GlobalState.varModel.BooleanConstraints)
            {
                ConfigurationOption opt = getAltGroup(c);

                if (opt != null && !currAltGroups.ContainsKey(opt))
                {
                    currAltGroups.Add(opt, c);
                    currAltGroupsListBox.Items.Add(opt);
                }
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
            selectedOptionAddButton.Enabled = !currAltGroups.ContainsKey(
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
            string altConstraint = createAltConstraint(selectedOption);
            List<ConfigurationOption> opts = selectedOption.Children;

            GlobalState.varModel.BooleanConstraints.Add(altConstraint);
            
            foreach (ConfigurationOption o1 in opts)
            {
                List<ConfigurationOption> list = new List<ConfigurationOption>();
                list.AddRange(opts);
                list.Remove(o1);

                o1.Excluded_Options.Add(list);
            }

            currAltGroups.Add(selectedOption, altConstraint);
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
                string altConstraint = currAltGroups[selectedOption];
                List<ConfigurationOption> opts = selectedOption.Children;

                GlobalState.varModel.BooleanConstraints.Remove(altConstraint);

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
        /// Creates a constraint that depicts the specified option as alternative group.
        /// </summary>
        /// <param name="opt">Specified option for alternative group</param>
        /// <returns>Constraint</returns>
        private string createAltConstraint(ConfigurationOption opt)
        {
            string constraint = opt.Name + " => ";

            for (int i = 0; i < opt.Children.Count; i++)
            {
                constraint += opt.Children[i].Name + " ";

                foreach (ConfigurationOption o in opt.Children)
                    if (opt.Children[i] != o)
                        constraint += "& !" + o.Name + " ";

                if (i < opt.Children.Count - 1)
                    constraint += "| ";
            }

            return constraint.TrimEnd();
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
