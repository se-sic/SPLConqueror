using System.Collections.Generic;
using System;

namespace MachineLearning.Sampling.Hybrid.Distributive
{
    public class BinomialDistribution : IDistribution
    {
        // This could be another parameter for specifying the distribution
        const double PROBABILITY = 0.50;

        static Func<int, double> Factorial = x => x < 0 ? -1 : x == 1 || x == 0 ? 1.0 : x * Factorial(x - 1);

        /// <summary>
        /// See <see cref="Distribution.CreateDistribution(List{double})"/>.
        /// </summary>
        public Dictionary<double, double> CreateDistribution(List<double> allBuckets)
        {
            Dictionary<double, double> result = new Dictionary<double, double>();

            int numberOfBuckets = allBuckets.Count - 1;


            for (int k = 0; k <= numberOfBuckets; k++)
            {
                double firstPart = (Factorial(numberOfBuckets) / (Factorial(k) * Factorial(numberOfBuckets - k)));
                double secondPart = Math.Pow(PROBABILITY, k) * Math.Pow((1 - PROBABILITY), (numberOfBuckets - k));
                double probability = firstPart * secondPart;

                result[allBuckets[k]] = probability;
            }

            // Adjust it to be 1.0 in the sum
            result = DistributionUtils.AdjustToOne(result);

            return result;
        }

        /// <summary>
        /// See <see cref="Distribution.GetName"/>.
        /// </summary>
        public string GetName()
        {
            return "BINOMIAL";
        }
    }
}