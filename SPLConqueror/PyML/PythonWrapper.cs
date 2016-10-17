using System.Diagnostics;
using SPLConqueror_Core;
using System.Collections.Generic;
using System;
using System.Text;

namespace ProcessWrapper
{
    public class PythonWrapper
    {
        private Process pythonProcess;

        protected string SCRIPT_LOCATION = System.IO.Path.DirectorySeparatorChar + "PyML" + System.IO.Path.DirectorySeparatorChar + "pyScripts";

        public const string COMMUNICATION_SCRIPT = "Communication.py";

        public static string PYTHON_PATH = "python";

        // Messages sent to the python process.

        /* These messages are used indicate the start and end of the sequenz, where 
         * the configuration data of the learner will be sent.
         * Used in initialize Learning.
        */
        private const string LEARNING_SETTINGS_STREAM_START = "settings_start";

        private const string LEARNING_SETTINGS_STREAM_END = "settings_end";
        
        /* Meassages to indicate the start and end of the stream of configurations.
         * Configurations will be sent in between the messages.
        */
        private const string CONFIG_LEARN_STREAM_START = "config_learn_start";

        private const string CONFIG_LEARN_STREAM_END = "config_learn_end";

        /* Meassages to indicate the start and end of the stream of configurations.
         * Configurations will be sent in between the messages.
        */
        private const string CONFIG_PREDICT_STREAM_START = "config_predict_start";

        private const string CONFIG_PREDICT_STREAM_END = "config_predict_end";

        /* Message to send the task that should be performed by the application.
         * Only one task can be performed by the process before terminating.
        */
        public const string START_LEARN = "start_learn";

        public const string START_PARAM_TUNING = "start_param_tuning";

        // Message to request of the results of the process.
        private const string REQUESTING_LEARNING_RESULTS = "req_results";


        // Messages received by the python process

        // Message to indicate that settings can be sent.
        private const string AWAITING_SETTINGS = "req_settings";

        // Message to indicate that configurations can be sent.
        private const string AWAITING_CONFIGS = "req_configs";

        // ACK.
        private const string PASS_OK = "pass_ok";

        // Message to indicate that the process has performed the task.
        private const string FINISHED_LEARNING = "learn_finished";

        private string[] mlProperties = null;

        /// <summary>
        /// Create a new wrapper that contains a running Python Process.
        /// </summary>
        /// <param name="path">The path of the source file called to start Python.</param>
        /// <param name="mlProperties">Configurations for the machine learning algorithm.</param>
        public PythonWrapper(string path, string[] mlProperties)
        {
            this.mlProperties = mlProperties;
            ProcessStartInfo pythonSetup = new ProcessStartInfo(PYTHON_PATH, path);
            pythonSetup.UseShellExecute = false;
            pythonSetup.RedirectStandardInput = true;
            pythonSetup.RedirectStandardOutput = true;
            pythonProcess = Process.Start(pythonSetup);
        }

        private string waitForNextReceivedLine()
        {
            string processOutput = "";
            while (processOutput == null || processOutput.Length == 0)
            {
                processOutput = pythonProcess.StandardOutput.ReadLine();
            }
            return processOutput;
        }

        private void passLineToApplication(string toPass)
        {
            pythonProcess.StandardInput.WriteLine(toPass);
        }

        /// <summary>
        /// Kill the process.
        /// </summary>
        public void endProcess()
        {
            pythonProcess.Close();
        }

        private void passConfigurations(List<Configuration> toPass)
        {
            passLineToApplication(CONFIG_LEARN_STREAM_START);

            foreach (Configuration config in toPass)
            {
                passLineToApplication(config.toNumeric());
                while (!waitForNextReceivedLine().Equals(PASS_OK))
                {
                    // Wait to make sure the previous input is processed in order to make sure receiver wont be overwhelmed
                }
            }

            passLineToApplication(CONFIG_LEARN_STREAM_END);
        }

        private void passConfigurationsPredict(List<Configuration> toPass)
        {
            passLineToApplication(CONFIG_PREDICT_STREAM_START);

            foreach (Configuration config in toPass)
            {
                passLineToApplication(config.toNumeric());
                while (!waitForNextReceivedLine().Equals(PASS_OK))
                {
                    // Wait to make sure the previous input is processed in order to make sure receiver wont be overwhelmed
                }
            }

            passLineToApplication(CONFIG_PREDICT_STREAM_END);
        }

