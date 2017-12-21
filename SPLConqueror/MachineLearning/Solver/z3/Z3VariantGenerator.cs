using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using Microsoft.Z3;

namespace MachineLearning.Solver
{
    /// <summary>
    /// This class represents the variant generator for the z3 solver and is responsible for
    /// using the z3 solver to generate different kinds of solutions.
    /// </summary>
    public class Z3VariantGenerator : IVariantGenerator
    {
        /// <summary>
        /// Generates all valid configurations by using the given <see cref="VariabilityModel"/>.
        /// </summary>
        /// <param name="vm">The <see cref="VariabilityModel"/> that describes the software.</param>
        /// <returns>A list of configurations. The configurations are represented by a list of <see cref="BinaryOption"/>.</returns>
        public List<List<BinaryOption>> GenerateAllVariantsFast(VariabilityModel vm)
        {
            return GenerateUpToNFast(vm, -1);
        }

        /// <summary>
        /// Generates up to n solutions of the given variability model. 
        /// Note that this method could also generate less than n solutions if the variability model does not contain sufficient solutions.
        /// Moreover, in the case that <code>n &lt; 0</code>, all solutions are generated.
        /// </summary>
        /// <param name="vm">The <see cref="VariabilityModel"/> to obtain solutions for.</param>
        /// <param name="n">The number of solutions to obtain.</param>
        /// <returns>A list of configurations, in which a configuration is a list of SELECTED binary options.</returns>
        public List<List<BinaryOption>> GenerateUpToNFast(VariabilityModel vm, int n)
        {
            List<BoolExpr> variables;
            Dictionary<BoolExpr, BinaryOption> termToOption;
            Dictionary<BinaryOption, BoolExpr> optionToTerm;
            Tuple<Context, BoolExpr> z3Tuple = Z3Solver.GetInitializedBooleanSolverSystem(out variables, out optionToTerm, out termToOption, vm);
            Context z3Context = z3Tuple.Item1;
            BoolExpr z3Constraints = z3Tuple.Item2;

            List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();

            Microsoft.Z3.Solver s = z3Context.MkSolver();
            s.Assert(z3Constraints);
            s.Push();

            Model model = null;
            while(s.Check() == Status.SATISFIABLE && (configurations.Count < n || n < 0))
            {
                model = s.Model;

                List<BinaryOption> config = RetrieveConfiguration(variables, model, termToOption);

                configurations.Add(config);

                s.Add(Z3Solver.NegateExpr(z3Context, Z3Solver.ConvertConfiguration(z3Context, config, optionToTerm, vm)));
            }

            return configurations;
        }

        private List<BinaryOption> RetrieveConfiguration(List<BoolExpr> variables, Model model, Dictionary<BoolExpr, BinaryOption> termToOption)
        {
            List<BinaryOption> config = new List<BinaryOption>();
            foreach (BoolExpr variable in variables)
            {
                Expr allocation = model.Evaluate(variable);
                BoolExpr boolExpr = (BoolExpr)allocation;
                if (boolExpr.IsTrue)
                {
                    config.Add(termToOption[variable]);
                }
            }
            return config;
        }

        public List<BinaryOption> GenerateConfigWithoutOption(BinaryOption optionToBeRemoved, List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm)
        {
            throw new NotImplementedException();
        }

        public List<List<BinaryOption>> GenerateRandomVariants(VariabilityModel vm, int treshold, int modulu)
        {
            throw new NotImplementedException();
        }

        public List<List<BinaryOption>> MaximizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unwantedOptions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loaded assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unWantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them.</param>
        /// <returns>The valid configuration (or null if there is none) that satisfies the VM and the goal.</returns>
        public List<BinaryOption> MinimizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unWantedOptions)
        {
            List<BoolExpr> variables;
            Dictionary<BoolExpr, BinaryOption> termToOption;
            Dictionary<BinaryOption, BoolExpr> optionToTerm;
            Tuple<Context, BoolExpr> z3Tuple = Z3Solver.GetInitializedBooleanSolverSystem(out variables, out optionToTerm, out termToOption, vm);
            Context z3Context = z3Tuple.Item1;
            BoolExpr z3Constraints = z3Tuple.Item2;

            List<BoolExpr> constraints = new List<BoolExpr>();
            constraints.Add(z3Constraints);

            //Feature Selection
            foreach (BinaryOption binOpt in config)
            {
                BoolExpr term = optionToTerm[binOpt];
                constraints.Add(term);
            }

            Model model = null;

            if (minimize == true)
            {

                //Defining Goals
                ArithExpr[] optimizationGoals = new ArithExpr[variables.Count];
                Dictionary<BinaryOption, ArithExpr> termToNumeric = new Dictionary<BinaryOption, ArithExpr>();

                for (int r = 0; r < variables.Count; r++)
                {
                    BinaryOption currOption = termToOption[variables[r]];
                    ArithExpr numericVariable = z3Context.MkInt(currOption.Name);
                    termToNumeric.Add(currOption, numericVariable);

                    int weight = 1;
                    if (unWantedOptions != null && (unWantedOptions.Contains(termToOption[variables[r]]) && !config.Contains(termToOption[variables[r]]))) {
                        weight = 100;
                    }

                    constraints.Add(z3Context.MkEq(numericVariable, z3Context.MkITE(variables[r], z3Context.MkInt(weight), z3Context.MkInt(0))));

                    optimizationGoals[r] = numericVariable;
                    
                }
                // For minimization, we need the class 'Optimize'
                Optimize optimizer = z3Context.MkOptimize();
                optimizer.Assert(constraints.ToArray());
                optimizer.MkMinimize(z3Context.MkAdd(optimizationGoals));

                if (optimizer.Check() != Status.SATISFIABLE)
                {
                    return new List<BinaryOption>();
                }
                else
                {
                    model = optimizer.Model;
                }

            } else
            {
                // Return the first configuration returned by the solver
                Microsoft.Z3.Solver solver = z3Context.MkSolver();
                solver.Assert(constraints.ToArray());
                
                if (solver.Check() != Status.SATISFIABLE)
                {
                    return new List<BinaryOption>();
                } else
                {
                    model = solver.Model;
                }

            }


            List<BinaryOption> result = RetrieveConfiguration(variables, model, termToOption);

            return result;
        }

        public List<BinaryOption> WeightMinimization(VariabilityModel vm, int numberSelectedFeatures, Dictionary<BinaryOption, int> featureWeight, List<Configuration> lastSampledConfigurations)
        {
            throw new NotImplementedException();
        }
    }
}
