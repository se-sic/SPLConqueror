﻿using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MachineLearning.Sampling.Hybrid.Distributive.SelectionHeuristic
{
    /// <summary>
    /// This class is another selection mechanism by using the Microsoft Solver Foundation solver.
    /// The advantage of this class is, that the whole population is not needed, since it relies
    /// only on the solver.
    /// Be aware that this selection method currently works only for binary features.
    /// </summary>
    public class SolverSelection : ISelectionHeuristic
    {
        // The seed for the random-class
        private int seed = 0;

        // number of features considered for weight optimization
        private Tuple<int, int> featureRange = new Tuple<int, int>(1, 1);

        private VariabilityModel vm = GlobalState.varModel;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MachineLearning.Sampling.Hybrid.Distributive.SelectionHeuristic.SolverSelection"/> class.
        /// </summary>
        public SolverSelection()
        {
        }

        /// <summary>
        /// Sets the seed of the random number generator.
        /// </summary>
        /// <param name="seed">Value of the seed.</param>
        public void setSeed(int seed)
        {
            this.seed = seed;
        }

        /// <summary>
        /// Sets the number of features, that will be considered for weight optimization.
        /// </summary>
        /// <param name="features">The number of features.</param>
        public void setNumberFeatures(Tuple<int, int> featureRange)
        {
            this.featureRange = featureRange;
        }

        /// <summary>
        /// Set the variability model.
        /// </summary>
        /// <param name="vm">the variability model</param>
        public void setVariabilityModel(VariabilityModel vm)
        {
            this.vm = vm;
        }

        /// <summary>
        /// This method selects a given number of configurations by using the sample mechanism of the CSP solver.
        /// </summary>
        /// <returns>A set of configurations that have been picked to fit the given distribution.</returns>
        /// <param name="wantedDistribution">The wanted distribution of the samples.</param>
        /// <param name="allBuckets">The buckets containing at least one configuration.</param>
        /// <param name="count">The number of configurations to select.</param>
        /// <param name="optimization">The optimization to use</param>
        public List<Configuration> SampleFromDistribution(Dictionary<double, double> wantedDistribution, List<double> allBuckets, int count, Optimization optimization = Optimization.NONE)
        {
            Random rand = new Random(seed);
            List<Configuration> selectedConfigurations = new List<Configuration>();
            Dictionary<int, Configuration> selectedConfigurationsFromBucket = new Dictionary<int, Configuration>();
            for (int i = 0; i < allBuckets.Count; i++)
            {
                selectedConfigurationsFromBucket[i] = null;
            }

            // Create and initialize the weight function
            Dictionary<int, Dictionary<List<BinaryOption>, int>> featureWeight = new Dictionary<int, Dictionary<List<BinaryOption>, int>>();

            if (optimization == Optimization.GLOBAL)
            {
                if (this.featureRange.Item1 != 0 && this.featureRange.Item2 != 0)
                {
                    featureWeight.Add(0, InitializeWeightDict(vm));
                }
                else
                {
                    featureWeight.Add(0, new Dictionary<List<BinaryOption>, int>());
                }
            }
            else if (optimization == Optimization.LOCAL)
            {
                for (int i = 0; i < allBuckets.Count; i++)
                {
                    if (this.featureRange.Item1 != 0 && this.featureRange.Item2 != 0)
                    {
                        featureWeight.Add(i, InitializeWeightDict(vm));
                    }
                    else
                    {
                        featureWeight.Add(i, new Dictionary<List<BinaryOption>, int>());
                    }
                }
            }

            bool[] noSamples = new bool[allBuckets.Count];


            while (selectedConfigurations.Count < count && HasSamples(noSamples))
            {
                double randomDouble = rand.NextDouble();
                double currentProbability = 0;
                int currentBucket = 0;

                while (randomDouble > currentProbability + wantedDistribution.ElementAt(currentBucket).Value)
                {
                    currentProbability += wantedDistribution.ElementAt(currentBucket).Value;
                    currentBucket++;
                }

                // Note: This method works only for binary features and therefore, only integer buckets
                int distanceOfBucket = Convert.ToInt32(wantedDistribution.ElementAt(currentBucket).Key);

                // Repeat if there are currently no solutions in the bucket.
                // This is intended to reduce the work of the solver.
                // Should not happen anymore!
                if (noSamples[currentBucket] || !allBuckets.Contains(distanceOfBucket))
                {
                    throw new InvalidProgramException("A bucket was selected that already contains no more samples! This shouldn't happen.");
                }

                if (ConfigurationBuilder.vg is Solver.Z3VariantGenerator)
                {
                    ((Solver.Z3VariantGenerator)ConfigurationBuilder.vg).setSeed(Convert.ToUInt32(this.seed));
                }

                List<BinaryOption> solution = null;
                // Now select the configuration by using the solver
                if (optimization == Optimization.NONE)
                {
                    solution = ConfigurationBuilder.vg.GenerateConfigurationFromBucket(vm,
                        distanceOfBucket, null, selectedConfigurationsFromBucket[currentBucket]);
                }
                else if (optimization == Optimization.GLOBAL)
                {
                    solution = ConfigurationBuilder.vg.GenerateConfigurationFromBucket(vm,
                        distanceOfBucket, featureWeight[0], selectedConfigurationsFromBucket[currentBucket]);

                }
                else if (optimization == Optimization.LOCAL)
                {
                    solution = ConfigurationBuilder.vg.GenerateConfigurationFromBucket(vm,
                        distanceOfBucket, featureWeight[currentBucket], selectedConfigurationsFromBucket[currentBucket]);
                }

                // If a bucket was selected that now contains no more configurations, repeat the procedure
                if (solution == null)
                {
                    noSamples[currentBucket] = true;

                    // As a consequence, the probability to pick this bucket is set to 0 and the whole
                    // distribution is readjusted so that the sum of all probabilities is equal to 1 (i.e., 100%).
                    wantedDistribution[wantedDistribution.ElementAt(currentBucket).Key] = 0d;
                    wantedDistribution = DistributionUtils.AdjustToOne(wantedDistribution);
                    continue;
                }

                Configuration currentSelectedConfiguration = new Configuration(solution);

                selectedConfigurations.Add(currentSelectedConfiguration);
                selectedConfigurationsFromBucket[currentBucket] = currentSelectedConfiguration;
                if (optimization == Optimization.GLOBAL)
                {
                    UpdateWeights(vm, featureWeight[0], currentSelectedConfiguration);
                }
                else if (optimization == Optimization.LOCAL)
                {
                    UpdateWeights(vm, featureWeight[currentBucket], currentSelectedConfiguration);
                }
            }

            if (selectedConfigurations.Count < count)
            {
                GlobalState.logError.logLine("Sampled only " + selectedConfigurations.Count + " configurations as there are no more configurations.");
            }

            ConfigurationBuilder.vg.ClearCache();

            return selectedConfigurations;
        }

        /// <summary>
        /// This method initializes the weight function. 
        /// </summary>
        /// <returns>A new dictionary with initialized values.</returns>
        /// <param name="vm">The variability model.</param>
        private Dictionary<List<BinaryOption>, int> InitializeWeightDict(VariabilityModel vm)
        {
            List<BinaryOption> features = vm.BinaryOptions;
            Dictionary<List<BinaryOption>, int> weights = new Dictionary<List<BinaryOption>, int>();

            List<List<BinaryOption>> allCombinations = BuildAllCombinations(vm, new List<BinaryOption>());

            foreach (List<BinaryOption> combination in allCombinations)
            {
                weights.Add(combination, 0);
            }

            return weights;
        }

        /// <summary>
        /// Builds all combinations of features according to feature range.
        /// Thereby, mandatory features are excluded.
        /// </summary>
        /// <returns>All combinations of features according to the feature range.</returns>
        /// <param name="vm">The variability model.</param>
        /// <param name="listToExpand">The current list of binary options.</param>
        private List<List<BinaryOption>> BuildAllCombinations(VariabilityModel vm, List<BinaryOption> listToExpand)
        {
            List<List<BinaryOption>> result = new List<List<BinaryOption>>();
            if (listToExpand.Count == featureRange.Item2)
            {
                return result;
            }

            foreach (BinaryOption binOpt in vm.BinaryOptions)
            {
                List<BinaryOption> localCopy = new List<BinaryOption>(listToExpand);

                // Skip mandatory features since they are included in every configuration
                if (!binOpt.Optional && binOpt.Excluded_Options.Count == 0)
                {
                    continue;
                }

                if (!listToExpand.Contains(binOpt))
                {
                    localCopy.Add(binOpt);
                    if (TestExcludedOptions(localCopy))
                    {
                        if (localCopy.Count >= featureRange.Item1)
                        {
                            result.Add(localCopy);
                        }

                        // Add the elements if they are not already included in another order
                        List<List<BinaryOption>> allCombinations = BuildAllCombinations(vm, localCopy);
                        foreach (List<BinaryOption> combination in allCombinations)
                        {
                            if (!IsIncluded(result, combination))
                            {
                                result.Add(combination);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private static bool IsIncluded(List<List<BinaryOption>> allCombinations, List<BinaryOption> combinationToFind)
        {
            foreach (List<BinaryOption> combination in allCombinations)
            {
                List<BinaryOption> allRelevantOptions = new List<BinaryOption>();
                allRelevantOptions.AddRange(combination);
                allRelevantOptions.AddRange(combinationToFind);

                if (combination.Count == combinationToFind.Count)
                {
                    bool found = true;
                    foreach (BinaryOption binOpt in allRelevantOptions)
                    {
                        if (!combination.Contains(binOpt) || !combinationToFind.Contains(binOpt))
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool TestExcludedOptions(List<BinaryOption> binOpts)
        {
            if (binOpts.Count == 1)
            {
                return true;
            }
            else
            {
                List<BinaryOption> excluded = new List<BinaryOption>();
                binOpts.ForEach(opt => opt.Excluded_Options.
                    ForEach(nonValid => excluded.AddRange(nonValid.Select(confOpt => (BinaryOption)confOpt))));

                foreach (BinaryOption binOpt in binOpts)
                {
                    if (excluded.Contains(binOpt))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// This method updates the weight-dictionary by incrementing the counts.
        /// </summary>
        /// <param name="vm">The variability model.</param>
        /// <param name="weights">The dictionary containing the number of features in all configurations.</param>
        /// <param name="addedConfiguration">The newly added configuration.</param>
        private void UpdateWeights(VariabilityModel vm, Dictionary<List<BinaryOption>, int> weights, Configuration addedConfiguration)
        {
            List<BinaryOption> features = vm.BinaryOptions;

            for (int i = 0; i < weights.Keys.Count; i++)
            {
                List<BinaryOption> combination = weights.Keys.ElementAt(i);
                bool isIncluded = true;
                foreach (BinaryOption binOpt in combination)
                {
                    if (!addedConfiguration.BinaryOptions.ContainsKey(binOpt) || addedConfiguration.BinaryOptions[binOpt] == BinaryOption.BinaryValue.Deselected)
                    {
                        isIncluded = false;
                    }
                }
                if (isIncluded)
                {
                    weights[combination]++;
                }
            }
        }

        /// <summary>
        /// Returns whether there are more samples or not.
        /// </summary>
        /// <returns><c>true</c> if there is at least one sample; otherwise, <c>false</c>.</returns>
        /// <param name="noSamples">The array containing the information if a bucket has no more samples.</param>
        private bool HasSamples(bool[] noSamples)
        {
            foreach (bool b in noSamples)
            {
                if (!b)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

