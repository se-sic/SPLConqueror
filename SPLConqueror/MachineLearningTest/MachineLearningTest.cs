using NUnit.Framework;
using CommandLine;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

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
            consoleOutput.NewLine = "\r\n";
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
                .Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None)[1], "");
            command = null;
            if (isCIEnvironment)
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathCI;
            } else
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathVS;
            }
            cmd.performOneCommand(command);
            Console.Error.Write(consoleOutput.ToString());
            bool allConfigurationsLoaded = consoleOutput.ToString().Contains("2560 configurations loaded.");
            Assert.True(allConfigurationsLoaded);
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " MainMemory");
            cmd.performOneCommand(Commands.COMMAND_SAMPLE_FEATUREWISE);
            cmd.performOneCommand(Commands.COMMAND_EXERIMENTALDESIGN + " " + Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE);
            cmd.performOneCommand(Commands.COMMAND_START_LEARNING);
            Console.Error.Write(consoleOutput.ToString());
            string rawLearningRounds = consoleOutput.ToString().Split(new string[] { "Learning progress:" }, StringSplitOptions.None)[1];
            Console.Error.Write(1);
            rawLearningRounds = rawLearningRounds.Split(new string[] { "average model" }, StringSplitOptions.None)[0];
            Console.Error.Write(2);
            string[] learningRounds = rawLearningRounds.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Console.Error.Write(3);
            Assert.True(isExpectedResult(learningRounds[learningRounds.Length - 2].Split(new char[] { ';' })[1]));
        }

        private bool isExpectedResult(string learningResult)
        {
            Console.Error.Write(learningResult);
            bool isExpected = true;
            string[] polynoms = learningResult.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> variables = new List<string>();
            List<double> coefficients = new List<double>();
            foreach (string polynom in polynoms)
            {
                string[] coefficientAndVariable = polynom.Split(new char[] { '*' }, 2);
                variables.Add(coefficientAndVariable[1].Trim());
                coefficients.Add(Double.Parse(coefficientAndVariable[0].Trim()));
            }
            isExpected &= variables.Count == 5;
            isExpected &= variables[0].Equals("PAGESIZE");
            isExpected &= variables[1].Equals("CS16MB");
            isExpected &= variables[2].Equals("PS8K");
            isExpected &= variables[3].Equals("PS16K");
            isExpected &= variables[4].Equals("PS32K");
            isExpected &= Math.Round(coefficients[0], 2) == 16328.73;
            isExpected &= Math.Round(coefficients[1], 2) == -112.73;
            isExpected &= Math.Round(coefficients[2], 2) == -14746.73;
            isExpected &= Math.Round(coefficients[3], 2) == -14742.73;
            isExpected &= Math.Round(coefficients[4], 2) == 4455.27;
            return isExpected;
        }
    }
}
