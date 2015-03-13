using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    /// <summary>
    /// The PlackettBurmanDesign as presented in http://www.jstor.org/discover/10.2307/2332195. 
    /// </summary>
    public class PlackettBurmanDesign : ExperimentalDesign
    {

        private static Dictionary<string, string> parameter = new Dictionary<string, string>();
        static PlackettBurmanDesign()
        {
            parameter.Add("measurements", "int");
            parameter.Add("level", "int");
        }
       
        public override Dictionary<string, string> getParameterTypes()
        {
            return PlackettBurmanDesign.parameter;
        }


        /// <summary>
        /// Constants for the different seeds of this design. A seed states how many different values of one option
        /// and how many combinations are considered.
        /// </summary>
        public enum Seed { seed9_3, seed27_3, seed81_3, seed25_5, seed125_5, seed49_7 };


        /// <summary>
        /// Return the number of measurements for one seed. 
        /// </summary>
        /// <param name="seed">A seed for the Plackett Burman Design.</param>
        /// <returns>Number of measurements for the specific seed. Or zero if the seed is not defined.</returns>
        public int getMeasurements(PlackettBurmanDesign.Seed seed)
        {
            switch (seed)
            {
                case Seed.seed9_3:
                    return 9;
                case Seed.seed81_3:
                    return 81;
                case Seed.seed27_3:
                    return 27;
                case Seed.seed25_5:
                    return 25;
                case Seed.seed125_5:
                    return 125;
                case Seed.seed49_7:
                    return 49;
            }
            return 0;
        }

        /// <summary>
        /// Return the number of distinct values of one numerical feature that are considered from the design using a specific seed. 
        /// </summary>
        /// <param name="seed">A seed for the Plackett Burman Design.</param>
        /// <returns>Number of distinct values of one numerical feature for the specific seed. Or zero if the seed is not defined.</returns>
        public int getLevel(PlackettBurmanDesign.Seed seed)
        {
            switch (seed)
            {
                case Seed.seed9_3:
                case Seed.seed81_3:
                case Seed.seed27_3:
                    return 3;
                case Seed.seed25_5:
                case Seed.seed125_5:
                    return 5;
                case Seed.seed49_7:
                    return 7;
            }
            return 0;
        }


        private Dictionary<Seed, int[]> seeds;
        private Seed chosenSeed;

        private int[,] matrix;
        private HashSet<List<int>> runs = new HashSet<List<int>>(new SequenceComparer<int>());

        public override string getName()
        {
            return "PlackettBurmanDesign";
        }


        public PlackettBurmanDesign(List<NumericOption> options)
            : base(options)
        {
            initSeeds();
            chosenSeed = chooseSeedDymamic();
        }

        private Seed chooseSeedDymamic()
        {
            int numOptions = options.Count;
            // TODO Domain Knowlegde : currently we only consider linear interaction and the features also have only a linear influence
            int firstOrderInteractions = getFak(numOptions) / (getFak(2) * getFak(numOptions - 2));

            int numberToIdentifyAllForstOrder = numOptions * 2 + firstOrderInteractions;

            if (numberToIdentifyAllForstOrder <= 9)
                return Seed.seed9_3;
            if (numberToIdentifyAllForstOrder <= 27)
                return Seed.seed27_3;
            return Seed.seed81_3;
        }

        public static int getFak(int n)
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Creates a Plackett Burman design for the configuration options using a specific seed. 
        /// </summary>
        /// <param name="options">The set of numeric configuration options the design is created for.</param>
        /// <param name="currSeed">The seed used during the generation process.</param>
        public PlackettBurmanDesign(List<NumericOption> options, Seed currSeed)
            : base(options)
        {
            initSeeds();
            chosenSeed = currSeed;
        }


        private void initSeeds()
        {
            // set the seed values (according to http://www.jstor.org/discover/10.2307/2332195)
            seeds = new Dictionary<Seed, int[]>();
            seeds.Add(Seed.seed9_3, new int[] { 0, 1, 2, 2, 0, 2, 1, 1 });
            seeds.Add(Seed.seed27_3, new int[] { 0, 0, 1, 0, 1, 2, 1, 1, 2, 0, 1, 1, 1, 0, 0, 2, 0, 2, 1, 2, 2, 1, 0, 2, 2, 2 });
            seeds.Add(Seed.seed81_3, new int[] { 0, 1, 1, 1, 1, 2, 0, 1, 2, 1, 1, 2, 1, 2, 0, 2, 0, 2, 2, 1, 1, 0, 2, 0, 1, 1, 0, 0, 1, 2, 2, 2, 0, 2, 1, 0, 0, 2, 0, 0, 0, 2, 2, 2, 2, 1, 0, 2, 1, 2, 2, 1, 2, 1, 0, 1, 0, 1, 1, 2, 2, 0, 1, 0, 2, 2, 0, 0, 2, 1, 1, 1, 0, 1, 2, 0, 0, 1, 0, 0 });
            seeds.Add(Seed.seed25_5, new int[] { 0, 4, 1, 1, 2, 1, 0, 3, 2, 2, 4, 2, 0, 1, 4, 4, 3, 4, 0, 2, 3, 3, 1, 3 });
            seeds.Add(Seed.seed125_5, new int[] { 0, 2, 2, 2, 1, 0, 4, 1, 1, 4, 1, 3, 1, 3, 4, 1, 2, 0, 2, 1, 1, 0, 2, 4, 4, 3, 1, 4, 0, 2, 0, 0, 4, 4, 4, 2, 0, 3, 2, 2, 3, 2, 1, 2, 1, 3, 2, 4, 0, 4, 2, 2, 0, 4, 3, 3, 1, 2, 3, 0, 4, 0, 0, 3, 3, 3, 4, 0, 1, 4, 4, 1, 4, 2, 4, 2, 1, 4, 3, 0, 3, 4, 4, 0, 3, 1, 1, 2, 4, 1, 0, 3, 0, 0, 1, 1, 1, 3, 0, 2, 3, 3, 2, 3, 4, 3, 4, 2, 3, 1, 0, 1, 3, 3, 0, 1, 2, 2, 4, 3, 2, 0, 1, 0 });
            seeds.Add(Seed.seed49_7, new int[] { 0, 1, 2, 6, 2, 2, 1, 6, 0, 5, 3, 2, 3, 3, 5, 2, 0, 4, 1, 3, 1, 1, 4, 3, 0, 6, 5, 1, 5, 5, 6, 1, 0, 2, 4, 5, 4, 4, 2, 5, 0, 3, 6, 4, 6, 6, 3, 4 });
        }

        public override bool computeDesign()
        {
            int numFeatures = options.Count;

            int[] seed = seeds[chosenSeed];
            matrix = new int[seed.Length + 1, numFeatures];

            int offset = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (j == matrix.GetLength(0) - 1)
                    {
                        matrix[j, i] = 0;
                    }
                    else
                    {
                        matrix[j, i] = seed[(j + offset) % seed.Length];
                    }
                }
                offset++;
            }


            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                List<int> run = new List<int>();
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    run.Add(matrix[i, j]);
                }
                runs.Add(run);
            }

            return compute();
        }

        /// <summary>
        /// Computes the selected values combinations for the Plackett Burman design using the specific parameters. The two parameters are "level" and "measurements" defining a seed used during the
        /// generation. 
        /// 
        /// Possible combinations of the level and measurements are: 
        /// 3 9, 3 27, 81 3, 5 25, 5 125, and 7 49. 
        /// 
        /// If non of this combiantions is used, the method uses the seed with tree levels and 9 measurements.
        /// 
        /// </summary>
        /// <param name="designOptions">The parameters used during computation of samplings.</param>
        /// <returns>True if the samplings could be computed.</returns>
        public override bool computeDesign(Dictionary<string, string> designOptions)
        {
            int level = 3;
            int measurements = 9;

            foreach (KeyValuePair<string, string> param in designOptions)
            {
                if (param.Key == "level")
                    level = Convert.ToInt32(param.Value);
                if (param.Key == "measurements")
                    measurements = Convert.ToInt32(param.Value);

            }

            this.chosenSeed = getSeed(measurements, level);



            return computeDesign();

        }

        private bool compute()
        {

            // select values of numerical options by number of levels defined in seed
            Dictionary<Tuple<NumericOption, int>, double> values = new Dictionary<Tuple<NumericOption, int>, double>();
            int maxVal = this.getLevel(chosenSeed) - 1;
            foreach (NumericOption vf in options)
            {
                List<double> valuesOfAFeature = sampleOption(vf, this.getLevel(chosenSeed), true);

                while (valuesOfAFeature.Count < this.getLevel(chosenSeed))
                {
                    valuesOfAFeature.Add(vf.getRandomValue(valuesOfAFeature.Count));
                }


                for (int i = 0; i < valuesOfAFeature.Count; i++)
                {
                    values.Add(Tuple.Create(vf, i), valuesOfAFeature[i]);
                }
                
            }
            //-----------------------------



            List<Dictionary<NumericOption, double>> configs = new List<Dictionary<NumericOption, double>>();
            // use runs for iteration if there where duplicates
            int count = runs.Count == 0 ? matrix.GetLength(0) : runs.Count;

            for (int i = 0; i < count; i++)
            {
                Dictionary<NumericOption, double> run = new Dictionary<NumericOption, double>();

                int j = 0;
                foreach (NumericOption vf in options)
                {
                    run.Add(vf, values[Tuple.Create(vf, runs.Count == 0 ? matrix[i, j] : runs.ElementAt(i)[j])]);
                    j++;
                }

                configs.Add(run);
            }

            this.selectedConfigurations = configs;

            return true;
        }


        class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
        {
            public bool Equals(IEnumerable<T> seq1, IEnumerable<T> seq2)
            {
                return seq1.SequenceEqual(seq2);
            }

            public int GetHashCode(IEnumerable<T> seq)
            {
                int hash = 1234;
                foreach (T elem in seq)
                    hash = hash * 42 + elem.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns a seed specified by a given number of measurements and levels. If no seed for a given combiantion of measurements and levels is defined, the a seed using three level and 9
        /// measurements is returned. 
        /// </summary>
        /// <param name="measurements">A number of measurements.</param>
        /// <param name="level">Distinc values from one numeric option that have to be consided.</param>
        /// <returns>A seed defined by the measurements and levels. </returns>
        public static Seed getSeed(int measurements, int level)
        {
            if (measurements == 9 && level == 3)
                return Seed.seed9_3;
            if (measurements == 27 && level == 3)
                return Seed.seed27_3;
            if (measurements == 81 && level == 3)
                return Seed.seed81_3;
            if (measurements == 25 && level == 5)
                return Seed.seed25_5;
            if (measurements == 125 && level == 5)
                return Seed.seed125_5;
            if (measurements == 49 && level == 7)
                return Seed.seed49_7;
            return Seed.seed9_3;
        }

        public void setSeed(int measurements, int level)
        {
            chosenSeed = getSeed(measurements, level);
        }
    }
}
