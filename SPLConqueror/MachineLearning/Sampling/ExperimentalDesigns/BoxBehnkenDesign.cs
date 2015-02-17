using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class BoxBehnkenDesign : ExperimentalDesign
    {

        int[,] matrix;

        public BoxBehnkenDesign(List<NumericOption> options)
            : base(options)
        {
        }

        public override Dictionary<string, string> getParameterTypes()
        {
            return new Dictionary<string, string>();
        }

        public override string getName()
        {
            return "BoxBehnkenDesign";
        }

        public override bool computeDesign()
        {
            int k = this.options.Count;

            List<int> items = new List<int>();
            items.AddRange(Enumerable.Range(0, k));

            List<Tuple<int, int>> combinations = combinate(items);
            matrix = new int[combinations.Count() * 4 + 1, k]; // TODO > add multiple center point runs?

            int offset = 0;
            foreach (Tuple<int, int> c in combinations)
            {
                matrix[offset, c.Item1] = -1;
                matrix[offset, c.Item2] = -1;
                matrix[offset + 1, c.Item1] = +1;
                matrix[offset + 1, c.Item2] = -1;
                matrix[offset + 2, c.Item1] = -1;
                matrix[offset + 2, c.Item2] = +1;
                matrix[offset + 3, c.Item1] = +1;
                matrix[offset + 3, c.Item2] = +1;
                offset += 4;
            }

            this.selectedConfigurations = new List<Dictionary<NumericOption, double>>();
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Dictionary<NumericOption, double> run = new Dictionary<NumericOption, double>();

                int j = 0;
                foreach (NumericOption vf in this.options)
                {
                    double delta = vf.Max_value - vf.Min_value;
                    if (matrix[i, j] == 0)
                    {
                        run.Add(vf, vf.getCenterValue());
                    }
                    else if (matrix[i, j] < 0)
                    {
                        run.Add(vf, vf.Min_value);
                    }
                    else
                    {
                        run.Add(vf, vf.Max_value);
                    }
                    j++;
                }

                selectedConfigurations.Add(run);
            }
            return true;
        }

        public override bool computeDesign(Dictionary<string, string> options)
        {
            return computeDesign();
        }

        // Find all possible combinations - TODO > improve?
        private List<Tuple<int, int>> combinate(List<int> items)
        {
            List<Tuple<int, int>> combinations = new List<Tuple<int, int>>();

            foreach (int i1 in items)
            {
                foreach (int i2 in items)
                {
                    if (i1 != i2 && !(combinations.Contains(Tuple.Create(i1, i2)) || combinations.Contains(Tuple.Create(i2, i1))))
                    {
                        combinations.Add(Tuple.Create(i1, i2));
                    }
                }
            }

            return combinations;
        }
    }
}
