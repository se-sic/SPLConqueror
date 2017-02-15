using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Persistence
{
    public class PersistCommandHistory
    {
        private PersistCommandHistory() { }

        public static string dump(CommandHistory toDump)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<string>));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, toDump.commandHistory.ToList());
            return sw.ToString().Replace("utf-16", "utf-8");
        }

        public static CommandHistory recoverFromDump(string path)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<string>));
            StreamReader sr = new StreamReader(path);
            List<string> cmdHistoryList = (List<string>)xmls.Deserialize(sr);
            CommandHistory cmdHistory = new CommandHistory();
            foreach (string command in cmdHistoryList)
            {
                cmdHistory.addCommand(command);
            }
            return cmdHistory;
        }
    }
}
