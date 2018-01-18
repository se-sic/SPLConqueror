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
        /// <param name="vm">The variability model of the given system.</param>
        /// <returns>A configuration that has a small weight.</returns>
        public static List<BinaryOption> getSmallWeightConfig(List<KeyValuePair<List<BinaryOption>, int>> sortedRanking,
            Z3Cache cache, VariabilityModel vm)
        {
            KeyValuePair<List<BinaryOption>, int>[] ranking = sortedRanking.ToArray();
            Microsoft.Z3.Solver solver = cache.GetSolver();
            Context z3Context = cache.GetContext();
            
            for (int i = 0; i < ranking.Length; i++)
            {
                List<BinaryOption> candidates = ranking [i].Key;
                solver.Push ();
                solver.Assert (forceFeatures(candidates, z3Context, cache.GetOptionToTermMapping()));

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

    }
}
