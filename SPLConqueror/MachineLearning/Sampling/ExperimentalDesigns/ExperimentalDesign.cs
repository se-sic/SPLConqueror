using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    /// <summary>
    /// The abstract super class for all experimental designs.
    /// </summary>
    public abstract class ExperimentalDesign
    {

        protected Dictionary<string, string> designParameter = new Dictionary<string, string>();
        
        protected List<NumericOption> options = null;

        /// <summary>
        /// Defines the minimal number of different values of one numeric option that are considered during sampling. 
        /// </summary>
        public int minNumberOfSamplingsPerNumericOption = 2;

        protected List<Dictionary<NumericOption, double>> selectedConfigurations = new List<Dictionary<NumericOption,double>>();
        
        /// <summary>
        /// Configurations selected from the experimental design.
        /// </summary>
        public List<Dictionary<NumericOption, double>> SelectedConfigurations
        {
            get { return selectedConfigurations; }
        }

        /// <summary>
        /// Creates a new experimental design for a given set of numeric options. 
        /// </summary>
        /// <param name="samplingDomain">Set of numeric options that are considered by the design.</param>
        public ExperimentalDesign(List<NumericOption> samplingDomain)
        {
            options = samplingDomain;
        }

        /// <summary>
        /// Creates a new empty experimental design.
        /// </summary>
        public ExperimentalDesign()
        {

        }

        /// <summary>
        /// Sets the sampling domain of the experimental design.
        /// </summary>
        /// <param name="samplingDomain">Set of numeric options that are considered by the design.</param>
        public void setSamplingDomain(List<NumericOption> samplingDomain)
        {
            options = samplingDomain;
        }

        /// <summary>
        /// Gets the sampling domain of the experimental design.
        /// </summary>
        /// <returns>Set of numeric options that are considered by the design.</returns>
        public List<NumericOption> getSamplingDomain()
        {
            return options;
        }

        /// <summary>
        /// Instatiates a new experimental design based on a given string. the format is: "[NumericOption1,Num2,...] Parameter1:VALUE Parameter2:VALUE"
        /// </summary>
        /// <param name="param">The string containing the parameters and numeric options used by an experimental design</param>
        public ExperimentalDesign(String param)
        {
            String[] parameters = param.Split(' ');
            if (param.Length > 0)
            {
                foreach (string par in parameters)
                {
                    if (par.Contains("["))
                    {
                        string[] localOptions = par.Substring(1, par.Length - 2).Split(',');
                        foreach (string option in localOptions)
                        {
                            this.options.Add(GlobalState.varModel.getNumericOption(option));
                        }
                    }
                    else
                    {
                        if (par.Contains(':'))
                        {
                            string[] nameAndValue = par.Split(':');
                            designParameter.Add(nameAndValue[0], nameAndValue[1]);
                        }
                        else
                        {
                            designParameter.Add(par, "");
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Get the value of a given sampling parameter from a parameter dictionary.
        /// </summary>
        /// <param name="parameter">Dictionary with names of parameters and their value.</param>
        /// <param name="key">Name of the requested parameter.</param>
        /// <returns></returns>
        protected static int parseFromParameters(Dictionary<String, String> parameter, String key)
        {
            String valueAsString;
            parameter.TryGetValue(key, out valueAsString);
            return Int32.Parse(valueAsString);
        }

        /// <summary>
        /// Create a string representing the selected parameters.
        /// </summary>
        /// <returns>String representation of the parameters.</returns>
        public abstract string parameterIdentifier();

        /// <summary>
        /// The Methods returns the name of the experimental design.
        /// </summary>
        /// <returns>Name of the experimental design.</returns>
        public abstract string getName();

        /// <summary>
        /// Set the sampling parameters of the experimental design.
        /// </summary>
        /// <param name="parameterNameToValue">Dictionary with the parameter names and the values that will be set.</param>
        public abstract void setSamplingParameters(Dictionary<String, String> parameterNameToValue);
       
        /// <summary>
        /// Computes the design using the default parameters. 
        /// </summary>
        /// <returns>True if the computation was successful.</returns>
        public abstract bool computeDesign();

        /// <summary>
        /// Samples the value space of one numeric option. The values are equal distributed. The number of values is defined by the <see cref="minNumberOfSamplingsPerNumericOption"/> field. The minimal and 
        /// maximal value of the numeric option are not considered during the sampling.
        /// </summary>
        /// <param name="option">The numeric option to sample.</param>
        /// <returns>A list of equal distributed values for the numeric option. The list might be empty.</returns>
        public List<double> sampleOption(NumericOption option)
        {
            if (this.minNumberOfSamplingsPerNumericOption > option.getNumberOfSteps())
                return sampleOption(option, option.getNumberOfSteps(), false);
            return sampleOption(option, this.minNumberOfSamplingsPerNumericOption, false);
        }

        /// <summary>
        /// Samples the value space of one numeric option. The values are equal distributed. If the numeric option has less values than the desired number of samplings, all values of the numeric option are 
        /// returned. 
        /// </summary>
        /// <param name="option">The numeric option to sample.</param>
        /// <param name="numberOfSamples">The number of different values of the numeric option.</param>
        /// <param name="useMinMaxValues">States whether the minimal and maximal value of the numeric option have to be considered during sampling.</param>
        /// <returns>A list of equal distributed values for the numeric option. The list might be empty.</returns>
        public static List<double> sampleOption(NumericOption option, long numberOfSamples, bool useMinMaxValues)
        {
            List<double> resultList = new List<double>();

            long numberOfValues = option.getNumberOfSteps();
            if (numberOfValues <= numberOfSamples)
            {
                double val = option.Min_value;
                for (int k = 0; k < numberOfValues; k++)
                {
                    resultList.Add(val);
                    val = option.getNextValue(val);
                }
                return resultList;
            }

            if (useMinMaxValues)
            {
                resultList.Add(option.Min_value);
                resultList.Add(option.Max_value);

                numberOfSamples -= 2;
            }


            int offsetForOddNumberOfValues = 1;
            int offSetBetweenValues = (int)Math.Round((double)(numberOfValues - 2 + offsetForOddNumberOfValues) / (double)(numberOfSamples + 1));

            double value = option.Min_value;
            for (int i = 0; i < numberOfSamples; i++)
            {
                int currNumSteps = 0;

                while (currNumSteps < offSetBetweenValues)
                {
                    value = option.getNextValue(value);
                    currNumSteps++;
                }
                resultList.Add(value);

            }
            resultList.Sort();
            return resultList;
        }
    }
}
