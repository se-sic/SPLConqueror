using CommandLine;
using NUnit.Framework;
using System.IO;

namespace SamplingUnitTest
{
    [TestFixture]
    public class CleanSamplingTest
    {
        [Test]
        public void TestCleanSampling()
        {
            string modelPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//..."))
            + Path.DirectorySeparatorChar + "ExampleFiles"
            + Path.DirectorySeparatorChar + "VariabilityModelSampling.xml";
            if (!File.Exists(modelPath))
            {
                modelPath = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/VariabilityModelSampling.xml";
            }
            Commands cmd = new Commands();
            assertNoSamplingStrategies(cmd);
            cmd.performOneCommand(Commands.COMMAND_VARIABILITYMODEL + " " + modelPath);
            cmd.performOneCommand(Commands.COMMAND_SAMPLE_FEATUREWISE);
            cmd.performOneCommand(Commands.COMMAND_SAMPLE_BINARY_TWISE + " " + Commands.COMMAND_VALIDATION);
            cmd.performOneCommand(Commands.COMMAND_EXPERIMENTALDESIGN + " " + Commands.COMMAND_EXPDESIGN_FULLFACTORIAL);
            cmd.performOneCommand(Commands.COMMAND_EXPERIMENTALDESIGN + " " + Commands.COMMAND_EXPDESIGN_BOXBEHNKEN
                + " " + Commands.COMMAND_VALIDATION);
            cmd.performOneCommand(Commands.COMMAND_CLEAR_SAMPLING);
            assertNoSamplingStrategies(cmd); 
        }

        private void assertNoSamplingStrategies(Commands cmd)
        {
            Assert.AreEqual(0, cmd.NumericToSample.Count);
            Assert.AreEqual(0, cmd.NumericToSampleValidation.Count);
            Assert.AreEqual(0, cmd.BinaryToSample.Count);
            Assert.AreEqual(0, cmd.BinaryToSampleValidation.Count);
        }
    }
}
