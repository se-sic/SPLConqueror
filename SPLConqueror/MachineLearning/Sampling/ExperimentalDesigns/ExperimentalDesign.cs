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
        protected List<NumericOption> options = null;


        protected List<Dictionary<NumericOption, double>> selectedConfigurations;
        
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
        /// <param name="designOptions">Expeimental specific parameters.</param>
        /// <returns>True of the computation was successful</returns>
        public abstract bool computeDesing(Dictionary<Object, Object> designOptions);
    }
}
