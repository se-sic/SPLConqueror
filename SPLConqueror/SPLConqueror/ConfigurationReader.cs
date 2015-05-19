using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    public class ConfigurationReader
    {

        /// <summary>
        /// This method returns a list of all configurations stored in a given file. All options of the configurations have to be defined in the variability model.
        /// 
        /// The file should be structured as follows:
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
        /// </summary>
        /// <param name="dat">Object representing the configuration file.</param>
        /// <param name="model">Variability model of the configurations.</param>
        /// <returns></returns>
        public static List<Configuration> readConfigurations(string file, VariabilityModel model)
        {
            XmlDocument dat = new System.Xml.XmlDocument();
            dat.Load(file);
            return readConfigurations(dat, model);
        }
        
        /// <summary>
        /// This method returns a list of all configurations stored in a given file. All options of the configurations have to be defined in the variability model. 
        /// </summary>
        /// <param name="dat">Object representing the configuration file.</param>
        /// <param name="model">Variability model of the configurations.</param>
        /// <returns>Returns a list of configurations that were defined in the XML document. Can be an empty list.</returns>
        public static List<Configuration> readConfigurations(XmlDocument dat, VariabilityModel model)
        {
            
            
            XmlElement currentElemt = dat.DocumentElement;

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
                                configID = childNode.InnerText.Replace("_","%;%");
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
                                if (property.Name != "run-real")
                                    continue;
                                String[] m = childNode.InnerText.ToString().Split(',');
                                double val1 = 0;
                                if(!Double.TryParse(m[0], out val1))
                                    break;
                                if (m.Length > 1)
                                {
                                    List<double> values = new List<double>();
                                    double avg = 0;
                                    foreach (var i in m)
                                    {
                                        double d = Convert.ToDouble(i);
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
                                measuredValue = Convert.ToDouble(childNode.InnerText.ToString().Replace(',', '.'));
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
                        GlobalState.logError.log("Mutiple definition of one configuration in the configurations file:  " + c.ToString());
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
                            GlobalState.logError.log("No Binary option found with name: " + currOption);
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
                            GlobalState.logError.log("No numeric option found with name: " + numOptionsKeyValue[0]);
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
            GlobalState.logInfo.log("Configs with too large deviation: " + configsWithTooLargeDeviation);
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
    }
}
