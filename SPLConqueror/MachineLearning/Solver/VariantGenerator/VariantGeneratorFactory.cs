

namespace MachineLearning.Solver
{
    public class VariantGeneratorFactory
    {

        /// <summary>
        /// This method returns the variant generator using the given solver.
        /// </summary>
        /// <param name="solver">The solver, which should be used (e.g. CSP solver from the Microsoft solver foundation, or the z3 SMT solver).</param>
        /// <returns>The variant generator or <code>null</code> if there is no variant generator using the given solver.</returns>
        public static IVariantGenerator GetVariantGenerator(string solver)
        {
            switch (solver.ToLower())
            {
                case "z3":
                case "smt":
                    return new Z3VariantGenerator();
                case "csp":
                case "microsoft solver foundation":
                case "msf":
                    return new VariantGenerator();
                default:
                    return null;
            }
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
