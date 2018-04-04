using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    /// <summary>
    /// An influence model describes all identified influences of the configuration options on the non-functional property that is considered.
    /// </summary>
    public class InfluenceModel
    {
        private VariabilityModel vm = null;

        /// <summary>
        /// The varaibility model the influence model is learned for.
        /// </summary>
        public VariabilityModel Vm
        {
            get { return vm; }
            set { vm = value; }
        }
        private NFProperty nfp = null;

        Dictionary<BinaryOption, InfluenceFunction> binaryOptionsInfluence = new Dictionary<BinaryOption, InfluenceFunction>();

        /// <summary>
        /// The set of the individual influences identified for binary configration options.
        /// </summary>
        public Dictionary<BinaryOption, InfluenceFunction> BinaryOptionsInfluence
        {
            get { return binaryOptionsInfluence; }
            set { binaryOptionsInfluence = value; }
        }

        Dictionary<NumericOption, InfluenceFunction> numericOptionsInfluence = new Dictionary<NumericOption, InfluenceFunction>();

        /// <summary>
        /// Gets or sets the influence of the numeric option numeric options.
        /// </summary>
        /// <value>The influence of numeric options.</value>
        public Dictionary<NumericOption, InfluenceFunction> NumericOptionsInfluence
        {
            get { return numericOptionsInfluence; }
            set { numericOptionsInfluence = value; }
        }

        private Dictionary<Interaction, InfluenceFunction> interactionInfluence = new Dictionary<Interaction, InfluenceFunction>();

        /// <summary>
        /// Gets or sets the influence of interactions.
        /// </summary>
        /// <value>The influence of interactions.</value>
        public Dictionary<Interaction, InfluenceFunction> InteractionInfluence
        {
            get { return interactionInfluence; }
            set { interactionInfluence = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SPLConqueror_Core.InfluenceModel"/> class.
        /// </summary>
        /// <param name="vm">The variability model.</param>
        /// <param name="nfp">The non-functional property to investigate.</param>
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
            sw.Write(printModelAsFunction());
            sw.Close();
        }
        /// <summary>
        /// Gives the influence model as a single function (all influence functions will be concatenated)
        /// </summary>
        ///  <returns>A String representation of the current influence model. It concatenates all influence functions.</returns>
        public String printModelAsFunction()
        {
            StringBuilder sb = new StringBuilder();
            foreach (BinaryOption binOpt in this.BinaryOptionsInfluence.Keys)
            {
                sb.AppendLine(this.BinaryOptionsInfluence[binOpt].ToString() + " + ");
            }
            foreach (NumericOption numOpt in this.NumericOptionsInfluence.Keys)
            {
                sb.AppendLine(this.NumericOptionsInfluence[numOpt].ToString() + " + ");
            }
            foreach (var interaction in this.InteractionInfluence.Keys)
            {
                sb.AppendLine(this.InteractionInfluence[interaction].ToString() + " + ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// The function parses the model and collects all individual influences and the interactions.
        /// </summary>
        /// <returns>Returns a list of Features, which represent the influences of individual configuration options or their interactions</returns>
        public List<Feature> getListOfFeatures()
        {
            List<Feature> resultList = new List<Feature>();
            foreach (BinaryOption binOpt in this.BinaryOptionsInfluence.Keys)
            {
                if (this.BinaryOptionsInfluence[binOpt] is Feature)
                    resultList.Add((Feature)this.BinaryOptionsInfluence[binOpt]);
            }
            foreach (NumericOption numOpt in this.NumericOptionsInfluence.Keys)
            {
                if (this.NumericOptionsInfluence[numOpt] is Feature)
                    resultList.Add((Feature)this.NumericOptionsInfluence[numOpt]);
            }
            foreach (var interaction in this.InteractionInfluence.Keys)
            {
                if (this.InteractionInfluence[interaction] is Feature)
                    resultList.Add((Feature)this.InteractionInfluence[interaction]);
            }
            return resultList;
        }
    }
}
