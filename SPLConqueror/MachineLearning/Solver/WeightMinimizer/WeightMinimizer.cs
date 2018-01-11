using Microsoft.Z3;
using SPLConqueror_Core;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Solver
{

    /// <summary>
    /// Class that tries to approximately minimize the combined weight of all features of a configuration.
    /// </summary>
    class WeightMinimizer
    {
        private WeightMinimizer() {}

        /// <summary>
        /// Try to find a configuration with low weight.
        /// </summary>
        /// <param name="sortedRanking">A list of binary options and their weight ordered by their weight.</param>
        /// <param name="cache">A sat solver cache instance that already contains the contraints of 
        /// size and unallowed features.</param>
        /// <param name="numberFeature">The number of features that should be used for minimization.</param>
        /// <param name="maxFeatures">The maximum number of </param>
        /// <returns>A configuration that has a small weight.</returns>
        public static List<BinaryOption> getSmallWeightConfig(List<KeyValuePair<BinaryOption, int>> sortedRanking,
            Z3Cache cache, int numberFeature, int maxFeatures, VariabilityModel vm)
        {
            KeyValuePair<BinaryOption, int>[] ranking = sortedRanking.ToArray();
            Microsoft.Z3.Solver solver = cache.GetSolver();
            Context z3Context = cache.GetContext();

            ranking = removeMandatoryOptions(ranking);
            
            for (int i = 0; i < ranking.Length; i++)
            {
                List<BinaryOption> candidates = new List<BinaryOption>();
                candidates.Add(ranking[i].Key);

                for (int j = i + numberFeature - 1; j < ranking.Length && numberFeature <=  j - i; j++)
                {
                    candidates.Add(ranking[j].Key);
                }

                if (!testExcludedOptions(candidates))
                    continue;

                if (candidates.Count == numberFeature)
                {
                    solver.Push();
                    solver.Assert(forceFeatures(candidates, z3Context, cache.GetOptionToTermMapping()));
                }

                if (solver.Check() == Status.SATISFIABLE)
                {
                    Model model = solver.Model;
                    solver.Pop();
                    return Z3VariantGenerator.RetrieveConfiguration(cache.GetVariables(), model,
                        cache.GetTermToOptionMapping());
                }
                solver.Pop();
            }

            return null;
        }

        private static BoolExpr forceFeatures(List<BinaryOption> binOpts, Context z3context, Dictionary<BinaryOption, BoolExpr> optionToTerm)
        {
            List<BoolExpr> and = new List<BoolExpr>();

            foreach (BinaryOption binOpt in binOpts)
            {
                and.Add(optionToTerm[binOpt]);
            }

            return z3context.MkAnd(and);

        }

        private static KeyValuePair<BinaryOption, int>[] removeMandatoryOptions(KeyValuePair<BinaryOption, int>[] options)
        {
            return options.Where(x => x.Key.Optional || x.Key.Excluded_Options.Count > 0).ToArray();
        }

        private static bool testExcludedOptions(List<BinaryOption> binOpts)
        {
            if (binOpts.Count == 1)
            {
                return true;
            } else
            {
                List<BinaryOption> excluded = new List<BinaryOption>();
                binOpts.ForEach(opt => opt.Excluded_Options.
                ForEach(nonValid => excluded.AddRange(nonValid.Select(confOpt => (BinaryOption)confOpt))));

                foreach (BinaryOption binOpt in binOpts)
                {
                    if (excluded.Contains(binOpt))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
