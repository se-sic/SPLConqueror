using NUnit.Framework;
using CommandLine;
using System.IO;
using System;
using System.Collections.Generic;

namespace MachineLearningTest
{
    [TestFixture]
    public class MachineLearningTest
    {

        private Commands cmd;
        private StringWriter consoleOutput = new StringWriter();

        private static string modelPathVS = getApplicationDirectory() + "ExampleFiles" 
            + Path.DirectorySeparatorChar + "BerkeleyDBFeatureModel.xml";

        private static string modelPathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/BerkeleyDBFeatureModel.xml";

        private static string measurementPathVS = getApplicationDirectory() + "ExampleFiles" 
            + Path.DirectorySeparatorChar + "BerkeleyDBMeasurements.xml";

        private static string measurementPathCI = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/BerkeleyDBMeasurements.xml";

        private static string getApplicationDirectory()
        {
            return Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//..")) 
                + Path.DirectorySeparatorChar;
        }
        private bool isCIEnvironment;

        /* Not needed currently
        public static bool IsMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string monoVers()
        {
            Type type = Type.GetType("Mono.Runtime");
            MethodInfo dispalayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
            return dispalayName.Invoke(null, null).ToString();
        }

        public static bool isHigherThanV16()
        {
            string version = monoVers();
            string[] numbers = version.Split(new char[] { '.' });
            return (int.Parse(numbers[0]) > 5) || (int.Parse(numbers[0]) == 5 && int.Parse(numbers[1]) >= 16);
        } */

        [OneTimeSetUp]
        public void init()
        {
            cmd = new Commands();
            Console.SetOut(consoleOutput);
            isCIEnvironment = File.Exists(modelPathCI);
            consoleOutput.Flush();
        }

        private void initModel(Commands cmd)
        {
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
        }

        private void initMeasurements(Commands cmd)
        {
            string command = null;
            if (isCIEnvironment)
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathCI;
            }
            else
            {
                command = Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementPathVS;
            }
            cmd.performOneCommand(command);
        }

