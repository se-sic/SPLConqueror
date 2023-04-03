﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SPLConqueror_Core
{

    /// <summary>
    /// The influence function class that represents a term (e.g., 1.3 * x ).
    /// </summary>
    public class InfluenceFunction
    {

        double noise = 0.0;
        private VariabilityModel varModel = null;

        /// <summary>
        /// This field includes the most important information as a well-formed expression (e.g., 1.3 * x ).
        /// </summary>
        protected string wellFormedExpression = "";


        /// <summary>
        /// All binary configuration options that are considered in the influence function. 
        /// </summary>
        public HashSet<BinaryOption> participatingBoolOptions = new HashSet<BinaryOption>();

        /// <summary>
        /// All numeric configuration options that are considered in the influence function. 
        /// </summary>
        public HashSet<NumericOption> participatingNumOptions = new HashSet<NumericOption>();

        /// <summary>
        /// The number of configuration options (features) that are participating in this influence function.
        /// </summary>
        protected int numberOfParticipatingFeatures = 0;

        /// <summary>
        /// Contains the whole expression in the array.
        /// Every element of this array contains an operand or an operator.
        /// </summary>
        protected string[] expressionArray = null;

        /// <summary>
        /// The <see cref="NumericOption"/>, the <see cref="InfluenceFunction"/> is related to.
        /// </summary>
        protected NumericOption numOption = null;


        /// <summary>
        /// Creates an influence function based on the expression. The variability model is used to identify binary and numeric
        /// configuration options. 
        /// </summary>
        /// <param name="expression">A function consisting of numbers, operators and configuration-option names.</param>
        /// <param name="varModel">The variability model of the configuration options.</param>
        public InfluenceFunction(string expression, VariabilityModel varModel)
        {
            this.varModel = varModel;
            parseExpressionToPolishNotation(expression);
        }

        /// <summary>
        /// Creates an influence function based on the expression. All token wich are neither number nor operators are considered to be 
        /// numeric configuration options. 
        /// </summary>
        /// <param name="expression">A function consisting of numbers, operators and configuration-option names.</param>
        public InfluenceFunction(String expression)
        {

            this.varModel = extractFeatureModelFromExpression(createWellFormedExpression(expression));
            parseExpressionToPolishNotation(expression);
        }

        /// <summary>
        /// Creates an influence function based on the expression. Only the name of the numeric option, numbers, operators, 
        /// and " n " should exist in the expression. 
        /// </summary>
        /// <param name="expression">A function consisting of numbers, operators and the configuration-option name.</param>
        /// <param name="option">A configuration option.</param>
        public InfluenceFunction(String expression, NumericOption option)
        {
            numOption = option;
            if (option.Name != "n" && expression.Split(' ').Contains("n"))
                expression = expression.Replace("n", option.Name);
            parseExpressionToPolishNotation(expression);
        }

        /// <summary>
        /// An empty constructor for the <see cref="InfluenceFunction"/>.
        /// </summary>
        protected InfluenceFunction()
        {
        }

        private VariabilityModel extractFeatureModelFromExpression(String expression)
        {
            VariabilityModel varModel = new VariabilityModel("TEMP");

            string[] parts = expression.Split(new char[] { '+', '-', '*', '[', ']' });

            double value = 0.0;
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i].Trim();
                if (part.Length > 0)
                {
                    if (!Double.TryParse(part, out value))
                    {
                        if (varModel.getNumericOption(part) == null)
                        {
                            NumericOption option = new NumericOption(varModel, part);
                            varModel.addConfigurationOption(option);
                        }
                    }
                }
            }
            return varModel;
        }

        /**
         * 
         * Adds a whitespace before and after each special character (+,*,[,],....)
         * besides, it replaces each two pair of whitespaces with a single whitespace
         * 
         * 
         **/
        private string createWellFormedExpression(string expression)
        {
            while (expression.Contains(" "))
            {
                expression = expression.Replace(" ", "");
            }

            expression = replaceDifferentLogParts(expression);

            expression = expression.Replace("\n", " ");
            expression = expression.Replace("\t", " ");

            expression = expression.Replace("+", " + ");
            expression = expression.Replace("-", " - ");
            expression = expression.Replace("*", " * ");
            expression = expression.Replace("/", " / ");

            expression = expression.Replace("(", " ( ");
            expression = expression.Replace(")", " ) ");

            expression = expression.Replace("[", " [ ");
            expression = expression.Replace("]", " ] ");

            while (expression.Contains("  "))
            {
                expression = expression.Replace("  ", " ");
            }

            expression = expression.Replace("+ - ", "+ -");
            expression = removeWhitespacesExponentialPart(expression);

            return expression;
        }

        private string removeWhitespacesExponentialPart(String expression)
        {
            Regex r = new Regex(@"([0-9])E\s(\+|-)\s([0-9])", RegexOptions.None);
            expression = r.Replace(expression, @"$1E$2$3");
            return expression;
        }

        private string replaceDifferentLogParts(string expression)
        {

            while (expression.Contains("log10("))
            {
                expression = InfluenceFunction.replaceLogAndClosingBracket(expression, "log10(", '[', ']');
            }

            return expression;
        }


        private static string replaceLogAndClosingBracket(string expression, string logStart, char newLeftBracket, char newRightBracket)
        {

            string[] parts = expression.Split(new string[] { logStart }, 2, StringSplitOptions.None);
            if (parts.Length == 1)
                return expression;

            // there is no "log(" in parts[0], as a consequence, we can return this part

            StringBuilder secondPart = new StringBuilder(parts[1]);

            secondPart[InfluenceFunction.findOffsetToClosingBracket(secondPart.ToString())] = newRightBracket;

            return parts[0] + newLeftBracket + secondPart.ToString();
        }

        /// <summary>
        /// Returns an copy of the influence function in reverse polish notation, where an operator or operand is stored in one element of the array.
        /// </summary>
        /// <returns>The reverse polish notation of the influence function.</returns>
        public string[] getExpressionTree()
        {
            string[] copy = new string[expressionArray.Length];
            Array.Copy(expressionArray, copy, copy.Length);
            return copy;
        }

        /// <summary>
        /// This method creates an influence function that is the combination of the left and the right functions. Both funtions should
        /// be defined over the same variability model. 
        /// </summary>
        /// <param name="left">The first summand of the new influence function.</param>
        /// <param name="right">The second summand of the new influence function.</param>
        /// <returns>The combination of the two influence functions.</returns>
        public static InfluenceFunction combineFunctions(InfluenceFunction left, InfluenceFunction right)
        {
            InfluenceFunction expTree = new InfluenceFunction();
            expTree.noise = left.noise;
            expTree.varModel = left.varModel;
            expTree.wellFormedExpression = left.wellFormedExpression + " * " + right.wellFormedExpression;

            expTree.participatingBoolOptions.UnionWith(left.participatingBoolOptions);
            expTree.participatingBoolOptions.UnionWith(right.participatingBoolOptions);

            expTree.participatingNumOptions.UnionWith(left.participatingNumOptions);
            expTree.participatingNumOptions.UnionWith(right.participatingNumOptions);

            expTree.numberOfParticipatingFeatures = expTree.participatingBoolOptions.Count + expTree.participatingNumOptions.Count;

            expTree.expressionArray = left.expressionArray.ToList().Concat(right.expressionArray.ToList()).Concat(new List<String>() { "*" }).ToArray();

            return expTree;
        }


        /// <summary>
        /// We check whether all options in the influence function are also selected in the configuration. 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool validConfig(Configuration config)
        {
            Dictionary<BinaryOption, BinaryOption.BinaryValue> binOptions = config.BinaryOptions;

            foreach (BinaryOption bOpt in this.participatingBoolOptions)
            {
                if (bOpt.Name.Equals("root"))
                    continue;

                // if option is deselected
                if (!binOptions.ContainsKey(bOpt) || binOptions[bOpt] == BinaryOption.BinaryValue.Deselected)
                {
                    bool found = false;
                    foreach (BinaryOption binOpt in config.BinaryOptions.Keys)
                    {
                        if ((binOpt.Name == bOpt.Name))
                            found = true;
                    }
                    if (!found)
                        return false;
                }
            }

            List<NumericOption> numOptions = config.NumericOptions.Keys.ToList();
            foreach (NumericOption num in this.participatingNumOptions)
            {
                if (!numOptions.Contains(num))
                    return false;
            }

            return true;
        }



        private void parseExpressionToPolishNotation(String expression)
        {
            Queue<string> queue = new Queue<string>();
            Stack<string> stack = new Stack<string>();

            expression = createWellFormedExpression(expression).Trim();



            this.wellFormedExpression = expression;
            string[] expr = expression.Split(' ');

            for (int i = 0; i < expr.Length; i++)
            {
                string token = expr[i];


                if (InfluenceFunction.isOperator(token))
                {
                    if (stack.Count > 0)
                    {
                        while (stack.Count > 0 && InfluenceFunction.isOperator(stack.Peek()) && InfluenceFunction.operatorHasGreaterPrecedence(stack.Peek(), token))
                        {
                            queue.Enqueue(stack.Pop());
                        }
                    }
                    stack.Push(token);
                    continue;
                }
                else if (token.Equals("("))
                    stack.Push(token);
                else if (token.Equals("["))
                    stack.Push(token);
                else if (token.Equals("{"))
                    stack.Push(token);
                else if (token.Equals("<"))
                    stack.Push(token);

                else if (token.Equals(")"))
                {
                    while (!stack.Peek().Equals("("))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    stack.Pop();
                    continue;
                }

                else if (token.Equals("]"))
                {
                    while (!stack.Peek().Equals("["))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    queue.Enqueue("]");
                    stack.Pop();
                    continue;
                }
                // token is number or a feature which has a value
                // features existing in the function but not in the feature model, have to be accepted too
                if (tokenIsAFeatureOrNumber(token, this.varModel))
                    queue.Enqueue(token);

            }

            // stack abbauen
            while (stack.Count > 0)
            {
                queue.Enqueue(stack.Pop());
            }
            expressionArray = queue.ToArray();
        }

        private double evaluationOfRPN(Configuration config)
        {
            int counter = 0;

            Stack<double> stack = new Stack<double>();

            if (expressionArray.Length == 0)
                return 1;

            while (counter < expressionArray.Length)
            {
                string curr = expressionArray[counter];
                if (wellFormedExpression.Contains("Enabled"))
                {
                    int dbg = 1;
                }
                counter++;

                if (!InfluenceFunction.isOperatorEval(curr))
                {
                    stack.Push(getValueOfToken(config, curr, varModel));
                }
                else
                {
                    if (curr.Equals("+"))
                    {
                        double rightHandSide = stack.Pop();
                        if (stack.Count == 0)
                            stack.Push(rightHandSide);
                        double leftHandSide = stack.Pop();
                        if (counter < expressionArray.Length && expressionArray[counter].Equals("-"))
                            stack.Push(leftHandSide - rightHandSide);
                        else
                            stack.Push(leftHandSide + rightHandSide);
                    }
                    if (curr.Equals("-"))
                    {
                        double rightHandSide = stack.Pop();
                        if (stack.Count == 0)
                            stack.Push(rightHandSide);
                        double leftHandSide = stack.Pop();
                        if (counter < expressionArray.Length && expressionArray[counter].Equals("-"))
                            stack.Push(leftHandSide + rightHandSide);
                        else
                            stack.Push(leftHandSide - rightHandSide);
                    }
                    if (curr.Equals("*"))
                    {
                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        stack.Push(leftHandSide * rightHandSide);
                    }
                    if (curr.Equals("/"))
                    {
                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        if (leftHandSide == 0.0 || rightHandSide == 0.0)
                            stack.Push(0.0);
                        else
                            stack.Push(leftHandSide / rightHandSide);
                    }
                    if (curr.Equals("]"))
                    {
                        double leftHandSide = stack.Pop();
                        if (leftHandSide == 0.0)
                        {
                            GlobalState.logError.log("part of the performance-influence model leads to a NegativeInfinity (compute log(0)) ");
                            stack.Push(0.0);
                        }
                        else
                        {
                            stack.Push(Math.Log(leftHandSide, 10.0));
                        }
                    }
                }

            }

            return stack.Pop();
        }

        private double evaluationOfRPN(Dictionary<NumericOption, double> config)
        {
            int counter = 0;

            if (expressionArray.Length == 0)
                return 0;

            Stack<double> stack = new Stack<double>();

            while (counter < expressionArray.Length)
            {
                string curr = expressionArray[counter];
                counter++;

                if (!InfluenceFunction.isOperatorEval(curr))
                {


                    stack.Push(getValueOfToken(config, curr, varModel));
                }
                else
                {


                    if (curr.Equals("+"))
                    {
                        if (stack.Count < 2)
                            Console.WriteLine("Error. Possible reason: name of numeric option does not match the name in the stepsize XML-tag");

                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        stack.Push(leftHandSide + rightHandSide);
                    }
                    if (curr.Equals("-"))
                    {
                        if (stack.Count < 2)
                            Console.WriteLine("Error. Possible reason: name of numeric option does not match the name in the stepsize XML-tag");

                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        stack.Push(leftHandSide - rightHandSide);
                    }

                    if (curr.Equals("*"))
                    {
                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        stack.Push(leftHandSide * rightHandSide);
                    }
                    if (curr.Equals("/"))
                    {
                        double rightHandSide = stack.Pop();
                        double leftHandSide = stack.Pop();
                        if (leftHandSide == 0.0 || rightHandSide == 0.0)
                            stack.Push(0.0);
                        else
                            stack.Push(leftHandSide / rightHandSide);
                    }
                    if (curr.Equals("]"))
                    {
                        double leftHandSide = stack.Pop();
                        if (leftHandSide == 0.0)
                        {
                            stack.Push(Double.MinValue);
                        }
                        else
                        {
                            stack.Push(Math.Log(leftHandSide, 10.0));
                        }
                    }
                }

            }

            return stack.Pop();
        }


        private static double getValueOfToken(Configuration config, string token, VariabilityModel fm)
        {
            token = token.Trim();

            double value = 0.0;

            if (Double.TryParse(token, out value))
            {
                return value;
            }

            NumericOption numOpt = fm.getNumericOption(token);
            if (numOpt != null)
            {
                return config.NumericOptions[numOpt];
            }
            BinaryOption binOpt = fm.getBinaryOption(token);

            if (token.StartsWith("Enabled") || token.StartsWith("Disabled"))
            {
                binOpt = getAbstractOption(token, fm);
            }

            if (binOpt != null)
            {
                if (token.Equals("base") || token.Equals("root"))
                    return 1.0;

                if (config.BinaryOptions.Keys.Contains(binOpt) && config.BinaryOptions[binOpt] == BinaryOption.BinaryValue.Selected)
                    return 1.0;
                else
                {
                    foreach (BinaryOption option in config.BinaryOptions.Keys)
                    {
                        // option has to be selected in the configuration
                        if (option.Name == binOpt.Name && config.BinaryOptions[option] == BinaryOption.BinaryValue.Selected)
                        {
                            return 1.0;
                        }
                    }
                }

            }
            return 0.0;

        }

        private static BinaryOption getAbstractOption(String token, VariabilityModel vm)
        {
            NumericOption numOpt;
            if (token.StartsWith("Enabled"))
            {
                numOpt = vm.getNumericOption(token.Substring(7));
                if (numOpt != null)
                    return numOpt.abstractEnabledConfigurationOption();
            }
            else
            {
                numOpt = vm.getNumericOption(token.Substring(8));
                if (numOpt != null)
                    return numOpt.abstractDisabledConfigurationOption();
            }
            return null;
        }

        private double getValueOfToken(Dictionary<NumericOption, double> config, string token, VariabilityModel varModel)
        {
            token = token.Trim();

            double value = 0.0;

            if (Double.TryParse(token, out value))
            {
                return value;
            }

            if (varModel == null)
            {
                if (token.Equals(this.numOption.Name))
                    return config[this.numOption];
            }
            else
            {
                NumericOption numOpt = varModel.getNumericOption(token);
                if (numOpt != null)
                    return (config[numOpt]);
            }
            return 0.0;

        }

        private static bool operatorHasGreaterPrecedence(string thisToken, string otherToken)
        {
            thisToken = thisToken.Trim();
            otherToken = otherToken.Trim();

            if (thisToken.Equals("*") && otherToken.Equals("+"))
                return true;

            if (thisToken.Equals("/") && otherToken.Equals("+"))
                return true;

            if (thisToken.Equals("*") && otherToken.Equals("-"))
                return true;

            if (thisToken.Equals("/") && otherToken.Equals("-"))
                return true;

            return false;
        }

        /// <summary>
        /// Retuns the number of configuration options participating in the function. If an option apears multiple times in the function, it is counted multiple times.
        /// </summary>
        /// <returns>The overall number of configuration-option occurrences in the function.</returns>
        public int getNumberOfParticipatingOptions()
        {
            return numberOfParticipatingFeatures;
        }

        /// <summary>
        /// Returns the number of different configuration options participating in the function. Mutliple occurrences of an option are ignored. 
        /// </summary>
        /// <returns>The number of distinct options participating in the function.</returns>
        public int getNumberOfDistinctParticipatingOptions()
        {
            return this.participatingBoolOptions.Count + this.participatingNumOptions.Count;
        }

        private bool tokenIsAFeatureOrNumber(string token, VariabilityModel varModel)
        {
            token = token.Trim();

            double value = 0.0;

            if (Double.TryParse(token, out value))
            {
                return true;
            }

            if (varModel != null)
            {
                NumericOption numOption = varModel.getNumericOption(token);
                if (numOption != null)
                {
                    if (!participatingNumOptions.Contains(numOption))
                        this.participatingNumOptions.Add(numOption);
                    numberOfParticipatingFeatures++;
                    return true;
                }

                BinaryOption binOption = varModel.getBinaryOption(token);

                if (token.StartsWith("Enabled") || token.StartsWith("Disabled"))
                {
                    binOption = getAbstractOption(token, varModel);
                }

                if (binOption != null)
                {
                    if (!participatingBoolOptions.Contains(binOption))
                        this.participatingBoolOptions.Add(binOption);
                    numberOfParticipatingFeatures++;
                    return true;
                }
            }
            else
            {
                if (token.Equals(this.numOption.Name))
                    return true;
            }
            return false;
        }

        private static bool isOperator(string token)
        {
            token = token.Trim();

            // schöner machen 

            if (token.Equals("+"))
                return true;

            if (token.Equals("-"))
                return true;

            if (token.Equals("*"))
                return true;

            if (token.Equals("/"))
                return true;

            return false;
        }

        public static bool isOperatorEval(string token)
        {
            token = token.Trim();

            // schöner machen 

            if (token.Equals("]"))
                return true;

            return isOperator(token);
        }

        /// <summary>
        /// Returns the variability model the influence function is defined for.
        /// </summary>
        /// <returns>The variability model of the influence function.</returns>
        public VariabilityModel getVariabilityModel()
        {
            return this.varModel;
        }


        /// <summary>
        /// The method evaluates the influence function with the selection of numerical-feature values. This method 
        /// should only been used if no binary configuration option is considered in the influence function. 
        /// </summary>
        /// <param name="values">A set of numerical options with values.</param>
        /// <returns></returns>
        public double eval(Dictionary<NumericOption, double> values)
        {
            Random r = new Random();

            double realValue = this.evaluationOfRPN(values);
            return realValue + (realValue * noise * r.NextDouble());
        }


        /// <summary>
        /// The method evaluates the influence function with the configuration. 
        /// </summary>
        /// <param name="config">A configurtion.</param>
        /// <returns>The value of the influence function for the configuration.</returns>
        public double eval(Configuration config)
        {
            Random r = new Random();

            double realValue = this.evaluationOfRPN(config);
            return realValue + (realValue * noise * r.NextDouble());
        }


        /// <summary>
        /// Adds a noise to the influence function. The strength of the noise is defined in percentage share of the value predicted
        /// without noise. 
        /// </summary>
        /// <param name="noise">noise </param>
        public void setNoise(double noise)
        {
            this.noise = noise;
        }

        /// <summary>
        /// Returns the textual representation of the <see cref="InfluenceFunction"/>.
        /// </summary>
        /// <returns>The textual representation of the <see cref="InfluenceFunction"/>.</returns>
        public override String ToString()
        {
            String returnString = wellFormedExpression;

            returnString = returnString.Replace("[", "log10(");
            returnString = returnString.Replace("]", ")");

            return returnString;
        }

        private static int findOffsetToClosingBracket(String subStringAfterOpeningBracket)
        {
            int offset = 0;
            int numberOfOpeningBrackets = 0;
            for (int i = 0; i < subStringAfterOpeningBracket.Length; i++)
            {
                char curr = subStringAfterOpeningBracket[i];
                if (curr.Equals('('))
                    numberOfOpeningBrackets++;

                if (curr.Equals(')') && numberOfOpeningBrackets > 0)
                    numberOfOpeningBrackets--;

                else if (curr.Equals(')') && numberOfOpeningBrackets == 0)
                {
                    offset = i;
                    break;
                }
            }
            return offset;
        }

        /// <summary>
        /// Returns whether the influence function coints a specific configuration option. 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public bool containsOption(ConfigurationOption option)
        {
            if (this.participatingBoolOptions.Contains(option))
                return true;

            if (this.participatingNumOptions.Contains(option))
                return true;
            return false;
        }

        //public void filterFeatureModelToExpression()
        //{
        //    bool actionDone = true;
        //    List<BinaryOption> featuresToRemove = new List<Element>();
        //    foreach (Element feature in this.fm.getAllElements())
        //    {
        //        if (!this.participatingBoolFeatures.Contains(feature))
        //            featuresToRemove.Add(feature);
        //    }
        //    foreach (Element f in featuresToRemove)
        //        this.fm.removeElement(f);

        //    List<NumericOption> numOptionsToRemove = new List<NumericOption>();
        //    foreach (NumericOption no in this.fm.numericOptions)
        //    {
        //        if (!this.participatingNumFeatures.Contains(no))
        //            numOptionsToRemove.Add(no);
        //    }

        //    foreach (NumericOption f in numOptionsToRemove)
        //        this.fm.removeNumericOption(f);
        //}

        /// <summary>
        /// Checks using the ToString method whether this InfluenceFunction is equal to another one
        /// </summary>
        /// <param name="other">The other influence function to be compared with</param>
        /// <returns>True if the same, false otherwise</returns>
        public bool Equals(InfluenceFunction other)
        {
            if (other.ToString() == this.ToString())
                return true;

            return false;
        }

        internal bool containsParent(InfluenceFunction featureToAdd)
        {
            foreach (BinaryOption binOpt in featureToAdd.participatingBoolOptions)
            {
                if (this.participatingBoolOptions.Contains(binOpt))
                    return true;
                BinaryOption parent = (BinaryOption)binOpt.Parent;
                while (parent != null && parent != varModel.Root)
                {
                    if (this.participatingBoolOptions.Contains(parent))
                        return true;
                    parent = (BinaryOption)parent.Parent;
                }
            }

            return false;
        }

    }
}
