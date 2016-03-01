using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPLConqueror_Core;

namespace VariabilitModel_GUI
{
    public partial class NewFeatureDialog : Form
    {
        private const string TITLE = "Creating new Feature";
        private const string ERROR_NAME_EMPTY = "Please enter a feature name!";
        private const string ERROR_NAME_EXISTS = "This feature name already exists!";
        private const string ERROR_INVALID_NAME = "The entered feature name contains invalid symbols!";
        private const string ERROR_INVALID_RANGE = "The entered ranges of the values is invalid!";
        private const string ERROR_INVALID_STEP_FUNCTION = "The entered step function is invalid!";

        public NewFeatureDialog(string parentFeature)
        {
            InitializeComponent();
            
            this.Text = TITLE;

            optionalCheckBox.Enabled = false;
            addOptionButton.Enabled = false;
            
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();

            for (int i = 0; i < options.Count; i++)
                this.parentComboBox.Items.Add(options[i].Name);

            int selIndex = 0;

            if (parentFeature == null || parentFeature == "root")
                selIndex = parentComboBox.Items.IndexOf("root");
            else
                selIndex = parentComboBox.Items.IndexOf(parentFeature);

            parentComboBox.SelectedIndex = selIndex;

            numericRadioButton_CheckedChanged(null, null);
            prePostCheckBox_CheckedChanged(null, null);

            errorLabel.Visible = true;
            errorLabel.Text = ERROR_NAME_EMPTY;
        }

        /// <summary>
        /// Invokes if the 'Cancel'-button was pressed.
        /// 
        /// This method will dispose this form.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Invokes if the text of the corresponding textbox has changed.
        /// 
        /// This method will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void featureNameTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the check state of the corresponding radio button has changed.
        /// 
        /// This method will update the corresponding ui elements and will check
        /// of the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void numericRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            numericSettingsGroupBox.Enabled = numericRadioButton.Checked;
            optionalCheckBox.Enabled = binaryRadioButton.Checked;

            stepSizeCheckBox_CheckedChanged(null, null);

            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the text of the corresponding text box has changed.
        /// 
        /// This method will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void minValueTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the text of the corresponding text box has changed.
        /// 
        /// This method will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void maxValueTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the check state of the corresponding check box has changed.
        /// 
        /// This method will update all ui elements of the step size options and
        /// will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void stepSizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            stepSizeLabel.Enabled = stepSizeCheckBox.Checked;
            stepSizeTextBox.Enabled = stepSizeCheckBox.Checked;
            stepSizeExampleLabel.Enabled = stepSizeCheckBox.Checked;

            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the text of the corresponding check box has changed.
        /// 
        /// This method will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void stepSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            checkValidityOfData();
        }

