using System.IO;
using SPLConqueror_Core;
using MachineLearning.Sampling;
using NUnit.Framework;
using System.Collections.Generic;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Hybrid;
using System.Reflection;
using MachineLearning.Sampling.Hybrid.Distributive;

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
        private const int EXPECTED_NEG_FEATURE_WISE = 70;
        private const int EXPECTED_PAIRWISE = 280;
        private const int EXPECTED_OPTIONWISE = 70;
        private const int EXPECTED_BOXBEHNKEN = 200;
        private const int EXPECTED_HYPERSAMPLING_50 = 160;
        private const int EXPECTED_HYPERSAMPLING_40 = 160;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_5 = 200;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_3 = 164;
        private const int EXPECTED_RANDOM_12_1 = 480;
        private const int EXPECTED_RANDOM_10_0 = 400;
        private const int EXPECTED_PLACKETT_BURMAN_3_9 = 240;
        private const int EXPECTED_PLACKETT_BURMAN_5_125 = 328;
        private const int EXPECTED_KEXCHANGE_7_2 = 240;
        private const int EXPECTED_KEXCHANGE_3_1 = 120;
        private const int EXPECTED_T_WISE_3 = 588;
        private const int EXPECTED_T_WISE_2 = 280;
        private const int EXPECTED_BINARY_RANDOM_TW_15 = 7;
        private const int EXPECTED_DIST_AW = 40;
        private const int EXPECTED_DIST_AW_SOLVER = 40;
        private const int EXPECTED_DIST_PRESERVING = 40;

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
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference") 
                + Path.DirectorySeparatorChar + "AllbinarySampling.csv";
            List<Configuration> result = testBinary(SamplingStrategies.ALLBINARY, EXPECTED_CENTRALCOMP_ALLBINARY);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        private bool containsAllMeasurements(List<Configuration> result, List<Configuration> expected)
        {
            bool allExist = true;
            if (result.Count != expected.Count)
                return false;

            foreach (Configuration toFind in expected)
            {
                allExist &= result.Contains(toFind);
            }

            return allExist;
        }

        [Test, Order(3)]
        public void TestNegFeatureWise()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "NegFWSampling.csv";
            List<Configuration> result = testBinary(SamplingStrategies.NEGATIVE_OPTIONWISE, EXPECTED_NEG_FEATURE_WISE);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        private List<Configuration> testBinary(SamplingStrategies strategy, int expected)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(strategy);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(expected, result.Count);
            return result;
        }

        [Test, Order(4)]
        public void TestPairWise()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "PairwiseSampling.csv";
            List<Configuration> result = testBinary(SamplingStrategies.PAIRWISE, EXPECTED_PAIRWISE);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(5)]
        public void TestFeatureWise()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "FeaturewiseSampling.csv";
            List<Configuration> result = testBinary(SamplingStrategies.OPTIONWISE, EXPECTED_OPTIONWISE);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        private List<Configuration> testNumeric(ExperimentalDesign design, int expected)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.PAIRWISE);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(design);
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(expected, result.Count);
            return result;
        }

        [Test, Order(6)]
        public void TestBoxBehnken()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "BoxbehnkenSampling.csv";
            List<Configuration> result = testNumeric(new BoxBehnkenDesign(), EXPECTED_BOXBEHNKEN);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(7)]
        public void TestHypersampling()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "HyperSampling50.csv";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("precision", "50");
            HyperSampling sampling = new HyperSampling();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling, EXPECTED_HYPERSAMPLING_50);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
            parameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "HyperSampling40.csv";
            parameters.Add("precision", "40");
            sampling.setSamplingParameters(parameters);
            result = testNumeric(sampling, EXPECTED_HYPERSAMPLING_40);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(8)]
        public void TestOneFactorAtATime()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "OnefactorSampling5.csv";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("distinctValuesPerOption", "5");
            OneFactorAtATime sampling = new OneFactorAtATime();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling, EXPECTED_ONE_FACTOR_AT_A_TIME_5);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
            parameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "OnefactorSampling3.csv";
            sampling = new OneFactorAtATime();
            parameters.Add("distinctValuesPerOption", "3");
            sampling.setSamplingParameters(parameters);
            result = testNumeric(sampling, EXPECTED_ONE_FACTOR_AT_A_TIME_3);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(9)]
        public void TestNumericRandom()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "RandomSampling112.csv";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", "1");
            parameters.Add("sampleSize", "12");
            RandomSampling sampling = new RandomSampling();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling, EXPECTED_RANDOM_12_1);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
            parameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "RandomSampling010.csv";
            parameters.Add("seed", "0");
            parameters.Add("sampleSize", "10");
            sampling.setSamplingParameters(parameters);
            result = testNumeric(sampling, EXPECTED_RANDOM_10_0);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(10)]
        public void TestPlackettBurman()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "PlackettSampling39.csv";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("level", "3");
            parameters.Add("measurements", "9");
            PlackettBurmanDesign sampling = new PlackettBurmanDesign();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling, EXPECTED_PLACKETT_BURMAN_3_9);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
            parameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "PlackettSampling5125.csv";
            parameters.Add("level", "5");
            parameters.Add("measurements", "125");
            sampling.setSamplingParameters(parameters);
            result = testNumeric(sampling, EXPECTED_PLACKETT_BURMAN_5_125);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(11)]
        public void TestKExchange()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "KexchangeSampling72.csv";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sampleSize", "7");
            parameters.Add("k", "2");
            KExchangeAlgorithm sampling = new KExchangeAlgorithm();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling, EXPECTED_KEXCHANGE_7_2);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
            parameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "KexchangeSampling31.csv";
            sampling = new KExchangeAlgorithm();
            parameters.Add("sampleSize", "3");
            parameters.Add("k", "1");
            sampling.setSamplingParameters(parameters);
            result = testNumeric(sampling, EXPECTED_KEXCHANGE_3_1);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(12)]
        public void TestTWise()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "TwiseSampling3.csv";
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.T_WISE);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("t", "3");
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());

            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.AreEqual(EXPECTED_T_WISE_3, result.Count);
            Assert.True(containsAllMeasurements(result, expected));
            ConfigurationBuilder.binaryParams.tWiseParameters.Clear();
            loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "TwiseSampling2.csv";
            parameters = new Dictionary<string, string>();
            parameters.Add("t", "2");
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            Assert.AreEqual(EXPECTED_T_WISE_2, result.Count);
            expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(13)]
        public void TestBinaryRandom()
        {
            string loc = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar + "BinrandomSampling.csv";
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
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(14)]
        public void TestDistributionAware()
        {
            string locTemplate = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", "0");
            List<Configuration> distAwareBinAndNum = buildSampleSetHybrid(new DistributionAware(), parameters);
            Assert.AreEqual(EXPECTED_DIST_AW, distAwareBinAndNum.Count);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(
                locTemplate + "DistributionAwareCompleteConfigurations.csv", GlobalState.varModel);
            Assert.True(containsAllMeasurements(distAwareBinAndNum, expected));


        }

        private List<Configuration> buildSampleSetHybrid(HybridStrategy design)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return buildSampleSetHybrid(design, parameters);
        }

        private List<Configuration> buildSampleSetHybrid(HybridStrategy design, Dictionary<string, string> parameters)
        {
            List<HybridStrategy> hybridStrategies = new List<HybridStrategy>();
            design.SetSamplingParameters(parameters);
            hybridStrategies.Add(design);
            return ConfigurationBuilder.buildConfigs(model, new List<SamplingStrategies>(),
                new List<ExperimentalDesign>(), hybridStrategies);
        }

        [Test, Order(17)]
        public void TestDistributionAwareSolverSelection()
        {
            // Execute the test
            string locTemplate = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", "0");
            parameters.Add("selection", "SolverSelection");
            parameters.Add("onlyBinary", "True");

            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());

            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            HybridStrategy distAwSolver = new DistributionAware();
            distAwSolver.SetSamplingParameters(parameters);
            hybridStrat.Add(distAwSolver);
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(
                locTemplate + "DistributionAwareSolverSampling.csv", GlobalState.varModel);

            Assert.AreEqual(EXPECTED_DIST_AW_SOLVER, result.Count);
            
            Assert.True(containsAllMeasurements(result, expected));
        }

        [Test, Order(15)]
        public void TestDistributionPreserving()
        {
            string locTemplate = (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", "sampleReference")
                + Path.DirectorySeparatorChar;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", "0");
            List<Configuration> distPreserving = buildSampleSetHybrid(new DistributionPreserving(), parameters);
            Assert.AreEqual(EXPECTED_DIST_PRESERVING, distPreserving.Count);
            List<Configuration> expected = ConfigurationReader.readConfigurations_Header_CSV(
                locTemplate + "DistributionPreserving.csv", GlobalState.varModel);
            Assert.True(containsAllMeasurements(distPreserving, expected));
        }

        [Test, Order(16)]
        public void TestSamplingDomain()
        {
            List<BinaryOption> samplingDomainBinary = new List<BinaryOption>();
            samplingDomainBinary.Add(GlobalState.varModel.getBinaryOption("binOpt1"));
            samplingDomainBinary.Add(GlobalState.varModel.getBinaryOption("binOpt2"));
            samplingDomainBinary.Add(GlobalState.varModel.getBinaryOption("binOpt6"));
            ConfigurationBuilder.optionsToConsider.Add(SamplingStrategies.PAIRWISE, samplingDomainBinary);
            List<NumericOption> samplingDomainNumeric = new List<NumericOption>();
            samplingDomainNumeric.Add(GlobalState.varModel.getNumericOption("numOpt1"));
            ExperimentalDesign exp = new CentralCompositeInscribedDesign();
            exp.setSamplingDomain(samplingDomainNumeric);
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.PAIRWISE);
            List<ExperimentalDesign> exps = new List<ExperimentalDesign>();
            exps.Add(exp);
            List<Configuration> confs = ConfigurationBuilder
                .buildConfigs(GlobalState.varModel, binaryStrat, exps, new List<HybridStrategy>());
            Assert.AreEqual(6, confs.Count);

            // Due to how the space is modeled valid options can only contain root or in sampling 
            // domain specified options
            foreach(Configuration conf in confs)
            {
                foreach(BinaryOption binOpt in conf.BinaryOptions.Keys)
                {
                    if (binOpt.Name != "root")
                    {
                        Assert.True(samplingDomainBinary.Contains(binOpt));
                    }
                }

                foreach (NumericOption numOpt in conf.NumericOptions.Keys)
                {
                    Assert.True(numOpt.Name == "numOpt1");
                }
            }

        }
    }
}
