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

        List<string> currentEquation = new List<string>();

        VariabilityModel_Form parent = null;

        public EditOptionDialog(VariabilityModel_Form parentForm)
        {
            parent = parentForm;
            InitializeComponent();
            updateGUI();
        }

        private void updateGUI()
        {
            this.otherBox.Items.Clear();
            this.selectBox.Items.Clear();
            this.nbConstraintBox.Items.Clear();

            List<ConfigurationOption> options = GlobalState.varModel.getOptions();
            for (int i = 0; i < options.Count; i++)
            {
                this.selectBox.Items.Add(options[i]);
                if (options[i] is BinaryOption)
                {
                    this.otherBox.Items.Add(options[i]);
                }
                this.nbConstOptions.Items.Add(options[i]);
            }
            List<NonBooleanConstraint> nbConst = GlobalState.varModel.NonBooleanConstraints;
            for (int i = 0; i < nbConst.Count; i++)
            {
                this.nbConstraintBox.Items.Add(nbConst[i]);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (otherBox.Text != "")
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
            if (otherBox.Text != "")
            {
                this.excludeSingleListBox.Items.Add(otherBox.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (otherBox.Text != "")
            {
                this.requiresSingleListBox.Items.Add(otherBox.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (otherBox.Text != "")
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
            if (otherBox.Text != "")
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
            if (this.excludeSingleListBox.SelectedItem != null)
            {
                //this.exc.Add(this.excludeSingleListBox.SelectedItem.ToString());
                this.excludeSingleListBox.Items.Remove(this.excludeSingleListBox.SelectedItem);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.requiresSingleListBox.SelectedItem != null)
            {
                //this.req.Add(this.requiresSingleListBox.SelectedItem.ToString());
                this.requiresSingleListBox.Items.Remove(this.requiresSingleListBox.SelectedItem);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.otherBox.Items.Remove(this.selectBox.Text);
            
            BinaryOption temp = GlobalState.varModel.getBinaryOption(this.selectBox.Text);

            if (temp == null)
                return;
            
            if (temp.Optional)
                this.checkBox1.Checked = true;
            else
                this.checkBox1.Checked = false;
            //fill all boxes
            this.nbConstraintBox.Items.Clear();

            excludesOverview.Items.Clear();
            excludeSingleListBox.Items.Clear();
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
            requiresSingleListBox.Items.Clear();
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
            if (otherBox.Text != "")
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
            if (this.renameOption_TextBox.Text.Length > 0)
            {
                if (this.renameOption_TextBox.Text.Contains("-"))
                {
                    WarningLabel.Text = ("No '-' in option name!");
                    return;
                }
                GlobalState.varModel.getOption(this.selectBox.Text).Name = renameOption_TextBox.Text;
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
            ConfigurationOption toDelete = GlobalState.varModel.getOption(this.selectBox.Text);
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
            BinaryOption curr = GlobalState.varModel.getBinaryOption(selectBox.Text);
            curr.Excluded_Options.Add(newExcludeList);
        }

        private void finRequire_Click(object sender, EventArgs e)
        {
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
            BinaryOption curr = GlobalState.varModel.getBinaryOption(selectBox.Text);
            curr.Excluded_Options.Add(newRequireList);
        }

        private void excludesOverview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (excludesOverview.SelectedIndex == -1)
                return;

            excludeSingleListBox.Items.Clear();
            String currEx = excludesOverview.Items[excludesOverview.SelectedIndex].ToString();
            String[] currExParts = currEx.Split('|');
            for(int i = 0; i < currExParts.Length;i++)
            {
                excludeSingleListBox.Items.Add(currExParts[i].Trim());
            }
        }

        private void excludesOverviewDelButton_Click(object sender, EventArgs e)
        {
            if (excludesOverview.SelectedIndex != -1)
            {
                ConfigurationOption curr = GlobalState.varModel.getBinaryOption(selectBox.SelectedItem.ToString());
                curr.Excluded_Options.RemoveAt(excludesOverview.SelectedIndex);
            }
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void requiresOverviewDelButton_Click(object sender, EventArgs e)
        {
            if (requiresOverview.SelectedIndex != -1)
            {
                ConfigurationOption curr = GlobalState.varModel.getBinaryOption(selectBox.SelectedItem.ToString());
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
        }

       


       

        


    }
}