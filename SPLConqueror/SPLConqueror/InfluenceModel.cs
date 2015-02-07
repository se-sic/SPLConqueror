using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class InfluenceModel
    {
        private VariabilityModel vm = null;
        private NFProperty nfp = null;

        Dictionary<BinaryOption, InfluenceFunction> binaryOptionsInfluence = new Dictionary<BinaryOption, InfluenceFunction>();

        public Dictionary<BinaryOption, InfluenceFunction> BinaryOptionsInfluence
        {
            get { return binaryOptionsInfluence; }
            set { binaryOptionsInfluence = value; }
        }

        Dictionary<NumericOption, InfluenceFunction> numericOptionsInfluence = new Dictionary<NumericOption, InfluenceFunction>();

        public Dictionary<NumericOption, InfluenceFunction> NumericOptionsInfluence
        {
            get { return numericOptionsInfluence; }
            set { numericOptionsInfluence = value; }
        }

        private Dictionary<List<ConfigurationOption>, InfluenceFunction> interactionInfluence = new Dictionary<List<ConfigurationOption>, InfluenceFunction>();

        public Dictionary<List<ConfigurationOption>, InfluenceFunction> InteractionInfluence
        {
            get { return interactionInfluence; }
            set { interactionInfluence = value; }
        }

        public InfluenceModel(VariabilityModel vm, NFProperty nfp)
        {
            this.vm = vm;
            this.nfp = nfp;
        }

        /// <summary>
        /// Estimates for the given confugration the corresponding value of the non-functional property based on the determined influences of all configuration options.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Estimated value</returns>
        public double estimate(Configuration c)
        {
            double estimate = 0;

            return estimate;
        }
    }
}
