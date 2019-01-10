using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using System.Xml;
using System.IO;

namespace Util
{
    public class ConvertUtil
    {
        public static void convertToBinaryCSV(string source, string target, VariabilityModel vm)
        {
            List<Configuration> confs = ConfigurationReader.readConfigurations(source, vm);
            Dictionary<string, string> index = new Dictionary<string, string>();
            GlobalState.varModel.getOptions().ForEach(opt =>
            {
                if (opt is BinaryOption)
                    index.Add(opt.Name, opt.Name);
                else
                    ((NumericOption)opt).getAllValues().ForEach(val => index.Add(opt.Name + "_" + (int)val, opt.Name));
            });
            GlobalState.nfProperties.Keys.ToList().ForEach(k => index.Add(k, k));
            StreamWriter sw = new StreamWriter(target);
            sw.WriteLine(string.Join(";", index.Keys));
            foreach (Configuration conf in confs)
            {
                List<string> values = new List<string>();
                index.Keys.ToList().ForEach(x =>
                {
                    ConfigurationOption opt = vm.getOption(index[x]);
                    if (opt != null)
                    {
                        if (opt is BinaryOption)
                        {
                            if (conf.BinaryOptions.ContainsKey((BinaryOption)opt))
                                values.Add(conf.BinaryOptions[(BinaryOption)opt] == BinaryOption.BinaryValue.Selected ? "1" : "0");
                            else
                                values.Add("0");
                        }
                        else
                        {
                            if (x == opt.Name + "_" + (int)conf.NumericOptions[(NumericOption)opt])
                                values.Add("1");
                            else
                                values.Add("0");
                        }
                    }
                    else
                    {
                        values.Add(conf.GetNFPValue(GlobalState.nfProperties[x]).ToString());
                    }
                });
                sw.WriteLine(string.Join(";", values));
            }
            sw.Flush();
            sw.Close();
        }

        public static void convertToBinaryXml(string source, string target)
        {
            XmlDocument sourceMeasurements = new XmlDocument();
            sourceMeasurements.Load(source);
            XmlElement resultNode = sourceMeasurements.DocumentElement;
            foreach (XmlNode rowNode in resultNode.ChildNodes)
            {
                string binaryFeatures = "";
                XmlNode binaryNode = null;
                foreach (XmlNode data in rowNode)
                {
                    if (data.Attributes[0].Value.Equals("Configuration") || data.Attributes[0].Value.Equals("BinaryOptions"))
                    {
                        binaryFeatures += data.InnerText.TrimEnd();
                        if (binaryFeatures.EndsWith(","))
                        {
                            binaryFeatures = binaryFeatures.Substring(0, binaryFeatures.Length - 1);
                        }
                        binaryNode = data;
                    }
                    else if (data.Attributes[0].Value.Equals("Variable Features") || data.Attributes[0].Value.Equals("NumericOptions"))
                    {
                        StringBuilder artificialParents = new StringBuilder();
                        data.InnerText.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList()
                            .ForEach(x => artificialParents.Append(x.Trim().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0] + ","));
                        binaryFeatures += "," + artificialParents.ToString() + data.InnerText.Replace(';', '_').Trim() + System.Environment.NewLine;
                        data.InnerText = "";
                    }
                }
                binaryNode.InnerText = binaryFeatures;
            }
            sourceMeasurements.Save(target);
            sourceMeasurements = null;
        }

