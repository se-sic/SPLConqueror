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
        public VariabilityModel_Form()
        {
            InitializeComponent();
        }

        private void buttonSavePLM_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                String folder = fbd.SelectedPath;
                GlobalState.varModel.Path = folder + Path.DirectorySeparatorChar + GlobalState.varModel.Name + ".xml";
                GlobalState.varModel.saveXML();
            }
        }

        protected void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode tn = e.Node;
                ConfigurationOption f = (ConfigurationOption)tn.Tag;
                String featureName = (f == null ? null : f.Name);

                NewFeatureDialog dlg = new NewFeatureDialog(featureName);
                dlg.ShowDialog();

                InitTreeView();
            }
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            //this.toolTipElement = (Element)e.Node.Tag;
            //if (toolTipElement == null)
            //    return;



            //toolTip1.ShowAlways = true;
            //toolTip1.ReshowDelay = 50;
            //toolTip1.InitialDelay = 250;

            //toolTip1.ToolTipTitle = toolTipElement.getName();
            //Point p = treeView1.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));
            //if (toolTipElement.description == null)
            //    toolTip1.Show("No description", this, p.X + 25, p.Y + 50, 2000);
            //else
            //    toolTip1.Show(toolTipElement.description, this, p.X + 25, p.Y + 50, 2000);
        }

        protected void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //this.lbStatus.Text = "not validated";
            //this.lbStatus.ForeColor = Color.Red;
            //if (e.Node.Parent == null)
            //{
            //    if (e.Node.Checked == false)
            //        e.Node.Checked = true;
            //    return;
            //}

            ////es sollten nur erreichbare stellen enabled sein
            ////String element_Name = e.Node.Text;
            ////if (element_Name.Contains("("))
            ////{
            ////    element_Name = element_Name.Substring(0, element_Name.IndexOf("(") - 1);
            ////}
            //Element tempele = (Element)e.Node.Tag;//fm.getElement(element_Name);

            ////Check for alternatives
            //if (e.Node.Checked == true)
            //{
            //    TreeNode t;
            //    for (int i = 0; i < tempele.getAlternatives().Count; i++)
            //    {
            //        if (e.Node.NextNode != null)
            //        {
            //            t = e.Node.NextNode;
            //            while (t != null)
            //            {
            //                if (t.Text == tempele.getAlternatives()[i].getName() && t.Checked == true)
            //                {
            //                    /* Code ausgergraut für ReplaceFeatureMethode
            //                    //Alternative node is already selected
            //                    e.Node.Checked = false;
            //                    MessageBox.Show("An alternative element is already selected!");
            //                    */

            //                    t.Checked = false;
            //                    if (t.Nodes != null)
            //                        for (int n = 0; n < t.Nodes.Count; n++)
            //                        {
            //                            if (t.Nodes[n].Checked)
            //                            {
            //                                t.Nodes[n].Checked = false;
            //                                if (FeatModList != null) FeatModList.RemoveFeature(fm.getElementByNameUnsafe(t.Nodes[n].Text));
            //                            }
            //                        }

            //                    if (FeatModList != null) FeatModList.ReplaceFeature(fm.getElementByNameUnsafe(t.Text), tempele);

            //                    return;
            //                }
            //                t = t.NextNode;
            //            }

            //        }
            //        if (e.Node.PrevNode != null)
            //        {
            //            t = e.Node.PrevNode;
            //            while (t != null)
            //            {
            //                if (t.Text == tempele.getAlternatives()[i].getName() && t.Checked == true)
            //                {
            //                    /* Code ausgergraut für ReplaceFeatureMethode
            //                    //Alternative node is already selected
            //                    e.Node.Checked = false;
            //                    MessageBox.Show("An alternative element is already selected!");
            //                    */

            //                    t.Checked = false;
            //                    if (t.Nodes != null)
            //                        for (int n = 0; n < t.Nodes.Count; n++)
            //                        {
            //                            if (t.Nodes[n].Checked)
            //                            {
            //                                t.Nodes[n].Checked = false;
            //                                if (FeatModList != null) FeatModList.RemoveFeature(fm.getElementByNameUnsafe(t.Nodes[n].Text));
            //                            }
            //                        }
            //                    if (FeatModList != null) FeatModList.ReplaceFeature(fm.getElementByNameUnsafe(t.Text), tempele);

            //                    return;
            //                }
            //                t = t.PrevNode;
            //            }
            //        }
            //    }
            //}

            ////Check for excludes
            //for (int i = 0; i < tempele.getExcludes().Count; i++)
            //{
            //    if (checkExcludeNode(fm.getElementById(tempele.getExcludes()[i]).getName()) == true)
            //    {
            //        //An excluded element is already selected
            //        e.Node.Checked = false;
            //        MessageBox.Show("The excluded element (" + fm.getElementById(tempele.getExcludes()[i]).getName() + ") is already selected!");
            //        return;
            //    }
            //}

            //button16.Enabled = false;


            //if (e.Node.Parent != null && e.Node.Checked)
            //{
            //    if (e.Node.Parent.Checked == false)
            //    {
            //        Element parent = (Element)e.Node.Parent.Tag;// fm.getElement(e.Node.Parent.Text);
            //        e.Node.Parent.Checked = true;
            //        //Listener Parent aktiviert 
            //        if (FeatModList != null) FeatModList.AddFeature(parent);
            //    }
            //}

            ////Listener Featurestatus geaendert
            ////element_Name = e.Node.Text;
            ////if (element_Name.Contains("("))
            ////{
            ////    element_Name = element_Name.Substring(0, element_Name.IndexOf("(") - 1);
            ////}
            //Element current = (Element)e.Node.Tag;//fm.getElement(element_Name);
            //if (e.Node.Checked)
            //{
            //    if (FeatModList != null) FeatModList.AddFeature(current);
            //}
            //else
            //{
            //    if (e.Node.Nodes != null)
            //        for (int n = 0; n < e.Node.Nodes.Count; n++)
            //        {
            //            if (e.Node.Nodes[n].Checked)
            //            {
            //                e.Node.Nodes[n].Checked = false;
            //                if (FeatModList != null)
            //                {
            //                    Element tempE = (Element)e.Node.Nodes[n].Tag;
            //                    FeatModList.RemoveFeature(tempE);
            //                }
            //            }
            //        }
            //    if (FeatModList != null) FeatModList.RemoveFeature(current);
            //}
        }


        private void loadModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new FileInfo(pfd.FileName);
                GlobalState.varModel = VariabilityModel.loadFromXML(fi.FullName);
                InitTreeView();
            }

        }




        public void InitTreeView()
        {
            this.treeView1.Nodes.Clear();
            this.treeView1.CheckBoxes = true;

            TreeNode root = new TreeNode(GlobalState.varModel.Name);
            List<ConfigurationOption> options =  GlobalState.varModel.getOptions();
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
            this.treeView1.Nodes.Add(root);
            this.treeView1.ExpandAll();

            //InitCheckedElements(root);
        }

        protected void insertSubElements(ConfigurationOption element, TreeNode t, bool bParentChecked)
        {
            bool bChecked = false;

            t.Tag = element;
            if (element is SPLConqueror_Core.NumericOption)
                t.ForeColor = Color.Red;
            //else
            //{
            //    if (element.getCommulatives().Count > 0)
            //        t.ForeColor = Color.LightBlue;
            //    else
            //    {
            //        if (element.isOptional())
            //            t.ForeColor = Color.Green;
            //        //check optional childs if parent already checked
            //        else if (bParentChecked)
            //        {
            //            bChecked = true;
            //            t.Checked = true;
            //        }
            //    }
            //}

            //rekursiv die unterelemente einfügen
            element.updateChildren();
            foreach (ConfigurationOption elem in element.Children)
            {

                TreeNode tn = new TreeNode(elem.Name);
                insertSubElements(elem, tn, bChecked);

                t.Nodes.Add(tn);
                
            }
        }

        private void newModel_Click(object sender, EventArgs e)
        {
            if (this.modelNameBox.Text.Length == 0)
            {
                return;
            }
            GlobalState.varModel = new VariabilityModel(this.modelNameBox.Text);
            InitTreeView();
        }

        private void buttonEditFeatures_Click(object sender, EventArgs e)
        {
            if (GlobalState.varModel == null)
                return;
            EditOptionDialog featureEditDlg = new EditOptionDialog(this);
            featureEditDlg.ShowDialog();
        }

      
    }
}
