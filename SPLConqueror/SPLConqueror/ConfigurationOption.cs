using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    abstract class ConfigurationOption : IComparable<ConfigurationOption>
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

        
    }
}
