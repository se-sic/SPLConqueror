using Microsoft.Z3;
using SPLConqueror_Core;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MachineLearning.Solver
{
    static class Z3Solver
    {
        public static List<BoolExpr> lastConstraints { get; private set; } = null;

        private static Dictionary<string, double> numericLookUpTable = new Dictionary<string, double>();

        /// <summary>
        /// Generates a solver system (in z3: context) based on a variability model. The solver system can be used to check for satisfiability of configurations as well as optimization.
        /// Additionally to <see cref="Z3Solver.GetInitializedBooleanSolverSystem(out List{BoolExpr}, out Dictionary{BinaryOption, BoolExpr}, out Dictionary{BoolExpr, BinaryOption}, VariabilityModel, bool, int)"/>, this method supports numeric features.
        /// Note that we do not support Henard's randomized approach here, because it is defined only on boolean constraints.
        /// </summary>
        /// <param name="variables">Empty input, outputs a list of CSP terms that correspond to the configuration options of the variability model</param>
        /// <param name="optionToTerm">A map to get for a given configuration option the corresponding CSP term of the constraint system</param>
        /// <param name="termToOption">A map that gives for a given CSP term the corresponding configuration option of the variability model</param>
        /// <param name="vm">The variability model for which we generate a constraint system</param>
        /// <param name="randomSeed">The z3 random seed</param>
        /// <returns>The generated constraint system consisting of logical terms representing configuration options as well as their constraints.</returns>
        internal static Tuple<Context, BoolExpr> GetInitializedSolverSystem(out List<Expr> variables, out Dictionary<ConfigurationOption, Expr> optionToTerm, out Dictionary<Expr, ConfigurationOption> termToOption, VariabilityModel vm, int randomSeed = 0)
        {
            // Create a context and turn on model generation
            Context context = new Context(new Dictionary<string, string>() { { "model", "true" } });

            // Assign the out-parameters
            variables = new List<Expr>();
            optionToTerm = new Dictionary<ConfigurationOption, Expr>();
            termToOption = new Dictionary<Expr, ConfigurationOption>();

            // Create the binary configuration options
            foreach (BinaryOption binOpt in vm.WithAbstractBinaryOptions)
            {
                BoolExpr booleanVariable = GenerateBooleanVariable(context, binOpt.Name);
                variables.Add(booleanVariable);
                optionToTerm.Add(binOpt, booleanVariable);
                termToOption.Add(booleanVariable, binOpt);
            }

            // Create the numeric configuration options
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                Expr numericVariable = GenerateDoubleVariable(context, numOpt.Name);
                variables.Add(numericVariable);
                optionToTerm.Add(numOpt, numericVariable);
                termToOption.Add(numericVariable, numOpt);
            }

            // Initialize variables for constraint parsing
            List<List<ConfigurationOption>> alreadyHandledAlternativeOptions = new List<List<ConfigurationOption>>();

            List<BoolExpr> andGroup = new List<BoolExpr>();

            // Parse the constraints of the binary (boolean) configuration options
            foreach (BinaryOption current in vm.WithAbstractBinaryOptions)
            {
                BoolExpr expr = (BoolExpr)optionToTerm[current];
                if (current.Parent == null || current.Parent == vm.Root)
                {
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        andGroup.Add(expr);
                }

                if (current.Parent != null && current.Parent != vm.Root)
                {
                    BoolExpr parent = (BoolExpr)optionToTerm[(BinaryOption)current.Parent];
                    andGroup.Add(context.MkImplies(expr, parent));
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        andGroup.Add(context.MkImplies(parent, expr));
                }

                // Alternative or other exclusion constraints                
                if (current.Excluded_Options.Count > 0)
                {
                    List<ConfigurationOption> alternativeOptions = current.collectAlternativeOptions();
                    if (alternativeOptions.Count > 0)
                    {
                        // Check whether we handled this group of alternatives already
                        foreach (List<ConfigurationOption> alternativeGroup in alreadyHandledAlternativeOptions)
                            foreach (ConfigurationOption alternative in alternativeGroup)
                                if (current == alternative)
                                    goto handledAlternative;

                        // It is not allowed that an alternative group has no parent element
                        BoolExpr parent = null;
                        if (current.Parent == null)
                            parent = context.MkTrue();
                        else
                            parent = (BoolExpr)optionToTerm[(BinaryOption)current.Parent];

                        BoolExpr[] terms = new BoolExpr[alternativeOptions.Count + 1];
                        terms[0] = expr;
                        int i = 1;
                        foreach (BinaryOption altEle in alternativeOptions)
                        {
                            BoolExpr temp = (BoolExpr)optionToTerm[altEle];
                            terms[i] = temp;
                            i++;
                        }

                        BoolExpr[] exactlyOneOfN = new BoolExpr[] { context.MkAtMost(terms, 1), context.MkOr(terms) };
                        andGroup.Add(context.MkImplies(parent, context.MkAnd(exactlyOneOfN)));
                        alreadyHandledAlternativeOptions.Add(alternativeOptions);

                        // Go-To label
                        handledAlternative: { }
                    }

                    // Excluded option(s) as cross-tree constraint(s)
                    List<List<ConfigurationOption>> nonAlternative = current.getNonAlternativeExlcudedOptions();
                    if (nonAlternative.Count > 0)
                    {
                        foreach (var excludedOption in nonAlternative)
                        {
                            BoolExpr[] orTerm = new BoolExpr[excludedOption.Count];
                            int i = 0;
                            foreach (var opt in excludedOption)
                            {
                                BoolExpr target = (BoolExpr)optionToTerm[(BinaryOption)opt];
                                orTerm[i] = target;
                                i++;
                            }
                            andGroup.Add(context.MkImplies(expr, context.MkNot(context.MkOr(orTerm))));
                        }
                    }
                }
                // Handle implies
                if (current.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> impliedOr in current.Implied_Options)
                    {
                        BoolExpr[] orTerms = new BoolExpr[impliedOr.Count];
                        // Possible error: if a binary option implies a numeric option
                        for (int i = 0; i < impliedOr.Count; i++)
                            orTerms[i] = (BoolExpr)optionToTerm[(BinaryOption)impliedOr.ElementAt(i)];
                        andGroup.Add(context.MkImplies((BoolExpr)optionToTerm[current], context.MkOr(orTerms)));
                    }
                }
            }

            // Parse the constraints (ranges, step) of the numeric features
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                Expr numExpression = optionToTerm[numOpt];
                List<double> allValues = numOpt.getAllValues();
                List<BoolExpr> valueExpressions = new List<BoolExpr>();
                foreach (double value in allValues)
                {
                     
                    FPNum fpNum = context.MkFPNumeral(value, context.MkFPSortDouble());
                    if (!numericLookUpTable.ContainsKey(fpNum.ToString()))
                    {
                        numericLookUpTable.Add(fpNum.ToString(), value);
                    }
                    valueExpressions.Add(context.MkEq(numExpression, fpNum));
                }
                andGroup.Add(context.MkOr(valueExpressions.ToArray()));
            }


            // Parse the boolean cross-tree constraints
            foreach (string constraint in vm.BinaryConstraints)
            {
                bool and = false;
                string[] terms;
                if (constraint.Contains("&"))
                {
                    and = true;
                    terms = constraint.Split('&');
                }
                else
                    terms = constraint.Split('|');

                BoolExpr[] smtTerms = new BoolExpr[terms.Count()];
                int i = 0;
                foreach (string t in terms)
                {
                    string optName = t.Trim();

                    if (optName.StartsWith("-") || optName.StartsWith("!"))
                    {
                        optName = optName.Substring(1);
                        BinaryOption binOpt = vm.getBinaryOption(optName);
                        BoolExpr boolVar = (BoolExpr)optionToTerm[binOpt];
                        boolVar = context.MkNot(boolVar);
                        smtTerms[i] = boolVar;
                    }
                    else
                    {
                        BinaryOption binOpt = vm.getBinaryOption(optName);
                        BoolExpr boolVar = (BoolExpr)optionToTerm[binOpt];
                        smtTerms[i] = boolVar;
                    }


                    i++;
                }
                if (and)
                    andGroup.Add(context.MkAnd(smtTerms));
                else
                    andGroup.Add(context.MkOr(smtTerms));
            }
            
            // Parse the non-boolean constraints
            Dictionary<BinaryOption, Expr> optionMapping = new Dictionary<BinaryOption, Expr>();
            if (vm.NonBooleanConstraints.Count > 0)
            {
                foreach (NonBooleanConstraint nonBooleanConstraint in vm.NonBooleanConstraints)
                {
                    andGroup.Add(ProcessMixedConstraint(nonBooleanConstraint, optionMapping, context, optionToTerm));
                }
            }

            // Parse the mixed constraints
            // Note that this step is currently omitted due to critical performance issues.
            // Therefore, we check whether the mixed constraints are satisfied after finding the configuration.
            //if (vm.MixedConstraints.Count > 0)
            //{
                
                //foreach (MixedConstraint constr in vm.MixedConstraints)
                //{
                //    andGroup.Add(ProcessMixedConstraint(constr, optionMapping, context, optionToTerm));
                //}
            //}



            // Return the initialized system
            BoolExpr generalConstraint = context.MkAnd(andGroup.ToArray());

            return new Tuple<Context, BoolExpr>(context, generalConstraint);
        }

        /// <summary>
        /// Returns the numeric double value since z3's internal representation is not trivial to understand.
        /// To store the mapping from the internal representation to C# doubles, we use a lookup table. 
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The according <see cref="double"/> value.</returns>
        public static double lookUpNumericValue(string value)
        {
            return numericLookUpTable[value];
        }

        /// <summary>
        /// The non-boolean constraints are constraints among binary and numeric configuration options.
        /// Currently, these constraints are implemented as inequation of multiplicative terms.
        /// However, z3 does not support the multiplication among boolean variables (i.e., binary configuration options)
        /// and numeric variables (i.e., numeric configuration options).
        /// Thus, the approach is as follows:
        /// (1) Create a numeric variable for each binary configuration option in the mixed constraint and add a constraint
        /// for each of them, so that the numeric variable has the value 1 when the binary configuration option is true; 0 otherwise
        /// (2) Translate the constraints into z3
        /// </summary>
        /// <param name="constr">The <see cref="NonBooleanConstraint"/> to translate.</param>
        /// <param name="optionMapping">The mapping of already used binary options and their <see cref="Expr"/> counterparts.</param>
        /// <param name="context">The z3 <see cref="Context"/> needed for the translatation of these constraints.</param>
        /// <param name="optionToTerm">A mapping that maps the given option to a term.</param>
        /// <returns>A <see cref="BoolExpr"/> that represents this constraint.</returns>
        private static BoolExpr ProcessMixedConstraint(NonBooleanConstraint constr, Dictionary<BinaryOption, Expr> optionMapping, Context context, Dictionary<ConfigurationOption, Expr> optionToTerm)
        {
            List<BoolExpr> constraints = new List<BoolExpr>();

            // Process the binary options in the formula
            HashSet<BinaryOption> binOpts = constr.leftHandSide.participatingBoolOptions;
            foreach (BinaryOption binOpt in constr.rightHandSide.participatingBoolOptions)
            {
                if (!binOpts.Contains(binOpt))
                {
                    binOpts.Add(binOpt);
                }
            }
            foreach (BinaryOption binOpt in binOpts)
            {
                if (!optionMapping.ContainsKey(binOpt))
                {
                    Expr newExpr = GenerateDoubleVariable(context, binOpt.Name + "_num");
                    optionMapping[binOpt] = newExpr;

                    // Add the constraints to ensure that the values of the binary and the numeric variable correspond to each other.
                    BoolExpr constraint = (BoolExpr)optionToTerm[binOpt];
                    constraints.Add(context.MkImplies(constraint, context.MkEq(newExpr, context.MkFPNumeral(1, context.MkFPSortDouble()))));
                    constraints.Add(context.MkImplies(context.MkNot(constraint), context.MkEq(newExpr, context.MkFPNumeral(0, context.MkFPSortDouble()))));
                }
            }

            // Note: The 'require'-tag is ignored here, as it does not make sense when translating the mixed constraints for z3

            // Translate the mixed constraint
            FPExpr leftSide = (FPExpr) TranslateMixedConstraint(constr.leftHandSide, optionMapping, context, optionToTerm);
            FPExpr rightSide = (FPExpr) TranslateMixedConstraint(constr.rightHandSide, optionMapping, context, optionToTerm);


            // Next, check if the constraint has to evaluate to true or not
            string comparator = constr.comparator;
            if (constr.GetType() == typeof(MixedConstraint) &&
                ((MixedConstraint) constr).negativeOrPositiveExpr.Equals(MixedConstraint.NEGATIVE))
            {
                comparator = GetNegatedInequation(comparator);
            }

            switch (comparator)
            {
                case ">=":
                    return context.MkFPGEq(leftSide, rightSide);
                case "<=":
                    return context.MkFPLEq(leftSide, rightSide);
                case "!=":
                    return context.MkNot(context.MkEq(leftSide, rightSide));
                case "=":
                    return context.MkEq(leftSide, rightSide);
                case ">":
                    return context.MkFPGt(leftSide, rightSide);
                case "<":
                    return context.MkFPLt(leftSide, rightSide);
                default:
                    throw new ArgumentException("The inequation sign " + comparator + " is not supported.");
            }
        }

        private static string GetNegatedInequation(string inequSign)
        {
            switch(inequSign)
            {
                case ">=":
                    return "<";
                case "<=":
                    return ">";
                case "!=":
                    return "=";
                case "=":
                    return "!=";
                case ">":
                    return "<=";
                case "<":
                    return ">=";
                default:
                    throw new ArgumentException("The inequation sign " + inequSign + " is not supported.");
            }
        }


        private static Expr TranslateMixedConstraint(InfluenceFunction influenceFunction, Dictionary<BinaryOption, Expr> optionMapping, Context context, Dictionary<ConfigurationOption, Expr> optionToTerm)
        {
            string[] expression = influenceFunction.getExpressionTree();

            if (expression.Length == 0)
            {
                return null;
            }

            Stack<Expr> expressionStack = new Stack<Expr>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (!InfluenceFunction.isOperatorEval(expression[i]))
                {
                    double value;
                    if (Double.TryParse(expression[i], out value))
                    {
                        expressionStack.Push(context.MkFPNumeral(value, context.MkFPSortDouble()));
                    }
                    else
                    {
                        ConfigurationOption option = GlobalState.varModel.getOption(expression[i]);
                        Expr expr = null;
                        if (option is BinaryOption && optionMapping.ContainsKey((BinaryOption) option))
                        {
                            expr = optionMapping[(BinaryOption) option];
                        }
                        else
                        {
                            expr = optionToTerm[option];
                        }

                        expressionStack.Push(expr);
                    }
                } else
                {
                    // Be aware of the rounding mode
                    FPRMExpr roundingMode = context.MkFPRoundNearestTiesToEven();
                    switch (expression[i])
                    {
                        case "+":
                            FPExpr left = (FPExpr) expressionStack.Pop();
                            FPExpr right = (FPExpr)expressionStack.Pop();
                            
                            expressionStack.Push(context.MkFPAdd(roundingMode, left, right));
                            break;
                        case "-":
                            FPExpr leftSub = (FPExpr)expressionStack.Pop();
                            FPExpr rightSub = (FPExpr)expressionStack.Pop();

                            expressionStack.Push(context.MkFPSub(roundingMode, leftSub, rightSub));
                            break;
                        case "/":
                            FPExpr leftDiv = (FPExpr)expressionStack.Pop();
                            FPExpr rightDiv = (FPExpr)expressionStack.Pop();

                            expressionStack.Push(context.MkFPDiv(roundingMode, leftDiv, rightDiv));
                            break;
                        case "*":
                            FPExpr leftMul = (FPExpr)expressionStack.Pop();
                            FPExpr rightMul = (FPExpr)expressionStack.Pop();

                            expressionStack.Push(context.MkFPMul(roundingMode, leftMul, rightMul));
                            break;
                        // Note that the logarithm function is not supported by z3.
                        default:
                            throw new ArgumentException("The operator " + expression[i] + " is currently not supported in mixed constraints by z3.");
                    }
                }
            }

            return expressionStack.Pop();
        }




        /// <summary>
        /// Generates a solver system (in z3: context) based on a variability model. The solver system can be used to check for satisfiability of configurations as well as optimization.
        /// </summary>
        /// <param name="variables">Empty input, outputs a list of CSP terms that correspond to the configuration options of the variability model</param>
        /// <param name="optionToTerm">A map to get for a given configuration option the corresponding CSP term of the constraint system</param>
        /// <param name="termToOption">A map that gives for a given CSP term the corresponding configuration option of the variability model</param>
        /// <param name="vm">The variability model for which we generate a constraint system</param>
        /// <param name="randomSeed">The z3 random seed</param>
        /// <returns>The generated constraint system consisting of logical terms representing configuration options as well as their boolean constraints.</returns>
        internal static Tuple<Context, BoolExpr> GetInitializedBooleanSolverSystem(out List<BoolExpr> variables, out Dictionary<BinaryOption, BoolExpr> optionToTerm, out Dictionary<BoolExpr, BinaryOption> termToOption, VariabilityModel vm, bool henard, int randomSeed = 0)
        {
            // Create a context and turn on model generation
            Context context = new Context(new Dictionary<string, string>() { { "model", "true" } });

            // Assign the out-parameters
            variables = new List<BoolExpr>();
            optionToTerm = new Dictionary<BinaryOption, BoolExpr>();
            termToOption = new Dictionary<BoolExpr, BinaryOption>();

            if (henard)
            {
                // Select different (shuffled) literals for the features
                Random random = new Random(randomSeed);
                List<BinaryOption> randomizedOptions = (from item in vm.WithAbstractBinaryOptions
                                                        orderby random.Next()
                                                        select item).ToList();

                char name = 'A';

                // Read in the binary variables
                foreach (BinaryOption binOpt in randomizedOptions)
                {
                    BoolExpr booleanVariable = GenerateBooleanVariable(context, name.ToString());
                    variables.Add(booleanVariable);
                    optionToTerm.Add(binOpt, booleanVariable);
                    termToOption.Add(booleanVariable, binOpt);

                    if (name == 90)
                    {
                        name = 'a';
                    }
                    else
                    {
                        name++;
                    }
                }
            }
            else
            {

                // Read in the binary variables
                foreach (BinaryOption binOpt in vm.WithAbstractBinaryOptions)
                {
                    BoolExpr booleanVariable = GenerateBooleanVariable(context, binOpt.Name);
                    variables.Add(booleanVariable);
                    optionToTerm.Add(binOpt, booleanVariable);
                    termToOption.Add(booleanVariable, binOpt);
                }
            }

            List<List<ConfigurationOption>> alreadyHandledAlternativeOptions = new List<List<ConfigurationOption>>();

            List<BoolExpr> andGroup = new List<BoolExpr>();

            //Constraints of a single configuration option
            foreach (BinaryOption current in vm.WithAbstractBinaryOptions)
            {
                BoolExpr expr = (BoolExpr)optionToTerm[current];
                if (current.Parent == null || current.Parent == vm.Root)
                {
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        andGroup.Add(expr);
                }

                if (current.Parent != null && current.Parent != vm.Root)
                {
                    BoolExpr parent = (BoolExpr)optionToTerm[(BinaryOption)current.Parent];
                    andGroup.Add(context.MkImplies(expr, parent));
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        andGroup.Add(context.MkImplies(parent, expr));
                }

                //Alternative or other exclusion constraints                
                if (current.Excluded_Options.Count > 0)
                {
                    List<ConfigurationOption> alternativeOptions = current.collectAlternativeOptions();
                    if (alternativeOptions.Count > 0)
                    {
                        //Check whether we handled this group of alternatives already
                        foreach (var alternativeGroup in alreadyHandledAlternativeOptions)
                            foreach (var alternative in alternativeGroup)
                                if (current == alternative)
                                    goto handledAlternative;

                        //It is not allowed that an alternative group has no parent element
                        BoolExpr parent = null;
                        if (current.Parent == null)
                            parent = context.MkTrue();
                        else
                            parent = (BoolExpr)optionToTerm[(BinaryOption)current.Parent];

                        BoolExpr[] terms = new BoolExpr[alternativeOptions.Count + 1];
                        terms[0] = expr;
                        int i = 1;
                        foreach (BinaryOption altEle in alternativeOptions)
                        {
                            BoolExpr temp = (BoolExpr)optionToTerm[altEle];
                            terms[i] = temp;
                            i++;
                        }

                        BoolExpr[] exactlyOneOfN = new BoolExpr[] { context.MkAtMost(terms, 1), context.MkOr(terms) };
                        andGroup.Add(context.MkImplies(parent, context.MkAnd(exactlyOneOfN)));
                        alreadyHandledAlternativeOptions.Add(alternativeOptions);

                    // Go-To label
                    handledAlternative: { }
                    }

                    //Excluded option(s) as cross-tree constraint(s)
                    List<List<ConfigurationOption>> nonAlternative = current.getNonAlternativeExlcudedOptions();
                    if (nonAlternative.Count > 0)
                    {
                        foreach (var excludedOption in nonAlternative)
                        {
                            BoolExpr[] orTerm = new BoolExpr[excludedOption.Count];
                            int i = 0;
                            foreach (var opt in excludedOption)
                            {
                                BoolExpr target = (BoolExpr)optionToTerm[(BinaryOption)opt];
                                orTerm[i] = target;
                                i++;
                            }
                            andGroup.Add(context.MkImplies(expr, context.MkNot(context.MkOr(orTerm))));
                        }
                    }
                }
                //Handle implies
                if (current.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> impliedOr in current.Implied_Options)
                    {
                        BoolExpr[] orTerms = new BoolExpr[impliedOr.Count];
                        //Possible error: if a binary option implies a numeric option
                        for (int i = 0; i < impliedOr.Count; i++)
                            orTerms[i] = (BoolExpr)optionToTerm[(BinaryOption)impliedOr.ElementAt(i)];
                        andGroup.Add(context.MkImplies((BoolExpr)optionToTerm[current], context.MkOr(orTerms)));
                    }
                }
            }

            //Handle global cross-tree constraints involving multiple options at a time
            // the constraints should be in conjunctive normal form 
            foreach (string constraint in vm.BinaryConstraints)
            {
                andGroup.Add(ParseBooleanConstraint(optionToTerm, vm, constraint, context));
            }

            if (henard)
            {
                lastConstraints = andGroup;
            }

            BoolExpr generalConstraint = context.MkAnd(andGroup.ToArray());

            return new Tuple<Context, BoolExpr>(context, generalConstraint);
        }

        
        /// <summary>
        /// This method parses a boolean constraint recursively.
        /// </summary>
        /// <param name="optionToTerm">The option to term mapping</param>
        /// <param name="vm">The variability model</param>
        /// <param name="constraint">The constraint to parse</param>
        /// <param name="context">The context of the solver</param>
        /// <returns></returns>
        private static BoolExpr ParseBooleanConstraint(Dictionary<BinaryOption, BoolExpr> optionToTerm, VariabilityModel vm, string constraint,
            Context context)
        {
            string[] terms;
            if (constraint.Contains("|"))
            {
                terms = constraint.Split('|');
                List<BoolExpr> orGroup = new List<BoolExpr>();
                foreach (String term in terms)
                {
                    orGroup.Add(ParseBooleanConstraint(optionToTerm, vm, term, context));
                }

                return context.MkOr(orGroup);
            }
            else if (constraint.Contains("&"))
            {
                terms = constraint.Split('&');
                List<BoolExpr> andGroup = new List<BoolExpr>();
                foreach (String term in terms)
                {
                    andGroup.Add(ParseBooleanConstraint(optionToTerm, vm, term, context));
                }

                return context.MkAnd(andGroup);
            }
            else
            {
                string optName = constraint.Trim();

                if (optName.StartsWith("-") || optName.StartsWith("!"))
                {
                    optName = optName.Substring(1);
                    BinaryOption binOpt = vm.getBinaryOption(optName);
                    BoolExpr boolVar = (BoolExpr)optionToTerm[binOpt];
                    boolVar = context.MkNot(boolVar);
                    return boolVar;
                }
                else
                {
                    BinaryOption binOpt = vm.getBinaryOption(optName);
                    BoolExpr boolVar = (BoolExpr)optionToTerm[binOpt];
                    return boolVar;
                }
            }
            
        }

        /// <summary>
        /// Converts the constraints to the new context.
        /// </summary>
        /// <returns>The converted constraints.</returns>
        /// <param name="oldInverseMapping">The old mapping of expressions to the features.</param>
        /// <param name="newMapping">The new mapping that should be applied on the constraints.</param>
        /// <param name="toConvert">Expressions to convert.</param>
        /// <param name="z3Context">Z3 context.</param>
        public static List<BoolExpr> ConvertConstraintsToNewContext(Dictionary<BoolExpr, BinaryOption> oldInverseMapping, Dictionary<BinaryOption, BoolExpr> newMapping, List<BoolExpr> toConvert, Context z3Context)
        {
            List<BoolExpr> newConfigurationList = new List<BoolExpr>();
            foreach (BoolExpr configuration in toConvert)
            {
                List<BoolExpr> options = new List<BoolExpr>();
                for (int i = 0; i < configuration.Args[0].Args.Length; i++)
                {
                    BoolExpr exprToReplace = (BoolExpr)configuration.Args[0].Args[i];
                    if (exprToReplace.IsNot)
                    {
                        options.Add(z3Context.MkNot(newMapping[oldInverseMapping[(BoolExpr)configuration.Args[0].Args[i].Args[0]]]));
                    }
                    else
                    {
                        options.Add(newMapping[oldInverseMapping[(BoolExpr)configuration.Args[0].Args[i]]]);
                    }
                }
                newConfigurationList.Add(z3Context.MkNot(z3Context.MkAnd(options)));
            }
            return newConfigurationList;
        }

        /// <summary>
        /// Shuffle the specified constraints randomly.
        /// </summary>
        /// <returns>The shuffled constraints.</returns>
        /// <param name="constraints">Constraints.</param>
        /// <param name="random">Random.</param>
        public static List<BoolExpr> Shuffle(List<BoolExpr> constraints, Random random)
        {
            return (from item in constraints
                    orderby random.Next()
                    select item).ToList();
        }

        /// <summary>
        /// Converts the given configuration into a <see cref="BoolExpr"/>.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object.</param>
        /// <param name="options">The options that are selected.</param>
        /// <param name="optionToTerm">The mapping from <see cref="BinaryOption"/> to the according <see cref="BoolExpr"/>.</param>
        /// <param name="vm">The variability model that contains all configuration options and constraints.</param>
        /// <param name="partial"><code>true</code> if the configuration is a partial configuration; <code>false</code> otherwise.</param>
        /// <returns>The corresponding <see cref="BoolExpr"/>.</returns>
        public static BoolExpr ConvertConfiguration(Context context, List<BinaryOption> options, Dictionary<BinaryOption, BoolExpr> optionToTerm, VariabilityModel vm, bool partial = false)
        {
            List<BoolExpr> andGroup = new List<BoolExpr>();
            foreach (BinaryOption binOpt in vm.WithAbstractBinaryOptions)
            {
                if (options.Contains(binOpt))
                {
                    andGroup.Add(optionToTerm[binOpt]);
                }
                else if (!partial)
                {
                    andGroup.Add(context.MkNot(optionToTerm[binOpt]));
                }
            }

            return context.MkAnd(andGroup.ToArray());
        }

        /// <summary>
        /// Converts the given configuration into a <see cref="BoolExpr"/>.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object.</param>
        /// <param name="options">The options that are selected.</param>
        /// <param name="optionToTerm">The mapping from <see cref="BinaryOption"/> to the according <see cref="Expr"/>.</param>
        /// <param name="vm">The variability model that contains all configuration options and constraints.</param>
        /// <param name="partial"><code>true</code> if the configuration is a partial configuration; <code>false</code> otherwise.</param>
        /// <returns>The corresponding <see cref="ConfigurationOption"/>.</returns>
        public static BoolExpr ConvertConfiguration(Context context, List<BinaryOption> options, Dictionary<ConfigurationOption, Expr> optionToTerm, VariabilityModel vm, bool partial = false, Dictionary<NumericOption, double> numericValues = null)
        {
            List<BoolExpr> andGroup = new List<BoolExpr>();
            foreach (BinaryOption binOpt in vm.WithAbstractBinaryOptions)
            {
                if (options.Contains(binOpt))
                {
                    andGroup.Add((BoolExpr) optionToTerm[binOpt]);
                }
                else if (!partial)
                {
                    andGroup.Add(context.MkNot((BoolExpr) optionToTerm[binOpt]));
                }
            }

            // Now, also do this for numeric configuration options
            foreach (NumericOption numOpt in vm.NumericOptions)
            {
                Expr numericExpression = optionToTerm[numOpt];
                // Throw an exception if the configuration is not partial and does not contain a numeric option
                if (!partial && !numericValues.Keys.Contains(numOpt))
                {
                    throw new InvalidOperationException("The numeric option " + numOpt.ToString() + " is missing in the whole configuration.");
                } else if (numericValues.Keys.Contains(numOpt))
                {
                    andGroup.Add(context.MkEq(numericExpression, context.MkFPNumeral(numericValues[numOpt], context.MkFPSortDouble())));
                }
            }

            return context.MkAnd(andGroup.ToArray());
        }

        /// <summary>
        /// This method returns the negated boolean expression.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object.</param>
        /// <param name="expressionToNegate">The <see cref="BoolExpr"/> to negate.</param>
        /// <returns>The negated given expression as <see cref="BoolExpr"/>.</returns>
        public static BoolExpr NegateExpr(Context context, BoolExpr expressionToNegate)
        {
            return context.MkNot(expressionToNegate);
        }

        /// <summary>
        /// Generates a boolean variable with the given name.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object, from which the boolean variable should be generated.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>An <see cref="Expr"/>-object containing the boolean variable.</returns>
        private static BoolExpr GenerateBooleanVariable(Context context, string name)
        {
            return context.MkBoolConst(name);
        }

        /// <summary>
        /// Generates an integer variable with the given name.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object, from which the boolean variable should be generated.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>An <see cref="Expr"/>-object containing the integer variable.</returns>
        private static Expr GenerateIntVariable(Context context, string name)
        {
            return context.MkIntConst(name);
        }
        
        /// <summary>
        /// Generates an integer variable with the given name.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object, from which the boolean variable should be generated.</param>
        /// <param name="name">The name of the variable.</param>
        /// <returns>An <see cref="Expr"/>-object containing the integer variable.</returns>
        private static Expr GenerateDoubleVariable(Context context, string name)
        {
            return context.MkConst(name, context.MkFPSortDouble());
        }

        /// <summary>
        /// Disposes the given <see cref="Context"/>-object.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>-object to dispose.</param>
        public static void DisposeContext(Context context)
        {
            context.Dispose();
        }

    }
}
