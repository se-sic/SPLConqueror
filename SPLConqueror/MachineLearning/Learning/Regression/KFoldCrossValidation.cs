using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;


namespace MachineLearning.Learning.Regression
{
    public class KFoldCrossValidation
    {

        ML_Settings settings;
        List<Configuration> trainingSet = null;

        public KFoldCrossValidation(ML_Settings _settings, List<Configuration> _trainingSet)
        {
            this.settings = _settings;
            trainingSet = _trainingSet;
        }

        public double learn()
        {
            int sizeOfTestSet = trainingSet.Count / settings.crossValidation_k;

            List<Configuration>[] subsets = new List<Configuration>[settings.crossValidation_k];
            for (int i = 0; i < settings.crossValidation_k; i++)
            {
                subsets[i] = new List<Configuration>();
            }

            // separate the training Set into k roughly equal parts
            Random r = new Random(settings.crossValidation_k);
            for (int i = 0; i < trainingSet.Count; i++)
            {
                int set = r.Next(settings.crossValidation_k);
                subsets[set].Add(trainingSet[i]);
            }

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


    }
}
