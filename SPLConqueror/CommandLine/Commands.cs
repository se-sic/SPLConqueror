using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using MachineLearning.Sampling.ExperimentalDesigns;


namespace CommandLine
{
    class Commands
    {

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

                case "clean":

                    SPLConqueror_Core.GlobalState.clear();
                    break;
                case "all":
                    GlobalState.allMeasurements.Configurations = ConfigurationReader.readConfigurations(task, GlobalState.varModel);
                    SPLConqueror_Core.ErrorLog.logError("Configurations loaded.");

                    break;
                case "allBoolean": // all binary configurations 
                    break;
                case "expDesign":

                    performOneCommand_ExpDesign(task);
                    // TODO start measurement here or insert new command

                    break;

                case "vm":
                // TODO remove fm 
                case "fm":
                    GlobalState.varModel = VariabilityModel.loadFromXML(task);
                    break;
                case "featureWise":
                    // Sampling FW
                    break;

                case "log":
                    // Define log file. 

                    // TODO add more log file functionality
                    break;
                case "MLsettings":
                    // TODO add MLsettings and dependency to ML/Sampling
                    break;

                case "pairWise":
                    // TODO add PW sampling
                    break;

                case "random":
                    // ramdom sampling
                    break;

                case "start":
                    // starts the machine learning 
                    break;

                case "trueModel":
                    // True model stored in GlobalState
                    break;
                case "negFW":
                    // TODO negFW sampling
                    break;
                default:
                    return command;
            }
            return "";
        }
        private string performOneCommand_ExpDesign(string task)
        {
            // splits the task in design and parameters of the design
            string[] designAndParams = task.Split(new Char[] { ' ' }, 2);
            string design = designAndParams[0];
            string param = "";
            if (designAndParams.Length > 1)
                param = designAndParams[1];
            string[] parameters = param.Split(' ');

            switch (design)
            {
                case "boxBehnken":
                    break;

                case "centralComposite":
                    CentralCompositeInscribedDesign cci = new CentralCompositeInscribedDesign(null);

                    break;

                case "featureInteraction":

                    break;

                case "fedorov":
                    break;

                case "hyperSampling":
                    break;

                case "independentLinear":
                    break;

                case "kExchange":
                    break;

                case "plackettBurman":

                    break;

                case "random":
                    break;

                default:
                    return task;
            }

            return "";
        }
    }
}