        #region transformModelToAllbinary
        public static VariabilityModel transformVarModelAllbinary(VariabilityModel vm)
        {
            VariabilityModel transformedVarModel = new VariabilityModel(vm.Name);

            vm.BinaryOptions.ForEach(x => transformedVarModel.addConfigurationOption(x));
            vm.BinaryConstraints.ForEach(constraint =>
                convertToCNF(constraint).ForEach(convertedConstraint => transformedVarModel.BinaryConstraints.Add(convertedConstraint)));

            foreach (NumericOption currNumOpt in vm.NumericOptions)
            {
                BinaryOption parent = new BinaryOption(vm, currNumOpt.Name);
                transformedVarModel.addConfigurationOption(parent);
                // Create Binary Options for each numeric Option( #Steps)
                List<ConfigurationOption> allChildren = new List<ConfigurationOption>();
                foreach (double step in currNumOpt.getAllValues())
                {
                    BinaryOption toAdd = new BinaryOption(vm, currNumOpt.Name + "_" + step);
                    toAdd.Optional = false;
                    toAdd.OutputString = currNumOpt.Prefix + step + currNumOpt.Postfix;
                    toAdd.Parent = parent;
                    allChildren.Add(toAdd);
                    transformedVarModel.addConfigurationOption(toAdd);
                }

                // Add a exclude statement so that it isnt possible to select 2 values for a numeric option at the same time
                foreach (ConfigurationOption currentOption in allChildren)
                {
                    List<List<ConfigurationOption>> excluded = new List<List<ConfigurationOption>>();
                    List<ConfigurationOption> currentOptionWrapper = new List<ConfigurationOption>();
                    currentOptionWrapper.Add(currentOption);
                    allChildren.Except(currentOptionWrapper).ToList()
                        .ForEach(x => excluded.Add(new ConfigurationOption[] { x }.ToList()));
                    currentOption.Excluded_Options = excluded;
                }
            }
            transformNumericConstraintsToBoolean(transformedVarModel, vm);
            return transformedVarModel;

        }

        private static void transformNumericConstraintsToBoolean(VariabilityModel newVariabilityModel, VariabilityModel vm)
        {
            foreach (NonBooleanConstraint constraintToTransform in vm.NonBooleanConstraints)
            {
                String constraintAsExpression = constraintToTransform.ToString();

                string[] parts = constraintAsExpression.Split(new String[] { "=", "<", ">", "<=", ">=" }, StringSplitOptions.None);
                InfluenceFunction leftHandSide = new InfluenceFunction(parts[0], vm);
                InfluenceFunction rightHandSide = new InfluenceFunction(parts[parts.Length - 1], vm);

                // Find all possible assignments for the participating numeric options to turn it into a CNF clause
                // TODO: Maybe use a solver 
                List<NumericOption> allParticipatingNumericOptions = leftHandSide.participatingNumOptions.ToList()
                                                            .Union(rightHandSide.participatingNumOptions.ToList()).Distinct().ToList();

                List<List<double>> allPossibleValues = new List<List<double>>();
                allParticipatingNumericOptions.ForEach(x => allPossibleValues.Add(x.getAllValues()));

                var cartesianProduct = allPossibleValues.First().Select(x => x.ToString());
                foreach (List<double> possibleValue in allPossibleValues.Skip(1))
                {
                    cartesianProduct = from product in cartesianProduct
                                       from newValue in possibleValue
                                       select product + "$" + newValue;
                }

                // Find all invalid Configurations, turn each invalid Configuration into a or clause and combine them to a CNF 
                IEnumerable<string> invalidConfigs = cartesianProduct
                    .Where(x => !testIfValidConfiguration(x, allParticipatingNumericOptions, constraintToTransform));
                StringBuilder nonBooleanConstraintAsBoolean = new StringBuilder();
                foreach (string validConfig in invalidConfigs)
                {
                    List<string> binaryRepresentation = validConfig.Split(new char[] { '$' })
                        .Zip(allParticipatingNumericOptions, (x, y) => "!" + y.Name + "_" + x).ToList();
                    nonBooleanConstraintAsBoolean.Append(binaryRepresentation.First());
                    for (int i = 1; i < binaryRepresentation.Count; i++)
                    {
                        nonBooleanConstraintAsBoolean.Append(" | " + binaryRepresentation.ElementAt(i));
                    }
                    nonBooleanConstraintAsBoolean.Append(" & ");
                }

                if (nonBooleanConstraintAsBoolean.Length > 0)
                {
                    // Remove trailing ' & '
                    nonBooleanConstraintAsBoolean.Length = nonBooleanConstraintAsBoolean.Length - 3;
                    convertToCNF(nonBooleanConstraintAsBoolean.ToString()).ForEach(clause => newVariabilityModel.BinaryConstraints.Add(clause.Trim()));
                }
            }

        }

