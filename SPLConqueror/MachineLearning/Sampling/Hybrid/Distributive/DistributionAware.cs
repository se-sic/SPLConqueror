using SPLConqueror_Core;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    /// <summary>
    /// This enum indicates the optimization that should be executed.
    /// Both optimizations try to cover all features uniformly.
    /// Without the optimization, the possibility to miss certain features is higher.
    /// </summary>
    public enum Optimization
    {
        NONE,
        GLOBAL,
        LOCAL
    }

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
            // If a normal distribution is wanted, the parameters can be computed
            // from the whole population if available
            if (this.distribution is NormalDistribution && !this.strategyParameter[USE_WHOLE_POPULATION].Equals("false"))
            {
                Dictionary<double, int> distr = DistributionUtils.CountConfigurations(wholeDistribution);

                double sum = 0;
                int count = 0;
                foreach (double key in distr.Keys)
                {
                    sum += key * distr[key];
                    count += distr[key];
                }

                // Compute the mean value of the whole distribution
                double mean = sum / count;

                // Compute the deviation
                double deviation = Math.Sqrt(distr.Sum(x => x.Value * Math.Pow(x.Key - mean, 2)) / count);

                return ((NormalDistribution)this.distribution).CreateDistribution(allBuckets, mean, deviation);
            }
            else
            {
                return this.distribution.CreateDistribution(allBuckets);
            }
        }

        /// <summary>
        /// See <see cref="HybridStrategy.GetName"/>.
        /// </summary>
        public override string GetName()
        {
            return "DISTRIBUTION-AWARE";
        }

        public override string getTag()
        {
            return "DISTAW";
        }
    }
}
