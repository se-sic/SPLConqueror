using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;


namespace MachineLearning.Learning.Regression
{
    /// <summary>
    /// This class represents the 'K-Fold Cross Validation', which splits the training set into k partitions.
    /// Afterwards, k-1 partitions are used as training set while the remaining partition is used as validation set.
    /// This process is repeated k times, each time using a different partition as validation set.
    /// </summary>
    public class KFoldCrossValidation
    {

        ML_Settings settings;
        List<Configuration> trainingSet = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MachineLearning.Learning.Regression.KFoldCrossValidation"/> class.
        /// </summary>
        /// <param name="_settings">The settings that are needed for the cross validation.</param>
        /// <param name="_trainingSet">The sample set to perform cross validation on.</param>
        public KFoldCrossValidation(ML_Settings _settings, List<Configuration> _trainingSet)
        {
            this.settings = _settings;
            trainingSet = _trainingSet;
        }

        /// <summary>
		/// Perform 'K-Fold Cross Validation'. For further details <see cref="T:MachineLearning.Learning.Regression.KFoldCrossValidation"/>.
        /// </summary>
        /// <returns>The mean error rate of all k learning rounds.</returns>
        public double learn()
        {
            int sizeOfTestSet = trainingSet.Count / settings.crossValidation_k;

            // Shuffle the list with random seed k
            trainingSet = Shuffle<Configuration>(trainingSet, settings.crossValidation_k);

            // Split the list in multiple partitions
            List<Configuration>[] subsets = SplitList<Configuration>(trainingSet, settings.crossValidation_k);

            List<LearningRound> lastModels = new List<LearningRound>();

            // learn models for each split 
            for (int i = 0; i < settings.crossValidation_k; i++)
            {
                List<Configuration> currTrainingSet = new List<Configuration>();

                for (int j = 0; j < settings.crossValidation_k; j++)
                {
                    if (j != i)
                    {
                        currTrainingSet.AddRange(subsets[j]);
                    }
                }

                Learning experiment = new MachineLearning.Learning.Regression.Learning(currTrainingSet, subsets[i]);
                experiment.mlSettings = settings;
                experiment.learn();
                lastModels.Add(experiment.models[0].LearningHistory[experiment.models[0].LearningHistory.Count - 1]);

            }

            double error = 0;
            for (int i = 0; i < lastModels.Count; i++)
            {
                error += lastModels[i].validationError_relative;
            }


            return error / settings.crossValidation_k;
        }


        /// <summary>
        /// Splits the list into different partitions.
        /// </summary>
        /// <returns>The list to split.</returns>
        /// <param name="list">List.</param>
        /// <param name="partitions">Partitions.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
	private static List<T>[] SplitList<T>(List<T> list, int partitions)
        {
            List<T>[] splitList = new List<T>[partitions];
            int sizeOfPartitions = list.Count / partitions;

            // Initialize the lists
            for (int i = 0; i < splitList.Length; i++)
            {
                splitList[i] = new List<T>();
            }

            // Firstly, insert the elements of the partitions
            for (int i = 0; i < sizeOfPartitions * partitions; i++)
            {
                int partitionNumber = i / sizeOfPartitions;
                splitList[partitionNumber].Add(list[i]);
            }

            // Secondly, add the remaining elements
            for (int i = sizeOfPartitions * partitions; i < list.Count; i++)
            {
                int partitionNumber = i % sizeOfPartitions;
                splitList[partitionNumber].Add(list[i]);
            }

            return splitList;
        }


        /// <summary>
        /// Shuffle the specified list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>      
        /// <typeparam name="T">The element type of the list</typeparam>
        /// <param name="seed">The seed to use for shuffling.</param>
        private static List<T> Shuffle<T>(List<T> list, int seed = 0)
        {
            List<T> shuffledList = new List<T>(list);
            int n = list.Count;
            Random rnd = new Random(seed);
            while (n > 1)
            {
                int k = (rnd.Next(0, n));
                n--;

                // Swap
                T value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }
            return shuffledList;
        }


    }
}
