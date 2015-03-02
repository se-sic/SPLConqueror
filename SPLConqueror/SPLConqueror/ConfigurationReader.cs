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

            List<Configuration> configurations = new List<Configuration>();

            int numberOfConfigs = currentElemt.ChildNodes.Count;
            foreach (XmlNode node in currentElemt.ChildNodes)
            {
                Dictionary<NFProperty, double> propertiesForConfig = new Dictionary<NFProperty, double>(); ;

                string binaryString = "";
                string numericString = "";
                Dictionary<NFProperty, double> measuredProperty = new Dictionary<NFProperty, double>();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Attributes[0].Value)
                    {
                        // TODO we use this to support result files having the old structure
                        case "Configuration":
                            binaryString = childNode.InnerText.ToString();
                            break;
                        case "Variable Features":
                            numericString = childNode.InnerText.ToString();
                            break;


                        case "BinaryOptions":
                            binaryString = childNode.InnerText.ToString();
                            break;
                        case "NumericOptions":
                            numericString = childNode.InnerText.ToString();
                            break;
                        default: 
                            NFProperty property = GlobalState.getOrCreateProperty(childNode.Attributes[0].Value);
                            double measuredValue = Convert.ToDouble(childNode.InnerText.ToString().Replace(',', '.'));
                            measuredProperty.Add(property, measuredValue);
                            break;
                    }
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
            return configurations;
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
