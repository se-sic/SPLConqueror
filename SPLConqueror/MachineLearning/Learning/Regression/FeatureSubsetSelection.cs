using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILNumerics;
using System.IO;
using Accord.Math;
using Accord.Math.Decompositions;
using System.Diagnostics;
using SPLConqueror_Core;
using MachineLearning.Learning;

namespace MachineLearning.Learning.Regression
{
    public class FeatureSubsetSelection
    {
        //Information about the state of learning
        protected InfluenceModel infModel = null;
        protected List<LearningRound> learningHistory = new List<LearningRound>();
        protected Dictionary<Feature, InfluenceFunction> currentModel = new Dictionary<Feature, InfluenceFunction>();
        protected List<Feature> initialFeatures = new List<Feature>();

        //Learning and validation data sets
        protected List<Configuration> learningSet = new List<Configuration>();
        protected List<Configuration> validationSet = new List<Configuration>();
        protected ILArray<double> Y_learning, Y_validation = ILMath.empty();
        protected Dictionary<Feature, ILArray<double>> DM_columns = new Dictionary<Feature, ILArray<double>>();
        
        
        public FeatureSubsetSelection(InfluenceModel infModel)
        {
            this.infModel = infModel;
            foreach (var opt in infModel.BinaryOptionsInfluence)
                initialFeatures.Add(new Feature());
        }

        #region learning algorithm
        
        /// <summary>
        /// Performs learning using an adapted feature-selection algorithm.
        /// Learning is done in rounds, in which each round adds an additional feature (i.e., configuration option or interaction) to the current model containing all influences.
        /// Abort criteria is derived from ML_Settings class, such as number of rounds, minimum error, etc.
        /// </summary>
        public void learn()
        {
            if (!allInformationAvailable())
                return;

            LearningRound current = new LearningRound();

            while (!abortLearning(current))
            {
                current = performForwardStep(current);
                learningHistory.Add(current);

                if (ML_Settings.backward)
                {
                    current = performBackwardStep(current);
                    learningHistory.Add(current);
                }
            }
        }

        
        protected LearningRound performForwardStep(LearningRound current)
        {
            //Error in this round (depends on crossvalidation)
            double minimalRoundError = Double.MaxValue;

        }
        


        protected LearningRound performBackwardStep(LearningRound current)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region Check for learning abort

        protected bool abortLearning(LearningRound current)
        {
            if (current.round >= ML_Settings.maxRounds)
                return true;
            if (abortDueError(current))
                return true;
            return false;
        }

        protected bool abortDueError(LearningRound current)
        {
            throw new NotImplementedException();
        }
        #endregion

        protected bool allInformationAvailable()
        {
            if (this.learningSet.Count == 0 || this.validationSet.Count == 0 || this.infModel == null)
            {
                ErrorLog.logError("Error: you need to specify a learning and validation set.");
                return false;
            }
            return true;
        }

        #region set data set
        /// <summary>
        /// Converts a List of configurations with its measured value (we take the non-functional property of the global state) into a a learning matrix.
        /// </summary>
        /// <param name="measurements">The configurations containing the measured values.</param>
        public void setLearningSet(List<Configuration> measurements)
        {
            double[] temparryLearn = new double[measurements.Count]; ;//measured values
            for (int i = 0; i < measurements.Count; i++)
            {
                this.learningSet.Add(measurements[i]);
                double val = 0;
                try
                {
                    val = measurements[i].GetNFPValue(GlobalState.currentNFP);
                } catch(ArgumentException argEx) {
                    ErrorLog.logError(argEx.Message);
                    val = measurements[i].GetNFPValue();
                }
                temparryLearn[i] = val;
            }
            Y_learning = temparryLearn;
            Y_learning = Y_learning.T;
        }


        /// <summary>
        /// Converts the given configurations into a validation matrix used to compute the error for a learned model.
        /// </summary>
        /// <param name="measurements">The configurations containing the measured values.</param>
        public void setValidationSet(List<Configuration> measurements)
        {
            double[] tempArrayValid = new double[measurements.Count];
            for (int i = 0; i < measurements.Count; i++)
            {
                this.learningSet.Add(measurements[i]);
                double val = 0;
                try
                {
                    val = measurements[i].GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    ErrorLog.logError(argEx.Message);
                    val = measurements[i].GetNFPValue();
                }
                tempArrayValid[i] = val;
            }
            Y_validation = tempArrayValid;
            Y_validation = Y_validation.T;
        }

        //TODO: K-fold crossvalidation
        #endregion


        #region Constructing data matrix for regression
        protected ILArray<double> createDataMatrix(List<Feature> featureSet)
        {
            ILArray<double> DM = ILMath.zeros(featureSet.Count, this.learningSet.Count);
            for (int i = 0; i < featureSet.Count; i++)
            {
                if (DM_columns.Keys.Contains(featureSet[i]))
                    DM[ILMath.full, i] = DM_columns[featureSet[i]];
                else
                {
                    generateDM_column(featureSet[i]);
                    DM[":", i] = DM_columns[featureSet[i]];
                }
            }

            return DM;
        }

        protected void generateDM_column(Feature feature)
        {
            ILArray<double> column = ILMath.zeros(this.learningSet.Count);
            int i = 0;
            foreach (Configuration config in this.learningSet)
            {
                if (feature.validConfig(config))
                {
                    column[i] = feature.eval(config);
                }
                i++;
            }
            this.DM_columns.Add(feature, column);
        }
        #endregion

    }
}
