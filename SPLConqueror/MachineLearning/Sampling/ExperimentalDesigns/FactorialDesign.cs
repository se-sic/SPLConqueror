using System;
using System.Collections.Generic;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    /// <summary>
    /// This class adds an experimental design called factorial design with the difference 
    /// that both n is variable.
    /// </summary>
    public class FactorialDesign : ExperimentalDesign
    {
        /// <summary>
        /// The number of values per option that should be included.
        /// </summary>
        private int n;


        /// <summary>
        /// Initializes a new instance of the <see cref="MachineLearning.Sampling.ExperimentalDesigns.FactorialDesign"/> class
        /// by setting the n parameter.
        /// </summary>
        /// <param name="n">The number of values per option that should be included.</param>
        public FactorialDesign (int n = 2)
        {
            if (n < 2) {
                throw new ArgumentException ("n has to be greater or equal to 2.");
            }

            this.n = n;
        }

        public override string getName ()
        {
            return "FACTORIAL";
        }

        public override string getTag()
        {
            return "FA";
        }

        public override string parameterIdentifier ()
        {
            return "n-" + this.n + "_";
        }

        public override void setSamplingParameters (Dictionary<string, string> parameterNameToValue)
        {
            if (parameterNameToValue.ContainsKey ("n")) {
                this.n = parseFromParameters (parameterNameToValue, "n");
                if (this.n < 2) {
                    throw new ArgumentException ("n has to be greater or equal to 2.");
                }
            }
        }

        public override bool computeDesign ()
        {
            return compute (this.n);
        }

        /// <summary>
        /// This method creates a list of configurations according to the n^k design.
        /// </summary>
        /// <param name="_n">The number of values per option that should be included.</param>
        /// <returns><code>true</code> iff the computation was successful;<code>false</code> otherwise.
        public bool compute (int _n)
        {
            // Firstly, check if the options are provided at all 
            if (options.Count == 0)
                return false;

            // Check if all options have at least n different values
            foreach (NumericOption opt in options) {
                if (opt.getAllValues ().Count < _n) {
                    throw new InvalidOperationException ("Can not perform factorial sampling. A numeric option has less than " + _n + " values.");
                }
            }

            // Now, start with creating the factorial samples
            Dictionary<NumericOption, List<double>> valuesOfOptions = new Dictionary<NumericOption, List<double>> ();

            // Iterate over all options and find the right values
            foreach (NumericOption opt in options) {
                List<double> values = new List<double> ();
                List<double> allValues = opt.getAllValues ();

                double min = opt.Min_value;
                double max = opt.Max_value;
                double step = (max - min) / (_n - 1);

                for (int i = 0; i < _n; i++) {
                    values.Add (opt.nearestValidValue (i * step + min));
                }

                valuesOfOptions.Add (opt, values);
            }

            //List<Dictionary<NumericOption, double>>
            this.selectedConfigurations = createConfigurationList(valuesOfOptions, options);

            return true;
        }

        /// <summary>
        /// This method creates recursively a list of all configurations.
        /// </summary>
        /// <returns>The configuration list.</returns>
        /// <param name="valuesOfOptions">The values of all numeric options.</param>
        /// <param name="remainingOptions">The remaining options to add.</param>
        private List<Dictionary<NumericOption, double>> createConfigurationList (Dictionary<NumericOption, List<double>> valuesOfOptions, 
            List<NumericOption> remainingOptions)
        {
            List<Dictionary<NumericOption, double>> result;

            NumericOption currentOption = remainingOptions [0];
            remainingOptions.Remove (currentOption);
            List<double> valuesOfCurrentOption = valuesOfOptions [currentOption];

            result = new List<Dictionary<NumericOption, double>> ();

            if (remainingOptions.Count == 0) {
                foreach (double value in valuesOfCurrentOption) {
                    Dictionary<NumericOption, double> newDict = new Dictionary<NumericOption, double> ();
                    newDict.Add (currentOption, value);
                    result.Add (newDict);
                }

            } else {
                List<Dictionary<NumericOption, double>> previousConfigurations = createConfigurationList (valuesOfOptions, remainingOptions);

                foreach (Dictionary<NumericOption, double> config in previousConfigurations) {
                    foreach (double value in valuesOfCurrentOption) {
                        Dictionary<NumericOption, double> newDict = new Dictionary<NumericOption, double> (config);
                        newDict.Add (currentOption, value);
                        result.Add (newDict);
                    }
                }
            }

            return result;
        }
    }
}

