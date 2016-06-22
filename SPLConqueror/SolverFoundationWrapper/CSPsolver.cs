using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SolverFoundation;
using Microsoft.SolverFoundation.Services;
using SPLConqueror_Core;
using Microsoft.SolverFoundation.Solvers;

namespace MicrosoftSolverFoundation
{
    class CSPsolver
    {
        private static ConstraintSystem csystem = null;
        private static List<CspTerm> variables_global = null;
        private static Dictionary<BinaryOption, CspTerm> optionToTerm_global = null;
        private static Dictionary<CspTerm, BinaryOption> termToOption_global = null;
        private static VariabilityModel vm_global = null;

        /// <summary>
        /// Generates a constraint system based on a variability model. The constraint system can be used to check for satisfiability of configurations as well as optimization.
        /// </summary>
        /// <param name="variables">Empty input, outputs a list of CSP terms that correspond to the configuration options of the variability model</param>
        /// <param name="optionToTerm">A map to get for a given configuration option the corresponding CSP term of the constraint system</param>
        /// <param name="termToOption">A map that gives for a given CSP term the corresponding configuration option of the variability model</param>
        /// <param name="vm">The variability model for which we generate a constraint system</param>
        /// <returns>The generated constraint system consisting of logical terms representing configuration options as well as their boolean constraints.</returns>
        internal static ConstraintSystem getConstraintSystem(out List<CspTerm> variables, out Dictionary<BinaryOption, CspTerm> optionToTerm, out Dictionary<CspTerm, BinaryOption> termToOption, VariabilityModel vm)
        {
            //Reusing seems to not work correctely. The problem: configurations are realized as additional constraints for the system. 
            //however, when checking for the next config, the old config's constraints remain in the solver such that we have a wrong result.
            /*
            if (csystem != null && variables_global != null && optionToTerm_global != null && termToOption_global != null && vm != null)
            {//For optimization purpose
                if (vm.BinaryOptions.Count == vm_global.BinaryOptions.Count && vm.Name.Equals(vm_global.Name))
                {
                    variables = variables_global;
                    optionToTerm = optionToTerm_global;
                    termToOption = termToOption_global;
                    return csystem;
                }
            }*/

            ConstraintSystem S = ConstraintSystem.CreateSolver();
            
            optionToTerm = new Dictionary<BinaryOption, CspTerm>();
            termToOption = new Dictionary<CspTerm, BinaryOption>();
            variables = new List<CspTerm>();
            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                CspDomain domain = S.DefaultBoolean;
                CspTerm temp = S.CreateVariable(domain, binOpt);
                optionToTerm.Add(binOpt, temp);
                termToOption.Add(temp, binOpt);
                variables.Add(temp);
            }

            List<List<ConfigurationOption>> alreadyHandledAlternativeOptions = new List<List<ConfigurationOption>>();

            //Constraints of a single configuration option
            foreach (BinaryOption current in vm.BinaryOptions)
            {
                CspTerm cT = optionToTerm[current];
                if (current.Parent == null || current.Parent == vm.Root)
                {
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        S.AddConstraints(S.Implies(S.True, cT));
                    else
                        S.AddConstraints(S.Implies(cT, optionToTerm[vm.Root]));
                }

                if (current.Parent != null && current.Parent != vm.Root)
                {
                    CspTerm parent = optionToTerm[(BinaryOption)current.Parent];
                    S.AddConstraints(S.Implies(cT, parent));
                    if (current.Optional == false && current.Excluded_Options.Count == 0)
                        S.AddConstraints(S.Implies(parent, cT));//mandatory child relationship
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
                        CspTerm parent = null;
                        if (current.Parent == null)
                            parent = S.True;
                        else
                            parent = optionToTerm[(BinaryOption)current.Parent];

                        CspTerm[] terms = new CspTerm[alternativeOptions.Count + 1];
                        terms[0] = cT;
                        int i = 1;
                        foreach (BinaryOption altEle in alternativeOptions)
                        {
                            CspTerm temp = optionToTerm[altEle];
                            terms[i] = temp;
                            i++;
                        }
                        S.AddConstraints(S.Implies(parent, S.ExactlyMofN(1, terms)));
                        alreadyHandledAlternativeOptions.Add(alternativeOptions);
                        handledAlternative: { }
                    }
                
                    //Excluded option(s) as cross-tree constraint(s)
                    List<List<ConfigurationOption>> nonAlternative = current.getNonAlternativeExlcudedOptions();
                    if(nonAlternative.Count > 0) {
                        foreach(var excludedOption in nonAlternative){
                            CspTerm[] orTerm = new CspTerm[excludedOption.Count];
                            int i = 0;
                            foreach (var opt in excludedOption)
                            {
                                CspTerm target = optionToTerm[(BinaryOption)opt];
                                orTerm[i] = target;
                                i++;
                            }
                            S.AddConstraints(S.Implies(cT, S.Not(S.Or(orTerm))));
                        }
                    }
                }
                //Handle implies
                if (current.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> impliedOr in current.Implied_Options)
                    {
                        CspTerm[] orTerms = new CspTerm[impliedOr.Count];
                        //Possible error: if a binary option impies a numeric option
                        for (int i = 0; i < impliedOr.Count; i++)
                            orTerms[i] = optionToTerm[(BinaryOption)impliedOr.ElementAt(i)];
                        S.AddConstraints(S.Implies(optionToTerm[current], S.Or(orTerms)));
                    }
                }
            }

            //Handle global cross-tree constraints involving multiple options at a time
            // the constraints should be in conjunctive normal form 
            foreach (string constraint in vm.BooleanConstraints)
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

                CspTerm[] cspTerms = new CspTerm[terms.Count()];
                int i = 0;
                foreach (string t in terms)
                {
                    string optName = t.Trim();
                    if (optName.StartsWith("-") || optName.StartsWith("!"))
                    {
                        optName = optName.Substring(1);
                        BinaryOption binOpt = vm.getBinaryOption(optName);
                        CspTerm cspElem = optionToTerm[binOpt];
                        CspTerm notCspElem = S.Not(cspElem);
                        cspTerms[i] = notCspElem;
                    }
                    else
                    {
                        BinaryOption binOpt = vm.getBinaryOption(optName);
                        CspTerm cspElem = optionToTerm[binOpt];
                        cspTerms[i] = cspElem;
                    }
                    i++;
                }
                if (and)
                    S.AddConstraints(S.And(cspTerms));
                else
                    S.AddConstraints(S.Or(cspTerms));
            }
            csystem = S;
            optionToTerm_global = optionToTerm;
            vm_global = vm;
            termToOption_global = termToOption;
            variables_global = variables;
            return S;
        }
    }
}
