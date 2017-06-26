using NUnit.Framework;
using CommandLine;
using System.IO;
using System;
using System.Text;

namespace MachineLearningTest
{
    [TestFixture]
    public class MachineLearningTest
    {

        private Commands cmd;
        private StringWriter consoleOutput = new StringWriter();

        private static string modelPathVS = Path.GetFullPath(
            Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//...")) + Path.DirectorySeparatorChar
            + "ExampleFiles" + Path.DirectorySeparatorChar + "BerkeleyDBFeatureModel.xml";

        private static string modelPathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/BerkeleyDBFeatureModel.xml";

        private static string measurementPathVS = Path.GetFullPath(
            Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//...")) + Path.DirectorySeparatorChar
            + "ExampleFiles" + Path.DirectorySeparatorChar + "BerkeleyDBMeasurements.xml";

        private static string measurementPathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/BerkeleyDBMeasurements.xml";

        private bool isCIEnvironment;

        [OneTimeSetUp]
        public void init()
        {
            cmd = new Commands();
            Console.SetOut(consoleOutput);
            isCIEnvironment = File.Exists(modelPathCI);
            consoleOutput.Flush();
        }


        [Test, Order(1)]
        public void TestLoadVM()
        {
            consoleOutput.Flush();
            string command = null;
            if (isCIEnvironment)
            {
                command = Commands.COMMAND_VARIABILITYMODEL + " " + modelPathCI;
            }
            else
            {
                command = Commands.COMMAND_VARIABILITYMODEL + " " + modelPathVS;
            }
            cmd.performOneCommand(command);
            Equals(consoleOutput.ToString()
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[1], "");
            command = null;
            if (isCIEnvironment)
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathCI;
            } else
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathVS;
            }
            cmd.performOneCommand(command);
            bool allConfigurationsLoaded = consoleOutput.ToString().Contains("2560 configurations loaded.");
            Assert.True(allConfigurationsLoaded);
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " MainMemory");
            cmd.performOneCommand(Commands.COMMAND_SAMPLE_FEATUREWISE);
            cmd.performOneCommand(Commands.COMMAND_EXERIMENTALDESIGN + " " + Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE);
            cmd.performOneCommand(Commands.COMMAND_START_LEARNING);
            string[] learningRounds = consoleOutput.ToString().Split(new string[] { "Learning progress:" }, StringSplitOptions.None)[1]
                .Split(new string[] { "average model" }, StringSplitOptions.None)[0].Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
