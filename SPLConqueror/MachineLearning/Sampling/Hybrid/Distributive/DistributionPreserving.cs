using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class represents the distribution-preserving sampling strategy, where two distributions are multiplied.
    /// The first distribution is the one selected by the user (e.g., uniform), whereas the second distribution is the original distribution of the subject system.
    /// To obtain a distribution with a total probability of 1, a normalization constant is used.
    /// </summary>
    class DistributionPreserving : DistributionSensitive
    {
        /// <summary>
        /// This method multiplies two distributions, namely the distribution provided by the user and the 
        /// original distribution of the subject system.
        /// </summary>
        /// <param name="wholeDistribution">the distribution of all configurations</param>
        /// <param name="allBuckets">all buckets of the distribution</param>
        /// <returns>a distribution made of the distribution provided by the user and the original distribution of the subject system</returns>
        public override Dictionary<double, double> CreateDistribution(Dictionary<double, List<Configuration>> wholeDistribution, List<double> allBuckets)
        {
            // Create the distributions that will be multiplied to generate another distribution for the samples
            Dictionary<double, double> wantedDistribution = this.distribution.CreateDistribution(allBuckets);
            Dictionary<double, double> originalDistribution = CreateOriginalDistribution(wholeDistribution);

            Dictionary<double, double> combinedDistribution = CombineDistributions(wantedDistribution, originalDistribution);

            return combinedDistribution;
        }

        /// <summary>
        /// This method returns the distribution of the whole population as <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="wholeDistribution">the distribution containing all configurations</param>
        /// <returns>a <see cref="Dictionary{TKey, TValue}"/> where the key is the distance as <see cref="double"/> and the value is the probability in <see cref="double"/></returns>
        private Dictionary<double, double> CreateOriginalDistribution(Dictionary<double, List<Configuration>> wholeDistribution)
        {
            // Build the sum of all found configurations
            int sum = 0;
            foreach (double d in wholeDistribution.Keys)
            {
                sum += wholeDistribution[d].Count;
            }

            Dictionary<double, double> result = new Dictionary<double, double>();

            // Now, compute the probabilities
            foreach (double d in wholeDistribution.Keys)
            {
                result[d] = ((double) wholeDistribution[d].Count) / sum;
            }

            return result;
        }

        /// <summary>
        /// This method combines two distributions by multiplying them.
        /// Afterwards, a normalizing constant is used to transfer the resulting probability function into a probability density function with total probability of 1 (or 100%).
        /// </summary>
        /// <param name="firstDistribution">the first distribution</param>
        /// <param name="secondDistribution">the second distribution to multiply with</param>
        /// <returns>a distribution with total probability 1</returns>
        private Dictionary<double, double> CombineDistributions(Dictionary<double, double> firstDistribution, Dictionary<double, double> secondDistribution)
        {
            // The distributions must have the same keys
            if (firstDistribution.Keys.Count != secondDistribution.Keys.Count)
            {
                throw new ArgumentException("The distributions have not the same sizes!");
            }

            foreach (double d in firstDistribution.Keys)
            {
                if (!secondDistribution.Keys.Contains(d))
                {
                    throw new ArgumentException("The distributions have not the same keys!");
                }
            }

            foreach (double d in secondDistribution.Keys)
            {
                if (!firstDistribution.Keys.Contains(d))
                {
                    throw new ArgumentException("The distributions have not the same keys!");
                }
            }

            // Now that the sanity checks are done, compute the new distribution
            Dictionary<double, double> result = new Dictionary<double, double>();

            double sum = 0;
            foreach (double d in firstDistribution.Keys)
            {
                result[d] = firstDistribution[d] * secondDistribution[d];
                sum += result[d];
            }

            // Afterwards, divide it by the sum to obtain a total probability of 1
            foreach (double d in result.Keys)
            {
                result[d] = result[d] / sum;
            }

            return result;
        }

        /// <summary>
        /// See <see cref="HybridStrategy.GetName"/>.
        /// </summary>
        public override string GetName()
        {
            return "DISTRIBUTION-PRESERVING";
        }
    }
}
