using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLConqueror_Core;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Persistence
{
    public class PersistGlobalState
    {

        private PersistGlobalState()
        {

        }

        /// <summary>
        /// String that represents the current global state object to save it as persistent file.
        /// </summary>
        /// <returns>GlobalState as string.</returns>
        public static string dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<globalState>\n");
            sb.Append("  <vmSource>\n");
            sb.Append("  " + GlobalState.vmSource + "\n");
            sb.Append("  </vmSource>\n");
            sb.Append("  <measurementSource>\n");
            sb.Append("  " + GlobalState.measurementSource + "\n");
            sb.Append("  </measurementSource>\n");
            sb.Append("  <currentNFP>\n");
            sb.Append("  " + GlobalState.currentNFP.Name + "\n");
            sb.Append("  </currentNFP>\n");
            sb.Append("  <logInfo>\n");
            sb.Append("  " + GlobalState.logInfo.ToString() + "\n");
            sb.Append("  </logInfo>\n");
            sb.Append("  <logError>\n");
            sb.Append("  " + GlobalState.logError.ToString() + "\n");
            sb.Append("  </logError>\n");
            sb.Append("</globalState>");
            return sb.ToString();
        }

        /// <summary>
        /// Try to recover the state from log files.
        /// </summary>
        /// <param name="logFile">Log files</param>
        public void recoverFromLogFile(params string[] logFile)
        {
            GlobalState.rollback = true;
        }

        /// <summary>
        /// Recover a older Global State from the persistent dump.
        /// </summary>
        /// <param name="persistentDump">Global state object as string</param>
        public static void recoverFromPersistentDump(string persistentDump)
        {
            GlobalState.rollback = true;
            XmlDocument persistentGlobalState = new System.Xml.XmlDocument();
            persistentGlobalState.Load(persistentDump);
            XmlElement globalState = persistentGlobalState.DocumentElement;
            foreach (XmlElement data in globalState)
            {
                switch (data.Name)
                {
                    case "measurementSource":
                        GlobalState.allMeasurements.Configurations = (GlobalState.allMeasurements.Configurations.Union(ConfigurationReader.readConfigurations(data.InnerText.Trim(), GlobalState.varModel))).ToList();
                        GlobalState.measurementSource = data.InnerText.Trim();
                        break;
                    case "vmSource":
                        GlobalState.vmSource = data.InnerText.Trim();
                        GlobalState.varModel = VariabilityModel.loadFromXML(data.InnerText.Trim());
                        break;
                    case "currentNFP":
                        GlobalState.currentNFP = GlobalState.getOrCreateProperty(data.InnerText.Trim());
                        break;
                    case "logInfo":
                        if (data.InnerText.Trim() == "null")
                        {
                            GlobalState.logInfo = new InfoLogger(null);
                        }
                        else
                        {
                            GlobalState.logInfo = new InfoLogger(data.InnerText.Trim(), true);
                        }
                        break;
                    case "logError":
                        if (data.InnerText.Trim() == "null")
                        {
                            GlobalState.logError = new ErrorLogger(null);
                        }
                        else
                        {
                            GlobalState.logError = new ErrorLogger(data.InnerText.Trim(), true);
                        }
                        break;
                }
            }
            GlobalState.logInfo.logLine("Recovered GlobalState from dump");

        }
    }
}
