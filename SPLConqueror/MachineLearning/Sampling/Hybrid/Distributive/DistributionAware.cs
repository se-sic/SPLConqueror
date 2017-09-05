using SPLConqueror_Core;
using System.Collections.Generic;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This class represents the distribution-aware sampling strategy.
    /// In this sampling strategy, the configurations are divided in buckets and the sampled configurations are selected 
    /// from this buckets according to a given distribution (e.g., uniform, normal distribution).
    /// </summary>
    public class DistributionAware : DistributionSensitive
    {

        /// <summary>
        /// Returns the corresponding distribution. 
        /// </summary>
        /// <param name="wholeDistribution">the distribution of all configurations</param>
        /// <param name="allBuckets">all buckets of the distribution</param>
        /// <returns>the user-specified distribution (e.g., uniform)</returns>
        public override Dictionary<double, double> CreateDistribution(Dictionary<double, List<Configuration>> wholeDistribution, List<double> allBuckets)
        {
            return this.distribution.CreateDistribution(allBuckets);
        }

        /// <summary>
        /// See <see cref="HybridStrategy.GetName"/>.
        /// </summary>
        public override string GetName()
        {
            return "DISTRIBUTION-AWARE";
        }
    }
}
