using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using SPLConqueror_Core;

using MachineLearning;
using MachineLearning.Learning;
using System.IO;

namespace ScriptGenerator
{

    public partial class Form1 : Form
    {
        public const string PARAMETER_NOT_SPECIFIED = "Parameter not specified!"; 

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
                l.Name = fields[i].Name+"_label";
                l.Size = new System.Drawing.Size(35, 15);
                l.TabIndex = i*2;
                l.Text = fields[i].Name;

                TextBox t = new TextBox();
                mlSettingsPanel.Controls.Add(t);

                t.Location = new System.Drawing.Point(150, 5 + ML_FIELDS_OFFSET * i);
                t.Name = fields[i].Name + "_textBox";
                t.Size = new System.Drawing.Size(150, 20);
                t.TabIndex = i*2+1;
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
            addedElementsList.Items.Add(new Container("mlSettings",setting));
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
                // TODO replace "\\" with the path separator
                List<NFProperty> properties = ConfigurationReader.propertiesOfConfigurations(resultFile.Directory+"\\"+resultFile.Name);
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
                samplingNames.Add("featureWise " + validation);
                keyInfo += "FW ";
            }
            if (bsamp_PW_box.Checked)
            {
                samplingNames.Add("pairWise " + validation);
                keyInfo += "PW ";
            }
            if (bsamp_negFW_box.Checked)
            {
                samplingNames.Add("negFW " + validation);
                keyInfo += "negFW ";
            }
            if (bsamp_all_box.Checked)
            {
                samplingNames.Add("allBinary " + validation);
                keyInfo += "all ";
            }
            if (bsamp_random_box.Checked)
            {
                // TODO text of the textField should contain numeric characters only.
                samplingNames.Add("random " + bsamp_random_textBox.Text + " " + bsamp_random__modulo_textBox.Text + " " + validation);
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
            string containerKey = "numeric ";
            string keyInfo = "";
            List<string> samplingNames = new List<string>();
            string validation = "";

            if (num_forValidationCheckBox.Checked)
            {
                validation = "validation";
                containerKey += "validation";
            }

            if (num_BoxBehnken_check.Checked)
            {
                samplingNames.Add("expDesign BoxBehnken " + validation);
                keyInfo += "BoxBehnken ";
            }
            if (num_CentralComposite_check.Checked)
            {
                samplingNames.Add("expDesign CentralComposite " + validation);
                keyInfo += "CentralComposite ";
            }
            if (num_FullFactorial_check.Checked)
            {
                if (num_fullFac_percentText.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                samplingNames.Add("expDesign FullFactorial " + num_fullFac_percentText.Text + " " + validation);
                keyInfo += "FullFactorial " + num_fullFac_percentText.Text + " ";

            }
            if (num_hyperSampling_check.Checked)
            {
                if (num_hyper_percent_text.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                samplingNames.Add("expDesign HyperSampling " + num_hyper_percent_text.Text + " " + validation);
                keyInfo += "HyperSampling " + num_hyper_percent_text.Text + " ";
            }
            if (num_kEx_check.Checked)
            {
                if (num_kEx_n_Box.Text.Trim() == "" || num_kEx_k_Box.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = "expDesign kExchange sampleSize:" + num_kEx_n_Box.Text.Trim() + " k:" + num_kEx_k_Box.Text.Trim();
                samplingNames.Add(str+" "+validation );
                keyInfo += str + " ";
            }
            if (num_randomSampling_num.Checked)
            {
                if (num_random_n_Text.Text.Trim() == "" || num_rand_seed_Text.Text.Trim() == "")
                {
                    informatioLabel.Text = PARAMETER_NOT_SPECIFIED;
                    return;
                }
                string str = "expDesign random sampleSize:" + num_random_n_Text.Text.Trim() + " seed:"+num_rand_seed_Text.Text.Trim();
                samplingNames.Add(str+" "+validation );
                keyInfo += str + " ";
            }

            // TODO add PlackettBurman

            Container cont = new Container(containerKey, samplingNames);
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
                    case "mlSettings":
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
            if(mlSettings.Count == 0)
            {
                informatioLabel.Text = "No mlSettings specified!";
                return;
            }

            StringBuilder scriptContent = new StringBuilder();


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
                            List<NFProperty> prop = (List < NFProperty >) nfp.Content;

                            foreach (NFProperty pro in prop)
                            {

                                System.IO.FileInfo varModel = (System.IO.FileInfo)varModelContainer.Content;
                                System.IO.FileInfo measurement = (System.IO.FileInfo)measurementContainer.Content;
                                NFProperty nfpName = (NFProperty)pro;


                                scriptContent.Append("vm "+varModel + System.Environment.NewLine);
                                scriptContent.Append("all " + measurement + System.Environment.NewLine);
                                scriptContent.Append("nfp " + pro.Name + System.Environment.NewLine);
                                scriptContent.Append(samplingsToConsider(runs));
                            }
                        }
                    }
                }
                scriptContent.Append("clear-learning");
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
        /// numeric
        /// numeric validation
        /// binary 
        /// binary validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string samplingsToConsider(Dictionary<string, List<ScriptGenerator.Container>> runs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Container cBSamp in runs["binary"])
            {
                List<string> samplingNamesBinary = (List<string>)cBSamp.Content;
                string binarySamplingString = "";
                foreach (string samplingBinary in samplingNamesBinary)
                {
                    binarySamplingString += (samplingBinary + System.Environment.NewLine);
                }

                foreach (Container cNumeric in runs["numeric"])
                {
                    List<string> samplingNamesNumeric = (List<string>)cNumeric.Content;
                    string numericSamplingString = "";
                    foreach (string samplingNumeric in samplingNamesNumeric)
                    {
                        numericSamplingString += (samplingNumeric + System.Environment.NewLine);
                    }

                    foreach (Container cBinaryValid in runs["binary validation"])
                    {
                        List<string> samplingNamesBinaryValid = (List<string>)cBinaryValid.Content;
                        string binaryValidSamplingString = "";
                        foreach (string samplingBinaryValid in samplingNamesBinaryValid)
                        {
                            binaryValidSamplingString += (samplingBinaryValid + System.Environment.NewLine);
                        }

                        foreach (Container cNumericValid in runs["numeric validation"])
                        {
                            List<string> samplingNamesNumericValid = (List<string>)cNumericValid.Content;
                            string numericValidSamplingString = "";
                            foreach (string samplingNumericValid in samplingNamesNumericValid)
                            {
                                numericValidSamplingString += (samplingNumericValid + System.Environment.NewLine);
                            }

                            sb.Append(binarySamplingString);
                            sb.Append(numericSamplingString);
                            sb.Append(binaryValidSamplingString);
                            sb.Append(numericValidSamplingString);
                            sb.Append("start" + System.Environment.NewLine);
                            sb.Append("clean-sampling" + System.Environment.NewLine);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private string variabilityModelWithMeasurementFile(ScriptGenerator.Container c, ScriptGenerator.Container measurement)
        {
            throw new NotImplementedException();
        }


        private string mlSettingsContent(ML_Settings settings)
        {
            return "mlSettings " + settings.ToString();
        }


    }
}
