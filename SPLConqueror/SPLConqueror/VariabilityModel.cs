using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// Central model to store all configuration options and their constraints
    /// </summary>
    public class VariabilityModel
    {
        List<NumericOption> numericOptions = new List<NumericOption>();

        internal List<NumericOption> NumericOptions
        {
            get { return numericOptions; }
            set { numericOptions = value; }
        }

        List<BinaryOption> binaryOptions = new List<BinaryOption>();

        internal List<BinaryOption> BinaryOptions
        {
            get { return binaryOptions; }
            set { binaryOptions = value; }
        }

        String name = "empty";

        /// <summary>
        /// Name of the variability model or configurable program
        /// </summary>
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        String path = "";

        /// <summary>
        /// Local path, in which the model is located
        /// </summary>
        public String Path
        {
            get { return path; }
            set { path = value; }
        }
    }
}
