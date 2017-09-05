using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Sampling.Hybrid.Distribution_Aware.Distributive.Distribution
{
    /// <summary>
    /// This interface provides methods to calculate samples from a given distribution.
    /// </summary>
    public interface Distribution
    {
        /// <summary>
        /// This method returns a distribution according to the given buckets.
        /// </summary>
        /// <param name="allBuckets">the buckets from the distribution</param>
        /// <returns>a <see cref="Dictionary{TKey, TValue}"/> containing the likeliness of the buckets to be chosen</returns>
        Dictionary<double, double> CreateDistribution(List<double> allBuckets);

        /// <summary>
        /// Returns the name of the distribution.
        /// </summary>
        /// <returns>the name of the distribution as <see cref="string"/>.</returns>
        string GetName();
    }
}
