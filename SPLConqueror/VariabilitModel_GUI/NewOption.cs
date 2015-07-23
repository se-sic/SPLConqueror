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
        String featurepath;
        String parentFeature;
        bool optional;
        string featureSourcePath;

        public NewFeatureDialog(string parentFeature)
        {
            InitializeComponent();
            //this.featurepath = "";
            this.parentFeature = parentFeature;
            //this.optional = false;
            //this.featureSourcePath = featureSourcePath;
            //this.vm = source;
            List<ConfigurationOption> options = GlobalState.varModel.getOptions();
            for (int i = 0; i < options.Count; i++)
            {
                String name = options[i].Name;
                this.parentBox.Items.Add(name);
                if (name.Equals(parentFeature))
                    this.parentBox.SelectedIndex = i;
            }
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //this.optional = this.checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (DialogResult.OK.Equals(this.folderBrowserDialog1.ShowDialog()))
            //{
            //    this.featurepath = folderBrowserDialog1.SelectedPath;
            //    this.textBox2.Text = folderBrowserDialog1.SelectedPath;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (this.textBox1.Text != "")
            //{
            //    if (this.textBox1.Text.Contains("-"))
            //    {
            //        src.HelperClass.printContent("No '-' in option name!");
            //        return;
            //    }
            //    if (this.textBox2.Text == "Standard" && this.comboBox1.Text == "")
            //        featurepath = this.featureSourcePath + "\\" + this.textBox1.Text;
            //    string varGen = "";
            //    if (this.textBox3.Text.Length > 0)
            //        varGen = this.textBox3.Text;
            //    Element newFeature = this.fm.addElement(this.textBox1.Text, this.comboBox1.Text, this.featurepath, this.checkBox1.Checked, true, "feature", varGen, this.textBox4.Text);
            //    newFeature.Passing_option = setPassingOption();
            //    this.Dispose();
            //}
            //else
            //{
            //    MessageBox.Show("Some values are missing!");
            //}
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void numericButton_CheckedChanged(object sender, EventArgs e)
        {
            if (numericButton.Checked)
            {
                workloadDefinition_numercialEquiDistant_group.Visible = true;
            }
            else
            {
                workloadDefinition_numercialEquiDistant_group.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.optionName.Text != "")
            {
                if (this.optionName.Text.Contains("-"))
                {
                    hintLabel.Text = ("No '-' in option name!");
                    return;
                }
                ConfigurationOption newOption = null;
                if (numericButton.Checked)
                {
                    newOption = new NumericOption(GlobalState.varModel, this.optionName.Text);
                    ((NumericOption)newOption).Min_value = Convert.ToDouble(minValue.Text);
                    ((NumericOption)newOption).Max_value = Convert.ToDouble(maxValue.Text);
                    ((NumericOption)newOption).StepFunction = new InfluenceFunction(stepSize.Text, (NumericOption)newOption);
                    ((NumericOption)newOption).Prefix = prefix.Text;
                    ((NumericOption)newOption).Postfix = suffix.Text;
                }
                else
                {
                    newOption = new BinaryOption(GlobalState.varModel, this.optionName.Text);
                }
                if (this.parentBox.Text.Length > 0)
                    newOption.Parent = GlobalState.varModel.getOption(this.parentBox.Text);

                if (!GlobalState.varModel.addConfigurationOption(newOption))
                {
                    hintLabel.Text = ("Option with the name already exists.");
                    return;  
                } 
                this.Dispose();
                GlobalState.varModel.addConfigurationOption(newOption);
            }
            else
            {
                hintLabel.Text = ("Some values are missing!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



    }
}
