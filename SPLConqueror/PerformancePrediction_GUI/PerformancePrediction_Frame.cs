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
using System.IO;

namespace PerformancePrediction_GUI
{
    public partial class PerformancePrediction_Frame : Form
    {
        Logger_Gui log = null;
        Commands cmd = new Commands();

        public const string ERROR = "an error occurred";
        private static System.Threading.Thread executionThread;

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
                filePath = fi.FullName;
            }

            if (filePath == "")
                return;

            this.nfpSelection.Items.Clear();

            cmd.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + filePath);

            foreach (var item in GlobalState.nfProperties.Keys)
            {
                this.nfpSelection.Items.Add(item);
            }
            if (nfpSelection.Items.Count == 0)
                this.nfpSelection.Items.Add(GlobalState.currentNFP);
            else
            {
                this.nfpSelection.SelectedItem = this.nfpSelection.Items[0];
                this.nfpSelection.SetItemChecked(0, true);

                GlobalState.currentNFP = GlobalState.nfProperties[(string)this.nfpSelection.SelectedItem];
            }
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

            nfpSelection.Items.Clear();
            cmd.performOneCommand(Commands.COMMAND_CLEAR_GLOBAL);
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
            button1.Enabled = true;

            bool ableToStart = createSamplingCommands();

            if (ableToStart)
            {
                if (executionThread != null && executionThread.IsAlive) executionThread.Abort();
                executionThread = new System.Threading.Thread(new System.Threading.ThreadStart(startLearning));
                executionThread.Start();
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
            cmd.performOneCommand(Commands.COMMAND_SET_MLSETTING + " " + setting.ToString());
        }

        void roundFinished(object sender, NotifyCollectionChangedEventArgs e)
        {
            //e.NewItems will be an IList of all the items that were added in the AddRange method...
            MachineLearning.Learning.Regression.LearningRound lastRound = (MachineLearning.Learning.Regression.LearningRound)e.NewItems[0];

            UpdateDataGridView(lastRound);

        }

        private NotifyCollectionChangedEventHandler notifyer = null;

        void initLearning(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine();
            if (cmd.exp.models.Count > 0)
            {
                if (notifyer == null)
                    notifyer = new NotifyCollectionChangedEventHandler(roundFinished);

                cmd.exp.models[cmd.exp.models.Count - 1].LearningHistory.CollectionChanged -= notifyer;
                cmd.exp.models[cmd.exp.models.Count - 1].LearningHistory.CollectionChanged += notifyer;

            }

        }

        private void startLearning()
        {
            try
            {
                // make sure that all measurements are deselected
                cmd.performOneCommand(Commands.COMMAND_SELECT_ALL_MEASUREMENTS + " false");

                cmd.exp.models.CollectionChanged += new NotifyCollectionChangedEventHandler(initLearning);
                cmd.performOneCommand(Commands.COMMAND_START_LEARNING_SPL_CONQUEROR);
            }
            catch (System.Threading.ThreadAbortException)
            {
                return;
            }
        }

        private void startWithAllMeasurements()
        {
            try
            {
                cmd.performOneCommand(Commands.COMMAND_SELECT_ALL_MEASUREMENTS + " true");
                cmd.exp.models.CollectionChanged += new NotifyCollectionChangedEventHandler(initLearning);
                cmd.performOneCommand(Commands.COMMAND_START_LEARNING_SPL_CONQUEROR);
            }
            catch (System.Threading.ThreadAbortException)
            {
                return;
            }
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
            // TODO useEpsilonTube from Ml Settings
            double relativeError = cmd.exp.models[0].computeError(lastRound.FeatureSet, GlobalState.allMeasurements.Configurations, false);
            row[2] = relativeError.ToString();


            lastRound.learningError.ToString();

            foreach (Feature f in lastRound.FeatureSet)
            {
                string name = f.getPureString();
                if (!termToIndex.ContainsKey(name))
                {
                    perfInfGridView.Invoke((MethodInvoker)(() => perfInfGridView.Columns[termToIndex.Count + perfInfGrid_definedColumns].Name = name));


                    termToIndex.Add(name, termToIndex.Count + perfInfGrid_definedColumns);

                }
                row[termToIndex[name]] = Math.Round(f.Constant, 2).ToString();
            }


            perfInfGridView.Invoke((MethodInvoker)(() => this.perfInfGridView.Rows.Add(row)));
        }

        private void error()
        {
            GlobalState.logInfo.logLine(ERROR + "\n");

        }

        private void performBinarySampling(Commands cmd, string param)
        {
            cmd.performOneCommand(Commands.COMMAND_BINARY_SAMPLING + " " + param);
        }

        private void performNumericSampling(Commands cmd, string param)
        {
            cmd.performOneCommand(Commands.COMMAND_NUMERIC_SAMPLING + " " + param);
        }

        private bool createSamplingCommands()
        {
            bool binarySelected = false;
            bool numSelected = false;
            string validation = "";

            // Binary sampling strategies
            if (this.OW.Checked)
            {
                binarySelected = true;
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_OPTIONWISE + " " + validation);
            }
            if (this.twise.Checked)
            {
                binarySelected = true;
                string param = "";
                if (!twiseParam.Text.Equals(""))
                {
                    param = "t:" + twiseParam.Text + " ";
                }
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_BINARY_TWISE + " " + param + validation);
            }
            if (this.binRandom.Checked)
            {
                binarySelected = true;
                string param = "";
                if (!numConfigsParam.Text.Equals(""))
                {
                    param += "numConfigs:" + numConfigsParam.Text + " ";
                }
                if (!binRandomSeed.Text.Equals(""))
                {
                    param += "seed:" + binRandomSeed.Text;
                }
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_BINARY_RANDOM + " " + param + validation);
            }
            if (this.PW.Checked)
            {
                binarySelected = true;
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_PAIRWISE + " " + validation);
            }
            if (this.negOW.Checked)
            {
                binarySelected = true;
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE + " " + validation);
            }
            if (this.binWholePop.Checked)
            {
                binarySelected = true;
                performBinarySampling(cmd, Commands.COMMAND_SAMPLE_ALLBINARY + " " + validation);
            }


            // Numeric sampling strategies
            if (num_BoxBehnken_check.Checked)
            {
                numSelected = true;
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_BOXBEHNKEN + " " + validation);
            }
            if (num_CentralComposite_check.Checked)
            {
                numSelected = true;
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE + " " + validation);
            }
            if (num_FullFactorial_check.Checked)
            {
                numSelected = true;
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_FULLFACTORIAL + " " + validation);
            }
            if (num_hyperSampling_check.Checked)
            {
                if (num_hyper_percent_text.Text.Trim() == "")
                {
                    error();
                    return false;
                }
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_HYPERSAMPLING + " "
                    + num_hyper_percent_text.Text + " " + validation);
                numSelected = true;
            }
            if (num_kEx_check.Checked)
            {
                if (num_kEx_n_Box.Text.Trim() == "" || num_kEx_k_Box.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_KEXCHANGE + " sampleSize:" +
                    num_kEx_n_Box.Text.Trim() + " k:" + num_kEx_k_Box.Text.Trim());
                numSelected = true;
            }
            if (num_randomSampling_num.Checked)
            {
                if (num_random_n_Text.Text.Trim() == "" || num_rand_seed_Text.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_RANDOM + " sampleSize:" 
                    + num_random_n_Text.Text.Trim() + " seed:" + num_rand_seed_Text.Text.Trim());
                numSelected = true;
            }

            if (num_oneFactorAtATime_Box.Checked)
            {
                if (num_oneFactorAtATime_num_Text.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_ONEFACTORATATIME + " distinctValuesPerOption:" 
                    + num_oneFactorAtATime_num_Text.Text.Trim());
                numSelected = true;

            }
            if (num_PlackettBurman_check.Checked)
            {
                if (num_Plackett_Level_Box.Text.Trim() == "" || num_Plackett_n_Box.Text.Trim() == "")
                {
                    error();
                    return false;

                }
                performNumericSampling(cmd, Commands.COMMAND_EXPDESIGN_PLACKETTBURMAN + " measurements:" 
                    + num_Plackett_n_Box.Text.Trim() + " level:" + num_Plackett_Level_Box.Text.Trim());
                numSelected = true;
            }

            return numSelected && binarySelected;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (perfInfGridView.SelectedRows.Count == 1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                StreamWriter sw = new StreamWriter(sfd.FileName);
                StringBuilder sb = new StringBuilder();
                var row = perfInfGridView.SelectedRows[0];
                for (int i = 3; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Value == null)
                        break;
                    if (i == row.Cells.Count - 1 || row.Cells[i + 1].Value == null)
                        sb.Append(row.Cells[i].Value + " * " + perfInfGridView.Columns[i].HeaderText);
                    else
                        sb.Append(row.Cells[i].Value + " * " + perfInfGridView.Columns[i].HeaderText + " + ");
                }
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
            else
            {
                MessageBox.Show("Please select exactly one row of the result set.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            cleanButton_Click(null, null);
            button1.Enabled = true;
            setMLSettings();

            if (executionThread != null && executionThread.IsAlive) executionThread.Abort();
            executionThread = new System.Threading.Thread(new System.Threading.ThreadStart(startWithAllMeasurements));
            executionThread.Start();
            InitDataGridView();

        }

        private void printConfigs(string path)
        {
            string prefix = PrefixTextBox.Text;
            string postfix = PostFixTextBox.Text;
            if (prefix.Trim().Equals("") && postfix.Trim().Equals(""))
            {
                cmd.performOneCommand(Commands.COMMAND_PRINT_CONFIGURATIONS + " " + path);
            }
            else if (postfix.Trim().Equals(""))
            {
                cmd.performOneCommand(Commands.COMMAND_PRINT_CONFIGURATIONS + " " + path + " " + prefix);
            }
            else
            {
                cmd.performOneCommand(Commands.COMMAND_PRINT_CONFIGURATIONS + " " + path + " " + prefix + " " + postfix);
            }
            MessageBox.Show("Configurations printed", "Finished");
        }

        private void PrintconfigsButton_Click(object sender, EventArgs e)
        {
            bool ableToStart = createSamplingCommands();
            if (!ableToStart)
            {
                MessageBox.Show("No sampling selected!", "Error");

            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                if (sfd.CheckPathExists)
                {
                    System.Threading.Thread printConfigsThread = new System.Threading.Thread(() => printConfigs(sfd.FileName));
                    printConfigsThread.Start();

                }
                else
                {
                    MessageBox.Show("Invalid path!", "Error");
                }
            }
        }
    }
}
