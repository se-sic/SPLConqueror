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
using System.Runtime.InteropServices;

namespace MachineLearning.Learning.Regression
{
    public class FeatureSubsetSelection
    {
        //Information about the state of learning
        protected InfluenceModel infModel = null;
        protected List<LearningRound> learningHistory = new List<LearningRound>();
        //protected Dictionary<Feature, InfluenceFunction> currentModel = new Dictionary<Feature, InfluenceFunction>();
        protected List<Feature> initialFeatures = new List<Feature>();
        protected List<Feature> strictlyMandatoryFeatures = new List<Feature>();

        //Learning and validation data sets
        protected List<Configuration> learningSet = new List<Configuration>();
        protected List<Configuration> validationSet = new List<Configuration>();
        protected ILArray<double> Y_learning, Y_validation = ILMath.empty();
        protected Dictionary<Feature, ILArray<double>> DM_columns = new Dictionary<Feature, ILArray<double>>();
        
        /// <summary>
        /// Constructor of the learning class. It reads all configuration options and generates candidates for possible influences (i.e., features).
        /// </summary>
        /// <param name="infModel">The influence model which will later hold all determined influences and contains the variability model from which we derive all configuration options.</param>
        public FeatureSubsetSelection(InfluenceModel infModel)
        {
            this.infModel = infModel;
            foreach (var opt in infModel.Vm.BinaryOptions)
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
            foreach (var opt in infModel.Vm.NumericOptions)
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
        }

