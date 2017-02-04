using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLConqueror_Core;
using System.Xml.Serialization;
using System.IO;

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
        /// <returns></returns>
        public static string dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<globalState>\n");
            sb.Append("  <measurementSource>\n");
            sb.Append("  " + GlobalState.measurementSource + "\n");
            sb.Append("  </measurementSource>\n");
            sb.Append("  <vmSource>\n");
            sb.Append("  " + GlobalState.vmSource + "\n");
            sb.Append("  </vmSource>\n");
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

        }

        /// <summary>
        /// Recover a older Global State from the persistent dump.
        /// </summary>
        /// <param name="persistentDump">Global state object as string</param>
        public void recoverFromPersistentDump(string persistentDump)
        {

        }
    }
}
