﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    /// <summary>
    /// This design selects a speficied number of value combiantions for a set of numberic options. The value combinations are created using a random selection of values of the numeric options.
    /// </summary>
    public class RandomSampling : ExperimentalDesign
    {
        private int seed;

        private int sampleSize;

        public RandomSampling(List<NumericOption> options)
            : base(options)
        {
        }

        public RandomSampling(int seed = 0, int sampleSize = 10)
        {

        }

        public void setSeed(int seed)
        {
            this.seed = seed;
        }

        public override string getName()
        {
            return "RANDOM";
        }

        public override string getTag()
        {
            return "RANDN";
        }

        public override void setSamplingParameters(Dictionary<string, string> parameterNameToValue)
        {
            if (parameterNameToValue.ContainsKey("seed"))
            {
                seed = parseFromParameters(parameterNameToValue, "seed");
            }
            if (parameterNameToValue.ContainsKey("sampleSize"))
            {
                sampleSize = parseFromParameters(parameterNameToValue, "sampleSize");
            }
        }

        /// <summary>
        /// Computes the design using the default parameters. 
        /// </summary>
        /// <returns>True if the design could be computed using the desired parameters.</returns>
        public override bool computeDesign()
        {
            return compute();
        }

        private bool compute()
        {
            Random rand = new Random(seed);
            this.selectedConfigurations = new List<Dictionary<NumericOption, double>>();

            Dictionary<NumericOption, List<double>> optionsWithValues = new Dictionary<NumericOption, List<double>>();

            // select random values of numeric options
            if (options.Count == 0)
                return false;
            foreach (NumericOption no in options)
            {
                List<double> values = new List<double>();
                for (int i = 0; i < sampleSize; i++)
                {
                    values.Add(no.getValueForStep((int)(rand.NextDouble() * (no.getNumberOfSteps()))));
                }
                optionsWithValues.Add(no, values);
            }

            List<String> configsCode = new List<string>(); // test to not insert configs twice

            // generate configs
            for (int i = 0; i < sampleSize; i++)
            {
                String codeCurrConfig = "";
                Dictionary<NumericOption, double> currConfig = new Dictionary<NumericOption, double>();
                foreach (NumericOption num in this.options)
                {
                    currConfig.Add(num, optionsWithValues[num][i]);
                    codeCurrConfig += optionsWithValues[num][i];
                }

                if (configsCode.Contains(codeCurrConfig))
                {
                    // config already used
                    foreach (NumericOption num in options)
                    {
                        optionsWithValues[num][i] = num.getValueForStep((int)(rand.NextDouble() * (num.getNumberOfSteps())));
                    }
                    i--;
                }
                else
                {
                    configsCode.Add(codeCurrConfig);
                    selectedConfigurations.Add(currConfig);
                }
            }


            return true;
        }

        public override string parameterIdentifier()
        {
            return "seed-" + seed + "_" + "sampleSize-" + sampleSize + "_";
        }
    }
}