        private static bool testIfValidConfiguration(string configuration
            , List<NumericOption> participatingOptions, NonBooleanConstraint constraintToTest)
        {
            Dictionary<NumericOption, double> numericSelection = new Dictionary<NumericOption, double>();
            string[] values = configuration.Split(new char[] { '$' });
            for (int i = 0; i < participatingOptions.Count; i++)
            {
                numericSelection.Add(participatingOptions.ElementAt(i), Double.Parse(values[i]));
            }
            return constraintToTest.configIsValid(numericSelection);
        }

        private static List<string> convertToCNF(string nonCNFFormula)
        {
            // Assumption no '(',')', etc are used since these arent supported by SPLConqueror either
            // and functions with both & and | are already in CNF
            if (!nonCNFFormula.Contains("|"))
            {
                return nonCNFFormula.Split(new char[] { '&' }).Select(variable => variable.Trim()).ToList();
            }
            if (!nonCNFFormula.Contains("&"))
            {
                List<string> oneClause = new List<string>();
                oneClause.Add(nonCNFFormula);
                return oneClause;
            }
            List<string> inCNF = nonCNFFormula.Split(new char[] { '&' }).ToList();
            return inCNF;

        }

        #endregion


        #region parser dimacs
        public static string parseToDimacs(VariabilityModel toParse)
        {
            if (toParse.NumericOptions.Count > 0 || toParse.NonBooleanConstraints.Count > 0)
            {
                throw new ArgumentException();
            }
            StringBuilder parsedModel = new StringBuilder();
            Dictionary<string, int> nameToIndex = binaryOptionsToIndex(toParse.BinaryOptions);
            foreach (KeyValuePair<string, int> nameAndIndex in nameToIndex)
            {
                parsedModel.Append("c " + nameAndIndex.Value + " " + nameAndIndex.Key + System.Environment.NewLine);
            }
            List<string> parsedNonOptionNonExclusive = parseNonOptionalAndNotExcluded(nameToIndex, toParse);
            List<string> parsedParentExpressions = parseParentExpressions(nameToIndex, toParse);
            List<string> parsedImplicationExpressions = parseImplicationExpressions(nameToIndex, toParse);
            List<string> parsedAlternativeGroupExpressions = parseAlternativeGroupExpression(nameToIndex, toParse);
            List<string> parsedBooleanConstraints = parseBooleanConstraint(nameToIndex, toParse);
            int numberOfClauses = parsedNonOptionNonExclusive.Count + parsedParentExpressions.Count +
                parsedImplicationExpressions.Count + parsedAlternativeGroupExpressions.Count + parsedBooleanConstraints.Count;
            parsedModel.Append("p cnf " + toParse.BinaryOptions.Count + " " + numberOfClauses + System.Environment.NewLine);
            parsedNonOptionNonExclusive.ForEach(expression => parsedModel.Append(expression));
            parsedParentExpressions.ForEach(expression => parsedModel.Append(expression));
            parsedImplicationExpressions.ForEach(expression => parsedModel.Append(expression));
            parsedAlternativeGroupExpressions.ForEach(expression => parsedModel.Append(expression));
            parsedBooleanConstraints.ForEach(expression => parsedModel.Append(expression));
            return parsedModel.ToString();
        }

        private static Dictionary<string, int> binaryOptionsToIndex(List<BinaryOption> options)
        {
            Dictionary<string, int> nameToIndex = new Dictionary<string, int>();
            for (int i = 1; i <= options.Count; i++)
            {
                nameToIndex.Add(options.ElementAt(i - 1).Name, i);
            }
            return nameToIndex;
        }

        private static List<string> parseNonOptionalAndNotExcluded(Dictionary<string, int> nameToIndex, VariabilityModel toParse)
        {
            List<BinaryOption> nonOptionalAndNotExcluded = toParse.BinaryOptions
                .Where(x => x.Excluded_Options.Count == 0 && x.Optional == false).ToList();
            List<string> parsedExpressions = new List<string>();
            foreach (BinaryOption optionToParse in nonOptionalAndNotExcluded)
            {
                int i = getIndex(nameToIndex, optionToParse.Name);
                parsedExpressions.Add(i + " 0" + System.Environment.NewLine);
            }
            return parsedExpressions;
        }

