using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Sampling.Hybrid.Distributive.SelectionHeuristic
{
    public class RandomSelection : ISelectionHeuristic
    {
        private int seed = 0;

        /// <summary>
        /// Create RandomSelection object.
        /// </summary>
        public RandomSelection() {
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
        /// Selects configurations of the given distribution by using the specified distribution (e.g., uniform) using a random selection mechanism.
        /// </summary>
        /// <param name="wholeDistribution">the distribution of all configurations</param>
        /// <param name="allBuckets">all buckets of the distribution</param>
        /// <param name="count">the number of configurations to sample</param>
        /// <returns>The configurations that were selected.</returns>
        public List<Configuration> SampleFromDistribution(Dictionary<double, List<Configuration>> wholeDistribution, 
            Dictionary<double, double> wantedDistribution, List<double> allBuckets, int count)
        {
            Random rand = new Random(seed);
            List<Configuration> selectedConfigurations = new List<Configuration>();

            while (selectedConfigurations.Count < count && HasSamples(wholeDistribution))
            {
                double randomDouble = rand.NextDouble();
                double currentProbability = 0;
                int currentBucket = 0;

                while (randomDouble > currentProbability + wantedDistribution.ElementAt(currentBucket).Value)
                {
                    currentBucket++;
                    currentProbability += wantedDistribution.ElementAt(currentBucket).Value;
                }

                double distanceOfBucket = wantedDistribution.ElementAt(currentBucket).Key;

                // If a bucket was selected that contains no more configurations, repeat the procedure
                if (wholeDistribution[distanceOfBucket].Count == 0)
                {
                    continue;
                }

                int numberConfiguration = rand.Next(0, wholeDistribution[distanceOfBucket].Count);

                selectedConfigurations.Add(wholeDistribution[distanceOfBucket][numberConfiguration]);
                wholeDistribution[distanceOfBucket].RemoveAt(numberConfiguration);

            }

            if (selectedConfigurations.Count < count)
            {
                GlobalState.logError.logLine("Sampled only " + selectedConfigurations.Count + " configurations as there are no more configurations.");
            }
            return selectedConfigurations;
        }
        
        private bool HasSamples(Dictionary<double, List<Configuration>> wholeDistribution)
        {
            foreach (double d in wholeDistribution.Keys)
            {
                if (wholeDistribution[d].Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
