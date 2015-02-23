using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    public class Feature : InfluenceFunction , IEquatable<Feature>, IComparer<Feature>
    {
        private double constant = 1.0;
        private String name = "";

        public String Name
        {
            get { return name; }
        }

        public double Constant
        {
            get { return constant; }
            set { constant = value; }
        }

        private int hashCode;

        public bool Equals(Feature f)
        {
            return this.Equals((Object)f);
        }

        /// <summary>
        /// Compares two features based on the components of the functions. 
        /// </summary>
        /// <param name="obj">The object to comare with.</param>
        /// <returns>True if both features represents the same configuration option combination, false otherwise.</returns>
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
            hashCode = initHashCode();
        }


        internal Feature(String expression, VariabilityModel vm) : base(expression, vm) { }

        public override string ToString()
        {
            return base.ToString();
        }

        private int initHashCode()
        {
            string[] thisFuntion = this.wellFormedExpression.Split('*');
            Array.Sort<string>(thisFuntion);

            StringBuilder sb = new StringBuilder();
            for(int i = 0; i <thisFuntion.Length; i++)
            {
                sb.Append(thisFuntion[i]);
                if (i < thisFuntion.Length - 1)
                    sb.Append("#");
            }

            name = sb.ToString();

            return name.GetHashCode();
        }


        public int Compare(Feature x, Feature y)
        {
            return x.name.CompareTo(y.name);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }
}
