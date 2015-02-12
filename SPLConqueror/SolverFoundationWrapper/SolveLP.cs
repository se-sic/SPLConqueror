using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SolverFoundation;
using Microsoft.SolverFoundation.Solvers;
using Microsoft.SolverFoundation.Services;
using System.IO;
using System.Data;
using System.ComponentModel.Composition;
using MicrosoftSolverFoundation;
using SPLConqueror_Core;

namespace MicrosoftSolverFoundation
{
    [Export(typeof(MachineLearning.Learning.LinearProgramming.ISolverLP))]
    [ExportMetadata ("SolverType", "MSSolverFoundation")]
    public class SolveLP : MachineLearning.Learning.LinearProgramming.ISolverLP
    {
        private Dictionary<BinaryOption, double> featureValues = new Dictionary<BinaryOption, double>();
        private bool evaluateInteractionsOnly = false;
        private bool withStandardDeviation = false;
        private double standardDeviation = 0.0f;
        public SolveLP() { }
        public SolveLP(Dictionary<BinaryOption, double> featureValues)
        {
            this.featureValues = featureValues;
        }

        /// <summary>
        /// Computes the influence of all configuration options based on the measurements of the given result db. It uses linear programming (simplex) and is an exact algorithm.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing options and interactions. The state of the model will be changed by the result of the process</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <returns>A map of binary options to their computed influences.</returns>
        public Dictionary<BinaryOption, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db)
        {
            List<BinaryOption> variables = infModel.Vm.BinaryOptions;
            List<double> results = new List<double>();
            List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
            foreach (Configuration c in db.Configurations)
            {
                configurations.Add(c.getBinaryOptions(BinaryOption.BinaryValue.Selected));
                if (nfp != null)
                    results.Add(c.GetNFPValue(nfp));
                else
                    results.Add(c.GetNFPValue());
            }
            List<String> errorEqs = new List<string>();
            Dictionary<String, double> faultRates = new Dictionary<string, double>();
            List<int> indexOfErrorMeasurements = new List<int>();
            Dictionary<String, double> featureValuedAsStrings = solve(variables, results, configurations, infModel.InteractionInfluence.Keys.ToList());
            foreach (String current in featureValuedAsStrings.Keys)
            {
                BinaryOption temp = infModel.Vm.getBinaryOption(current);
                this.featureValues[temp] = featureValuedAsStrings[current];
                InfluenceFunction influence = new InfluenceFunction(temp.Name + " + " + featureValuedAsStrings[current].ToString(),infModel.Vm);
                if (infModel.BinaryOptionsInfluence.Keys.Contains(temp))
                    infModel.BinaryOptionsInfluence[temp] = influence;
                else
                    infModel.BinaryOptionsInfluence.Add(temp, influence);
            }
            return this.featureValues;
        }


