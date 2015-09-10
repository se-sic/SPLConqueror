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

namespace CommandLine
{
    public class Commands
    {
        public const string COMMAND = "command: ";
        public const string COMMAND_TRUEMODEL = "truemodel";

        public const string COMMAND_LOG = "log";

        public const string COMMAND_CLEAR_GLOBAL = "clean-global";
        public const string COMMAND_CLEAR_SAMPLING = "clean-sampling";
        public const string COMMAND_CLEAR_LEARNING = "clean-learning";

        public const string COMMAND_LOAD_CONFIGURATIONS = "all";
        public const string COMMAND_LOAD_MLSETTINGS = "load_mlsettings";

        public const string COMMAND_VALIDATION = "validation";

        public const string COMMAND_EVALUATION_SET = "evaluationset";

        public const string COMMAND_SAMPLE_ALLBINARY = "allbinary";
        public const string COMMAND_SAMPLE_OPTIONWISE = "featurewise";
        public const string COMMAND_SAMPLE_PAIRWISE = "pairwise";
        public const string COMMAND_SAMPLE_NEGATIVE_OPTIONWISE = "negfw";
        public const string COMMAND_SAMPLE_BINARY_RANDOM = "random";

        public const string COMMAND_ANALYZE_LEARNING = "analyze-learning";
        public const string COMMAND_PRINT_MLSETTINGS = "printsettings";
        
        // using this option, a partial or full option order can be defined. The order is used during the printconfigs command. To define an order, the names of the options separated with whitespace have to be defined. If an option is not defined in the ordering its name and the value is printed at the end of the configurtion. 
        public const string COMMAND_SAMPLING_OPTIONORDER = "optionorder";
        public const string COMMAND_PRINT_CONFIGURATIONS = "printconfigs";

        public const string COMMAND_VARIABILITYMODEL = "vm";
        public const string COMMAND_SET_NFP = "nfp";
        public const string COMMAND_SET_MLSETTING = "mlsettings";

        public const string COMMAND_START_LEARNING = "start";

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


        public ExperimentState exp = new ExperimentState();

