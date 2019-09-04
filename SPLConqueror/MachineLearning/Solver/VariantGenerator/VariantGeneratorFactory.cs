

using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Solver
{
    public class VariantGeneratorFactory
    {

        private static Dictionary<string, Type> variantGenerators;

        /// <summary>
        /// This method returns the variant generator using the given solver.
        /// </summary>
        /// <param name="solver">The solver, which should be used (e.g. CSP solver from the Microsoft solver foundation, or the z3 SMT solver).</param>
        /// <returns>The variant generator or <code>null</code> if there is no variant generator using the given solver.</returns>
        public static IVariantGenerator GetVariantGenerator(string solver)
        {
            if (variantGenerators == null)
                initSolvers();
            string solverKey = solver.ToLowerInvariant();
            Type generator;
            variantGenerators.TryGetValue(solverKey, out generator);

            if (generator == null)
            {
                IEnumerable<string> keys = variantGenerators.Keys.Where(dictKey => dictKey.StartsWith(solverKey));
                if (keys.Count() == 0)
                    throw new ArgumentException("Invalid VariantGenerator type: " + solver);
                string key = keys.First();
                Type value = variantGenerators[key];
                // save previous results so that consecutive calls dont
                // need to search for appropriate keys;
                variantGenerators[solverKey] = value;
                generator = value;
            }

            return (IVariantGenerator)Activator.CreateInstance(generator);
        }

        // Use Reflection to find all classes that implement variant generator
        // so modification of this interface is not required every time a new variant generator is
        // added and additionally conditional import depending on the presence of libraries of further 
        // variant generators is possible
        private static void initSolvers()
        {
            Type iVariantGenerator = typeof(IVariantGenerator);
            variantGenerators = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(type => !type.IsInterface && iVariantGenerator.IsAssignableFrom(type)).ToDictionary(type => type.Name.ToLowerInvariant());
            // initialize dictionary with some utility abreviations
            variantGenerators["z3"] = variantGenerators["z3variantgenerator"];
            variantGenerators["smt"] = variantGenerators["z3"];
            variantGenerators["csp"] = variantGenerators["microsoft solver foundation"] 
                = variantGenerators["msf"] 
                = variantGenerators["variantgenerator"];
        }

        /// <summary>
        /// Returns the currently supported solver.
        /// </summary>
        /// <returns>The currently supported solver.</returns>
        public static string GetSolver()
        {
            return "csp, smt";
        }

    }
}
