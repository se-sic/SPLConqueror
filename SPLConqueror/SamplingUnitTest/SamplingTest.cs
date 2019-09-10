using SPLConqueror_Core;
using MachineLearning.Sampling;
using NUnit.Framework;
using System.Collections.Generic;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Hybrid;
using MachineLearning.Sampling.Hybrid.Distributive;

namespace SamplingUnitTest
{
    [TestFixture]
    public class SamplingTest
    {
        private const int EXPECTED_NEG_FEATURE_WISE = 70;
        private const int EXPECTED_OPTIONWISE = 70;
        private const int EXPECTED_BOXBEHNKEN = 200;
        private const int EXPECTED_HYPERSAMPLING_50 = 160;
        private const int EXPECTED_HYPERSAMPLING_40 = 160;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_5 = 200;
        private const int EXPECTED_ONE_FACTOR_AT_A_TIME_3 = 160;
        private const int EXPECTED_RANDOM_12_1 = 480;
        private const int EXPECTED_RANDOM_10_0 = 400;
        private const int EXPECTED_PLACKETT_BURMAN_3_9 = 240;
        private const int EXPECTED_PLACKETT_BURMAN_5_125 = 320;
        private const int EXPECTED_KEXCHANGE_7_2 = 240;
        private const int EXPECTED_KEXCHANGE_3_1 = 120;
        private const int EXPECTED_T_WISE_3 = 588;
        private const int EXPECTED_T_WISE_2 = 280;
        private const int EXPECTED_BINARY_RANDOM_AS_TW_15 = 280;
        private const int EXPECTED_DIST_AW = 40;
        private const int EXPECTED_DIST_AW_SOLVER = 280;
        private const int EXPECTED_DIST_PRESERVING = 40;

        [Test, Order(1)]
        public void TestLoadingTestVM()
        {
            Assert.IsTrue(SampleUtil.loadVM());
        }

        [Test, Order(2)]
        public void TestAllbinaryCCD()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(SampleUtil.EXPECTED_CENTRALCOMP_ALLBINARY, "msSolver"
                , SamplingStrategies.ALLBINARY, "AllbinarySampling.csv"));
        }

        [Test, Order(3)]
        public void TestNegFeatureWiseCCD()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(EXPECTED_NEG_FEATURE_WISE, "msSolver"
                , SamplingStrategies.NEGATIVE_OPTIONWISE, "NegFWSampling.csv"));
        }

        [Test, Order(4)]
        public void TestPairWiseCCD()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(SampleUtil.EXPECTED_PAIRWISE, "msSolver"
                , SamplingStrategies.PAIRWISE, "PairwiseSampling.csv"));
        }

        [Test, Order(5)]
        public void TestFeatureWiseCCD()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(EXPECTED_OPTIONWISE, "msSolver"
                , SamplingStrategies.OPTIONWISE, "FeaturewiseSampling.csv"));
        }

        [Test, Order(6)]
        public void TestBoxBehnkenPairwise()
        {
            Assert.True(SampleUtil.TestBoxBehnken("msSolver", EXPECTED_BOXBEHNKEN));
        }

        [Test, Order(7)]
        public void TestHypersamplingPairwise()
        {
            Assert.True(SampleUtil.TestHypersampling("msSolver", EXPECTED_HYPERSAMPLING_50, 50));
            Assert.True(SampleUtil.TestHypersampling("msSolver", EXPECTED_HYPERSAMPLING_40, 40));
        }

        [Test, Order(8)]
        public void TestOneFactorAtATimePairwise()
        {
            Assert.True(SampleUtil.TestOneFactorAtATime("msSolver", EXPECTED_ONE_FACTOR_AT_A_TIME_5, 5));
            Assert.True(SampleUtil.TestOneFactorAtATime("msSolver", EXPECTED_ONE_FACTOR_AT_A_TIME_3, 3));
        }

        [Test, Order(9)]
        public void TestNumericRandomPairwise()
        {
            Assert.True(SampleUtil.TestNumericRandom("msSolver", EXPECTED_RANDOM_12_1, 1, 12));
            Assert.True(SampleUtil.TestNumericRandom("msSolver", EXPECTED_RANDOM_10_0, 0, 10));
        }

        [Test, Order(10)]
        public void TestPlackettBurmanPairwise()
        {
            Assert.True(SampleUtil.TestPlackettBurman("msSolver", EXPECTED_PLACKETT_BURMAN_3_9, 3, 9));
            Assert.True(SampleUtil.TestPlackettBurman("msSolver", EXPECTED_PLACKETT_BURMAN_5_125, 5, 125));
        }

        [Test, Order(11)]
        public void TestKExchangePairwisePairwise()
        {
            Assert.True(SampleUtil.TestKExchange("msSolver", EXPECTED_KEXCHANGE_7_2, 7, 2));
            Assert.True(SampleUtil.TestKExchange("msSolver", EXPECTED_KEXCHANGE_3_1, 3, 1));
        }

        [Test, Order(12)]
        public void TestTWiseCCD()
        {
            Assert.True(SampleUtil.TestTWise("msSolver", EXPECTED_T_WISE_3, 3));
            Assert.True(SampleUtil.TestTWise("msSolver", EXPECTED_T_WISE_2, 2));
        }

        [Test, Order(13)]
        public void TestBinaryRandomCCD()
        {
            // Test binary random with the same number of configurations as Twise 2
            Assert.True(SampleUtil.TestBinaryRandom("msSolver", EXPECTED_BINARY_RANDOM_AS_TW_15, "asTW2", 15));
        }

        [Test, Order(14)]
        public void TestDistributionAware()
        {
            Assert.True(SampleUtil.TestHybridStrategy("msSolver", EXPECTED_DIST_AW, 0
                , new DistributionAware(), "DistributionAwareCompleteConfigurations"));
        }

        [Test, Order(15)]
        public void TestDistributionPreserving()
        {
            Assert.True(SampleUtil.TestHybridStrategy("msSolver", EXPECTED_DIST_PRESERVING, 0
                , new DistributionPreserving(), "DistributionPreserving"));
        }

        [Test, Order(17)]
        public void TestDistributionAwareSolverSelection()
        {
            Assert.True(SampleUtil.TestDistributionAwareSolverSelection("msSolver", EXPECTED_DIST_AW_SOLVER
                , 0, "SolverSelection", "True"));
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
            foreach (Configuration conf in confs)
            {
                foreach (BinaryOption binOpt in conf.BinaryOptions.Keys)
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

        [Test, Order(18)]
        public void TestSamplingOptionalNumeric()
        {
            Assert.That(SampleUtil.testOptionalNumSample());
        }
    }
}
