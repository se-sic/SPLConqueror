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

        /// <summary>
        /// This method returns a dictionary holding a key value pair for each parameter of the 
        /// experimental design. The name of the parameter is stored as key and the type of the parameter as value. 
        /// </summary>
        /// <returns>Dirctionary consisting of an key value pair for each parameter of the design.</returns>
        public abstract Dictionary<string, string> getParameterTypes();

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
        /// The Methods returns the name of the experimental design.
        /// </summary>
        /// <returns>Name of the experimental design.</returns>
        public abstract string getName();
       
        /// <summary>
        /// Computes the design using the default parameters. 
        /// </summary>
        /// <returns>True if the computation was successful.</returns>
        public abstract bool computeDesign();

        /// <summary>
        /// Computes the design using the experimental design specific parameters provided as parameter. 
        /// </summary>
        /// <param name="designOptions">Expeimental specific parameters. Keys of the dictionary are the names of the parameters and the values of the dictionary are the values of the parameters.</param>
        /// <returns>True of the computation was successful</returns>
        public abstract bool computeDesign(Dictionary<string, string> designOptions);


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
