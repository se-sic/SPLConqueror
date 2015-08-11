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
            this.buttonEditFeatures = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.newModel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.loadModel = new System.Windows.Forms.Button();
            this.modelNameBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonEditFeatures
            // 
            this.buttonEditFeatures.BackColor = System.Drawing.Color.Transparent;
            this.buttonEditFeatures.Location = new System.Drawing.Point(23, 502);
            this.buttonEditFeatures.Name = "buttonEditFeatures";
            this.buttonEditFeatures.Size = new System.Drawing.Size(89, 20);
            this.buttonEditFeatures.TabIndex = 65;
            this.buttonEditFeatures.Text = "Edit Options";
            this.buttonEditFeatures.UseVisualStyleBackColor = false;
            this.buttonEditFeatures.Click += new System.EventHandler(this.buttonEditFeatures_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(23, 11);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(264, 486);
            this.treeView1.TabIndex = 63;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeView1_NodeMouseHover);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // newModel
            // 
            this.newModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.newModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.newModel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newModel.Location = new System.Drawing.Point(23, 548);
            this.newModel.Name = "newModel";
            this.newModel.Size = new System.Drawing.Size(111, 21);
            this.newModel.TabIndex = 70;
            this.newModel.Text = "Create Model";
            this.newModel.UseVisualStyleBackColor = false;
            this.newModel.Click += new System.EventHandler(this.newModel_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(176, 584);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 21);
            this.button1.TabIndex = 69;
            this.button1.Text = "Save Model";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.buttonSavePLM_Click);
            // 
            // loadModel
            // 
            this.loadModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.loadModel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.loadModel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadModel.Location = new System.Drawing.Point(23, 584);
            this.loadModel.Name = "loadModel";
            this.loadModel.Size = new System.Drawing.Size(111, 21);
            this.loadModel.TabIndex = 68;
            this.loadModel.Text = "Load Model";
            this.loadModel.UseVisualStyleBackColor = false;
            this.loadModel.Click += new System.EventHandler(this.loadModel_Click);
            // 
            // modelNameBox
            // 
            this.modelNameBox.Location = new System.Drawing.Point(156, 549);
            this.modelNameBox.Name = "modelNameBox";
            this.modelNameBox.Size = new System.Drawing.Size(131, 20);
            this.modelNameBox.TabIndex = 71;
            // 
            // VariabilityModel_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 616);
            this.Controls.Add(this.modelNameBox);
            this.Controls.Add(this.newModel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.loadModel);
            this.Controls.Add(this.buttonEditFeatures);
            this.Controls.Add(this.treeView1);
            this.Name = "VariabilityModel_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button buttonEditFeatures;
        protected System.Windows.Forms.TreeView treeView1;
        protected System.Windows.Forms.Button newModel;
        protected System.Windows.Forms.Button button1;
        protected System.Windows.Forms.Button loadModel;
        private System.Windows.Forms.TextBox modelNameBox;
    }
}