        /// <summary>
        /// Performs the functionality of one command. If no functionality is found for the command, the command is retuned by this method. 
        /// </summary>
        /// <param name="line">One command with its parameters.</param>
        /// <returns>Returns an empty string if the command could be performed by the method. If the command could not be performed by the method, the original command is returned.</returns>
        public string performOneCommand(string line)
        {
            GlobalState.logInfo.log(COMMAND + line);


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
                case COMMAND_TRUEMODEL:
                    StreamReader readModel = new StreamReader(task);
                    String model = readModel.ReadLine().Trim();
                    readModel.Close();
                    exp.TrueModel = new InfluenceFunction(model.Replace(',','.'), GlobalState.varModel);
                    NFProperty artificalProp = new NFProperty("artificial");
                    GlobalState.currentNFP = artificalProp;
                    computeEvaluationDataSetBasedOnTrueModel();
                    break;

                case COMMAND_SUBSCRIPT:
                    {

                        FileInfo fi = new FileInfo(task);
                        StreamReader reader = null;
                        if (!fi.Exists)
                            throw new FileNotFoundException(@"Automation script not found. ", fi.ToString());

                        reader = fi.OpenText();
                        Commands co = new Commands();
                        co.exp = this.exp;

                        while (!reader.EndOfStream)
                        {
                            String oneLine = reader.ReadLine().Trim();
                            co.performOneCommand(oneLine);

                        }
                    }
                    break;
                case COMMAND_EVALUATION_SET:
                    {
                        GlobalState.evalutionSet.Configurations = ConfigurationReader.readConfigurations(task, GlobalState.varModel);
                        GlobalState.logInfo.log("Evaluation set loaded.");
                    }
                    break;
                case COMMAND_CLEAR_GLOBAL:
                    SPLConqueror_Core.GlobalState.clear();
                    break;
                case COMMAND_CLEAR_SAMPLING:
                    exp.clearSampling();
                    break;
                case COMMAND_CLEAR_LEARNING:
                    exp.clear();
                    break;
                case COMMAND_LOAD_CONFIGURATIONS:
                    GlobalState.allMeasurements.Configurations = (GlobalState.allMeasurements.Configurations.Union(ConfigurationReader.readConfigurations(task, GlobalState.varModel))).ToList();
                    GlobalState.logInfo.log(GlobalState.allMeasurements.Configurations.Count + " configurations loaded.");

                    break;
                case COMMAND_SAMPLE_ALLBINARY:
                    {
                        VariantGenerator vg = new VariantGenerator(null);
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            exp.addBinarySelection_Validation(vg.generateAllVariantsFast(GlobalState.varModel));
                            exp.addBinarySampling_Validation(COMMAND_SAMPLE_ALLBINARY);
                        }
                        else
                        {
                            exp.addBinarySelection_Learning(vg.generateAllVariantsFast(GlobalState.varModel));
                            exp.addBinarySampling_Learning(COMMAND_SAMPLE_ALLBINARY);
                        }

                        break;
                    }
                case COMMAND_ANALYZE_LEARNING:
                    {
                        GlobalState.logInfo.log("Models:");
                        FeatureSubsetSelection learning = exp.learning;
                        if (learning == null)
                        {
                            GlobalState.logError.log("Error... learning was not performed!");
                            break;
                        }
                        foreach (LearningRound lr in learning.LearningHistory)
                        {
                            double relativeError = 0;
                            if (GlobalState.evalutionSet.Configurations.Count > 0)
                            {
                                double relativeErro2r = exp.learning.computeError(lr.FeatureSet, GlobalState.evalutionSet.Configurations, out relativeError);
                            }
                            else
                            {
                                double relativeErro2r = exp.learning.computeError(lr.FeatureSet, GlobalState.allMeasurements.Configurations, out relativeError);
                            }

                            GlobalState.logInfo.log(lr.ToString() + relativeError);
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
                        GlobalState.logError.log("No variability model found at " + task);
                    break;
                case COMMAND_SET_NFP:
                    GlobalState.currentNFP = GlobalState.getOrCreateProperty(task.Trim());
                    break;
                case COMMAND_SAMPLE_OPTIONWISE:
                    FeatureWise fw = new FeatureWise();
                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        exp.addBinarySelection_Validation(fw.generateFeatureWiseConfigsCSP(GlobalState.varModel));
                        exp.addBinarySampling_Validation("FW");
                    }
                    else
                    {
                        //exp.addBinarySelection_Learning(fw.generateFeatureWiseConfigsCSP(GlobalState.varModel));
                        exp.addBinarySelection_Learning(fw.generateFeatureWiseConfigurations(GlobalState.varModel));
                        exp.addBinarySampling_Learning("FW");
                    }
                    break;

                case COMMAND_LOG:

                    string location = task.Trim();
                    GlobalState.logInfo.close();
                    GlobalState.logInfo = new InfoLogger(location);

                    GlobalState.logError.close();
                    GlobalState.logError = new ErrorLogger(location + "_error");
                    break;
                case COMMAND_SET_MLSETTING:
                    exp.mlSettings = ML_Settings.readSettings(task);
                    break;
                case COMMAND_LOAD_MLSETTINGS:
                    exp.mlSettings = ML_Settings.readSettingsFromFile(task);
                    break;

                case COMMAND_SAMPLE_PAIRWISE:
                    PairWise pw = new PairWise();
                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        exp.addBinarySelection_Validation(pw.generatePairWiseVariants(GlobalState.varModel));
                        exp.addBinarySampling_Validation("PW");
                    }
                    else
                    {
                        exp.addBinarySelection_Learning(pw.generatePairWiseVariants(GlobalState.varModel));
                        exp.addBinarySampling_Learning("PW");
                    }
                    break;

                case COMMAND_PRINT_MLSETTINGS:
                    GlobalState.logInfo.log(exp.mlSettings.ToString());
                    break;

