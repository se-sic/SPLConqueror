using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class OneFactorAtATime : ExperimentalDesign
    {
        private int distinctValuesPerOption;

        /// <summary>
        /// Creates a new istance considering the provided numeric configuration options.
        /// </summary>
        /// <param name="optionsToConsider">The set of configuration options that are considered in this design.</param>
        public OneFactorAtATime(List<NumericOption> optionsToConsider)
            : base(optionsToConsider)
        {
        }

        /// <summary>
        /// Creates a new instance considering the distinctValuePerOption parameter.
        /// </summary>
        /// <param name="distinctValuePerOption">Distince value per option parameter.</param>
        public OneFactorAtATime(int distinctValuePerOption = 3) : base()
        {
            this.distinctValuesPerOption = distinctValuePerOption;
        } 

        public OneFactorAtATime(String s)
            : base(s)
        {
            if (this.designParameter.ContainsKey("distinctValuesPerOption"))
                this.distinctValuesPerOption = Int32.Parse(this.designParameter["distinctValuesPerOption"]);
        }

        public override void setSamplingParameters(Dictionary<string, string> parameterNameToValue)
        {
            if (parameterNameToValue.ContainsKey("distinctValuesPerOption"))
            {
                this.distinctValuesPerOption = parseFromParameters(parameterNameToValue, "distinctValuesPerOption");
            }
        }

        public override bool computeDesign()
        {
            return computeDesign(distinctValuesPerOption);
        }

        public override string getName()
        {
            return "ONEFACTORATATIME";
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

            this.selectedConfigurations.Add (centerPoints);

            foreach (NumericOption vf in this.options)
            {
                //Getting values for learning the variable feature
                List<double> valuesOneOption = ExperimentalDesign.sampleOption(vf, (int)Math.Min(distinctValuesPerOption - 1, vf.getNumberOfSteps()), true);
                if(valuesOneOption.Contains(vf.getCenterValue()))
                {
                    valuesOneOption = ExperimentalDesign.sampleOption(vf, (int)Math.Min(distinctValuesPerOption, vf.getNumberOfSteps()), true);
                    valuesOneOption.Remove(vf.getCenterValue());
                }
                    
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

        public override string parameterIdentifier()
        {
            return "";
        }
    }
}
