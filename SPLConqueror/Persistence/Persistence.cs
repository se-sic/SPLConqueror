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

        public static List<string> learningHistory;

        /// <summary>
        /// Simulate the programm flow and find all relevant Commands
        /// </summary>
        /// <param name="aScript">A script file with the commands.</param>
        /// <returns>Tuple with a dictionary with the relevant commands and the value that has to be set to restore the state
        /// and a bool value indicating if the end was reached.</returns>
        public static Tuple<bool, Dictionary<string, string>> findRelevantCommandsLogFiles(string aScript, Dictionary<string, string> performedCommands, StreamReader log = null)
        {
            Dictionary<string, string> relevantCommands = performedCommands;
            StreamReader aScriptReader = new StreamReader(aScript);
            StreamReader logReader = log;
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
                            return Tuple.Create(wasPerformedAndLogReader.Item1, relevantCommands);
                        }
                        else
                        {
                            logReader = wasPerformedAndLogReader.Item2;
                            history.addCommand(line);
                        }
                        break;

                    case "mlsettings":
                        bool wasPerformed = reconstructMLSettingsCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "nfp":
                        wasPerformed = reconstructNfpCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "vm":
                        wasPerformed = reconstructVMCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "featurewise":
                    case "random":
                    case "pairwise":
                    case "negfw":
                    case "allbinary":
                        wasPerformed = reconstructBinarySampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "expdesign":
                        wasPerformed = reconstructNumericSampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "all":
                        wasPerformed = reconstructReadingMeasurements(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "script":
                        Tuple<bool, Dictionary<string, string>> subscriptResults = reconstructRecursiveAScript(logReader, relevantCommands, task, command, line);
                        if (!subscriptResults.Item1)
                        {
                            return subscriptResults;
                        }
                        else
                        {
                            relevantCommands = subscriptResults.Item2;
                            history.addCommand(line);
                        }
                        break;

                    case "load_mlsettings":
                        wasPerformed = reconstructLoadMLSettingsCommand(logReader, relevantCommands, task, "mlsettings", line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "clean-sampling":
                        wasPerformed = reconstructCleanSampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "clean-global":
                        Tuple<bool, Dictionary<string, string>> wasPerformedAndNewState = reconstructCleanGlobal(logReader, relevantCommands, task, command, line);
                        if (!wasPerformedAndNewState.Item1)
                        {
                            return Tuple.Create(wasPerformedAndNewState.Item1, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                            relevantCommands = wasPerformedAndNewState.Item2;
                        }
                        break;

                    case "learnwithallmeasurements":
                        wasPerformed = reconstructLearnWithAllMeasurements(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "analyze-learning":
                        wasPerformed = reconstructAnalyzeLearning(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;

                    case "clean-learning":
                        wasPerformed = reconstructCleanLearning(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            learningHistory = null;
                            history.addCommand(line);
                        }
                        break;

                    case "start":
                        wasPerformed = reconstructLearn(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line);
                        }
                        break;
                }
            }
            return Tuple.Create(true, relevantCommands);
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
                        addOrReplace(relevantCommands, command, task);
                        FileStream ostrm = new FileStream(task.Trim(), FileMode.Open, FileAccess.Read);
                        logReader = new StreamReader(ostrm);
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
                addOrReplace(relevantCommands, command, task);
                return Tuple.Create(true, logReader);
            }
        }

        private static void addOrReplace(Dictionary<string, string> dict, string key, string value)
        {
            try
            {
                dict.Add(key, value);
            }
            catch (ArgumentException argexc)
            {
                dict.Remove(key);
                dict.Add(key, value);
            }
        }

        private static bool reconstructCleanSampling(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                relevantCommands.Remove("expdesign");
                relevantCommands.Remove("expdesign validation");
                relevantCommands.Remove("featurewise");
                relevantCommands.Remove("random");
                relevantCommands.Remove("pairwise");
                relevantCommands.Remove("negfw");
                relevantCommands.Remove("allbinary");
                relevantCommands.Remove("featurewise validation");
                relevantCommands.Remove("random validation");
                relevantCommands.Remove("pairwise validation");
                relevantCommands.Remove("negfw validation");
                relevantCommands.Remove("allbinary validation");
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool reconstructMLSettingsCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
        }

        private static bool reconstructLoadMLSettingsCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
        }

        private static bool reconstructNfpCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
        }

        private static bool reconstructVMCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
        }

        private static bool reconstructBinarySampling(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            if (commandLine.Contains("validation"))
            {
                return reconstructValidationSampling(logReader, relevantCommands, task, command, commandLine);
            }
            else
            {
                return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
            }
        }

        private static bool reconstructNumericSampling(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            if (commandLine.Contains("validation"))
            {
                return reconstructValidationSampling(logReader, relevantCommands, task, command, commandLine);
            }
            else
            {
                return reconstructTrivialCommand(logReader, relevantCommands, task, command, commandLine);
            }
        }

        private static bool reconstructValidationSampling(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                addOrReplace(relevantCommands, command + " validation", (commandLine.Replace(command, "").Replace("validation", "")));
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Tuple<bool, Dictionary<string, string>> reconstructCleanGlobal(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                Dictionary<string, string> newState = new Dictionary<string, string>();
                string logPath = null;
                relevantCommands.TryGetValue("log", out logPath);
                if (logPath == null)
                {
                    return Tuple.Create(true, newState);
                }
                else
                {
                    newState.Add("log", logPath);
                    return Tuple.Create(true, newState);
                }
            }
            else
            {
                return Tuple.Create(false, relevantCommands);
            }
        }

        private static bool reconstructLearnWithAllMeasurements(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                learningHistory = new List<string>();
                while (!logReader.EndOfStream)
                {
                    string line = logReader.ReadLine();
                    if (line.Contains(";"))
                    {
                        learningHistory.Add(line);
                    }
                    else if (line.StartsWith("Finished"))
                    {
                        addOrReplace(relevantCommands, task, commandLine);
                        return true;
                    }
                    else if (!(line.Contains("Elapsed") || line.Contains("NumberOfConfigurations")))
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private static bool reconstructAnalyzeLearning(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                while (!logReader.EndOfStream)
                {
                    string line = logReader.ReadLine();
                    if (line.Contains("Analyze finished") || line.Contains("learning"))
                    {
                        return true;
                    }
                    else if (!(line.Contains(";") || line.Contains("Termination reason") || line.Contains("Model")))
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private static bool reconstructReadingMeasurements(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                if (logReader.ReadLine().Contains("Configs with too large deviation"))
                {
                    if (logReader.ReadLine().Contains("onfigurations loaded"))
                    {
                        addOrReplace(relevantCommands, command, commandLine);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static bool reconstructTrivialCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                addOrReplace(relevantCommands, command, task.Trim());
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Tuple<bool, Dictionary<string, string>> reconstructRecursiveAScript(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                addOrReplace(relevantCommands, command, task.Trim());
                return findRelevantCommandsLogFiles(task, relevantCommands, logReader);
            }
            else
            {
                return Tuple.Create(false, relevantCommands);
            }
        }

        private static bool reconstructCleanLearning(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                learningHistory = null;
                return true;
            }
            {
                return false;
            }
        }

        private static bool reconstructLearn(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = "command: " + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                learningHistory = new List<string>();
                while (!logReader.EndOfStream)
                {
                    string line = logReader.ReadLine();
                    if (line.Contains(";"))
                    {
                        learningHistory.Add(line);
                    }
                    else if (line.StartsWith("Error: "))
                    {
                        addOrReplace(relevantCommands, task, commandLine);
                        return true;
                    }
                }
                return false;
            } else
            {
                return false;
            }
        }
    }
}