        /// <summary>
        /// Invokes if the check state of the check box for prefixes and postfixes has changed.
        /// 
        /// This method will update all ui elements of the prefix and postfix options and
        /// will check if the current data is valid.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void prePostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            prefixLabel.Enabled = prePostCheckBox.Checked;
            prefixTextBox.Enabled = prePostCheckBox.Checked;
            postfixLabel.Enabled = prePostCheckBox.Checked;
            postfixTextBox.Enabled = prePostCheckBox.Checked;
        }

        /// <summary>
        /// Checks if the current data is valid.
        /// 
        /// The data is valid if
        /// - the feature name is neither empty, already used or contains invalid characters
        /// - the range of values contains valid numbers and form a valid range and
        /// - the step function is in the right form
        /// 
        /// If the data is not valid, the option cannot be added and an error message will be
        /// displayed.
        /// </summary>
        private void checkValidityOfData()
        {
            // Check if feature name is empty
            if (featureNameTextBox.Text == "")
            {
                errorLabel.Text = ERROR_NAME_EMPTY;
                errorLabel.Visible = true;
                addOptionButton.Enabled = false;
                return;
            }

            // Check if feature name already exists
            if (GlobalState.varModel.getOption(featureNameTextBox.Text) != null)
            {
                errorLabel.Text = ERROR_NAME_EXISTS;
                errorLabel.Visible = true;
                addOptionButton.Enabled = false;
                return;
            }

            // Check if feature name is invalid
            if (featureNameTextBox.Text != ConfigurationOption.removeInvalidCharsFromName(featureNameTextBox.Text))
            {
                errorLabel.Text = ERROR_INVALID_NAME;
                errorLabel.Visible = true;
                addOptionButton.Enabled = false;
                return;
            }

            if (numericRadioButton.Checked)
            {
                // Check if the range of values is invalid
                if (!isRangeValid())
                {
                    errorLabel.Text = ERROR_INVALID_RANGE;
                    errorLabel.Visible = true;
                    addOptionButton.Enabled = false;
                    return;
                }

                // Check if step function is valid
                if (stepSizeCheckBox.Checked && !isStepSizeValid())
                {
                    errorLabel.Text = ERROR_INVALID_STEP_FUNCTION;
                    errorLabel.Visible = true;
                    addOptionButton.Enabled = false;
                    return;
                }
            }

            errorLabel.Visible = false;
            addOptionButton.Enabled = true;
        }

        /// <summary>
        /// Checks if the current range of values is valid.
        /// 
        /// This is the case if the corresponding text boxes contain valid numbers
        /// and if the max value is bigger than the min value or the max value equals
        /// the min value.
        /// </summary>
        /// <returns>true, if the range of values is valid, else false</returns>
        private bool isRangeValid()
        {
            bool stillOk = true;

            // Check if min value is a number
            foreach (char c in minValueTextBox.Text)
                stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

            stillOk &= minValueTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;

            // Check if max value is a number
            foreach (char c in maxValueTextBox.Text)
                stillOk &= Char.IsNumber(c) || c.Equals('-') || c.Equals('.');

            stillOk &= maxValueTextBox.Text.Replace("-", "").Replace(".", "").Length > 0;

            if (!stillOk)
                return stillOk;

            // Check if range is valid
            double min, max;

            Double.TryParse(minValueTextBox.Text, out min);
            Double.TryParse(maxValueTextBox.Text, out max);

            stillOk &= min <= max;

            return stillOk;
        }

        /// <summary>
        /// Checks if the current step function is valid.
        /// </summary>
        /// <returns>true, if the step function is valid, else false</returns>
        private bool isStepSizeValid()
        {
            try
            {
                new InfluenceFunction(stepSizeTextBox.Text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Invokes if the 'Add option'-button was pressed.
        /// 
        /// This method will add the option the the current variability model and
        /// will dispose this form.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void addOptionButton_Click(object sender, EventArgs e)
        {
            ConfigurationOption newOption = null;

            if (numericRadioButton.Checked)
            {
                newOption = new NumericOption(GlobalState.varModel, this.featureNameTextBox.Text);
                ((NumericOption)newOption).Min_value = Convert.ToDouble(minValueTextBox.Text);
                ((NumericOption)newOption).Max_value = Convert.ToDouble(maxValueTextBox.Text);

                if (stepSizeCheckBox.Checked)
                    ((NumericOption)newOption).StepFunction = new InfluenceFunction(
                        stepSizeTextBox.Text == "" ? "n + 1": stepSizeTextBox.Text, (NumericOption)newOption);
                else
                    ((NumericOption)newOption).StepFunction = new InfluenceFunction("n + 1", (NumericOption)newOption);

            }
            else
            {
                newOption = new BinaryOption(GlobalState.varModel, this.featureNameTextBox.Text);
                ((BinaryOption)newOption).Optional = optionalCheckBox.Checked;
            }

            if (prePostCheckBox.Checked)
            {
                newOption.Prefix = prefixTextBox.Text;
                newOption.Postfix = postfixTextBox.Text;
            }

            newOption.OutputString = outputStringTextBox.Text;
            newOption.Parent = GlobalState.varModel.getOption(this.parentComboBox.Text);

            newOption.Parent.Children.Add(newOption);

            this.Dispose();
            GlobalState.varModel.addConfigurationOption(newOption);
        }
    }
}
