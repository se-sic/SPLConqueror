using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MachineLearning.Solver;
using SPLConqueror_Core;

namespace SamplingUnitTest
{
    [TestFixture]
    public class CplexVariantGeneratorTest
    {
        [SetUp]
        public static void loadSamplingVM()
        {
            SampleUtil.loadVM();
        }

        [Test]
        public static void TestGenerateAllVariantsFast()
        {
            CplexVariantGenerator vg = new CplexVariantGenerator();
            List<List<BinaryOption>> partialConfigurations = vg.GenerateAllVariantsFast(GlobalState.varModel);

            // Assert that all valid partial binary configurations are sampled
            Assert.AreEqual(108, partialConfigurations.Count);

            // Assert that variability model properties are not violoated
            AssertVariabilityModelProperties(partialConfigurations);
        }

        [Test]
        public static void TestGenerateUpToNFast()
        {
            CplexVariantGenerator vg = new CplexVariantGenerator();
            List<List<BinaryOption>> partialConfigurations;
            partialConfigurations = vg.GenerateUpToNFast(GlobalState.varModel, 10);

            // Assert that only 10 valid partial configurations are generated
            Assert.AreEqual(10, partialConfigurations.Count);
            AssertVariabilityModelProperties(partialConfigurations);

            partialConfigurations = vg.GenerateUpToNFast(GlobalState.varModel, 200);

            // Assert that no more than the number of possible valid partial configurations are
            // generated
            Assert.AreEqual(108, partialConfigurations.Count);
            AssertVariabilityModelProperties(partialConfigurations);
        }

        [Test]
        public static void TestMinimizeConfig()
        {
            CplexVariantGenerator cplex = new CplexVariantGenerator();

            List<BinaryOption> minimalConf = 
                cplex.MinimizeConfig(new List<BinaryOption>(), GlobalState.varModel, true, new List<BinaryOption>());

            // Assert that the most minimal valid partial configuration only has 6 options selected
            Assert.AreEqual(6, minimalConf.Count);

            List<BinaryOption> maximalConf =
                cplex.MinimizeConfig(new List<BinaryOption>(), GlobalState.varModel, false, new List<BinaryOption>());

            // Assert that the most maximal valid partial configuration has exactly 10 options selected
            Assert.AreEqual(10, maximalConf.Count);

            List<BinaryOption> blacklist = new List<BinaryOption>();
            blacklist.Add(GlobalState.varModel.getBinaryOption("binOpt4"));

            List<BinaryOption> maximalBlacklisted =
                cplex.MinimizeConfig(new List<BinaryOption>(), GlobalState.varModel, false, blacklist);

            // Assert that the blacklisted optional option is not selected in the maximal configuration
            Assert.AreEqual(9, maximalBlacklisted.Count);
            Assert.True(!maximalBlacklisted.Contains(GlobalState.varModel.getBinaryOption("binOpt4")));

            List<BinaryOption> whitelist = new List<BinaryOption>();
            whitelist.Add(GlobalState.varModel.getBinaryOption("binOpt6"));
            List<BinaryOption> minimalRequirement =
                cplex.MinimizeConfig(whitelist, GlobalState.varModel, true, new List<BinaryOption>());

            // Assert that the required optional option is selected in the minimal configuration
            Assert.AreEqual(7, minimalRequirement.Count);
            Assert.True(minimalRequirement.Contains(GlobalState.varModel.getBinaryOption("binOpt6")));
        }

