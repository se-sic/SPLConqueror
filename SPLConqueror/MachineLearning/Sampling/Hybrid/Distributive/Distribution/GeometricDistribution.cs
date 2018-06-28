using System;
using System.Collections.Generic;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class represents the geometric distribution.
    /// </summary>
    public class GeometricDistribution : IDistribution
    {
        const double PROBABILITY = 0.50;

        /// <summary>
        /// See <see cref="Distribution.CreateDistribution(List{double})"/>.
        /// </summary>
        public Dictionary<double, double> CreateDistribution(List<double> allBuckets)
        {
            Dictionary<double, double> result = new Dictionary<double, double>();

            double previousResultCache = 0;
            for (int i = 0; i < allBuckets.Count; i++) {
                double currentResult = 1 - Math.Pow(1 - PROBABILITY, i + 1);
                result[allBuckets[i]] = currentResult - previousResultCache;
                previousResultCache = currentResult;
            }

            return DistributionUtils.AdjustToOne(result);
        }

        /// <summary>
        /// See <see cref="Distribution.GetName"/>.
        /// </summary>
        public string GetName()
        {
            return "GEOMETRIC";
        }
    }
}
