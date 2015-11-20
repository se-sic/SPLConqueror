using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPLConqueror_Core;
using MachineLearning;
using CommandLine;
using System.Reflection;
using System.Collections.Specialized;
using MachineLearning.Learning.Regression;

namespace PerformancePrediction_GUI
{
    public partial class PerformancePrediction_Frame : Form
    {
        Logger_Gui log = null;
        Commands cmd = new Commands();

        public const string ERROR = "an error occurred";

        public PerformancePrediction_Frame()
        {
            log = new Logger_Gui(this);
            GlobalState.logInfo = log;


            InitializeComponent();
            addMlSettingsBoxContent();

            
        }


        private const int ML_FIELDS_OFFSET = 18;
        private void addMlSettingsBoxContent()
        {
            MachineLearning.Learning.ML_Settings settingsObject = new MachineLearning.Learning.ML_Settings();

            FieldInfo[] fields = settingsObject.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                    Label l = new Label();
                    mlSettingsPanel.Controls.Add(l);

                    l.AutoSize = true;
                    l.Location = new System.Drawing.Point(5, 5 + ML_FIELDS_OFFSET * i);
                    l.Name = fields[i].Name + "_label";
                    l.Size = new System.Drawing.Size(50, 15);
                    l.TabIndex = i * 2;
                    l.Text = fields[i].Name;

                    TextBox t = new TextBox();
                    mlSettingsPanel.Controls.Add(t);

                    t.Location = new System.Drawing.Point(150, 5 + ML_FIELDS_OFFSET * i);
                    t.Name = fields[i].Name + "_textBox";
                    t.Size = new System.Drawing.Size(150, 15);
                    t.TabIndex = i * 2 + 1;
                    t.Text = fields[i].GetValue(settingsObject).ToString();
            }
        }

