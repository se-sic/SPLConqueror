using System.Collections.Generic;
using MathNet.Numerics.Distributions;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class represents the normal distribution. 
    /// </summary>
    public class NormalDistribution : IDistribution
    {

        /// <summary>
        /// See <see cref="Distribution.CreateDistribution(List{double})"/>.
        /// </summary>
        public Dictionary<double, double> CreateDistribution(List<double> allBuckets)
        {
            double mean = allBuckets[allBuckets.Count - 1] - (allBuckets[allBuckets.Count - 1] - allBuckets[0] / 2);
            double deviation = (mean - allBuckets[0]) / 2;


            return CreateDistribution(allBuckets, mean, deviation);
        }

        /// <summary>
        /// Creates the distribution by using the given mean and deviation value.
        /// </summary>
        /// <param name="allBuckets">All buckets.</param>
        /// <param name="mean">The mean for the normal distribution.</param>
        /// <param name="deviation">The standard deviation for the normal distribution.</param>
        public Dictionary<double, double> CreateDistribution(List<double> allBuckets, double mean, double deviation) {
            Dictionary<double, double> result = new Dictionary<double, double>();

            double previousResultCache = 0;
            for (int i = 0; i < allBuckets.Count; i++) {
                double currentResult = Normal.CDF(mean, deviation, allBuckets[i]);
                result[allBuckets[i]] = currentResult - previousResultCache;
                previousResultCache = currentResult;
            }


            return result;
        }

        /// <summary>
        /// See <see cref="Distribution.GetName"/>.
        /// </summary>
        public string GetName()
        {
            return "NORMAL";
        }
    }
}
