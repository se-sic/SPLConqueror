using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MachineLearning.Learning;
using MachineLearning.Learning.Regression;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Heuristics;
using MachineLearning.Solver;
using SPLConqueror_Core;
using MachineLearning.Sampling;
using Persistence;
using MachineLearning;
using ProcessWrapper;

namespace CommandLine
{
    public class Commands
    {
        public const string COMMAND = "command: ";
        public const string COMMAND_TRUEMODEL = "truemodel";

        public const string COMMAND_LOG = "log";
        public const string COMMAND_MEASUREMENTS_TO_CSV = "measurementstocsv";

        public const string COMMAND_CLEAR_GLOBAL = "clean-global";
        public const string COMMAND_CLEAR_SAMPLING = "clean-sampling";
        public const string COMMAND_CLEAR_LEARNING = "clean-learning";

        public const string COMMAND_LOAD_CONFIGURATIONS = "all";
        public const string COMMAND_LOAD_MLSETTINGS = "load_mlsettings";

        public const string RESUME_FROM_DUMP = "resume-dump";

        //resume a A script with only log files. 
        public const string RESUME_FROM_LOG = "resume-log";

        //save current SPLConqueror state to a file.
        public const string COMMAND_SAVE = "save";

        // shouldnt be used by user.
        public const string COMMAND_ROLLBACK = "rollback";

        public const string COMMAND_VALIDATION = "validation";

        public const string COMMAND_EVALUATION_SET = "evaluationset";

        public const string COMMAND_SAMPLE_ALLBINARY = "allbinary";
        public const string COMMAND_SAMPLE_FEATUREWISE = "featurewise";
        public const string COMMAND_SAMPLE_OPTIONWISE = "optionwise";
        public const string COMMAND_SAMPLE_PAIRWISE = "pairwise";
        public const string COMMAND_SAMPLE_NEGATIVE_OPTIONWISE = "negfw";
        public const string COMMAND_SAMPLE_BINARY_RANDOM = "random";
        public const string COMMAND_SAMPLE_BINARY_TWISE = "twise";

        public const string COMMAND_START_ALLMEASUREMENTS = "learnwithallmeasurements";

        public const string COMMAND_PREDICT_ALL_CONFIGURATIONS = "predictall";
        public const string COMMAND_PREDICT_TRUEMODEL = "predicttruemodel";
        public const string COMMAND_ANALYZE_LEARNING = "analyze-learning";
        public const string COMMAND_PRINT_MLSETTINGS = "printsettings";
        public const string COMMAND_PREDICT_CONFIGURATIONS = "predict-configurations";

        // using this option, a partial or full option order can be defined. The order is used in printconfigs. To define an order, the names of the options have to be defined separated with whitespace. If an option is not defined in the order its name and the value is printed at the end of the configurtion. 
        public const string COMMAND_SAMPLING_OPTIONORDER = "optionorder";
        public const string COMMAND_PRINT_CONFIGURATIONS = "printconfigs";

        public const string COMMAND_VARIABILITYMODEL = "vm";
        public const string COMMAND_SET_NFP = "nfp";
        public const string COMMAND_SET_MLSETTING = "mlsettings";

        public const string COMMAND_START_LEARNING = "start";
        public const string COMMAND_OPTIMIZE_PARAMETER = "optimize-parameter";

        public const string COMMAND_EXERIMENTALDESIGN = "expdesign";
        public const string COMMAND_EXPDESIGN_BOXBEHNKEN = "boxbehnken";
        public const string COMMAND_EXPDESIGN_CENTRALCOMPOSITE = "centralcomposite";
        public const string COMMAND_EXPDESIGN_FULLFACTORIAL = "fullfactorial";
        public const string COMMAND_EXPDESIGN_FACTORIAL = "factorial";
        public const string COMMAND_EXPDESIGN_HYPERSAMPLING = "hypersampling";
        public const string COMMAND_EXPDESIGN_ONEFACTORATATIME = "onefactoratatime";
        public const string COMMAND_EXPDESIGN_KEXCHANGE = "kexchange";
        public const string COMMAND_EXPDESIGN_PLACKETTBURMAN = "plackettburman";
        public const string COMMAND_EXPDESIGN_RANDOM = "random";

        public const string COMMAND_SUBSCRIPT = "script";

        public const string DEFINE_PYTHON_PATH = "define-python-path";
        public const string COMMAND_PYTHON_LEARN = "learn-python";
        public const string COMMAND_PYTHON_LEARN_OPT = "learn-python-opt";

        List<SamplingStrategies> binaryToSample = new List<SamplingStrategies>();
        List<SamplingStrategies> binaryToSampleValidation = new List<SamplingStrategies>();
        List<ExperimentalDesign> numericToSample = new List<ExperimentalDesign>();
        List<ExperimentalDesign> numericToSampleValidation = new List<ExperimentalDesign>();
        ML_Settings mlSettings = new ML_Settings();
        InfluenceFunction trueModel = null;

        private CommandHistory currentHistory = new CommandHistory();
        private bool hasLearnData = false;

        public MachineLearning.Learning.Regression.Learning exp = new MachineLearning.Learning.Regression.Learning();

        public static string targetPath = "";

        public static string pyResult = "";

