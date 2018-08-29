using SPLConqueror_Core;
using System.Collections.Generic;

namespace MachineLearning.Sampling.Hybrid.Distributive.SelectionHeuristic
{
    /// <summary>
    /// Interface for heuristics that select configurations of distribution.
    /// </summary>
    public interface ISelectionHeuristic
    {
        /// <summary>
        /// Selects configurations of the given distribution by using the specified distribution (e.g., uniform) and a certain selection
        /// mechanism.
        /// </summary>
        /// <param name="wholeDistribution">the distribution of all configurations</param>
        /// <param name="allBuckets">all buckets of the distribution</param>
        /// <param name="wantedDistribution">The distribution used for selection.</param>
        /// <param name="count">the number of configurations to sample</param>
        /// <param name="optimization">The optimization to use</param>
        /// <returns>The configurations that were selected.</returns>
        List<Configuration> SampleFromDistribution(Dictionary<double, double> wantedDistribution, List<double> allBuckets, int count, Optimization optimization);
    }
}
