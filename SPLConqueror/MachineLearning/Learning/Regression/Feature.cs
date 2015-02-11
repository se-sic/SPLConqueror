using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    internal class Feature : InfluenceFunction
    {
        internal double constant = 1.0;

        internal override bool Equals(object obj)
        {
            Feature other = (Feature) obj;

            string[] thisFuntion = this.wellFormedExpression.Split('*');
            string[] otherFunction = other.wellFormedExpression.Split('*');

            if (thisFuntion.Length != otherFunction.Length)
                return false;

            for (int i = 0; i < otherFunction.Length; i++)
            {
                otherFunction[i] = otherFunction[i].Trim(); 
            }

            foreach (string thisPart in thisFuntion)
            {
                string x = thisPart.Trim();
                if (!otherFunction.Contains(x))
                {
                    return false;
                }
            }


            return false;
        }

        internal Feature(Feature original, Feature toAdd, VariabilityModel vm)
            : base(original.ToString() + " * " + toAdd.ToString(), vm)
        {

        }

        internal Feature(String expression, VariabilityModel vm) : base(expression, vm) { }
    }
}
 