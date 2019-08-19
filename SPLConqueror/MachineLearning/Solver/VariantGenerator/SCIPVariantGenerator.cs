using MachineLearning.Solver.scip;
using SCIPExternal;
using SCIPInternal;
using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MachineLearning.Solver
{
    // TODO whole class if fully experimental and not really suited for the tasks of the variant generator
    public class SCIPVariantGenerator : IVariantGenerator
    {
        private SCIP scip;
        private Var oneConstant;
        private Dictionary<BinaryOption, Var> binaryProblemVariables;
        private Dictionary<NumericOption, Var> numericProblemVariables;

        private Dictionary<String, int> numericOptionToPseudoOptionID = new Dictionary<string, int>();
        private Dictionary<string, Tuple<ConfigurationOption, int>> nameToOptionID = 
            new Dictionary<string, Tuple<ConfigurationOption, int>>();

        public SCIPVariantGenerator()
        {
        }

        private Dictionary<BinaryOption, Var> initializeBinaryVariables(VariabilityModel vm, SCIP problem)
        {
            oneConstant = new Var(problem, "oneConstant", 1.0, 1.0, 0, Vartype.SCIP_VARTYPE_INTEGER);
            Dictionary<BinaryOption, Var> variables = new Dictionary<BinaryOption, Var>();
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                Var var;
                // Mandatory
                // Note if option has excluded options it is automatically interpreted as optional 
                // even if outside of alternative group.
                if (binOpt.Optional == false && binOpt.Excluded_Options.Count == 0)
                {
                    // create Instances of Var objects with the constructor: public Var(SCIP scip, string name,
                    // double lowerBound, double upperBound, double objectiveValue, Vartype vartype)
                    var = new Var(problem, binOpt.Name, 1.0, 1.0, 0, Vartype.SCIP_VARTYPE_BINARY);
                // Optional
                }
                else
                {
                    // create Instances of Var objects with the constructor: public Var(SCIP scip, string name, 
                    // double lowerBound, double upperBound, double objectiveValue, Vartype vartype)
                    var = new Var(problem, binOpt.Name, 0.0, 1.0, 0, Vartype.SCIP_VARTYPE_BINARY);
                }
                variables[binOpt] = var;
            }
            return variables;
        }

        private void initializeBinaryParentChildConstraints(VariabilityModel vm, SCIP problem)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                if (binOpt.Parent != null)
                {
                    Var[] variables = new Var[2];
                    variables[0] = binaryProblemVariables[binOpt];
                    variables[1] = binaryProblemVariables[(BinaryOption)binOpt.Parent];
                    double[] coeffs = { 1, -1 };
                    if (binOpt.Optional == false && binOpt.Excluded_Options.Count == 0)
                    {
                        // 0 <= child - parent <= 0
                        // both options imply each other. Either both are selected or none are selected
                        new LinearConstraint( problem,
                            binOpt.Name + "<->" + binOpt.Parent.Name, variables.Length, variables, coeffs, 0.0, 0.0);
                    }
                    else
                    {
                        // -1 <= child - parent <= 0
                        // Selection of child implies selection of parent
                        new LinearConstraint(problem, 
                            binOpt.Name + "->" + binOpt.Parent.Name, variables.Length, variables, coeffs, -1.0, 0.0);
                    }
                }
            }
        }

        private void initializeBinaryImplications(VariabilityModel vm, SCIP problem)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                if (binOpt.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> implied in binOpt.Implied_Options)
                    {
                        Var[] implicationsVars = new Var[implied.Count + 1];
                        implicationsVars[0] = binaryProblemVariables[binOpt].negate(problem);
                        for (int i = 1; i <= implied.Count; i++)
                            implicationsVars[i] = binaryProblemVariables[(BinaryOption)implied.ElementAt(i - 1)];
                        new LogicOrConstraint(problem, "impl" + binOpt.Name, implicationsVars.Count(), implicationsVars);

                    }
                }
            }
        }

        private void initializeArbitraryBooleanConstraints(VariabilityModel vm, SCIP problem)
        {
            int i = 0;
            foreach (string booleanConstraint in vm.BinaryConstraints)
            {
                string[] cnfExpressions = booleanConstraint.Split('&');

                foreach (string cnfExpression in cnfExpressions)
                {
                    string[] variables = cnfExpression.Split('|');
                    Var[] orExprVars = new Var[variables.Count()];
                    int counter = 0;

                    foreach (string variable in variables)
                    {
                        if (variable.StartsWith("-") || variable.StartsWith("!"))
                        {
                            orExprVars[counter] = binaryProblemVariables[vm.getBinaryOption(variable.Substring(1))].negate(problem);
                        }
                        else
                        {
                            orExprVars[counter] = binaryProblemVariables[vm.getBinaryOption(variable)];
                        }
                        counter++;
                    }
                    new LogicOrConstraint(problem, "booleanConstr" + i++, orExprVars.Count(), orExprVars);
                }
            }
        }

        // Initialize alternative groups
        private void initializeBinaryAlternativeGroups(VariabilityModel vm, SCIP problem)
        {
            HashSet<BinaryOption> alreadyHandledGroup = new HashSet<BinaryOption>();
            foreach (BinaryOption binOpt in vm.BinaryOptions.Where(x => !x.Optional))
            {
                IEnumerable<BinaryOption> alternatives = binOpt.collectAlternativeOptions()
                    .Select(opt => (BinaryOption)opt);
                if (alternatives.Count() > 0 && !alreadyHandledGroup.Contains(binOpt))
                {
                    Var[] vars;
                    double[] coeffs;
                    vars = new Var[alternatives.Count() + 2];

                    if (binOpt.Parent != null)
                        vars[0] = binaryProblemVariables[(BinaryOption)binOpt.Parent];
                    else
                        vars[0] = oneConstant;

                    vars[1] = binaryProblemVariables[binOpt];
                    coeffs = new double[vars.Length];
                    coeffs[0] = 1;
                    for (int i = 1; i < coeffs.Length; i++)
                        coeffs[i] = -1;

                    for (int i = 2; i <= alternatives.Count() + 1; i++)
                        vars[i] = binaryProblemVariables[alternatives.ElementAt(i - 2)];
                    // create instance of LinearConstraint with constructor:
                    // LinearConstraint(SCIP scip, string name, int nVars, Var[] vars, double[] vals, double lhs, double rhs) 
                    // creates constraint in the form of 0 <= parent - sum_of_alternative_group <= 0 so at most
                    // 1 option in the group is selected 
                    new LinearConstraint(problem, "alt-" + binOpt.Name, vars.Length, vars, coeffs, 0.0, 0.0);
                    alreadyHandledGroup.Add(binOpt);
                    foreach (BinaryOption alternative in alternatives)
                        alreadyHandledGroup.Add(alternative);
                }
            }
        }

        // handle not alternative group excluded options
        private void initializeBinaryNonAlternativeExcluded(SCIP problem, VariabilityModel vm)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                foreach (List<ConfigurationOption> alternativeGroup in binOpt.getNonAlternativeExlcudedOptions())
                {
                    Var[] vars = new Var[alternativeGroup.Count + 1];
                    double[] coeffs = new double[vars.Length];
                    vars[0] = binaryProblemVariables[binOpt];
                    coeffs[0] = 1;
                    for (int i = 1; i <= alternativeGroup.Count; i++)
                    {
                        vars[i] = binaryProblemVariables[(BinaryOption)alternativeGroup.ElementAt(i - 1)];
                        coeffs[i] = 1;
                    }
                    // creates constraint in the form of 1 <= sum_of_exclusive_group <= 1 so at most 
                    // 1 option in the group is selected 
                    new LinearConstraint(problem, "excl-" + binOpt.Name, vars.Length, vars, coeffs, 1.0, 1.0);
                }
            }
        }

        private SCIP initializeBinaryConstraintSystem(VariabilityModel vm)
        {
            // create Instance of scip object with the constructor: public SCIP(bool useDefaultPlugins, string problemName)
            SCIP problem = new SCIP(true, vm.Name);
            binaryProblemVariables = initializeBinaryVariables(vm, problem);
            initializeBinaryParentChildConstraints(vm, problem);
            initializeBinaryAlternativeGroups(vm, problem);
            initializeBinaryNonAlternativeExcluded(problem, vm);
            initializeBinaryImplications(vm, problem);
            initializeArbitraryBooleanConstraints(vm, problem);
            return problem;
        }

        private void addNumericOptionsToConstraintSystem(VariabilityModel vm, SCIP problem)
        {
            numericProblemVariables = new Dictionary<NumericOption, Var>();
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                Var var = new Var(problem, numOpt.Name, numOpt.Min_value, numOpt.Max_value, 0, Vartype.SCIP_VARTYPE_CONTINUOUS);
                numericProblemVariables[numOpt] = var;
                // Parent can be ignored because we dont support optional numeric configuration options
                Var temp = new Var(problem, "tmp" + numOpt.Name, Math.Floor(numOpt.Min_value), Math.Ceiling(numOpt.Max_value), 0, Vartype.SCIP_VARTYPE_CONTINUOUS);
                // To model the step constraint in any efficient manner it has to be assumed that
                // a numeric option can only be a multiple of its initial value
                Var[] stepConstraint = new Var[2];
                stepConstraint[0] = var;
                stepConstraint[1] = temp;
                double[] coeffs = new double[2];
                coeffs[0] = 1;
                coeffs[1] = -numOpt.getAllValues().Min(x => x);
                new LinearConstraint(problem, "step-" + numOpt.Name, stepConstraint.Length, stepConstraint, coeffs, 0.0, 0.0);
            }
        }

        public Configuration findOptimalSolutionOfInfModel(InfluenceFunction infMod, VariabilityModel vm,
            string additionalConstraintFile, bool maximization)
        {
            OsilGenerator generator = new OsilGenerator();
            string osilContent = generator.generateOsil_Syntax(vm, infMod, additionalConstraintFile,
                numericOptionToPseudoOptionID, nameToOptionID, maximization);
            string tmpFileName = Path.GetTempPath() + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".osil";
            File.AppendAllText(tmpFileName, osilContent);
            //solver = new SCIP(tmpFileName, "osil");
            scip = new SCIP(tmpFileName, "osil");
            File.Delete(tmpFileName);

            Var[] problemVariables = scip.getVars();
            scip.solve();

            if (scip.getNSols() == 0)
            {
                return null;
            }
            else
            {
                Sol bestSolution = scip.getBestSol();
                List<BinaryOption> bestBinOpts = new List<BinaryOption>();
                Dictionary<NumericOption, double> bestNumOpts = new Dictionary<NumericOption, double>();
                foreach (Var problemVariable in problemVariables)
                {
                    double value = bestSolution.getSolVal(problemVariable);
                    ConfigurationOption opt = vm.getOption(problemVariable.Name);
                    if (opt == null)
                    {
                        continue;
                    }
                    else if (opt is BinaryOption && value == 1)
                    {
                        bestBinOpts.Add((BinaryOption)opt);
                    }
                    else if (opt is NumericOption)
                    {
                        bestNumOpts.Add((NumericOption)opt, value);
                    }
                }
                return new Configuration(bestBinOpts, bestNumOpts);
            }
        }

        public List<List<BinaryOption>> DistanceMaximization(VariabilityModel vm,
            List<BinaryOption> minimalConfiguration, int numberToSample, int optionWeight)
        {
            throw new NotImplementedException();
        }

        public List<Configuration> GenerateAllVariants(VariabilityModel vm, 
            List<ConfigurationOption> optionsToConsider)
        {
            throw new NotImplementedException();
        }

        public List<List<BinaryOption>> GenerateAllVariantsFast(VariabilityModel vm)
        {
            return GenerateUpToNFast(vm, -1);
        }

        private List<List<BinaryOption>> generateUpToNWithGivenProblem(VariabilityModel vm, 
            int n, SCIP problem)
        {
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            bool hasSol = true;
            while (hasSol && (n != 0))
            {
                SCIP copy = new SCIP(problem);
                Var[] vars = copy.getVars();

                copy.solve();
                if (copy.getNSols() == 0)
                {
                    hasSol = false;
                    continue;
                }

                Sol solution = copy.getBestSol();
                List<BinaryOption> configuration = parseSolution(vm, vars, solution);

                Var[] blacklist = new Var[vm.BinaryOptions.Count];
                for (int i = 0; i < vm.BinaryOptions.Count; i++)
                {
                    if (configuration.Contains(vm.BinaryOptions.ElementAt(i)))
                    {
                        blacklist[i] = binaryProblemVariables[vm.BinaryOptions.ElementAt(i)].negate(problem);
                    }
                    else
                    {
                        blacklist[i] = binaryProblemVariables[vm.BinaryOptions.ElementAt(i)];
                    }
                }
                new LogicOrConstraint(problem, "blacklist" + n, blacklist.Count(), problem.unboxVars(blacklist));
                results.Add(configuration);

                n--;
            }
            return results;
        }

        public List<List<BinaryOption>> GenerateUpToNFast(VariabilityModel vm, int n)
        {
            SCIP problem = initializeBinaryConstraintSystem(vm);
            return generateUpToNWithGivenProblem(vm, n, problem);
        }

        private void updateObjectiveValues(VariabilityModel vm, List<BinaryOption> unWantedOptions,
            List<BinaryOption> neededOptions, bool minimize)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                if (unWantedOptions.Contains(binOpt))
                {
                    binaryProblemVariables[binOpt].changeObjectiveValue(100);
                }
                else if (neededOptions.Contains(binOpt))
                {
                    binaryProblemVariables[binOpt].changeObjectiveValue(-100);
                }
                else
                {
                    binaryProblemVariables[binOpt].changeObjectiveValue(minimize ? 100 : -100);
                }
            }
        }

        public List<BinaryOption> MinimizeConfig(List<BinaryOption> config, VariabilityModel vm,
            bool minimize, List<BinaryOption> unWantedOptions)
        {
            SCIP problem = initializeBinaryConstraintSystem(vm);
            updateObjectiveValues(vm, unWantedOptions, config, minimize);
            return generateUpToNWithGivenProblem(vm, 1, problem).First();
        }

        private List<BinaryOption> parseSolution(VariabilityModel vm, Var[] vars, Sol solution)
        {
            List<BinaryOption> configuration = new List<BinaryOption>();
            for (int i = 0; i < vars.Length; i++)
            {
                if (solution.getSolVal(vars[i]) == 1)
                {
                    BinaryOption toAdd = vm.getBinaryOption(vars[i].Name);
                    if (toAdd != null)
                        configuration.Add(vm.getBinaryOption(vars[i].Name));
                }
            }
            return configuration;
        }

        public List<List<BinaryOption>> MaximizeConfig(List<BinaryOption> config, VariabilityModel vm,
            bool minimize, List<BinaryOption> unwantedOptions)
        {
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            SCIP problem = initializeBinaryConstraintSystem(vm);
            updateObjectiveValues(vm, unwantedOptions, config, minimize);

            Var[] vars = problem.getVars();

            problem.solve();
            if (problem.getNSols() == 0)
            {
                return results;
            }

            // TODO: might actually not produce all best partial configuarations
            // might need iteration like the up to n version but with abort on configuration with
            // worse objsense
            Sol[] solutions = problem.getSols();

            foreach (Sol solution in solutions)
            {
                results.Add(parseSolution(vm, vars, solution));
            }
            return results;
        }

        public List<BinaryOption> GenerateConfigWithoutOption(BinaryOption optionToBeRemoved, 
            List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm)
        {
            throw new NotImplementedException();
        }

        public List<BinaryOption> GenerateConfigurationFromBucket(VariabilityModel vm, int numberSelectedFeatures,
            Dictionary<List<BinaryOption>, int> featureWeight, Configuration lastSampledConfiguration)
        {
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
        }
    }
}
