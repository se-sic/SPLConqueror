using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    /// <summary>
    /// Central model to store all configuration options and their constraints.
    /// </summary>
    public class VariabilityModel
    {
        private List<NumericOption> numericOptions = new List<NumericOption>();

        public List<NumericOption> NumericOptions
        {
            get { return numericOptions; }
            //set { numericOptions = value; }
        }

        private List<BinaryOption> binaryOptions = new List<BinaryOption>();

        public List<BinaryOption> BinaryOptions
        {
            get { return binaryOptions; }
          //  set { binaryOptions = value; }
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

        private BinaryOption root = null;

        public BinaryOption Root
        {
            get { return root; }
        }

        private List<String> booleanConstraints = new List<string>();

        public List<String> BooleanConstraints
        {
            get { return booleanConstraints; }
            set { booleanConstraints = value; }
        }

        public VariabilityModel(String name)
        {
            this.name = name;
            root = new BinaryOption(this, "root");
            this.BinaryOptions.Add(root);
        }

        /// <summary>
        /// Stores the current variability model into an XML file with the already stored path
        /// </summary>
        /// <returns>Returns true if sucessfully saved, false otherwise</returns>
        public bool saveXML()
        {
            if (this.path.Length > 0)
                return saveXML(this.path);
            else
                return false;
        }

        /// <summary>
        /// Stores the current variability model into an XML file
        /// </summary>
        /// <param name="path">Path to which the XML file is stored</param>
        /// <returns>Returns false if the saving was not successfull</returns>
        public bool saveXML(String path)
        {
            //Create XML Document
            XmlDocument doc = new XmlDocument();

            //Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);

            //Add a root node to the document.
            XmlNode xmlroot = doc.CreateNode(XmlNodeType.Element, "vm", ""); // ***

            XmlAttribute xmlattr = doc.CreateAttribute("name");
            xmlattr.Value = this.name;
            xmlroot.Attributes.Append(xmlattr);

            //Add binary options
            XmlNode xmlBin = doc.CreateNode(XmlNodeType.Element, "binaryOptions", "");
            foreach (BinaryOption binOpt in this.binaryOptions)
            {
                xmlBin.AppendChild(binOpt.saveXML(doc));
            }
            xmlroot.AppendChild(xmlBin);

            //Add numeric options
            XmlNode xmlNum = doc.CreateNode(XmlNodeType.Element, "numericOptions", "");
            foreach (NumericOption numOpt in this.numericOptions)
            {
                xmlNum.AppendChild(numOpt.saveXML(doc));
            }
            xmlroot.AppendChild(xmlNum);

            //Add boolean constraints
            XmlNode boolConstraints = doc.CreateNode(XmlNodeType.Element, "booleanConstraints", "");
            foreach (var constraint in this.booleanConstraints)
            {
                XmlNode conNode = doc.CreateNode(XmlNodeType.Element, "constraint", "");
                conNode.InnerText = constraint;
                boolConstraints.AppendChild(conNode);
            }

            xmlroot.AppendChild(boolConstraints);

            doc.AppendChild(xmlroot);

            try
            {
                doc.Save(path);
            }
            catch (Exception e)
            {
                ErrorLog.logError(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Static method that reads an xml file and constructs a variability model using the stored information
        /// </summary>
        /// <param name="path">Path to the XML File</param>
        /// <returns>The intatiated variability model</returns>
        public static VariabilityModel loadFromXML(String path)
        {
            VariabilityModel vm = new VariabilityModel("temp");
            vm.loadXML(path);
            return vm;
        }

        /// <summary>
        /// Loads an XML file containing information about the variability model.
        /// </summary>
        /// <param name="path">Path to the XML file</param>
        public void loadXML(String path)
        {
            XmlDocument dat = new System.Xml.XmlDocument();
            dat.Load(path);
            XmlElement currentElemt = dat.DocumentElement;
            this.name = currentElemt.Attributes["name"].Value.ToString();

            foreach (XmlElement xmlNode in currentElemt.ChildNodes)
            {
                switch (xmlNode.Name)
                {
                    case "binaryOptions":
                        loadBinaryOptions(xmlNode);
                        break;
                    case "numericOptions":
                        loadNumericOptions(xmlNode);
                        break;
                    case "booleanConstraints":
                        loadBooleanConstraints(xmlNode);
                        break;
                }
            }

            initOptions();
        }

        /// <summary>
        /// After loading all options, we can replace the names for children, the parent, etc. with the actual objects
        /// </summary>
        private void initOptions()
        {
            foreach (var binOpt in binaryOptions)
                binOpt.init();
            foreach (var numOpt in binaryOptions)
                numOpt.init();
        }

        private void loadBooleanConstraints(XmlElement xmlNode)
        {
            foreach (XmlElement boolConstr in xmlNode.ChildNodes)
            {
                this.booleanConstraints.Add(boolConstr.InnerText);
            }
        }

        private void loadNumericOptions(XmlElement xmlNode)
        {
            foreach (XmlElement numOptNode in xmlNode.ChildNodes)
            {
                if (addConfigurationOption(NumericOption.loadFromXML(numOptNode, this)) == false)
                    ErrorLog.logError("Could not add option to the variability model. Possible reasons: invalid name, option already exists.");
            }
        }

        private void loadBinaryOptions(XmlElement xmlNode)
        {
            foreach (XmlElement binOptNode in xmlNode.ChildNodes)
            {
                if (addConfigurationOption(BinaryOption.loadFromXML(binOptNode, this)) == false)
                    ErrorLog.logError("Could not add option to the variability model. Possible reasons: invalid name, option already exists.");
            }
        }

        /// <summary>
        /// Adds a configuration option to the variability model.
        /// The method checks whether an option with the same name already exists and whether invalid characters are within the name
        /// </summary>
        /// <param name="option">The option to be added to the variability model.</param>
        /// <returns>True if the option was added to the model, false otherwise</returns>
        public bool addConfigurationOption(ConfigurationOption option)
        {
            if (option.Name.Contains('-') || option.Name.Contains('+'))
                return false;

            // the vitrual root configuration option does not have to be added to the variability model. 
            if (option.Name.Equals("root"))
                return true;

            foreach (var opt in binaryOptions)
            {
                if (opt.Name.Equals(option.Name))
                    return false;
            }
            foreach (var opt in numericOptions)
            {
                if (opt.Name.Equals(option.Name))
                    return false;
            }

            //Every option must have a parent
            if (option.Parent == null)
                option.Parent = this.root;
            if (option is BinaryOption)
                this.binaryOptions.Add((BinaryOption)option);
            else
                this.numericOptions.Add((NumericOption)option);
            return true;
        }

        /// <summary>
        /// Searches for a binary option with the given name
        /// </summary>
        /// <param name="name">Name of the option</param>
        /// <returns>Either the binary option with the given name or NULL if not found</returns>
        public BinaryOption getBinaryOption(String name)
        {
            foreach (var binO in binaryOptions)
            {
                if (binO.Name.Equals(name))
                    return binO;
            }
            return null;
        }

        /// <summary>
        /// Searches for a numeric option with the given name
        /// </summary>
        /// <param name="name">Name of the option</param>
        /// <returns>Either the numeric option with the given name or NULL if not found</returns>
        public NumericOption getNumericOption(String name)
        {
            foreach (var numO in numericOptions)
            {
                if (numO.Name.Equals(name))
                    return numO;
            }
            return null;
        }
        
    }
}
