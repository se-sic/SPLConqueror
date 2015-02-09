using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class Configuration : IEquatable<Configuration>
    {

        private static String DEFAULT_SEPARATOR = "%;%";

        private Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptions = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

        /// <summary>
        /// Binary options of this configuration. For each option it is stored whether the option is selceted or deselected in this configuration.
        /// </summary>
        public Dictionary<BinaryOption, BinaryOption.BinaryValue> BinaryOptions
        {
            get { return binaryOptions; }
        }

        /// <summary>
        /// Numeric option of this configuration. The selected value for each numeric option is stored in the value. 
        /// </summary>
        public Dictionary<NumericOption, double> NumericOptions
        {
            get { return numericOptions; }
        }

        private Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();

        private Dictionary<NFProperty, double> nfpValues = new Dictionary<NFProperty, double>();

        private string identifier;


        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection">A valid set of binary options.</param>
        /// <param name="numericSelection">A valid selection of values of numeric options.</param>
        /// <param name="measuremements">Measured values of non functional properties, such as performance or footprint.</param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection, Dictionary<NFProperty, double> measuremements)
        {
            binaryOptions = enrichBinarySelection(binarySelection);
            numericOptions = numericSelection;
            nfpValues = measuremements;


            identifier = generateIdentifier(DEFAULT_SEPARATOR);

        }


        /// <summary>
        /// Returns an identifier describing the choice of binary configuration options and numeric configuration-option values of the configuration. 
        /// The default separator is used in the identifier. 
        /// </summary>
        /// <returns>the identifier</returns>
        public string getIdentifier()
        {
            return identifier;
        }



        /// <summary>
        /// Returns the identifier describing the choice of binary configuration options and numeric configuration-option values of the configuration. 
        /// The separator is used in the identifier. 
        /// </summary>
        /// <param name="separator">The configuration options </param>
        /// <returns>the identifier with the specified separator</returns>
        public string generateIdentifier(String separator)
        {
            // sort binary features by name
            var binarySelection = binaryOptions.Keys.ToList();
            binarySelection.Sort();

            // sort numeric features by name
            var numericSelection = numericOptions.Keys.ToList();
            numericSelection.Sort();

            StringBuilder sb = new StringBuilder();

            foreach (BinaryOption binary in binarySelection)
            {
                if(binaryOptions[binary].Equals(BinaryOption.BinaryValue.Selected))
                    sb.Append(binary.Name+ separator);
            }

            foreach (NumericOption numeric in numericSelection)
            {
                sb.Append(numeric.Name+ numericOptions[numeric] + separator);
            }
            


            return sb.ToString();

        }

        /// <summary>
        /// The list of binary features is enriched with all features existing in the variability model. The value of the features being enriched is the default value of the specific features. 
        /// </summary>
        /// <param name="binarySelection"></param>
        /// <returns></returns>
        private Dictionary<BinaryOption, BinaryOption.BinaryValue> enrichBinarySelection(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection)
        {
            Dictionary<BinaryOption, BinaryOption.BinaryValue> options = new Dictionary<BinaryOption,BinaryOption.BinaryValue>();

            foreach(BinaryOption bOption in GlobalState.varModel.BinaryOptions){
                if (binarySelection.ContainsKey(bOption))
                {
                    options.Add(bOption, binarySelection[bOption]);
                }
                else
                {
                    options.Add(bOption, bOption.DefaultValue);
                }
            }
            return options;
        }

        /// <summary>
        /// This method returns the configuration specific value of the current nf-property or null of the configuration has no value for the current nfp.
        /// </summary>
        /// <returns></returns>
        public double GetNFPValue()
        {
            return nfpValues[GlobalState.currentNFP];
        }

        /// <summary>
        /// This method returns the configuration specific value of the property. 
        /// </summary>
        /// <param name="property">A non functional property.</param>
        /// <returns>The value of the property stored in the configuration.</returns>
        public double GetNFPValue(NFProperty property)
        {
            return nfpValues[property];
        }

        /// <summary>
        /// String representing the configuration. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return identifier;
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public int CompareTo(Configuration other)
        {
            return this.identifier.CompareTo(other.identifier);
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public bool Equals(Configuration other)
        {
            return this.identifier.Equals(other.identifier);
        }


        public List<BinaryOption> getBinaryOptions(BinaryOption.BinaryValue binaryValue)
        {
            List<BinaryOption> result = new List<BinaryOption>();

            foreach(KeyValuePair<BinaryOption, BinaryOption.BinaryValue> bin in this.binaryOptions)
                if(bin.Value.Equals(binaryValue))
                    result.Add(bin.Key);

            return result;
        }
    }
}
