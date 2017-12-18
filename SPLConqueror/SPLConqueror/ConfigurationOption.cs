using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class encapsulates all properties that have binary and numeric options in common.  
    /// </summary>
    public abstract class ConfigurationOption : IComparable<ConfigurationOption>, IEquatable<ConfigurationOption>
    {
        private String name = "";

        /// <summary>
        /// The name of the configuration option.
        /// </summary>
        public String Name
        {
            get { return name; }
            set {
                if (!value.All(Char.IsLetter))
                    this.name = removeInvalidCharsFromName(value);
                else
                    this.name = value;
                }
        }

        private String outputString = "";

        /// <summary>
        /// Textual representation of the configuration to use it as an parameter/configuration option of a customizable system. 
        /// </summary>
        public String OutputString
        {
            get { return outputString; }
            set { outputString = value; }
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
        private List<List<String>> implied_Options_names = new List<List<String>>();

        /// <summary>
        /// List, in which the current option implies one and/or a combination of other options
        /// </summary>
        public List<List<ConfigurationOption>> Implied_Options
        {
            get { return implied_Options; }
            set { implied_Options = value; }
        }

        private List<List<ConfigurationOption>> excluded_Options = new List<List<ConfigurationOption>>();
        private List<List<String>> excluded_Options_names = new List<List<string>>();
 
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
        private String parentName = "";
        
        /// <summary>
        /// Restuns the name of the parent feature.
        /// </summary>
        public String ParentName
        {
            get { return parentName; }
            set { parentName = value;  }
        }

        /// <summary>
        /// This options implies the selection of its parent (hence, it is also present in the implied_Options field
        /// </summary>
        public ConfigurationOption Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// A list of all configuration options with this option as parent.
        /// </summary>
        public List<ConfigurationOption> Children
        {
            get { return children; }
            set { children = value; }
        }

        private List<ConfigurationOption> children = new List<ConfigurationOption>();

        /// <summary>
        /// Creates a new configuration option of the given name for the variability model. 
        /// </summary>
        /// <param name="vm">The variability model, the option is defined for.</param>
        /// <param name="name">The name of the configuration option.</param>
        public ConfigurationOption(VariabilityModel vm, String name)
        {
            this.vm = vm;
            if (!name.All(Char.IsLetter))
                this.name = removeInvalidCharsFromName(name);
            else
                this.name = name;
        }

        /// <summary>
        /// Compares two configuration option based on the names of the options.
        /// </summary>
        /// <param name="other">The configuration option ot compare with.</param>
        /// <returns></returns>
        public int CompareTo(ConfigurationOption other)
        {
            return this.name.CompareTo(other.name);
        }

        /// <summary>
        /// Stores the configuration option as an XML Node
        /// </summary>
        /// <param name="doc">The XML document to which the node will be added</param>
        /// <returns>XmlNode containing information about the option</returns>
        internal XmlNode saveXML(XmlDocument doc)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "configurationOption", "");
            
            //Name
            XmlNode nameNode = doc.CreateNode(XmlNodeType.Element, "name", "");
            nameNode.InnerText = this.name;
            node.AppendChild(nameNode);

            //outputString
            XmlNode outputStringNode = doc.CreateNode(XmlNodeType.Element, "outputString", "");
            outputStringNode.InnerText = this.outputString;
            node.AppendChild(outputStringNode);

            //prefix
            XmlNode prefixNode = doc.CreateNode(XmlNodeType.Element, "prefix", "");
            prefixNode.InnerText = this.prefix;
            node.AppendChild(prefixNode);

            //postfix
            XmlNode postfixNode = doc.CreateNode(XmlNodeType.Element, "postfix", "");
            postfixNode.InnerText = this.postfix;
            node.AppendChild(postfixNode);

            //parent
            XmlNode parentNode = doc.CreateNode(XmlNodeType.Element, "parent", "");
            if (this.parent != null)
                parentNode.InnerText = this.parent.Name;
            else
                parentNode.InnerText = "";
            node.AppendChild(parentNode);

            //implied_options
            XmlNode implNode = doc.CreateNode(XmlNodeType.Element, "impliedOptions", "");
            foreach (List<ConfigurationOption> impOptions in this.implied_Options)
            {
                XmlNode implies = doc.CreateNode(XmlNodeType.Element, "options", "");
                StringBuilder sb = new StringBuilder();
                foreach (var opt in impOptions)
                {
                    sb.Append(opt.Name + " | ");
                }
                sb.Remove(sb.Length - 3, 3);
                implies.InnerText = sb.ToString();
                implNode.AppendChild(implies);
            }
            node.AppendChild(implNode);

            //excluded_options
            XmlNode exclNode = doc.CreateNode(XmlNodeType.Element, "excludedOptions", "");
            foreach (List<ConfigurationOption> exOptions in this.excluded_Options)
            {
                XmlNode excludes = doc.CreateNode(XmlNodeType.Element, "options", "");
                StringBuilder sb = new StringBuilder();
                foreach (var opt in exOptions)
                {
                    sb.Append(opt.Name + " | ");
                }
                sb.Remove(sb.Length - 3, 3);
                excludes.InnerText = sb.ToString();
                exclNode.AppendChild(excludes);
            }
            node.AppendChild(exclNode);

            return node;
        }

        internal void loadFromXML(XmlElement node)
        {
            foreach (XmlElement xmlInfo in node.ChildNodes)
            {
                switch (xmlInfo.Name)
                {
                    case "name":
                        this.Name = xmlInfo.InnerText;
                        break;
                    case "outputString":
                        this.outputString = xmlInfo.InnerText;
                        break;
                    case "prefix":
                        this.prefix = xmlInfo.InnerText;
                        break;
                    case "postfix":
                        this.postfix = xmlInfo.InnerText;
                        break;
                    case "parent":
                        this.parentName = xmlInfo.InnerText;
                        break;
                    case "impliedOptions":
                        foreach (XmlElement elem in xmlInfo.ChildNodes)
                            implied_Options_names.Add(elem.InnerText.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        break;
                    case "excludedOptions":
                        foreach (XmlElement elem in xmlInfo.ChildNodes)
                            excluded_Options_names.Add(elem.InnerText.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        break;
                }
            }
        }

        /// <summary>
        /// Replaces the names for parent, children, etc. with their actual objects of the variability model
        /// </summary>
        internal void init()
        {
            if (ParentName != null)
            {
                this.parent = vm.getBinaryOption(parentName);
            }

            if (this.parent != null)
                this.parent.Children.Add(this);

            foreach (var imply_names in this.implied_Options_names)
            {
                List<ConfigurationOption> optionImplies = new List<ConfigurationOption>();
                foreach (var optName in imply_names)
                {
                    ConfigurationOption c = vm.getBinaryOption(optName);
                    if (c == null)
                        c = vm.getNumericOption(optName);
                    if (c == null)
                        continue;
                    optionImplies.Add(c);
                }
                this.implied_Options.Add(optionImplies);
            }

            foreach (var exclud_names in this.excluded_Options_names)
            {
                List<ConfigurationOption> excImplies = new List<ConfigurationOption>();
                foreach (var optName in exclud_names)
                {
                    ConfigurationOption c = vm.getBinaryOption(optName);
                    if (c == null)
                        c = vm.getNumericOption(optName);
                    if (c == null)
                        continue;
                    excImplies.Add(c);
                }
                this.excluded_Options.Add(excImplies);
            }
        }

        /// <summary>
        /// Checks whether the given option is an ancestor of the current option (recursive method).
        /// </summary>
        /// <param name="optionToCompare">The configuration option that might be an ancestor.</param>
        /// <returns>True if it is an ancestor, false otherwise</returns>
        public bool isAncestor(ConfigurationOption optionToCompare)
        {
            if (this.Parent != null)
            {
                if (this.Parent == optionToCompare)
                    return true;
                else
                    return this.Parent.isAncestor(optionToCompare);
            }
            return false;
        }

        /// <summary>
        /// Sets this configuration option as parent for all binary option 
        /// in a model, that have this option as parent.
        /// </summary>
        /// <param name="vm">The model that will be used to update the children.</param>
        public void updateChildren(VariabilityModel vm)
        {
            foreach (ConfigurationOption option in vm.BinaryOptions)
            {
                if (option.parentName != null && option.parentName.Equals(this.name))
                {
                    option.Parent = this;
                }
            }
        }

        /// <summary>
        /// This method removes all characters form the string that are neither a letter nor '_'. This is necessary because a valid mane for a configuration option should only contains this characters.  
        /// </summary>
        /// <param name="s">The desired name for a configuration option.</param>
        /// <returns>A valid name for the configuration option.</returns>
        public static String removeInvalidCharsFromName(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!Char.IsLetter(c) && !c.Equals('_') && !Char.IsNumber(c))
                    continue;
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// String reprensentation of the configuration option.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Compares this configuration option with the option given as parameter.
        /// </summary>
        /// <param name="other">The configuration option to compare to.</param>
        /// <returns>True if both options are the same.</returns>
        public bool Equals(ConfigurationOption other)
        {
            return this.name.Equals(other.name);
        }

        public static bool hasConstraint(List<List<ConfigurationOption>> list, List<ConfigurationOption> newContstraint)
        {
            foreach(List<ConfigurationOption> existingConstraint in list)
            {
                if (! (existingConstraint.Count == newContstraint.Count))
                    continue;

                if ((newContstraint.Union(existingConstraint)).ToList().Count == existingConstraint.Count)
                    return true;
            }
            return false;
        }
    }
}