        private static List<string> parseParentExpressions(Dictionary<string, int> nameToIndex, VariabilityModel toParse)
        {
            List<string> parsedParentExpression = new List<string>();
            foreach (BinaryOption toCheck in toParse.BinaryOptions)
            {
                if (toCheck.Parent != null)
                {
                    int thisOption = getIndex(nameToIndex, toCheck.Name);
                    int parentOption = getIndex(nameToIndex, toCheck.Parent.Name);
                    parsedParentExpression.Add(parentOption + " -" + thisOption + " 0" + System.Environment.NewLine);
                }
            }
            return parsedParentExpression;
        }

        private static List<string> parseImplicationExpressions(Dictionary<string, int> nameToIndex, VariabilityModel toParse)
        {
            List<string> parsedImplicationExpressions = new List<string>();
            foreach (BinaryOption toCheck in toParse.BinaryOptions)
            {
                if (toCheck.Implied_Options.Count > 0)
                {
                    foreach (List<ConfigurationOption> impliedOption in toCheck.Implied_Options)
                    {
                        // a->b <=> -a b
                        foreach (ConfigurationOption option in impliedOption)
                        {
                            int thisOptionIndex = getIndex(nameToIndex, toCheck.Name);
                            int impliedOptionIndex = getIndex(nameToIndex, toCheck.Name);
                            parsedImplicationExpressions.Add("-" + thisOptionIndex + " "
                                + impliedOptionIndex + " 0" + System.Environment.NewLine);
                        }
                    }
                }
            }
            return parsedImplicationExpressions;
        }

