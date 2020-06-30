using System;
using System.Diagnostics;
using System.Collections.Generic;
using MachineLearning.Solver;
using SPLConqueror_Core;
using System.IO;
using System.Numerics;

namespace MachineLearning.Sampling.Heuristics.UniformHeuristics
{
    public class UniformGrammarSamplingBigInt : UniformSampling
    {
        private Grammar Grammar;
        private FMTree Tree;
        private List<List<string>> MergedTerminals = new List<List<string>>();
        private Dictionary<string, string> strategyParameter;

        #region constants
        public const string NUM_CONFIGS = "numConfigs";
        public const string AS_TW = "asTW";
        public const string SEED = "seed";
        public const string WHOLE_POPULATION = "wholePopulation";
        public const string LANGUAGE_FILE = "languageFile";
        public const string CONF_FAULTS_REPORT = "confFaultsReport";
        #endregion
        // <summary>
        // This is only to convert an integer to a list of active features! Each position has the product of all bases that are left.
        // </summary>
        private List<BigInteger> Bases = new List<BigInteger>();

        private BigInteger NumberWords;
        private readonly int samples;
        private readonly bool wholePopulation;
        private RandomBigInteger rand;


        public UniformGrammarSamplingBigInt(Dictionary<string, string> parameters)
        {
            this.strategyParameter = new Dictionary<string, string>()
            {
                {SEED, "0" },
                {NUM_CONFIGS, "asTW2" },
                {WHOLE_POPULATION, "false"},
                {LANGUAGE_FILE, "language.csv"},
            };
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                if (strategyParameter.ContainsKey(keyValue.Key))
                {
                    strategyParameter[keyValue.Key] = keyValue.Value;
                }
            }
            rand = new RandomBigInteger(int.Parse(this.strategyParameter[SEED]));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            GenerateGrammar();
            MergeTerminals();
            if (!int.TryParse(this.strategyParameter[NUM_CONFIGS], out samples))
            {
                samples = CountConfigurations();
            }
            ComputeSamplingStrategy();
            stopwatch.Stop();
            Console.WriteLine("ConfigurationSampling done in {0} ms", stopwatch.ElapsedMilliseconds);

            Grammar.print();
            if (!bool.TryParse(this.strategyParameter[WHOLE_POPULATION], out wholePopulation))
            {
                wholePopulation = false;
            }
        }


        public override bool ComputeSamplingStrategy()
        {
            List<List<string>> featureLists = new List<List<string>>();
            List<List<BinaryOption>> configurationList = new List<List<BinaryOption>>();
            CheckConfigSATZ3 configSAT = new CheckConfigSATZ3();
            BigInteger[] randomNumbers = new BigInteger[samples];
            for (int i = 0; i < samples; i++)
            {
                randomNumbers[i] = -1;
            }

            for (int i = 0; i < samples; i++)
            {
                BigInteger number = rand.NextBigInteger(0, NumberWords);
                while (Array.IndexOf(randomNumbers, number) > -1)
                {
                    number = rand.NextBigInteger(0, NumberWords);
                }
                List<string> featureList = ConvertIntegerToFeatureList(number);
                List<BinaryOption> configuration = ConvertFeatureListToConfiguration(featureList);
                while (!configSAT.checkConfigurationSAT(configuration, GlobalState.varModel, false))
                {
                    number = rand.NextBigInteger(0, NumberWords);
                    while (Array.IndexOf(randomNumbers, number) > -1)
                    {
                        number = rand.NextBigInteger(0, NumberWords);
                    }
                    featureList = ConvertIntegerToFeatureList(number);
                    configuration = ConvertFeatureListToConfiguration(featureList);
                }
                randomNumbers[i] = number;
                featureLists.Add(featureList);
                configurationList.Add(configuration);
            }
            foreach (List<string> featureList in featureLists)
            {
                selectedConfigurations.Add(ConvertFeatureListToConfiguration(featureList));
            }
            return true;
        }

        private List<string> ConvertIntegerToFeatureList(BigInteger index)
        {
            List<string> features = new List<string>();
            BigInteger remainder = index;
            for (int i = 0; i < Bases.Count; i++)
            {
                BigInteger resBigInt = remainder / Bases[i];
                int res = (int) resBigInt;
                remainder %= Bases[i];
                string feature = MergedTerminals[i][res];
                if (feature != "\u03B5") features.Add(feature);

            }
            return features;
        }

        private List<BinaryOption> ConvertFeatureListToConfiguration(List<string> featureList)
        {
            List<string> configNames = new List<string>();
            foreach (string name in featureList)
            {
                FMNode node = Tree.getNodeByName(name);
                while (node.Parent != null)
                {
                    if (configNames.Contains(node.Name)) break;
                    configNames.Add(node.Name);
                    node = node.Parent;
                }
            }
            List<BinaryOption> configuration = new List<BinaryOption>();
            foreach (string name in configNames)
            {
                configuration.Add(Tree.getNodeByName(name).ActualOption);
            }
            configuration.Add(Tree.Root.ActualOption);
            return configuration;
        }

        private void GenerateGrammar()
        {
            Tree = new FMTree();
            Grammar = new Grammar(Tree);
        }

        private void MergeTerminals()
        {
            List<string> terminals = Grammar.Terminals;
            foreach (string terminal in terminals)
            {
                List<string> rule = Grammar.GetRule(terminal);
                if (!MergedTerminals.Contains(rule)) MergedTerminals.Add(rule);
            }
            for (int i = 0; i < MergedTerminals.Count; i++)
            {
                Bases.Add(MergedTerminals[i].Count);
            }
            int numWords = 1;
            foreach (int i in Bases)
            {
                numWords *= i;
            }
            NumberWords = numWords;
            for (int i = 0; i < Bases.Count; i++)
            {
                BigInteger basis = 1;
                for (int j = i + 1; j < Bases.Count; j++)
                {
                    basis *= Bases[j];
                }
                Bases[i] = basis;
            }
        }

        private void printMergedTerminals()
        {
            Console.Write("Merged Terminals: {");
            foreach (List<string> eqivClass in MergedTerminals)
            {
                Console.Write("{" + String.Join(", ", eqivClass) + "}");
            }
            Console.WriteLine("};");
            Console.WriteLine("Bases: {" + String.Join(",", Bases) + "}");
        }

        private void printSamplingSet()
        {
            Console.WriteLine("Grammar-based Sample Set:");
            foreach (List<BinaryOption> conf in selectedConfigurations)
            {
                foreach (BinaryOption feature in conf)
                {
                    Console.Write(feature.Name + " ");
                }
                Console.WriteLine(";");
            }
        }

        private int CountConfigurations()
        {
            int numberConfigs;

            string numConfigsValue = this.strategyParameter[NUM_CONFIGS];
            // Only "asTW" is currently supported
            if (numConfigsValue.Contains(AS_TW))
            {
                numConfigsValue = numConfigsValue.Replace(AS_TW, "").Trim();
                int t;
                int.TryParse(numConfigsValue, out t);
                TWise tw = new TWise();
                numberConfigs = tw.generateT_WiseVariants_new(GlobalState.varModel, t).Count;
            }
            else
            {
                throw new ArgumentException("Only asTW is currently supported. But was: " + numConfigsValue);
            }

            return numberConfigs;
        }
        public override string GetName()
        {
            return "UNIFORM-GRAMMAR-SAMPLING";
        }

        public override string GetTag()
        {
            return "UNIFORMGR";
        }

    }
}
