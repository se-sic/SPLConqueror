using MachineLearning.Learning;
using MachineLearning.Learning.Regression;
using MachineLearning.Sampling;
using Persistence;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommandLine
{
    public class CommandPersistence
    {
        private CommandPersistence() { }

        /// <summary>
        /// Recovered command history.
        /// </summary>
        public static CommandHistory history = new CommandHistory();

        /// <summary>
        /// Recovered learning informations. Bool value indicates if it has to be relearned.
        /// </summary>
        public static Tuple<bool, List<string>> learningHistory;

        /// <summary>
        /// Dump the current state of SPLConqueror to Files.
        /// </summary>
        /// <param name="pathArray">Array with file paths. Needs 5 paths. Data will be stored in these files.</param>
        /// <param name="mlSettings">Current ML_Settings, to save.</param>
        /// <param name="toSample">Sampling strategies to save.</param>
        /// <param name="toSampleValidation">Validation sampling strategies to save.</param>
        /// <param name="exp">Learning object to save.</param>
        /// <param name="history">Command history to save.</param>
        public static void dump(string[] pathArray, ML_Settings mlSettings, List<SamplingStrategies> toSample, List<SamplingStrategies> toSampleValidation, Learning exp, CommandHistory history)
        {
            if (pathArray.Length >= 6)
            {
                StreamWriter sw = new StreamWriter(pathArray[0]);
                sw.Write(PersistGlobalState.dump());
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(pathArray[1]);
                sw.Write(PersistMLSettings.dump(mlSettings));
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(pathArray[2]);
                sw.Write(PersistSampling.dump(toSample));
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(pathArray[3]);
                sw.Write(PersistSampling.dump(toSampleValidation));
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(pathArray[4]);
                sw.Write(PersistLearning.dump(exp));
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(pathArray[5]);
                sw.Write(PersistCommandHistory.dump(history));
                sw.Flush();
                sw.Close();
            }
            else
            {
                GlobalState.logError.logLine("Couldnt dump the data. Not all target paths are given");
            }
        }

        /// <summary>
        /// Recover the SPLConqueror data from files.
        /// </summary>
        /// <param name="pathArray">String array with the file paths. Needs 6 file paths.</param>
        /// <returns>Tuple containing all recovered data.</returns>
        public static Tuple<ML_Settings, List<SamplingStrategies>, List<SamplingStrategies>> recoverDataFromDump(string[] pathArray)
        {
            if (pathArray.Length >= 6)
            {
                PersistGlobalState.recoverFromPersistentDump(pathArray[0]);
                ML_Settings mlSettings = PersistMLSettings.recoverFromPersistentDump(pathArray[1]);
                List<SamplingStrategies> toSample = PersistSampling.recoverFromDump(pathArray[2]);
                List<SamplingStrategies> toSampleValidation = PersistSampling.recoverFromDump(pathArray[3]);
                List<List<string>> learningRounds = PersistLearning.recoverFromPersistentDump(pathArray[4]);
                learningHistory = Tuple.Create(true, learningRounds.Last());
                history = PersistCommandHistory.recoverFromDump(pathArray[5]);
                return Tuple.Create(mlSettings, toSample, toSampleValidation);
            }
            else
            {
                GlobalState.logError.logLine("Couldnt recover from dump. Not all source paths are given");
                return null;
            }
        }

        /// <summary>
        /// Simulate the programm flow and find all relevant Commands
        /// </summary>
        /// <param name="aScript">A script file with the commands.</param>
        /// <param name="performedCommands">Empty dictionary to store the performed commands and arguments.</param>
        /// <param name="log">StreamReader that reads the logs of the A script. Should be null.</param>
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
                    case Commands.COMMAND_TRUEMODEL:
                        bool wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SAMPLING_OPTIONORDER:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_PRINT_CONFIGURATIONS:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_MEASUREMENTS_TO_CSV:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_PRINT_MLSETTINGS:
                        wasPerformed = reconstructPrintMLSettings(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_EVALUATION_SET:
                        wasPerformed = reconstructEvaluationSetCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SAVE:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_LOG:
                        Tuple<bool, StreamReader> wasPerformedAndLogReader = reconstructLogCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformedAndLogReader.Item1)
                        {
                            return Tuple.Create(wasPerformedAndLogReader.Item1, relevantCommands);
                        }
                        else
                        {
                            logReader = wasPerformedAndLogReader.Item2;
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SET_MLSETTING:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SET_NFP:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_VARIABILITYMODEL:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SAMPLE_OPTIONWISE:
                    case Commands.COMMAND_SAMPLE_BINARY_RANDOM:
                    case Commands.COMMAND_SAMPLE_PAIRWISE:
                    case Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE:
                    case Commands.COMMAND_SAMPLE_ALLBINARY:
                        wasPerformed = reconstructBinarySampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_EXPERIMENTALDESIGN:
                        wasPerformed = reconstructNumericSampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_LOAD_CONFIGURATIONS:
                        wasPerformed = reconstructReadingMeasurements(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_SUBSCRIPT:
                        history.addCommand(line.Trim());
                        Tuple<bool, Dictionary<string, string>> subscriptResults = reconstructRecursiveAScript(logReader, relevantCommands, task, command, line);
                        if (!subscriptResults.Item1)
                        {
                            logReader.Close();
                            return subscriptResults;
                        }
                        else
                        {
                            relevantCommands = subscriptResults.Item2;
                        }
                        break;

                    case Commands.COMMAND_LOAD_MLSETTINGS:
                        wasPerformed = reconstructTrivialCommand(logReader, relevantCommands, task, "mlsettings", line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_CLEAR_SAMPLING:
                        wasPerformed = reconstructCleanSampling(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_CLEAR_GLOBAL:
                        Tuple<bool, Dictionary<string, string>> wasPerformedAndNewState = reconstructCleanGlobal(logReader, relevantCommands, task, command, line);
                        if (!wasPerformedAndNewState.Item1)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformedAndNewState.Item1, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                            relevantCommands = wasPerformedAndNewState.Item2;
                        }
                        break;

                    case Commands.COMMAND_START_ALLMEASUREMENTS:
                        wasPerformed = reconstructLearnWithAllMeasurements(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            learningHistory = Tuple.Create(false, learningHistory.Item2);
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_ANALYZE_LEARNING:
                        wasPerformed = reconstructAnalyzeLearning(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_CLEAR_LEARNING:
                        wasPerformed = reconstructCleanLearning(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            learningHistory = null;
                            history.addCommand(line.Trim());
                        }
                        break;

                    case Commands.COMMAND_START_LEARNING:
                        wasPerformed = reconstructLearn(logReader, relevantCommands, task, command, line);
                        if (!wasPerformed)
                        {
                            logReader.Close();
                            return Tuple.Create(wasPerformed, relevantCommands);
                        }
                        else
                        {
                            learningHistory = Tuple.Create(false, learningHistory.Item2);
                            history.addCommand(line.Trim());
                        }
                        break;
                }
            }
            if (logReader.EndOfStream)
            {
                logReader.Close();
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
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        private static Tuple<bool, StreamReader> reconstructLogCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            if (logReader != null)
            {
                string lineInLog = Commands.COMMAND + commandLine;
                if (lineInLog.Equals(logReader.ReadLine()))
                {
                    if (logExists(task.Trim()))
                    {
                        addOrReplace(relevantCommands, command, commandLine);
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
                addOrReplace(relevantCommands, command, commandLine);
                return Tuple.Create(true, logReader);
            }
        }

        private static void addOrReplace(Dictionary<string, string> dict, string key, string value)
        {
            try
            {
                dict.Add(key, value);
            }
            catch (ArgumentException)
            {
                dict.Remove(key);
                dict.Add(key, value);
            }
        }

        private static bool reconstructCleanSampling(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                relevantCommands.Remove(Commands.COMMAND_EXPERIMENTALDESIGN);
                relevantCommands.Remove(Commands.COMMAND_EXPERIMENTALDESIGN + " validation");
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_OPTIONWISE);
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_BINARY_RANDOM);
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_PAIRWISE);
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE);
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_ALLBINARY);
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_OPTIONWISE + " validation");
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_BINARY_RANDOM + " validation");
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_PAIRWISE + " validation");
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_NEGATIVE_OPTIONWISE + " validation");
                relevantCommands.Remove(Commands.COMMAND_SAMPLE_ALLBINARY + " validation");
                return true;
            }
            else
            {
                return false;
            }
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
            string lineInLog = Commands.COMMAND + commandLine;
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
            string lineInLog = Commands.COMMAND + commandLine;
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
            string lineInLog = Commands.COMMAND + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                learningHistory = Tuple.Create(false, new List<string>());
                while (!logReader.EndOfStream)
                {
                    string line = logReader.ReadLine();
                    if (line.Contains(";"))
                    {
                        learningHistory.Item2.Add(line);
                    }
                    else if (line.StartsWith("Finished"))
                    {
                        addOrReplace(relevantCommands, command, commandLine);
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
            string lineInLog = Commands.COMMAND + commandLine;
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
            string lineInLog = Commands.COMMAND + commandLine;
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

        private static bool reconstructEvaluationSetCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine.TrimEnd();
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                if (logReader.ReadLine().Trim().Equals("Evaluation set loaded."))
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

        private static bool reconstructPrintMLSettings(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine.TrimEnd();
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                if (!logReader.EndOfStream)
                {
                    logReader.ReadLine();
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

        private static bool reconstructTrivialCommand(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine.Trim();
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                addOrReplace(relevantCommands, command, commandLine);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Tuple<bool, Dictionary<string, string>> reconstructRecursiveAScript(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                addOrReplace(relevantCommands, command, commandLine);
                return findRelevantCommandsLogFiles(task, relevantCommands, logReader);
            }
            else
            {
                return Tuple.Create(false, relevantCommands);
            }
        }

        private static bool reconstructCleanLearning(StreamReader logReader, Dictionary<string, string> relevantCommands, string task, string command, string commandLine)
        {
            string lineInLog = Commands.COMMAND + commandLine;
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
            string lineInLog = Commands.COMMAND + commandLine;
            if (lineInLog.Equals(logReader.ReadLine()))
            {
                learningHistory = Tuple.Create(true, new List<string>());
                while (!logReader.EndOfStream)
                {
                    string line = logReader.ReadLine();
                    if (line.Contains(";"))
                    {
                        learningHistory.Item2.Add(line);
                    }
                    else if (line.Contains("Finished"))
                    {
                        addOrReplace(relevantCommands, command, commandLine);
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
