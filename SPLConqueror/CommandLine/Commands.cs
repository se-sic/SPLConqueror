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

        public const string COMMAND_VALIDATION = "validation";

        public const string COMMAND_EVALUATION_SET = "evaluationset";

        public const string COMMAND_SAMPLE_ALLBINARY = "allbinary";
        public const string COMMAND_SAMPLE_FEATUREWISE = "featurewise";
        public const string COMMAND_SAMPLE_OPTIONWISE = "optionwise";
        public const string COMMAND_SAMPLE_PAIRWISE = "pairwise";
        public const string COMMAND_SAMPLE_NEGATIVE_OPTIONWISE = "negfw";
        public const string COMMAND_SAMPLE_BINARY_RANDOM = "random";
        public const string COMMAND_SAMPLE_BINARY_LINEAR = "linear";
        public const string COMMAND_SAMPLE_BINARY_QUADRATIC = "quadratic";
        public const string COMMAND_SAMPLE_BINARY_MINMAX = "minmax";
        public const string COMMAND_SAMPLE_BINARY_TWISE = "twise";

        public const string COMMAND_START_ALLMEASUREMENTS = "learnwithallmeasurements";

        public const string COMMAND_ANALYZE_LEARNING = "analyze-learning";
        public const string COMMAND_PRINT_MLSETTINGS = "printsettings";

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
        public const string COMMAND_EXPDESIGN_HYPERSAMPLING = "hypersampling";
        public const string COMMAND_EXPDESIGN_ONEFACTORATATIME = "onefactoratatime";
        public const string COMMAND_EXPDESIGN_KEXCHANGE = "kexchange";
        public const string COMMAND_EXPDESIGN_PLACKETTBURMAN = "plackettburman";
        public const string COMMAND_EXPDESIGN_RANDOM = "random";

        public const string COMMAND_SUBSCRIPT = "script";

        public const string DEFINE_PYTHON_PATH = "define-python-path";
        public const string COMMAND_PYTHON_LEARN = "learn-python";
        public const string COMMAND_PYTHON_LEARN_OPT = "learn-python-opt";

        List<SamplingStrategies> toSample = new List<SamplingStrategies>();
        List<SamplingStrategies> toSampleValidation = new List<SamplingStrategies>();
        ML_Settings mlSettings = new ML_Settings();
        InfluenceFunction trueModel = null;

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
            GlobalState.logInfo.logLine(COMMAND + line);


            // remove comment part of the line (the comment starts with an #)
            line = line.Split(new Char[] { '#' }, 2)[0];
            if (line.Length == 0)
                return "";

            // split line in command and parameters of the command
            string[] components = line.Split(new Char[] { ' ' }, 2);
            string command = components[0];
            string task = "";
            if (components.Length > 1)
                task = components[1];

            string[] taskAsParameter = task.Split(new Char[] { ' ' });

            switch (command.ToLower())
            {
                case COMMAND_START_ALLMEASUREMENTS:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);

                        List<Configuration> configurations_Learning = new List<Configuration>();

                        foreach (Configuration config in GlobalState.allMeasurements.Configurations)
                        {
                            if (config.nfpValues.ContainsKey(GlobalState.currentNFP))
                                configurations_Learning.Add(config);
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
                    }
                    break;

                case COMMAND_TRUEMODEL:
                    StreamReader readModel = new StreamReader(task);
                    String model = readModel.ReadLine().Trim();
                    readModel.Close();
                    this.trueModel = new InfluenceFunction(model.Replace(',', '.'), GlobalState.varModel);
                    NFProperty artificalProp = new NFProperty("artificial");
                    GlobalState.currentNFP = artificalProp;
                    //computeEvaluationDataSetBasedOnTrueModel();
                    break;

                case COMMAND_SUBSCRIPT:
                    {

                        FileInfo fi = new FileInfo(task);
                        StreamReader reader = null;
                        if (!fi.Exists)
                            GlobalState.logError.logLine(@"Automation script not found. " + fi.ToString());
                        else
                        {
                            reader = fi.OpenText();
                            Commands co = new Commands();
                            co.exp = this.exp;

                            while (!reader.EndOfStream)
                            {
                                String oneLine = reader.ReadLine().Trim();
                                co.performOneCommand(oneLine);

                            }
                        }
                    }
                    break;
                case COMMAND_EVALUATION_SET:
                    {
                        GlobalState.evalutionSet.Configurations = ConfigurationReader.readConfigurations(task, GlobalState.varModel);
                        GlobalState.logInfo.logLine("Evaluation set loaded.");
                    }
                    break;
                case COMMAND_CLEAR_GLOBAL:
                    SPLConqueror_Core.GlobalState.clear();
                    toSample.Clear();
                    toSampleValidation.Clear();
                    break;
                case COMMAND_CLEAR_SAMPLING:
                    exp.clearSampling();
                    toSample.Clear();
                    toSampleValidation.Clear();
                    break;
                case COMMAND_CLEAR_LEARNING:
                    exp.clear();
                    toSample.Clear();
                    toSampleValidation.Clear();
                    break;
                case COMMAND_LOAD_CONFIGURATIONS:
                    GlobalState.allMeasurements.Configurations = (GlobalState.allMeasurements.Configurations.Union(ConfigurationReader.readConfigurations(task, GlobalState.varModel))).ToList();
                    GlobalState.logInfo.logLine(GlobalState.allMeasurements.Configurations.Count + " configurations loaded.");

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
                            this.toSampleValidation.Add(SamplingStrategies.ALLBINARY);
                            this.exp.info.binarySamplings_Validation = "ALLBINARY";
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.ALLBINARY);
                            this.exp.info.binarySamplings_Learning = "ALLBINARY";
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
                                    if (GlobalState.evalutionSet.Configurations.Count > 0)
                                    {
                                        double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.evalutionSet.Configurations, out relativeError);
                                    }
                                    else
                                    {
                                        double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError);
                                    }

                                    GlobalState.logInfo.logLine(lr.ToString() + relativeError);
                                }
                            }
                        }
                        else
                        {
                            FeatureSubsetSelection learnedModel = exp.models[0];
                            if (learnedModel == null)
                            {
                                GlobalState.logError.logLine("Error... learning was not performed!");
                                break;
                            }
                            GlobalState.logInfo.logLine("Termination reason: " + learnedModel.LearningHistory.Last().terminationReason);
                            foreach (LearningRound lr in learnedModel.LearningHistory)
                            {
                                double relativeError = 0;
                                if (GlobalState.evalutionSet.Configurations.Count > 0)
                                {
                                    double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.evalutionSet.Configurations, out relativeError);
                                }
                                else
                                {
                                    double relativeErro2r = learnedModel.computeError(lr.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError);
                                }

                                GlobalState.logInfo.logLine(lr.ToString() + relativeError);
                            }
                        }


                        break;
                    }
                case COMMAND_EXERIMENTALDESIGN:
                    performOneCommand_ExpDesign(task);
                    break;

                case COMMAND_SAMPLING_OPTIONORDER:
                    parseOptionOrder(task);
                    break;

                case COMMAND_VARIABILITYMODEL:
                    GlobalState.varModel = VariabilityModel.loadFromXML(task);
                    if (GlobalState.varModel == null)
                        GlobalState.logError.logLine("No variability model found at " + task);
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
                        this.toSampleValidation.Add(SamplingStrategies.OPTIONWISE);
                        this.exp.info.binarySamplings_Validation = "OPTIONSWISE";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.OPTIONWISE);
                        this.exp.info.binarySamplings_Learning = "OPTIONSWISE";
                    }
                    break;

                case COMMAND_SAMPLE_BINARY_LINEAR:
                    {
                        string[] para = task.Split(new char[] { ' ' });

                        Dictionary<String, String> prameters = parseParametersToLinearAndQuadraticBinarySampling(para);

                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.toSampleValidation.Add(SamplingStrategies.BINARY_LINEAR);
                            this.exp.info.binarySamplings_Validation = "BINARY_LINEAR "+ task.Replace(":", "_");
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.BINARY_LINEAR);
                            this.exp.info.binarySamplings_Learning = "BINARY_LINEAR "+ task.Replace(":", "_");
                        }
                        if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.BINARY_LINEAR))
                        {
                            ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.BINARY_LINEAR, new List<Dictionary<string, string>>());
                        }
                        ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.BINARY_LINEAR].Add(prameters);
                    }
                    break;

                case COMMAND_SAMPLE_BINARY_QUADRATIC:
                    { 
                        string[] para = task.Split(new char[] { ' ' });

                        Dictionary<String, String> prameters = parseParametersToLinearAndQuadraticBinarySampling(para);

                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.toSampleValidation.Add(SamplingStrategies.BINARY_QUADRATIC);
                            this.exp.info.binarySamplings_Validation = "BINARY_QUADRATIC "+ task.Replace(":","_");
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.BINARY_QUADRATIC);
                            this.exp.info.binarySamplings_Learning = "BINARY_QUADRATIC " + task.Replace(":", "_");
                        }

                        if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.BINARY_LINEAR))
                        {
                            ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.BINARY_LINEAR, new List<Dictionary<string, string>>());
                        }
                        ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.BINARY_LINEAR].Add(prameters);
                    }
                    break;

                case COMMAND_SAMPLE_BINARY_TWISE:
                    {
                        string[] para = task.Split(new char[] { ' ' });

                        Dictionary<String, String> prameters = parseParametersToLinearAndQuadraticBinarySampling(para);

                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.toSampleValidation.Add(SamplingStrategies.T_WISE);
                            this.exp.info.binarySamplings_Validation = "T_WISE ";
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.T_WISE);
                            this.exp.info.binarySamplings_Learning = "T_WISE ";
                        }

                        if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.T_WISE))
                        {
                            ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.T_WISE, new List<Dictionary<string, string>>());
                        }
                        ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.T_WISE].Add(prameters);
                    }
                    break;
                case COMMAND_SAMPLE_BINARY_MINMAX:
                    {
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.toSampleValidation.Add(SamplingStrategies.MINMAX);
                            this.exp.info.binarySamplings_Validation = "MINMAX";
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.MINMAX);
                            this.exp.info.binarySamplings_Learning = "MINMAX";
                        }
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
                        this.toSampleValidation.Add(SamplingStrategies.PAIRWISE);
                        this.exp.info.binarySamplings_Validation = "PAIRWISE";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.PAIRWISE);
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

                        var configs = ConfigurationBuilder.buildConfigs(GlobalState.varModel, this.toSample);

                        string[] para = task.Split(new char[] { ' ' });
                        // TODO very error prone..
                        ConfigurationPrinter printer = new ConfigurationPrinter(para[0], para[1], para[2], GlobalState.optionOrder);
                        printer.print(configs);

                        break;
                    }
                case COMMAND_SAMPLE_BINARY_RANDOM:
                    {
                        string[] para = task.Split(new char[] { ' ' });
                        ConfigurationBuilder.binaryThreshold = Convert.ToInt32(para[0]);
                        ConfigurationBuilder.binaryModulu = Convert.ToInt32(para[1]);

                        VariantGenerator vg = new VariantGenerator(null);
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            this.toSampleValidation.Add(SamplingStrategies.BINARY_RANDOM);
                            this.exp.info.binarySamplings_Validation = "BINARY_RANDOM";
                        }
                        else
                        {
                            this.toSample.Add(SamplingStrategies.BINARY_RANDOM);
                            this.exp.info.binarySamplings_Learning = "BINARY_RANDOM " + task;
                        }
                        break;
                    }

                case DEFINE_PYTHON_PATH:
                    {
                        PythonWrapper.PYTHON_PATH = taskAsParameter[0] + "python.exe";
                        break;
                    }


                case COMMAND_PYTHON_LEARN:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        List<Configuration> configurationsLearning = buildSet(this.toSample);
                        List<Configuration> configurationsValidation = buildSet(this.toSampleValidation);

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
                        if (ProcessWrapper.LearningSettings.isLearningStrategy(taskAsParameter[0])) {
                            PythonWrapper pyInterpreter = new PythonWrapper(this.getLocationPythonScript() + Path.DirectorySeparatorChar + PythonWrapper.COMMUNICATION_SCRIPT, taskAsParameter);
                            GlobalState.logInfo.logLine("Starting Prediction");
                            pyInterpreter.setupApplication(configurationsLearning, GlobalState.allMeasurements.Configurations, PythonWrapper.START_LEARN);
                            PythonPredictionWriter csvWriter = new PythonPredictionWriter(targetPath, taskAsParameter, GlobalState.varModel.Name + "_"+ samplingIdentifier);
                            pyInterpreter.getLearningResult(GlobalState.allMeasurements.Configurations, csvWriter);
                            GlobalState.logInfo.logLine("Prediction finished, results written in " + csvWriter.getPath());
                            csvWriter.close();
                        } else
                        {
                            GlobalState.logInfo.logLine("Invalid Learning strategy " + taskAsParameter[0] + "! Aborting learning");
                        }
                        break;
                    }


                case COMMAND_PYTHON_LEARN_OPT:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        List<Configuration> configurationsLearning = buildSet(this.toSample);
                        List<Configuration> configurationsValidation = buildSet(this.toSampleValidation);
                        
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
                        } else
                        {
                            GlobalState.logInfo.logLine("Invalid learning strategy " + taskAsParameter[0] + "! Aborting Learning");
                        }
                        break;
                    }

                    
                case COMMAND_START_LEARNING:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        List<Configuration> configurationsLearning = buildSet(this.toSample);
                        List<Configuration> configurationsValidation = buildSet(this.toSampleValidation);

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
                        exp.models.Clear();
                        var mod = exp.models;
                        exp = new MachineLearning.Learning.Regression.Learning(configurationsLearning, configurationsValidation);
                        exp.models = mod;

                        exp.metaModel = infMod;
                        exp.mlSettings = this.mlSettings;
                        exp.learn();
                        GlobalState.logInfo.logLine("average model: \n" + exp.metaModel.printModelAsFunction());
                        double relativeerror = 0;
                        if (GlobalState.evalutionSet.Configurations.Count > 0)
                        {
                            relativeerror = FeatureSubsetSelection.computeError(exp.metaModel, GlobalState.evalutionSet.Configurations, ML_Settings.LossFunction.RELATIVE);
                        }
                        else
                        {
                            relativeerror = FeatureSubsetSelection.computeError(exp.metaModel, GlobalState.allMeasurements.Configurations, ML_Settings.LossFunction.RELATIVE);
                        }

                        //    globalstate.loginfo.logline("error :" + relativeerror);

                        break;
                    }
                case COMMAND_OPTIMIZE_PARAMETER:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                        List<Configuration> configurationsLearning = buildSet(this.toSample);
                        List<Configuration> configurationsValidation = buildSet(this.toSampleValidation);

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
                        this.toSampleValidation.Add(SamplingStrategies.NEGATIVE_OPTIONWISE);
                        this.exp.info.binarySamplings_Validation = "NEGATIVE_OPTIONWISE";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.NEGATIVE_OPTIONWISE);
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
            sb.Append(this.exp.info.binarySamplings_Learning+"_");

            // add numeric sampling strategy to the identifier
            foreach (KeyValuePair<SamplingStrategies,List<Dictionary<string,string>>> sampling in ConfigurationBuilder.parametersOfExpDesigns)
            {
                foreach(Dictionary<string,string> allParameters in sampling.Value)
                {
                    StringBuilder parameterString = new StringBuilder();
                    foreach (KeyValuePair<string,string> parameters in allParameters)
                    {
                        parameterString.Append(parameters.Key + "-" + parameters.Value+"_");
                    }
                    sb.Append("_"+sampling.Key + "--" + parameterString);
                }
            }
            return sb.ToString();
        }

        private List<Configuration> buildSet(List<SamplingStrategies> strats)
        {
            List<Configuration> configurationsTest = ConfigurationBuilder.buildConfigs(GlobalState.varModel, strats);
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


            switch (designName.ToLower())
            {
                case COMMAND_EXPDESIGN_BOXBEHNKEN:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.BOXBEHNKEN);
                        this.exp.info.numericSamplings_Validation = "BOXBEHNKEN";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.BOXBEHNKEN);
                        this.exp.info.numericSamplings_Learning = "BOXBEHNKEN";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.BOXBEHNKEN))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.BOXBEHNKEN, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.BOXBEHNKEN].Add(parameter);
                    break;
                case COMMAND_EXPDESIGN_CENTRALCOMPOSITE:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.CENTRALCOMPOSITE);
                        this.exp.info.numericSamplings_Validation = "CENTRALCOMPOSITE";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.CENTRALCOMPOSITE);
                        this.exp.info.numericSamplings_Learning = "CENTRALCOMPOSITE";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.CENTRALCOMPOSITE))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.CENTRALCOMPOSITE, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.CENTRALCOMPOSITE].Add(parameter);
                    break;
                case COMMAND_EXPDESIGN_FULLFACTORIAL:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.FULLFACTORIAL);
                        this.exp.info.numericSamplings_Validation = "FULLFACTORIAL";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.FULLFACTORIAL);
                        this.exp.info.numericSamplings_Learning = "FULLFACTORIAL";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.FULLFACTORIAL))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.FULLFACTORIAL, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.FULLFACTORIAL].Add(parameter);
                    break;
                case "featureInteraction":
                    GlobalState.logError.logLine("not implemented yet");
                    break;

                case COMMAND_EXPDESIGN_HYPERSAMPLING:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.HYPERSAMPLING);
                        this.exp.info.numericSamplings_Validation = "HYPERSAMPLING";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.HYPERSAMPLING);
                        this.exp.info.numericSamplings_Learning = "HYPERSAMPLING";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.HYPERSAMPLING))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.HYPERSAMPLING, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.HYPERSAMPLING].Add(parameter);
                    break;

                case COMMAND_EXPDESIGN_ONEFACTORATATIME:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.ONEFACTORATATIME);
                        this.exp.info.numericSamplings_Validation = "ONEFACTORATATIME";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.ONEFACTORATATIME);
                        this.exp.info.numericSamplings_Learning = "ONEFACTORATATIME";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.ONEFACTORATATIME))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.ONEFACTORATATIME, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.ONEFACTORATATIME].Add(parameter);
                    break;

                case COMMAND_EXPDESIGN_KEXCHANGE:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.KEXCHANGE);
                        this.exp.info.numericSamplings_Validation = "KEXCHANGE";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.KEXCHANGE);
                        this.exp.info.numericSamplings_Learning = "KEXCHANGE";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.KEXCHANGE))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.KEXCHANGE, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.KEXCHANGE].Add(parameter);
                    break;

                case COMMAND_EXPDESIGN_PLACKETTBURMAN:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.PLACKETTBURMAN);
                        this.exp.info.numericSamplings_Validation = "PLACKETTBURMAN";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.PLACKETTBURMAN);
                        this.exp.info.numericSamplings_Learning = "PLACKETTBURMAN";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.PLACKETTBURMAN))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.PLACKETTBURMAN, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.PLACKETTBURMAN].Add(parameter);
                    break;

                case COMMAND_EXPDESIGN_RANDOM:
                    if (parameter.ContainsKey("validation"))
                    {
                        this.toSampleValidation.Add(SamplingStrategies.RANDOM);
                        this.exp.info.numericSamplings_Validation = "RANDOM";
                    }
                    else
                    {
                        this.toSample.Add(SamplingStrategies.RANDOM);
                        this.exp.info.numericSamplings_Learning = "RANDOM";
                    }
                    if (!ConfigurationBuilder.parametersOfExpDesigns.ContainsKey(SamplingStrategies.RANDOM))
                    {
                        ConfigurationBuilder.parametersOfExpDesigns.Add(SamplingStrategies.RANDOM, new List<Dictionary<string, string>>());
                    }
                    ConfigurationBuilder.parametersOfExpDesigns[SamplingStrategies.RANDOM].Add(parameter);
                    break;

                default:
                    return task;
            }

            return "";
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


        public Dictionary<String,String> parseParametersToLinearAndQuadraticBinarySampling(string[] param)
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
