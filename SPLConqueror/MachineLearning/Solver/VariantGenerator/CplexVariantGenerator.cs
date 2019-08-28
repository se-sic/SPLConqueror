using ILOG.Concert;
using ILOG.CPLEX;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Solver
{
    public class CplexVariantGenerator : IVariantGenerator
    {
        Dictionary<BinaryOption, INumVar> binOptsToCplexVars = new Dictionary<BinaryOption, INumVar>();
        Dictionary<NumericOption, INumVar> numOptsToCplexVars = new Dictionary<NumericOption, INumVar>();

        private Dictionary<int, Cplex> cplexCache = new Dictionary<int, Cplex>();

        private Dictionary<int, IObjective> objCache = new Dictionary<int, IObjective>();

        private IIntExpr one;

        /* Threshold below which we consider values to be 0, due to inaccuracy during the calculation */
        private const double EPSILON_THRESHOLD = 0.01;

        /* CPlex is limited in the number of solutions that are generated in the solution pool
           Since Int32.max is too high we set a hard limit to 2.1 billion solutions */
        private const int MAX_NUMBER_SOLUTION = 2100000000;

        private Cplex initCplex(VariabilityModel vm)
        {
            Cplex cplex = new Cplex();
            one = cplex.Constant(1);
            initializeBinaryVariables(cplex, vm);
            return cplex;
        }

        private INumVar[] populateArray(INumVar current, List<ConfigurationOption> others)
        {
            INumVar[] alternativeGroup = new INumVar[1 + others.Count];
            alternativeGroup[0] = current;
            for (int i = 1; i <= others.Count; i++)
                alternativeGroup[i] = binOptsToCplexVars[(BinaryOption)others.ElementAt(i - 1)];
            return alternativeGroup;
        }

        private void initializeNumericVariables(Cplex plex, VariabilityModel vm)
        {
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                // Initialize with the numeric options as numeric variables with a range from min to max
                INumVar curr = plex.NumVar(numOpt.Min_value, numOpt.Max_value, NumVarType.Float);

         /*       plex.IfThen(
                    plex.Eq(binOptsToCplexVars[vm.Root], 1), 
                    plex.Or(new IConstraint[] { plex.Ge(curr, numOpt.Min_value),
                                                    plex.Le(curr, numOpt.Max_value)}
                    )
                ); */

                numOptsToCplexVars[numOpt] = curr;
                List<double> values = numOpt.getAllValues();
                IConstraint[] valueSet = new IConstraint[values.Count];
                // Limit the values numeric options can have to the precomputed valid values
                for (int i = 0; i < values.Count; i++)
                    valueSet[i] = plex.Eq(curr, values.ElementAt(i));
                plex.Add(plex.Or(valueSet));
            }
        }


        private void initializeBinaryVariables(Cplex cplex, VariabilityModel vm)
        {
            // Add the binary variables
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                // Limit mandatory options to the value of 1
                // Any option that has excluded options is not mandatory even if optional
                // is set to false
                if (binOpt.Optional == false && binOpt.Excluded_Options.Count == 0)
                    binOptsToCplexVars[binOpt] = cplex.NumVar(1, 1, NumVarType.Bool);
                else
                    binOptsToCplexVars[binOpt] = cplex.NumVar(0, 1, NumVarType.Bool);
            }

            HashSet<ConfigurationOption> alreadyHandled = new HashSet<ConfigurationOption>();

            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                INumVar curr = binOptsToCplexVars[binOpt];
                // Add parent implication
                if (binOpt.Parent != null)
                {
                    INumVar parent = binOptsToCplexVars[(BinaryOption)binOpt.Parent];

                    cplex.Add(cplex.IfThen(cplex.Eq(1, curr), cplex.Eq(1, parent)));
                }

                // Add exclusions
                if (binOpt.Excluded_Options.Count > 0)
                {
                    List<ConfigurationOption> alternatives = binOpt.collectAlternativeOptions();
                    if (alternatives.Count > 0 && !alreadyHandled.Contains(binOpt))
                    {
                        alternatives.ForEach(x => alreadyHandled.Add(x));
                        alreadyHandled.Add(binOpt);
                        // Initialize a constraint that states that exactly one of the group has to be
                        // selected
                        if (binOpt.Parent != null)
                        {
                            initializeExclusionConstraint(cplex, binOpt, curr, alternatives,
                                binOptsToCplexVars[(BinaryOption)binOpt.Parent]);
                        }
                        else
                            initializeExclusionConstraint(cplex, binOpt, curr, alternatives, one);
                    }

                    List<List<ConfigurationOption>> nonAlternativeExclusives = 
                        binOpt.getNonAlternativeExlcudedOptions();
                    foreach (List<ConfigurationOption> nonAlternativeExclusiveGroup in nonAlternativeExclusives)
                    {
                        // Initialize constraint that states that if curr is selected all nonAlternative exclusives
                        // have to be deselected
                        initializeExclusionConstraint(cplex, binOpt, curr, nonAlternativeExclusiveGroup, curr);
                    }

                }

                // Add implications
                initializeImpliedOptions(cplex, curr, binOpt);
            }

            initializeArbitraryBooleanConstraints(cplex, vm, binOptsToCplexVars);

        }

        private void initializeExclusionConstraint(Cplex cplex, BinaryOption currentBinOpt, INumVar current,
            List<ConfigurationOption> notAllowed, INumExpr trigger)
        {
            INumVar[] excluded = populateArray(current, notAllowed);

            // If there is some kind of exclusiion then the sum of the group has to be one if the 
            // trigger(either the parent for alternative groups or the current option for cross tree exclusions)
            // is selected
            cplex.Add(cplex.IfThen(
                                    cplex.Eq(one, trigger),
                                    cplex.Eq(one, cplex.Sum(excluded))
            ));
        }

        private void initializeImpliedOptions(Cplex cplex, INumVar currentOption, BinaryOption currentBinOpt)
        {
            List<List<ConfigurationOption>> impliedOptions = currentBinOpt.Implied_Options;
            foreach (List<ConfigurationOption> impliedGroup in impliedOptions)
            {
                INumVar[] implVars = populateArray(currentOption, impliedGroup);

                // For implication groups the sum of the options has to be the number of implied options
                // if the current options is selected
                cplex.Add(cplex.IfThen(
                                        cplex.Eq(one, currentOption),
                                        cplex.Eq(cplex.Sum(implVars), cplex.Constant(implVars.Count()))
                ));
            }
        }

        private void initializeArbitraryBooleanConstraints(Cplex cplex, VariabilityModel vm,
            Dictionary<BinaryOption, INumVar> optsToCplex)
        {
            foreach (string booleanConstraint in vm.BinaryConstraints)
            {
                string[] cnfParts = booleanConstraint.Split('&');

                foreach (string cnfPart in cnfParts)
                {
                    string[] variables = cnfPart.Split('|');
                    INumExpr[] logicOrConstr = new INumExpr[variables.Length];

                    for (int i = 0; i < variables.Length; i++)
                    {

                        string var = variables[i].Trim();
                        // In binary domain (1 - x) equals the negation of x 
                        if (var.StartsWith("!") || var.StartsWith("-"))
                            logicOrConstr[i] = cplex.Sum(1, cplex.Negative((optsToCplex[vm.getBinaryOption(var.Substring(1))])));
                        else
                            logicOrConstr[i] = optsToCplex[vm.getBinaryOption(var)];
                    }

                    // Since we use cnf notation, it is enough to check if the sum of a single clause is
                    // greater or equal 1
                    cplex.AddGe(cplex.Sum(logicOrConstr), one);
                }
            }
        }

        public void ClearCache()
        {
            cplexCache.Keys.ToList().ForEach(x => cplexCache[x].Dispose());
            cplexCache.Clear();
            objCache.Clear();
        }

        public List<List<BinaryOption>> DistanceMaximization(VariabilityModel vm, List<BinaryOption> minimalConfiguration, int numberToSample, int optionWeight)
        {
            Cplex plex = initCplex(vm);
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            // Set up a solution pool with a maximum size of numberToSample and set the replacement strategy to
            // replacing least diverse solutions and generate #BinaryOptions * number of wanted samples solutions
            plex.SetParam(Cplex.Param.MIP.Pool.Capacity, numberToSample);
            plex.SetParam(Cplex.Param.MIP.Pool.Intensity, 4);
            plex.SetParam(Cplex.Param.MIP.Pool.AbsGap, 0.0);
            plex.SetParam(Cplex.Param.MIP.Limits.Populate, vm.BinaryOptions.Count * numberToSample);
            plex.SetParam(Cplex.Param.MIP.Pool.Replace, 2);
            plex.Populate();

            for (int i = 0; i < plex.GetSolnPoolNsolns(); i++)
            {
                List<BinaryOption> solution = new List<BinaryOption>();
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    if (plex.GetValue(binOptsToCplexVars[binOpt], i) > EPSILON_THRESHOLD)
                        solution.Add(binOpt);
                }
                results.Add(solution);
            }

            plex.Dispose();
            return results;
        }

        public List<Configuration> GenerateAllVariants(VariabilityModel vm, List<ConfigurationOption> optionsToConsider)
        {

            HashSet<Configuration> results = new HashSet<Configuration>();
            Cplex plex = initCplex(vm);
            initializeNumericVariables(plex, vm);
            setPopulateParams(plex, MAX_NUMBER_SOLUTION);
            populate(plex, MAX_NUMBER_SOLUTION);

            int nSols = plex.GetSolnPoolNsolns();
            for (int i = 0; i < plex.GetSolnPoolNsolns(); i++)
            {
                Dictionary<BinaryOption, BinaryOption.BinaryValue> binOpts = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
                Dictionary<NumericOption, double> numOpts = new Dictionary<NumericOption, double>();
                Configuration toAdd;

                foreach (ConfigurationOption option in vm.getOptions())
                {
                    if (optionsToConsider != null && !optionsToConsider.Contains(option))
                        continue;

                    if (option is BinaryOption)
                    {
                        if (plex.GetValue(binOptsToCplexVars[(BinaryOption)option], i) > EPSILON_THRESHOLD)
                            binOpts[(BinaryOption)option] = BinaryOption.BinaryValue.Selected;
                        else
                            binOpts[(BinaryOption)option] = BinaryOption.BinaryValue.Deselected;
                    }
                    else
                    {
                        numOpts[(NumericOption)option] = plex.GetValue(numOptsToCplexVars[(NumericOption)option], i);
                    }
                }

                toAdd = new Configuration(binOpts, numOpts);
                // Check if the non-boolean constraints are satisfied
                if (!results.Contains(toAdd) && vm.configurationIsValid(toAdd) 
                        && vm.MixedConstraints.TrueForAll(constr => constr.requirementsFulfilled(toAdd)))
                        results.Add(toAdd);
            }

            plex.Dispose();
            return results.ToList();
        }

        public List<List<BinaryOption>> GenerateAllVariantsFast(VariabilityModel vm)
        {
            return GenerateUpToNFast(vm, MAX_NUMBER_SOLUTION);
        }

        public List<BinaryOption> GenerateConfigurationFromBucket(VariabilityModel vm, int numberSelectedFeatures, Dictionary<List<BinaryOption>, int> featureWeight, Configuration lastSampledConfiguration)
        {
            Cplex plex;

            List<BinaryOption> solution = new List<BinaryOption>();

            if (cplexCache[numberSelectedFeatures] == null)
            {
                plex = initCplex(vm);
                countConstraint(plex, vm.BinaryOptions, numberSelectedFeatures);
                cplexCache[numberSelectedFeatures] = plex;
            } else
            {
                plex = cplexCache[numberSelectedFeatures];

                // Set intensity to store only litte information to generate few configurations fast
                plex.SetParam(Cplex.Param.MIP.Pool.Intensity, 1);
                plex.SetParam(Cplex.Param.MIP.Pool.Capacity, numberSelectedFeatures < 31 ? (int)Math.Pow(2, numberSelectedFeatures) : 2100000000);
                // Set replacement to FIFO
                plex.SetParam(Cplex.Param.MIP.Pool.Replace, 1);

                plex.Remove(objCache[numberSelectedFeatures]);
            }

            // Solution pool will take care of the last sampled configuration and try to generate new configurations

            if (featureWeight != null)
            {
                List<List<BinaryOption>> features = featureWeight.Keys.ToList(); 
                double[] weights = new double[features.Count];
                INumExpr linearSum = null;
                for (int i = 0; i < features.Count; i++)
                {
                    List<BinaryOption> feature = features.ElementAt(i);
                    INumExpr previousMul = null;
                    weights[i] = featureWeight[feature];
                    foreach (BinaryOption option in feature)
                    {
                        if (previousMul == null)
                            previousMul = binOptsToCplexVars[option];
                        else
                            previousMul = plex.Prod(previousMul, binOptsToCplexVars[option]);
                    }
                    previousMul = plex.Prod(featureWeight[feature], previousMul);

                    if (linearSum == null)
                        linearSum = previousMul;
                    else
                        linearSum = plex.Sum(previousMul, linearSum);
                }

                IObjective obj = plex.Objective(ObjectiveSense.Minimize, linearSum);
                plex.Add(obj);
                objCache[numberSelectedFeatures] = obj;
            }

            if (plex.Solve())
            {
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    int nSolns = plex.GetSolnPoolNsolns();
                    if (plex.GetValue(binOptsToCplexVars[binOpt], nSolns - 1) > EPSILON_THRESHOLD)
                        solution.Add(binOpt);
                }
            }


            return solution;
        }

        private IConstraint addConfigurationPartToConstraint(IConstraint previous, List<INumVar> toAdd, Cplex plex)
        {
            foreach (INumVar var in toAdd)
            {
                if (previous == null)
                {

                    previous = plex.Eq(1, var);
                }
                else
                {
                    previous = plex.And(previous, plex.Eq(1, var));
                }
            }

            if (previous != null)
            {
                previous = plex.Not(previous);
            }
            return previous;
        }


        public List<BinaryOption> GenerateConfigWithoutOption(BinaryOption optionToBeRemoved, List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm)
        {
            List<BinaryOption> result = new List<BinaryOption>();
            Cplex plex = initCplex(vm);
            List<BinaryOption> blackListed = new List<BinaryOption>();
            blackListed.Add(optionToBeRemoved);
            initializeBlacklist(plex, blackListed);
            initializeMinMaxObjective(vm, plex, true, originalConfig, blackListed);

            if (plex.Solve())
            {
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    if (plex.GetValue(binOptsToCplexVars[binOpt]) > EPSILON_THRESHOLD)
                    {
                        result.Add(binOpt);
                    }
                }
            }
            removedElements = originalConfig.Except(result).ToList();
            plex.Dispose();
            return result;
        }

        private void setPopulateParams(Cplex plex, int n)
        {
            // Set parameters to ensure as much as possible solutions are generated
            plex.SetParam(Cplex.Param.MIP.Pool.Intensity, 4);
            plex.SetParam(Cplex.Param.MIP.Pool.AbsGap, 0.0);
            plex.SetParam(Cplex.Param.MIP.Pool.Capacity, n);
            plex.SetParam(Cplex.Param.MIP.Limits.Populate, n);
        }

        private int populate(Cplex plex, int n)
        {
            int nSolutions = 0;
            int delta = 1;
            // generate - not necessarily optimal - solutions until there are no more or we have
            // reached the necessary amount or the maximum number of supporteed
            while (plex.Populate() && delta != 0 && nSolutions < n)
            {
                int tmp = plex.GetSolnPoolNsolns();
                delta = tmp - nSolutions;
                nSolutions = tmp;
            }

            return nSolutions;
        }

        public List<List<BinaryOption>> GenerateUpToNFast(VariabilityModel vm, int n)
        {

            Cplex plex = initCplex(vm);
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            setPopulateParams(plex, n);
            int nSolutions = populate(plex, n);

            for (int i = 0; i < Math.Min(nSolutions, n); i++)
            {
                List<BinaryOption> solution = new List<BinaryOption>();
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    if (plex.GetValue(binOptsToCplexVars[binOpt], i) > EPSILON_THRESHOLD)
                        solution.Add(binOpt);
                }
                results.Add(solution);
            }
            plex.Dispose();
            return results;
        }

        public List<List<BinaryOption>> MaximizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unwantedOptions)
        {
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            Cplex plex = initCplex(vm);
            initializeMinMaxObjective(vm, plex, minimize, config, unwantedOptions);
            initializeRequire(plex, config);

            double currentObjValue = Double.MaxValue;
            while (plex.Solve())
            {
                if (plex.GetObjValue() > currentObjValue)
                    break;
                else
                    currentObjValue = plex.GetObjValue();

                List<BinaryOption> optimalSolution = new List<BinaryOption>();
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    if (plex.GetValue(binOptsToCplexVars[binOpt]) > EPSILON_THRESHOLD)
                    {
                        optimalSolution.Add(binOpt);
                    }
                }
                results.Add(optimalSolution);
            }

            plex.Dispose();
            return results;

        }

        public List<BinaryOption> MinimizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unWantedOptions)
        {
            List<BinaryOption> result = new List<BinaryOption>();
            Cplex plex = initCplex(vm);
            initializeMinMaxObjective(vm, plex, minimize, config, unWantedOptions);
            initializeRequire(plex, config);

            if (plex.Solve())
            {
                foreach (BinaryOption binOpt in vm.BinaryOptions)
                {
                    if (plex.GetValue(binOptsToCplexVars[binOpt]) > EPSILON_THRESHOLD)
                        result.Add(binOpt);
                }
            }

            plex.Dispose();
            return result;
        }

        private void initializeRequire(Cplex plex, List<BinaryOption> required)
        {
            countConstraint(plex, required, required.Count);
        }

        private void initializeBlacklist(Cplex plex, List<BinaryOption> blacklisted)
        {
            countConstraint(plex, blacklisted, 0.0);
        }

        private void countConstraint(Cplex plex, List<BinaryOption> options, double count)
        {
            INumVar[] blacklistedVars = new INumVar[options.Count];
            for (int i = 0; i < options.Count; i++)
                blacklistedVars[i] = binOptsToCplexVars[options.ElementAt(i)];

            plex.Add(plex.Eq(
                plex.Constant(count),
                plex.Sum(blacklistedVars)
            ));
        }

        private void initializeMinMaxObjective(VariabilityModel vm, Cplex plex, bool minimize,
            List<BinaryOption> wanted, List<BinaryOption> unwanted)
        {
            INumVar[] variables = new INumVar[vm.BinaryOptions.Count];
            double[] weights = new double[vm.BinaryOptions.Count];
            for (int i = 0; i < vm.BinaryOptions.Count; i++)
            {
                BinaryOption curr = vm.BinaryOptions.ElementAt(i);
                variables[i] = binOptsToCplexVars[curr];
                if (wanted != null && wanted.Contains(curr))
                {
                    weights[i] = -100.0;
                }
                else if (unwanted != null && unwanted.Contains(curr))
                {
                    weights[i] = 100.0;
                }
                else
                {
                    weights[i] = minimize ? 100.0 : -100.0;
                }
            }
            ILinearNumExpr weightedVariables = plex.ScalProd(variables, weights);
            IObjective objective = plex.Minimize(weightedVariables);
            plex.Add(objective);
        }
    }
}
