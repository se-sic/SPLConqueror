using MachineLearning.Solver;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Sampling.Heuristics
{

    /// <summary>
    /// This class performs a t-wise sampling on the binary configuration space defined by a  variability model.
    /// To this end, the variability model as well as the t for the sampling have to be provided by a user.
    /// </summary>
    class TWise
    {

        public const string PARAMETER_T_NAME = "t";

        /// <summary>
        /// Creates the t-wise sampling according to the given t-value.
        /// </summary>
        /// <param name="vm">The variability model containing the binary options for which we want to generate the pair-wise configurations.</param>
        /// <param name="t"> The t of the t-wise</param>
        /// <returns>A list of configurations in which each configuration is represented by a list of SELECTED binary options</returns>
        public List<List<BinaryOption>> generateT_WiseVariants_new(VariabilityModel vm, int t)
        {
            // dirty fix for twise issue
            if (t == 1)
            {
                FeatureWise fw = new FeatureWise();
                return fw.generateFeatureWiseConfigurations(vm);
            }

            List<BinaryOption> candidate = new List<BinaryOption>();
            List<List<BinaryOption>> result = new List<List<BinaryOption>>();
            generatePowerSet(vm, candidate, t, result, 0);

            //remove double entries...
            List<List<BinaryOption>> resultCleaned = new List<List<BinaryOption>>();
            List<String> configs = new List<string>();

            foreach (List<BinaryOption> options in result)
            {
                options.Sort(delegate (BinaryOption o1, BinaryOption o2) { return o1.Name.CompareTo(o2.Name); });

                String currConfig = "";

                foreach (BinaryOption binOpt in options)
                {
                    currConfig = currConfig + " " + binOpt.Name;
                }

                if (!configs.Contains(currConfig))
                {
                    resultCleaned.Add(options);
                    configs.Add(currConfig);
                }
            }


            return resultCleaned;

        }

        private void generatePowerSet(VariabilityModel vm, List<BinaryOption> candidates, int t, List<List<BinaryOption>> result, int index)
        {
            if (candidates.Count == t)
            {
                candidates = ConfigurationBuilder.vg.MinimizeConfig(candidates, vm, true, null);
                if (candidates.Count != 0)
                {
                    result.Add(candidates);
                }
                return;
            }

            for (int i = index; i < GlobalState.varModel.BinaryOptions.Count; i++)
            {
                if (candidates.Count < t)
                {
                    if (!candidates.Contains(GlobalState.varModel.BinaryOptions[i]))
                    {
                        List<BinaryOption> newCand = new List<BinaryOption>();
                        newCand.AddRange(candidates);
                        newCand.Add(GlobalState.varModel.BinaryOptions[i]);

                        if (newOptionIsValidForCandidate(candidates, GlobalState.varModel.BinaryOptions[i]))
                        {
                            generatePowerSet(vm, newCand, t, result, i + 1);
                        }
                    }

                }

            }
            return;
        }

        private bool newOptionIsValidForCandidate(List<BinaryOption> candidates, BinaryOption binaryOption)
        {

            for (int i = 0; i < candidates.Count; i++)
            {
                BinaryOption inList = candidates[i];

                //Check parent-child relationship
                if (inList.isAncestor(binaryOption) || binaryOption.isAncestor(inList) || inList == binaryOption)
                    return false;

                //Check if one option implies the presence of the other option
                bool impliedOption = false;
                foreach (var implied in inList.Implied_Options)
                {
                    if (implied.Count == 1 && implied[0] == binaryOption)
                    {
                        impliedOption = true;
                        break;
                    }
                }
                if (impliedOption)
                    return false;
                //vice versa
                foreach (var implied in binaryOption.Implied_Options)
                {
                    if (implied.Count == 1 && implied[0] == inList)
                    {
                        impliedOption = true;
                        break;
                    }
                }
                if (impliedOption)
                    return false;
            }
            return true;
        }
    }
}