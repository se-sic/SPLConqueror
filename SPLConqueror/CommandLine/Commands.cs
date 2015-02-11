using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Heuristics;


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
                    SPLConqueror_Core.ErrorLog.logError("Configurations loaded.");

                    break;
                case "allBoolean": // all binary configurations 
                    break;
                case "expDesign":
                    performOneCommand_ExpDesign(task);
                    break;

                case "vm":
                    GlobalState.varModel = VariabilityModel.loadFromXML(task);
                    break;
                case "featureWise":
                    FeatureWise fw = new FeatureWise();
                    exp.addBinarySelection(fw.generateFeatureWiseConfigurations(GlobalState.varModel));
                    exp.addBinarySampling("FW");

                    break;

                case "log":
                    // Define log file. 

                    // TODO add more log file functionality
                    break;
                case "MLsettings":
                    // TODO add MLsettings and dependency to ML/Sampling
                    break;

                case "pairWise":
                    PairWise pw = new PairWise();
                    exp.addBinarySelection(pw.generatePairWiseVariants(GlobalState.varModel));
                    exp.addBinarySampling("PW");                    
                    break;

                case "random":
                    // ramdom sampling
                    break;

                case "start":
                    // starts the machine learning 
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
