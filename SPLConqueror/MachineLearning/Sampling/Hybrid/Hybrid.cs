using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearning.Sampling.Hybrid
{
    /// <summary>
    /// The abstract class for the hybrid classes.
    /// Hybrid classes can be used both for binary and numeric features.
    /// </summary>
    public abstract class HybridStrategy
    {
        /// <summary>
        /// This dictionary contains the parameter provided for this strategy.
        /// </summary>
        protected Dictionary<string, string> strategyParameter;

        /// <summary>
        /// This list contains the options, which have to be considered.
        /// </summary>
        protected List<ConfigurationOption> optionsToConsider = null;

        /// <summary>
        /// This list contains the configurations that were selected by the sampling strategy.
        /// </summary>
        public List<Configuration> selectedConfigurations = new List<Configuration>();

        /// <summary>
        /// This constructor creates a new empty hybrid sampling strategy.
        /// </summary>
        public HybridStrategy()
        {

        }

        /// <summary>
        /// Sets the sampling domain to the given one.
        /// </summary>
        /// <param name="samplingDomain">a <see cref="List"/> of <see cref="ConfigurationOption"/> containing the configuration options that have to be considered by the sampling strategy</param>
        public void SetSamplingDomain(List<ConfigurationOption> samplingDomain)
        {
            this.optionsToConsider = samplingDomain;
        }

        /// <summary>
        /// Set the sampling parameters of the experimental design.
        /// </summary>
        /// <param name="parameterNameToValue">Dictionary with the parameter names and the values that will be set.</param>
        public virtual void SetSamplingParameters(Dictionary<String, String> parameterNameToValue)
        {
            if (strategyParameter == null)
            {
                throw new NotImplementedException("No strategy parameters are implemented for the strategy " + this.GetType() + ".");
            }

            foreach (string option in parameterNameToValue.Keys)
            {
                if (this.strategyParameter.ContainsKey(option))
                {
                    this.strategyParameter[option] = parameterNameToValue[option];
                }
                else
                {
                    // Instead of throwing an argument exception, an error could be printed.
                    throw new ArgumentException("The parameter " + option + " is not supported in the strategy " + this.GetType() + ".");
                }
            }
        }

        /// <summary>
        /// Computes the sampling set using the provided parameters. 
        /// </summary>
        /// <returns><code>True</code> if the computation was successful.</returns>
        public abstract bool ComputeSamplingStrategy();

        /// <summary>
        /// Create a string representing the selected parameters.
        /// </summary>
        /// <returns>String representation of the parameters.</returns>
        public virtual string ParameterIdentifier()
        {
            if (this.strategyParameter == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (string o in this.strategyParameter.Keys)
            {
                sb.Append(o + "-" + this.strategyParameter[o] + "_");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// The Methods returns the name of the experimental design.
        /// </summary>
        /// <returns>Name of the experimental design.</returns>
        public abstract string GetName();

        /// <summary>
        /// Return the tag that identifies the hybrid design.
        /// </summary>
        /// <returns>Tag of the hybrid design as string.</returns>
        public abstract string getTag();
    }
}