        [Test]
        public static void TestMaximizeConfig()
        {
            CplexVariantGenerator cplex = new CplexVariantGenerator();

            List<List<BinaryOption>> allMinimalConfs = 
                cplex.MaximizeConfig(new List<BinaryOption>(), GlobalState.varModel, true, new List<BinaryOption>());

            // Assert that all possible minimal configurations are generated 
            // and that all minimal configurations are really only minimal
            Assert.AreEqual(9, allMinimalConfs.Count);
            Assert.True(allMinimalConfs.TrueForAll(minimalConf => minimalConf.Count == 6));

            List<List<BinaryOption>> allMaximalConfs =
                cplex.MaximizeConfig(new List<BinaryOption>(), GlobalState.varModel, false, new List<BinaryOption>());

            // Assert that all possible maximal configurations are generated
            // and that all maximal configuration are really only maximal
            Assert.AreEqual(9, allMaximalConfs.Count);
            Assert.True(allMaximalConfs.TrueForAll(maximalConf => maximalConf.Count == 10));

            List<BinaryOption> blacklist = new List<BinaryOption>();
            blacklist.Add(GlobalState.varModel.getBinaryOption("binOpt4"));

            List<List<BinaryOption>> allMaiximalConfsBlacklisted =
                cplex.MaximizeConfig(new List<BinaryOption>(), GlobalState.varModel, false, blacklist);

            // Assert that all possible maximal configurations without binOpt4
            // are generated and that all configurations do not contain binOpt 4
            // and are as maximal as possible
            Assert.AreEqual(18, allMaiximalConfsBlacklisted.Count);
            Assert.True(allMaiximalConfsBlacklisted.TrueForAll(maximalBlacklisted => maximalBlacklisted.Count == 9));
            Assert.True(allMaiximalConfsBlacklisted.TrueForAll(maximalBlacklisted => 
                !maximalBlacklisted.Contains(GlobalState.varModel.getBinaryOption("binOpt4")))
            );

            List<BinaryOption> whitelist = new List<BinaryOption>();
            whitelist.Add(GlobalState.varModel.getBinaryOption("binOpt6"));

            List<List<BinaryOption>> allMinimalWhitelisted =
                cplex.MaximizeConfig(whitelist, GlobalState.varModel, true, new List<BinaryOption>());

            // Assert that all possible minimal configurations containing binOpt6
            // are generated and that they are as minimal as possible
            Assert.AreEqual(9, allMinimalWhitelisted.Count);
            Assert.True(allMinimalWhitelisted.TrueForAll(minimalWhitelisted => minimalWhitelisted.Count == 7));
            Assert.True(allMinimalWhitelisted.TrueForAll(minimalWhitelisted =>
                minimalWhitelisted.Contains(GlobalState.varModel.getBinaryOption("binOpt6")))
            );
        }

        [Test]
        public static void TestGenerateConfigWithoutOption()
        {
            CplexVariantGenerator cplex = new CplexVariantGenerator();

            List<BinaryOption> removed;

            List<BinaryOption> maximalWithoutBlacklist =
                cplex.GenerateConfigWithoutOption(GlobalState.varModel.getBinaryOption("binOpt4"),
                GlobalState.varModel.BinaryOptions, out removed, GlobalState.varModel);

            // Assert that a maximal configuration without binOpt4 is generated and all invalid
            // options are deselected
            Assert.AreEqual(9, maximalWithoutBlacklist.Count);
            Assert.True(!maximalWithoutBlacklist.Contains(GlobalState.varModel.getBinaryOption("binOpt4")));
            Assert.AreEqual(6, removed.Count);

        }

        private static void AssertVariabilityModelProperties(List<List<BinaryOption>> partialConfigurations)
        {
            // Assert that every partial configurations contains the root and mandatory options
            Assert.True(partialConfigurations.TrueForAll(partialConf => partialConf.Contains(GlobalState.varModel.Root)));
            BinaryOption mandatory = GlobalState.varModel.getBinaryOption("binOpt1");
            Assert.True(partialConfigurations.TrueForAll(partialConf => partialConf.Contains(mandatory)));

            // Assert that no partial configuration has exactly one member of an alternative group selected
            BinaryOption alternative1 = GlobalState.varModel.getBinaryOption("member21");
            BinaryOption alternative2 = GlobalState.varModel.getBinaryOption("member22");
            BinaryOption alternative3 = GlobalState.varModel.getBinaryOption("member23");
            Assert.True(partialConfigurations.TrueForAll(partialConf =>
            {
                return (partialConf.Contains(alternative1) && !partialConf.Contains(alternative2) && !partialConf.Contains(alternative3))
                || (!partialConf.Contains(alternative1) && partialConf.Contains(alternative2) && !partialConf.Contains(alternative3))
                || (!partialConf.Contains(alternative1) && !partialConf.Contains(alternative2) && partialConf.Contains(alternative3));

            }));

            // Assert that there exists at least one partial configuration that has a optional option selected
            // and one that has a option option selected
            BinaryOption optional = GlobalState.varModel.getBinaryOption("binOpt6");
            Assert.True(partialConfigurations.Any(partialConf => partialConf.Contains(optional))
                && partialConfigurations.Any(partialConf => !partialConf.Contains(optional)));

            // Assert that implication constraints are not violated, i.e. if binOpt5 is selected then
            // binOpt6 also has to be selected
            BinaryOption implicant = GlobalState.varModel.getBinaryOption("binOpt5");
            Assert.True(partialConfigurations.TrueForAll(partialConf =>
            {
                return !partialConf.Contains(implicant) || (partialConf.Contains(implicant) && partialConf.Contains(optional));
            }));

            // Assert that exclusion constraints are not violated, i.e if binOpt3 is selected then
            // binOpt2 is not selected
            BinaryOption exclusionTrigger = GlobalState.varModel.getBinaryOption("binOpt3");
            BinaryOption excluded = GlobalState.varModel.getBinaryOption("binOpt2");
            Assert.True(partialConfigurations.TrueForAll(partialConf =>
            {
                return !partialConf.Contains(exclusionTrigger) || !partialConf.Contains(excluded);
            }));
        }

