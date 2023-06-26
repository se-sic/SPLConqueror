﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{

    /// <summary>
    /// Represents a numeric configuration option that can be used to customize the case study.
    /// </summary>
    public class NumericOption : ConfigurationOption
    {
        private double min_value = 0;

        /// <summary>
        /// The minimal value of the value domain of the option.
        /// </summary>
        public double Min_value
        {
            get { return min_value; }
            set { min_value = value; }
        }

        private double max_value = 10;

        /// <summary>
        /// The maximal value of the value domain of the option.
        /// </summary>
        public double Max_value
        {
            get { return max_value; }
            set { max_value = value; }
        }

        private NumericValues values = null;

        /// <summary>
        /// All valid values of the value domain of the numeric option.
        /// </summary>
        public NumericValues Values
        {
            get { return values; }
        }

        private InfluenceFunction stepFunction = null;

        private BinaryOption abstractEnabled = null;

        private BinaryOption abstractDisabled = null;

        /// <summary>
        /// Value of the numeric option when it is deselected.
        /// </summary>
        public int OptionalFlag { get; private set; }

        private BinaryOption getOrCreateEnabled()
        {
            if (this.abstractEnabled == null)
            {
                abstractEnabled = new BinaryOption(base.vm, "Enabled" + this.Name);
                abstractEnabled.IsStrictlyAbstract = true;
                abstractEnabled.Optional = true;
                abstractEnabled.Parent = vm.Root;
                base.vm.AbrstactOptions.Add(abstractEnabled);
            }
                
            return abstractEnabled;
        }

        private BinaryOption getOrCreateDisabled()
        {
            if (this.abstractDisabled == null)
            {
                abstractDisabled = new BinaryOption(base.vm, "Disabled" + this.Name);
                abstractDisabled.IsStrictlyAbstract = true;
            }

            return abstractDisabled;
        }

        /// <summary>
        /// Method that returns the abstract option that serves as flag to mark this configuration option as enabled.
        /// </summary>
        /// <returns>Abstract configuration option.</returns>
        public BinaryOption abstractEnabledConfigurationOption()
        {
            return Optional ? getOrCreateEnabled() : null;
        }

        /// <summary>
        /// Method that returns the abstract option that serves as flag to mark this configuration option as disabled.
        /// </summary>
        /// <returns>Abstract configuration option.</returns>
        public BinaryOption abstractDisabledConfigurationOption()
        {
            return Optional ? getOrCreateDisabled() : null;
        }

        /// <summary>
        /// Set optional value of the configuration option.
        /// </summary>
        /// <param name="optional">Boolean that indicates the optionality of this option.</param>
        /// <param name="flag">The flag that indicates that this option was deselected. 
        /// Only required when setting the option to optional.</param>
        public void setOptional(bool optional, int flag = -1)
        {
            this.Optional = optional;
            if (optional)
            {
                this.OptionalFlag = flag;
                getOrCreateEnabled();
                getOrCreateDisabled();
            } else
            {
                base.vm.AbrstactOptions.Remove(abstractEnabled);
                this.abstractEnabled = null;
                this.abstractDisabled = null;
            }
        }

        /// <summary>
        /// A function that computes each value within the value range of that option
        /// </summary>
        public InfluenceFunction StepFunction
        {
            get { return stepFunction; }
            set
            {
                stepFunction = value;
                if (value != null)
                {
                    values = null;
                }
            }
        }

        /// <summary>
        /// Constructor to create a new numeric option. All values are set to zero (calls basic constructor)
        /// </summary>
        /// <param name="vm">The variability model to which the option belongs to</param>
        /// <param name="name">Name of that option</param>
        public NumericOption(VariabilityModel vm, String name)
            : base(vm, name)
        {
            this.Optional = false;
        }

        /// <summary>
        /// Computes the next value of the numeric option using the step function
        /// </summary>
        /// <param name="value">Value from which the next value shall be computed</param>
        /// <returns>The next valid value within the value range of that option</returns>
        public double getNextValue(double value)
        {
            if (stepFunction != null)
            {
                Dictionary<NumericOption, double> t = new Dictionary<NumericOption, double>();
                t.Add(this, value);
                return Math.Round(stepFunction.eval(t), 3);
            }
            else
            {
                double nextGreaterValue = -1;
                for (int i = 0; i < values.Count(); i++)
                {
                    if (values.Values[i] > value && (Math.Abs(value - values.Values[i]) < Math.Abs(nextGreaterValue - values.Values[i])))
                    {
                        nextGreaterValue = values.Values[i];
                    }
                }
                return nextGreaterValue;
            }
        }

        /// <summary>
        /// Sets the numerical values.
        /// </summary>
        /// <param name="values">The values to set.</param>
        public void SetValues(double[] values)
        {
            this.min_value = values.Min();
            this.max_value = values.Max();
            if (this.values == null)
            {
                this.values = new NumericValues(values);
            }
            else
            {
                this.values.Values = values;
            }
            // Either use values or step function
            this.StepFunction = null;
        }

        /// <summary>
        /// Sets the numerical values.
        /// </summary>
        /// <param name="values">The values to set.</param>
        public void SetValues(String values)
        {
            SetValues(values.Replace("(", "").Replace(")", "").Split(';').Select(Double.Parse).ToArray());
        }

        /// <summary>
        /// Gives the step number for a given parameter starting by 0 representing the minValue: For example, minVal = 5; maxVal = 20; stepSize = 5; getStep(15) returns 2; 
        /// Required for drawing the influence model 
        /// </summary>
        /// <param name="parameter">The value for which we want to know the step</param>
        /// <returns>The step within the value range</returns>
        public int getStep(double parameter)
        {
            if (stepFunction == null)
            {
                for (int i = 0; i < values.Count(); i++)
                {
                    if (values.Values[i].Equals(parameter))
                        return i;
                }
            }

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
        /// Returns the step number for a given parameter, as <see cref="getStep(double)"/>.
        /// The difference to the method <see cref="getStep(double)"/> is, that the step is retrieved with less effort if the <see cref="stepFunction"/> is given.
        /// </summary>
        /// <param name="parameter">The value for which the step number should be returned.</param>
        /// <returns>The step within the value range.</returns>
        public int getStepFast(double parameter)
        {
            if (stepFunction == null)
                return getStep(parameter);


            double stepDistance = Math.Abs(this.min_value - this.getNextValue(this.min_value));
            double steps = (parameter - this.min_value) / stepDistance;
            return (int)Math.Round(steps, 0);
        }

        /// <summary>
        /// Computes the value within the options value range for a given step. Required for some experimental designs
        /// </summary>
        /// <param name="step">The step for which we want to know the corresponding value</param>
        /// <returns>The value that corresponds to the given step within the value range of this option</returns>
        public double getValueForStep(int step)
        {
            double value = this.Min_value;

            if (stepFunction == null)
            {
                // if there are less values defined than the number of steps 
                if (step > values.Values.Count())
                    return value;

                return values.Values[step];
            }

            for (int i = 0; i < step; i++)
            {
                value = this.getNextValue(value);
            }
            return value;
        }

        private long numberOfSteps = -1;


        /// <summary>
        /// Returns the number of distinct values of the numeric option.
        /// </summary>
        /// <returns>Number of distinct values of the option.</returns>
        public long getNumberOfSteps()
        {
            if (stepFunction == null)
            {
                return values.Count();
            }

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
        internal new XmlNode saveXML(System.Xml.XmlDocument doc)
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

            if (this.stepFunction != null)
            {
                //StepFunction
                XmlNode stepNode = doc.CreateNode(XmlNodeType.Element, "stepFunction", "");
                stepNode.InnerText = this.stepFunction.ToString();
                node.AppendChild(stepNode);
            }

            if (values != null)
            {
                //Values
                XmlNode valuesNode = doc.CreateNode(XmlNodeType.Element, "values", "");
                valuesNode.InnerText = values.ToString();
                node.AppendChild(valuesNode);
            }

            if (this.Optional)
            {
                XmlNode deselectedFlag = doc.CreateNode(XmlNodeType.Element, "deselectedFlag", "");
                deselectedFlag.InnerText = this.OptionalFlag.ToString();
                node.AppendChild(deselectedFlag);
            }
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

        /// <summary>
        /// Loads all information about the numeric connfiguration option from the XML file consisting of the variability model. 
        /// </summary>
        /// <param name="node">The root note of the numeric configuration option.</param>
        internal new void loadFromXML(XmlElement node)
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
                    case "stepFunction":
                        this.stepFunction = new InfluenceFunction(xmlInfo.InnerText.Replace(',', '.'), this);
                        break;
                    case "deselectedFlag":
                        this.setOptional(true, Int32.Parse(xmlInfo.InnerText.Replace(',', '.')));
                        break;
                    case "values":
                        String[] valueArray = xmlInfo.InnerText.Replace(',', '.').Split(';');
                        double[] values_As_Double = new double[valueArray.Count()];
                        for (int i = 0; i < valueArray.Count(); i++)
                        {
                            values_As_Double[i] = Convert.ToDouble(valueArray[i]);
                        }
                        SetValues(values_As_Double);
                        break;

                }
            }
        }

        /// <summary>
        /// This method returns the valid value with the smallest distance to the value given.  
        /// </summary>
        /// <param name="inputValue">A value the nearest valid have to be computet for.</param>
        /// <returns>The valid value with the smallest distance to the input value.</returns>
        public double nearestValidValue(double inputValue)
        {

            if (stepFunction == null)
            {
                double nearestValue = values.Values[0];
                for (int i = 1; i < values.Count(); i++)
                {
                    if (Math.Abs(values.Values[i] - inputValue) < Math.Abs(nearestValue - inputValue))
                        nearestValue = values.Values[i];
                }
                return nearestValue;
            }


            ////HACK can lead to performance problems for numeric options with a large number of valid values
            double lowerValue = 0;
            double upperValue = 0;

            double curr = Min_value;

            while (curr < inputValue)
            {
                curr = Math.Round(getNextValue(curr), 3);
            }
            lowerValue = curr;
            upperValue = Math.Round(getNextValue(curr), 3);

            if (Math.Abs(lowerValue - curr) < Math.Abs(upperValue - curr))
            {
                return lowerValue;
            }
            return upperValue;
        }

        /// <summary>
        /// Returns the center value of the value domain of the numeric configuration option. 
        /// </summary>
        /// <returns>The center value of the value domain.</returns>
        public double getCenterValue()
        {
            if (stepFunction == null)
                return values.Values[(int)values.Values.Count() / 2];
            return Math.Round(getAllValues()[(int)getAllValues().Count / 2], 3);
        }


        private List<double> allValues = null;

        /// <summary>
        /// Computes all valid values of the numeric option. Danger! This can be huge for a large value range!
        /// </summary>
        /// <returns>A list containing all values of the numeric options</returns>
        public List<double> getAllValues()
        {
            if (stepFunction == null)
                return values.Values.ToList();


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

        /// <summary>
        /// Provides a random value of the value domain of the numeric configuration option.
        /// </summary>
        /// <param name="seed">The seed for the random generator.</param>
        /// <returns>The random value of the value domain.</returns>
        public double getRandomValue(int seed)
        {
            Random r = new Random(seed);
            if (stepFunction == null)
                return values.Values[r.Next(values.Count())];
            return getValueForStep(r.Next((int)this.numberOfSteps));
        }


        /// <summary>
        /// This method clones a numeric option by using the provided variability model.
        /// </summary>
        /// <param name="variabilityModel">The variability model to use for cloning the option.</param>
        /// <returns>A clone of this option.</returns>
        public NumericOption Clone(VariabilityModel variabilityModel)
        {
            NumericOption clone = new NumericOption(variabilityModel, Name);
            clone.Min_value = Min_value;
            clone.Max_value = Max_value;
            InfluenceFunction clonedStepFunction = new InfluenceFunction(StepFunction.ToString(), clone);
            clone.StepFunction = clonedStepFunction;
            clone.values = values;
            clone.Optional = Optional;
            clone.OutputString = OutputString;
            clone.Prefix = Prefix;
            clone.Postfix = Postfix;

            return clone;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is NumericOption))
            {
                return false;   
            }
            return Name.Equals(((NumericOption)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
