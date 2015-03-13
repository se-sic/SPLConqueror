using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    public class NumericOption : ConfigurationOption
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
        public InfluenceFunction StepFunction
        {
            get { return stepFunction; }
            set { stepFunction = value; }
        }

        /// <summary>
        /// Constructor to create a new numeric option. All values are set to zero (calls basic constructor)
        /// </summary>
        /// <param name="vm">The variability model to which the option belongs to</param>
        /// <param name="name">Name of that option</param>
        public NumericOption(VariabilityModel vm, String name)
            : base(vm, name)
        {

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
            return Math.Round(stepFunction.eval(t),3);
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

        public int getStepFast(double parameter)
        {
            double stepDistance = Math.Abs(this.min_value - this.getNextValue(this.min_value));
            double steps = (parameter - this.min_value) / stepDistance;
            return (int)Math.Round(steps,0);
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

 //       private List<double> allValues = null;


        private long numberOfSteps = -1;

        public long getNumberOfSteps()
        {
            if (numberOfSteps == -1)
            {
                numberOfSteps = 0;
                double curr = this.min_value;
                while (curr <= this.max_value)
                {
                    curr = this.getNextValue(curr);
                    numberOfSteps += 1;
                }

            }
            return numberOfSteps;
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


        /// <summary>
        /// Creates a numeric option based on the information of the given XML Node (calls base function)
        /// </summary>
        /// <param name="numOptNode">The XML Element containing the information</param>
        /// <param name="variabilityModel">The variability model to which the option belongs to</param>
        /// <returns>The newly created option</returns>
        internal static ConfigurationOption loadFromXML(XmlElement numOptNode, VariabilityModel variabilityModel)
        {
            NumericOption numOpt = new NumericOption(variabilityModel, "temp");
            numOpt.loadFromXML(numOptNode);
            return numOpt;
        }

        internal void loadFromXML(XmlElement node)
        {
            base.loadFromXML(node);
            foreach (XmlElement xmlInfo in node.ChildNodes)
            {
                switch (xmlInfo.Name)
                {
                    case "minValue":
                        this.min_value = Double.Parse(xmlInfo.InnerText.Replace(',', '.'));
                        break;
                    case "maxValue":
                        this.max_value = Double.Parse(xmlInfo.InnerText.Replace(',', '.'));
                        break;
                    case "defaultValue":
                        this.defaultValue = Double.Parse(xmlInfo.InnerText.Replace(',', '.'));
                        break;
                    case "stepFunction":
                        this.stepFunction = new InfluenceFunction(xmlInfo.InnerText.Replace(',','.'),this);
                        break;
                }
            }
        }

        /// <summary>
        /// This mehtod returns nearest valid value of the numerical option to the given value.  
        /// </summary>
        /// <param name="inputValue">A value the nearest valid have to be computet for.</param>
        /// <returns>A valid value.</returns>
        public double nearestValidValue(double inputValue)
        {
            ////TODO improve performance with Dictionary as described in Wunderlist TODO
            double lowerValue = 0;
            double upperValue = 0;

            double curr = Min_value;

            while (curr < inputValue)
            {
                curr = Math.Round(getNextValue(curr),3);
            }
            lowerValue = curr;
            upperValue = Math.Round(getNextValue(curr),3);

            if (Math.Abs(lowerValue - curr) < Math.Abs(upperValue - curr))
            {
                return lowerValue;
            }
            return upperValue;
        }


        public double getCenterValue()
        {
            
           // return getAllValues()[(int)getAllValues().Count / 2];
            return Math.Round(getAllValues()[(int)getAllValues().Count / 2],3);
        }


        private List<double> allValues = null;

        /// <summary>
        /// Computes all valid values of the numeric option. Danger! This can be huge for a large value range!
        /// </summary>
        /// <returns>A list containing all values of the numeric options</returns>
        public List<double> getAllValues()
        {
            if (allValues == null)
            {
                allValues = new List<double>();

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

        public double getRandomValue()
        {
            Random r = new Random();
            return getValueForStep(r.Next((int)this.numberOfSteps));
        }

        public double getRandomValue(int seed)
        {
            Random r = new Random(seed);
            return getValueForStep(r.Next((int)this.numberOfSteps));
        }
    }
}
