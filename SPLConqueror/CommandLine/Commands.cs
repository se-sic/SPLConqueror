using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Heuristics;

using MachineLearning.Learning;
using MachineLearning.Solver;

using MachineLearning.Learning.Regression;

namespace CommandLine
{
    class Commands
    {

        ExperimentState exp = new ExperimentState();

        /// <summary>
        /// Performs the functionality of one command. If no functionality is found for the command, the command is retuned by this method. 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string performOneCommand(string line)
        {
            InfoLog.logInfo("Command: "+line);


            // remove comment part of the line (the comment starts with an #)
            line = line.Split(new Char[] { '%' }, 2)[0];
            if (line.Length == 0)
                return "";

            // split line in command and parameters of the command
            string[] components = line.Split(new Char[] { ' ' }, 2);
            string command = components[0];
            string task = "";
            if (components.Length > 1)
                task = components[1];

            switch (command)
            {

                case "clean-global":
                    SPLConqueror_Core.GlobalState.clear();
                    break;
                case "clean-sampling":
                    exp.clearSampling();
                    break;
                case "clean-learning":
                    exp.clear();
                    break;
                case "all":
                    GlobalState.allMeasurements.Configurations = ConfigurationReader.readConfigurations(task, GlobalState.varModel);
                    InfoLog.logInfo("Configurations loaded.");

                    break;
                case "allBinary":
                    {
                        VariantGenerator vg = new VariantGenerator(null);
                        exp.addBinarySelection(vg.generateAllVariantsFast(GlobalState.varModel));
                        exp.addBinarySampling("all-Binary");
                        break;
                    }
                case "expDesign":
                    performOneCommand_ExpDesign(task);
                    break;

                case "vm":
                    GlobalState.varModel = VariabilityModel.loadFromXML(task);
                    break;
                case "featureWise":
                    FeatureWise fw = new FeatureWise();
                    exp.addBinarySelection(fw.generateFeatureWiseConfigsCSP(GlobalState.varModel));
                    //exp.addBinarySelection(fw.generateFeatureWiseConfigurations(GlobalState.varModel));
                    exp.addBinarySampling("FW");

                    break;

                case "log":
                    // Define log file. 

                    // TODO add more log file functionality
                    break;
                case "MLsettings":
                    {
                        string[] para = task.Split(new char[] { ' ' });
                        exp.mlSettings.setSetting(para[0], para[1]);
                        break;
                    }
                case "load_MLsettings":
                    exp.mlSettings = ML_Settings.readSettings(task);
                    break;

                case "pairWise":
                    PairWise pw = new PairWise();
                    exp.addBinarySelection(pw.generatePairWiseVariants(GlobalState.varModel));
                    exp.addBinarySampling("PW");                    
                    break;

                case "printSettings":
                    InfoLog.logInfo(exp.mlSettings.ToString());
                    break;

                case "printConfigs":
                    {
                        List<Dictionary<NumericOption, double>> numericSampling = exp.NumericSelection;
                        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySampling = exp.BinarySelections;

                        List<Configuration> configurations = new List<Configuration>();

                        foreach (Dictionary<NumericOption, double> numeric in numericSampling)
                        {
                            foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> binary in binarySampling)
                            {
                                Configuration config = Configuration.getConfiguration(binary, numeric);
                                if (!configurations.Contains(config))
                                {
                                    configurations.Add(config);
                                }
                            }
                        }

                        string[] para = task.Split(new char[] { ' ' });

                        ConfigurationPrinter printer = new ConfigurationPrinter(para[0], para[1], para[2]);
                        printer.print(configurations);

                        break;
                    }
                case "random":
                    {
                        string[] para = task.Split(new char[] { ' ' });
                        int treshold = Convert.ToInt32(para[0]);
                        int modulu = Convert.ToInt32(para[1]);

                        VariantGenerator vg = new VariantGenerator(null);
                        exp.addBinarySelection(vg.generateRandomVariants(GlobalState.varModel, treshold, modulu));
                        exp.addBinarySampling("random " + task);
                        break;
                    }
                case "start":
                    {
                        List<Dictionary<NumericOption, double>> numericSampling = exp.NumericSelection;
                        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySampling = exp.BinarySelections;

                        List<Configuration> configurations = new List<Configuration>();

                        foreach(Dictionary<NumericOption, double> numeric in numericSampling)
                        {
                            foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> binary in binarySampling)
                            {
                                Configuration config = Configuration.getConfiguration(binary, numeric);
                                if (!configurations.Contains(config))
                                {
                                    configurations.Add(config);
                                }
                            }
                        }
                        InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                    
                        // starts the machine learning 
                        FeatureSubsetSelection fss = new FeatureSubsetSelection(infMod,exp.mlSettings);


                    }
                    break;

                case "trueModel":
                    // True model stored in GlobalState?
                    break;
                case "negFW":
                    // TODO there are two different variants in generating NegFW configurations. 
                    NegFeatureWise neg = new NegFeatureWise();
                    exp.addBinarySelection(neg.generateNegativeFW(GlobalState.varModel));
                    exp.addBinarySampling("newFW");
                    break;
                default:
                    return command;
            }
            return "";
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
        /// Note: An experimental design might have parameters and might consider only a specific set of numeric options. 
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
                        string[] nameAndValue = par.Split(':');
                        parameter.Add(nameAndValue[0], nameAndValue[1]);
                    }
                }

            }
            if (optionsToConsider.Count == 0)
                optionsToConsider = GlobalState.varModel.NumericOptions;

            ExperimentalDesign design = null;

            switch (designName)
            {
                case "boxBehnken":
                    design = new BoxBehnkenDesign(optionsToConsider);
                    exp.addNumericSampling("boxBehnken");
                    break;
                case "centralComposite":
                    design = new CentralCompositeInscribedDesign(optionsToConsider);
                    exp.addNumericSampling("centralComposite");
                    break;
                case "fullFactorial":
                    design = new FullFactorialDesign(optionsToConsider);
                    exp.addNumericSampling("fullFactorial");
                    break;
                case "featureInteraction":

                    break;

                case "fedorov":
                    break;

                case "hyperSampling":
                    design = new HyperSampling(optionsToConsider);
                    exp.addNumericSampling("hyperSampling");
                    break;

                case "independentLinear":
                    break;

                case "kExchange":
                    design = new KExchangeAlgorithm(optionsToConsider);
                    exp.addNumericSampling("kExchange");
                    break;

                case "plackettBurman":
                    design = new PlackettBurmanDesign(optionsToConsider);
                    exp.addNumericSampling("plackettBurman");
                    break;

                case "random":
                    design = new RandomSampling(optionsToConsider);
                    exp.addNumericSampling("random");
                    break;

                default:
                    return task;
            }

            design.computeDesign(parameter);
            exp.addNumericalSelection(design.SelectedConfigurations);


            return "";
        }
    }
}
