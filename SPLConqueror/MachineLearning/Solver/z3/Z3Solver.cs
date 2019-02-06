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
            foreach (BinaryOption binOpt in vm.BinaryOptions)
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
            foreach (BinaryOption current in vm.BinaryOptions)
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
                    valueExpressions.Add(context.MkEq(numExpression, context.MkFPNumeral(value, context.MkFPSortDouble())));
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

            // TODO: Parse the mixed constraints


            // Return the initialized system
            BoolExpr generalConstraint = context.MkAnd(andGroup.ToArray());

            return new Tuple<Context, BoolExpr>(context, generalConstraint);
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
                List<BinaryOption> randomizedOptions = (from item in vm.BinaryOptions
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
                foreach (BinaryOption binOpt in vm.BinaryOptions)
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
            foreach (BinaryOption current in vm.BinaryOptions)
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

            if (henard)
            {
                lastConstraints = andGroup;
            }

            BoolExpr generalConstraint = context.MkAnd(andGroup.ToArray());

            return new Tuple<Context, BoolExpr>(context, generalConstraint);
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
            foreach (BinaryOption binOpt in vm.BinaryOptions)
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
            foreach (BinaryOption binOpt in vm.BinaryOptions)
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