        /// <summary>
        /// Performs the functionality of one command. If no functionality is found for the command, the command is retuned by this method. 
        /// </summary>
        /// <param name="line">One command with its parameters.</param>
        /// <returns>Returns an empty string if the command could be performed by the method. If the command could not be performed by the method, the original command is returned.</returns>
        public string performOneCommand(string line)
        {
            string command;

            // remove comment part of the line (the comment starts with an #)
            line = line.Split(new Char[] { '#' }, 2)[0];
            if (line.Length == 0)
                return "";

            currentHistory.addCommand(line);

            // split line in command and parameters of the command
            string[] components = line.Split(new Char[] { ' ' }, 2);

            string task = "";
            if (components.Length > 1)
                task = components[1];

            string[] taskAsParameter = task.Split(new Char[] { ' ' });
            if (!GlobalState.rollback)
            {
                GlobalState.logInfo.logLine(COMMAND + line);

                command = components[0];
            }
            else
            {
                command = components[0];
                if (!command.Equals(COMMAND_SUBSCRIPT))
                {
                    command = COMMAND_ROLLBACK;
                }
            }

            switch (command.ToLower())
            {
                case COMMAND_START_ALLMEASUREMENTS:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);

                        List<Configuration> configurations_Learning = new List<Configuration>();

                        if (allMeasurementsValid())
                        {
                            configurations_Learning = GlobalState.allMeasurements.Configurations;

                        }
                        else
                        {
                            foreach (Configuration config in GlobalState.allMeasurements.Configurations)
                            {
                                if (config.nfpValues.ContainsKey(GlobalState.currentNFP))
                                    configurations_Learning.Add(config);
                            }
                        }

                        if (configurations_Learning.Count == 0)
                        {
                            GlobalState.logInfo.logLine("The learning set is empty! Cannot start learning!");
                            break;
                        }

                        GlobalState.logInfo.logLine("Learning: " + "NumberOfConfigurationsLearning:" + configurations_Learning.Count);
                        // prepare the machine learning 



                        //PythonWrapper pyInterpreter = new PythonWrapper(this.getLocationPythonScript() + Path.DirectorySeparatorChar + PythonWrapper.COMMUNICATION_SCRIPT, new string[] { });

                        //LearningSettings.LearningStrategies currentStrategy = LearningSettings.getStrategy(taskAsParameter[0]);
                        //// SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE, DecisionTreeRegressor
                        //pyInterpreter.setupApplication(configurations_Learning, currentStrategy, GlobalState.allMeasurements.Configurations, PythonWrapper.START_LEARN);

                        //pyResult = pyInterpreter.getLearningResult(GlobalState.allMeasurements.Configurations);
                        //Console.WriteLine("Py result:\n" + pyResult);
                        exp.models.Clear();
                        var mod = exp.models;
                        exp = new MachineLearning.Learning.Regression.Learning(configurations_Learning, configurations_Learning);
                        exp.models = mod;
                        exp.metaModel = infMod;
                        exp.mlSettings = this.mlSettings;
                        exp.learn();
                        GlobalState.logInfo.logLine("Finished");
                    }
                    break;

                case RESUME_FROM_DUMP:
                    {
                        Tuple<ML_Settings, List<SamplingStrategies>, List<SamplingStrategies>> recoveredData = CommandPersistence.recoverDataFromDump(taskAsParameter);
                        if (recoveredData == null)
                        {
                            GlobalState.logError.logLine("Couldnt recover.");
                        }
                        else
                        {
                            this.mlSettings = recoveredData.Item1;
                            this.binaryToSample = recoveredData.Item2;
                            this.binaryToSampleValidation = recoveredData.Item3;

                            FileInfo fi = new FileInfo(taskAsParameter[7]);
                            StreamReader reader = null;
                            if (!fi.Exists)
                                throw new FileNotFoundException(@"Automation script not found. ", fi.ToString());

                            reader = fi.OpenText();
                            Commands co = new Commands();
                            if (CommandPersistence.learningHistory != null && CommandPersistence.learningHistory.Item2.Count > 0 && CommandPersistence.learningHistory.Item1)
                            {
                                //restore exp
                                hasLearnData = true;
                            }
                            co.exp = this.exp;
                            co.binaryToSample = this.binaryToSample;
                            co.binaryToSampleValidation = this.binaryToSampleValidation;
                            co.mlSettings = this.mlSettings;
                            GlobalState.rollback = true;

                            while (!reader.EndOfStream)
                            {
                                String oneLine = reader.ReadLine().Trim();
                                co.performOneCommand(oneLine);

                            }
                        }
                        break;
                    }
                case COMMAND_SAVE:
                    {
                        CommandPersistence.dump(taskAsParameter, this.mlSettings, this.binaryToSample,
                            this.binaryToSampleValidation, this.exp, this.currentHistory);
                        break;
                    }
                case COMMAND_ROLLBACK:
                    if (currentHistory.Equals(CommandPersistence.history))
                    {
                        GlobalState.rollback = false;
                        GlobalState.logInfo.logLine("Performed rollback");
                    }
                    break;

                case RESUME_FROM_LOG:
                    Tuple<bool, Dictionary<string, string>> reachedEndAndRelevantCommands = CommandPersistence.findRelevantCommandsLogFiles(task.TrimEnd(), new Dictionary<string, string>());
                    if (reachedEndAndRelevantCommands.Item1)
                    {
                        GlobalState.logInfo.logLine("The end of the script was already reached");
                    }
                    else
                    {
                        string logBuffer = null;
                        foreach (KeyValuePair<string, string> kv in reachedEndAndRelevantCommands.Item2)
                        {
                            if (!kv.Key.Equals(COMMAND_SUBSCRIPT))
                            {
                                if (kv.Key.Equals(COMMAND_LOG))
                                {
                                    logBuffer = kv.Value.Split()[1].Trim();
                                }
                                else if (!(kv.Key.Equals(COMMAND_START_LEARNING) || kv.Key.Equals(COMMAND_START_ALLMEASUREMENTS)))
                                {
                                    performOneCommand(kv.Value);
                                }
                            }
                        }
                        GlobalState.logInfo = new InfoLogger(logBuffer, true);

                        if (CommandPersistence.learningHistory != null && CommandPersistence.learningHistory.Item2.Count > 0 && CommandPersistence.learningHistory.Item1)
                        {
                            //restore exp
                            hasLearnData = true;
                        }
                        FileInfo fi = new FileInfo(task.TrimEnd());
                        StreamReader reader = null;
                        if (!fi.Exists)
                            throw new FileNotFoundException(@"Automation script not found. ", fi.ToString());

                        reader = fi.OpenText();
                        Commands co = new Commands();
                        if (CommandPersistence.learningHistory != null && CommandPersistence.learningHistory.Item2.Count > 0 && CommandPersistence.learningHistory.Item1)
                        {
                            //restore exp
                            co.hasLearnData = true;
                        }
                        co.exp = this.exp;
                        co.binaryToSample = this.binaryToSample;
                        co.binaryToSampleValidation = this.binaryToSampleValidation;
                        co.mlSettings = this.mlSettings;
                        GlobalState.rollback = true;

                        while (!reader.EndOfStream)
                        {
                            String oneLine = reader.ReadLine().Trim();
                            co.performOneCommand(oneLine);

                        }
                    }
                    break;

                case COMMAND_TRUEMODEL:
                    StreamReader readModel = new StreamReader(task.TrimEnd());
                    String model = readModel.ReadLine().Trim();
                    readModel.Close();
                    this.trueModel = new InfluenceFunction(model.Replace(',', '.'), GlobalState.varModel);
                    NFProperty artificalProp = new NFProperty("artificial");
                    GlobalState.currentNFP = artificalProp;
                    //computeEvaluationDataSetBasedOnTrueModel();
                    break;

