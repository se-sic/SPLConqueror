using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using MachineLearning.Solver;

namespace MachineLearning.Sampling.Heuristics
{
    /// <summary>
    /// This sampling strategy offers the possibility of selecting a set of random binary partial configurations.
    /// </summary>
    public class RandomBinary
    {
        private List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        private VariabilityModel varModel;

        /// <summary>
        /// The constructror of this class.
        /// </summary>
        /// <param name="varModel">The variability model defining the configuration space.</param>
        public RandomBinary(VariabilityModel varModel)
        {
            this.varModel = varModel;
        }

        /// <summary>
        /// Returns a set of random binary partial configurations. 
        /// </summary>
        /// <param name="parameters">Parameters for this random sampling. The following paramters are supported:
        /// seed = the seed for the random generator (int required)
        /// numConfigs = the number of configurations that have to be selected. 
        ///              To be able ot select a number of configurations equal to the number selected by the OW heuristic or 
        ///              the TWise heuristics, two special values can be given for this paramter. To select a number equal to 
        ///              the OW heuristics use "asOW" as value and to select a number equal to a TWise heuristics with a t of X
        ///              use "asTWX".
        /// </param>
        /// <returns>A list of random binary partial configuartions.</returns>
        public List<List<BinaryOption>> getRandomConfigs(Dictionary<String, String> parameters)
        {
            configurations.Clear();

            int seed = 0;
            int numConfigs = varModel.BinaryOptions.Count;

            // parse parameters
            if (parameters.ContainsKey("numConfigs"))
            {
                String numConfigsValue = parameters["numConfigs"];
                if (!int.TryParse(numConfigsValue, out numConfigs))
                {
                    // special constants as parameter (numConfigs = asOW or asTWX
                    if (numConfigsValue.Contains("asOW"))
                    {
                        FeatureWise fw = new FeatureWise();
                        numConfigs = fw.generateFeatureWiseConfigsCSP(varModel).Count;
                    }
                    else if (numConfigsValue.Contains("asTW"))
                    {
                        numConfigsValue = numConfigsValue.Replace("asTW", "").Trim();
                        int.TryParse(numConfigsValue, out numConfigs);
                        TWise tw = new TWise();
                        numConfigs = tw.generateT_WiseVariants_new(varModel, numConfigs).Count;
                    }
                }
            }
            if(parameters.ContainsKey("seed"))
                int.TryParse(parameters["seed"],out seed);

            // build set of all valid binary partial configurations
            VariantGenerator vg = new VariantGenerator();
            List<List<BinaryOption>> allConfigs = vg.generateAllVariantsFast(varModel);

            //repair wrong parameters
            if (numConfigs >= allConfigs.Count)
            {
                if (numConfigs > allConfigs.Count)
                    GlobalState.logError.logLine("Random Sampling: numConfigs to large for variability model. num set to " + allConfigs.Count);
                configurations = allConfigs;
                return allConfigs;
            }

            // select random configurations
            Random r = new Random(seed);
            for (int i = 0; i < numConfigs; i++)
            {
                List<BinaryOption> selectedConfig = allConfigs[r.Next(allConfigs.Count )];

                if(configurations.Contains(selectedConfig))
                {
                    i -= 1;
                }
                else
                {
                    configurations.Add(selectedConfig);
                }

            }
            return configurations;
        }
        
    }
}
