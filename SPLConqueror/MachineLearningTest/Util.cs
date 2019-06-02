using System.IO;

namespace MachineLearningTest
{
    public class Util
    {
        private Util() { }

        public static void printToTmpFile(string fileName, string content)
        {
            StreamWriter sw = new StreamWriter(Path.GetTempPath() + fileName);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }

        public static string readTmpFile(string fileName)
        {
            StreamReader sr = new StreamReader(Path.GetTempPath() + fileName);
            string result = sr.ReadToEnd();
            sr.Close();
            return result;
        }

        public static void cleanUpTmpFiles(params string[] toDelete)
        {
            foreach (string file in toDelete)
            {
                File.Delete(Path.GetTempPath() + file);
            }
        }
    }
}
