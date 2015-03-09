using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class HyperSampling : ExperimentalDesign
    {

        private static Dictionary<string, string> parameter = new Dictionary<string, string>(); 
        static HyperSampling(){
            parameter.Add("precision", "double");
        }
       
        public override Dictionary<string, string> getParameterTypes()
        {
            return HyperSampling.parameter;
        }

        public override string getName()
        {
            return "HyperSampling";
        }

        public HyperSampling(List<NumericOption> options)
            : base(options)
        {
        }

        public HyperSampling(String s) : base (s)
        {
            if (this.designParameter.ContainsKey("precision"))
                this.precision = Int32.Parse(this.designParameter["precision"]);
        }

        private int precision = 50;

        public int Precision
        {
            get { return precision; }
            set { precision = value; }
        }
        
        
        /*
         * in: int precision; defines the densitiy of the grid. range from 0 - 100. 100 means full variant space. 0 means 0 variants.
         * out: bool; whether the computation was sucessfull.
         * Approach: For each binary configuration exist x configurations for metric configuration options
         * But we need not all x configurations but a percentage of all x configurations. precision acts as the percentage value.
         * We scale the percentage for all metric options, but use at least 2 different values
         * We compute then the metric configurations y and use them for all binary configurations.
         * Finally, we filter invalid metric configurations for binary configurations
         * **************
         */
        private bool computeDesign(double precision)
        {
            if (precision <= 0 || precision > 100)
                return false;
            double percentage = 0;
            if (precision > 1)
                percentage = (double)(Convert.ToDouble(precision) / 100.0);
            else
                percentage = precision;
            Dictionary<NumericOption, List<double>> valuesOfMetricOption = new Dictionary<NumericOption, List<double>>();


            List<List<double>> allMetricValues = new List<List<double>>();

            foreach (NumericOption vf in this.options)
            {
                ////set number of sampling values according to the percentage


                List<double> values = sampleOption(vf, setSamplingValue(vf, percentage), true);

                valuesOfMetricOption.Add(vf, values);
                allMetricValues.Add(values);
            }

            double[] result = new double[allMetricValues.Count];
            Recurse(result, 0, allMetricValues);

            return true;
        }

        /// <summary>
        /// Computes the sampling points using the default parameters of this design. 
        /// </summary>
        /// <returns></returns>
        public override bool computeDesign()
        {
            return computeDesign(precision);
        }

        /// <summary>
        /// This mehtod computes the samplings using the specified parameters. The option to specify is "precision". The value of this parameter specifies the percentage share of 
        /// values of the cpnfiguration option considered. A int value of [0,100] is needed to specify this parameter.
        /// </summary>
        /// <param name="designOptions">Paremeter of the design. </param>
        /// <returns>True if the design could be computed.</returns>
        public override bool computeDesign(Dictionary<string, string> designOptions)
        {
            double precision = 50;

            foreach (KeyValuePair<string, string> param in designOptions)
            {
                if (param.Key == "precision")
                    precision = Convert.ToInt32(param.Value);
            }
            return computeDesign(precision);
        }

        protected long runningIndex = 0;
        protected List<long> takenConfigs = new List<long>();
        
        protected void Recurse<TList>(double[] selected, int index, IEnumerable<TList> remaining) where TList : IEnumerable<double>
        {
            if (this.selectedConfigurations.Count > 1000)
            {
                GlobalState.logInfo.log("Found more than 1000 numeric configurations. Use only 1000.");
                return;
            }
                
            IEnumerable<double> nextList = remaining.FirstOrDefault();
            if (nextList == null)
            {
                if (takenConfigs.Contains(runningIndex) || takenConfigs.Count == 0)
                {
                    Dictionary<NumericOption, double> tempDict = new Dictionary<NumericOption, double>();
                    for (int i = 0; i < this.options.Count; i++)
                    {
                        tempDict.Add(this.options[i], selected[i]);
                    }
                    this.selectedConfigurations.Add(tempDict);
                }
                runningIndex++;
            }
            else
            {
                foreach (double i in nextList)
                {
                    selected[index] = i;
                    Recurse(selected, index + 1, remaining.Skip(1));
                }
            }
        }

        /*
         * in: VariableFeature vf; is the feature for which we have to know the sampling values
         * in: double percentage; range 0..1 the percentage of all values we need
         * out: void
         * Sets the maximum sampling size according to the percentage. The logarithm must be used, because the values of a single metric value multplies with the values of all others leading to an exponential number
         * *****************/
        protected int setSamplingValue(NumericOption vf, double percentage)
        {

            double number = vf.getNumberOfSteps() * percentage;
            if (number < this.minNumberOfSamplingsPerNumericOption)
                return this.minNumberOfSamplingsPerNumericOption;
            
            return Convert.ToInt32(number);
        }
    }
}