        private void performSimpleLearning(Commands cmd)
        {
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " MainMemory");
            cmd.performOneCommand(Commands.COMMAND_BINARY_SAMPLING + " " + Commands.COMMAND_SAMPLE_OPTIONWISE);
            cmd.performOneCommand(Commands.COMMAND_NUMERIC_SAMPLING + " "
                + Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE);
            cmd.performOneCommand(Commands.COMMAND_START_LEARNING_SPL_CONQUEROR);
        }


        [Test, Order(1)]
        public void TestLearning()
        {
            consoleOutput.Flush();
            consoleOutput.NewLine = "\r\n";
            initModel(cmd);
            Equals(consoleOutput.ToString()
                .Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None)[1], "");

            initMeasurements(cmd);
            Console.Error.Write(consoleOutput.ToString());
            bool allConfigurationsLoaded = consoleOutput.ToString()
                .Contains("2560 configurations loaded.");
            Assert.True(allConfigurationsLoaded);

            performSimpleLearning(cmd);
            Console.Error.Write(consoleOutput.ToString());
            string rawLearningRounds = consoleOutput.ToString()
                .Split(new string[] { "Learning progress:" }, StringSplitOptions.None)[1];
            rawLearningRounds = rawLearningRounds.Split(new string[] { "average model" }, StringSplitOptions.None)[0];
            string[] learningRounds = rawLearningRounds
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(isExpectedResult(learningRounds[0].Split(new char[] { ';' })[1]));
        }

        [Test, Order(2)]
        public void testBagging()
        {
            cleanUp(cmd, "");
            cmd.performOneCommand(Commands.COMMAND_SET_MLSETTING + " bagging:true baggingNumbers:3");
            cmd.performOneCommand(Commands.COMMAND_START_LEARNING_SPL_CONQUEROR);

            string averageModel = consoleOutput.ToString()
                .Split(new string[] { "average model:" }, StringSplitOptions.RemoveEmptyEntries)[2];
            string[] polynoms = averageModel.Replace("\n", "").Trim()
                .Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            Console.Error.Write(consoleOutput.ToString());
            Assert.AreEqual(2, polynoms.Length);
            Assert.AreEqual("1585.8 * PAGESIZE", polynoms[0].Trim().Replace(",","."));
            Assert.AreEqual("507.033333333333 * PS32K", polynoms[1].Trim().Replace(",","."));
        }

        private void cleanUp(Commands cmd, String mlSettings)
        {
            cmd.performOneCommand(Commands.COMMAND_CLEAR_LEARNING);
            cmd.performOneCommand(Commands.COMMAND_BINARY_SAMPLING + " " + Commands.COMMAND_SAMPLE_OPTIONWISE);
            cmd.performOneCommand(Commands.COMMAND_NUMERIC_SAMPLING + " "
                + Commands.COMMAND_EXPDESIGN_CENTRALCOMPOSITE);
            cmd.performOneCommand(Commands.COMMAND_SET_MLSETTING + " bagging:false baggingNumbers:3");
        }

        [Test, Order(3)]
        public void testParameterOptimization()
        {
            cleanUp(cmd, " bagging:false baggingNumbers:3");
            cmd.performOneCommand(Commands.COMMAND_OPTIMIZE_PARAMETER_SPLCONQUEROR
                + " lossFunction=[RELATIVE,LEASTSQUARES,ABSOLUTE]");

            Assert.IsTrue(consoleOutput.ToString()
                .Split(new string[] { "Optimal parameters " }, StringSplitOptions.None)[1].Contains("lossFunction:RELATIVE;"));

        }

        [Test, Order(4)]
        public void testCleanGlobal()
        {
            Assert.DoesNotThrow(() =>
            {
                Commands cmd = new Commands();
                performSimpleUseCase(cmd);
                cmd.performOneCommand(Commands.COMMAND_CLEAR_GLOBAL);
                performSimpleUseCase(cmd);
            });
        }

        [Test, Order(5)]
        public void testTrueModel()
        {
            cleanUp(cmd, "");
            string trueModel = "HAVE_HASH\nHAVE_CRYPTO\nPAGESIZE\nPS1K * CS64MB";
            string trueModelFile = "true.model";
            Util.printToTmpFile(trueModelFile, trueModel);

            cmd.performOneCommand(Commands.COMMAND_TRUEMODEL + " " + trueModelFile);
            string result = consoleOutput.ToString().Split(new string[] { "command: truemodel" }, StringSplitOptions.None)[1].Replace(",",".");
            Util.cleanUpTmpFiles(trueModelFile);

            Assert.That(result.Contains("-29.9015624999827 * HAVE_HASH"));
            Assert.That(result.Contains("56.4309375000046 * HAVE_CRYPTO"));
            Assert.That(result.Contains("-70.4853618421029 * PS1K * CS64MB"));
        }

        [Test, Order(6)]
        public void testEvaluateModel()
        {
            cleanUp(cmd, "");
            string trueModel = "100 * HAVE_HASH + -20 * HAVE_CRYPTO + 30 * PAGESIZE + 250 * PS1K * CS32MB + 5 * CS64MB";
            string trueModelPath = "true.model";
            string measurementTrueModel = "measurement_true_model.xml";
            string trueModelPrediction = "prediction_0.csv";
            string measurements = "<results><row>" +
                "<data columname=\"Configuration\" > PAGESIZE, PS1K, HAVE_HASH, HAVE_CRYPTO, CACHESIZE, CS32MB,</data>" +
                "<data columname=\"MainMemory\" > 1620.8 </data>" +
                "</row></results>";

            Util.printToTmpFile(trueModelPath, trueModel);
            Util.printToTmpFile(measurementTrueModel, measurements);

            cmd.performOneCommand(Commands.COMMAND_LOAD_CONFIGURATIONS + " " + measurementTrueModel);
            cmd.performOneCommand(Commands.COMMAND_SET_NFP + " MainMemory");
            consoleOutput.Flush();
            cmd.performOneCommand(Commands.COMMAND_EVALUATE_MODEL + " " + Path.GetTempPath() + trueModelPath + " " 
                + Path.GetTempPath() + "prediction.csv");

            bool predicted360 = Util.readTmpFile(trueModelPrediction).TrimEnd().EndsWith("360");
            Util.cleanUpTmpFiles(measurementTrueModel, trueModelPath, trueModelPrediction);

            Assert.That(predicted360);
        }

        [OneTimeTearDown]
        public void clear()
        {
            consoleOutput.Dispose();
            consoleOutput.Close();
        }

        private void performSimpleUseCase(Commands cmd)
        {
            initModel(cmd);
            initMeasurements(cmd);
            performSimpleLearning(cmd);
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
            isExpected &= variables.Count == 2;
            isExpected &= variables[0].Equals("PAGESIZE");
            isExpected &= variables[1].Equals("PS32K");
            isExpected &= Math.Round(coefficients[0], 2) == 1600.2;
            isExpected &= Math.Round(coefficients[1], 2) == 495.95;
            return isExpected;
        }

    }
}