        private static List<string> parseBooleanConstraint(Dictionary<string, int> nameToIndex, VariabilityModel toParse)
        {
            List<string> parsedBooleanConstraints = new List<string>();
            foreach (string booleanConstraint in toParse.BinaryConstraints)
            {
                // replace each option name with their index, ! with - and remove |, since boolean expressions are already in CNF
                StringBuilder booleanConstraintInDimacs = new StringBuilder();
                string parsedConstraint = booleanConstraint.Replace("|", "");
                string[] participatingOptions = parsedConstraint.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string participatingOption in participatingOptions)
                {
                    if (participatingOption.Contains("!"))
                    {
                        booleanConstraintInDimacs.Append("-");
                        booleanConstraintInDimacs.Append(getIndex(nameToIndex, participatingOption.Replace("!", "").Trim()));
                        booleanConstraintInDimacs.Append(" ");
                    }
                    else
                    {
                        booleanConstraintInDimacs.Append(getIndex(nameToIndex, participatingOption.Trim()));
                        booleanConstraintInDimacs.Append(" ");
                    }
                }
                booleanConstraintInDimacs.Append("0");
                booleanConstraintInDimacs.Append(System.Environment.NewLine);
                parsedBooleanConstraints.Add(booleanConstraintInDimacs.ToString());
            }
            return parsedBooleanConstraints;
        }

        private static List<string> parseAlternativeGroupExpression(Dictionary<string, int> nameToIndex, VariabilityModel toParse)
        {
            List<string> parsedAlternativeGroupExpressions = new List<string>();
            List<string> alreadyHandled = new List<string>();
            foreach (BinaryOption optionToParse in toParse.BinaryOptions)
            {
                if (!alreadyHandled.Contains(optionToParse.Name) && optionToParse.hasAlternatives())
                {
                    // Add all configurations of an alternative group to a list
                    List<ConfigurationOption> alternativeOptions = new List<ConfigurationOption>();
                    optionToParse.Excluded_Options
                        .ForEach(optionGroup => optionGroup.ForEach(option => alternativeOptions.Add(option)));
                    alternativeOptions.Add(optionToParse);

                    // Write expression that indicates that at least one has to be selected or the parent has to be deselected
                    // Eg. -1 2 3 4 5 0
                    // With 2,3,4,5 forming a alternative group and 1 being the parent of them
                    ConfigurationOption parent = alternativeOptions.First().Parent;
                    StringBuilder sb = new StringBuilder();
                    alternativeOptions.ForEach(option => sb.Append(getIndex(nameToIndex, option.Name) + " "));
                    if (parent != null)
                    {
                        sb.Append("-" + getIndex(nameToIndex, parent.Name) + " ");
                    }
                    sb.Append("0" + System.Environment.NewLine);
                    parsedAlternativeGroupExpressions.Add(sb.ToString());

                    // Write a expression that indicates that at most one option in a alternative group can be selected
                    // E.g if 1,2,3 form a alternative group
                    // -1 -2 0
                    // -1 -3 0
                    // -2 -3 0
                    for (int i = 0; i < alternativeOptions.Count - 1; i++)
                    {
                        ConfigurationOption firstAlternative = alternativeOptions.ElementAt(i);
                        foreach (ConfigurationOption otherAlternative in alternativeOptions.Skip(i + 1))
                        {
                            StringBuilder mutualExclusive = new StringBuilder("-");
                            mutualExclusive.Append(getIndex(nameToIndex, firstAlternative.Name));
                            mutualExclusive.Append(" -");
                            mutualExclusive.Append(getIndex(nameToIndex, otherAlternative.Name));
                            mutualExclusive.Append(" 0");
                            mutualExclusive.Append(System.Environment.NewLine);
                            parsedAlternativeGroupExpressions.Add(mutualExclusive.ToString());
                        }
                    }
                    alternativeOptions.ForEach(option => alreadyHandled.Add(option.Name));
                }
            }
            return parsedAlternativeGroupExpressions;
        }

        private static int getIndex(Dictionary<string, int> nameToIndex, String option)
        {
            int i;
            nameToIndex.TryGetValue(option, out i);
            return i;
        }
        #endregion parser dimacs

        #region ConvertLegacyModel 
        public static void convertLegacyModel(string source, string target)
        {
            XmlDocument sourceModel = new XmlDocument();
            sourceModel.Load(source);
            XmlElement vmNode = sourceModel.DocumentElement;
            foreach (XmlNode configurationNode in vmNode.ChildNodes)
            {
                if (configurationNode.Name.Equals("binaryOptions"))
                {
                    Dictionary<string, string> childToParent = new Dictionary<string, string>();
                    foreach (XmlNode optionNode in configurationNode.ChildNodes)
                    {
                        List<XmlNode> toDelete = new List<XmlNode>();
                        string name = null;
                        foreach (XmlNode optionData in optionNode)
                        {
                            if (optionData.Name.Equals("defaultValue"))
                            {
                                toDelete.Add(optionData);
                            }
                            else if (optionData.Name.Equals("children"))
                            {
                                toDelete.Add(optionData);
                                foreach (XmlNode child in optionData.ChildNodes)
                                {
                                    childToParent.Add(child.InnerText, name);
                                }
                            }
                            else if (optionData.Name.Equals("name"))
                            {
                                name = optionData.InnerText;
                            }
                        }
                        toDelete.ForEach(node => optionNode.RemoveChild(node));
                    }
                    foreach (XmlNode optionNode in configurationNode.ChildNodes)
                    {
                        string parent = null;
                        string name = null;
                        foreach (XmlNode optionData in optionNode)
                        {
                            if (optionData.Name.Equals("name"))
                            {
                                name = optionData.InnerText;
                            }
                        }
                        childToParent.TryGetValue(name, out parent);
                        if (parent != null)
                        {
                            foreach (XmlNode optionData in optionNode)
                            {
                                if (optionData.Name.Equals("parent"))
                                {
                                    optionData.InnerText = parent;
                                }
                            }
                        }
                    }
                }
                else if (configurationNode.Name.Equals("numericOptions"))
                {

                    foreach (XmlNode option in configurationNode.ChildNodes)
                    {
                        List<XmlNode> toDelete = new List<XmlNode>();
                        foreach (XmlNode optionData in option.ChildNodes)
                        {
                            if (optionData.Name.Equals("defaultValue"))
                            {
                                toDelete.Add(optionData);
                            }
                        }
                        toDelete.ForEach(node => option.RemoveChild(node));
                    }
                }
            }
            sourceModel.Save(target);
            sourceModel = null;
        }
        #endregion
    }
}
