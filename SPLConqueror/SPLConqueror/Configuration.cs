using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class Configuration : IEquatable<Configuration>
    {

        private Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptions = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
        private Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();

        private Dictionary<NFProperty, double> nfpValues = new Dictionary<NFProperty, double>();

        private string identifier;


        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection"></param>
        /// <param name="numericSelection"></param>
        /// <param name="measuremements"></param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection, Dictionary<NFProperty, double> measuremements)
        {
            binaryOptions = enrichBinarySelection(binarySelection);
            numericOptions = numericSelection;
            nfpValues = measuremements;


            identifier = generateIdentifier();

        }

        public string generateIdentifier()
        {
            return generateIdentifier("%;%");
        }

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
                if(binaryOptions[binary].Equals(BinaryOption.Value.Selected))
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
        /// This method returns the configuration specific value of the nf-property being selected to be the default property. 
        /// </summary>
        /// <returns></returns>
        public double GetNFPValue()
        {
            return nfpValues[GlobalState.currentNFP];
        }

        /// <summary>
        /// This method returns the configuration specific value of the given property. 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public double GetNFPValue(NFProperty property)
        {
            return nfpValues[property];
        }


        public override string ToString()
        {
            return identifier;
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Configuration other)
        {
            return this.identifier.CompareTo(other.identifier);
        }
   
    }
}
