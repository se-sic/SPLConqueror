using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class Persistence
    {
        private Persistence() { }

        public static CommandHistory history = new CommandHistory();

        /// <summary>
        /// Simulate the programm flow and find all relevant Commands
        /// </summary>
        /// <param name="aScript">A script file with the commands.</param>
        /// <returns>Dictionary with the relevant commands and the value that has to be set to restore the state</returns>
        public static Dictionary<string, string> findRelevantCommandsLogFiles(string aScript)
        {
            Dictionary<string, string> relevantCommands = new Dictionary<string, string>();
            StreamReader aScriptReader = new StreamReader(aScript);
            StreamReader logReader = null;
            while (!aScriptReader.EndOfStream)
            {
                string command;
                string line = aScriptReader.ReadLine();
                line = line.Split(new Char[] { '#' }, 2)[0];
                string[] components = line.Split(new Char[] { ' ' }, 2);
                command = components[0];
                string task = "";
                if (components.Length > 1)
                    task = components[1];
                string[] taskAsParameter = task.Split(new Char[] { ' ' });
                switch (command)
                {
                    case "log":
                        Tuple<bool, StreamReader> wasPerformedAndLogReader = reconstructLogCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformedAndLogReader.Item1)
                        {
                            return relevantCommands;
                        }
                        else
                        {
                            logReader = wasPerformedAndLogReader.Item2;
                            history.addCommand(line);
                        }
                        break;

                    case "mlsettings":
                        break;
                }
            }
            return relevantCommands;
        }

        private static bool logExists(string log)
        {
            try
            {
                StreamReader sr = new StreamReader(log);
                sr.Close();
                return true;
            }
            catch (FileNotFoundException exc)
            {
                return false;
            }
        }

        private static Tuple<bool, StreamReader> reconstructLogCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            if (logReader != null)
            {
                string lineInLog = "command: " + commandLine;
                if (lineInLog.Equals(logReader.ReadLine()))
                {
                    if (logExists(task.Trim()))
                    {
                        try
                        {
                            relevantCommands.Add(command, task);
                        }
                        catch (ArgumentException argexc)
                        {
                            relevantCommands.Remove(command);
                            relevantCommands.Add(command, task);
                        }
                        return Tuple.Create(true, logReader);
                    }
                    else
                    {
                        return Tuple.Create(false, logReader);
                    }
                }
                else
                {
                    return Tuple.Create(false, logReader);
                }
            }
            else
            {
                if (!logExists(task.Trim()))
                {
                    return Tuple.Create(false, logReader);
                }
                FileStream ostrm = new FileStream(task.Trim(), FileMode.OpenOrCreate, FileAccess.Read);
                logReader = new StreamReader(ostrm);
                try
                {
                    relevantCommands.Add(command, task);
                }
                catch (ArgumentException argexc)
                {
                    relevantCommands.Remove(command);
                    relevantCommands.Add(command, task);
                }
                return Tuple.Create(true, logReader);
            }
        }

        private static bool reconstructMLSettingsCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            return true;
        }
    }
}
