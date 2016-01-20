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
        private const string REMOVE_WARNING = "Are you sure about removing this feature?\n"
            + "All children features will be deleted as well.";

        public VariabilityModel_Form()
        {
            InitializeComponent();

            this.Text = TITLE;
            this.saveModelToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Enabled = false;
        }

        protected void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode tn = treeView.GetNodeAt(treeView.PointToClient(new Point(contextMenuStrip.Left, contextMenuStrip.Top)));
                removeFeatureToolStripMenuItem.Enabled = tn != null && tn.Text != "root";

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
            // EXTENSION: Ask if there is any unsaved data
            Tuple<DialogResult, string> result = CreationDialog();

            if (result.Item1 == DialogResult.Cancel)
                return;

            GlobalState.varModel = new VariabilityModel(result.Item2);
            this.saveModelToolStripMenuItem.Enabled = true;
            this.editToolStripMenuItem.Enabled = true;
            this.Text = TITLE + ": " + result.Item2;

            InitTreeView();
        }

        /// <summary>
        /// Invokes if the 'File -> Save model'-option in the menu strip was clicked.
        /// 
        /// This will open a dialog to determine where to save the current model.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void saveModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                String folder = fbd.SelectedPath;
                GlobalState.varModel.Path = folder + Path.DirectorySeparatorChar + GlobalState.varModel.Name + ".xml";
                GlobalState.varModel.saveXML();
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
            // EXTENSION: Ask if there is any unsaved data

            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new FileInfo(pfd.FileName);
                GlobalState.varModel = VariabilityModel.loadFromXML(fi.FullName);
                this.saveModelToolStripMenuItem.Enabled = true;
                this.editToolStripMenuItem.Enabled = true;
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
            // EXTENSION: Ask if there is any unsaved data
            this.Dispose();
        }

        /// <summary>
        /// Invokes if the 'Edit -> Edit Options'-option in the menu strip was clicked.
        /// 
        /// This will open the corresponding dialog for editing options.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void editOptionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EditOptionDialog form = new EditOptionDialog(this, null);
            form.Show();
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
            TreeNode tn = treeView.GetNodeAt(treeView.PointToClient(new Point(contextMenuStrip.Left, contextMenuStrip.Top)));
            String featureName = tn == null ? null : tn.Text;

            NewFeatureDialog dlg = new NewFeatureDialog(featureName);
            dlg.ShowDialog();

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
            TreeNode tn = treeView.GetNodeAt(treeView.PointToClient(new Point(contextMenuStrip.Left, contextMenuStrip.Top)));
            ConfigurationOption selected;

            if (tn == null)
                selected = null;
            else if (tn.Text == "root")
                selected = GlobalState.varModel.Root;
            else
                selected = GlobalState.varModel.getOption(tn.Text);

            EditOptionDialog dlg = new EditOptionDialog(this, selected);
            dlg.ShowDialog();

            InitTreeView();
        }

        /// <summary>
        /// Invokes if the 'Remove Feature'-option in the context menu strip was clicked.
        /// 
        /// This will open a dialog to get sure if the user really wants to remove the
        /// feature at the position where the context menu strip was placed. All children
        /// of this feature will be deleted too.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        private void removeFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(REMOVE_WARNING, "", MessageBoxButtons.YesNo);

            if (DialogResult == DialogResult.Yes)
            {
                TreeNode tn = treeView.GetNodeAt(treeView.PointToClient(new Point(contextMenuStrip.Left, contextMenuStrip.Top)));
                ConfigurationOption selected = GlobalState.varModel.getOption(tn.Text);

                GlobalState.varModel.deleteOption(selected);

                InitTreeView();
            }
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
    }
}
