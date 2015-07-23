namespace VariabilitModel_GUI
{
    partial class EditOptionDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.selectBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.excludeSingleListBox = new System.Windows.Forms.ListBox();
            this.requiresSingleListBox = new System.Windows.Forms.ListBox();
            this.otherBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.delSingleExclude = new System.Windows.Forms.Button();
            this.delSingleRequire = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.finRequire = new System.Windows.Forms.Button();
            this.finExclude = new System.Windows.Forms.Button();
            this.requiresOverviewDelButton = new System.Windows.Forms.Button();
            this.excludesOverviewDelButton = new System.Windows.Forms.Button();
            this.excludesOverview = new System.Windows.Forms.ListBox();
            this.requiresOverview = new System.Windows.Forms.ListBox();
            this.addOption = new System.Windows.Forms.Button();
            this.deleteNBConst = new System.Windows.Forms.Button();
            this.nbConstraintBox = new System.Windows.Forms.ListBox();
            this.removeNBConst = new System.Windows.Forms.Button();
            this.addNBConst = new System.Windows.Forms.Button();
            this.nonBoolConstraint = new System.Windows.Forms.TextBox();
            this.numEq = new System.Windows.Forms.Button();
            this.numGre = new System.Windows.Forms.Button();
            this.numDif = new System.Windows.Forms.Button();
            this.numTimes = new System.Windows.Forms.Button();
            this.numPlus = new System.Windows.Forms.Button();
            this.num0 = new System.Windows.Forms.Button();
            this.num3 = new System.Windows.Forms.Button();
            this.num2 = new System.Windows.Forms.Button();
            this.num1 = new System.Windows.Forms.Button();
            this.num6 = new System.Windows.Forms.Button();
            this.num5 = new System.Windows.Forms.Button();
            this.num4 = new System.Windows.Forms.Button();
            this.num9 = new System.Windows.Forms.Button();
            this.num8 = new System.Windows.Forms.Button();
            this.num7 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nbConstOptions = new System.Windows.Forms.ComboBox();
            this.nBConstLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.setParent = new System.Windows.Forms.Button();
            this.deleteOption = new System.Windows.Forms.Button();
            this.renameOption = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.renameOption_TextBox = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Option:";
            // 
            // selectBox
            // 
            this.selectBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectBox.FormattingEnabled = true;
            this.selectBox.Location = new System.Drawing.Point(149, 14);
            this.selectBox.Name = "selectBox";
            this.selectBox.Size = new System.Drawing.Size(167, 23);
            this.selectBox.TabIndex = 1;
            this.selectBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.OrangeRed;
            this.label6.Location = new System.Drawing.Point(10, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Excludes:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.OrangeRed;
            this.label7.Location = new System.Drawing.Point(10, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Requires:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(349, 18);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 19);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Optional";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(348, 44);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(74, 19);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "Dynamic";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // excludeSingleListBox
            // 
            this.excludeSingleListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludeSingleListBox.FormattingEnabled = true;
            this.excludeSingleListBox.ItemHeight = 15;
            this.excludeSingleListBox.Location = new System.Drawing.Point(158, 40);
            this.excludeSingleListBox.Name = "excludeSingleListBox";
            this.excludeSingleListBox.Size = new System.Drawing.Size(120, 79);
            this.excludeSingleListBox.TabIndex = 14;
            // 
            // requiresSingleListBox
            // 
            this.requiresSingleListBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresSingleListBox.FormattingEnabled = true;
            this.requiresSingleListBox.ItemHeight = 15;
            this.requiresSingleListBox.Location = new System.Drawing.Point(158, 205);
            this.requiresSingleListBox.Name = "requiresSingleListBox";
            this.requiresSingleListBox.Size = new System.Drawing.Size(120, 79);
            this.requiresSingleListBox.TabIndex = 15;
            // 
            // otherBox
            // 
            this.otherBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otherBox.FormattingEnabled = true;
            this.otherBox.Location = new System.Drawing.Point(149, 42);
            this.otherBox.Name = "otherBox";
            this.otherBox.Size = new System.Drawing.Size(167, 23);
            this.otherBox.TabIndex = 22;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(10, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 15);
            this.label13.TabIndex = 23;
            this.label13.Text = "New Relation with:";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(158, 125);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(158, 290);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(38, 23);
            this.button3.TabIndex = 26;
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // delSingleExclude
            // 
            this.delSingleExclude.BackColor = System.Drawing.Color.Transparent;
            this.delSingleExclude.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delSingleExclude.Location = new System.Drawing.Point(242, 125);
            this.delSingleExclude.Name = "delSingleExclude";
            this.delSingleExclude.Size = new System.Drawing.Size(35, 23);
            this.delSingleExclude.TabIndex = 32;
            this.delSingleExclude.Text = "Del";
            this.delSingleExclude.UseVisualStyleBackColor = false;
            this.delSingleExclude.Click += new System.EventHandler(this.button9_Click);
            // 
            // delSingleRequire
            // 
            this.delSingleRequire.BackColor = System.Drawing.Color.Transparent;
            this.delSingleRequire.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delSingleRequire.Location = new System.Drawing.Point(242, 290);
            this.delSingleRequire.Name = "delSingleRequire";
            this.delSingleRequire.Size = new System.Drawing.Size(36, 23);
            this.delSingleRequire.TabIndex = 33;
            this.delSingleRequire.Text = "Del";
            this.delSingleRequire.UseVisualStyleBackColor = false;
            this.delSingleRequire.Click += new System.EventHandler(this.button10_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label14.Location = new System.Drawing.Point(12, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(215, 15);
            this.label14.TabIndex = 38;
            this.label14.Text = "Parent-Child and Constraint Relations";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(395, 107);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 15);
            this.label15.TabIndex = 39;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(395, 78);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 15);
            this.label17.TabIndex = 41;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.finRequire);
            this.groupBox2.Controls.Add(this.finExclude);
            this.groupBox2.Controls.Add(this.requiresOverviewDelButton);
            this.groupBox2.Controls.Add(this.excludesOverviewDelButton);
            this.groupBox2.Controls.Add(this.excludesOverview);
            this.groupBox2.Controls.Add(this.requiresOverview);
            this.groupBox2.Controls.Add(this.addOption);
            this.groupBox2.Controls.Add(this.deleteNBConst);
            this.groupBox2.Controls.Add(this.nbConstraintBox);
            this.groupBox2.Controls.Add(this.removeNBConst);
            this.groupBox2.Controls.Add(this.addNBConst);
            this.groupBox2.Controls.Add(this.nonBoolConstraint);
            this.groupBox2.Controls.Add(this.numEq);
            this.groupBox2.Controls.Add(this.numGre);
            this.groupBox2.Controls.Add(this.numDif);
            this.groupBox2.Controls.Add(this.numTimes);
            this.groupBox2.Controls.Add(this.numPlus);
            this.groupBox2.Controls.Add(this.num0);
            this.groupBox2.Controls.Add(this.num3);
            this.groupBox2.Controls.Add(this.num2);
            this.groupBox2.Controls.Add(this.num1);
            this.groupBox2.Controls.Add(this.num6);
            this.groupBox2.Controls.Add(this.num5);
            this.groupBox2.Controls.Add(this.num4);
            this.groupBox2.Controls.Add(this.num9);
            this.groupBox2.Controls.Add(this.num8);
            this.groupBox2.Controls.Add(this.num7);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.nbConstOptions);
            this.groupBox2.Controls.Add(this.nBConstLabel);
            this.groupBox2.Controls.Add(this.excludeSingleListBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.requiresSingleListBox);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.delSingleExclude);
            this.groupBox2.Controls.Add(this.delSingleRequire);
            this.groupBox2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.OrangeRed;
            this.groupBox2.Location = new System.Drawing.Point(16, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 327);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Constraints";
            // 
            // finRequire
            // 
            this.finRequire.BackColor = System.Drawing.Color.Transparent;
            this.finRequire.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finRequire.Location = new System.Drawing.Point(201, 290);
            this.finRequire.Name = "finRequire";
            this.finRequire.Size = new System.Drawing.Size(36, 23);
            this.finRequire.TabIndex = 63;
            this.finRequire.Text = "Fin";
            this.finRequire.UseVisualStyleBackColor = false;
            this.finRequire.Click += new System.EventHandler(this.finRequire_Click);
            // 
            // finExclude
            // 
            this.finExclude.BackColor = System.Drawing.Color.Transparent;
            this.finExclude.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finExclude.Location = new System.Drawing.Point(201, 125);
            this.finExclude.Name = "finExclude";
            this.finExclude.Size = new System.Drawing.Size(36, 23);
            this.finExclude.TabIndex = 62;
            this.finExclude.Text = "Fin";
            this.finExclude.UseVisualStyleBackColor = false;
            this.finExclude.Click += new System.EventHandler(this.finExclude_Click);
            // 
            // requiresOverviewDelButton
            // 
            this.requiresOverviewDelButton.BackColor = System.Drawing.Color.Transparent;
            this.requiresOverviewDelButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresOverviewDelButton.Location = new System.Drawing.Point(13, 265);
            this.requiresOverviewDelButton.Name = "requiresOverviewDelButton";
            this.requiresOverviewDelButton.Size = new System.Drawing.Size(48, 23);
            this.requiresOverviewDelButton.TabIndex = 61;
            this.requiresOverviewDelButton.Text = "Del";
            this.requiresOverviewDelButton.UseVisualStyleBackColor = false;
            this.requiresOverviewDelButton.Click += new System.EventHandler(this.requiresOverviewDelButton_Click);
            // 
            // excludesOverviewDelButton
            // 
            this.excludesOverviewDelButton.BackColor = System.Drawing.Color.Transparent;
            this.excludesOverviewDelButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesOverviewDelButton.Location = new System.Drawing.Point(13, 95);
            this.excludesOverviewDelButton.Name = "excludesOverviewDelButton";
            this.excludesOverviewDelButton.Size = new System.Drawing.Size(48, 23);
            this.excludesOverviewDelButton.TabIndex = 60;
            this.excludesOverviewDelButton.Text = "Del";
            this.excludesOverviewDelButton.UseVisualStyleBackColor = false;
            this.excludesOverviewDelButton.Click += new System.EventHandler(this.excludesOverviewDelButton_Click);
            // 
            // excludesOverview
            // 
            this.excludesOverview.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.excludesOverview.FormattingEnabled = true;
            this.excludesOverview.ItemHeight = 15;
            this.excludesOverview.Location = new System.Drawing.Point(13, 40);
            this.excludesOverview.Name = "excludesOverview";
            this.excludesOverview.Size = new System.Drawing.Size(101, 49);
            this.excludesOverview.TabIndex = 59;
            this.excludesOverview.SelectedIndexChanged += new System.EventHandler(this.excludesOverview_SelectedIndexChanged);
            // 
            // requiresOverview
            // 
            this.requiresOverview.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiresOverview.FormattingEnabled = true;
            this.requiresOverview.ItemHeight = 15;
            this.requiresOverview.Location = new System.Drawing.Point(13, 210);
            this.requiresOverview.Name = "requiresOverview";
            this.requiresOverview.Size = new System.Drawing.Size(101, 49);
            this.requiresOverview.TabIndex = 58;
            // 
            // addOption
            // 
            this.addOption.Location = new System.Drawing.Point(368, 65);
            this.addOption.Name = "addOption";
            this.addOption.Size = new System.Drawing.Size(75, 23);
            this.addOption.TabIndex = 57;
            this.addOption.Text = "Add";
            this.addOption.UseVisualStyleBackColor = true;
            this.addOption.Click += new System.EventHandler(this.addOption_Click);
            // 
            // deleteNBConst
            // 
            this.deleteNBConst.Location = new System.Drawing.Point(526, 261);
            this.deleteNBConst.Name = "deleteNBConst";
            this.deleteNBConst.Size = new System.Drawing.Size(64, 23);
            this.deleteNBConst.TabIndex = 56;
            this.deleteNBConst.Text = "Delete";
            this.deleteNBConst.UseVisualStyleBackColor = true;
            this.deleteNBConst.Click += new System.EventHandler(this.deleteNBConst_Click);
            // 
            // nbConstraintBox
            // 
            this.nbConstraintBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nbConstraintBox.FormattingEnabled = true;
            this.nbConstraintBox.ItemHeight = 15;
            this.nbConstraintBox.Location = new System.Drawing.Point(318, 247);
            this.nbConstraintBox.Name = "nbConstraintBox";
            this.nbConstraintBox.Size = new System.Drawing.Size(202, 79);
            this.nbConstraintBox.TabIndex = 55;
            // 
            // removeNBConst
            // 
            this.removeNBConst.Location = new System.Drawing.Point(526, 217);
            this.removeNBConst.Name = "removeNBConst";
            this.removeNBConst.Size = new System.Drawing.Size(65, 23);
            this.removeNBConst.TabIndex = 54;
            this.removeNBConst.Text = "Remove";
            this.removeNBConst.UseVisualStyleBackColor = true;
            this.removeNBConst.Click += new System.EventHandler(this.removeNBConst_Click);
            // 
            // addNBConst
            // 
            this.addNBConst.Location = new System.Drawing.Point(318, 218);
            this.addNBConst.Name = "addNBConst";
            this.addNBConst.Size = new System.Drawing.Size(75, 23);
            this.addNBConst.TabIndex = 53;
            this.addNBConst.Text = "Add";
            this.addNBConst.UseVisualStyleBackColor = true;
            this.addNBConst.Click += new System.EventHandler(this.addNBConst_Click);
            // 
            // nonBoolConstraint
            // 
            this.nonBoolConstraint.Enabled = false;
            this.nonBoolConstraint.Location = new System.Drawing.Point(318, 188);
            this.nonBoolConstraint.Name = "nonBoolConstraint";
            this.nonBoolConstraint.Size = new System.Drawing.Size(272, 23);
            this.nonBoolConstraint.TabIndex = 52;
            // 
            // numEq
            // 
            this.numEq.Location = new System.Drawing.Point(556, 151);
            this.numEq.Name = "numEq";
            this.numEq.Size = new System.Drawing.Size(35, 23);
            this.numEq.TabIndex = 51;
            this.numEq.Text = "=";
            this.numEq.UseVisualStyleBackColor = true;
            this.numEq.Click += new System.EventHandler(this.numEq_Click);
            // 
            // numGre
            // 
            this.numGre.Location = new System.Drawing.Point(515, 151);
            this.numGre.Name = "numGre";
            this.numGre.Size = new System.Drawing.Size(35, 23);
            this.numGre.TabIndex = 50;
            this.numGre.Text = ">";
            this.numGre.UseVisualStyleBackColor = true;
            this.numGre.Click += new System.EventHandler(this.numGre_Click);
            // 
            // numDif
            // 
            this.numDif.Location = new System.Drawing.Point(474, 152);
            this.numDif.Name = "numDif";
            this.numDif.Size = new System.Drawing.Size(35, 23);
            this.numDif.TabIndex = 49;
            this.numDif.Text = "-";
            this.numDif.UseVisualStyleBackColor = true;
            this.numDif.Click += new System.EventHandler(this.numDif_Click);
            // 
            // numTimes
            // 
            this.numTimes.Location = new System.Drawing.Point(556, 122);
            this.numTimes.Name = "numTimes";
            this.numTimes.Size = new System.Drawing.Size(35, 23);
            this.numTimes.TabIndex = 48;
            this.numTimes.Text = "*";
            this.numTimes.UseVisualStyleBackColor = true;
            this.numTimes.Click += new System.EventHandler(this.numTimes_Click);
            // 
            // numPlus
            // 
            this.numPlus.Location = new System.Drawing.Point(515, 122);
            this.numPlus.Name = "numPlus";
            this.numPlus.Size = new System.Drawing.Size(35, 23);
            this.numPlus.TabIndex = 47;
            this.numPlus.Text = "+";
            this.numPlus.UseVisualStyleBackColor = true;
            this.numPlus.Click += new System.EventHandler(this.numPlus_Click);
            // 
            // num0
            // 
            this.num0.Location = new System.Drawing.Point(474, 123);
            this.num0.Name = "num0";
            this.num0.Size = new System.Drawing.Size(35, 23);
            this.num0.TabIndex = 46;
            this.num0.Text = "0";
            this.num0.UseVisualStyleBackColor = true;
            this.num0.Click += new System.EventHandler(this.num0_Click);
            // 
            // num3
            // 
            this.num3.Location = new System.Drawing.Point(556, 93);
            this.num3.Name = "num3";
            this.num3.Size = new System.Drawing.Size(35, 23);
            this.num3.TabIndex = 45;
            this.num3.Text = "3";
            this.num3.UseVisualStyleBackColor = true;
            this.num3.Click += new System.EventHandler(this.num3_Click);
            // 
            // num2
            // 
            this.num2.Location = new System.Drawing.Point(515, 93);
            this.num2.Name = "num2";
            this.num2.Size = new System.Drawing.Size(35, 23);
            this.num2.TabIndex = 44;
            this.num2.Text = "2";
            this.num2.UseVisualStyleBackColor = true;
            this.num2.Click += new System.EventHandler(this.num2_Click);
            // 
            // num1
            // 
            this.num1.Location = new System.Drawing.Point(474, 94);
            this.num1.Name = "num1";
            this.num1.Size = new System.Drawing.Size(35, 23);
            this.num1.TabIndex = 43;
            this.num1.Text = "1";
            this.num1.UseVisualStyleBackColor = true;
            this.num1.Click += new System.EventHandler(this.num1_Click);
            // 
            // num6
            // 
            this.num6.Location = new System.Drawing.Point(556, 65);
            this.num6.Name = "num6";
            this.num6.Size = new System.Drawing.Size(35, 23);
            this.num6.TabIndex = 42;
            this.num6.Text = "6";
            this.num6.UseVisualStyleBackColor = true;
            this.num6.Click += new System.EventHandler(this.num6_Click);
            // 
            // num5
            // 
            this.num5.Location = new System.Drawing.Point(515, 65);
            this.num5.Name = "num5";
            this.num5.Size = new System.Drawing.Size(35, 23);
            this.num5.TabIndex = 41;
            this.num5.Text = "5";
            this.num5.UseVisualStyleBackColor = true;
            this.num5.Click += new System.EventHandler(this.num5_Click);
            // 
            // num4
            // 
            this.num4.Location = new System.Drawing.Point(474, 66);
            this.num4.Name = "num4";
            this.num4.Size = new System.Drawing.Size(35, 23);
            this.num4.TabIndex = 40;
            this.num4.Text = "4";
            this.num4.UseVisualStyleBackColor = true;
            this.num4.Click += new System.EventHandler(this.num4_Click);
            // 
            // num9
            // 
            this.num9.Location = new System.Drawing.Point(556, 36);
            this.num9.Name = "num9";
            this.num9.Size = new System.Drawing.Size(35, 23);
            this.num9.TabIndex = 39;
            this.num9.Text = "9";
            this.num9.UseVisualStyleBackColor = true;
            this.num9.Click += new System.EventHandler(this.num9_Click);
            // 
            // num8
            // 
            this.num8.Location = new System.Drawing.Point(515, 36);
            this.num8.Name = "num8";
            this.num8.Size = new System.Drawing.Size(35, 23);
            this.num8.TabIndex = 38;
            this.num8.Text = "8";
            this.num8.UseVisualStyleBackColor = true;
            this.num8.Click += new System.EventHandler(this.num8_Click);
            // 
            // num7
            // 
            this.num7.Location = new System.Drawing.Point(474, 37);
            this.num7.Name = "num7";
            this.num7.Size = new System.Drawing.Size(35, 23);
            this.num7.TabIndex = 37;
            this.num7.Text = "7";
            this.num7.UseVisualStyleBackColor = true;
            this.num7.Click += new System.EventHandler(this.num7_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 36;
            this.label2.Text = "Option:";
            // 
            // nbConstOptions
            // 
            this.nbConstOptions.FormattingEnabled = true;
            this.nbConstOptions.Location = new System.Drawing.Point(368, 37);
            this.nbConstOptions.Name = "nbConstOptions";
            this.nbConstOptions.Size = new System.Drawing.Size(97, 23);
            this.nbConstOptions.TabIndex = 35;
            // 
            // nBConstLabel
            // 
            this.nBConstLabel.AutoSize = true;
            this.nBConstLabel.Location = new System.Drawing.Point(315, 18);
            this.nBConstLabel.Name = "nBConstLabel";
            this.nBConstLabel.Size = new System.Drawing.Size(145, 15);
            this.nBConstLabel.TabIndex = 34;
            this.nBConstLabel.Text = "Non-Boolean Constraints";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.setParent);
            this.groupBox4.Controls.Add(this.deleteOption);
            this.groupBox4.Controls.Add(this.renameOption);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.renameOption_TextBox);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.selectBox);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.checkBox2);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.otherBox);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(16, 41);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(600, 111);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General Properties";
            // 
            // setParent
            // 
            this.setParent.Location = new System.Drawing.Point(149, 74);
            this.setParent.Name = "setParent";
            this.setParent.Size = new System.Drawing.Size(128, 23);
            this.setParent.TabIndex = 57;
            this.setParent.Text = "Set Parent";
            this.setParent.UseVisualStyleBackColor = true;
            this.setParent.Click += new System.EventHandler(this.setParent_Click);
            // 
            // deleteOption
            // 
            this.deleteOption.Location = new System.Drawing.Point(13, 74);
            this.deleteOption.Name = "deleteOption";
            this.deleteOption.Size = new System.Drawing.Size(128, 23);
            this.deleteOption.TabIndex = 56;
            this.deleteOption.Text = "Delete Option";
            this.deleteOption.UseVisualStyleBackColor = true;
            this.deleteOption.Click += new System.EventHandler(this.button24_Click);
            // 
            // renameOption
            // 
            this.renameOption.Location = new System.Drawing.Point(515, 40);
            this.renameOption.Name = "renameOption";
            this.renameOption.Size = new System.Drawing.Size(75, 23);
            this.renameOption.TabIndex = 52;
            this.renameOption.Text = "Ok";
            this.renameOption.UseVisualStyleBackColor = true;
            this.renameOption.Click += new System.EventHandler(this.renameOption_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(431, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 15);
            this.label11.TabIndex = 51;
            this.label11.Text = "Rename:";
            // 
            // renameOption_TextBox
            // 
            this.renameOption_TextBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renameOption_TextBox.Location = new System.Drawing.Point(490, 15);
            this.renameOption_TextBox.Name = "renameOption_TextBox";
            this.renameOption_TextBox.Size = new System.Drawing.Size(100, 23);
            this.renameOption_TextBox.TabIndex = 50;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox3);
            this.groupBox5.Controls.Add(this.textBox2);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(16, 158);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(600, 79);
            this.groupBox5.TabIndex = 53;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Settings";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(315, 19);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(285, 54);
            this.textBox3.TabIndex = 24;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(13, 37);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(172, 23);
            this.textBox2.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(198, 19);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(117, 15);
            this.label18.TabIndex = 23;
            this.label18.Text = "Feature Description:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(10, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(173, 15);
            this.label12.TabIndex = 23;
            this.label12.Text = "Variant Generation Parameter:";
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Location = new System.Drawing.Point(13, 573);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(46, 16);
            this.WarningLabel.TabIndex = 54;
            this.WarningLabel.Text = "label3";
            // 
            // EditOptionDialog
            // 
            this.ClientSize = new System.Drawing.Size(639, 600);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label14);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditOptionDialog";
            this.Text = "Assignment of Options and Constraints";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox selectBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ListBox excludeSingleListBox;
        private System.Windows.Forms.ListBox requiresSingleListBox;
        private System.Windows.Forms.ComboBox otherBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button delSingleExclude;
        private System.Windows.Forms.Button delSingleRequire;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox renameOption_TextBox;
        private System.Windows.Forms.Button renameOption;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button deleteOption;
        private System.Windows.Forms.ComboBox nbConstOptions;
        private System.Windows.Forms.Label nBConstLabel;
        private System.Windows.Forms.Button num3;
        private System.Windows.Forms.Button num2;
        private System.Windows.Forms.Button num1;
        private System.Windows.Forms.Button num6;
        private System.Windows.Forms.Button num5;
        private System.Windows.Forms.Button num4;
        private System.Windows.Forms.Button num9;
        private System.Windows.Forms.Button num8;
        private System.Windows.Forms.Button num7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button numEq;
        private System.Windows.Forms.Button numGre;
        private System.Windows.Forms.Button numDif;
        private System.Windows.Forms.Button numTimes;
        private System.Windows.Forms.Button numPlus;
        private System.Windows.Forms.Button num0;
        private System.Windows.Forms.TextBox nonBoolConstraint;
        private System.Windows.Forms.Button removeNBConst;
        private System.Windows.Forms.Button addNBConst;
        private System.Windows.Forms.Button deleteNBConst;
        private System.Windows.Forms.ListBox nbConstraintBox;
        private System.Windows.Forms.Button addOption;
        private System.Windows.Forms.ListBox excludesOverview;
        private System.Windows.Forms.ListBox requiresOverview;
        private System.Windows.Forms.Button requiresOverviewDelButton;
        private System.Windows.Forms.Button excludesOverviewDelButton;
        private System.Windows.Forms.Button finRequire;
        private System.Windows.Forms.Button finExclude;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Button setParent;
    }
}