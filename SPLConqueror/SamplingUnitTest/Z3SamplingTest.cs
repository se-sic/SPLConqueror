using CommandLine;
using MachineLearning.Sampling;
using MachineLearning.Sampling.Hybrid.Distributive;
using MachineLearning.Solver;
using NUnit.Framework;

namespace SamplingUnitTest
{
    [TestFixture]
    class Z3SamplingTest
    {
        private int EXPECTED_PAIRWISE = 308;

        [OneTimeSetUp]
        public void setupEnvironment()
        {
            Commands cmd = new Commands();
            cmd.performOneCommand(Commands.COMMAND_CLEAR_GLOBAL);
            SampleUtil.loadVM();
            ConfigurationBuilder.vg = VariantGeneratorFactory.GetVariantGenerator("z3");
        }

        [Test, Order(1)]
        public void testWholePop()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(SampleUtil.EXPECTED_CENTRALCOMP_ALLBINARY, "z3",
                SamplingStrategies.ALLBINARY, "AllbinarySampling.csv"));
        }

        [Test, Order(2)]
        public void TestNegFeatureWise()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(105, "z3"
                , SamplingStrategies.NEGATIVE_OPTIONWISE, "NegFWSampling.csv"));
        }

        [Test, Order(3)]
        public void TestPairwise()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(EXPECTED_PAIRWISE, "z3"
                , SamplingStrategies.PAIRWISE, "PairwiseSampling.csv"));
        }

        [Test, Order(4)]
        public void TestFeatureWise()
        {
            Assert.True(SampleUtil.TestBinaryNoParam(70, "z3"
                , SamplingStrategies.OPTIONWISE, "FeaturewiseSampling.csv"));
        }

        [Test, Order(5)]
        public void TestBoxBehnken()
        {
            Assert.True(SampleUtil.TestBoxBehnken("z3", 220));
        }

        [Test, Order(6)]
        public void TestHypersampling()
        {
            Assert.True(SampleUtil.TestHypersampling("z3", 176, 50));
        }

        [Test, Order(7)]
        public void TestOneFactorAtATime()
        {
            Assert.True(SampleUtil.TestOneFactorAtATime("z3", 220, 5));
        }

        [Test, Order(8)]
        public void TestNumericRandom()
        {
            Assert.True(SampleUtil.TestNumericRandom("z3", 352, 1, 8));
        }

        [Test, Order(9)]
        public void TestPlackettBurman()
        {
            Assert.True(SampleUtil.TestPlackettBurman("z3", 264, 3, 9));
        }

        [Test, Order(10)]
        public void TestKExchange()
        {
            Assert.True(SampleUtil.TestKExchange("z3", 264, 7, 2));
        }

        [Test, Order(12)]
        public void TestBinaryRandom()
        {
            Assert.True(SampleUtil.TestBinaryRandom("z3", 308, "asTW2", 15));
        }

        //[Test, Order(13)]
        //public void TestDistributionAware()
        //{
        //    Assert.True(SampleUtil.TestHybridStrategy("z3", 40, 0
        //        , new DistributionAware(), "DistributionAwareCompleteConfigurations"));
        //}

        //[Test, Order(14)]
        //public void TestDistributionPreserving()
        //{
        //    Assert.True(SampleUtil.TestHybridStrategy("z3", 40, 0
        //        , new DistributionPreserving(), "DistributionPreserving"));
        //}

        [Test, Order(15)]
        public void TestDistributionAwareSolverSelection()
        {
            Assert.True(SampleUtil.TestDistributionAwareSolverSelection("z3", 217,
             0, "SolverSelection", "True"));
        }

        [Test, Order(16)]
        public void TestTWise()
        {
            setupEnvironment();
            Assert.True(SampleUtil.TestTWise("z3", 602, 3));
        }
    }
}
