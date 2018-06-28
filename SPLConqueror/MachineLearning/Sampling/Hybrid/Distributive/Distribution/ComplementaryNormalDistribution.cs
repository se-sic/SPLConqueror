using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class represents the complementary normal distribution.
    /// </summary>
    public class ComplementaryNormalDistribution : NormalDistribution
    {

        /// <summary>
        /// Creates the distribution by using the given mean and deviation value.
        /// </summary>
        /// <param name="allBuckets">All buckets.</param>
        /// <param name="mean">The mean for the normal distribution.</param>
        /// <param name="deviation">The standard deviation for the normal distribution.</param>
		public override Dictionary<double, double> CreateDistribution(List<double> allBuckets, double mean, double deviation)
        {
            Dictionary<double, double> result = base.CreateDistribution(allBuckets, mean, deviation);

			// Now, use the complement
			List<double> keyList = new List<double> (result.Keys);
			double maxValue = result.Values.Max ();
			double minValue = result.Values.Min ();
            foreach (double key in keyList) {
				result[key] = maxValue - (result[key] - minValue);
            }

            result = DistributionUtils.AdjustToOne(result);

            return result;
        }

        /// <summary>
        /// See <see cref="Distribution.GetName"/>.
        /// </summary>
        public override string GetName()
        {
            return "COMPLEMENTARY-NORMAL";
        }
        
    }
}
