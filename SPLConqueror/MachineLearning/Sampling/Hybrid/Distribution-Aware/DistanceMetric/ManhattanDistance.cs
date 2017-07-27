using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Hybrid.Distribution_Aware.DistanceMetric
{
    /// <summary>
    /// This class represents the manhattan distance of features.
    /// For binary features either 1 (selected) or 0 (unselected) is returned.
    /// For numeric features, the minimum value is mapped on 0 and the maximum value on 1.
    /// The other values are computed relatively to the minimum and maximum value.
    /// </summary>
    public class ManhattanDistance : DistanceMetric
    {
        /// <summary>
        /// Computes the distance of the given configuration.
        /// </summary>
        /// <param name="configuration">a <see cref="Configuration"/> consisting of all features and their respective values</param>
        /// <returns>the manhattan distance as <see cref="double"/></returns>
        public double ComputeDistance(Configuration configuration)
        {
            double distance = 0;

            // Traverse the binary options
            foreach (BinaryOption binOpt in configuration.BinaryOptions.Keys)
            {
                double val = configuration.BinaryOptions[binOpt] == BinaryOption.BinaryValue.Selected ? 1 : 0;
                distance += ComputeDistanceOfBinaryFeature(val);
            }

            foreach (NumericOption numOpt in configuration.NumericOptions.Keys)
            {
                distance += ComputeDistanceOfNumericFeature(configuration.NumericOptions[numOpt], numOpt.Min_value, numOpt.Max_value);
            }

            return distance;
        }

        /// <summary>
        /// Computes the distance of binary features.
        /// </summary>
        /// <param name="selected"><code>1</code> iff the feature is selected;<code>0</code> otherwise</param>
        /// <returns><code>1</code> iff the feature is selected;<code>0</code> otherwise</returns>
        public double ComputeDistanceOfBinaryFeature(double selected)
        {
            return selected;
        }

        /// <summary>
        /// Computes the distance of numeric features by using the minimum value, maximum value and the current value of the feature.
        /// </summary>
        /// <param name="value">the current value of the numeric feature</param>
        /// <param name="minValue">the minimum value of the numeric feature</param>
        /// <param name="maxValue">the maximum value of the numeric feature</param>
        /// <returns>the distance of the numeric feature with the given current value</returns>
        public double ComputeDistanceOfNumericFeature(double value, double minValue, double maxValue)
        {
            return (value - minValue) / (maxValue - minValue);
        }

        /// <summary>
        /// See <see cref="DistanceMetric.GetName"/>.
        /// </summary>
        public string GetName()
        {
            return "MANHATTAN";
        }
    }
}
