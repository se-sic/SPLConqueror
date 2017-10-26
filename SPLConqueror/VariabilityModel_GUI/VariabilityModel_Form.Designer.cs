namespace VariabilitModel_GUI
{
    partial class VariabilityModel_Form
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveModelAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editConstraintsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editAlternativeGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertNumericOptionsToBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDimacsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertLegacyVariabilityModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAlternativeGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSXFMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.Location = new System.Drawing.Point(13, 37);
            this.treeView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(799, 984);
            this.treeView.TabIndex = 63;
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(827, 28);
            this.menuStrip.TabIndex = 72;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newModelToolStripMenuItem,
            this.loadModelToolStripMenuItem,
            this.loadSXFMToolStripMenuItem,
            this.saveModelToolStripMenuItem,
            this.saveModelAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newModelToolStripMenuItem
            // 
            this.newModelToolStripMenuItem.Name = "newModelToolStripMenuItem";
            this.newModelToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.newModelToolStripMenuItem.Text = "New Model...";
            this.newModelToolStripMenuItem.Click += new System.EventHandler(this.newModelToolStripMenuItem_Click);
            // 
            // loadModelToolStripMenuItem
            // 
            this.loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            this.loadModelToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.loadModelToolStripMenuItem.Text = "Load Model...";
            this.loadModelToolStripMenuItem.Click += new System.EventHandler(this.loadModelToolStripMenuItem_Click);
            // 
            // saveModelToolStripMenuItem
            // 
            this.saveModelToolStripMenuItem.Name = "saveModelToolStripMenuItem";
            this.saveModelToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.saveModelToolStripMenuItem.Text = "Save Model...";
            this.saveModelToolStripMenuItem.Click += new System.EventHandler(this.saveModelToolStripMenuItem_Click);
            // 
            // saveModelAsToolStripMenuItem
            // 
            this.saveModelAsToolStripMenuItem.Name = "saveModelAsToolStripMenuItem";
            this.saveModelAsToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.saveModelAsToolStripMenuItem.Text = "Save Model As...";
            this.saveModelAsToolStripMenuItem.Click += new System.EventHandler(this.saveModelAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editOptionsToolStripMenuItem,
            this.editConstraintsToolStripMenuItem,
            this.editAlternativeGroupsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // editOptionsToolStripMenuItem
            // 
            this.editOptionsToolStripMenuItem.Name = "editOptionsToolStripMenuItem";
            this.editOptionsToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
            this.editOptionsToolStripMenuItem.Text = "Edit Options...";
            this.editOptionsToolStripMenuItem.Click += new System.EventHandler(this.editOptionsToolStripMenuItem_Click);
            // 
            // editConstraintsToolStripMenuItem
            // 
            this.editConstraintsToolStripMenuItem.Name = "editConstraintsToolStripMenuItem";
            this.editConstraintsToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
            this.editConstraintsToolStripMenuItem.Text = "Edit Constraints...";
            this.editConstraintsToolStripMenuItem.Click += new System.EventHandler(this.editConstraintsToolStripMenuItem_Click);
            // 
            // editAlternativeGroupsToolStripMenuItem
            // 
            this.editAlternativeGroupsToolStripMenuItem.Name = "editAlternativeGroupsToolStripMenuItem";
            this.editAlternativeGroupsToolStripMenuItem.Size = new System.Drawing.Size(246, 26);
            this.editAlternativeGroupsToolStripMenuItem.Text = "Edit Alternative Groups...";
            this.editAlternativeGroupsToolStripMenuItem.Click += new System.EventHandler(this.editAlternativeGroupsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(28, 24);
            this.helpToolStripMenuItem.Text = "?";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertNumericOptionsToBinaryToolStripMenuItem,
            this.exportToDimacsToolStripMenuItem,
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem,
            this.convertLegacyVariabilityModelToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // convertNumericOptionsToBinaryToolStripMenuItem
            // 
            this.convertNumericOptionsToBinaryToolStripMenuItem.Name = "convertNumericOptionsToBinaryToolStripMenuItem";
            this.convertNumericOptionsToBinaryToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.convertNumericOptionsToBinaryToolStripMenuItem.Text = "Convert numeric options to binary";
            this.convertNumericOptionsToBinaryToolStripMenuItem.Click += new System.EventHandler(this.convertNumericOptionsToBinaryToolStripMenuItem_Click);
            // 
            // exportToDimacsToolStripMenuItem
            // 
            this.exportToDimacsToolStripMenuItem.Name = "exportToDimacsToolStripMenuItem";
            this.exportToDimacsToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.exportToDimacsToolStripMenuItem.Text = "Export to dimacs";
            this.exportToDimacsToolStripMenuItem.Click += new System.EventHandler(this.exportToDimacsToolStripMenuItem_Click);
            // 
            // convertMeasurementsToBinaryOnlyToolStripMenuItem
            // 
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem.Name = "convertMeasurementsToBinaryOnlyToolStripMenuItem";
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem.Text = "Convert measurements to binary only";
            this.convertMeasurementsToBinaryOnlyToolStripMenuItem.Click += new System.EventHandler(this.convertMeasurementsToBinaryOnlyToolStripMenuItem_Click);
            // 
            // convertLegacyVariabilityModelToolStripMenuItem
            // 
            this.convertLegacyVariabilityModelToolStripMenuItem.Name = "convertLegacyVariabilityModelToolStripMenuItem";
            this.convertLegacyVariabilityModelToolStripMenuItem.Size = new System.Drawing.Size(330, 26);
            this.convertLegacyVariabilityModelToolStripMenuItem.Text = "Convert legacy Variability Model";
            this.convertLegacyVariabilityModelToolStripMenuItem.Click += new System.EventHandler(this.convertLegacyVariabilityModelToolStripMenuItem_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFeatureToolStripMenuItem,
            this.editFeatureToolStripMenuItem,
            this.removeFeatureToolStripMenuItem,
            this.addAlternativeGroupToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(228, 100);
            // 
            // addFeatureToolStripMenuItem
            // 
            this.addFeatureToolStripMenuItem.Name = "addFeatureToolStripMenuItem";
            this.addFeatureToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.addFeatureToolStripMenuItem.Text = "Add Feature";
            this.addFeatureToolStripMenuItem.Click += new System.EventHandler(this.addFeatureToolStripMenuItem_Click);
            // 
            // editFeatureToolStripMenuItem
            // 
            this.editFeatureToolStripMenuItem.Name = "editFeatureToolStripMenuItem";
            this.editFeatureToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.editFeatureToolStripMenuItem.Text = "Edit Feature";
            this.editFeatureToolStripMenuItem.Click += new System.EventHandler(this.editFeatureToolStripMenuItem_Click);
            // 
            // removeFeatureToolStripMenuItem
            // 
            this.removeFeatureToolStripMenuItem.Name = "removeFeatureToolStripMenuItem";
            this.removeFeatureToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.removeFeatureToolStripMenuItem.Text = "Remove Feature";
            this.removeFeatureToolStripMenuItem.Click += new System.EventHandler(this.removeFeatureToolStripMenuItem_Click);
            // 
            // addAlternativeGroupToolStripMenuItem
            // 
            this.addAlternativeGroupToolStripMenuItem.Name = "addAlternativeGroupToolStripMenuItem";
            this.addAlternativeGroupToolStripMenuItem.Size = new System.Drawing.Size(227, 24);
            this.addAlternativeGroupToolStripMenuItem.Text = "Add Alternative Group";
            this.addAlternativeGroupToolStripMenuItem.Click += new System.EventHandler(this.addAlternativeGroupToolStripMenuItem_Click);
            // 
            // loadSXFMToolStripMenuItem
            // 
            this.loadSXFMToolStripMenuItem.Name = "loadSXFMToolStripMenuItem";
            this.loadSXFMToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.loadSXFMToolStripMenuItem.Text = "Load SXFM...";
            this.loadSXFMToolStripMenuItem.Click += new System.EventHandler(this.loadSXFMToolStripMenuItem_Click);
            // 
            // VariabilityModel_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 1034);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VariabilityModel_Form";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editConstraintsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveModelAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editAlternativeGroupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAlternativeGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertNumericOptionsToBinaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToDimacsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertMeasurementsToBinaryOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertLegacyVariabilityModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSXFMToolStripMenuItem;
    }
}

