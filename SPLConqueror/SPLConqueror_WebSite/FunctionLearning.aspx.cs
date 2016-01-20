using CommandLine;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPLConqueror_WebSite
{
    public partial class FunctionLearning : System.Web.UI.Page
    {
        private static string ERROR_MODEL_NO_FILE = "There is no model to load!";
        private static string ERROR_MODEL_WRONG_FORMAT = "The loaded file is not a .xml-file!";
        private static string ERROR_MODEL_PROBLEM_LOADING = "The loaded model file contains errors.";
        private static string SUCCESS_MODEL_LOADING = "Model loaded.";
        private static string ERROR_MEASUREMENTS_NO_FILE = "There are no measurements to load!";
        private static string ERROR_MEASUREMENTS_WRONG_FORMAT = "The loaded measurement file is not a .xml-file!";
        private static string ERROR_MEASUREMENTS_PROBLEM_LOADING = "The loaded measurements contains errors.";
        private static string SUCCESS_MEASUREMENTS_LOADING = "Measurements loaded.";

        private Commands cmd = new Commands();
        private DataTable data = new DataTable();
        private Dictionary<string, TextBox> mlSettings = new Dictionary<string, TextBox>();
        private Dictionary<string, int> termToIndex;
        private int dataGridView_definedColumns = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // TODO: Initialisierung
                //addMlSettingsBoxContent();
            }
        }

        /// <summary>
        /// Invokes if the button for loading a model was pressed.
        /// 
        /// This method will try to load the uploaded model file. If there are any problems,
        /// an appropriate error message will be shown.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void loadModelButton_Click(object sender, EventArgs e)
        {
            if (!modelFileUpload.HasFile)
            {
                modelErrorLabel.ForeColor = System.Drawing.Color.Red;
                modelErrorLabel.Text = ERROR_MODEL_NO_FILE;
                return;
            }

            string filename = Path.GetFileName(modelFileUpload.FileName);
            modelFileUpload.SaveAs(Server.MapPath("~/") + filename);

            if (!filename.EndsWith(".xml"))
            {
                modelErrorLabel.ForeColor = System.Drawing.Color.Red;
                modelErrorLabel.Text = ERROR_MODEL_WRONG_FORMAT;
                return;
            }

            try
            {
                cmd.performOneCommand(Commands.COMMAND_VARIABILITYMODEL + " " + filename);
            }
            catch
            {
                modelErrorLabel.ForeColor = System.Drawing.Color.Red;
                modelErrorLabel.Text = ERROR_MODEL_PROBLEM_LOADING;
                return;
            }

            modelErrorLabel.ForeColor = System.Drawing.Color.Blue;
            modelErrorLabel.Text = SUCCESS_MODEL_LOADING;
        }

        /// <summary>
        /// Invokes if the button for loading measurements was pressed.
        /// 
        /// This method will try to load the uploaded measurement file. If there are any
        /// problems, an appropriate error message will be shown.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void loadMeasurementsButton_Click(object sender, EventArgs e)
        {
            if (!measurementsFileUpload.HasFile)
            {
                measurementsErrorLabel.ForeColor = System.Drawing.Color.Red;
                measurementsErrorLabel.Text = ERROR_MEASUREMENTS_NO_FILE;
                return;
            }

            string filename = Path.GetFileName(measurementsFileUpload.FileName);
            measurementsFileUpload.SaveAs(Server.MapPath("~/") + filename);

            if (!filename.EndsWith(".xml"))
            {
                measurementsErrorLabel.ForeColor = System.Drawing.Color.Red;
                measurementsErrorLabel.Text = ERROR_MEASUREMENTS_WRONG_FORMAT;
                return;
            }

            try
            {
                cmd.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + filename);
            }
            catch
            {
                measurementsErrorLabel.ForeColor = System.Drawing.Color.Red;
                measurementsErrorLabel.Text = ERROR_MEASUREMENTS_PROBLEM_LOADING;
                return;
            }

            measurementsErrorLabel.ForeColor = System.Drawing.Color.Blue;
            measurementsErrorLabel.Text = SUCCESS_MEASUREMENTS_LOADING;

            foreach (var item in GlobalState.nfProperties.Keys)
            {
                this.nfpCheckBoxList.Items.Add(item);
            }
            this.nfpCheckBoxList.SelectedIndex = 0;
            this.nfpCheckBoxList.Items[0].Selected = true;

            GlobalState.currentNFP = GlobalState.nfProperties[(string)this.nfpCheckBoxList.SelectedItem.Text];
        }

        /// <summary>
        /// Invokes if another index was selected in the corresponding check box list.
        /// 
        /// This method will set the current non-boolean property to the selected one.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void nfpCheckBoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " " + nfpCheckBoxList.SelectedItem.Text);
        }

        /// <summary>
        /// Invokes if the button for cleaning the sampling was pressed.
        /// 
        /// This method wil clean the current sampling.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void cleanSamplingButton_Click(object sender, EventArgs e)
        {
            cmd.performOneCommand(Commands.COMMAND_CLEAR_SAMPLING);
            data.Rows.Clear();
        }

        /// <summary>
        /// Invokes if the check state of the DOptimalCheckBox has changed.
        /// 
        /// This method will activate/deactivate the corresponding textboxes of all possible
        /// parameters.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void ndDOptimalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ndDOptimalKTextBox.Enabled = ndDOptimalCheckBox.Checked;
            ndDOptimalNTextBox.Enabled = ndDOptimalCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the check state of the PlackettBurmanCheckBox has changed.
        /// 
        /// This method will activate/deactivate the corresponding textboxes of all possible
        /// parameters.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void ndPlackettBurmanCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ndPlackettBurmanLevelTextBox.Enabled = ndPlackettBurmanCheckBox.Checked;
            ndPlackettBurmanNTextBox.Enabled = ndPlackettBurmanCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the check state of the RandomSamplingCheckBox has changed.
        /// 
        /// This method will activate/deactivate the corresponding textboxes of all possible
        /// parameters.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void ndRandomSamplingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ndRandomSamplingNTextBox.Enabled = ndRandomSamplingCheckBox.Checked;
            ndRandomSamplingSeedTextBox.Enabled = ndRandomSamplingCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the check state of the HyperSamplingCheckBox has changed.
        /// 
        /// This method will activate/deactivate the corresponding textboxes of all possible
        /// parameters.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void ndHyperSamplingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ndHyperSamplingPercentTextBox.Enabled = ndHyperSamplingCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the check state of the OneFactorCheckBox has changed.
        /// 
        /// This method will activate/deactivate the corresponding textboxes of all possible
        /// parameters.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void ndOneFactorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ndOneFactorValuesTextBox.Enabled = ndOneFactorCheckBox.Checked;
        }

        /// <summary>
        /// Invokes if the button for learning was pressed.
        /// 
        /// This method will start the learning process. The current sampling will be
        /// used and cleaned. All settings for the machine learning will be set. If all
        /// settings are correct, a thread will be started for learning. 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event</param>
        protected void startLearningButton_Click(object sender, EventArgs e)
        {
            cleanSamplingButton_Click(null, null);
            setMLSettings();

            bool ableToStart = createSamplingCommands();

            if (ableToStart)
            {
                //System.Threading.Thread myThread;
                //myThread = new System.Threading.Thread(new System.Threading.ThreadStart(startLearning));
                //myThread.Start();
                startLearning();
                InitDataGridView();
            }
        }

        private void addMlSettingsBoxContent()
        {
            mlSettingsTable.Rows.Clear();
            mlSettings.Clear();

            MachineLearning.Learning.ML_Settings settingsObject = new MachineLearning.Learning.ML_Settings();
            FieldInfo[] fields = settingsObject.GetType().GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                TableRow tRow = new TableRow();

                TableCell labelCell = new TableCell();
                Label l = new Label();
                labelCell.Controls.Add(l);

                l.Text = fields[i].Name;

                TableCell textBoxCell = new TableCell();
                TextBox t = new TextBox();
                textBoxCell.Controls.Add(t);

                t.Text = fields[i].GetValue(settingsObject).ToString();

                tRow.Cells.Add(labelCell);
                tRow.Cells.Add(textBoxCell);

                mlSettingsTable.Rows.Add(tRow);
                mlSettings.Add(l.Text, t);
            }
        }

        private void setMLSettings()
        {
            MachineLearning.Learning.ML_Settings setting = new MachineLearning.Learning.ML_Settings();

            // TODO: Schauen, ob das so passt
            foreach (KeyValuePair<string, TextBox> pair in mlSettings.ToList())
                setting.setSetting(pair.Key, pair.Value.Text);

            cmd.performOneCommand(Commands.COMMAND_SET_MLSETTING + " " + setting.ToString());
        }

        private void roundFinished(object sender, NotifyCollectionChangedEventArgs e)
        {
            //e.NewItems will be an IList of all the items that were added in the AddRange method...
            MachineLearning.Learning.Regression.LearningRound lastRound = (MachineLearning.Learning.Regression.LearningRound)e.NewItems[0];

            UpdateDataGridView(lastRound);
        }

        private void startLearning()
        {
            cmd.exp.models[0].LearningHistory.CollectionChanged += new NotifyCollectionChangedEventHandler(roundFinished);

            cmd.performOneCommand(Commands.COMMAND_START_LEARNING);
        }

        private void InitDataGridView()
        {
            termToIndex = new Dictionary<string, int>();

            DataColumn dc1 = new DataColumn("Round", typeof(Int32));
            DataColumn dc2 = new DataColumn("Learning error", typeof(double));
            DataColumn dc3 = new DataColumn("Global error", typeof(double));

            data.Columns.Add(dc1);
            data.Columns.Add(dc2);
            data.Columns.Add(dc3);

            for (int i = 0; i < cmd.exp.info.mlSettings.numberOfRounds * 2 - 3; i++)
                data.Columns.Add(new DataColumn());

            dataGridView.DataSource = data;
            dataGridView.DataBind();
        }

        private void UpdateDataGridView(MachineLearning.Learning.Regression.LearningRound lastRound)
        {
            DataRow row = data.NewRow();
            row["Round"] = lastRound.round;
            row["Learning error"] = lastRound.learningError;
            double relativeError = 0.0;
            cmd.exp.models[0].computeError(lastRound.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError);
            row["Global error"] = relativeError;

            foreach (Feature f in lastRound.FeatureSet)
            {
                string name = f.ToString();
                if (!termToIndex.ContainsKey(name))
                {
                    data.Columns[termToIndex.Count + dataGridView_definedColumns].ColumnName = name;

                    termToIndex.Add(name, termToIndex.Count + dataGridView_definedColumns);
                }

                row[termToIndex[name]] = Math.Round(f.Constant, 2).ToString();
            }

            data.Rows.Add(row);
        }

        private bool createSamplingCommands()
        {
            bool binarySelected = false;
            bool numSelected = false;
            string validation = "";

            if (bhOptionCheckBox.Checked) // TODO: Nachfragen, ob das stimmt
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_ALLBINARY + " " + validation);
            }

            if (bhOptionCheckBox.Checked)
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_OPTIONWISE + " " + validation);
            }

            if (bhPairCheckBox.Checked)
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_PAIRWISE + " " + validation);
            }

            if (bhNegOptionCheckBox.Checked)
            {
                binarySelected = true;
                cmd.performOneCommand(Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE + " " + validation);
            }

            // TODO: Whole Population?
            if (ndBehnkenCheckBox.Checked)
            {
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_BOXBEHNKEN + " " + validation);
            }

            if (ndCentralCompositeCheckBox.Checked)
            {
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE + " " + validation);
            }

            if (ndFullFactorialCheckBox.Checked)
            {
                numSelected = true;
                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_FULLFACTORIAL + " " + validation);
            }

            if (ndHyperSamplingCheckBox.Checked)
            {
                if (ndHyperSamplingPercentTextBox.Text.Trim() == "")
                {
                    learningErrorLabel.Visible = true;
                    return false;
                }

                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_HYPERSAMPLING + " " + ndHyperSamplingPercentTextBox.Text + " " + validation);
                numSelected = true;
            }

            if (ndDOptimalCheckBox.Checked)
            {
                if (ndDOptimalNTextBox.Text.Trim() == "" || ndDOptimalKTextBox.Text.Trim() == "")
                {
                    learningErrorLabel.Visible = true;
                    return false;
                }

                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_KEXCHANGE + " sampleSize:" + ndDOptimalNTextBox.Text.Trim() + " k:" + ndDOptimalKTextBox.Text.Trim());
                numSelected = true;
            }

            if (ndRandomSamplingCheckBox.Checked)
            {
                if (ndRandomSamplingNTextBox.Text.Trim() == "" || ndRandomSamplingSeedTextBox.Text.Trim() == "")
                {
                    learningErrorLabel.Visible = true;
                    return false;
                }

                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_RANDOM + " sampleSize:" + ndRandomSamplingNTextBox.Text.Trim() + " seed:" + ndRandomSamplingSeedTextBox.Text.Trim());
                numSelected = true;
            }

            if (ndOneFactorCheckBox.Checked)
            {
                if (ndOneFactorValuesTextBox.Text.Trim() == "")
                {
                    learningErrorLabel.Visible = true;
                    return false;
                }

                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_ONEFACTORATATIME + " distinctValuesPerOption:" + ndOneFactorValuesTextBox.Text.Trim());
                numSelected = true;
            }

            if (ndPlackettBurmanCheckBox.Checked)
            {
                if (ndPlackettBurmanLevelTextBox.Text.Trim() == "" || ndPlackettBurmanNTextBox.Text.Trim() == "")
                {
                    learningErrorLabel.Visible = true;
                    return false;
                }

                cmd.performOneCommand(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_PLACKETTBURMAN + " measurements:" + ndPlackettBurmanNTextBox.Text.Trim() + " level:" + ndPlackettBurmanLevelTextBox.Text.Trim());
                numSelected = true;
            }

            learningErrorLabel.Visible = false;

            return numSelected && binarySelected;
        }
    }
}