        /// <summary>
        /// If we know the shape of a function or we know the existence of an interaction, we can use this knowledge to learn for exact those functions.
        /// </summary>
        /// <param name="knownInfluences">A list of influence functions, which might be a certain polynomial degree of a option or an interaction between options.</param>
        /// <param name="strict">If true, we always fit with the given influences not matter how good/bad they are. If false, we use these as hints but might not include them in the resulting model.</param>
        public void integrateDomainKnowledge(List<InfluenceFunction> knownInfluences, bool strict)
        {
            foreach (var influence in knownInfluences)
            {
                if(!strict)
                    initialFeatures.Add(new Feature(influence.ToString(), this.infModel.Vm);
                else
                    strictlyMandatoryFeatures.Add(new Feature(infModel.ToString(),this.infModel.Vm));
            }
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
            if (this.strictlyMandatoryFeatures.Count > 0)
                current.featureSet.AddRange(this.strictlyMandatoryFeatures);

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

        
        /// <summary>
        /// Makes one further step in learning. That is, it adds a further feature to the current model.
        /// </summary>
        /// <param name="currentModel">This parameter holds a list of features determined as important influencing factors so far.</param>
        /// <returns>Returns a new model (i.e. learning round) with an additional feature.</returns>
        internal LearningRound performForwardStep(LearningRound currentModel)
        {
            //Error in this round (depends on crossvalidation)
            double minimalRoundError = Double.MaxValue;
            List<Feature> minimalErrorModel = null;

            //Go through each feature of the initial set and combine them with the already present features to build new candidates
            List<Feature> candidates = new List<Feature>();
            foreach (Feature basicFeature in this.initialFeatures)
                candidates.AddRange(generateCandidates(currentModel.featureSet, basicFeature));

            //Learn for each candidate a new model and compute the error for each newly learned model
            foreach (Feature candidate in candidates)
            {
                List<Feature> newModel = copyCombination(currentModel.featureSet);
                newModel.Add(candidate);
                if (!learnModel(newModel))
                    continue;

                double errorOfModel = computeModelError(newModel);
                if (errorOfModel < minimalRoundError)
                {
                    minimalRoundError = errorOfModel;
                    minimalErrorModel = newModel;
                }
            }
            return new LearningRound(minimalErrorModel, computeLearningError(minimalErrorModel), computeValidationError(minimalErrorModel), currentModel.round++);
        }



        private bool learnModel(List<Feature> newModel)
        {
            ILArray<double> DM = createDataMatrix(newModel);
            if (DM.Size.NumberOfElements == 0)
                return false;
            ILArray<double> DMT = DM.T;
            ILArray<double> temparray =null;

            //We need to compute the pseudo inverse of the matrix, which might result in a runtime exception
            try
            {
                temparray = ILMath.pinv(DMT);
            }
            catch (Exception e)
            {
                double[,] fixSVDwithACCORD;
                var exp = toSystemMatrix<double>(DM.T);
                fixSVDwithACCORD = (double[,])exp;
                fixSVDwithACCORD = fixSVDwithACCORD.PseudoInverse();
                temparray = fixSVDwithACCORD;
            }

            ILArray<double> constants;
            if (temparray.IsEmpty)
                constants = ILMath.multiply(DM, Y_learning.T);
            else
                constants = ILMath.multiply(temparray, Y_learning.T);
            double[] fittedConstant = constants.ToArray<double>();
            for(int i = 0; i < constants.Length; i++)
            {
                newModel[i].constant = fittedConstant[i];
                //constants.GetValue(i);
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="basicFeature"></param>
        /// <returns></returns>
        protected List<Feature> generateCandidates(List<Feature> list, Feature basicFeature)
        {


            if (ML_Settings.quadraticFunctionSupport && basicFeature.participatingNumFeatures.Count > 0)
            { 
            }
        }
        


        protected LearningRound performBackwardStep(LearningRound current)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region error computation
        private double computeValidationError(List<Feature> currentModel)
        {
            double error_sum = 0;
            foreach (Configuration c in this.validationSet)
            {
                double estimatedValue = estimate(currentModel, c);
                double realValue = 0;
                try
                {
                    realValue = c.GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    ErrorLog.logError(argEx.Message);
                    realValue = c.GetNFPValue();
                }

                double error = 0;
                switch (ML_Settings.lossFunction)
                {
                    case ML_Settings.LossFunction.RELATIVE:
                        error = Math.Abs(100 - ((estimatedValue * 100) / realValue));
                        break;
                    case ML_Settings.LossFunction.LEASTSQUARES:
                        error = Math.Pow(realValue - estimatedValue, 2);
                        break;
                    case ML_Settings.LossFunction.ABSOLUTE:
                        break;
                }
                error_sum += error;
            }
            return error_sum / this.validationSet.Count;
        }

        private double estimate(List<Feature> currentModel, Configuration c)
        {
            double prediction = 0;
            for (int i = 0; i < currentModel.Count; i++)
            {
                ////First check whether the current feature or combination of feature is present in the current configuration
                if (currentModel[i].validConfig(c))
                {
                    prediction += currentModel[i].eval(c) * currentModel[i].constant;
                }
            }
            return prediction;
        }

        private double computeLearningError(List<Feature> currentModel)
        {
            throw new NotImplementedException();
        }

        private double computeModelError(List<Feature> currentModel)
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

        #region utility functions
        private List<Feature> copyCombination(List<Feature> oldList)
        {
            List<Feature> resultList = new List<Feature>();
            if (oldList == null)
                return resultList;
            foreach (Feature subset in oldList)
            {
                resultList.Add(new Feature(subset.ToString(), subset.getVariabilityModel()));
            }
            return resultList;
        }

        private static System.Array toSystemMatrix<T>(ILInArray<T> A)
        {
            using (ILScope.Enter(A))
            {
                // some error checking (to be improved...)
                if (object.Equals(A, null)) throw new ArgumentException("A may not be null");
                if (!A.IsMatrix) throw new ArgumentException("Matrix expected");

                // create return array
                System.Array ret = Array.CreateInstance(typeof(T), A.S.ToIntArray().Reverse().ToArray());
                // fetch underlying system array
                T[] workArr = A.GetArrayForRead();
                // copy memory block 
                Buffer.BlockCopy(workArr, 0, ret, 0, Marshal.SizeOf(typeof(T)) * A.S.NumberOfElements);
                return ret;
            }
        }
        #endregion
    }
}
