using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    class NumericOption : ConfigurationOption
    {
        private double min_value = 0;

        public double Min_value
        {
            get { return min_value; }
            set { min_value = value; }
        }
        private double max_value = 0;

        public double Max_value
        {
            get { return max_value; }
            set { max_value = value; }
        }

        private double defaultValue = 0;

        public double DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        private InfluenceFunction stepFunction = null;

        /// <summary>
        /// A function that computes each value within the value range of that option
        /// </summary>
        internal InfluenceFunction StepFunction
        {
            get { return stepFunction; }
            set { stepFunction = value; }
        }

        /// <summary>
        /// Computes the next value of the numeric option using the step function
        /// </summary>
        /// <param name="value">Value from which the next value shall be computed</param>
        /// <returns>The next valid value within the value range of that option</returns>
        public double getNextValue(double value)
        {
            Dictionary<NumericOption, double> t = new Dictionary<NumericOption, double>();
            t.Add(this, value);
            return stepFunction.eval(t);
        }

        /// <summary>
        /// Gives the step number for a given parameter starting by 0 representing the minValue: For example, minVal = 5; maxVal = 20; stepSize = 5; getStep(15) returns 2; 
        /// Required for drawing the influence model 
        /// </summary>
        /// <param name="parameter">The value for which we want to know the step</param>
        /// <returns>The step within the value range</returns>
        public int getStep(double parameter)
        {
            double curr = this.min_value;
            int count = 0;
            while (curr < parameter)
            {
                curr = this.getNextValue(curr);
                count++;
            }
            return count;
        }

        /// <summary>
        /// Computes the value within the options value range for a given step. Required for some experimental designs
        /// </summary>
        /// <param name="step">The step for which we want to know the corresponding value</param>
        /// <returns>The value that corresponds to the given step within the value range of this option</returns>
        public double getValueForStep(int step)
        {
            double value = this.Min_value;

            for (int i = 0; i < step; i++)
            {
                value = this.getNextValue(value);
            }
            return value;
        }

        private List<double> allValues = null;

        /// <summary>
        /// Computes all valid values of the numeric option. Danger! This can be huge for a large value range!
        /// </summary>
        /// <returns>A list containing all values of the numeric options</returns>
        public List<double> getAllValues()
        {
            allValues = new List<double>();
            if (allValues == null)
            {
                // compute
                double curr = this.min_value;
                allValues.Add(curr); // add minimal Value
                while (curr < this.max_value)
                {
                    curr = this.getNextValue(curr);
                    allValues.Add(curr);
                }
            }
            return allValues;
        }



        /// <summary>
        /// Stores the numeric option as an XML Node (calls base implementation)
        /// </summary>
        /// <param name="doc">The XML document to which the node will be added</param>
        /// <returns>The XML node containing the information of numeric option</returns>
        internal XmlNode saveXML(System.Xml.XmlDocument doc)
        {
            XmlNode node = base.saveXML(doc);

            //Min_Value
            XmlNode minNode = doc.CreateNode(XmlNodeType.Element, "minValue", "");
            minNode.InnerText = this.min_value.ToString();
            node.AppendChild(minNode);

            //Max_Value
            XmlNode maxNode = doc.CreateNode(XmlNodeType.Element, "maxValue", "");
            maxNode.InnerText = this.max_value.ToString();
            node.AppendChild(maxNode);

            //StepFunction
            XmlNode stepNode = doc.CreateNode(XmlNodeType.Element, "stepFunction", "");
            stepNode.InnerText = this.stepFunction.ToString();
            node.AppendChild(stepNode);

            //DefaultValue
            XmlNode defNode = doc.CreateNode(XmlNodeType.Element, "defaultValue", "");
            defNode.InnerText = this.defaultValue.ToString();
            node.AppendChild(defNode);
            
            return node;
        }


    }
}
