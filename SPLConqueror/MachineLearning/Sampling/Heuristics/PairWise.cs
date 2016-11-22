using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Heuristics
{
    public class PairWise
    {
        private List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
        private Solver.VariantGenerator generator = new Solver.VariantGenerator(null);

        

        /// <summary>
        /// Generates a configuration for each pair of configuration options. Exceptions: parent-child-relationships, impliciation-relationships
        /// </summary>
        /// <param name="vm">The variability model containing the binary options for which we want to generate the pair-wise configurations.</param>
        /// <returns>A list of configurations in which each configuration is represented by a list of SELECTED binary options</returns>
        public List<List<BinaryOption>> generatePairWiseVariants(VariabilityModel vm)
        {
            //List<String> activeLearning = new List<string>(new string[] { "ls", "inl", "cf", "dcr", "saa", "ive", "wlur", "lir", "vp", "saacyc" });
            this.configurations.Clear();
            List<BinaryOption> measuredElements = new List<BinaryOption>();
            foreach (BinaryOption current in vm.BinaryOptions)
            {
                measuredElements.Add(current);
                foreach (BinaryOption pair in vm.BinaryOptions)
                {
                    //if (!activeLearning.Contains(pair.Name))
                    //    continue;
                    //Check parent-child relationship
                    if (pair.isAncestor(current) || current.isAncestor(pair) || pair == current)
                        continue;
                    
                    //Check if one option implies the presence of the other option
                    bool impliedOption = false;
                    foreach (var implied in pair.Implied_Options)
                    {
                        if (implied.Count == 1 && implied[0] == current)
                        {
                            impliedOption = true;
                            break;
                        }
                    }
                    if (impliedOption)
                        continue;
                    //vice versa
                    foreach (var implied in current.Implied_Options)
                    {
                        if (implied.Count == 1 && implied[0] == pair)
                        {
                            impliedOption = true;
                            break;
                        }
                    }
                    if (impliedOption)
                        continue;


                    if (pair != current && !measuredElements.Contains(pair))
                    {
                        List<BinaryOption> tempConfig = new List<BinaryOption>();
                        tempConfig.Add(current);
                        tempConfig.Add(pair);
                        tempConfig = generator.minimizeConfig(tempConfig, vm, true, null);

                        if (tempConfig.Count > 0 && !Configuration.containsBinaryConfiguration(configurations,tempConfig))
                            configurations.Add(tempConfig);
                    }
                }
            }
            return this.configurations;
        }
    }
}
