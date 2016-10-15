using System.IO;
using System.Linq;
using System.Text;

namespace ProcessWrapper
{
    public class PythonPredictionWriter
    {

        private StreamWriter csvWriter = null;

        private string learningSettingsAttachment = null;

        public const string csvFilename = "PredictedValues.csv";

        private const string csvSeparator = ";";

        public PythonPredictionWriter(string path, string[] learningSettings)
        {
            string logFilename = (path.Split(Path.DirectorySeparatorChar)).Last();
            path = path.Substring(0, (path.Length - ((logFilename).Length)));
            path += csvFilename;
            FileStream csvFileStream = new FileStream(path, FileMode.Append, FileAccess.Write);
            csvWriter = new StreamWriter(csvFileStream);
            learningSettingsAttachment = parseLearningSettings(learningSettings);
        }

        public void writePredictions(string toWrite)
        {
            csvWriter.Write(learningSettingsAttachment + toWrite);
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

        public string getPath()
        {
            return ((FileStream)csvWriter.BaseStream).Name;
        }

    }
}