                case COMMAND_SUBSCRIPT:
                    {
                        FileInfo fi = new FileInfo(task.TrimEnd());
                        StreamReader reader = null;
                        if (!fi.Exists)
                            GlobalState.logError.logLine(@"Automation script not found. " + fi.ToString());
                        else
                        {
                            reader = fi.OpenText();
                            Commands co = new Commands();
                            co.exp = this.exp;

                            // Set the root directory to the location of the referenced file
                            String previousRootDirectory = Directory.GetCurrentDirectory();
                            String filePath = fi.DirectoryName;
                            Directory.SetCurrentDirectory(filePath);

                            reader = fi.OpenText();

                            co.currentHistory = this.currentHistory;
                            if (GlobalState.rollback)
                            {
                                co.binaryToSample = this.binaryToSample;
                                co.binaryToSampleValidation = this.binaryToSampleValidation;
                                co.mlSettings = this.mlSettings;
                            }

                            co.hasLearnData = this.hasLearnData;
                            co.exp = this.exp;

                            this.hasLearnData = co.hasLearnData;

                            while (!reader.EndOfStream)
                            {
                                String oneLine = reader.ReadLine().Trim();
                                co.performOneCommand(oneLine);

                            }

                            // Reset the root directory after the execution of the sub-script
                            Directory.SetCurrentDirectory(previousRootDirectory);
                        }
                        break;
                    }

                case COMMAND_EVALUATION_SET:
                    {
                        GlobalState.evaluationSet.Configurations = ConfigurationReader.readConfigurations(task, GlobalState.varModel);
                        GlobalState.logInfo.logLine("Evaluation set loaded.");
                    }
                    break;
                case COMMAND_CLEAR_GLOBAL:
                    SPLConqueror_Core.GlobalState.clear();
                    binaryToSample.Clear();
                    binaryToSampleValidation.Clear();
                    break;
                case COMMAND_CLEAR_SAMPLING:
                    exp.clearSampling();
                    binaryToSample.Clear();
                    binaryToSampleValidation.Clear();
                    numericToSample.Clear();
                    numericToSampleValidation.Clear();
                    break;
                case COMMAND_CLEAR_LEARNING:
                    exp.clear();
                    binaryToSample.Clear();
                    binaryToSampleValidation.Clear();
                    break;
                case COMMAND_LOAD_CONFIGURATIONS:
                    GlobalState.allMeasurements.setBlackList(mlSettings.blacklisted);
                    GlobalState.allMeasurements.Configurations = (GlobalState.allMeasurements.Configurations.Union(ConfigurationReader.readConfigurations(task.TrimEnd(), GlobalState.varModel))).ToList();
                    GlobalState.measurementSource = task.TrimEnd();
                    string attachement = "";
                    if (GlobalState.measurementDeviation > 0 && this.mlSettings != null && mlSettings.abortError == 1)
                    {
                        this.mlSettings.abortError = GlobalState.measurementDeviation;
                        attachement = " abortError set to highest deviation value: " + GlobalState.measurementDeviation + ".";
                    }
                    GlobalState.logInfo.logLine(GlobalState.allMeasurements.Configurations.Count + " configurations loaded." + attachement);
                    break;


                case COMMAND_MEASUREMENTS_TO_CSV:
                    FileStream ostrm;
                    ostrm = new FileStream(task.Trim(), FileMode.OpenOrCreate, FileAccess.Write);
                    ostrm.SetLength(0);
                    StreamWriter writer = new StreamWriter(ostrm);
                    StringBuilder header = new StringBuilder();
                    List<NFProperty> propertiesOrder = new List<NFProperty>();
                    for (int i = 0; i < GlobalState.varModel.optionToIndex.Count; i++)
                    {
                        header.Append(GlobalState.varModel.optionToIndex[i].Name + ";");
                    }
                    foreach (NFProperty prop in GlobalState.nfProperties.Values)
                    {
                        header.Append(prop.Name + ";");
                        propertiesOrder.Add(prop);
                    }
                    header.Append("\n");
                    StringBuilder configurations = new StringBuilder();
                    foreach (Configuration config in GlobalState.allMeasurements.Configurations)
                    {
                        for (int i = 0; i < GlobalState.varModel.optionToIndex.Count; i++)
                        {
                            ConfigurationOption opt = GlobalState.varModel.optionToIndex[i];
                            if (opt.GetType() == typeof(BinaryOption))
                            {
                                if (config.BinaryOptions.ContainsKey((BinaryOption)opt) && config.BinaryOptions[(BinaryOption)opt] == BinaryOption.BinaryValue.Selected)
                                    configurations.Append("1;");
                                else
                                    configurations.Append("0;");
                            }
                            else
                            {
                                configurations.Append(config.NumericOptions[(NumericOption)opt] + ";");
                            }
                        }
                        for (int i = 0; i < propertiesOrder.Count; i++)
                        {
                            if (!config.nfpValues.ContainsKey(propertiesOrder[i]))
                                configurations.Append("0;");
                            else
                                configurations.Append(config.nfpValues[propertiesOrder[i]] + ";");
                        }
                        configurations.Append("\n");
                    }

                    writer.Write(header);
                    writer.Write(configurations);
                    writer.Flush();
                    writer.Close();
                    ostrm.Close();
                    break;

                case COMMAND_SAMPLE_ALLBINARY:
                    {
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.binaryToSampleValidation.Add(SamplingStrategies.ALLBINARY);
                            this.exp.info.binarySamplings_Validation = "ALLBINARY";
                        }
                        else
                        {
                            this.binaryToSample.Add(SamplingStrategies.ALLBINARY);
                            this.exp.info.binarySamplings_Learning = "ALLBINARY";
                        }

                        break;
                    }

                case COMMAND_PREDICT_ALL_CONFIGURATIONS:
                    {
                        if (this.exp.models.Count == 0)
                        {
                            GlobalState.logInfo.logLine("Can't predict configurations. No learning was performed");
                        }
                        else if (this.exp.models.ElementAt(this.exp.models.Count - 1).LearningHistory.Count == 0)
                        {
                            GlobalState.logInfo.logLine("Can't predict configurations. No model was learned.");
                        }
                        else if (task.Length == 0)
                        {
                            GlobalState.logInfo.logLine("Target file is required to print prediction results");
                        }
                        else if (GlobalState.allMeasurements.Configurations.Count == 0)
                        {
                            GlobalState.logError.logLine("No measurements loaded.");
                        }
                        else
                        {
                            predict(task);
                        }
                        break;
                    }

