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

        public string dump(CommandHistory toDump)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(CommandHistory));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, toDump);
            return sw.ToString().Replace("utf-16", "utf-8");
        }

        public CommandHistory recoverFromDump(string path)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(CommandHistory));
            StreamReader sr = new StreamReader(path);
            object cmdHistory = xmls.Deserialize(sr);
            return (CommandHistory)cmdHistory;
        }
    }
}
