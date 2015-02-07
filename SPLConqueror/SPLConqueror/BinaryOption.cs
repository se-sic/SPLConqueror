using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class BinaryOption : ConfigurationOption
    {
        /// <summary>
        /// A binary feature can either be selected or selected in a specific configuration of a programm.
        /// </summary>
        public enum Value {
            Selected = "selected",
            Deselected = "deselected"
        };

        /// <summary>
        /// The default value of a binary option.
        /// </summary>
        public Value defaultValue { get; set; }

        public BinaryOption(VariabilityModel vm, String name)
            : base(vm, name)
        {

        }

    }
}