                case COMMAND_PREDICT_TRUEMODEL:
                    {
                        if (this.trueModel == null)
                        {
                            GlobalState.logInfo.logLine("No trueModel is loaded.");
                        }
                        else if (task.Length == 0)
                        {
                            GlobalState.logInfo.logLine("Target file is required to print prediction results");
                        }
                        else if (GlobalState.allMeasurements.Configurations.Count == 0)
                        {
                            GlobalState.logError.logLine("No measurements loaded.");
                        }
                        else
                        {
                            predict(task, useTrueModel: true);
                        }
                        break;
                    }

                case COMMAND_PREDICT_CONFIGURATIONS:
                    {
                        FeatureSubsetSelection learnedModel = exp.models[exp.models.Count - 1];
                        String samplingIdentifier = createSamplingIdentifier();

                        PythonPredictionWriter csvWriter = new PythonPredictionWriter(targetPath, new String[] { "SPLConqueror" }, GlobalState.varModel.Name + "_" + samplingIdentifier);
                        List<Feature> features = learnedModel.LearningHistory[learnedModel.LearningHistory.Count - 1].FeatureSet;
                        csvWriter.writePredictions("Configuration;MeasuredValue;PredictedValue\n");
                        for (int i = 0; i < GlobalState.allMeasurements.Configurations.Count; i++)
                        {

                            Double predictedValue = FeatureSubsetSelection.estimate(features, GlobalState.allMeasurements.Configurations[i]);
                            csvWriter.writePredictions(GlobalState.allMeasurements.Configurations[i].ToString().Replace(";", "_") + ";" + Math.Round(GlobalState.allMeasurements.Configurations[i].GetNFPValue(), 4) + ";" + Math.Round(predictedValue, 4) + "\n");
                        }

                        break;
                    }

