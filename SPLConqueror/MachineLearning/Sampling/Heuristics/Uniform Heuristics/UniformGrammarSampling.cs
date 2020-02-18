using System;
using System.Collections.Generic;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Heuristics.UniformHeuristics
{
    public class UniformGrammarSampling : UniformSampling
    {
        private Grammar Grammar;
        private FMTree Tree;
        private List<List<string>> MergedTerminals = new List<List<string>>();

        // <summary>
        // This is only to convert an integer to a list of active features! Each position has the product of all bases that are left.
        // </summary>
        private List<int> Bases = new List<int>();

        private int NumberWords;
        private readonly int samples;


        public UniformGrammarSampling()
        {
            GenerateGrammar();
            MergeTerminals();
            samples = NumberWords / 10;
            ComputeSamplingStrategy();
        }

        public override bool ComputeSamplingStrategy()
        {
            Random random = new Random();
            List<List<string>> featureLists = new List<List<string>>();
            for (int i = 0; i < samples; i++)
            {
                featureLists.Add(ConvertIntegerToFeatureList(random.Next(0, NumberWords)));
            }
            foreach (List<string> featureList in featureLists)
            {
                selectedConfigurations.Add(ConvertFeatureListToConfiguration(featureList));
            }
            Grammar.print();
            printMergedTerminals();

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