                case COMMAND_PRINT_CONFIGURATIONS:
                    {
                        List<Dictionary<NumericOption, double>> numericSampling = exp.NumericSelection_Learning;
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
                        }
                        string[] para = task.Split(new char[] { ' ' });
                        // TODO very error prune..
                        ConfigurationPrinter printer = new ConfigurationPrinter(para[0], para[1], para[2], GlobalState.optionOrder);
                        printer.print(configurations);

                        break;
                    }
                case COMMAND_SAMPLE_BINARY_RANDOM:
                    {
                        string[] para = task.Split(new char[] { ' ' });
                        int treshold = Convert.ToInt32(para[0]);
                        int modulu = Convert.ToInt32(para[1]);

                        VariantGenerator vg = new VariantGenerator(null);
                        if (taskAsParameter.Contains(COMMAND_VALIDATION))
                        {
                            exp.addBinarySelection_Validation(vg.generateRandomVariants(GlobalState.varModel, treshold, modulu));
                            exp.addBinarySampling_Validation("random " + task);
                        }
                        else
                        {
                            exp.addBinarySelection_Learning(vg.generateRandomVariants(GlobalState.varModel, treshold, modulu));
                            exp.addBinarySampling_Learning("random " + task);
                        }
                        break;
                    }
                case COMMAND_START_LEARNING:
                    {
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);

                        List<Configuration> configurations_Learning = new List<Configuration>();

                        List<Configuration> configurations_Validation = new List<Configuration>();

                        if (exp.TrueModel == null)
                        {
                            //List<List<BinaryOption>> availableBinary 
                            //configurations_Learning = GlobalState.getMeasuredConfigs(exp.BinarySelections_Learning, exp.NumericSelection_Learning);
                            configurations_Learning = GlobalState.getMeasuredConfigs(Configuration.getConfigurations(exp.BinarySelections_Learning, exp.NumericSelection_Learning));
                            configurations_Learning = configurations_Learning.Distinct().ToList();

                            configurations_Validation = GlobalState.getMeasuredConfigs(Configuration.getConfigurations(exp.BinarySelections_Validation, exp.NumericSelection_Validation));
                            configurations_Validation = configurations_Validation.Distinct().ToList();
                            //break;//todo only to get the configurations that we haven't measured
                        } else
                        {
                            foreach (List<BinaryOption> binConfig in exp.BinarySelections_Learning)
                            {
                                if (exp.NumericSelection_Learning.Count == 0)
                                {
                                    Configuration c = new Configuration(binConfig);
                                    c.setMeasuredValue(GlobalState.currentNFP, exp.TrueModel.eval(c));
                                    if (!configurations_Learning.Contains(c))
                                        configurations_Learning.Add(c);
                                    continue;
                                }
                                foreach (Dictionary<NumericOption, double> numConf in exp.NumericSelection_Learning)
                                {

                                    Configuration c = new Configuration(binConfig, numConf);
                                    c.setMeasuredValue(GlobalState.currentNFP, exp.TrueModel.eval(c));
                                    if(GlobalState.varModel.configurationIsValid(c))
                //                    if (!configurations_Learning.Contains(c))
                                        configurations_Learning.Add(c);
                                }
                            }

                        }
                            if (configurations_Learning.Count == 0)
                            {
                                configurations_Learning = configurations_Validation;
                            }

                            if (configurations_Learning.Count == 0)
                            {
                                GlobalState.logInfo.log("The learning set is empty! Cannot start learning!");
                                break;
                            }

                            if (configurations_Validation.Count == 0)
                            {
                                configurations_Validation = configurations_Learning;
                            }
                            //break;
                            GlobalState.logInfo.log("Learning: " + "NumberOfConfigurationsLearning:" + configurations_Learning.Count + " NumberOfConfigurationsValidation:" + configurations_Validation.Count
                            + " UnionNumberOfConfigurations:" + (configurations_Learning.Union(configurations_Validation)).Count());
                            
