using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Sampling.Heuristics
{
    class TWise
    {
        private Solver.VariantGenerator generator = new Solver.VariantGenerator(null);

        public const string PARAMETER_T_NAME = "t";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm">The variability model containing the binary options for which we want to generate the pair-wise configurations.</param>
        /// <param name="t"> The t of the t-wise</param>
        /// <returns>A list of configurations in which each configuration is represented by a list of SELECTED binary options</returns>
        public List<List<BinaryOption>> generateT_WiseVariants_new(VariabilityModel vm, int t)
        {
            List<BinaryOption> candidate = new List<BinaryOption>();
            List<List<BinaryOption>> result = new List<List<BinaryOption>>();
            generatePowerSet(vm, candidate, t, result, 0);

            return result;

        }

        private List<List<BinaryOption>> generatePowerSet(VariabilityModel vm, List<BinaryOption> candidates, int t, List<List<BinaryOption>> result, int index)
        {
            if (candidates.Count == t)
            {
                candidates = generator.minimizeConfig(candidates, vm, true, null);
                result.Add(candidates);
                return null;
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

                            if (candidates.Count == t)
                            {
                                candidates = generator.minimizeConfig(candidates, vm, true, null);
                                result.Add(candidates);
                                break;
                            }

                        }
                    }

                }

            }
            return null;
        }

        private bool newOptionIsValidForCandidate(List<BinaryOption> candidates, BinaryOption binaryOption)
        {

            for (int i = 0; i < candidates.Count; i++)
            {
                BinaryOption inList = candidates[i];

                //if (!activeLearning.Contains(pair.Name))
                //    continue;
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