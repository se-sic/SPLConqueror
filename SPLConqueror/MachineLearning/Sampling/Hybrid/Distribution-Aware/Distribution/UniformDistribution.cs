using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Hybrid.Distribution_Aware.Distribution
{
    /// <summary>
    /// This class represents the uniform distribution, where it is equally likely to pick one of all buckets.
    /// </summary>
    public class UniformDistribution : Distribution
    {
        /// <summary>
        /// See <see cref="Distribution.CreateDistribution(List{double})"/>.
        /// </summary>
        public Dictionary<double, double> CreateDistribution(List<double> allBuckets)
        {
            Dictionary<double, double> result = new Dictionary<double, double>();

            double probabilityPerBucket = 1.0 / allBuckets.Count;

            foreach (double d in allBuckets)
            {
                result[d] = probabilityPerBucket;
            }

            return result;
        }

        /// <summary>
        /// See <see cref="Distribution.GetName"/>.
        /// </summary>
        public string GetName()
        {
            return "UNIFORM";
        }

    }
}