        [Test]
        public static void TestGenerateConfigurationFromBucket()
        {
            CplexVariantGenerator cplex = new CplexVariantGenerator();

            List<BinaryOption> firstConf = 
                cplex.GenerateConfigurationFromBucket(GlobalState.varModel, 7, null, null);

            List<BinaryOption> secondConf =
                cplex.GenerateConfigurationFromBucket(GlobalState.varModel, 7, null, new Configuration(firstConf));

            // Assert that both first and second conf have exactly 7 options selected
            // that they are not equal
            Assert.AreEqual(7, firstConf.Count);
            Assert.AreEqual(7, secondConf.Count);
            Assert.True(secondConf.Any(option => !firstConf.Contains(option)));

            cplex.ClearCache();

            List<BinaryOption> secondRun =
                cplex.GenerateConfigurationFromBucket(GlobalState.varModel, 7, null, null);

            // Assert that after clearing the cache we generate the first configuration again
            Assert.AreEqual(7, secondRun.Count);
            Assert.True(secondRun.TrueForAll(option => firstConf.Contains(option)));
        }

        [Test]
        public static void TestDistanceMaximization()
        {
            CplexVariantGenerator cplex = new CplexVariantGenerator();

            List<List<BinaryOption>> diverseSet = cplex.DistanceMaximization(GlobalState.varModel, null, 10, 0);

            // Assert that exactly 10 configurations are generated and that there
            // are no duplicates in the the set
            Assert.AreEqual(10, diverseSet.Count);
            Assert.True(diverseSet.TrueForAll(conf => diverseSet.TrueForAll(other => 
                conf == other || conf.Count != other.Count 
                || conf.Any(opt => !other.Contains(opt)) || other.Any(opt => !conf.Contains(opt)))
            )); 
        }

        [Test]
        public static void TestGenerateAllVariants()
        {
            CplexVariantGenerator vg = new CplexVariantGenerator();
            List<Configuration> configurations = vg
                .GenerateAllVariants(GlobalState.varModel, GlobalState.varModel.getOptions());

            // Assert that all possible configurations are sampled
            Assert.AreEqual(864, configurations.Count);

            NumericOption numOpt1 = GlobalState.varModel.getNumericOption("numOpt1");
            NumericOption numOpt2 = GlobalState.varModel.getNumericOption("numOpt2");

            // Assert that all configurations contain the numeric options
            Assert.True(configurations.TrueForAll(conf => conf.NumericOptions.ContainsKey(numOpt1) 
                && conf.NumericOptions.ContainsKey(numOpt2)));

            // Assert that ever possible value of the first numeric option appears at least once
            // and that all values are only within the valid range
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt1]) == 0));
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt1]) == 2));
            Assert.True(configurations.TrueForAll(conf =>
            {
                int numOpt1Val = Convert.ToInt32(conf.NumericOptions[numOpt1]);
                return numOpt1Val == 0 || numOpt1Val == 2;
            }));

            // Assert that ever possible value of the second numeric option appears at least once
            // and that all values are only within the valid range
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt2]) == 1));
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt2]) == 2));
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt2]) == 4));
            Assert.True(configurations.Any(conf => Convert.ToInt32(conf.NumericOptions[numOpt2]) == 8));
            Assert.True(configurations.TrueForAll(conf =>
            {
                int numOpt2Val = Convert.ToInt32(conf.NumericOptions[numOpt2]);
                return numOpt2Val == 1 || numOpt2Val == 2
                    || numOpt2Val == 4 || numOpt2Val == 8;
            }));
        }
    }
}