        /// <summary>
        /// Computes the influence of all configuration options and interactions based on the measurements of the given result db. It uses linear programming (simplex) and is an exact algorithm.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing the variability model, all configuration options and interactions.</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <param name="evaluateFeatureInteractionsOnly">Only interactions are learned.</param>
        /// <param name="withDeviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <param name="deviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <returns>Returns the learned infleunces of each option in a map whereas the String (Key) is the name of the option / interaction.</returns>
        public Dictionary<String, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db, bool evaluateFeatureInteractionsOnly, bool withDeviation, double deviation)
        {
            //Initialization
            List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
            this.evaluateInteractionsOnly = evaluateFeatureInteractionsOnly;
            this.withStandardDeviation = withDeviation;
            this.standardDeviation = deviation;
            List<double> results = new List<double>();
            foreach (Configuration c in db.Configurations)
            {
                configurations.Add(c.getBinaryOptions(BinaryOption.BinaryValue.Selected));
                if (nfp != null)
                    results.Add(c.GetNFPValue(nfp));
                else
                    results.Add(c.GetNFPValue());
            }

            List<BinaryOption> variables = new List<BinaryOption>();
            Dictionary<String, double> featureValues = new Dictionary<string, double>();
            Dictionary<String, double> faultRates = new Dictionary<string, double>();
            List<int> indexOfErrorMeasurements = new List<int>();

            if (configurations.Count == 0)
                return null;
            
            //For the case there is an empty base
            if (configurations.Count != 0)
            {
                if (configurations[0].Count == 0)
                {//Should never occur that we get a configuration with no option selected... at least the root must be there
                    BinaryOption root = infModel.Vm.Root;
                    //Element baseElement = new Element("base_gen", infModel.getID(), infModel);
                    //variables.Add(baseElement);
                    //  featureValues.Add(baseElement.getName(), 0);
                    foreach (List<BinaryOption> config in configurations)
                        if(!config.Contains(root))
                            config.Insert(0, root);
                }
            }


            //Building the variable list
            foreach (var elem in infModel.Vm.BinaryOptions)
            {
                variables.Add(elem);
                featureValues.Add(elem.Name, 0);
            }

            //First run
            
            featureValues = solve(variables, results, configurations, null);


            //if (evaluateFeatureInteractionsOnly == false)
                return featureValues;
            
            /*
            //We might have some interactions here and cannot compute all values
            //1. identify options that are only present in these equations
            Dictionary<Element, int> featureCounter = new Dictionary<Element, int>();
            for (int i = 0; i < indexOfErrorMeasurements.Count; i++)
            {

            }
            
            */
            /*Todo: get compute interactins from deviations / errors of the LP results
            if (errorEqs != null)
            {
                foreach (string eq in errorEqs)
                {
                    double value = Double.Parse(eq.Substring(eq.IndexOf("==") + 2));

                    StringBuilder sb = new StringBuilder();
                    List<Element> derivativeParents = new List<Element>();
                    sb.Append("derivate_");
                    string[] splittedEQ = eq.Split('+');
                    foreach (string element in splittedEQ)
                    {
                        string name = element;
                        if (name.Contains("=="))
                            name = name.Substring(0, name.IndexOf("=="));
                        if (name.Contains("yp") && name.Contains("-yn"))
                            continue;
                        // string featureName = name.Substring(0, name.IndexOf("_p-"));
                        Element elem = infModel.getElementByNameUnsafe(name);
                        if (elem == null)
                            continue;
                        sb.Append("_" + name);
                        derivativeParents.Add(elem);
                    }
                    Element interaction = new Element(sb.ToString(), infModel.getID(), infModel);
                    interaction.setType("derivative");
                    interaction.addDerivativeParents(derivativeParents);
                    infModel.addElement(interaction);
                    this.featureValues.Add(interaction, value);
                }
            }
            return featureValues;*/
        }


        private Dictionary<String, double> solve(List<BinaryOption> variables, List<double> results, List<List<BinaryOption>> configurations, List<Interaction> interactions)
        {
           
            Dictionary<String, double> optionInfluences = new Dictionary<string, double>();
            foreach (BinaryOption elem in variables)
                optionInfluences.Add(elem.Name, 0);
            if(interactions != null)
                foreach (Interaction inter in interactions)
                    optionInfluences.Add(inter.Name, 0);

            //Building the OML model
            StringBuilder omlModel = new StringBuilder();
            omlModel.Append("Model[");

            String decisions = generateDecisionOML(variables, results);
            omlModel.Append(decisions + ",");
            String constraints = generateConstraintOML(variables, configurations, interactions, results);
            omlModel.Append(constraints + ",");

            String goal = generateGoalOML(results.Count);
            omlModel.Append(goal + "]");

            Console.WriteLine(omlModel);
            SolverContext context = SolverContext.GetContext();
            context.ClearModel();
            //String model = "Model[Decisions[Reals[0,Infinity], f1,f2,y1,y2,y3],Constraints[f1+y1==1,f1+f2+y2==4,f2+y3==3],Goals[Minimize[y1+y2+y3]]]";
            //String model = "Model[Decisions[Reals[0,Infinity],fp1,fn1,fp2,fn2,yp1,yn1,yp2,yn2,yp3,yn3],Constraints[fp1-fn1+yp1-yn1==1,fp1-fn1+fp2-fn2+yp2-yn2==0,fp2-fn2+yp3-yn3==-1],Goals[Minimize[yp1+yn1+yp2+yn2+yp3+yn3]]]";
            //Console.WriteLine(omlModel.ToString());
            context.LoadModel(FileFormat.OML, new StringReader(omlModel.ToString()));

            //List<double> erglist = new List<double>();
            

            //Solve the optimization problem
            if (context.CheckModel().IsValid)
            {
                Solution solution = context.Solve();
                StringBuilder sb = new StringBuilder();
               
                foreach (Decision d in solution.Decisions)
                {
                    sb.Append(d.ToString()+"; ");

                    //Constructing feature Values
                    string option = d.Name.Substring(0, d.Name.Length - 2);
                    if (d.Name.EndsWith("_p"))
                        optionInfluences[option] += d.ToDouble();
                    else if (d.Name.EndsWith("_n"))
                        optionInfluences[option] -= d.ToDouble();
                }
                /*if (this.withStandardDeviation && this.standardDeviation != 0)
                {
                    foreach (BinaryOption elem in variables)
                    {
                         if(featureValues[elem.Name] == 0)
                             continue;
                        int configNb = 0;
                        foreach (List<BinaryOption> config in configurations)
                        {
                            bool derivativeRequired = true;
                            foreach(Element parent in elem.getDerivativeParents())
                            {
                                if(config.Contains(parent) == false)
                                {
                                    derivativeRequired = false;
                                    break;
                                }
                            }
                            if(derivativeRequired)
                            {
                                double equationValue = results[configNb];
                                if (Math.Abs(featureValues[elem.Name]) < Math.Abs(equationValue * this.standardDeviation / 100))
                                {        
                                    featureValues[elem.Name] = 0;
                                    break;
                                }
                            }
                            configNb++;
                        }
                    }
                }*/
            }
            return optionInfluences;
        }


        private string generateGoalOML(int p)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Goals[Minimize[");
            
            //Adding goals
            for(int i = 0; i < p; i++)
                sb.Append("yp" + i.ToString() + "+yn" + i.ToString() + "+");

            //Delete last "+"
            sb.Remove(sb.Length - 1, 1);

            sb.Append("]]");
            return sb.ToString();
        }

        private string generateConstraintOML(List<BinaryOption> variables, List<List<BinaryOption>> configurations, List<Interaction> interactions, List<double> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Constraints[");

            //Adding one constraint per measurement
            for(int i = 0; i < configurations.Count; i++)
            {
                StringBuilder constraint = new StringBuilder();
                double sumOfKnownValues = 0.0f;
                if (configurations[i].Contains(null))
                    configurations[i].Remove(null);

                //adding the generated base element to all configurations (only applied when the original base is empty)
                if (variables.Count > 0 && variables[0].Name == "base_gen")
                    constraint.Append("base_gen_p-base_gen_n+");
               

                //Add variables (features) to the constraint
                foreach (BinaryOption current in configurations[i])
                {
                    if (current == null)
                        continue;
                    if (this.featureValues.Keys.Contains(current))
                    {
                        //constraint.Append(this.featureValues[current].ToString()+"+");
                        sumOfKnownValues += this.featureValues[current];
                    }
                    else
                        constraint.Append(current.Name + "_p-" + current.Name + "_n+");
                }

                if (interactions != null && interactions.Count > 0) //Consider also interactions!
                {
                    //Adding interactions to the OML model
                    //collection all derivatives needed to evaluate for the subelement relationship
                    List<Interaction> presentInteractions = new List<Interaction>();
                    foreach (Interaction interact in interactions)
                    {
                        if (interact.isInConfiguration(configurations[i]))
                            presentInteractions.Add(interact);
                    }

                    foreach (Interaction inter in presentInteractions)
                        constraint.Append(inter.Name + "_p-" + inter.Name + "_n+");
                }

                //adding fault indicatior to the constraint
                constraint.Append("yp" + i.ToString() + "-yn" + i.ToString() + "==");

                //Adding measurement result to constraint
                if(this.evaluateInteractionsOnly)
                {
                    double realresult = results[i] - sumOfKnownValues;
                    constraint.Append(realresult.ToString().Replace(',', '.') + ",");
                }
                else
                    constraint.Append(results[i].ToString().Replace(',', '.') + ",");
                sb.Append(constraint.ToString());
            }
            //Delete last ","
            sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
            return sb.ToString();
        }

        private string generateDecisionOML(List<BinaryOption> variables, List<double> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Decisions[Reals[0,Infinity],");

            //Adding feature variables to decision set
            foreach (BinaryOption elem in variables)
            {
               // if (this.evaluateInteractionsOnly)
                //{
                  //  if (elem.getType() != "derivative")
                    //    continue;
               //}
                sb.Append(elem.Name + "_p," + elem.Name + "_n,");
            }


            //Adding fault indicator to decision set
            for (int i = 0; i < results.Count; i++)
                sb.Append("yp" + i.ToString() + ",yn" + i.ToString()+",");
            
            //Delete last ","
            sb.Remove(sb.Length - 1, 1);

            sb.Append("]");
            return sb.ToString();
        }

        /*
        public List<BinaryOption> solveForMultipleProperties(VariabilityModel vm, bool optimized, List<RuntimeProperty> properties, List<NFPConstraint> constraints, RuntimeProperty goalProp, bool maximize)
        {
            List<Element> resultConfig = new List<Element>();
            List<CspTerm> variables = new List<CspTerm>();
            List<List<Element>> knownConfigs = new List<List<Element>>();
            Dictionary<Element, CspTerm> elemToTerm = new Dictionary<Element, CspTerm>();
            Dictionary<CspTerm, Element> termToElem = new Dictionary<CspTerm, Element>();

            ConstraintSystem S = CSPsolver.getConstraintSystem(out variables, out elemToTerm, out termToElem, vm, true);
            if (optimized)
            {
                foreach (RuntimeProperty prop in properties)
                {
                    foreach (NFPConstraint c in constraints)
                    {
                        if (c.Nfp != prop)
                            continue;
                        CspTerm[] tempGoals = new CspTerm[variables.Count];
                        for (int r = 0; r < variables.Count; r++)
                        {
                            Element elem = termToElem[variables[r]];
                            if (maximize)
                                tempGoals[r] = variables[r] * prop.Value_per_feature[elem] + (-1);
                            else
                                tempGoals[r] = variables[r] * prop.Value_per_feature[elem];
                            // dynamic cost map
                        }
                        S.AddConstraints(S.Greater(S.Constant(c.Value), S.Sum(tempGoals)));
                    }
                }
            }
            CspTerm[] finalGoals = new CspTerm[variables.Count];
            for (int r = 0; r < variables.Count; r++)
            {
                Element elem = termToElem[variables[r]];
                if (maximize)
                    finalGoals[r] = variables[r] * goalProp.Value_per_feature[elem] * (-1);
                else
                    finalGoals[r] = variables[r] * goalProp.Value_per_feature[elem];
                // dynamic cost map
            }
            S.TryAddMinimizationGoals(S.Sum(finalGoals));
            DateTime before = DateTime.Now;

            ConstraintSolverSolution soln = S.Solve();
            DateTime after = DateTime.Now;
            System.TimeSpan span = after.Subtract(before);
            if (Settings.getParameter("CONSOLE") == "NO")
                HelperClass.printContent(span.TotalMilliseconds.ToString());
            List<string> erg2 = new List<string>();
            while (soln.HasFoundSolution)
            {
                List<Element> tempConfig = new List<Element>();
                string ausgabe = soln.Quality.ToString();
                if (soln.Quality == ConstraintSolverSolution.SolutionQuality.Infeasible)
                    return null;

                foreach (CspTerm cT in variables)
                {
                    if (soln.GetIntegerValue(cT) == 1)
                        tempConfig.Add(termToElem[cT]);
                }
                return tempConfig;
                soln.GetNext();

            }
            return resultConfig;
        }*/
    }
}
