using System;
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

        public RandomSampling()
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
            if (parameterNameToValue.ContainsKey("numConfigs"))
            {
                sampleSize = parseFromParameters(parameterNameToValue, "numConfigs");
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
            VariabilityModel numericVariabilityModel = GlobalState.varModel.ReduceModelToNumeric();
            List<ConfigurationOption> numericOptions = new List<ConfigurationOption>();
            foreach (NumericOption numOpt in numericVariabilityModel.NumericOptions)
            {
                numericOptions.Add(numOpt);
            }

            List<Configuration> configurations =
                ConfigurationBuilder.vg.GenerateAllVariants(numericVariabilityModel, numericOptions);

            // Return all configurations and an error if we can't sample enough configurations
            if (sampleSize >= configurations.Count)
            {
                if (sampleSize > configurations.Count)
                    GlobalState.logError.logLine(
                        "Numeric Random Sampling: numConfigs too large for variability model. num set to " +
                        configurations.Count);
                selectedConfigurations.AddRange(ExtractNumericConfiguration(configurations));
                return true;
            }

            // select random configurations
            Random r = new Random(seed);
            List<Configuration> selectedConfigs = new List<Configuration>();
            for (int i = 0; i < sampleSize; i++)
            {
                int position = r.Next(configurations.Count);
                Configuration selectedConfig = configurations[position];
                configurations.RemoveAt(position);
                selectedConfigs.Add(selectedConfig);
            }
            
            selectedConfigurations.AddRange(ExtractNumericConfiguration(selectedConfigs));

            return true;
        }

        private List<Dictionary<NumericOption, double>> ExtractNumericConfiguration(List<Configuration> configurations)
        {
            List<Dictionary<NumericOption, double>> result = new List<Dictionary<NumericOption, double>>();
            Dictionary<NumericOption, NumericOption> mapToOriginalVM = new Dictionary<NumericOption, NumericOption>();
            foreach (Configuration config in configurations)
            {
                Dictionary<NumericOption, double> newMap = new Dictionary<NumericOption, double>();
                foreach (NumericOption numOpt in config.NumericOptions.Keys)
                {
                    if (!mapToOriginalVM.ContainsKey(numOpt))
                    {
                        mapToOriginalVM[numOpt] = GlobalState.varModel.getNumericOption(numOpt.Name);
                    }

                    newMap[mapToOriginalVM[numOpt]] = config.NumericOptions[numOpt];

                }
                result.Add(newMap);
            }

            return result;
        }
        
        public override string parameterIdentifier()
        {
            return "seed-" + seed + "_" + "sampleSize-" + sampleSize + "_";
        }
    }
}
