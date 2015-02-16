using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class FullFactorialDesign : ExperimentalDesign
    {

        public FullFactorialDesign(List<NumericOption> options)
            : base(options)
        {
        }

        public override Dictionary<string, string> getParameterTypes()
        {
            return new Dictionary<string, string>();
        }

        public override string getName()
        {
            return "FullFactorialDesign";
        }


        public override bool computeDesign()
        {

            
            Dictionary<NumericOption, List<double>> allValuesOfNumericOptions = new Dictionary<NumericOption, List<double>>();
            foreach (NumericOption vf in this.options)
            {
                allValuesOfNumericOptions.Add(vf, vf.getAllValues());
            }

            this.selectedConfigurations = getAllPossibleCombinations(allValuesOfNumericOptions);


            return true;
        }

        public override bool computeDesign(Dictionary<string, string> options)
        {
            return computeDesign();
        }

        private List<Dictionary<NumericOption, double>> getAllPossibleCombinations(Dictionary<NumericOption, List<double>> elementValuePairs)
        {
            List<Dictionary<NumericOption, double>> allCombiantions = new List<Dictionary<NumericOption, double>>();
            int[] positions = new int[elementValuePairs.Keys.Count];

            int featureToIncrement = 0;
            Boolean notIncremented = true;

            do
            {
                notIncremented = true;

                // tests whether all combinations are computed
                if (featureToIncrement == elementValuePairs.Keys.Count)
                {
                    return allCombiantions;
                }
                Dictionary<NumericOption, double> values = getValues(elementValuePairs, positions);
                allCombiantions.Add(values);

                do
                {
                    if (positions[featureToIncrement] == elementValuePairs.ElementAt(featureToIncrement).Value.Count - 1)
                    {
                        positions[featureToIncrement] = 0;
                        featureToIncrement++;
                    }
                    else
                    {
                        positions[featureToIncrement] = positions[featureToIncrement] + 1;
                        notIncremented = false;
                        featureToIncrement = 0;
                    }

                } while (notIncremented && !(featureToIncrement == elementValuePairs.Keys.Count));

            } while (true);
        }

        private Dictionary<NumericOption, double> getValues(Dictionary<NumericOption, List<double>> optionValuePairs, int[] positions)
        {
            Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < optionValuePairs.Count; i++)
            {
                numericOptions.Add(optionValuePairs.ElementAt(i).Key, optionValuePairs.ElementAt(i).Value[positions[i]]);
            }
            return numericOptions;
        }
    }
}
