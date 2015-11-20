using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using SPLConqueror_Core;
using MachineLearning;
using MachineLearning.Learning;
using CommandLine;

namespace ScriptGenerator
{

    public partial class Form1 : Form
    {
        public const string PARAMETER_NOT_SPECIFIED = "Parameter not specified!";

        public const string CONTAINERKEY_MLSETTING = "mlSettings";
        public const string CONTAINERKEY_BINARY = "binary";
        public const string CONTAINERKEY_NUMERIC = "numeric";
        public const string CONTAINERKEY_BINARY_VALIDATION = "binary validation";
        public const string CONTAINERKEY_NUMERIC_VALIDATION = "numeric validation";
        public const string CONTAINERKEY_LOGFILE = "logFile";


        public Form1()
        {
            InitializeComponent();
            addMlSettingsBoxContent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #region MLSettings components
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
                l.Size = new System.Drawing.Size(35, 15);
                l.TabIndex = i * 2;
                l.Text = fields[i].Name;

                TextBox t = new TextBox();
                mlSettingsPanel.Controls.Add(t);

                t.Location = new System.Drawing.Point(150, 5 + ML_FIELDS_OFFSET * i);
                t.Name = fields[i].Name + "_textBox";
                t.Size = new System.Drawing.Size(150, 20);
                t.TabIndex = i * 2 + 1;
                t.Text = fields[i].GetValue(settingsObject).ToString();
            }
        }