                        // prepare the machine learning 
                        exp.learning.init(infMod, exp.mlSettings);
                        exp.learning.setLearningSet(configurations_Learning);
                        exp.learning.setValidationSet(configurations_Validation);
                        exp.learning.learn();

                    }
                    break;

                case COMMAND_SAMPLE_NEGATIVE_OPTIONWISE:
                    // TODO there are two different variants in generating NegFW configurations. 
                    NegFeatureWise neg = new NegFeatureWise();

                    if (taskAsParameter.Contains(COMMAND_VALIDATION))
                    {
                        exp.addBinarySelection_Validation(neg.generateNegativeFW(GlobalState.varModel));
                        exp.addBinarySampling_Validation("newFW");
                    }
                    else
                    {
                        exp.addBinarySelection_Learning(neg.generateNegativeFW(GlobalState.varModel));//neg.generateNegativeFWAllCombinations(GlobalState.varModel));
                        exp.addBinarySampling_Learning("newFW");
                    }
                    break;
                default:
                    return command;
            }
            return "";
        }

        private void parseOptionOrder(string task)
        {
            String[] optionNames = task.Split(' ');
            foreach(String option in optionNames){
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
            VariantGenerator vg = new VariantGenerator(null);
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
                    c.setMeasuredValue(GlobalState.currentNFP, exp.TrueModel.eval(c));
                    GlobalState.addConfiguration(c);
                }
                foreach (Dictionary<NumericOption, double> numConf in numericConfigs)
                {

                    Configuration c = new Configuration(binConfig, numConf);
                    c.setMeasuredValue(GlobalState.currentNFP, exp.TrueModel.eval(c));
                    GlobalState.addConfiguration(c);
                }
            }
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

            ExperimentalDesign design = null;

            switch (designName.ToLower())
            {
                case COMMAND_EXPDESIGN_BOXBEHNKEN:
                    design = new BoxBehnkenDesign(optionsToConsider);
                    break;
                case COMMAND_EXPDESIGN_CENTRALCOMPOSITE:
                    design = new CentralCompositeInscribedDesign(optionsToConsider);
                    break;
                case COMMAND_EXPDESIGN_FULLFACTORIAL:
                    design = new FullFactorialDesign(optionsToConsider);
                    break;
                case "featureInteraction":
                    break;

                case COMMAND_EXPDESIGN_HYPERSAMPLING:
                    design = new HyperSampling(optionsToConsider);
                    if(parameter.ContainsKey("precision"))
                        ((HyperSampling)design).Precision = Int32.Parse(parameter["precision"]);
                    break;

                case COMMAND_EXPDESIGN_ONEFACTORATATIME:
                    design = new OneFactorAtATime(optionsToConsider);
                    if(parameter.ContainsKey("distinctValuesPerOption"))
                        ((OneFactorAtATime)design).distinctValuesPerOption = Int32.Parse(parameter["distinctValuesPerOption"]);
                    break;

                case COMMAND_EXPDESIGN_KEXCHANGE:
                    design = new KExchangeAlgorithm(optionsToConsider);
                    break;

                case COMMAND_EXPDESIGN_PLACKETTBURMAN:
                    design = new PlackettBurmanDesign(optionsToConsider);
                    if(parameter.ContainsKey("measurements") && parameter.ContainsKey("level"))
                        ((PlackettBurmanDesign)design).setSeed(Int32.Parse(parameter["measurements"]), Int32.Parse(parameter["level"]));
                    break;

                case COMMAND_EXPDESIGN_RANDOM:
                    design = new RandomSampling(optionsToConsider);
                    break;

                default:
                    return task;
            }

            design.computeDesign(parameter);
            if (parameter.ContainsKey("validation"))
            {
                exp.addNumericSampling_Validation(design.getName());
                exp.addNumericalSelection_Validation(design.SelectedConfigurations);
            }
            else
            {
                exp.addNumericSampling_Learning(design.getName());
                exp.addNumericalSelection_Learning(design.SelectedConfigurations);
            }

            return "";
        }
    }
}
