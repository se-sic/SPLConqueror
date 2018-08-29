using System.IO;
using System.Linq;
using System.Text;

namespace ProcessWrapper
{
    public class PythonPredictionWriter
    {

        private StreamWriter csvWriter = null;

        public const string csvFilename = "PreVal";
        public const string csv = ".csv";

        private const string csvSeparator = ";";

        /// <summary>
        /// Class to write prediction results into a csv file.
        /// </summary>
        /// <param name="path">The path where the file will be written.</param>
        /// <param name="learningSettings">The learner configurations that were used for prediction.</param>
        /// <param name="identifyer">Identifier of the current experiment. This should consist of the sampling strategies used and the name of the case study.</param>
        public PythonPredictionWriter(string path, string[] learningSettings, string identifier)
        {
            string logFilename = (path.Split(Path.DirectorySeparatorChar)).Last();
            path = path.Substring(0, (path.Length - ((logFilename).Length)));
            path += csvFilename + "_";
            path += learningSettings[0] + "_";
            path += identifier.Replace(":", "_");
            if (path.Length > 250)
            {
                path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                System.Random rand = new System.Random();
                path += "predict" + rand.Next(99);
                SPLConqueror_Core.GlobalState.logInfo.logLine("File name for predictions file was too long. Changed to" + path);
            }
            path += csv;




            FileStream csvFileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
            csvWriter = new StreamWriter(csvFileStream);
        }

        /// <summary>
        /// Attach the learner settings to the line and write it into the file.
        /// </summary>
        /// <param name="toWrite">The string that should be written.</param>
        public void writePredictions(string toWrite)
        {
            csvWriter.Write(toWrite);
            csvWriter.Flush();
        }

        public void close()
        {
            csvWriter.Close();
        }

        private string parseLearningSettings(string[] learningSettings)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string setting in learningSettings)
            {
                sb.Append(setting + "_");
            }
            // remove unneeded underscore
            sb.Length--;
            sb.Append(csvSeparator);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the path of the target file.
        /// </summary>
        /// <returns>Path as string.</returns>
        public string getPath()
        {
            return ((FileStream)csvWriter.BaseStream).Name;
        }

    }
}
