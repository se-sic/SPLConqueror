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

        protected const string SCRIPT_LOCATION = "\\PyML\\pyScripts";

        public const string COMMUNICATION_SCRIPT = "Communication.py";

        public static string PYTHON_PATH = "python.exe";

        private const string CONFIG_STREAM_START = "config_start";

        private const string CONFIG_STREAM_END = "config_end";

        private const string LEARNING_SETTINGS_STREAM_START = "settings_start";

        private const string LEARNING_SETTINGS_STREAM_END = "settings_end";

        private const string AWAITING_SETTINGS = "req_settings";

        private const string AWAITING_CONFIGS = "req_configs";

        private const string REQUESTING_LEARNING_RESULTS = "req_results";

        private const string PASS_OK = "pass_ok";

        private const string FINISHED_LEARNING = "learn_finished";

        /// <summary>
        /// Create a new wrapper that contains a running Python Process.
        /// </summary>
        /// <param name="path">The path of the source file called to start Python.</param>
        /// <param name="mlProperties">Options for the machine learning algortihm.</param>
        public PythonWrapper(string path, string[] mlProperties)
        {
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
            passLineToApplication(CONFIG_STREAM_START);

            foreach (Configuration config in toPass)
            {
                passLineToApplication(config.toNumeric());
                while (!waitForNextReceivedLine().Equals(PASS_OK))
                {
                    // Wait to make sure the previous input is processed in order to make sure receiver wont be overwhelmed
                }
            }

                passLineToApplication(CONFIG_STREAM_END);
        }

        private void initializeLearning(LearningSettings.LearningStrategies strategy, LearningSettings.LearningKernel kernelSettings)
        {
            passLineToApplication(LEARNING_SETTINGS_STREAM_START);
            passLineToApplication(Enum.GetName(typeof(LearningSettings.LearningStrategies), (int)strategy));
            passLineToApplication(Enum.GetName(typeof(LearningSettings.LearningKernel), (int)kernelSettings));
            passLineToApplication(LEARNING_SETTINGS_STREAM_END);
        }

        private string formatPyResults(string pythonList)
        {
            string withoutBracket = pythonList.Substring(1, pythonList.Length - 2);
            string[] separators = new String[] { "," };
            string[] optionPrefix = withoutBracket.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            if (optionPrefix.Length == GlobalState.varModel.optionToIndex.Count)
            {
                int counter = 0;
                foreach (KeyValuePair<int, ConfigurationOption> option in GlobalState.varModel.optionToIndex)
                {
                    sb.Append(optionPrefix[counter] + " " + option.Value.ToString());
                    if (counter < optionPrefix.Length - 1)
                    {
                        sb.Append(" + ");
                    }
                    counter++;
                }
            }
            return sb.ToString();
        }

        public void setupApplication(List<Configuration> configs, LearningSettings.LearningStrategies strategy, LearningSettings.LearningKernel kernelSettings)
        {
            if (AWAITING_SETTINGS.Equals(waitForNextReceivedLine()))
            {
                initializeLearning(strategy, kernelSettings);

                if (AWAITING_CONFIGS.Equals(waitForNextReceivedLine()))
                {
                    passConfigurations(configs);
                }
            }
        }

        public void setupDefaultApplication(List<Configuration> configs, LearningSettings.LearningStrategies strategy)
        {
            setupApplication(configs, strategy, LearningSettings.LearningKernel.standard);
        }

        public string getLearningResult()
        {

            while (!waitForNextReceivedLine().Equals(FINISHED_LEARNING))
            {
                
            }

            passLineToApplication(REQUESTING_LEARNING_RESULTS);
            return formatPyResults(waitForNextReceivedLine());
        }

    }
}
