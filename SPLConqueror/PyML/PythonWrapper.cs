using System.Diagnostics;
using SPLConqueror_Core;
using System.Collections.Generic;
using System.Text;

namespace ProcessWrapper
{
    public class PythonWrapper
    {
        private Process pythonProcess;

        private const string PYTHON_APP = "Python.exe";

        private const string CONFIG_SEPERATOR = "$";

        /// <summary>
        /// Create a new wrapper that contains a running Python Process.
        /// </summary>
        /// <param name="args">Command line arguments passed to the process. First argument has to be the the path of the source file</param>
        public PythonWrapper(params string[] args)
        {
            string argumentLine = "";
            foreach (string statement in args)
            {
                argumentLine = argumentLine + statement + " ";
            }
            ProcessStartInfo pythonSetup = new ProcessStartInfo(PYTHON_APP, argumentLine);
            pythonSetup.UseShellExecute = false;
            pythonSetup.RedirectStandardInput = true;
            pythonSetup.RedirectStandardOutput = true;
            pythonSetup.RedirectStandardError = true;
            pythonProcess = Process.Start(pythonSetup);
        }

        public string waitForNextReceivedLine()
        {
            string processOutput = "";
            while (processOutput == null || processOutput.Length == 0)
            {
                processOutput = pythonProcess.StandardOutput.ReadLine();
            }
            return processOutput;
        }

        public void passLineToApplication(string toPass)
        {
            pythonProcess.StandardInput.WriteLine(toPass);
        }

        public void endProcess()
        {
            pythonProcess.Close();
        }

        public static string formatConfigurationsToString(List<Configuration> toParse)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Configurations" + CONFIG_SEPERATOR);
            foreach (Configuration config in toParse)
            {
                sb.Append(config.ToString() + "&;&" + config.GetNFPValue().ToString("R") + CONFIG_SEPERATOR);
            }
            return sb.ToString();
        }
    }
}
