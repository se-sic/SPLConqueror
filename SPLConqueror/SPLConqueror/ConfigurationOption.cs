using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class ConfigurationOption : IComparable<ConfigurationOption>
    {
        private String name = "";

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String prefix = "";

        /// <summary>
        /// Prefix may be required when printing a configuration (e.g., "-")
        /// </summary>
        public String Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        private String postfix = "";

        /// <summary>
        /// Postfix may be required when printing a configuration
        /// </summary>
        public String Postfix
        {
            get { return postfix; }
            set { postfix = value; }
        }

        private List<List<ConfigurationOption>> implied_Options = new List<List<ConfigurationOption>>();

        /// <summary>
        /// List, in which the current option implies one and/or a combination of other options
        /// </summary>
        public List<List<ConfigurationOption>> Implied_Options
        {
            get { return implied_Options; }
            set { implied_Options = value; }
        }

        private List<List<ConfigurationOption>> excluded_Options = new List<List<ConfigurationOption>>();
        
        /// <summary>
        /// List, in which the current option excludes the selection of one and/or a combination of other options
        /// </summary>
        public List<List<ConfigurationOption>> Excluded_Options
        {
            get { return excluded_Options; }
            set { excluded_Options = value; }
        }

        private VariabilityModel vm = null;

        private ConfigurationOption parent = null;

        /// <summary>
        /// This options implies the selection of its parent (hence, it is also present in the implied_Options field
        /// </summary>
        public ConfigurationOption Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private List<ConfigurationOption> children = new List<ConfigurationOption>();

        /// <summary>
        /// This option's child options. These are not necessarily implied.
        /// </summary>
        internal List<ConfigurationOption> Children
        {
            get { return children; }
            set { children = value; }
        }



        public ConfigurationOption(VariabilityModel vm, String name)
        {
            this.vm = vm;
            this.name = name;
        }

        public int CompareTo(ConfigurationOption other)
        {
            return this.name.CompareTo(other.name);
        }
   

    }
}
