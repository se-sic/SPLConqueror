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
            Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//...")) + Path.DirectorySeparatorChar
            + "ExampleFiles" + Path.DirectorySeparatorChar;

            string pathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/";

            string model;
            string measurements;

            if (Directory.Exists(pathCI))
            {
                model = pathCI + "BerkeleyDBFeatureModel.xml";
                measurements = pathCI + "BerkeleyDBMeasurements.xml";
            } else
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

        [Test]
        public void testLearn()
        {
            Commands cmd = setupCommandLine();
            cmd.performOneCommand(Commands.COMMAND_PYTHON_LEARN + " SVR");
        }

        [Test]
        public void testParameterOpt()
        {
            Commands cmd = setupCommandLine();
            cmd.performOneCommand(Commands.COMMAND_PYTHON_LEARN_OPT + " SVR");
        }
    }
}
