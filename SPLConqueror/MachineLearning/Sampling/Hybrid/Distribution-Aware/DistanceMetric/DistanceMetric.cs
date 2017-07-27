using SPLConqueror_Core;
using System.Collections.Generic;

namespace MachineLearning.Sampling.Hybrid.Distribution_Aware.DistanceMetric
{
    /// <summary>
    /// This class is an interface for all distance metrics that can be used for computing the distribution.
    /// </summary>
    public interface DistanceMetric
    {
        /// <summary>
        /// This method computes the distance of the whole configuration.
        /// </summary>
        /// <param name="configuration">the configuration including all relevant features</param>
        /// <returns>the distance of the configuration</returns>
        double ComputeDistance(Configuration configuration);

        /// <summary>
        /// This method computes the distance of a numeric option with a given value.
        /// Therefore, the minimum value, maximum value and current value is needed.
        /// </summary>
        /// <param name="value">the current value of the feature</param>
        /// <param name="minValue">the minimum value of the numeric feature</param>
        /// <param name="maxValue">the maximum value of the numeric feature</param>
        /// <returns>the distance of the given feature</returns>
        double ComputeDistanceOfNumericFeature(double value, double minValue, double maxValue);

        /// <summary>
        /// Computes the distance of a binary feature.
        /// </summary>
        /// <param name="selected">whether the binary feature is selected (1.0) or not (0.0)</param>
        /// <returns>the distance value according to the given value of the feature</returns>
        double ComputeDistanceOfBinaryFeature(double selected);

        /// <summary>
        /// Returns the name of the distance metric.
        /// </summary>
        /// <returns>the name of the distance metric as <see cref="string"/>.</returns>
        string GetName();

    }
}
