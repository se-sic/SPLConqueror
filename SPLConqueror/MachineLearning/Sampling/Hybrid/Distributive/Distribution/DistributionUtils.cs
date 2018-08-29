using System.Collections.Generic;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class includes some utilities that are suitable for distributions.
    /// </summary>
    public class DistributionUtils
    {

        /// <summary>
        /// This method adjusts the given distribution so that the sum is 1.0.
        /// </summary>
        /// <returns>The adjusted distribution.</returns>
        /// <param name="distribution">The distribution to adjust.</param>
        public static Dictionary<double, double> AdjustToOne(Dictionary<double, double> distribution)
        {
            Dictionary<double, double> newDistribution = new Dictionary<double, double>();

            double sum = 0;
            foreach (double key in distribution.Keys)
            {
                sum += distribution[key];
            }

            foreach (double key in distribution.Keys)
            {
                newDistribution[key] = distribution[key] / sum;
            }

            return newDistribution;
        }

        /// <summary>
        /// Returns the distribution of the given list of configurations.
        /// </summary>
        /// <returns>The distribution of the given list.</returns>
        /// <param name="configurations">List of all configurations.</param>
        /// <param name="distanceMetric">The distance metric to use.</param>
        public static Dictionary<double, int> CountConfigurations(Dictionary<double, List<Configuration>> configurations)
        {
            Dictionary<double, int> result = new Dictionary<double, int>();

            foreach (double key in configurations.Keys)
            {
                result[key] = configurations[key].Count;
            }

            return result;
        }
    }
}
