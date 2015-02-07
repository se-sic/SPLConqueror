using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SPLConqueror_Core
{
    /// <summary>
    /// Central model to store all configuration options and their constraints
    /// </summary>
    public class VariabilityModel
    {
        private List<NumericOption> numericOptions = new List<NumericOption>();

        internal List<NumericOption> NumericOptions
        {
            get { return numericOptions; }
            //set { numericOptions = value; }
        }

        private List<BinaryOption> binaryOptions = new List<BinaryOption>();

        internal List<BinaryOption> BinaryOptions
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



        /*
         * if (currentElemt.ChildNodes[i].Name == "furtherConstraints")
					{
						foreach(XmlNode constraint in currentElemt.ChildNodes[i].ChildNodes)
						{
							furtherConstraints.Add(constraint.InnerText);
							if (constraint.InnerText.ToLower().Contains("derivative"))
							{//-LoggingBase | -Evictor | -Statistics | -LoggingEvictor | Derivative_LoggingEvictor_Statistics_Evictor_LoggingBase
								Element derivative;
								List<Element> parents;
								if (identifyImpliesPattern(constraint.InnerText, out derivative, out parents))
								{
									derivative.addDerivativeParents(parents);
								}
								else
								{
									this.errormsg += "Unresolved derivative constraint: " + constraint.InnerText + "\n";
								}
							}                            
						}
					}*/
    }
}
