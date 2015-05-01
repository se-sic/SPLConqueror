using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class NonBooleanConstraint
    {
        private InfluenceFunction leftHandSide = null;
        private InfluenceFunction rightHandSide = null;
        private String comparator = "";

        /// <summary>
        /// Creates a new NonBooleanConstraint for a expression. The expression have to consist binary and numeric options and operators such as "+,*,>=,<=,>,<, and =" only. 
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
            rightHandSide = new InfluenceFunction(parts[parts.Length-1], varModel);


        }

        public bool configIsValid(Configuration config)
        {
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

    }
}
