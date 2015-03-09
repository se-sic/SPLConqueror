using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class OneFactorAtATime : ExperimentalDesign
    {
        public int distinctValuesPerOption = 3;

        private static Dictionary<string, string> parameter = new Dictionary<string, string>();
        private List<NumericOption> optionsToConsider;
        static OneFactorAtATime()
        {
            parameter.Add("distinctValuesPerOption", "int");
        }

        public override Dictionary<string, string> getParameterTypes()
        {
            return OneFactorAtATime.parameter;
        }

        public OneFactorAtATime(List<NumericOption> optionsToConsider)
            : base(optionsToConsider)
        {
        }

        public override bool computeDesign(Dictionary<string, string> designOptions)
        {
            int distinctValuesPerOption = 3;

            foreach (KeyValuePair<string, string> param in designOptions)
            {
                if (param.Key == "distinctValuesPerOption")
                    distinctValuesPerOption = Convert.ToInt32(param.Value);
            }
            return computeDesign(distinctValuesPerOption);
        }

        public OneFactorAtATime(String s)
            : base(s)
        {
            if (this.designParameter.ContainsKey("distinctValuesPerOption"))
                this.distinctValuesPerOption = Int32.Parse(this.designParameter["distinctValuesPerOption"]);
        }

        public override bool computeDesign()
        {
            return computeDesign(distinctValuesPerOption);
        }

        public override string getName()
        {
            return "OneFactorAtATime";
        }

        public Dictionary<NumericOption, double> computeCenterPoint()
        {
            Dictionary<NumericOption, double> centerPoint = new Dictionary<NumericOption, double>();
            foreach (NumericOption vf in options)
            {
                centerPoint.Add(vf, vf.getCenterValue());
            }

            return centerPoint;
        }

        public bool computeDesign(int distinctValuesPerOption)
        {
            Dictionary<NumericOption, double> centerPoints = computeCenterPoint();

            Dictionary<NumericOption, List<double>> values = new Dictionary<NumericOption, List<double>>();


            foreach (NumericOption vf in this.options)
            {
                //Getting values for learning the variable feature
                List<double> valuesOneOption = ExperimentalDesign.sampleOption(vf, (int)Math.Min(distinctValuesPerOption, vf.getNumberOfSteps()), false);
                valuesOneOption.Remove(vf.getCenterValue());


                foreach (double currValue in valuesOneOption)
                {
                    Dictionary<NumericOption, double> oneSample = new Dictionary<NumericOption, double>();
                    oneSample.Add(vf, currValue);
                    foreach (NumericOption other in this.options)
                    {
                        if(other.Equals(vf))
                            continue;
                        oneSample.Add(other, centerPoints[other]);
                    }
                    this.selectedConfigurations.Add(oneSample);
                }
            }
            return true;
        }

    }
}