        private void AddMlSetting_Click(object sender, EventArgs e)
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
            addedElementsList.Items.Add(new Container(CONTAINERKEY_MLSETTING, setting));
        }
        #endregion

        private void removeElement_Click(object sender, EventArgs e)
        {
            if (addedElementsList.SelectedIndex != -1)
                addedElementsList.Items.RemoveAt(addedElementsList.SelectedIndex);
        }


        #region variability model
        private void addVariabilityModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(pfd.FileName);
                addedElementsList.Items.Add(new Container("variabilityModel", fi));
            }
        }
        #endregion

        #region measurement files
        private System.IO.FileInfo resultFile = null;
        private Dictionary<int, Container> variabilityModelIndexToObject = new Dictionary<int, Container>();
        private Dictionary<CheckBox, NFProperty> nfpSelection_resultFile = new Dictionary<CheckBox, NFProperty>();
        private void LocateResultFile_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (pfd.ShowDialog() == DialogResult.OK)
            {
                resultFile = new System.IO.FileInfo(pfd.FileName);


                // define the corresponding variability model
                foreach (Container c in addedElementsList.Items)
                {
                    if (c.Type == "variabilityModel")
                    {
                        variabilityModelIndexToObject.Add(defineVMforResultcheckedListBox.Items.Count, c);
                        defineVMforResultcheckedListBox.Items.Add(((System.IO.FileInfo)c.Content).Name);
                    }
                }
                // select nfps to consider
                List<NFProperty> properties = ConfigurationReader.propertiesOfConfigurations(resultFile.Directory.ToString() + Path.DirectorySeparatorChar + resultFile.Name);
                nfpSelection_resultFile = new Dictionary<CheckBox, NFProperty>();

                for (int i = 0; i < properties.Count; i++)
                {
                    CheckBox box = new CheckBox();
                    addResultFile_Group.Controls.Add(box);
                    nfpSelection_resultFile.Add(box, properties[i]);

                    box.AutoSize = true;
                    box.Location = new System.Drawing.Point(10, 90 + i * 18);
                    box.Name = properties[i].Name;
                    box.Checked = true;
                    box.Size = new System.Drawing.Size(80, 17);
                    box.TabIndex = i;
                    box.Text = properties[i].Name;
                    box.UseVisualStyleBackColor = true;
                }
            }
        }

        private void addResultFile_Click(object sender, EventArgs e)
        {
            if (resultFile == null)
            {
                informatioLabel.Text = "No configuration file specified!";
                return;
            }


            // identify selected nfps
            List<NFProperty> selectedProperties = new List<NFProperty>();
            foreach (KeyValuePair<CheckBox, NFProperty> cBox in nfpSelection_resultFile)
            {
                if (cBox.Key.Checked == true)
                    selectedProperties.Add(cBox.Value);
            }
            if (selectedProperties.Count == 0)
            {
                informatioLabel.Text = "No non-functional property selected!";
                return;
            }

            // identify selected variability models
            List<Container> selectedVariabilityModelContainers = new List<Container>();
            foreach (int indice in defineVMforResultcheckedListBox.SelectedIndices)
            {
                selectedVariabilityModelContainers.Add(variabilityModelIndexToObject[indice]);
            }
            if (selectedVariabilityModelContainers.Count == 0)
            {
                informatioLabel.Text = "No variability model selected!";
                return;
            }

            // add result file to the information of the variability model. 
            Container measurements = new ScriptGenerator.Container("measurements", resultFile);
            measurements.addAdditionalInformation(new Container("nfpsToConsider", selectedProperties));
            foreach (Container c in selectedVariabilityModelContainers)
            {
                c.addAdditionalInformation(measurements);
            }
        }
        #endregion

        #region binary sampling heuristics
        private void bsamp_addButton_Click(object sender, EventArgs e)
        {
            string containerKey = "binary ";
            string keyInfo = "";
            List<string> samplingNames = new List<string>();
            string validation = "";

            if (bsamp_ForValidation.Checked)
            {
                containerKey += "validation";
                validation = "validation";
            }

            if (bsamp_FW_box.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_SAMPLE_OPTIONWISE+" " + validation);
                keyInfo += "FW ";
            }
            if (bsamp_PW_box.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_SAMPLE_PAIRWISE+" " + validation);
                keyInfo += "PW ";
            }
            if (bsamp_negFW_box.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE+" " + validation);
                keyInfo += "negFW ";
            }
            if (bsamp_all_box.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_SAMPLE_ALLBINARY+" " + validation);
                keyInfo += "all ";
            }
            if (bsamp_random_box.Checked)
            {
                // TODO text of the textField should contain numeric characters only.
                samplingNames.Add(CommandLine.Commands.COMMAND_SAMPLE_BINARY_RANDOM+" " + bsamp_random_textBox.Text + " " + bsamp_random__modulo_textBox.Text + " " + validation);
                keyInfo += "random " + bsamp_random_textBox.Text;
            }
            Container cont = new Container(containerKey, samplingNames);
            cont.AdditionalKeyInformation = keyInfo;
            addedElementsList.Items.Add(cont);
        }

        #endregion

        #region Experimental Designs
        private void expDesign_addButton_Click(object sender, EventArgs e)
        {
            StringBuilder containerKey = new StringBuilder();
            containerKey.Append(CONTAINERKEY_NUMERIC).Append(" ");
            string keyInfo = "";
            List<string> samplingNames = new List<string>();
            string validation = "";

            if (num_forValidationCheckBox.Checked)
            {
                validation = CommandLine.Commands.COMMAND_VALIDATION;
                containerKey.Append(CommandLine.Commands.COMMAND_VALIDATION);
            }

            if (num_BoxBehnken_check.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_BOXBEHNKEN+ " " + validation);
                keyInfo += "BoxBehnken ";
            }
            if (num_CentralComposite_check.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE + " " + validation);
                keyInfo += "CentralComposite ";
            }
            if (num_FullFactorial_check.Checked)
            {
                samplingNames.Add(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_FULLFACTORIAL + " " + validation);
                keyInfo += "FullFactorial ";
            }
            if (num_hyperSampling_check.Checked)
            {
                if (num_hyper_percent_text.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                samplingNames.Add(CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_HYPERSAMPLING + " " + num_hyper_percent_text.Text + " " + validation);
                keyInfo += "HyperSampling " + num_hyper_percent_text.Text + " ";
            }
            if (num_kEx_check.Checked)
            {
                if (num_kEx_n_Box.Text.Trim() == "" || num_kEx_k_Box.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_KEXCHANGE + " sampleSize:" + num_kEx_n_Box.Text.Trim() + " k:" + num_kEx_k_Box.Text.Trim();
                samplingNames.Add(str + " " + validation);
                keyInfo += str + " ";
            }
            if (num_randomSampling_num.Checked)
            {
                if (num_random_n_Text.Text.Trim() == "" || num_rand_seed_Text.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_RANDOM + " sampleSize:" + num_random_n_Text.Text.Trim() + " seed:" + num_rand_seed_Text.Text.Trim();
                samplingNames.Add(str + " " + validation);
                keyInfo += str + " ";
            }

            if (num_oneFactorAtATime_Box.Checked)
            {
                if (num_oneFactorAtATime_num_Text.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_ONEFACTORATATIME + " distinctValuesPerOption:" + num_oneFactorAtATime_num_Text.Text.Trim();
                samplingNames.Add(str + " " + validation);
                keyInfo += str + " ";

            }
            if (num_PlackettBurman_check.Checked)
            {
                if (num_Plackett_Level_Box.Text.Trim() == "" || num_Plackett_n_Box.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = CommandLine.Commands.COMMAND_EXERIMENTALDESIGN + " " + CommandLine.Commands.COMMAND_EXPDESIGN_PLACKETTBURMAN + " measurements:" + num_Plackett_n_Box.Text.Trim() + " level:" + num_Plackett_Level_Box.Text.Trim();
                samplingNames.Add(str + " " + validation);
                keyInfo += str + " ";
            }

            Container cont = new Container(containerKey.ToString(), samplingNames);
            cont.AdditionalKeyInformation = keyInfo;

            addedElementsList.Items.Add(cont);

        }
        #endregion



        private void GenerateScript_Click(object sender, EventArgs e)
        {
            List<MachineLearning.Learning.ML_Settings> mlSettings = new List<MachineLearning.Learning.ML_Settings>();
            Dictionary<string, List<Container>> runs = new Dictionary<string, List<Container>>();

            foreach (Container c in addedElementsList.Items)
            {

                switch (c.Type.Trim())
                {
                    case CONTAINERKEY_MLSETTING:
                        mlSettings.Add((MachineLearning.Learning.ML_Settings)c.Content);
                        break;
                    default:
                        if (runs.ContainsKey(c.Type.Trim()))
                        {
                            runs[c.Type.Trim()].Add(c);
                        }
                        else
                        {
                            List<Container> newKind = new List<Container>();
                            newKind.Add(c);
                            runs.Add(c.Type.Trim(), newKind);
                        }
                        break;
                }
            }

            generateScript(mlSettings, runs);


        }

        /// <summary>
        /// variability model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateScript(List<MachineLearning.Learning.ML_Settings> mlSettings, Dictionary<string, List<ScriptGenerator.Container>> runs)
        {
            if (mlSettings.Count == 0)
            {
                informatioLabel.Text = "No mlSettings specified!";
                return;
            }

            StringBuilder scriptContent = new StringBuilder();

            foreach (Container c in addedElementsList.Items)
            {

                switch (c.Type.Trim())
                {
                    case CONTAINERKEY_LOGFILE:
                        scriptContent.Append(CommandLine.Commands.COMMAND_LOG+" " + (c.Content) + "\n");
                        break;
                }
            }



            foreach (ML_Settings setting in mlSettings)
            {
                scriptContent.Append(mlSettingsContent(setting));

                foreach (Container varModelContainer in runs["variabilityModel"])
                {
                    foreach (Container measurementContainer in varModelContainer.AdditionalInformation)
                    {
                        List<Container> nfpContainer = measurementContainer.AdditionalInformation;

                        foreach (Container nfp in nfpContainer)
                        {
                            List<NFProperty> prop = (List<NFProperty>)nfp.Content;

                            foreach (NFProperty pro in prop)
                            {

                                System.IO.FileInfo varModel = (System.IO.FileInfo)varModelContainer.Content;
                                System.IO.FileInfo measurement = (System.IO.FileInfo)measurementContainer.Content;
                                NFProperty nfpName = (NFProperty)pro;


                                scriptContent.Append(Commands.COMMAND_VARIABILITYMODEL + " " + varModel + System.Environment.NewLine);
                                scriptContent.Append(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurement + System.Environment.NewLine);
                                scriptContent.Append(Commands.COMMAND_SET_NFP + " " + pro.Name + System.Environment.NewLine);
                                scriptContent.Append(samplingsToConsider(runs, varModel.Directory.FullName));
                            }
                        }
                    }
                }
                scriptContent.Append(Commands.COMMAND_CLEAR_LEARNING + System.Environment.NewLine);
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Script";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();

                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(scriptContent.ToString());
                s.Flush();
                fs.Close();
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runs"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private string samplingsToConsider(Dictionary<string, List<ScriptGenerator.Container>> runs, String directory)
        {
            runs = enrichSamplings(runs);


            StringBuilder sb = new StringBuilder();
            foreach (Container cBSamp in runs[CONTAINERKEY_BINARY])
            {
                string binarySamplingString = "";
                if (cBSamp.Content != null)
                {
                    List<string> samplingNamesBinary = (List<string>)cBSamp.Content;
                    foreach (string samplingBinary in samplingNamesBinary)
                    {
                        binarySamplingString += (samplingBinary + System.Environment.NewLine);
                    }
                }
                foreach (Container cNumeric in runs[CONTAINERKEY_NUMERIC])
                {
                    string numericSamplingString = "";
                    if (cNumeric.Content != null)
                    {
                        List<string> samplingNamesNumeric = (List<string>)cNumeric.Content;
                        foreach (string samplingNumeric in samplingNamesNumeric)
                        {
                            numericSamplingString += (samplingNumeric + System.Environment.NewLine);
                        }
                    }
                    foreach (Container cBinaryValid in runs[CONTAINERKEY_BINARY_VALIDATION])
                    {
                        string binaryValidSamplingString = "";

                        if (cBinaryValid.Content != null)
                        {
                            List<string> samplingNamesBinaryValid = (List<string>)cBinaryValid.Content;
                            foreach (string samplingBinaryValid in samplingNamesBinaryValid)
                            {
                                binaryValidSamplingString += (samplingBinaryValid + System.Environment.NewLine);
                            }
                        }
                        foreach (Container cNumericValid in runs[CONTAINERKEY_NUMERIC_VALIDATION])
                        {
                            string numericValidSamplingString = "";

                            if (cNumericValid.Content != null)
                            {
                                List<string> samplingNamesNumericValid = (List<string>)cNumericValid.Content;
                                foreach (string samplingNumericValid in samplingNamesNumericValid)
                                {
                                    numericValidSamplingString += (samplingNumericValid + System.Environment.NewLine);
                                }
                            }

                            sb.Append(binarySamplingString);
                            sb.Append(numericSamplingString);
                            sb.Append(binaryValidSamplingString);
                            sb.Append(numericValidSamplingString);
                            if (print_button.Checked)
                            {
                                string fileName = binarySamplingString + "_" + numericSamplingString + "_" + binaryValidSamplingString + "_" + numericValidSamplingString + ".txt";
                                fileName = fileName.Replace(System.Environment.NewLine, "-");
                                fileName = fileName.Replace(" ", "");
                                fileName = fileName.Replace(":", "-");

                                sb.Append(Commands.COMMAND_PRINT_CONFIGURATIONS + " " + directory + Path.DirectorySeparatorChar + fileName + " prefix suffix" + System.Environment.NewLine);
                                sb.Append(Commands.COMMAND_CLEAR_SAMPLING + System.Environment.NewLine);
                            }
                            if (learn_button.Checked)
                            {
                                sb.Append(Commands.COMMAND_START_LEARNING + System.Environment.NewLine);
                                sb.Append(Commands.COMMAND_ANALYZE_LEARNING + System.Environment.NewLine);
                                sb.Append(Commands.COMMAND_CLEAR_SAMPLING + System.Environment.NewLine);
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private Dictionary<string, List<ScriptGenerator.Container>> enrichSamplings(Dictionary<string, List<ScriptGenerator.Container>> runs)
        {
            if(!runs.ContainsKey(CONTAINERKEY_BINARY))
            {
                runs.Add(CONTAINERKEY_BINARY, new List<Container>() { new Container() });
            }
            if (!runs.ContainsKey(CONTAINERKEY_BINARY_VALIDATION))
            {
                runs.Add(CONTAINERKEY_BINARY_VALIDATION, new List<Container>() { new Container() });
            }
            if (!runs.ContainsKey(CONTAINERKEY_NUMERIC))
            {
                runs.Add(CONTAINERKEY_NUMERIC, new List<Container>() { new Container() });
            }
            if (!runs.ContainsKey(CONTAINERKEY_NUMERIC_VALIDATION))
            {
                runs.Add(CONTAINERKEY_NUMERIC_VALIDATION, new List<Container>() { new Container() });
            }

            return runs;
        }

    


        private string mlSettingsContent(ML_Settings settings)
        {
            return CommandLine.Commands.COMMAND_LOAD_MLSETTINGS+" " + settings.ToString();
        }

        private void logFile_Button_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Define log file";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {                
                // TODO what happen if there are multiple logFiles defined?
                addedElementsList.Items.Add(new Container(CONTAINERKEY_LOGFILE, saveFileDialog1.FileName));

            }
        }


    }
}
