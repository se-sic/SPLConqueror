using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    public abstract class ConfigurationOption : IComparable<ConfigurationOption>
    {
        private String name = "";

        public String Name
        {
            get { return name; }
            set {
                if (!value.All(Char.IsLetter))
                    this.name = removeInvalidChars(value);
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
        /// This options implies the selection of its parent (hence, it is also present in the implied_Options field
        /// </summary>
        public ConfigurationOption Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        

        private List<ConfigurationOption> children = new List<ConfigurationOption>();
        private List<String> children_names = new List<String>();

        /// <summary>
        /// This option's child options. These are not necessarily implied.
        /// </summary>
        public List<ConfigurationOption> Children
        {
            get { return children; }
            set { children = value; }
        }



        public ConfigurationOption(VariabilityModel vm, String name)
        {
            this.vm = vm;
            if (!name.All(Char.IsLetter))
                this.name = removeInvalidChars(name);
            else
                this.name = name;
        }

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

            //children
            XmlNode childrenNode = doc.CreateNode(XmlNodeType.Element, "children", "");
            foreach (ConfigurationOption co in this.children)
            {
                XmlNode childNode = doc.CreateNode(XmlNodeType.Element, "option", "");
                childNode.InnerText = co.Name;
                childrenNode.AppendChild(childNode);
            }
            node.AppendChild(childrenNode);

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
                        this.name = xmlInfo.InnerText;
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
                    case "children":
                        foreach (XmlElement elem in xmlInfo.ChildNodes)
                            children_names.Add(elem.InnerText);
                        break;
                    case "impliedOptions":
                        foreach (XmlElement elem in xmlInfo.ChildNodes)
                            implied_Options_names.Add(elem.InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        break;
                    case "excludedOptions":
                        foreach (XmlElement elem in xmlInfo.ChildNodes)
                            excluded_Options_names.Add(elem.InnerText.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        break;
                }
            }
        }

        /// <summary>
        /// Replaces the names for parent, children, etc. with their actual objects of the variability model
        /// </summary>
        internal void init()
        {
            this.parent = vm.getBinaryOption(parentName);
            foreach (var name in this.children_names)
            {
                ConfigurationOption c = vm.getBinaryOption(name);
                if(c == null)
                    c = vm.getNumericOption(name);
                if(c == null)
                    continue;
                this.children.Add(c);
            }

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

        private String removeInvalidChars(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (!Char.IsLetter(c))
                    continue;
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void updateChildren()
        {
            foreach(ConfigurationOption other in vm.getOptions())
            {
                if(other.parent != null && other.parent.Equals(this) && !this.children.Contains(other))
                    this.children.Add(other);
            }
        }

    }
}
