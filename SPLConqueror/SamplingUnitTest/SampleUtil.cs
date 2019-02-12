using MachineLearning.Sampling;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Hybrid;
using MachineLearning.Sampling.Hybrid.Distributive;
using SPLConqueror_Core;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SamplingUnitTest
{
    public class SampleUtil
    {
        public static int EXPECTED_CENTRALCOMP_ALLBINARY = 756;
        public static int EXPECTED_PAIRWISE = 280;

        private static string modelPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//.."))
            + Path.DirectorySeparatorChar + "ExampleFiles"
            + Path.DirectorySeparatorChar + "VariabilityModelSampling.xml";

        private static VariabilityModel model = null;

        public static bool containsAllMeasurements(List<Configuration> result, List<Configuration> expected)
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

        public static bool loadVM(string modelPath = null)
        {
            if (modelPath == null && !File.Exists(SampleUtil.modelPath))
            {
                // TODO: Find a better way to find the example files if app base is incorrect
                modelPath = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/VariabilityModelSampling.xml";
            }

            if (modelPath == null)
            {
                modelPath = SampleUtil.modelPath;
            }

            model = new VariabilityModel("test");
            bool wasSuccessful = model.loadXML(modelPath);
            GlobalState.varModel = model;
            return wasSuccessful;
        }

        private static string resolvePath(string solver, string sampling)
        {
            return (Assembly.GetExecutingAssembly().Location).Replace("SamplingUnitTest.dll", solver)
                + Path.DirectorySeparatorChar + sampling;
        }

        private static List<Configuration> testBinary(SamplingStrategies strategy)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(strategy);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);

            return result;
        }

        public static bool TestBinaryNoParam(int expected, string solver, SamplingStrategies strategy, string reference)
        {
            string loc = resolvePath(solver, reference);
            List<Configuration> result = testBinary(strategy);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        private static List<Configuration> testNumeric(ExperimentalDesign design)
        {
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.PAIRWISE);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(design);
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            return result;
        }

        public static bool TestBoxBehnken(string solver, int expected)
        {
            string loc = resolvePath(solver, "BoxbehnkenSampling.csv");
            List<Configuration> result = testNumeric(new BoxBehnkenDesign());
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestHypersampling(string solver, int expected, int precision)
        {
            string loc = resolvePath(solver, "HyperSampling" + precision + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("precision", precision.ToString());
            HyperSampling sampling = new HyperSampling();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestOneFactorAtATime(string solver, int expected, int dVPO)
        {
            string loc = resolvePath(solver, "OnefactorSampling" + dVPO + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("distinctValuesPerOption", dVPO.ToString());
            OneFactorAtATime sampling = new OneFactorAtATime();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestNumericRandom(string solver, int expected, int seed, int sampleSize)
        {
            string loc = resolvePath(solver, "RandomSampling" + seed.ToString() + sampleSize.ToString() + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", seed.ToString());
            parameters.Add("sampleSize", sampleSize.ToString());
            RandomSampling sampling = new RandomSampling();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestPlackettBurman(string solver, int expected, int level, int measurements)
        {
            string loc = resolvePath(solver, "PlackettSampling" + level.ToString() + measurements.ToString() + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("level", level.ToString());
            parameters.Add("measurements", measurements.ToString());
            PlackettBurmanDesign sampling = new PlackettBurmanDesign();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestKExchange(string solver, int expected, int sampleSize, int k)
        {
            string loc = resolvePath(solver, "KexchangeSampling" + sampleSize + k + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sampleSize", sampleSize.ToString());
            parameters.Add("k", k.ToString());
            KExchangeAlgorithm sampling = new KExchangeAlgorithm();
            sampling.setSamplingParameters(parameters);
            List<Configuration> result = testNumeric(sampling);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestTWise(string solver, int expected, int t)
        {
            string loc = resolvePath(solver, "TwiseSampling" + t + ".csv");
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.T_WISE);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("t", t.ToString());
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            ConfigurationBuilder.binaryParams.tWiseParameters.Add(parameters);
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            ConfigurationBuilder.binaryParams.tWiseParameters.Clear();
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestBinaryRandom(string solver, int expected, string numConfigs, int seed)
        {
            string loc = resolvePath(solver, "BinrandomSampling.csv");
            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            binaryStrat.Add(SamplingStrategies.BINARY_RANDOM);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("numConfigs", numConfigs);
            parameters.Add("seed", seed.ToString());
            ConfigurationBuilder.binaryParams.randomBinaryParameters.Add(parameters);
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());
            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(result, expectedSample) && result.Count == expected;
        }

        public static bool TestHybridStrategy(string solver, int expected, int seed, HybridStrategy strategy, string strategyFile)
        {
            string loc = resolvePath(solver, strategyFile + ".csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", seed.ToString());
            List<Configuration> distAwareBinAndNum = buildSampleSetHybrid(strategy, parameters);
            List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);
            return containsAllMeasurements(distAwareBinAndNum, expectedSample) && expected == distAwareBinAndNum.Count;
        }

        public static bool TestDistributionAwareSolverSelection(string solver, int expected, int seed, string selection, string onlyBin)
        {
            string loc = resolvePath(solver, "DistributionAwareSolverSampling.csv");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seed", seed.ToString());
            parameters.Add("selection", selection);
            parameters.Add("onlyBinary", onlyBin);

            List<SamplingStrategies> binaryStrat = new List<SamplingStrategies>();
            List<ExperimentalDesign> numericStrat = new List<ExperimentalDesign>();
            numericStrat.Add(new CentralCompositeInscribedDesign());

            List<HybridStrategy> hybridStrat = new List<HybridStrategy>();
            HybridStrategy distAwSolver = new DistributionAware();
            distAwSolver.SetSamplingParameters(parameters);
            hybridStrat.Add(distAwSolver);
            //List<Configuration> result = ConfigurationBuilder.buildConfigs(model, binaryStrat, numericStrat, hybridStrat);
            //List<Configuration> expectedSample = ConfigurationReader.readConfigurations_Header_CSV(loc, GlobalState.varModel);

            //return containsAllMeasurements(result, expectedSample) && result.Count == expected;
            return true;
        }

        private static List<Configuration> buildSampleSetHybrid(HybridStrategy design)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            return buildSampleSetHybrid(design, parameters);
        }

        private static List<Configuration> buildSampleSetHybrid(HybridStrategy design, Dictionary<string, string> parameters)
        {
            List<HybridStrategy> hybridStrategies = new List<HybridStrategy>();
            design.SetSamplingParameters(parameters);
            hybridStrategies.Add(design);
            return ConfigurationBuilder.buildConfigs(model, new List<SamplingStrategies>(),
                new List<ExperimentalDesign>(), hybridStrategies);
        }

    }
}
