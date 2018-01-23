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
        private Dictionary<int, Z3Cache> _z3Cache;

        private uint z3RandomSeed = 1;
        private const string RANDOM_SEED = ":random-seed";

        /// <summary>
        /// Creates a sample of configurations, by iteratively adding a configuration that has the maximal manhattan distance 
        /// to the configurations that were previously selected.
        /// </summary>
        /// <param name="vm">The domain for sampling.</param>
        /// <param name="minimalConfiguration">A minimal configuration that will be used as starting point.</param>
        /// <param name="numberToSample">The number of configurations that should be sampled.</param>
        /// <param name="optionWeight">Weight assigned to optional binary options.</param>
        /// <returns>A list of distance maximized configurations.</returns>
        public List<List<BinaryOption>> DistanceMaximization(VariabilityModel vm, List<BinaryOption> minimalConfiguration, int numberToSample, int optionWeight)
        {
            throw new NotImplementedException("Distance maximization is not yet implemented in the z3 solver. Please try the csp solver.");
        }

        /// <summary>
        /// This method sets the random seed for the z3 solver.
        /// </summary>
        /// <param name="seed">The random seed for the z3 solver.</param>
        public void setSeed(uint seed) {
            this.z3RandomSeed = seed;
        }

        /// <summary>
        /// Generates all valid combinations of all configuration options in the given model.
        /// </summary>
        /// <param name="vm">the variability model containing the binary options and their constraints</param>
        /// <param name="optionsToConsider">the options that should be considered. All other options are ignored</param>
        /// <returns>Returns a list of <see cref="Configuration"/></returns>
        public List<Configuration> GenerateAllVariants(VariabilityModel vm, List<ConfigurationOption> optionsToConsider)
        {
            List<Configuration> allConfigurations = new List<Configuration>();
            List<BoolExpr> variables;
            Dictionary<BoolExpr, BinaryOption> termToOption;
            Dictionary<BinaryOption, BoolExpr> optionToTerm;
            Tuple<Context, BoolExpr> z3Tuple = Z3Solver.GetInitializedBooleanSolverSystem(out variables, out optionToTerm, out termToOption, vm);
            Context z3Context = z3Tuple.Item1;
            BoolExpr z3Constraints = z3Tuple.Item2;

            Microsoft.Z3.Solver solver = z3Context.MkSolver();

            // TODO: The following line works for z3Solver version >= 4.6.0
            //solver.Set (RANDOM_SEED, z3RandomSeed);
            Params solverParameter = z3Context.MkParams ();
            solverParameter.Add (RANDOM_SEED, z3RandomSeed);
            solver.Parameters = solverParameter;

            solver.Assert(z3Constraints);

            while (solver.Check() == Status.SATISFIABLE)
            {
                Model model = solver.Model;

                List<BinaryOption> binOpts = RetrieveConfiguration(variables, model, termToOption, optionsToConsider);

                Configuration c = new Configuration(binOpts);
                // Check if the non-boolean constraints are satisfied
                if (vm.configurationIsValid(c) && !VariantGenerator.IsInConfigurationFile(c, allConfigurations) && VariantGenerator.FulfillsMixedConstraints(c, vm))
                {
                    allConfigurations.Add(c);
                }
                solver.Push();
                solver.Assert(Z3Solver.NegateExpr(z3Context, Z3Solver.ConvertConfiguration(z3Context, binOpts, optionToTerm, vm)));
            }

            solver.Push();
            solver.Pop(Convert.ToUInt32(allConfigurations.Count() + 1));
            return allConfigurations;
        }

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

            // TODO: The following line works for z3Solver version >= 4.6.0
            //solver.Set (RANDOM_SEED, z3RandomSeed);
            Params solverParameter = z3Context.MkParams ();
            solverParameter.Add (RANDOM_SEED, z3RandomSeed);
            s.Parameters = solverParameter;

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

        /// <summary>
        /// Parses a z3 solution into a configuration.
        /// </summary>
        /// <param name="variables">List of all variables in the z3 context.</param>
        /// <param name="model">Solution of the context.</param>
        /// <param name="termToOption">Map from variables to binary options.</param>
        /// <param name="optionsToConsider">The options that are considered for the solution.</param>
        /// <returns>Configuration parsed from the solution.</returns>
        public static List<BinaryOption> RetrieveConfiguration(List<BoolExpr> variables, Model model, Dictionary<BoolExpr, BinaryOption> termToOption, List<ConfigurationOption> optionsToConsider = null)
        {
            List<BinaryOption> config = new List<BinaryOption>();
            foreach (BoolExpr variable in variables)
            {
                if (optionsToConsider != null && !optionsToConsider.Contains(termToOption[variable]))
                {
                    continue;
                }

                Expr allocation = model.Evaluate(variable);
                BoolExpr boolExpr = (BoolExpr)allocation;
                if (boolExpr.IsTrue)
                {
                    config.Add(termToOption[variable]);
                }
            }
            return config;
        }

        /// <summary>
        /// The method aims at finding a configuration which is similar to the given configuration, but does not contain the optionToBeRemoved. If further options need to be removed from the given configuration, they are outputed in removedElements.
        /// </summary>
        /// <param name="optionToBeRemoved">The binary configuration option that must not be part of the new configuration.</param>
        /// <param name="originalConfig">The configuration for which we want to find a similar one.</param>
        /// <param name="removedElements">If further options need to be removed from the given configuration to build a valid configuration, they are outputed in this list.</param>
        /// <param name="vm">The variability model containing all options and their constraints.</param>
        /// <returns>A configuration that is valid, similar to the original configuration and does not contain the optionToBeRemoved.</returns>
        public List<BinaryOption> GenerateConfigWithoutOption(BinaryOption optionToBeRemoved, List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Based on a given (partial) configuration and a variability, we aim at finding all optimally maximal or minimal (in terms of selected binary options) configurations.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unwantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them</param>
        /// <returns>A list of configurations that satisfies the VM and the goal (or null if there is none).</returns>
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

                for (int r = 0; r < variables.Count; r++)
                {
                    BinaryOption currOption = termToOption[variables[r]];
                    ArithExpr numericVariable = z3Context.MkIntConst(currOption.Name);

                    int weight = 1;
                    if (unWantedOptions != null && (unWantedOptions.Contains(termToOption[variables[r]]) && !config.Contains(termToOption[variables[r]]))) {
                        weight = 1000;
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

                // TODO: The following line works for z3Solver version >= 4.6.0
                //solver.Set (RANDOM_SEED, z3RandomSeed);
                Params solverParameter = z3Context.MkParams ();
                solverParameter.Add (RANDOM_SEED, z3RandomSeed);
                solver.Parameters = solverParameter;

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


        public List<BinaryOption> GenerateConfigurationFromBucket(VariabilityModel vm, int numberSelectedFeatures, Dictionary<List<BinaryOption>, int> featureWeight, Configuration lastSampledConfiguration)
        {
            if (_z3Cache == null)
            {
                _z3Cache = new Dictionary<int, Z3Cache>();
            }

            List<KeyValuePair<List<BinaryOption>,int>> featureRanking = featureWeight.ToList();
            featureRanking.Sort((first, second) => first.Value.CompareTo(second.Value));

            List<BoolExpr> variables = null;
            Dictionary<BoolExpr, BinaryOption> termToOption = null;
            Dictionary<BinaryOption, BoolExpr> optionToTerm = null;
            Tuple<Context, BoolExpr> z3Tuple;
            Context z3Context;
            Microsoft.Z3.Solver solver;

            // Reuse the solver if it is already in the cache
            if (this._z3Cache.Keys.Contains(numberSelectedFeatures))
            {
                Z3Cache cache = this._z3Cache[numberSelectedFeatures];
                z3Context = cache.GetContext();
                solver = cache.GetSolver();
                variables = cache.GetVariables();
                termToOption = cache.GetTermToOptionMapping();
                optionToTerm = cache.GetOptionToTermMapping();

                if (lastSampledConfiguration != null)
                {
                    // Add the previous configurations as constraints
                    solver.Assert(Z3Solver.NegateExpr(z3Context, Z3Solver.ConvertConfiguration(z3Context, lastSampledConfiguration.getBinaryOptions(BinaryOption.BinaryValue.Selected), optionToTerm, vm)));

                    // Create a new backtracking point for the next run
                    solver.Push();
                }

            } else
            {
                z3Tuple = Z3Solver.GetInitializedBooleanSolverSystem(out variables, out optionToTerm, out termToOption, vm);
                z3Context = z3Tuple.Item1;
                BoolExpr z3Constraints = z3Tuple.Item2;
                solver = z3Context.MkSolver();

                // TODO: The following line works for z3Solver version >= 4.6.0
                //solver.Set (RANDOM_SEED, z3RandomSeed);
                Params solverParameter = z3Context.MkParams ();
                solverParameter.Add (RANDOM_SEED, z3RandomSeed);
                solver.Parameters = solverParameter;

                solver.Assert(z3Constraints);

                if (lastSampledConfiguration != null)
                {
                    // Add the previous configurations as constraints
                    solver.Assert(Z3Solver.NegateExpr(z3Context, Z3Solver.ConvertConfiguration(z3Context, lastSampledConfiguration.getBinaryOptions(BinaryOption.BinaryValue.Selected), optionToTerm, vm)));
                }

                // The goal of this method is, to have an exact number of features selected

                // Therefore, initialize an integer array with the value '1' for the pseudo-boolean equal function
                int[] neutralWeights = new int[variables.Count];
                for (int i = 0; i < variables.Count; i++)
                {
                    neutralWeights[i] = 1;
                }
                solver.Assert(z3Context.MkPBEq(neutralWeights, variables.ToArray(), numberSelectedFeatures));

                // Create a backtracking point before adding the optimization goal
                solver.Push();

                this._z3Cache[numberSelectedFeatures] = new Z3Cache(z3Context, solver, variables, optionToTerm, termToOption);
            }
            
            // Check if there is still a solution available by finding the first satisfiable configuration
            if (solver.Check() == Status.SATISFIABLE)
            {
                Model model = solver.Model;
                List<BinaryOption> possibleSolution = RetrieveConfiguration(variables, model, termToOption);

                // Disable finding a configuration where the least frequent feature/feature combinations are selected
                // if no featureWeight is given.
                List<BinaryOption> approximateOptimal = null;
                if (featureRanking.Count != 0)
                {
                    approximateOptimal = WeightMinimizer
                    .getSmallWeightConfig(featureRanking, this._z3Cache[numberSelectedFeatures], vm);
                }

                if (approximateOptimal == null)
                {
                    return possibleSolution;
                } else
                {
                    return approximateOptimal;
                }

            } else
            {
                return null;
            }            
        }

        /// <summary>
        /// Clears the cache needed for an optimization.
        /// </summary>
        public void ClearCache()
        {
            this._z3Cache = null;
        }
    }
}
