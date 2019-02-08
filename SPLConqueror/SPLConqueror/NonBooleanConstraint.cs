using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{


    /// <summary>
    /// This class represents a non boolean constraint contraining the configuration space of the variability model. 
    /// </summary>
    public class NonBooleanConstraint
    {
        public InfluenceFunction leftHandSide { protected set; get; } = null;
        public InfluenceFunction rightHandSide { protected set; get; } = null;
        public String comparator { private set; get; } = "";

        /// <summary>
        /// Creates a new NonBooleanConstraint for a expression. The expression have to consist binary and numeric options and operators such as +, *, &lt;=, &lt;, &gt;=, &gt;, and = only. 
        /// Where all binary and numeric options have to be defined in the variability model. 
        /// </summary>
        /// <param name="unparsedExpression"></param>
        /// <param name="varModel"></param>
        public NonBooleanConstraint(String unparsedExpression, VariabilityModel varModel)
        {
            if (unparsedExpression.Contains(">="))
            {
                comparator = ">=";
            }
            else if (unparsedExpression.Contains("<="))
            {
                comparator = "<=";
            }
            else if (unparsedExpression.Contains("!="))
            {
                comparator = "!=";
            }
            else if (unparsedExpression.Contains("="))
            {
                comparator = "=";
            }
            else if (unparsedExpression.Contains(">"))
            {
                comparator = ">";
            }
            else if (unparsedExpression.Contains("<"))
            {
                comparator = "<";
            }

            String[] parts = unparsedExpression.Split(comparator.ToCharArray());
            leftHandSide = new InfluenceFunction(parts[0], varModel);
            rightHandSide = new InfluenceFunction(parts[parts.Length - 1], varModel);


        }

        /// <summary>
        /// Tests whether a configuration holds for the given non-boolean constraint.
        /// </summary>
        /// <param name="config">The configuration of interest.</param>
        /// <returns>True is the configuration holds for the constraint.</returns>
        public bool configIsValid(Configuration config)
        {
            if (!configHasOptionsOfConstraint(config))
                return true;


            double left = leftHandSide.eval(config);
            double right = rightHandSide.eval(config);

            switch (comparator)
            {
                case ">=":
                    {
                        if (left >= right)
                            return true;
                        break;
                    }
                case "<=":
                    {
                        if (left <= right)
                            return true;
                        break;
                    }
                case "!=":
                    {
                        if (left != right)
                            return true;
                        break;
                    }
                case "=":
                    {
                        if (left == right)
                            return true;
                        break;
                    }
                case ">":
                    {
                        if (left > right)
                            return true;
                        break;
                    }
                case "<":
                    {
                        if (left < right)
                            return true;
                        break;
                    }
            }

            return false;
        }

        /// <summary>
        /// Tests whether the given partial configuraion (consistsing only of the numeric-configuration options and their selected value) holds
        /// for the given non-functional constraint.
        /// </summary>
        /// <param name="config">A parial configuration consisting of the numeric-configurations options and their selected values.</param>
        /// <returns>True if the partial configuration holds for the non-functional property.</returns>
        public bool configIsValid(Dictionary<NumericOption, double> config)
        {
            if (!configHasOptionsOfConstraint(config))
                return true;


            double left = leftHandSide.eval(config);
            double right = rightHandSide.eval(config);

            switch (comparator)
            {
                case ">=":
                    {
                        if (left >= right)
                            return true;
                        break;
                    }
                case "<=":
                    {
                        if (left <= right)
                            return true;
                        break;
                    }
                case "=":
                    {
                        if (left == right)
                            return true;
                        break;
                    }
                case ">":
                    {
                        if (left > right)
                            return true;
                        break;
                    }
                case "<":
                    {
                        if (left < right)
                            return true;
                        break;
                    }
            }

            return false;
        }


        /// <summary>
        /// Checks if a configuration violates a non boolean constraint.
        /// </summary>
        /// <param name="config">The configuration that will be checked.</param>
        /// <returns>Returns true if not all options of the constraint are present
        ///          or if the configuration violates the constraint. Else returns
        ///          false.
        /// </returns>
        protected bool configIsValidNeg(Configuration config)
        {
            if (!configHasOptionsOfConstraint(config))
            {
                return true;
            }
            else
            {
                return !configIsValid(config);
            }

        }

        private bool configHasOptionsOfConstraint(Configuration config)
        {
            foreach (BinaryOption bo in leftHandSide.participatingBoolOptions.Union(rightHandSide.participatingBoolOptions))
            {
                if (!config.BinaryOptions.ContainsKey(bo))
                    return false;
            }

            foreach (NumericOption no in leftHandSide.participatingNumOptions.Union(rightHandSide.participatingNumOptions))
            {
                if (!config.NumericOptions.ContainsKey(no))
                    return false;
            }

            return true;
        }

        private bool configHasOptionsOfConstraint(Dictionary<NumericOption, double> config)
        {
            foreach (NumericOption no in leftHandSide.participatingNumOptions.Union(rightHandSide.participatingNumOptions))
            {
                if (!config.ContainsKey(no))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Returns the string representation of the constraint consisting of the left hand side, the comparator and the right hand side of the 
        /// constraint.
        /// </summary>
        /// <returns>The string representation of the constraint.</returns>
        public override string ToString()
        {
            return leftHandSide + " " + comparator + " " + rightHandSide;
        }
    }
}