                case COMMAND_ANALYZE_LEARNING:
                    {//TODO: Analyzation is not supported in the case of bagging
                        GlobalState.logInfo.logLine("Round, Model, LearningError, LearningErrorRel, ValidationError, ValidationErrorRel, ElapsedSeconds, ModelComplexity, BestCandidate, BestCandidateSize, BestCandidateScore, TestError");
                        GlobalState.logInfo.logLine("Models:");
                        //GlobalState.logInfo.logLine(pyResult);
                        if (this.mlSettings.bagging)
                        {
                            // this.metaModel

                            for (int i = 0; i < this.exp.models.Count; i++)
                            {
                                FeatureSubsetSelection learnedModel = exp.models[i];
                                if (learnedModel == null)
                                {
                                    GlobalState.logError.logLine("Error... learning was not performed!");
                                    break;
                                }
                                GlobalState.logInfo.logLine("Termination reason: " + learnedModel.LearningHistory.Last().terminationReason);
                                foreach (LearningRound lr in learnedModel.LearningHistory)
                                {
                                    double relativeError = 0;
                                    if (GlobalState.evaluationSet.Configurations.Count > 0)
                                    {
                                        double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.evaluationSet.Configurations, out relativeError, false);
                                    }
                                    else
                                    {
                                        double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError, false);
                                    }

                                    GlobalState.logInfo.logLine(lr.ToString() + relativeError);
                                }
                            }
                        }
                        else
                        {
                            if (exp.models.Count == 0 || exp.models[0] == null)
                            {
                                GlobalState.logError.logLine("Error... learning was not performed!");
                                break;
                            }
                            
                            FeatureSubsetSelection learnedModel = exp.models[0];

                            GlobalState.logInfo.logLine("Termination reason: " + learnedModel.LearningHistory.Last().terminationReason);
                            foreach (LearningRound lr in learnedModel.LearningHistory)
                            {
                                double relativeError = 0;
                                if (GlobalState.evaluationSet.Configurations.Count > 0)
                                {
                                    double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.evaluationSet.Configurations, out relativeError, false);
                                }
                                else
                                {
                                    double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError, false);
                                }

                                GlobalState.logInfo.logLine(lr.ToString() + relativeError);
                            }
                        }
                        GlobalState.logInfo.logLine("Analyze finished");


                        break;
                    }
                case COMMAND_EXERIMENTALDESIGN:
                    performOneCommand_ExpDesign(task);
                    break;

                case COMMAND_SAMPLING_OPTIONORDER:
                    parseOptionOrder(task);
                    break;

                case COMMAND_VARIABILITYMODEL:
                    String debug = Directory.GetCurrentDirectory();
                    GlobalState.vmSource = task.TrimEnd();
                    GlobalState.varModel = VariabilityModel.loadFromXML(task.Trim());
                    if (GlobalState.varModel == null)
                    {
                        GlobalState.logError.logLine("No variability model found at " + task);
                    }
                    else if (mlSettings.blacklisted.Count > 0)
                    {
                        mlSettings.checkAndCleanBlacklisted();
                    }
                    if (targetPath.Length == 0)
                        targetPath = task.Substring(0, Math.Max(task.LastIndexOf("\\"), task.LastIndexOf("/"))) + Path.DirectorySeparatorChar;
                    break;
                case COMMAND_SET_NFP:
                    GlobalState.currentNFP = GlobalState.getOrCreateProperty(task.Trim());
                    break;
                case COMMAND_SAMPLE_FEATUREWISE:
                case COMMAND_SAMPLE_OPTIONWISE:
                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        this.binaryToSampleValidation.Add(SamplingStrategies.OPTIONWISE);
                        this.exp.info.binarySamplings_Validation = "OPTIONSWISE";
                    }
                    else
                    {
                        this.binaryToSample.Add(SamplingStrategies.OPTIONWISE);
                        this.exp.info.binarySamplings_Learning = "OPTIONSWISE";
                    }

                    break;

                case COMMAND_SAMPLE_BINARY_TWISE:
                    {
                        string[] para = task.Split(new char[] { ' ' });

                        // TODO something is wrong here....
                        Dictionary<String, String> prameters = new Dictionary<string, string>();
                        //parseParametersToLinearAndQuadraticBinarySampling(para);

 						for(int i = 0; i < para.Length; i++)
                        {
                            prameters.Add(para[i].Split(':')[0], para[i].Split(':')[1]);
                        }                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.binaryToSampleValidation.Add(SamplingStrategies.T_WISE);
                            this.exp.info.binarySamplings_Validation = "T_WISE ";
                        }
                        else
                        {
                            this.binaryToSample.Add(SamplingStrategies.T_WISE);
                            this.exp.info.binarySamplings_Learning = "T_WISE ";
                        }
                        ConfigurationBuilder.binaryParams.tWiseParameters.Add(prameters);
                    }
                    break;  

                case COMMAND_LOG:

                    string location = task.Trim();
                    targetPath = location;
                    GlobalState.logInfo.close();
                    GlobalState.logInfo = new InfoLogger(location);

                    GlobalState.logError.close();
                    GlobalState.logError = new ErrorLogger(location + "_error");
                    break;
                case COMMAND_SET_MLSETTING:
                    this.mlSettings = ML_Settings.readSettings(task);
                    break;
                case COMMAND_LOAD_MLSETTINGS:
                    this.mlSettings = ML_Settings.readSettingsFromFile(task);
                    break;

                case COMMAND_SAMPLE_PAIRWISE:

                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        this.binaryToSampleValidation.Add(SamplingStrategies.PAIRWISE);
                        this.exp.info.binarySamplings_Validation = "PAIRWISE";
                    }
                    else
                    {
                        this.binaryToSample.Add(SamplingStrategies.PAIRWISE);
                        this.exp.info.binarySamplings_Learning = "PAIRWISE";
                    }
                    break;

                case COMMAND_PRINT_MLSETTINGS:
                    GlobalState.logInfo.logLine(this.mlSettings.ToString());
                    break;

                case COMMAND_PRINT_CONFIGURATIONS:
                    {
                        /* List<Dictionary<NumericOption, double>> numericSampling = exp.NumericSelection_Learning;
                         List<List<BinaryOption>> binarySampling = exp.BinarySelections_Learning;

                         List<Configuration> configurations = new List<Configuration>();

                         foreach (Dictionary<NumericOption, double> numeric in numericSampling)
                         {
                             foreach (List<BinaryOption> binary in binarySampling)
                             {
                                 Configuration config = Configuration.getConfiguration(binary, numeric);
                                 if (!configurations.Contains(config) && GlobalState.varModel.configurationIsValid(config))
                                 {
                                     configurations.Add(config);
                                 }
                             }
                         }*/

                        string[] para = task.Split(new char[] { ' ' });
                        // TODO very error prone..
                        if (para.Length >= 1 && (para[0].Trim()).Length > 0)
                        {
                            ConfigurationPrinter printer = null;

                            ConfigurationBuilder.setBlacklisted(this.mlSettings.blacklisted);
                            var configs = ConfigurationBuilder.buildConfigs(GlobalState.varModel, this.binaryToSample, this.numericToSample);
                            if (para.Length >= 3)
                            {
                                printer = new ConfigurationPrinter(para[0], GlobalState.optionOrder, para[1], para[2]);
                            }
                            else if (para.Length == 2)
                            {
                                printer = new ConfigurationPrinter(para[0], GlobalState.optionOrder, para[1]);
                            }
                            else
                            {
                                printer = new ConfigurationPrinter(para[0], GlobalState.optionOrder);
                            }
                            printer.print(configs);
                        }
                        else
                        {
                            GlobalState.logInfo.logLine("Couldnt print configs");
                            GlobalState.logError.logLine("Error cant print configs without at least a outputfile");
                        }

                        break;
                    }
                case COMMAND_SAMPLE_BINARY_RANDOM:
                    {
                        Dictionary<String, String> parameter = new Dictionary<String, String>();
                        string[] para = task.Split(new char[] { ' ' });
                        for(int i = 0; i < para.Length; i++)
                        {
                            String key = para[i].Split(':')[0];
                            String value = para[i].Split(':')[1];
                            parameter.Add(key, value);
                        }
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.binaryToSampleValidation.Add(SamplingStrategies.BINARY_RANDOM);
                            this.exp.info.binarySamplings_Validation = "BINARY_RANDOM";
                        }
                        else
                        {
                            this.binaryToSample.Add(SamplingStrategies.BINARY_RANDOM);
                            this.exp.info.binarySamplings_Learning = "BINARY_RANDOM " + task;
                        }
                        ConfigurationBuilder.binaryParams.randomBinaryParameters.Add(parameter);

                        break;
                    }

                case DEFINE_PYTHON_PATH:
                    {
                        // Append a slash if it is not included
                        if (!taskAsParameter [0].EndsWith ("/") && !taskAsParameter [0].EndsWith ("\\")) {
                            PythonWrapper.PYTHON_PATH = taskAsParameter [0] + "/";
                        } else {
                            PythonWrapper.PYTHON_PATH = taskAsParameter [0];
                        }
                            // Here, a differentiation of the operating system is required
                        if (Environment.OSVersion.Platform == PlatformID.Win32Windows) {
                            PythonWrapper.PYTHON_PATH += "python.exe";
                        } else {
                            PythonWrapper.PYTHON_PATH += "python";
                        }
                        break;
                    }


                case COMMAND_PYTHON_LEARN:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        Tuple<List<Configuration>, List<Configuration>> learnAndValidation = buildSetsEfficient();
                        List<Configuration> configurationsLearning = learnAndValidation.Item1;
                        List<Configuration> configurationsValidation = learnAndValidation.Item2;

                        String samplingIdentifier = createSamplingIdentifier();

                        if (configurationsLearning.Count == 0)
                        {
                            configurationsLearning = configurationsValidation;
                        }

                        if (configurationsLearning.Count == 0)
                        {
                            GlobalState.logInfo.logLine("The learning set is empty! Cannot start learning!");
                            break;
                        }

                        if (configurationsValidation.Count == 0)
                        {
                            configurationsValidation = configurationsLearning;
                        }
                        GlobalState.logInfo.logLine("Learning: " + "NumberOfConfigurationsLearning:" + configurationsLearning.Count + " NumberOfConfigurationsValidation:" + configurationsValidation.Count);

                        // SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE, DecisionTreeRegressor
                        if (ProcessWrapper.LearningSettings.isLearningStrategy(taskAsParameter[0]))
                        {
                            PythonWrapper pyInterpreter = new PythonWrapper(this.getLocationPythonScript() + Path.DirectorySeparatorChar + PythonWrapper.COMMUNICATION_SCRIPT, taskAsParameter);
                            GlobalState.logInfo.logLine("Starting Prediction");
                            pyInterpreter.setupApplication(configurationsLearning, GlobalState.allMeasurements.Configurations, PythonWrapper.START_LEARN);
                            configurationsLearning = null;
                            PythonPredictionWriter csvWriter = new PythonPredictionWriter(targetPath, taskAsParameter, GlobalState.varModel.Name + "_" + samplingIdentifier);
                            pyInterpreter.getLearningResult(GlobalState.allMeasurements.Configurations, csvWriter);
                            GlobalState.logInfo.logLine("Prediction finished, results written in " + csvWriter.getPath());
                            csvWriter.close();
                        }
                        else
                        {
                            GlobalState.logInfo.logLine("Invalid Learning strategy " + taskAsParameter[0] + "! Aborting learning");
                        }
                        break;
                    }


                case COMMAND_PYTHON_LEARN_OPT:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        Tuple<List<Configuration>, List<Configuration>> learnAndValidation = buildSetsEfficient();
                        List<Configuration> configurationsLearning = learnAndValidation.Item1;
                        List<Configuration> configurationsValidation = learnAndValidation.Item2;

                        if (configurationsLearning.Count == 0)
                        {
                            configurationsLearning = configurationsValidation;
                        }

                        if (configurationsLearning.Count == 0)
                        {
                            GlobalState.logInfo.logLine("The learning set is empty! Cannot start learning!");
                            break;
                        }

                        if (configurationsValidation.Count == 0)
                        {
                            configurationsValidation = configurationsLearning;
                        }
                        GlobalState.logInfo.logLine("Learning: " + "NumberOfConfigurationsLearning:" + configurationsLearning.Count + " NumberOfConfigurationsValidation:" + configurationsValidation.Count);

                        // SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE, DecisionTreeRegressor
                        if (ProcessWrapper.LearningSettings.isLearningStrategy(taskAsParameter[0]))
                        {
                            PythonWrapper pyInterpreter = new PythonWrapper(this.getLocationPythonScript() + Path.DirectorySeparatorChar + PythonWrapper.COMMUNICATION_SCRIPT, taskAsParameter);
                            pyInterpreter.setupApplication(configurationsLearning, GlobalState.allMeasurements.Configurations, PythonWrapper.START_PARAM_TUNING);
                            string path = targetPath.Substring(0, (targetPath.Length - (((targetPath.Split(Path.DirectorySeparatorChar)).Last()).Length)));
                            pyResult = pyInterpreter.getOptimizationResult(GlobalState.allMeasurements.Configurations, path);
                            GlobalState.logInfo.logLine("Optimal parameters " + pyResult.Replace(",", ""));
                        }
                        else
                        {
                            GlobalState.logInfo.logLine("Invalid learning strategy " + taskAsParameter[0] + "! Aborting Learning");
                        }
                        break;
                    }


                case COMMAND_START_LEARNING:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        Tuple<List<Configuration>, List<Configuration>> learnAndValidation = buildSetsEfficient();
                        List<Configuration> configurationsLearning = learnAndValidation.Item1;
                        List<Configuration> configurationsValidation = learnAndValidation.Item2;

                        if (configurationsLearning.Count == 0)
                        {
                            configurationsLearning = configurationsValidation;
                        }

                        if (configurationsLearning.Count == 0)
                        {
                            GlobalState.logInfo.logLine("The learning set is empty! Cannot start learning!");
                            break;
                        }

                        if (configurationsValidation.Count == 0)
                        {
                            configurationsValidation = configurationsLearning;
                        }


                        GlobalState.logInfo.logLine("Learning: " + "NumberOfConfigurationsLearning:" + configurationsLearning.Count + " NumberOfConfigurationsValidation:" + configurationsValidation.Count);

                        // We have to reuse the list of models because of a NotifyCollectionChangedEventHandlers that might be attached to the list of models. 
                        if (!hasLearnData)
                        {
                            exp.models.Clear();
                            var mod = exp.models;
                            exp = new MachineLearning.Learning.Regression.Learning(configurationsLearning, configurationsValidation);
                            exp.models = mod;

                            exp.metaModel = infMod;
                            exp.mlSettings = this.mlSettings;
                            exp.learn();
                        }
                        else
                        {
                            GlobalState.logInfo.logLine("Continue learning");
                            exp.models.Clear();
                            var mod = exp.models;
                            exp = new MachineLearning.Learning.Regression.Learning(configurationsLearning, configurationsValidation);
                            exp.models = mod;
                            exp.metaModel = infMod;
                            exp.mlSettings = this.mlSettings;
                            List<LearningRound> lr = new List<LearningRound>();
                            foreach (string lrAsString in CommandPersistence.learningHistory.Item2)
                            {
                                lr.Add(LearningRound.FromString(lrAsString, GlobalState.varModel));
                            }
                            exp.continueLearning(lr);
                        }
                        GlobalState.logInfo.logLine("average model: \n" + exp.metaModel.printModelAsFunction());
                        double relativeerror = 0;
                        if (GlobalState.evaluationSet.Configurations.Count > 0)
                        {
                            relativeerror = FeatureSubsetSelection.computeError(exp.metaModel, GlobalState.evaluationSet.Configurations, ML_Settings.LossFunction.RELATIVE, exp.mlSettings, false);
                        }
                        else
                        {
                            relativeerror = FeatureSubsetSelection.computeError(exp.metaModel, GlobalState.allMeasurements.Configurations, ML_Settings.LossFunction.RELATIVE, exp.mlSettings, false);
                        }

                        //    globalstate.loginfo.logline("error :" + relativeerror);

                        break;
                        GlobalState.logInfo.logLine("Finished");
                    }
                case COMMAND_OPTIMIZE_PARAMETER:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        Tuple<List<Configuration>, List<Configuration>> learnAndValidation = buildSetsEfficient();
                        List<Configuration> configurationsLearning = learnAndValidation.Item1;
                        List<Configuration> configurationsValidation = learnAndValidation.Item2;

                        if (configurationsLearning.Count == 0)
                        {
                            configurationsLearning = configurationsValidation;
                        }

                        if (configurationsLearning.Count == 0)
                        {
                            GlobalState.logInfo.logLine("The learning set is empty! Cannot start learning!");
                            break;
                        }

                        if (configurationsValidation.Count == 0)
                        {
                            configurationsValidation = configurationsLearning;
                        }


                        GlobalState.logInfo.logLine("Learning: " + "NumberOfConfigurationsLearning:" + configurationsLearning.Count + " NumberOfConfigurationsValidation:" + configurationsValidation.Count);

                        List<ML_Settings> parameterSettings = new List<ML_Settings>();
                        parameterSettings = ML_SettingsGenerator.generateSettings(taskAsParameter);

                        ML_Settings optimalParameters = null;
                        double minimalError = Double.MaxValue;

                        foreach (ML_Settings parameters in parameterSettings)
                        {
                            // We have to reuse the list of models because of a NotifyCollectionChangedEventHandlers that might be attached to the list of models. 
                            KFoldCrossValidation kFold = new KFoldCrossValidation(parameters, configurationsLearning);
                            double error = kFold.learn();

                            if (error < minimalError)
                            {
                                optimalParameters = parameters;
                                minimalError = error;
                            }

                        }
                        GlobalState.logInfo.logLine("Error of optimal parameters: " + minimalError);
                        GlobalState.logInfo.logLine("Parameters: " + optimalParameters.ToString());

                        break;
                    }



                case COMMAND_SAMPLE_NEGATIVE_OPTIONWISE:
                    // TODO there are two different variants in generating NegFW configurations. 

                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        this.binaryToSampleValidation.Add(SamplingStrategies.NEGATIVE_OPTIONWISE);
                        this.exp.info.binarySamplings_Validation = "NEGATIVE_OPTIONWISE";
                    }
                    else
                    {
                        this.binaryToSample.Add(SamplingStrategies.NEGATIVE_OPTIONWISE);
                        this.exp.info.binarySamplings_Learning = "NEGATIVE_OPTIONWISE";
                    }
                    break;
                default:
                    return command;
            }
            return "";
        }

        private string createSamplingIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            // add binay sampling strategy to the identifier
            sb.Append(this.exp.info.binarySamplings_Learning + "_");

            // add numeric sampling strategy to the identifier
            foreach (ExperimentalDesign sampling in numericToSample)
            {
                sb.Append("_" + sampling.getName() + "--" + sampling.parameterIdentifier());
            }
            return sb.ToString();
        }

        private bool isAllMeasurementsToSample()
        {
            return this.binaryToSample.Contains(SamplingStrategies.ALLBINARY) && this.binaryToSample.Contains(SamplingStrategies.FULLFACTORIAL);
        }

        private bool isAllMeasurementsValidation()
        {
            return this.binaryToSampleValidation.Contains(SamplingStrategies.ALLBINARY) && this.binaryToSample.Contains(SamplingStrategies.FULLFACTORIAL);
        }

        private bool allMeasurementsValid()
        {
            foreach (Configuration conf in GlobalState.allMeasurements.Configurations)
            {
                if (!conf.nfpValues.ContainsKey(GlobalState.currentNFP))
                    return false;
            }
            return true;
        }

        private List<Configuration> buildSet(List<SamplingStrategies> binaryStrats, List<ExperimentalDesign> numericStrats)
        {
            ConfigurationBuilder.setBlacklisted(mlSettings.blacklisted);
            List<Configuration> configurationsTest = ConfigurationBuilder.buildConfigs(GlobalState.varModel, binaryStrats, numericStrats);
            //Construct configurations and compute the synthetic value if we have a given function that simulates the options' influences
            if (trueModel != null)
            {
                foreach (Configuration conf in configurationsTest)
                {
                    conf.setMeasuredValue(GlobalState.currentNFP, trueModel.eval(conf));
                    GlobalState.addConfiguration(conf);
                }
            }
            else
            {
                configurationsTest = GlobalState.getMeasuredConfigs(configurationsTest);
            }

            return configurationsTest;
        }

        private Tuple<List<Configuration>, List<Configuration>> buildSetsEfficient()
        {
            bool measurementsValid = false;
            List<Configuration> configurationsLearning = new List<Configuration>();
            List<Configuration> configurationsValidation = new List<Configuration>();

            if (isAllMeasurementsToSample() && allMeasurementsValid() && (mlSettings.blacklisted == null || mlSettings.blacklisted.Count == 0))
            {
                measurementsValid = true;
                configurationsLearning = GlobalState.allMeasurements.Configurations;
            }
            else
            {
                configurationsLearning = buildSet(this.binaryToSample, this.numericToSample);
            }

            if (isAllMeasurementsValidation() && (measurementsValid || allMeasurementsValid()) && (mlSettings.blacklisted == null || mlSettings.blacklisted.Count == 0))
            {
                configurationsValidation = GlobalState.allMeasurements.Configurations;
            }
            else
            {
                configurationsValidation = buildSet(this.binaryToSampleValidation, this.numericToSampleValidation);
            }
            return Tuple.Create(configurationsLearning, configurationsValidation);
        }

        private void parseOptionOrder(string task)
        {
            String[] optionNames = task.Split(' ');
            foreach (String option in optionNames)
            {
                if (option.Trim().Length == 0)
                    continue;
                GlobalState.optionOrder.Add(GlobalState.varModel.getOption(option.Trim()));
            }

        }

        /// <summary>
        /// The methods generates based on all binary sampling and 50% hyper sampling configurations for the validation set.
        /// If we have the true model, we can just compute the true value of the nonfunctional property for a given configuration. 
        /// </summary>
        private void computeEvaluationDataSetBasedOnTrueModel()
        {
            /*   VariantGenerator vg = new VariantGenerator(null);
               List<List<BinaryOption>> temp = vg.generateAllVariantsFast(GlobalState.varModel);
               List<List<BinaryOption>> binaryConfigs = new List<List<BinaryOption>>();
               //take only 10k
               if (temp.Count > 1000)
               {
                   GlobalState.logInfo.log("Found " + temp.Count + " configurations. Use only 1000.");
                   HashSet<int> picked = new HashSet<int>();
                   Random r = new Random(1);
                   for (int i = 0; i < 1000; i++)
                   {
                       int x = 0;
                       do
                       {
                           x = r.Next(1, temp.Count);
                       } while (picked.Contains(x));
                       picked.Add(x);
                       binaryConfigs.Add(temp[x]);
                   }
                   temp.Clear();
               }
               else
                   binaryConfigs = temp;
               exp.addBinarySelection_Validation(binaryConfigs);
               var expDesign = new HyperSampling(GlobalState.varModel.NumericOptions);
               expDesign.Precision = 10;
               expDesign.computeDesign();
               exp.addNumericalSelection_Validation(expDesign.SelectedConfigurations);
               var numericConfigs = expDesign.SelectedConfigurations;
               foreach (List<BinaryOption> binConfig in binaryConfigs)
               {
                   if (numericConfigs.Count == 0)
                   {
                       Configuration c = new Configuration(binConfig);
                       c.setMeasuredValue(GlobalState.currentNFP, this.trueModel.eval(c));
                       GlobalState.addConfiguration(c);
                   }
                   foreach (Dictionary<NumericOption, double> numConf in numericConfigs)
                   {

                       Configuration c = new Configuration(binConfig, numConf);
                       c.setMeasuredValue(GlobalState.currentNFP, this.trueModel.eval(c));
                       GlobalState.addConfiguration(c);
                   }
               }*/
        }


        private string performOneCommand_MlSetting(string command)
        {
            // splits the task in design and parameters of the design
            string[] commandAndParameter = command.Split(new Char[] { ' ' }, 2);
            string task = commandAndParameter[0];
            string param = "";
            if (commandAndParameter.Length > 1)
                param = commandAndParameter[1];
            string[] parameters = param.Split(' ');

            // parsing of the parameters
            List<NumericOption> optionsToConsider = new List<NumericOption>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();


            foreach (string par in parameters)
            {
                string[] nameAndValue = par.Split(':');
                if (nameAndValue.Length > 1)
                    parameter.Add(nameAndValue[0], nameAndValue[1]);
                else
                    parameter.Add(nameAndValue[0], "");
            }




            return "";

        }


        /// <summary>
        /// 
        /// Note: An experimental design might have parameters and also consider only a specific set of numeric options. 
        ///         [option1,option3,...,optionN] param1:value param2:value
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private string performOneCommand_ExpDesign(string task)
        {
            // splits the task in design and parameters of the design
            string[] designAndParams = task.Split(new Char[] { ' ' }, 2);
            string designName = designAndParams[0];
            string param = "";
            if (designAndParams.Length > 1)
                param = designAndParams[1];
            string[] parameters = param.Split(' ');



            // parsing of the parameters
            List<NumericOption> optionsToConsider = new List<NumericOption>();
            Dictionary<string, string> parameter = new Dictionary<string, string>();

            if (param.Length > 0)
            {
                foreach (string par in parameters)
                {
                    if (par.Contains("["))
                    {
                        string[] options = par.Substring(1, par.Length - 2).Split(',');
                        foreach (string option in options)
                        {
                            optionsToConsider.Add(GlobalState.varModel.getNumericOption(option));
                        }
                    }
                    else
                    {
                        if (par.Contains(':'))
                        {
                            string[] nameAndValue = par.Split(':');
                            parameter.Add(nameAndValue[0], nameAndValue[1]);
                        }
                        else
                        {
                            parameter.Add(par, "");
                        }

                    }
                }

            }
            if (optionsToConsider.Count == 0)
                optionsToConsider = GlobalState.varModel.NumericOptions;

            ExperimentalDesign expDesign = null;

            switch (designName.ToLower())
            {
                case COMMAND_EXPDESIGN_BOXBEHNKEN:
                    expDesign = new BoxBehnkenDesign ();
                    break;
                case COMMAND_EXPDESIGN_CENTRALCOMPOSITE:
                    expDesign = new CentralCompositeInscribedDesign ();
                    break;
                case COMMAND_EXPDESIGN_FULLFACTORIAL:
                    expDesign = new FullFactorialDesign ();
                    break;

                case COMMAND_EXPDESIGN_FACTORIAL:
                    expDesign = new FactorialDesign ();
                    break;

                case COMMAND_EXPDESIGN_HYPERSAMPLING:
                    expDesign = new HyperSampling();
                    break;

                case COMMAND_EXPDESIGN_ONEFACTORATATIME:
                    expDesign = new OneFactorAtATime();
                    break;

                case COMMAND_EXPDESIGN_KEXCHANGE:
                    expDesign = new KExchangeAlgorithm();
                    break;

                case COMMAND_EXPDESIGN_PLACKETTBURMAN:
                    expDesign = new PlackettBurmanDesign();
                    break;

                case COMMAND_EXPDESIGN_RANDOM:
                    expDesign = new RandomSampling();
                    break;

                default:
                    return task;
            }

            expDesign.setSamplingParameters (parameter);
            if (parameter.ContainsKey("validation"))
            {
                this.numericToSampleValidation.Add(expDesign);
                this.exp.info.numericSamplings_Validation = expDesign.getName ();
            }
            else
            {
                this.numericToSample.Add(expDesign);
                this.exp.info.numericSamplings_Learning = expDesign.getName();
            }

            return "";
        }

        private void predict(string task, bool useTrueModel = false)
        {
            StreamWriter sw = new StreamWriter(task);
            sw.Write("configuration;real value;prediction;deviation;percentage;" + Environment.NewLine);
            for (int i = 0; i < GlobalState.allMeasurements.Configurations.Count; ++i)
            {
                Configuration currentConfiguration = GlobalState.allMeasurements.Configurations.ElementAt(i);
                double realValue = GlobalState.allMeasurements.Configurations.ElementAt(i).GetNFPValue();
                double prediction;

                if (useTrueModel)
                {
                    prediction = trueModel.eval(currentConfiguration);
                }
                else
                {
                    prediction = FeatureSubsetSelection
                    .predict(this.exp.models.ElementAt(this.exp.models.Count - 1).LearningHistory.Last().FeatureSet, currentConfiguration);
                }
                double difference = Math.Abs(realValue - prediction);
                double percentage = 0;
                if (difference != 0)
                {
                    percentage = difference / realValue;
                }
                sw.Write(currentConfiguration.ToString().Replace(';', ',') + ";" + realValue + ";" + prediction + ";" + difference + ";" + percentage + ";" + Environment.NewLine);
            }
            sw.Flush();
            sw.Close();
        }


        public String getLocationPythonScript()
        {

            String location = AppDomain.CurrentDomain.BaseDirectory;
#if release
            if (pathToDll != null && pathToDll.Length > 0)
                    location = pathToDll;
                else
                    location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release").Length)));

#else
            location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug").Length)));
#endif

            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//Removing tailing dir sep
            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//Removing project path

#if release
            catalog.Catalogs.Add(new DirectoryCatalog(location));
            location = location + Path.DirectorySeparatorChar + "PyML" + Path.DirectorySeparatorChar + "pyScripts";
#else
            location = location + Path.DirectorySeparatorChar + "PyML" + Path.DirectorySeparatorChar + "pyScripts";
#endif
            return location;
        }


        public Dictionary<String, String> parseParametersToLinearAndQuadraticBinarySampling(string[] param)
        {
            Dictionary<string, string> parameter = new Dictionary<string, string>();

            if (param.Length > 0)
            {
                foreach (string par in param)
                {
                    if (par.Contains(':'))
                    {
                        string[] nameAndValue = par.Split(':');
                        parameter.Add(nameAndValue[0], nameAndValue[1]);
                    }
                    else
                    {
                        parameter.Add(par, "");
                    }
                }
            }
            return parameter;
        }
    }
}
