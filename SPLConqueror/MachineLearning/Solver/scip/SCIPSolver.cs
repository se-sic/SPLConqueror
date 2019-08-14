using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MachineLearning.Solver.scip
{
    public class SCIPSolver : IVariantGenerator
    {
        private Type VarType;
        private Type SolType;
        private Type ScipType;
        private Type LinearConstraintType;
        private Type LogicOrConstraintType;
        private Dictionary<string, object> varType = new Dictionary<string, object>();
        // Since we load the library dynamically at runtime we have to resort to using the dynamic type
        private dynamic scip;
        private dynamic oneConstant;
        private Dictionary<BinaryOption, dynamic> binaryProblemVariables;
        private Dictionary<NumericOption, dynamic> numericProblemVariables;

        private Dictionary<String, int> numericOptionToPseudoOptionID = new Dictionary<string, int>();
        private Dictionary<string, Tuple<ConfigurationOption, int>> nameToOptionID = 
            new Dictionary<string, Tuple<ConfigurationOption, int>>();

        public SCIPSolver(string interfaceLibPath)
        {
            // Option 2: Use it by dynamically loading at runtime, only requires dlls when actually used
            Assembly scipDll = Assembly.LoadFile(interfaceLibPath + "SharpScip.dll");

            foreach (Type t in scipDll.GetExportedTypes())
            {
                switch (t.Name)
                {
                    case "Var":
                        VarType = t;
                        break;
                    case "SCIP":
                        ScipType = t;
                        break;
                    case "Sol":
                        SolType = t;
                        break;
                    case "Vartype":
                        Array enumVal = t.GetEnumValues();
                        string[] names = t.GetEnumNames();
                        for (int i = 0; i < names.Length; i++)
                            varType[names[i]] = enumVal.GetValue(i);
                        break;
                    case "LinearConstraint":
                        LinearConstraintType = t;
                        break;
                    case "LogicOrConstraint":
                        LogicOrConstraintType = t;
                        break;
                }
            }
        }

        private Dictionary<BinaryOption, dynamic> initializeBinaryVariables(VariabilityModel vm, dynamic problem)
        {
            oneConstant = Activator.CreateInstance(VarType, problem, "oneConstant", 1.0, 1.0, 0,
                varType["SCIP_VARTYPE_INTEGER"]);
            Dictionary<BinaryOption, dynamic> variables = new Dictionary<BinaryOption, dynamic>();
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                dynamic var;
                // Mandatory
                // Note if option has excluded options it is automatically interpreted as optional 
                // even if outside of alternative group.
                if (binOpt.Optional == false && binOpt.Excluded_Options.Count == 0)
                {
                    // create Instances of Var objects with the constructor: public Var(SCIP scip, string name,
                    // double lowerBound, double upperBound, double objectiveValue, Vartype vartype)
                    var = Activator.CreateInstance(VarType, problem, binOpt.Name, 1.0, 1.0, 0,
                        varType["SCIP_VARTYPE_BINARY"]);
                    // Optional
                }
                else
                {
                    // create Instances of Var objects with the constructor: public Var(SCIP scip, string name, 
                    // double lowerBound, double upperBound, double objectiveValue, Vartype vartype)
                    var = Activator.CreateInstance(VarType, problem, binOpt.Name, 0.0, 1.0, 0,
                        varType["SCIP_VARTYPE_BINARY"]);
                }
                variables[binOpt] = var;
            }
            return variables;
        }

        private void initializeBinaryParentChildConstraints(VariabilityModel vm, dynamic problem)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                if (binOpt.Parent != null)
                {
                    // TODO passing dynamic arrays wont work this way
                    dynamic[] variables = new dynamic[2];
                    variables[0] = binaryProblemVariables[binOpt];
                    variables[1] = binaryProblemVariables[(BinaryOption)binOpt.Parent];
                    double[] coeffs = { 1, -1 };
                    if (binOpt.Optional == false && binOpt.Excluded_Options.Count == 0)
                    {
                        // 0 <= child - parent <= 0
                        // both options imply each other. Either both are selected or none are selected
                        Activator.CreateInstance(LinearConstraintType, problem,
                            binOpt.Name + "<->" + binOpt.Parent.Name, variables.Length, problem.unboxVars(variables), coeffs, 0.0, 0.0);
                    }
                    else
                    {
                        // -1 <= child - parent <= 0
                        // Selection of child implies selection of parent
                        Activator.CreateInstance(LinearConstraintType, problem, 
                            binOpt.Name + "->" + binOpt.Parent.Name, variables.Length, problem.unboxVars(variables), coeffs, -1.0, 0.0);
                    }
                }
            }
        }

        private void initializeBinaryImplications(VariabilityModel vm, dynamic problem)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                if (binOpt.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> implied in binOpt.Implied_Options)
                    {
                        dynamic[] implicationsVars = new dynamic[implied.Count + 1];
                        implicationsVars[0] = binaryProblemVariables[binOpt].negate(problem);
                        for (int i = 1; i <= implied.Count; i++)
                            implicationsVars[i] = binaryProblemVariables[(BinaryOption)implied.ElementAt(i - 1)];
                        Activator.CreateInstance(LogicOrConstraintType, problem, "impl" + binOpt.Name,
                            implicationsVars.Count(), implicationsVars);

                    }
                }
            }
        }

        private void initializeArbitraryBooleanConstraints(VariabilityModel vm, dynamic problem)
        {
            int i = 0;
            foreach (string booleanConstraint in vm.BinaryConstraints)
            {
                string[] cnfExpressions = booleanConstraint.Split('&');

                foreach (string cnfExpression in cnfExpressions)
                {
                    string[] variables = cnfExpression.Split('|');
                    dynamic[] orExprVars = new dynamic[variables.Count()];
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
                    Activator.CreateInstance(LogicOrConstraintType, problem, "booleanConstr" + i++,
                    orExprVars.Count(), problem.unboxVars(orExprVars));
                }
            }
        }

        // Initialize alternative groups
        private void initializeBinaryAlternativeGroups(VariabilityModel vm, dynamic problem)
        {
            HashSet<BinaryOption> alreadyHandledGroup = new HashSet<BinaryOption>();
            foreach (BinaryOption binOpt in vm.BinaryOptions.Where(x => !x.Optional))
            {
                IEnumerable<BinaryOption> alternatives = binOpt.collectAlternativeOptions()
                    .Select(opt => (BinaryOption)opt);
                if (alternatives.Count() > 0 && !alreadyHandledGroup.Contains(binOpt))
                {
                    dynamic[] vars;
                    double[] coeffs;
                    // TODO bug
                    vars = new dynamic[alternatives.Count() + 2];

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
                    Activator.CreateInstance(LinearConstraintType, problem, "alt-" + binOpt.Name, vars.Length,
                         problem.unboxVars(vars), coeffs, 0.0, 0.0);
                    alreadyHandledGroup.Add(binOpt);
                    foreach (BinaryOption alternative in alternatives)
                        alreadyHandledGroup.Add(alternative);
                }
            }
        }

        // handle not alternative group excluded options
        private void initializeBinaryNonAlternativeExcluded(dynamic problem, VariabilityModel vm)
        {
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                foreach (List<ConfigurationOption> alternativeGroup in binOpt.getNonAlternativeExlcudedOptions())
                {
                    dynamic[] vars = new dynamic[alternativeGroup.Count + 1];
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
                    Activator.CreateInstance(LinearConstraintType, problem, "excl-" + binOpt.Name, vars.Length,
                         problem.unboxVars(vars), coeffs, 1.0, 1.0);
                }
            }
        }

        private dynamic initializeBinaryConstraintSystem(VariabilityModel vm)
        {
            // create Instance of scip object with the constructor: public SCIP(bool useDefaultPlugins, string problemName)
            dynamic problem = Activator.CreateInstance(ScipType, true, vm.Name);
            binaryProblemVariables = initializeBinaryVariables(vm, problem);
            initializeBinaryParentChildConstraints(vm, problem);
            initializeBinaryAlternativeGroups(vm, problem);
            initializeBinaryNonAlternativeExcluded(problem, vm);
            initializeBinaryImplications(vm, problem);
            initializeArbitraryBooleanConstraints(vm, problem);
            return problem;
        }

        private void addNumericOptionsToConstraintSystem(VariabilityModel vm, dynamic problem)
        {
            numericProblemVariables = new Dictionary<NumericOption, dynamic>();
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                dynamic var = Activator.CreateInstance(VarType, problem, numOpt.Name, numOpt.Min_value,
                    numOpt.Max_value, 0, varType["SCIP_VARTYPE_CONTINUOUS"]);
                numericProblemVariables[numOpt] = var;
                // Parent can be ignored because we dont support optional numeric configuration options

                dynamic temp = Activator.CreateInstance(VarType, problem, Math.Floor(numOpt.Min_value), 
                    Math.Ceiling(numOpt.Max_value), 0, varType["SCIP_VARTYPE_CONTINUOUS"]);
                // To model the step constraint in any efficient manner it has to be assumed that
                // a numeric option can only be a multiple of its initial value
                dynamic[] stepConstraint = new dynamic[2];
                stepConstraint[0] = var;
                stepConstraint[1] = temp;
                double[] coeffs = new double[2];
                coeffs[0] = 1;
                coeffs[1] = -numOpt.getAllValues().Min(x => x);
                Activator.CreateInstance(LinearConstraintType, problem, "step-" + numOpt.Name,
                    stepConstraint.Length, problem.unboxVars(stepConstraint), coeffs, 0.0, 0.0);
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
            scip = Activator.CreateInstance(ScipType, tmpFileName, "osil");
            File.Delete(tmpFileName);

            binaryProblemVariables = scip.getVars();
            scip.solve();

            if (scip.getNSols() == 0)
            {
                return null;
            }
            else
            {
                dynamic bestSolution = scip.getBestSol();
                List<BinaryOption> bestBinOpts = new List<BinaryOption>();
                Dictionary<NumericOption, double> bestNumOpts = new Dictionary<NumericOption, double>();
                foreach (dynamic problemVariable in binaryProblemVariables)
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
            int n, dynamic problem)
        {
            List<List<BinaryOption>> results = new List<List<BinaryOption>>();
            bool hasSol = true;
            while (hasSol && (n != 0))
            {
                dynamic copy = Activator.CreateInstance(ScipType, problem);
                dynamic[] vars = copy.getVars();

                copy.solve();
                if (copy.getNSols() == 0)
                {
                    hasSol = false;
                    continue;
                }

                dynamic solution = copy.getBestSol();
                List<BinaryOption> configuration = parseSolution(vm, vars, solution);

                dynamic[] blacklist = new dynamic[vm.BinaryOptions.Count];
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
                Activator.CreateInstance(LogicOrConstraintType, problem, "blacklist" + n,
                    blacklist.Count(), problem.unboxVars(blacklist));
                results.Add(configuration);

                n--;
            }
            return results;
        }

        public List<List<BinaryOption>> GenerateUpToNFast(VariabilityModel vm, int n)
        {
            dynamic problem = initializeBinaryConstraintSystem(vm);
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
            dynamic problem = initializeBinaryConstraintSystem(vm);
            updateObjectiveValues(vm, unWantedOptions, config, minimize);
            return generateUpToNWithGivenProblem(vm, 1, problem).First();
        }

        private List<BinaryOption> parseSolution(VariabilityModel vm, dynamic[] vars, dynamic solution)
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
            dynamic problem = initializeBinaryConstraintSystem(vm);
            updateObjectiveValues(vm, unwantedOptions, config, minimize);

            dynamic[] vars = problem.getVars();

            problem.solve();
            if (problem.getNSols() == 0)
            {
                return results;
            }

            // TODO: might actually not produce all best partial configuarations
            // might need iteration like the up to n version but with abort on configuration with
            // worse objsense
            dynamic[] solutions = problem.getSols();

            foreach (dynamic solution in solutions)
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
