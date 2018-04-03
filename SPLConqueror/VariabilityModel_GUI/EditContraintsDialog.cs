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
    public partial class EditContraintsDialog : Form
    {
        private List<string> boolConstraintList = new List<string>();
        private List<string> nbConstraintList = new List<string>();

        public EditContraintsDialog()
        {
            InitializeComponent();

            initializeComponents();
        }

        /// <summary>
        /// Initializes all components.
        /// </summary>
        private void initializeComponents()
        {
            // Inserting all possible options into the right combo box
            foreach (ConfigurationOption option in GlobalState.varModel.getOptions())
            {
                boolOptionComboBox.Items.Add(option);
                mixedComboBox.Items.Add(option);

                if (option is NumericOption)
                    nbOptionComboBox.Items.Add(option);
            }

            mixedEvaluationComboBox.Items.Add("positive");
            mixedEvaluationComboBox.Items.Add("negative");
            mixedEvaluationComboBox.SelectedIndex = 0;

            if (boolOptionComboBox.Items.Count > 0)
                boolOptionComboBox.SelectedIndex = 0;

            if (nbOptionComboBox.Items.Count > 0)
                nbOptionComboBox.SelectedIndex = 0;

            // Inserting all existing constraints into the right list box
            foreach (string boolConstraint in GlobalState.varModel.BinaryConstraints)
                boolConstraintListBox.Items.Add(boolConstraint);

            foreach (NonBooleanConstraint nbConstraint in GlobalState.varModel.NonBooleanConstraints)
                nbConstraintListBox.Items.Add(nbConstraint.ToString());

            foreach (MixedConstraint mixedConstr in GlobalState.varModel.MixedConstraints)
                mixedListBox.Items.Add(mixedConstr.ToString());

            updateBoolConstraintBox();
            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the 'Add'-button for options in the boolean constraint area
        /// has been pressed.
        /// 
        /// This method will add the selected option to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolAddOptionButton_Click(object sender, EventArgs e)
        {
            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += boolOptionComboBox.SelectedItem.ToString();
            else
                boolConstraintList.Add(boolOptionComboBox.SelectedItem.ToString());

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the AND-operation has been pressed.
        /// 
        /// This method will add a '&' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolAndButton_Click(object sender, EventArgs e)
        {
            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += "&";
            else
                boolConstraintList.Add("&");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the OR-operation has been pressed.
        /// 
        /// This method will add a '|' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolOrButton_Click(object sender, EventArgs e)
        {
            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList[boolConstraintList.Count - 1] += "|";
            else
                boolConstraintList.Add("|");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the IMPLICATES-operation has been pressed.
        /// 
        /// This method will add a '=>' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolImplButton_Click(object sender, EventArgs e)
        {
            boolConstraintList.Add("=>");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the NEGATE-operation has been pressed.
        /// 
        /// This method will add a '!' to the current boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolNegButton_Click(object sender, EventArgs e)
        {
            if (boolConstraintList.Count > 0 && boolConstraintList[boolConstraintList.Count - 1] == "!")
                boolConstraintList.RemoveAt(boolConstraintList.Count - 1);
            else
                boolConstraintList.Add("!");

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding the current boolean constraint
        /// has been pressed.
        /// 
        /// This method will add the current boolean constraint to the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolAddConstraintButton_Click(object sender, EventArgs e)
        {
            boolConstraintListBox.Items.Add(String.Join(" ", boolConstraintList));
            GlobalState.varModel.BinaryConstraints.Add(String.Join(" ", boolConstraintList));

            boolConstraintList.Clear();

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for removing a part of the current boolean
        /// constraint has been pressed.
        /// 
        /// This method will remove the last part of the boolean constraint
        /// that was added before.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolRemoveButton_Click(object sender, EventArgs e)
        {
            boolConstraintList.RemoveAt(boolConstraintList.Count - 1);

            updateBoolConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for removing a part of the current boolean
        /// constraint has been pressed.
        /// 
        /// This method will remove the selected boolean constraint from the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void boolDeleteConstraintButton_Click(object sender, EventArgs e)
        {
            int selIndex = boolConstraintListBox.SelectedIndex;

            if (selIndex > -1)
            {
                GlobalState.varModel.BinaryConstraints.RemoveAt(selIndex);
                boolConstraintListBox.Items.RemoveAt(selIndex);

                updateBoolConstraintBox();
            }
        }

        /// <summary>
        /// Updates the UI components of the boolean constraint group box.
        /// </summary>
        private void updateBoolConstraintBox()
        {
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
                boolAddOptionButton.Enabled = boolOptionComboBox.Items.Count > 0;

                boolAndButton.Enabled = false;
                boolOrButton.Enabled = false;
                boolImplButton.Enabled = false;
            }

            boolAddConstraintButton.Enabled = boolConstraintList.Count >= 1
                && (boolConstraintList.Count - offset) % 2 == 1
                && boolConstraintList[boolConstraintList.Count - 1] != "!";
            boolRemoveButton.Enabled = boolConstraintTextBox.Text.Length > 0;
            boolDeleteConstraintButton.Enabled = boolConstraintListBox.Items.Count > 0;
        }

        /// <summary>
        /// Invokes if the 'Add'-button for options in the non-boolean constraint area
        /// has been pressed.
        /// 
        /// This method will add the selected option to the current non-boolean
        /// constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbAddOptionButton_Click(object sender, EventArgs e)
        {
            if (nbConstraintList.Count > 0 && nbConstraintList[nbConstraintList.Count - 1] == "-")
                nbConstraintList[nbConstraintList.Count - 1] += nbOptionComboBox.SelectedItem.ToString();
            else
                nbConstraintList.Add(nbOptionComboBox.SelectedItem.ToString());

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '0'-button has been pressed.
        /// 
        /// This method will add a '0' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb0Button_Click(object sender, EventArgs e)
        {
            addNumber(0);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '1'-button has been pressed.
        /// 
        /// This method will add a '1' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb1Button_Click(object sender, EventArgs e)
        {
            addNumber(1);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '2'-button has been pressed.
        /// 
        /// This method will add a '2' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb2Button_Click(object sender, EventArgs e)
        {
            addNumber(2);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '3'-button has been pressed.
        /// 
        /// This method will add a '3' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb3Button_Click(object sender, EventArgs e)
        {
            addNumber(3);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '4'-button has been pressed.
        /// 
        /// This method will add a '4' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb4Button_Click(object sender, EventArgs e)
        {
            addNumber(4);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '5'-button has been pressed.
        /// 
        /// This method will add a '5' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb5Button_Click(object sender, EventArgs e)
        {
            addNumber(5);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '6'-button has been pressed.
        /// 
        /// This method will add a '6' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb6Button_Click(object sender, EventArgs e)
        {
            addNumber(6);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '7'-button has been pressed.
        /// 
        /// This method will add a '7' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb7Button_Click(object sender, EventArgs e)
        {
            addNumber(7);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '8'-button has been pressed.
        /// 
        /// This method will add a '8' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb8Button_Click(object sender, EventArgs e)
        {
            addNumber(8);

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '9'-button has been pressed.
        /// 
        /// This method will add a '9' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nb9Button_Click(object sender, EventArgs e)
        {
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
        /// Invokes if the '+'-button has been pressed.
        /// 
        /// This method will add a '+' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbPlusButton_Click(object sender, EventArgs e)
        {
            nbConstraintList.Add("+");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '-'-button has been pressed.
        /// 
        /// This method will add a '-' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbDiffButton_Click(object sender, EventArgs e)
        {
            if (nbConstraintList.Count > 0 && nbConstraintList.Count % 2 == 1
                && nbConstraintList[nbConstraintList.Count - 1] == "-")
                nbConstraintList.RemoveAt(nbConstraintList.Count - 1);
            else
                nbConstraintList.Add("-");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '*'-button has been pressed.
        /// 
        /// This method will add a '*' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbTimesButton_Click(object sender, EventArgs e)
        {
            nbConstraintList.Add("*");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the '.'-button has been pressed.
        /// 
        /// This method will add a '.' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbPointButton_Click(object sender, EventArgs e)
        {
            nbConstraintList[nbConstraintList.Count - 1] += ".";

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the Equal-separator has been pressed.
        /// 
        /// This method will add a '=' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbEqButton_Click(object sender, EventArgs e)
        {
            nbConstraintList.Add("=");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the Greater-Equal-separator has been pressed.
        /// 
        /// This method will add a '>=' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbGreEqButton_Click(object sender, EventArgs e)
        {
            nbConstraintList.Add(">=");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for the Greater-separator has been pressed.
        /// 
        /// This method will add a '>' to the current non-boolean constraint.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbGreButton_Click(object sender, EventArgs e)
        {
            nbConstraintList.Add(">");

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for adding the current non-boolean constraint
        /// has been pressed.
        /// 
        /// This method will add the current non-boolean constraint to the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbAddConstraintButton_Click(object sender, EventArgs e)
        {
            nbConstraintListBox.Items.Add(nbConstraintTextBox.Text);
            GlobalState.varModel.NonBooleanConstraints.Add(
                new NonBooleanConstraint(nbConstraintTextBox.Text, GlobalState.varModel));

            nbConstraintList.Clear();

            updateNbConstraintBox();
        }

        /// <summary>
        /// Invokes if the button for removing a part of the current non-boolean
        /// constraint has been pressed.
        /// 
        /// This method will remove the last part of the non-boolean constraint
        /// that was added before.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbRemoveButton_Click(object sender, EventArgs e)
        {
            double d;

            if (Double.TryParse(nbConstraintList[nbConstraintList.Count - 1], out d))
            {
                if (nbConstraintList[nbConstraintList.Count - 1].Length > 1) {
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
        /// Invokes if the button for removing a part of the current non-boolean
        /// constraint has been pressed.
        /// 
        /// This method will remove the selected non-boolean constraint from the
        /// variability model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void nbDeleteConstraintButton_Click(object sender, EventArgs e)
        {
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
        /// Updates the UI components of the non-boolean constraint group box.
        /// </summary>
        private void updateNbConstraintBox()
        {
            nbConstraintTextBox.Text = String.Join(" ", nbConstraintList);

            // Enabling the buttons depending on the current state
            int offset = nbConstraintList.Count > 0
                && nbConstraintList[nbConstraintList.Count - 1] == "-" ? 1 : 0;

            if (nbConstraintList.Count % 2 == 0)
            {
                nbAddOptionButton.Enabled = true;

                setNumericalButtons(true);

                nbPlusButton.Enabled = false;
                nbDiffButton.Enabled = true;
                nbTimesButton.Enabled = false;
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
                    nbDiffButton.Enabled = true;
                    nbTimesButton.Enabled = false;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(false);
                }
                else if (nbConstraintList[nbConstraintList.Count - 1].EndsWith("."))
                {
                    // There is currently a number that gets decimal digits
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(true);

                    nbPlusButton.Enabled = false;
                    nbDiffButton.Enabled = false;
                    nbTimesButton.Enabled = false;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(false);
                }
                else if (Double.TryParse(nbConstraintList[nbConstraintList.Count - 1], out d))
                {
                    // The last element is a (decimal) number
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(true);

                    nbPlusButton.Enabled = true;
                    nbDiffButton.Enabled = true;
                    nbTimesButton.Enabled = true;
                    nbPointButton.Enabled = !nbConstraintList[nbConstraintList.Count - 1].Contains(".");

                    setSeparatorButtons(!nbConstraintList.Any(x => isSeparator(x)));
                }
                else
                {
                    // Only operators and/or separators are allowed
                    nbAddOptionButton.Enabled = false;

                    setNumericalButtons(false);

                    nbPlusButton.Enabled = true;
                    nbDiffButton.Enabled = true;
                    nbTimesButton.Enabled = true;
                    nbPointButton.Enabled = false;

                    setSeparatorButtons(!nbConstraintList.Any(x => isSeparator(x)));
                }
            }
            
            nbAddConstraintButton.Enabled = nbConstraintList.Any(isSeparator)
                && (nbConstraintList.Count - offset) % 2 == 1
                && !nbConstraintList[nbConstraintList.Count - 1].EndsWith(".")
                && nbConstraintList[nbConstraintList.Count - 1] != "-";
            nbRemoveButton.Enabled = nbConstraintTextBox.Text.Length > 0;
            nbDeleteConstraintButton.Enabled = nbConstraintListBox.Items.Count > 0;
        }

        private void zeroBtnMixed_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "0";
        }

        private void mixed1Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "1";
        }

        private void mixed2Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "2";
        }

        private void mixed3Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "3";
        }

        private void mixed4Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "4";
        }

        private void mixed5Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "5";
        }

        private void mixed6Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "6";
        }

        private void mixed7Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "7";
        }

        private void mixed8Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "8";
        }

        private void mixed9Btn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + "9";
        }

        private void mixedPntBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + ".";
        }

        private void mixedPlusBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " + ";
        }

        private void mixedMinBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " - ";
        }

        private void mixedMulBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " * ";
        }

        private void mixedGtBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " >= ";
        }

        private void mixedLessBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " > ";
        }

        private void mixedEqBtn_Click(object sender, EventArgs e)
        {
            mixedConstrTextBox.Text = mixedConstrTextBox.Text + " = ";
        }

        private void mixedAddOptionBtn_Click(object sender, EventArgs e)
        {
            if (mixedComboBox.SelectedItem != null)
            {
                mixedConstrTextBox.Text = mixedConstrTextBox.Text + " " + mixedComboBox.SelectedItem.ToString();
            } else
            {
                MessageBox.Show("No configuration option selected");
            }
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.button1, "In configurations where not all options of the constraint are present missing options will be assumed as deselected.");
            ToolTip1.InitialDelay = 0;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip2.SetToolTip(this.button2, "Configurations where not all options of the constraint are present automatically result in true.");
            ToolTip2.InitialDelay = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String[] leftAndRight = mixedConstrTextBox.Text.Split(new char[] { ':' });

            if (leftAndRight.Length == 1)
            {
                mixedConstrTextBox.Text = "None:" + leftAndRight[0];
            } else
            {
                mixedConstrTextBox.Text = "None:" + leftAndRight[1];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String[] leftAndRight = mixedConstrTextBox.Text.Split(new char[] { ':' });

            if (leftAndRight.Length == 1)
            {
                mixedConstrTextBox.Text = "All:" + leftAndRight[0];
            }
            else
            {
                mixedConstrTextBox.Text = "All:" + leftAndRight[1];
            }
        }

        private void addMixed_Click(object sender, EventArgs e)
        {
            String[] leftAndRight = mixedConstrTextBox.Text.Split(new char[] { ':' });
            if (leftAndRight.Length == 1)
            {
                MessageBox.Show("Warning: Either \"None\" or \"All\" prefix needs to be selected. Automatically assuming the \"None\" prefix.");
                leftAndRight = new String[] { "None", mixedConstrTextBox.Text };
            }
            MixedConstraint constr = null;
            if ((string)mixedEvaluationComboBox.SelectedItem == "positive")
                constr = new MixedConstraint(leftAndRight[1], GlobalState.varModel, leftAndRight[0]);
            else
                constr = new MixedConstraint(leftAndRight[1], GlobalState.varModel, leftAndRight[0], "neg");
            GlobalState.varModel.MixedConstraints.Add(constr);

            if ((string)mixedEvaluationComboBox.SelectedItem == "positive")
                mixedListBox.Items.Add(mixedConstrTextBox.Text);
            else
                mixedListBox.Items.Add("!:" + mixedConstrTextBox.Text);
            mixedConstrTextBox.Text = "None:";
        }

        private void removeMixed_Click(object sender, EventArgs e)
        {
            if (mixedListBox.Items.Count == 0)
            {
                MessageBox.Show("No item to remove.");
            } else
            {
                mixedListBox.Items.RemoveAt(mixedListBox.Items.Count - 1);
                GlobalState.varModel.MixedConstraints.RemoveAt(GlobalState.varModel.MixedConstraints.Count - 1);
            }
        }

        private void mixedDelete_Click(object sender, EventArgs e)
        {
            int selIndex = mixedListBox.SelectedIndex;
            if (selIndex == -1)
            {
                MessageBox.Show("No constraint selected.");
                return;
            }
            mixedListBox.Items.RemoveAt(selIndex);
            GlobalState.varModel.MixedConstraints.RemoveAt(selIndex);
        }
    }
}
