using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class Configuration
    {

        private Dictionary<BinaryOption, BinaryOption.Value> binaryOptions = new Dictionary<BinaryOption, BinaryOption.Value>();
        private Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();

        private Dictionary<NFProperty, double> nfpValues = new Dictionary<NFProperty, double>();

        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection"></param>
        /// <param name="numericSelection"></param>
        /// <param name="measuremements"></param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.Value> binarySelection, Dictionary<NumericOption, double> numericSelection, Dictionary<NFProperty, double> measuremements)
        {
            binaryOptions = enrichBinarySelection(binarySelection);
            numericOptions = numericSelection;
            nfpValues = measuremements;

        }

        /// <summary>
        /// The list of binary features is enriched with all features existing in the variability model. The value of the features being enriched is the default value of the specific features. 
        /// </summary>
        /// <param name="binarySelection"></param>
        /// <returns></returns>
        private Dictionary<BinaryOption, BinaryOption.Value> enrichBinarySelection(Dictionary<BinaryOption, BinaryOption.Value> binarySelection)
        {
            Dictionary<BinaryOption, BinaryOption.Value> options = new Dictionary<BinaryOption,BinaryOption.Value>();

            foreach(BinaryOption bOption in GlobalState.varModel.BinaryOptions){
                if (binarySelection.ContainsKey(bOption))
                {
                    options.Add(bOption, binarySelection[bOption]);
                }
                else
                {
                    options.Add(bOption, bOption.defaultValue);
                }
            }
            return options;
        }

        /// <summary>
        /// This method returns the configuration specific value of the nf-property being selected to be the default property. 
        /// </summary>
        /// <returns></returns>
        public double getNFPValue()
        {
            return nfpValues[GlobalState.currentNFP];
        }

        /// <summary>
        /// This method returns the configuration specific value of the property. 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public double getNFPValue(NFProperty property)
        {
            return nfpValues[property];
        }

    }
}
