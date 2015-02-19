using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class Interaction
    {
        private List<BinaryOption> binaryOptions = new List<BinaryOption>();

        /// <summary>
        /// A list of binary options that interact with each other.
        /// </summary>
        public List<BinaryOption> BinaryOptions
        {
            get { return binaryOptions; }
            set { binaryOptions = value; }
        }

        private List<NumericOption> numericOptions = new List<NumericOption>();

        /// <summary>
        /// A list of numeric options that interact with each other.
        /// </summary>
        public List<NumericOption> NumericOptions
        {
            get { return numericOptions; }
            set { numericOptions = value; }
        }

        private List<ConfigurationOption> binNumOptions = new List<ConfigurationOption>();

        /// <summary>
        /// A list of configuration options mixed binary and numeric that interaction with each other
        /// </summary>
        public List<ConfigurationOption> BinNumOptions
        {
            get { return binNumOptions; }
            set { binNumOptions = value; }
        }

        private String name = "";

        /// <summary>
        /// The name of the interaction
        /// </summary>
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Requires the name of an interaction to act as an identifier
        /// </summary>
        /// <param name="name">Name of the interaction</param>
        public Interaction(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Creates an interaction based on a given influence function. 
        /// </summary>
        /// <param name="f">A influence function consisting of configuration options.</param>
        public Interaction(InfluenceFunction f)
        {
            StringBuilder sb = new StringBuilder();
            foreach (BinaryOption opt in f.participatingBoolOptions)
            {
                sb.Append(opt.Name + "#");
                binaryOptions.Add(opt);
            }
            foreach (NumericOption opt in f.participatingNumOptions)
            {
                sb.Append(opt.Name + "#");
                this.numericOptions.Add(opt);
            }
        }

        /// <summary>
        /// Generates a descriptive name for the interaction containing the involved options
        /// </summary>
        /// <returns></returns>
        public String getDescriptiveName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var opt in binaryOptions)
                sb.Append(opt.Name + "#");
            foreach (var opt in numericOptions)
                sb.Append(opt.Name + "#");
            foreach (var opt in binNumOptions)
                sb.Append(opt.Name + "#");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Checks whether the interaction occurs in the configuration. Checks only for binary options, because numeric options are assumed to be always present
        /// </summary>
        /// <param name="c">The configuration for which we check whether it contains the interaction.</param>
        /// <returns>True if the interaction is present in the configuration, false otherwise.</returns>
        public bool isInConfiguration(Configuration c)
        {
            var selectedBinOptions = c.getBinaryOptions(BinaryOption.BinaryValue.Selected);
            foreach (var binOpt in this.binaryOptions)
            {
                if (!selectedBinOptions.Contains(binOpt))
                    return false;
            }
            foreach (ConfigurationOption confOpt in this.binNumOptions)
            {
                if (confOpt is BinaryOption && selectedBinOptions.Contains(confOpt) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the participating options of this interaction are solely binary options.
        /// </summary>
        /// <returns>True if only binary options interact, false otherwise.</returns>
        public bool isBinaryInteraction()
        {
            return this.binaryOptions.Count > 0 && this.numericOptions.Count == 0 && this.binNumOptions.Count == 0;
        }

        /// <summary>
        /// Checks if the participating options of this interaction are solely numeric options.
        /// </summary>
        /// <returns>True if only numeric options interact, false otherwise.</returns>
        public bool isNumericInteraction()
        {
            return this.binaryOptions.Count == 0 && this.numericOptions.Count > 0 && this.binNumOptions.Count == 0;
        }

        /// <summary>
        /// Checks whether the interaction occurs in the configuration. Checks only for binary options, because numeric options are assumed to be always present
        /// </summary>
        /// <param name="c">The configuration for which we check whether it contains the interaction.</param>
        /// <returns>True if the interaction is present in the configuration, false otherwise.</returns>
        public bool isInConfiguration(List<BinaryOption> selectedBinOptions)
        {
            foreach (var binOpt in this.binaryOptions)
            {
                if (!selectedBinOptions.Contains(binOpt))
                    return false;
            }
            foreach (ConfigurationOption confOpt in this.binNumOptions)
            {
                if (confOpt is BinaryOption && selectedBinOptions.Contains(confOpt) == false)
                    return false;
            }
            return true;
        }
    }
}
