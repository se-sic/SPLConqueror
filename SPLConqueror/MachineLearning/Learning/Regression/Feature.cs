using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    public class Feature : InfluenceFunction
    {
        private double constant = 1.0;

        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        /// <summary>
        /// Compares two features based on the components of the functions. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Feature other = (Feature) obj;

            string[] thisFuntion = this.wellFormedExpression.Split('*');
            string[] otherFunction = other.wellFormedExpression.Split('*');

            if (thisFuntion.Length != otherFunction.Length)
                return false;

            Dictionary<string, int> thisParts = new Dictionary<string, int>();
            foreach (string part in thisFuntion)
            {
                if (thisParts.ContainsKey(part))
                {
                    int value = thisParts[part] + 1;
                    thisParts.Remove(part);
                    thisParts.Add(part, value);
                }
                else
                {
                    thisParts.Add(part, 1);
                }
            }


            foreach (string part in otherFunction)
            {
                if (thisParts.ContainsKey(part))
                {
                    int remainingNumber = thisParts[part] - 1;
                    thisParts.Remove(part);
                    if (remainingNumber > 0)
                    {
                        thisParts.Add(part, remainingNumber);
                    }

                }
                else
                {
                    return false;
                }
            }

            if (thisParts.Count > 0)
                return false;

            return true;
        }

        public Feature(Feature original, Feature toAdd, VariabilityModel vm)
            : base(original.ToString() + " * " + toAdd.ToString(), vm)
        {

        }
        internal Feature(String expression, VariabilityModel vm) : base(expression, vm) { }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
