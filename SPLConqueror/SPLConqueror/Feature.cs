using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace SPLConqueror_Core
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
            return base.ToString().Equals(f.getPureString());
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
            : base(original.getPureString() + " * " + toAdd.getPureString(), vm)
        {
            hashCode = initHashCode();
        }


        public Feature(String expression, VariabilityModel vm) : base(expression, vm) {
            hashCode = initHashCode();
        }

        public override string ToString()
        {
            return this.Constant + " * " + base.ToString();
        }

        private int initHashCode()
        {
            string[] thisFunction = this.wellFormedExpression.Split('*').Select(element => element.Trim()).ToArray();
            Array.Sort<string>(thisFunction);
            var sb = new StringBuilder();
            string separator = "";
            foreach(string element in thisFunction)
            {
                sb.Append(separator).Append(element);
                separator = "#";
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

        public String getPureString()
        {
            return base.ToString();
        }
    }
}
