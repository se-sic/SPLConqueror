using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace SPLConqueror_Core
{
    public class ConfigurationReader
    {

        /// <summary>
        /// This method returns a list of all configurations stored in a given file. All options of the configurations have to be defined in the variability model.
        /// 
        /// If the file is a xml file, it should be structured as follows:
        /// 
        /// <![CDATA[ <results> ]]>
        /// <![CDATA[   <row> ]]>
        /// <![CDATA[       <data column="Configuration"> ]]>
        /// <![CDATA[           binaryOption1, binaryOption3,... ]]>
        /// <![CDATA[       </data> ]]>
        /// <![CDATA[       <data column="Variable Features"> ]]>
        /// <![CDATA[           numOption1;64,numOption2;4 ]]>
        /// <![CDATA[       </data> ]]>
        /// <![CDATA[       <data column="Performance"> ]]>
        /// <![CDATA[           21.178 ]]>
        /// <![CDATA[       </data> ]]>
        /// <![CDATA[       <data column="Footprint"> ]]>
        /// <![CDATA[           1679 ]]>
        /// <![CDATA[       </data> ]]>
        /// <![CDATA[   </row> ]]>
        /// <![CDATA[   <row> ]]>
        /// <![CDATA[       <data column="Configuration"> ]]>
        /// <![CDATA[   .... ]]>
        /// <![CDATA[ <results> ]]>
        /// 
        ///
        /// If the file is a csv file, two different formats are supported. 
        /// 
        /// For the first on, the file should have a header: 
        /// 
        /// binaryOption1;numOption1;binaryOption2;numOption2;Performance;Footprint
        /// true;64;false;4;21.178;1679
        /// 
        /// Note: If a binary option is selected in a configuration, the value can either be "true" or "1".  
        /// 
        /// In the second format, no header is needed and only binary options are supported. Additionally, the measurements have to consider only one non-functional property.
        /// 
        /// </summary>
        /// <param name="file">Full qualified file name.</param>
        /// <param name="model">Variability model of the configurations.</param>
        /// <returns></returns>
        public static List<Configuration> readConfigurations(string file, VariabilityModel model)
        {

            if (file.EndsWith(".xml"))
            {
                XmlDocument dat = new System.Xml.XmlDocument();
                dat.Load(file);
                return readConfigurations(dat, model);
            }
            else
            {
                return readCSV(file, model);
            }

        }

        // The default symbols for multiple measurements of the same configuration and nfp.
        private static char decimalDelimiter = '.';
        private static char separator = ',';

        private const string decimalDelimiterTag = "decimalDelimiter";
        private const string separatorTag = "separator";

        /// <summary>
        /// This method returns a list of all configurations stored in a given file. All options of the configurations have to be defined in the variability model. 
        /// </summary>
        /// <param name="dat">Object representing the configuration file.</param>
        /// <param name="model">Variability model of the configurations.</param>
        /// <returns>Returns a list of configurations that were defined in the XML document. Can be an empty list.</returns>
        public static List<Configuration> readConfigurations(XmlDocument dat, VariabilityModel model)
        {
            XmlElement currentElemt = dat.DocumentElement;

            // Retrieve the decimal delimiter and the separator sign if included
            if (currentElemt.HasAttribute(decimalDelimiterTag) && currentElemt.HasAttribute(separatorTag))
            {
                // I assume that the decimal delimiter as well as the separator are only one symbol
                ConfigurationReader.decimalDelimiter = currentElemt.GetAttribute(decimalDelimiterTag)[0];
                ConfigurationReader.separator = currentElemt.GetAttribute(separatorTag)[0];

                if (currentElemt.GetAttribute(decimalDelimiterTag).Length > 1 || currentElemt.GetAttribute(separatorTag).Length > 1)
                {
                    GlobalState.logError.log("The decimal delimiter and the separator must consist of only one symbol.");
                }
                if (ConfigurationReader.decimalDelimiter == ConfigurationReader.separator)
                {
                    GlobalState.logError.log("The decimal delimiter symbol and the separator symbol must be different.");
                }
            }
            else if (currentElemt.HasAttribute(decimalDelimiterTag))
            {
                ConfigurationReader.decimalDelimiter = currentElemt.GetAttribute(decimalDelimiterTag)[0];
                if (currentElemt.GetAttribute(decimalDelimiterTag).Length > 1)
                {
                    GlobalState.logError.log("The decimal delimiter must consist of only one symbol.");
                }
            }
            else if (currentElemt.HasAttribute(separatorTag))
            {
                ConfigurationReader.separator = currentElemt.GetAttribute(separatorTag)[0];
                if (currentElemt.GetAttribute(separatorTag).Length > 1)
                {
                    GlobalState.logError.log("The separator symbol must be different.");
                }
            }

            HashSet<Configuration> configurations = new HashSet<Configuration>();

            int numberOfConfigs = currentElemt.ChildNodes.Count;
            int configsWithTooLargeDeviation = 0;
            foreach (XmlNode node in currentElemt.ChildNodes)
            {
                bool readMultipleMeasurements = false;
                if (node.Attributes.Count > 0 && node.Attributes[0].Value.ToLower() == "true")
                {
                    readMultipleMeasurements = true;
                }
                Dictionary<NFProperty, double> propertiesForConfig = new Dictionary<NFProperty, double>(); ;
                bool alternativeFormat = false;
                string binaryString = "";
                string numericString = "";
                string configID = "";
                Dictionary<NFProperty, double> measuredProperty = new Dictionary<NFProperty, double>();
                Configuration c = null;
                bool hasSetConfig = false;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (c == null && hasSetConfig)
                        continue;
                    switch (childNode.Attributes[0].Value)
                    {
                        // TODO we use this to support result files having the old structure
                        case "Configuration":
                            binaryString = childNode.InnerText;
                            break;
                        case "Variable Features":
                            numericString = childNode.InnerText;
                            break;

                        case "BinaryOptions":
                            binaryString = childNode.InnerText;
                            break;
                        case "NumericOptions":
                            numericString = childNode.InnerText;
                            break;
                        case "ConfigID":
                            if (readMultipleMeasurements)
                            {
                                configID = childNode.InnerText.Replace("_", "%;%");
                            }
                            else
                                configID = childNode.InnerText;
                            if (configID.Contains("%;%seek") && configID.Contains("%;%seek0") == false)
                            {
                                hasSetConfig = true;
                                c = null;
                                break;
                            }
                            alternativeFormat = true;
                            c = Configuration.createFromHashString(configID, GlobalState.varModel);
                            hasSetConfig = true;
                            break;
                        case "CompilerOptions":
                            //todo
                            break;
                        case "ConfigFileOptions":
                            //todo
                            break;
                        case "ParameterOptions":
                            //todo
                            break;
                        case "ProgramName":
                            //todo
                            break;
                        case "StartupBegin":
                            //todo
                            break;
                        case "StartupEnd":
                            //todo
                            break;
                        default:
                            NFProperty property = GlobalState.getOrCreateProperty(childNode.Attributes[0].Value);
                            double measuredValue = 0;
                            //-1 means that measurement failed... 3rd values strongly devigates in C.'s measurements, hence we use it only in case we have no other measurements
                            if (readMultipleMeasurements)
                            {
                                //if (property.Name != "run-real")
                                //    continue;
                                String[] m = childNode.InnerText.ToString().Split(separator);
                                double val1 = 0;
                                if (!Double.TryParse(m[0], out val1))
                                    break;
                                if (m.Length > 1)
                                {
                                    List<double> values = new List<double>();
                                    double avg = 0;
                                    foreach (var i in m)
                                    {
                                        double d = Convert.ToDouble(i.Replace(decimalDelimiter, '.'));
                                        if (d != -1)
                                        {
                                            values.Add(d);
                                            avg += d;
                                        }
                                    }
                                    if (values.Count == 0)
                                    {
                                        configsWithTooLargeDeviation++;
                                        c = null;
                                        break;
                                    }
                                    avg = avg / values.Count;
                                    /* foreach (var d in values)
                                        {
                                            if ((d / avg) * 100 > 10)
                                            {
                                                configsWithTooLargeDeviation++;
                                                c = null;
                                                break;
                                            }
                                        }*/
                                    measuredValue = avg;
                                    /*double val2 = Convert.ToDouble(m[1]);
                                        if (val1 == -1)
                                            measuredValue = val2;
                                        else if (val1 == -1 && val2 == -1)
                                            measuredValue = Convert.ToDouble(m[2]);
                                        else if (val2 == -1)
                                            measuredValue = val1;
                                        else
                                            measuredValue = (val1 + val2) / 2;*/
                                }
                                else
                                    measuredValue = val1;
                            }
                            else
                                measuredValue = Convert.ToDouble(childNode.InnerText.ToString().Replace(decimalDelimiter, '.'));

                            // Save the largest measured value.
                            double currentMaxMeasuredValue;
                            if (GlobalState.allMeasurements.maxMeasuredValue.TryGetValue(property, out currentMaxMeasuredValue))
                            {
                                if (Math.Abs(measuredValue) > Math.Abs(currentMaxMeasuredValue))
                                {
                                    GlobalState.allMeasurements.maxMeasuredValue[property] = measuredValue;
                                }
                            }
                            else
                            {
                                GlobalState.allMeasurements.maxMeasuredValue.Add(property, measuredValue);
                            }

                            // Add measured value to the configuration.
                            if (alternativeFormat && c != null)
                            {
                                c.setMeasuredValue(property, measuredValue);
                            }
                            else
                            {
                                measuredProperty.Add(property, measuredValue);
                            }
                            break;
                    }
                }

                if (alternativeFormat && c != null)
                {
                    if (configurations.Contains(c))
                    {
                        GlobalState.logError.logLine("Mutiple definition of one configuration in the configurations file:  " + c.ToString());
                    }
                    else
                    {
                        // if (GlobalState.currentNFP != null && c.nfpValues.Keys.Contains(GlobalState.currentNFP) && c.nfpValues[GlobalState.currentNFP] != -1)
                        configurations.Add(c);
                    }
                cont: { }
                    continue;
                }

                Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptions = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

                string[] binaryOptionNames = binaryString.Split(',');
                foreach (string binaryOptionName in binaryOptionNames)
                {
                    string currOption = binaryOptionName.Trim();
                    if (currOption.Length > 0)
                    {
                        BinaryOption bOpt = null;

                        bOpt = model.getBinaryOption(currOption);

                        if (bOpt == null)
                            GlobalState.logError.logLine("No Binary option found with name: " + currOption);
                        binaryOptions.Add(bOpt, BinaryOption.BinaryValue.Selected);
                    }
                }

                // Add "root" binary option to the configuration
                if (!binaryOptions.ContainsKey(model.Root))
                    binaryOptions.Add(model.Root, BinaryOption.BinaryValue.Selected);


                Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();
                if (!string.IsNullOrEmpty(numericString))
                {
                    string[] numOptionArray = numericString.Trim().Split(',');
                    foreach (string numOption in numOptionArray)
                    {
                        string[] numOptionsKeyValue;
                        if (numOption.Contains(";"))
                            numOptionsKeyValue = numOption.Split(';');
                        else
                            numOptionsKeyValue = numOption.Split(' ');// added for rc-lookahead 40
                        numOptionsKeyValue[0] = numOptionsKeyValue[0].Trim();
                        if (numOptionsKeyValue[0].Length == 0)
                            continue;
                        NumericOption varFeat = model.getNumericOption(numOptionsKeyValue[0]);
                        if (varFeat == null)
                            GlobalState.logError.logLine("No numeric option found with name: " + numOptionsKeyValue[0]);
                        double varFeatValue = Convert.ToDouble(numOptionsKeyValue[1]);

                        numericOptions.Add(varFeat, varFeatValue);
                    }
                }

                Configuration config = new Configuration(binaryOptions, numericOptions, measuredProperty);

                //if(configurations.Contains(config))
                //{
                //    GlobalState.logError.log("Mutiple definition of one configuration in the configurations file:  " + config.ToString());
                //}else
                //{
                configurations.Add(config);
                //}
            }
            GlobalState.logInfo.logLine("Configs with too large deviation: " + configsWithTooLargeDeviation);
            return configurations.ToList();
        }


        /// <summary>
        /// Returns the of list of non function properties (nfps) measured for the configurations of the given file. To tune up performance, 
        /// we consider only the first configurtion of the file and assume that all configurations have the same nfps. 
        /// </summary>
        /// <param name="file">The xml file consisting of configurations.</param>
        /// <returns>The list of nfps the configurations have measurements.</returns>
        public static List<NFProperty> propertiesOfConfigurations(string file)
        {
            List<NFProperty> properties = new List<NFProperty>();

            XmlDocument dat = new System.Xml.XmlDocument();
            dat.Load(file);
            XmlElement currentElemt = dat.DocumentElement;
            XmlNode node = currentElemt.ChildNodes[0];

            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Attributes[0].Value)
                {
                    // TODO we use this to support result files of the old structure
                    case "Configuration":
                        break;
                    case "Variable Features":
                        break;


                    case "BinaryOptions":
                        break;
                    case "NumericOptions":
                        break;
                    default:
                        NFProperty property = new NFProperty(childNode.Attributes[0].Value);
                        properties.Add(property);
                        break;
                }
            }
            return properties;
        }

        /// <summary>
        /// This method reads all configurations specified in the .csv file. In this mehtod, we assume that the file has a header. 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="model">The variabiliy model for the configurations.</param>
        /// <returns>A list of all configurations </returns>
        public static List<Configuration> readConfigurations_Header_CSV(String file, VariabilityModel model)
        {
            List<Configuration> configurations = new List<Configuration>();

            StreamReader sr = new StreamReader(file);
            
            String[] optionOrder = new String[model.getOptions().Count - 1];
            String[] nfpOrder = null;

            bool isHeader = true;

            while(!sr.EndOfStream)
            {
                String[] tokens = sr.ReadLine().Split(';');

                if (isHeader)
                {
                    nfpOrder = new String[tokens.Length - optionOrder.Length];
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        String token = tokens[i];
                        if (i < optionOrder.Length)
                        {
                            optionOrder[i] = token;
                        }
                        else
                        {
                            nfpOrder[i - optionOrder.Length] = token;
                            if (!GlobalState.nfProperties.ContainsKey(token))
                            {
                                GlobalState.nfProperties.Add(token, new NFProperty(token));
                            }
                        }
                    }
                    isHeader = false;
                }
                else
                {
                    Dictionary<BinaryOption, BinaryOption.BinaryValue> binOptions = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
                    Dictionary<NumericOption, double> numOptions = new Dictionary<NumericOption, double>();
                    Dictionary<NFProperty, double> properties = new Dictionary<NFProperty, double>();

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        String token = tokens[i];
                        if (i < optionOrder.Length)
                        {
                            ConfigurationOption option = model.getOption(optionOrder[i]);
                            if (option.GetType() == typeof(BinaryOption))
                            {
                                if (token.Equals("true") || token.Equals("1"))
                                    binOptions.Add((BinaryOption)option, BinaryOption.BinaryValue.Selected);
                                else
                                    binOptions.Add((BinaryOption)option, BinaryOption.BinaryValue.Deselected);
                            }
                            else
                            {
                                double value = Convert.ToDouble(token);
                                numOptions.Add((NumericOption)option, value);
                            }
                        }
                        else
                        {
                            NFProperty nfp = GlobalState.nfProperties[nfpOrder[i - optionOrder.Length]];
                            double value = Convert.ToDouble(token);
                            properties.Add(nfp, value);

                            double currentMaxMeasuredValue;
                            if (GlobalState.allMeasurements.maxMeasuredValue.TryGetValue(nfp, out currentMaxMeasuredValue))
                            {
                                if (Math.Abs(value) > Math.Abs(currentMaxMeasuredValue))
                                {
                                    GlobalState.allMeasurements.maxMeasuredValue[nfp] = value;
                                }
                            }
                            else
                            {
                                GlobalState.allMeasurements.maxMeasuredValue.Add(nfp, value);
                            }

                        }

                    }

                    Configuration config = new Configuration(binOptions, numOptions, properties);
                    configurations.Add(config);
                }
            }
            sr.Close();
            return configurations;
        }

        //Two formats are possible: with header and 0,1s for binary selection or no header and giving the names of config options per per line (this excludex numeric options)
        private static List<Configuration> readCSV(string file, VariabilityModel model)
        {
            StreamReader sr = new StreamReader(file);
            String line1, line2;
            if (!sr.EndOfStream)
                line1 = sr.ReadLine();
            else return null;
            if (!sr.EndOfStream)
                line2 = sr.ReadLine();
            else
                return null;
            sr.Close();
            var l1 = line1.Split(';');
            var l2 = line2.Split(';');
            if (l1.Length < 2 || l2.Length < 2)
                return null;
            int d = 0;
            if (int.TryParse(l2[0], out d))
                return readConfigurations_Header_CSV(file, model);
            else
                return readCSVBinaryOptFormat(file, model);

        }

        private static List<Configuration> readCSVBinaryOptFormat(string file, VariabilityModel model)
        {
            List<Configuration> result = new List<Configuration>();
            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(';');
                List<BinaryOption> temp = new List<BinaryOption>();
                for (int i = 0; i < line.Length - 1; i++)
                {
                    BinaryOption b = model.getBinaryOption(line[i]);
                    temp.Add(b);
                }
                double value = Double.Parse(line[line.Length - 1].Replace(',', '.'));
                var c = new Configuration(temp);
                c.setMeasuredValue(GlobalState.currentNFP, value);
                result.Add(c);

            }
            sr.Close();
            return result;
        }
    }
}
