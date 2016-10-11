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

        private const string CONFIG_LEARN_STREAM_START = "config_learn_start";

        private const string CONFIG_LEARN_STREAM_END = "config_learn_end";


        private const string CONFIG_PREDICT_STREAM_START = "config_predict_start";

        private const string CONFIG_PREDICT_STREAM_END = "config_predict_end";


        private const string LEARNING_SETTINGS_STREAM_START = "settings_start";

        private const string LEARNING_SETTINGS_STREAM_END = "settings_end";

        private const string AWAITING_SETTINGS = "req_settings";

        private const string AWAITING_CONFIGS = "req_configs";

        private const string REQUESTING_LEARNING_RESULTS = "req_results";

        private const string PASS_OK = "pass_ok";

        private const string FINISHED_LEARNING = "learn_finished";

        public const string START_LEARN = "start_learn";

        public const string START_PARAM_TUNING = "start_param_tuning";

        private string[] mlProperties = null;

        /// <summary>
        /// Create a new wrapper that contains a running Python Process.
        /// </summary>
        /// <param name="path">The path of the source file called to start Python.</param>
        /// <param name="mlProperties">Options for the machine learning algortihm.</param>
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

        private void initializeLearning(LearningSettings.LearningStrategies strategy, string[] mlSettings)
        {
            passLineToApplication(LEARNING_SETTINGS_STREAM_START);
            passLineToApplication((Enum.GetName(typeof(LearningSettings.LearningStrategies), (int)strategy)).ToLower());
            if (mlSettings != null)
            {
                foreach (string setting in mlSettings)
                {
                    passLineToApplication(setting);
                }
            }
            passLineToApplication(LEARNING_SETTINGS_STREAM_END);
        }

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

        public void setupApplication(List<Configuration> configs, LearningSettings.LearningStrategies strategy, List<Configuration> configurationsToPredict, string task)
        {
            if (AWAITING_SETTINGS.Equals(waitForNextReceivedLine()))
            {
                initializeLearning(strategy, this.mlProperties);

                if (AWAITING_CONFIGS.Equals(waitForNextReceivedLine()))
                {
                    passConfigurations(configs);

                    passConfigurationsPredict(configurationsToPredict);
                }
                passLineToApplication(task);
            }
        }

        public string getLearningResult(List<Configuration> predictedConfigurations)
        {

            while (!waitForNextReceivedLine().Equals(FINISHED_LEARNING))
            {

            }

            passLineToApplication(REQUESTING_LEARNING_RESULTS);
            return nfpPredictionsPython(waitForNextReceivedLine(), predictedConfigurations);
        }

        public string getOptimizationResult(List<Configuration> predictedConfigurations)
        {

            while (!waitForNextReceivedLine().Equals(FINISHED_LEARNING))
            {

            }

            passLineToApplication(REQUESTING_LEARNING_RESULTS);
            return waitForNextReceivedLine();
        }


    }
}
