using NUnit.Framework;
using CommandLine;
using System.IO;

namespace PythonTest
{
    [TestFixture]
    public class PythonLearnTest
    {
        private Commands setupCommandLine()
        {
            Commands cmd = new Commands();
            string pathVS = Path.GetFullPath(
            Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//..")) + Path.DirectorySeparatorChar
            + "ExampleFiles" + Path.DirectorySeparatorChar;

            string pathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/";

            string model;
            string measurements;

            if (Directory.Exists(pathCI))
            {
                model = pathCI + "BerkeleyDBFeatureModel.xml";
                measurements = pathCI + "BerkeleyDBMeasurements.xml";
            }
            else
            {
                model = pathVS + "BerkeleyDBFeatureModel.xml";
                measurements = pathVS + "BerkeleyDBMeasurements.xml";
            }

            cmd.performOneCommand(Commands.COMMAND_LOG + " " + Path.GetTempPath() + "test.log");
            cmd.performOneCommand(Commands.DEFINE_PYTHON_PATH + " /home/travis/pyenv/bin/");
            cmd.performOneCommand(Commands.COMMAND_VARIABILITYMODEL + " " + model);
            cmd.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurements);
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " MainMemory");
            cmd.performOneCommand(Commands.COMMAND_SELECT_ALL_MEASUREMENTS + " true");
            return cmd;
        }

        private int getNumberPredictions()
        {
            int numberPredictions = -1;
            StreamReader sr = new StreamReader(Path.GetTempPath() + "PreVal_SVR_BerkeleyDbC__.csv");
            while (sr.ReadLine() != "" && !sr.EndOfStream)
            {
                numberPredictions += 1;
            }
            return numberPredictions;
        }

        [Test, Order(1)]
        public void testLearn()
        {
            Commands cmd = setupCommandLine();
            cmd.performOneCommand(Commands.COMMAND_PYTHON_LEARN + " SVR");
            Assert.True(File.Exists(Path.GetTempPath() + "PreVal_SVR_BerkeleyDbC__.csv"));
            Assert.AreEqual(2559, getNumberPredictions());
        }

        [Test, Order(2)]
        public void testParameterOpt()
        {
            Commands cmd = setupCommandLine();
            cmd.performOneCommand(Commands.COMMAND_PYTHON_LEARN_OPT + " SVR");
            cmd = null;
        }
    }
}