        private void initializeLearning(string[] mlSettings)
        {
            passLineToApplication(LEARNING_SETTINGS_STREAM_START);
            passLineToApplication(mlSettings[0].Trim().ToLower());
            if (mlSettings.Length > 1)
            {
                for(int i = 1; i < mlSettings.Length; ++i)
                {
                    passLineToApplication(mlSettings[i]);
                }
            }
            passLineToApplication(LEARNING_SETTINGS_STREAM_END);
        }

        // currently replaced by PrintNfpPredictionsPython
        private string nfpPredictionsPython(string pythonList, List<Configuration> predictedConfigurations)
        {
            string[] separators = new String[] { "," };
            string[] predictions = pythonList.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();

            if (predictedConfigurations.Count != predictions.Length)
                GlobalState.logError.log("number of predictions using a python learner does not match with number of configurations");

            sb.Append("Configuration;Measured;Predicted\n");
            for (int i = 0; i < predictedConfigurations.Count; i++)
            {
                sb.Append(predictedConfigurations[i].ToString() + ";" + predictedConfigurations[i].GetNFPValue() + ";" + predictions[i] + "\n");
            }


            if (predictions.Length == GlobalState.varModel.optionToIndex.Count)
            {
                int counter = 0;
                foreach (KeyValuePair<int, ConfigurationOption> option in GlobalState.varModel.optionToIndex)
                {
                    sb.Append(predictions[counter] + " " + option.Value.ToString());
                    if (counter < predictions.Length - 1)
                    {
                        sb.Append(" + ");
                    }
                    counter++;
                }
            }
            return sb.ToString();
        }

        private void printNfpPredictionsPython(string pythonList, List<Configuration> predictedConfigurations, PythonPredictionWriter writer)
        {
            string[] separators = new String[] { "," };
            string[] predictions = pythonList.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (predictedConfigurations.Count != predictions.Length)
                GlobalState.logError.log("number of predictions using a python learner does not match with number of configurations");

            for (int i = 0; i < predictedConfigurations.Count; i++)
            {
                writer.writePredictions(predictedConfigurations[i].ToString() + ";" + predictedConfigurations[i].GetNFPValue() + ";" + predictions[i] + "\n");
            }
        }

        /// <summary>
        /// Starts the python process by sending the learner configurations.
        /// Then sends the configurations that are used to train the learner and the configurations that should be used for prediction by
        /// the learner.
        /// At last sends the task that should be performed(learning or parameter tuning).
        /// This has to be performed before requesting results and can only be done once per lifetime of the process.
        /// </summary>
        /// <param name="configs">Configurations used to train.</param>
        /// <param name="configurationsToPredict">Configurations used for prediction.</param>
        /// <param name="task">Task that should be performed by the learner. Can either be parameter tuning
        /// or learning.</param>
        public void setupApplication(List<Configuration> configs, List<Configuration> configurationsToPredict, string task)
        {
            if (AWAITING_SETTINGS.Equals(waitForNextReceivedLine()))
            {
                initializeLearning(this.mlProperties);

                if (AWAITING_CONFIGS.Equals(waitForNextReceivedLine()))
                {
                    passConfigurations(configs);

                    passConfigurationsPredict(configurationsToPredict);
                }
                passLineToApplication(task);
            }
        }

        /// <summary>
        /// Send a request to the process to get the learning/prediction results.
        /// The process has to be set up before performing this method.
        /// The process will automatically terminate after this method was performed.
        /// </summary>
        /// <param name="predictedConfigurations">The configurations that were used to predict the nfp values by the learner.</param>
        /// <param name="writer">Writer to write the prediction results into a csv File.</param>
        public void getLearningResult(List<Configuration> predictedConfigurations, PythonPredictionWriter writer)
        {

            while (!waitForNextReceivedLine().Equals(FINISHED_LEARNING))
            {

            }

            passLineToApplication(REQUESTING_LEARNING_RESULTS);
            printNfpPredictionsPython(waitForNextReceivedLine(), predictedConfigurations, writer);
        }

        /// <summary>
        /// Send a request to the process to get the parameter tuning results(optimal configuration of the learner for the current
        /// scenario).
        /// The process has to be set up before performing this method.
        /// The process will automatically terminate after this method was performed.
        /// </summary>
        /// <param name="predictedConfigurations">The configurations that were used to predict the nfp values by the learner.</param>
        /// <param name="targetPath">Target path to write intermediate results into a file.</param>
        /// <returns>Optimal configuration</returns>
        public string getOptimizationResult(List<Configuration> predictedConfigurations, string targetPath)
        {

            while (!waitForNextReceivedLine().Equals(FINISHED_LEARNING))
            {

            }

            passLineToApplication(targetPath);

            passLineToApplication(REQUESTING_LEARNING_RESULTS);
            return waitForNextReceivedLine();
        }


    }
}