        private void readMeasurements_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            String filePath = "";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(pfd.FileName);
                filePath =  fi.FullName;
            }

            if (filePath == "")
                return;

            cmd.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + filePath);

            foreach (var item in GlobalState.nfProperties.Keys)
            {
                this.nfpSelection.Items.Add(item);
            }
            this.nfpSelection.SelectedItem = this.nfpSelection.Items[0];
            this.nfpSelection.SetItemChecked(0,true);

            GlobalState.currentNFP = GlobalState.nfProperties[(string)this.nfpSelection.SelectedItem];
           

         

        }

        private void readVarModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            String filePath = "";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(pfd.FileName);
                filePath = fi.FullName;
            }

            if (filePath == "")
                return;

            cmd.performOneCommand(Commands.COMMAND_VARIABILITYMODEL + " " + filePath);
        }

        private void nfpSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " " + nfpSelection.SelectedItem.ToString());
        }

        private void cleanButton_Click(object sender, EventArgs e)
        {
            cmd.performOneCommand(Commands.COMMAND_CLEAR_SAMPLING);
            perfInfGridView.Rows.Clear();
            perfInfGridView.Refresh();
        }

        private void StartLearningButton_Click(object sender, EventArgs e)
        {
            cleanButton_Click(null, null);

            setMLSettings();


            bool ableToStart = createSamplingCommands();

            if(ableToStart){
                System.Threading.Thread myThread;
                myThread = new System.Threading.Thread(new System.Threading.ThreadStart(startLearning));
                myThread.Start();
                InitDataGridView();
            }   
        }

        private void setMLSettings()
        {
            MachineLearning.Learning.ML_Settings setting = new MachineLearning.Learning.ML_Settings();

            foreach (Control c in mlSettingsPanel.Controls)
            {
                if (c.Name.EndsWith("_textBox"))
                {
                    string fieldName = c.Name.Substring(0, c.Name.Length - "_textBox".Length);
                    setting.setSetting(fieldName, ((TextBox)c).Text);
                }
            }
            cmd.performOneCommand(Commands.COMMAND_SET_MLSETTING+" "+setting.ToString());
        }

        void roundFinished(object sender, NotifyCollectionChangedEventArgs e)
        {
            //e.NewItems will be an IList of all the items that were added in the AddRange method...
            MachineLearning.Learning.Regression.LearningRound lastRound = (MachineLearning.Learning.Regression.LearningRound) e.NewItems[0];

            UpdateDataGridView(lastRound);

        }

        private void startLearning()
        {
            
            cmd.exp.models[0].LearningHistory.CollectionChanged += new NotifyCollectionChangedEventHandler(roundFinished);

            cmd.performOneCommand(Commands.COMMAND_START_LEARNING);
        }

        Dictionary<String, int> termToIndex = null;

        int perfInfGrid_definedColumns = 3;

        private void InitDataGridView()
        {
            termToIndex = new Dictionary<string, int>();

            perfInfGridView.ColumnCount = cmd.exp.info.mlSettings.numberOfRounds * 2;
            perfInfGridView.Columns[0].Name = "Round";
            perfInfGridView.Columns[1].Name = "Learning error";
            perfInfGridView.Columns[2].Name = "Global error";
        }

        private void UpdateDataGridView(MachineLearning.Learning.Regression.LearningRound lastRound)
        {
            string[] row = new string[cmd.exp.info.mlSettings.numberOfRounds * 2];
            row[0] = lastRound.round.ToString();
            row[1] = lastRound.learningError.ToString();
            double relativeError = 0.0;
            cmd.exp.models[0].computeError(lastRound.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError);
            row[2] = relativeError.ToString();


            lastRound.learningError.ToString();

            foreach (Feature f in lastRound.FeatureSet)
            {
                string name = f.ToString();
                if (!termToIndex.ContainsKey(name))
                {
                    perfInfGridView.Invoke((MethodInvoker)(() => perfInfGridView.Columns[termToIndex.Count + perfInfGrid_definedColumns].Name = name));


                    termToIndex.Add(name, termToIndex.Count + perfInfGrid_definedColumns);

                }
                row[termToIndex[name]] = Math.Round(f.Constant,2).ToString();
            }

            
            perfInfGridView.Invoke((MethodInvoker)(() => this.perfInfGridView.Rows.Add(row)));
        }

        private void error()
        {
            GlobalState.logInfo.log(ERROR + "\n");

        }

        private bool createSamplingCommands()
        {
            bool binarySelected = false;
            bool numSelected = false;
            string validation = "";

            if (this.OW.Checked)
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_ALLBINARY + " " + validation);
            }
            if (this.OW.Checked)
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_OPTIONWISE + " " + validation);
            }
            if (this.PW.Checked){
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_PAIRWISE + " " + validation);
            }
            if (this.negOW.Checked){
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE + " " + validation);
            }
            if (num_BoxBehnken_check.Checked)
            {
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_BOXBEHNKEN + " " + validation);
            }
            if (num_CentralComposite_check.Checked)
            {
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE + " " + validation);
            }
            if (num_FullFactorial_check.Checked){
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_FULLFACTORIAL + " " + validation);
            }
            if (num_hyperSampling_check.Checked)
            {
                if (num_hyper_percent_text.Text.Trim() == "")
                {
                    error();
                    return false;
                }
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_HYPERSAMPLING + " " + num_hyper_percent_text.Text + " " + validation);
                numSelected = true;
            }
            if (num_kEx_check.Checked)
            {
                if (num_kEx_n_Box.Text.Trim() == "" || num_kEx_k_Box.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_KEXCHANGE + " sampleSize:" + num_kEx_n_Box.Text.Trim() + " k:" + num_kEx_k_Box.Text.Trim());
                numSelected = true;
            }
            if (num_randomSampling_num.Checked)
            {
                if (num_random_n_Text.Text.Trim() == "" || num_rand_seed_Text.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_RANDOM + " sampleSize:" + num_random_n_Text.Text.Trim() + " seed:" + num_rand_seed_Text.Text.Trim());
                numSelected = true;
            }

            if (num_oneFactorAtATime_Box.Checked)
            {
                if (num_oneFactorAtATime_num_Text.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_ONEFACTORATATIME + " distinctValuesPerOption:" + num_oneFactorAtATime_num_Text.Text.Trim());
                numSelected = true;

            }
            if (num_PlackettBurman_check.Checked)
            {
                if (num_Plackett_Level_Box.Text.Trim() == "" || num_Plackett_n_Box.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_PLACKETTBURMAN + " measurements:" + num_Plackett_n_Box.Text.Trim() + " level:" + num_Plackett_Level_Box.Text.Trim());
                numSelected = true;
            }

            return numSelected && binarySelected;

        }

        
    }
}
