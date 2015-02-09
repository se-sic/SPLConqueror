using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    class ConfigurationReader
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
        public List<Configuration> readConfigurations(string file, VariabilityModel model)
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
        /// <returns></returns>
        public List<Configuration> readConfigurations(XmlDocument dat, VariabilityModel model)
        {
            //Progress Information
            ErrorLog.logError("Loading measurements...");

            int i = 0;
            int progress = 0;

            XmlElement currentElemt = dat.DocumentElement;

            List<Configuration> configurations = new List<Configuration>();

            int numberOfConfigs = currentElemt.ChildNodes.Count;
            foreach (XmlNode node in currentElemt.ChildNodes)
            {
                i++;
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

                string[] binaryFeatures = binaryString.Split(',');
                foreach (string element in binaryFeatures)
                {
                    string searchelement = element.Trim();
                    if (searchelement.Length > 0)
                    {
                        BinaryOption bOpt = null;

                        bOpt = GlobalState.varModel.getBinaryOption(searchelement);

                        if (bOpt == null)
                            ErrorLog.logError("No Binary option found with name: "+searchelement);
                        binaryOptions.Add(bOpt, BinaryOption.BinaryValue.Selected);
                    }
                }

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
                        NumericOption varFeat = GlobalState.varModel.getNumericOption(numOptionsKeyValue[0]);
                        if (varFeat == null)
                            ErrorLog.logError("No numeric option found with name: " + numOptionsKeyValue[0]);
                        double varFeatValue = Convert.ToDouble(numOptionsKeyValue[1]);

                        numericOptions.Add(varFeat, varFeatValue);
                    }
                }

                Configuration config = new Configuration(binaryOptions, numericOptions, measuredProperty);
                
                if(configurations.Contains(config))
                {
                    ErrorLog.logError("Mutiple definition of one configuration in the configurations file:  "+config.ToString());
                }else
                {
                    configurations.Add(config);
                }
            }
            return configurations;
        }

    }
}
