using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    public class InfluenceModel
    {
        private VariabilityModel vm = null;

        public VariabilityModel Vm
        {
            get { return vm; }
            set { vm = value; }
        }
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

        private Dictionary<Interaction, InfluenceFunction> interactionInfluence = new Dictionary<Interaction, InfluenceFunction>();

        public Dictionary<Interaction, InfluenceFunction> InteractionInfluence
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
        /// <param name="c">The configuration for which the estimation should be calculated</param>
        /// <returns>Estimated value</returns>
        public double estimate(Configuration c)
        {
            double estimate = 0;
            foreach (BinaryOption binOpt in this.BinaryOptionsInfluence.Keys)
            {
                estimate += this.BinaryOptionsInfluence[binOpt].eval(c);
            }
            foreach (NumericOption numOpt in this.NumericOptionsInfluence.Keys)
            {
                estimate += this.NumericOptionsInfluence[numOpt].eval(c);
            }
            foreach (var interaction in this.InteractionInfluence.Keys)
            {
                estimate += this.InteractionInfluence[interaction].eval(c);
            }
            return estimate;
        }

        /// <summary>
        /// Prints the influence model as a single function (all influence functions will be concatenated) in a single file
        /// </summary>
        /// <param name="file">Path of the file which stores the model</param>
        public void printModelAsFunction(String file)
        {
            StreamWriter sw = new StreamWriter(file);
            foreach (BinaryOption binOpt in this.BinaryOptionsInfluence.Keys)
            {
               sw.WriteLine(this.BinaryOptionsInfluence[binOpt].ToString() + " + ");
            }
            foreach (NumericOption numOpt in this.NumericOptionsInfluence.Keys)
            {
                sw.WriteLine(this.NumericOptionsInfluence[numOpt].ToString() + " + ");
            }
            foreach (var interaction in this.InteractionInfluence.Keys)
            {
                sw.WriteLine(this.InteractionInfluence[interaction].ToString() + " + ");
            }
            sw.Close();
        }

    }
}
