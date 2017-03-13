using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Sampling.Heuristics
{
    class MinMax
    {
        private List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();

        public List<List<BinaryOption>> generateMinMaxConfigurations(VariabilityModel vm)
        {
            configurations.Clear();

            // add minimal configurations 
            FeatureWise fw = new FeatureWise();
            List<List<BinaryOption>> optionWiseConfigs = fw.generateFeatureWiseConfigurations(vm);
            int minimalNumberOfOptions = int.MaxValue;
            for(int i = 0; i < optionWiseConfigs.Count; i++)
            {
                if (optionWiseConfigs[i].Count < minimalNumberOfOptions)
                    minimalNumberOfOptions = optionWiseConfigs[i].Count;
            }
            for(int j = 0; j < optionWiseConfigs.Count; j++)
            {
                if(optionWiseConfigs[j].Count == minimalNumberOfOptions)
                {
                    configurations.Add(optionWiseConfigs[j]);
                }                    
            }


            // add maximal configurations
            NegFeatureWise negFW = new NegFeatureWise();
            List<List<BinaryOption>> negOWConfigs = negFW.generateNegativeFWAllCombinations(vm);
            int maximalNumberOfOptions = int.MinValue;
            for (int i = 0; i < negOWConfigs.Count; i++)
            {
                if (maximalNumberOfOptions < negOWConfigs[i].Count)
                    maximalNumberOfOptions = negOWConfigs[i].Count;
            }
            for (int j = 0; j < optionWiseConfigs.Count; j++)
            {
                if (negOWConfigs[j].Count == maximalNumberOfOptions)
                {
                    if(!configurations.Contains(negOWConfigs[j]))
                        configurations.Add(negOWConfigs[j]);
                }
            }



            return configurations;

        }

    }
}
