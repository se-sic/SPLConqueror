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
        /// 
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        /*Format:
             * - <row>
                    <data columnname="Configuration">VioletDef, base,</data> 
                    <data columnname="NFProperty name">1516</data> 
                </row>*/

        public int readConfigurations(XmlDocument dat, VariabilityModel model)
        {
            //Progress Information
            ErrorLog.logError("Loading measurements...");

            int i = 0;
            int progress = 0;

            XmlElement currentElemt = dat.DocumentElement;

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
                            double measuredValue = Convert.ToDouble(childNode.InnerText.ToString().Replace('.', ','));
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

                GlobalState.addConfiguration(config);

            }
            return numberOfConfigs;
        }
    }
}
