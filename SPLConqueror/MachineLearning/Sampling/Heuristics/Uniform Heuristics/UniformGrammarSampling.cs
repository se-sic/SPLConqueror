using System;
using System.Collections.Generic;
using MachineLearning.Solver;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Heuristics.UniformHeuristics
{
    public class UniformGrammarSampling : UniformSampling
    {
        private Grammar Grammar;
        private FMTree Tree;
        private List<List<string>> MergedTerminals = new List<List<string>>();
        private Dictionary<string, string> strategyParameter;

        #region constants
        public const string NUM_SAMPLES = "numSamples";
        public const string SEED = "seed";
        #endregion
        // <summary>
        // This is only to convert an integer to a list of active features! Each position has the product of all bases that are left.
        // </summary>
        private List<int> Bases = new List<int>();

        private int NumberWords;
        private readonly int samples;


        public UniformGrammarSampling(Dictionary<string, string> parameters)
        {
            this.strategyParameter = new Dictionary<string, string>()
            {
                {NUM_SAMPLES, "0,1" },
                {SEED, "0" },
            };        
            foreach (KeyValuePair<string, string> keyValue in parameters)
            {
                if (strategyParameter.ContainsKey(keyValue.Key))
                {
                    strategyParameter[keyValue.Key] = keyValue.Value;
                }
            }
            GenerateGrammar();
            MergeTerminals();
            samples = (int) Math.Floor(NumberWords * float.Parse(this.strategyParameter[NUM_SAMPLES]));
            ComputeSamplingStrategy();
            // printSamplingSet();

        }

        public override bool ComputeSamplingStrategy()
        {
            Random random = new Random(int.Parse(this.strategyParameter[SEED]));
            List<List<string>> featureLists = new List<List<string>>();
            List<List<BinaryOption>> configurationList = new List<List<BinaryOption>>();
            CheckConfigSATZ3 configSAT = new CheckConfigSATZ3();
            for (int i = 0; i < samples; i++)
            {
                List<string> featureList = ConvertIntegerToFeatureList(random.Next(0, NumberWords));
                List<BinaryOption> configuration = ConvertFeatureListToConfiguration(featureList);
                while (! configSAT.checkConfigurationSAT(configuration, GlobalState.varModel, false))
                {
                    featureList = ConvertIntegerToFeatureList(random.Next(0, NumberWords));
                    configuration = ConvertFeatureListToConfiguration(featureList);
                    // Console.WriteLine("get a new configuration: " + String.Join(", ", featureList));
                }
                featureLists.Add(featureList);
                configurationList.Add(configuration);
            }
            foreach (List<string> featureList in featureLists)
            {
                selectedConfigurations.Add(ConvertFeatureListToConfiguration(featureList));
            }
            // Grammar.print();
            // printMergedTerminals();

            return true;
        }

        private List<string> ConvertIntegerToFeatureList(int index)
        {
            List<string> features = new List<string>();
            int remainder = index;
            for (int i = 0; i < Bases.Count; i++)
            {
                int res = remainder / Bases[i];
                remainder %= Bases[i];
                string feature = MergedTerminals[i][res];
                if (feature != "\u03B5") features.Add(feature);

            }
            return features;
        }

        private List<BinaryOption> ConvertFeatureListToConfiguration (List<string> featureList)
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
                int basis = 1;
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
            foreach(List<BinaryOption> conf in selectedConfigurations)
            {
                foreach(BinaryOption feature in conf)
                {
                    Console.Write(feature.Name + " ");
                }
                Console.WriteLine(";");
            }
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
