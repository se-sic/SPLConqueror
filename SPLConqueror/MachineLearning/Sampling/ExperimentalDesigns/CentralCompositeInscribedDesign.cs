using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using System.Collections;

namespace MachineLearning.Sampling.ExperimentalDesigns
{

    /// <summary>
    /// The central composite inscribe design. This design is defined for numeric options that have at least five different values. 
    /// </summary>
    public class CentralCompositeInscribedDesign : ExperimentalDesign
    {

        /// <summary>
        /// Creates a new design for the set of numeric options. 
        /// </summary>
        /// <param name="options"></param>
        public CentralCompositeInscribedDesign(List<NumericOption> options)
            : base(options)
        {
        }

        public override Dictionary<string, string> getParameterTypes()
        {
            return new Dictionary<string, string>();
        }
        

        public override string getName()
        {
            return "CentralCompositeInscribedDesign";
        }

        public override bool computeDesign(Dictionary<string, string> options)
        {
            return computeDesign();
        }

        public override bool computeDesign()
        {
            // center point 
            Dictionary<NumericOption, double> centerPoint = new Dictionary<NumericOption, double>();
            foreach (NumericOption vf in this.options)
            {
                centerPoint.Add(vf, vf.getCenterValue());
            }
            this.selectedConfigurations.Add(centerPoint);

            // axial points
            foreach (NumericOption vf in this.options)
            {
                Dictionary<NumericOption, double> minPoint = new Dictionary<NumericOption, double>();
                Dictionary<NumericOption, double> maxPoint = new Dictionary<NumericOption, double>();
                minPoint.Add(vf, vf.Min_value);
                maxPoint.Add(vf, vf.Max_value);
                foreach (NumericOption other in this.options)
                {
                    if (other.Equals(vf))
                        continue;
                    minPoint.Add(other, other.getCenterValue());
                    maxPoint.Add(other, other.getCenterValue());
                }
                this.selectedConfigurations.Add(minPoint);
                this.selectedConfigurations.Add(maxPoint);
            }

            // cube points 
            List<Dictionary<NumericOption, double>> fullFactorialPoints = getTwoLevelFactorial(this.options);

            double rootN = Math.Sqrt(this.options.Count);

            //Two of these designs -- CCC and CCI -- have a special characteristic; they are rotatable. A design is said to be rotatable, if upon rotating the design points about the center point the moments of the distribution of the design remain unchanged. For rotatable Central Composite designs the factor  $\frac{b_i}{a_i}$ must be $\sqrt{n}$. (http://www.iue.tuwien.ac.at/phd/plasun/node32.html)
            // b_i == centerPoint_i - min_i 
            Dictionary<NumericOption, double> lowerCubeValue = new Dictionary<NumericOption, double>();
            Dictionary<NumericOption, double> upperCubeValue = new Dictionary<NumericOption, double>();
            foreach (NumericOption vf in this.options)
            {
                double b_i = vf.getCenterValue() - vf.Min_value;
                double a_i = b_i / rootN;

                double lower = vf.getCenterValue() - a_i;
                double upper = vf.getCenterValue() + a_i;

                lowerCubeValue.Add(vf, vf.nearestValidValue(lower));
                upperCubeValue.Add(vf, vf.nearestValidValue(upper));


            }

            // create the cube points 
            for (int i = 0; i < Math.Pow(this.options.Count, 2); i++)
            {
                BitArray b = new BitArray(new int[] { i });
                Dictionary<NumericOption, double> curr = new Dictionary<NumericOption, double>();
                int pos = 0;
                foreach (NumericOption vf in this.options)
                {
                    if (b[pos])
                    {
                        curr.Add(vf, lowerCubeValue[vf]);
                    }
                    else
                    {
                        curr.Add(vf, upperCubeValue[vf]);
                    }
                    pos++;
                }
                this.selectedConfigurations.Add(curr);

            }
            return true;
        }

        private List<Dictionary<NumericOption, double>> getTwoLevelFactorial(List<NumericOption> keys)
        {
            int k = keys.Count;

            int[,] twolevelfac = new int[(int)Math.Pow(2, k), k];
            int signum = 1;
            int counter = 0;

            for (int i = 0; i < twolevelfac.GetLength(1); i++)
            {
                for (int j = 0; j < twolevelfac.GetLength(0); j++)
                {
                    counter++;

                    twolevelfac[j, i] = -1 * signum;
                    if (counter % (int)Math.Pow(2, i) == 0)
                    {
                        signum *= -1;
                    }
                }
                counter = 0;
                signum = 1;
            }

            List<Dictionary<NumericOption, double>> configs = new List<Dictionary<NumericOption, double>>();

            for (int i = 0; i < twolevelfac.GetLength(0); i++)
            {
                Dictionary<NumericOption, double> run = new Dictionary<NumericOption, double>();

                int j = 0;
                foreach (NumericOption vf in keys)
                {
                    if (twolevelfac[i, j] == -1)
                    {
                        run.Add(vf, vf.Min_value);
                    }
                    else
                    {
                        run.Add(vf, vf.Max_value);
                    }

                    j++;
                }

                configs.Add(run);
            }

            return configs;
        }

    }
}
