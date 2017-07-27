using System;
using System.IO;
using SPLConqueror_Core;
using MachineLearning.Sampling;
using NUnit.Framework;
using System.Collections.Generic;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Hybrid;

namespace SamplingUnitTest
{
    [TestFixture]
    public class SamplingTest
    {

        private static string modelPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory , "..//..//..."))
            + Path.DirectorySeparatorChar + "ExampleFiles"
            + Path.DirectorySeparatorChar + "VariabilityModelSampling.xml";

        private static VariabilityModel model = new VariabilityModel("test");

        private const int EXPECTED_CENTRALCOMP_ALLBINARY = 756;
        private const int EXPECTED_NEG_FEATURE_WISE = 63;
        private const int EXPECTED_PAIRWISE = 287;
        private const int EXPECTED_OPTIONWISE = 70;
        private const int EXPECTED_BOXBEHNKEN = 205;
        private const int EXPECTED_HYPERSAMPLING_50 = 164;
        private const int EXPECTED_HYPERSAMPLING_40 = 164;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_5 = 205;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_3 = 164;
        private const int EXPECTED_RANDOM_12_1 = 492;
        private const int EXPECTED_RANDOM_10_0 = 410;
        private const int EXPECTED_PLACKETT_BURMAN_3_9 = 246;
        private const int EXPECTED_PLACKETT_BURMAN_5_125 = 328;
        private const int EXPECTED_KEXCHANGE_7_2 = 246;
        private const int EXPECTED_KEXCHANGE_3_1 = 123;
        private const int EXPECTED_T_WISE_3 = 602;
        private const int EXPECTED_T_WISE_2 = 287;
        private const int EXPECTED_BINARY_RANDOM_TW_15 = 7;

        [Test, Order(1)]
        public void TestLoadingTestVM()
        {
            if (!File.Exists(modelPath))
            {
                // TODO: Find a better way to find the example files if app base is incorrect
                modelPath = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/VariabilityModelSampling.xml";
            }
            Assert.IsTrue(model.loadXML(modelPath));
            GlobalState.varModel = model;
        }

        [Test, Order(2)]
        public void TestWholePop()
        {
            testBinary(SamplingStrategies.ALLBINARY, EXPECTED_CENTRALCOMP_ALLBINARY);
        }

        [Test, Order(3)]
        public void TestNegFeatureWise()
        {
            testBinary(SamplingStrategies.NEGATIVE_OPTIONWISE, EXPECTED_NEG_FEATURE_WISE);
        }

        private void testBinary(SamplingStrategies strategy, int expected)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(strategy);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(expected, result.Count);
        }

        [Test, Order(4)]
        public void TestPairWise()
        {
            testBinary(SamplingStrategies.PAIRWISE, EXPECTED_PAIRWISE);
        }

        [Test, Order(5)]
        public void TestFeatureWise()
        {
            testBinary(SamplingStrategies.OPTIONWISE, EXPECTED_OPTIONWISE);
        }

        private void testNumeric(ExperimentalDesign design, int expected)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.PAIRWISE);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(design);
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(expected, result.Count);
        }

        [Test, Order(6)]
        public void TestBoxBehnken()
        {
            testNumeric(new BoxBehnkenDesign(), EXPECTED_BOXBEHNKEN);
        }

        [Test, Order(7)]
        public void TestHypersampling()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("precision", "50");
            HyperSampling sampling = new HyperSampling();
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_HYPERSAMPLING_50);
            parameters.Clear();
            parameters.Add("precision", "40");
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_HYPERSAMPLING_40);
        }

        [Test, Order(8)]
        public void TestOneFactorAtATime()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("distinctValuesPerOption", "5");
            OneFactorAtATime sampling = new OneFactorAtATime();
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_ONE_FACTOR_AT_A_TIME_5);
            parameters.Clear();
            sampling = new OneFactorAtATime();
            parameters.Add("distinctValuesPerOption", "3");
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_ONE_FACTOR_AT_A_TIME_3);
        }

        [Test, Order(9)]
        public void TestNumericRandom()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", "1");
            parameters.Add("sampleSize", "12");
            RandomSampling sampling = new RandomSampling();
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_RANDOM_12_1);
            parameters.Clear();
            parameters.Add("seed", "0");
            parameters.Add("sampleSize", "10");
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_RANDOM_10_0);
        }

        [Test, Order(10)]
        public void TestPlackettBurman()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("level", "3");
            parameters.Add("measurements", "9");
            PlackettBurmanDesign sampling = new PlackettBurmanDesign();
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_PLACKETT_BURMAN_3_9);
            parameters.Clear();
            parameters.Add("level", "5");
            parameters.Add("measurements", "125");
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_PLACKETT_BURMAN_5_125);
        }

        [Test, Order(11)]
        public void TestKExchange()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sampleSize", "7");
            parameters.Add("k", "2");
            KExchangeAlgorithm sampling = new KExchangeAlgorithm();
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_KEXCHANGE_7_2);
            parameters.Clear();
            sampling = new KExchangeAlgorithm();
            parameters.Add("sampleSize", "3");
            parameters.Add("k", "1");
            sampling.setSamplingParameters(parameters);
            testNumeric(sampling, EXPECTED_KEXCHANGE_3_1);
        }

        [Test, Order(12)]
        public void TestTWise()
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.T_WISE);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("t", "3");
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(EXPECTED_T_WISE_3, result.Count);
            ConfigurationBuilder.binaryParams.tWiseParameters.Clear();
            parameters = new Dictionary<string, string>();
            parameters.Add("t", "2");
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(EXPECTED_T_WISE_2, result.Count);
        }

        [Test, Order(13)]
        public void TestBinaryRandom()
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.BINARY_RANDOM);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("numConfigs", "asTW");
            parameters.Add("seed", "15");
            ConfigurationBuilder.binaryParams.randomBinaryParameters.Add(parameters);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(EXPECTED_BINARY_RANDOM_TW_15, result.Count);
        }
    }
}
