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
            ////Progress Information
            //HelperClass.setStatus("Loading measurements...");

            ////double sumPerformance = 0.0;

            //int i = 0;
            //int progress = 0;

            //XmlElement currentElemt = dat.DocumentElement;

            //List<List<Element>> minConfigs = new List<List<Element>>();



            //int allConfigs = currentElemt.ChildNodes.Count;
            //foreach (XmlNode node in currentElemt.ChildNodes)
            //{
            //    i++;
            //    Dictionary<NFProperty, double> propertiesForConfig = new Dictionary<NFProperty, double>(); ;
            //    string configuration = "";



            //    string variableFeatureValues = "";
            //    foreach (XmlNode childNode in node.ChildNodes)
            //    {
            //        switch (childNode.Attributes[0].Value)
            //        {
            //            case "Configuration":
            //                configuration = childNode.InnerText.ToString();
            //                break;
            //            case "Variable Features":
            //                variableFeatureValues = childNode.InnerText.ToString();
            //                break;
            //            default: // NFP Values
            //                string value = childNode.Attributes[0].Value;
            //                double measuredValue = Convert.ToDouble(childNode.InnerText.ToString().Replace('.', ','));
            //                if (nfpm.getProperty(value) != null)
            //                {
            //                    propertiesForConfig.Add(nfpm.getProperty(value), measuredValue);
            //                }
            //                // to support old measurements (before multiple nfps were supported (the nfp values are stored within the "Measured Value" Node))
            //                if (value == "Measured Value")
            //                {
            //                    propertiesForConfig.Add(nfpm.Properties.ElementAt(0), measuredValue);
            //                }
            //                break;



            //        }
            //    }
            //    List<Element> boolFeatures = new List<Element>();

            //    string[] elements = configuration.Split(',');
            //    foreach (string element in elements)
            //    {
            //        string searchelement = element.Trim();
            //        if (searchelement.Length > 0)
            //        {
            //            if (searchelement.StartsWith("dPair_") || searchelement.StartsWith("derivative_"))
            //                continue;
            //            Element tempEl = null;
            //            if (fm.genSetting.type == "Custom")
            //            {
            //                tempEl = fm.getElementByConfigParameter(searchelement);
            //                if (tempEl == null) //&& Char.IsDigit(element.Substring(element.Length-1)[0]))
            //                    tempEl = fm.getElementByNameUnsafe(searchelement);
            //            }
            //            else
            //                tempEl = fm.getElementByNameUnsafe(searchelement);
            //            if (tempEl == null) //&& Char.IsDigit(element.Substring(element.Length-1)[0]))
            //                tempEl = fm.getElementByConfigParameter(searchelement);//fm.getElementByNameUnsafe(searchelement.Replace(' ','_'));
            //            if (boolFeatures.Contains(tempEl))
            //            {
            //                // System.Windows.Forms.MessageBox.Show("Error while reading configuration. Found two identical elements");
            //                continue;
            //            }
            //            if (tempEl == null)
            //            {
            //                searchelement = "-" + searchelement;
            //                tempEl = fm.getElementByConfigParameter(searchelement);
            //                if (tempEl == null) //&& Char.IsDigit(element.Substring(element.Length-1)[0]))
            //                    tempEl = fm.getElementByNameUnsafe(searchelement);
            //            }
            //            boolFeatures.Add(tempEl);
            //        }
            //    }
            //    if (boolFeatures.Contains(null))
            //        HelperClass.printContent("Error in loading element in configuration: " + configuration);
            //    if (fm.genSetting.type == "Custom")
            //    {
            //        boolFeatures = vg.maximizeConfig(boolFeatures, fm, true, null)[0];
            //    }

            //    Dictionary<NumericOption, double> variableFeatureValueList = new Dictionary<NumericOption, double>();
            //    if (!string.IsNullOrEmpty(variableFeatureValues))
            //    {
            //        string[] varFeatureArray = variableFeatureValues.Split(',');
            //        foreach (string varFeatur in varFeatureArray)
            //        {
            //            string[] varFeatureTuple;
            //            if (varFeatur.Contains(";"))
            //                varFeatureTuple = varFeatur.Split(';');
            //            else
            //                varFeatureTuple = varFeatur.Split(' ');// added for rc-lookahead 40
            //            varFeatureTuple[0] = varFeatureTuple[0].Trim();
            //            if (varFeatureTuple[0].Length == 0)
            //                continue;
            //            NumericOption varFeat = fm.getNumericOptionsByName(varFeatureTuple[0]);
            //            if (varFeat == null)
            //                throw new NullReferenceException();
            //            double varFeatValue = Convert.ToDouble(varFeatureTuple[1]);

            //            variableFeatureValueList.Add(varFeat, varFeatValue);
            //        }
            //        List<ResultsOneVariant.MeasurementKind> kind_copy = kind.Copy();

            //    }

            //    //this.addConfigWithWorkload(config, rp, variableFeatureValueList, kind_copy, measuredValue);
            //    minConfigs.Add(boolFeatures);

            //    //new method of storing configurations
            //    Configuration config = new Configuration(boolFeatures, variableFeatureValueList, true);
            //    config.measuredValue = propertiesForConfig;
            //    this.measurements.Add(config);
            //    /*  foreach(NumericOption vf in variableFeatureValueList.Keys)
            //      {
            //          try
            //          {
            //              config.numericOptions.Add(vf, variableFeatureValueList[vf]);
            //          }
            //          catch (Exception e)
            //          {
            //              continue;
            //          }
            //      }*/
            //    //measurements.Add(config);

            //    if (i % 200 == 0)
            //    {
            //        progress = 100 * i / allConfigs;
            //        HelperClass.setProgress("Loaded " + i.ToString() + " of " + allConfigs + " measurements.", progress);
            //    }
            //    //this.min_configurations.Add(config);
            //    //this.min_values.Add(measuredValue);
            //}
            ////HelperClass.printContent("Sum of loaded performanceTimes " + sumPerformance);
            ////ResultDatabase.database.setStandardConfig(rp.getStandardConfig(minConfigs, fm));
            ////ResultDatabase.database.setStandardValue(rp.Db.getStandardConfig(), rp);
            //HelperClass.closeStatusForm();
            //return minConfigs.Count;
            return 0;
        }
    }
}
