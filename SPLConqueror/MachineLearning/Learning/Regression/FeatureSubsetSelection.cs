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

        public List<LearningRound> LearningHistory
        {
            get { return learningHistory; }
        }

        private LearningRound currentRound = null;

        protected LearningRound CurrentRound
        {
            get { if (learningHistory.Count == 0) return null; else return learningHistory[learningHistory.Count - 1]; }
        }

        protected List<Feature> initialFeatures = new List<Feature>();
        protected List<Feature> strictlyMandatoryFeatures = new List<Feature>();
        protected ML_Settings MLsettings = null;

        //Learning and validation data sets
        protected List<Configuration> learningSet = new List<Configuration>();
        protected List<Configuration> validationSet = new List<Configuration>();
        protected ILArray<double> Y_learning, Y_validation = ILMath.empty();
        protected Dictionary<Feature, ILArray<double>> DM_columns = new Dictionary<Feature, ILArray<double>>();
        
        /// <summary>
        /// Constructor of the learning class. It reads all configuration options and generates candidates for possible influences (i.e., features).
        /// </summary>
        /// <param name="infModel">The influence model which will later hold all determined influences and contains the variability model from which we derive all configuration options.</param>
        public FeatureSubsetSelection(InfluenceModel infModel, ML_Settings settings)
        {
            this.infModel = infModel;
            this.MLsettings = settings;
            foreach (var opt in infModel.Vm.BinaryOptions)
            {
                if (opt == infModel.Vm.Root)
                    continue;
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
            }
            this.strictlyMandatoryFeatures.Add(new Feature(infModel.Vm.Root.Name,infModel.Vm));
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
                    initialFeatures.Add(new Feature(influence.ToString(), this.infModel.Vm));
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
                current.FeatureSet.AddRange(this.strictlyMandatoryFeatures);

            do 
            {
                current = performForwardStep(current);
                learningHistory.Add(current);

                if (this.MLsettings.useBackward)
                {
                    current = performBackwardStep(current);
                    learningHistory.Add(current);
                }
            } while (!abortLearning(current));
            updateInfluenceModel();
        }

        /// <summary>
        /// Based on the given learning round, the method intantiates the influence model.
        /// </summary>
        /// <param name="current">The current learning round containing all determined features with their influences.</param>
        private void updateInfluenceModel()
        {
            this.infModel.BinaryOptionsInfluence.Clear();
            this.infModel.NumericOptionsInfluence.Clear();
            this.infModel.InteractionInfluence.Clear();
            LearningRound best = null;
            double lowestError = Double.MaxValue;
            foreach (LearningRound r in this.learningHistory)
            {
                if (r.validationError < lowestError)
                {
                    lowestError = r.validationError;
                    best = r;
                }
            }

            foreach (Feature f in best.FeatureSet)
            {
                //single binary option influence
                if (f.participatingBoolOptions.Count == 1 && f.participatingNumOptions.Count == 0 && f.getNumberOfParticipatingFeatures() == 1)
                {
                    this.infModel.BinaryOptionsInfluence.Add(f.participatingBoolOptions.ElementAt(0), f);
                    continue;
                }
                //single numeric option influence
                if (f.participatingBoolOptions.Count == 0 && f.participatingNumOptions.Count == 1 && f.getNumberOfParticipatingFeatures() == 1)
                {
                    if (this.infModel.NumericOptionsInfluence.Keys.Contains(f.participatingNumOptions.ElementAt(0)))
                    {
                        InfluenceFunction composed = new InfluenceFunction(this.infModel.NumericOptionsInfluence[f.participatingNumOptions.ElementAt(0)].ToString() + " + " + f.ToString(), f.participatingNumOptions.ElementAt(0));
                        this.infModel.NumericOptionsInfluence[f.participatingNumOptions.ElementAt(0)] = composed;
                    } 
                    else 
                        this.infModel.NumericOptionsInfluence.Add(f.participatingNumOptions.ElementAt(0), f);
                    continue;
                }
                //interaction influence
                Interaction i = new Interaction(f);
                this.infModel.InteractionInfluence.Add(i, f);
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
                candidates.AddRange(generateCandidates(currentModel.FeatureSet, basicFeature));

            //Learn for each candidate a new model and compute the error for each newly learned model
            foreach (Feature candidate in candidates)
            {
                List<Feature> newModel = copyCombination(currentModel.FeatureSet);
                newModel.Add(candidate);
                if (!fitModel(newModel))
                    continue;
                double temp = 0;
                double errorOfModel = computeModelError(newModel, out temp);
                if (errorOfModel < minimalRoundError)
                {
                    minimalRoundError = errorOfModel;
                    minimalErrorModel = newModel;
                }
            }
            double relativeErrorTrain = 0;
            double relativeErrorEval = 0;
            LearningRound newRound = new LearningRound(minimalErrorModel, computeLearningError(minimalErrorModel, out relativeErrorTrain), computeValidationError(minimalErrorModel, out relativeErrorEval), currentModel.round + 1);
            newRound.learningError_relative = relativeErrorTrain;
            newRound.validationError_relative = relativeErrorEval;
            return newRound;
        }



        private bool fitModel(List<Feature> newModel)
        {
            ILArray<double> DM = createDataMatrix(newModel);
            if (DM.Size.NumberOfElements == 0)
                return false;
            ILArray<double> DMT = DM.T;
            ILArray<double> temparray =null;

            double[,] fixSVDwithACCORD;
            var exp = toSystemMatrix<double>(DM.T);
            fixSVDwithACCORD = (double[,])exp;
            fixSVDwithACCORD = fixSVDwithACCORD.PseudoInverse();
            temparray = fixSVDwithACCORD;

            ILArray<double> constants;
            if (temparray.IsEmpty)
                constants = ILMath.multiply(DM, Y_learning.T);
            else
                constants = ILMath.multiply(temparray, Y_learning.T);
            double[] fittedConstant = constants.ToArray<double>();
            for(int i = 0; i < constants.Length; i++)
            {
                newModel[i].Constant = fittedConstant[i];
                //constants.GetValue(i);
            }
            //the fitting found no further influence
            if (newModel[constants.Length - 1].Constant == 0)
                return false;
            return true;
        }


        /// <summary>
        /// The method generates a list of candidates to be added to the current model. These candidates are later fitted using regression and rated for their accuracy in estimating the values of the validation set.
        /// The basicFeatures comes from the pool of initial features (e.g., all configuration options of the variability model or predefined combinations of options).
        /// Further candidates are combinations of the basic feature with features already present in the model. That is, we generate candidates as representatives of interactions or higher polynomial functions.
        /// Which candidates and polynomial degrees are generated depends on the parameters given in ML_settings.
        /// </summary>
        /// <param name="currentModel">The model containing the features found so far. These features are combined with the basic feature.</param>
        /// <param name="basicFeature">The feature for which we generate new candidates.</param>
        /// <returns>Returns a list of candidates that can be added to the current model.</returns>
        protected List<Feature> generateCandidates(List<Feature> currentModel, Feature basicFeature)
        {
            List<Feature> listOfCandidates = new List<Feature>();
            //add the feature to the list of candidates if it is not already in the model
            if (!currentModel.Contains(basicFeature))
                listOfCandidates.Add(basicFeature);

            foreach (var feature in currentModel)
            {
                if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingFeatures() == this.MLsettings.featureSizeTrehold))
                    continue;
                //We do not want to generate interactions with the root option
                if ((feature.participatingNumOptions.Count == 0 && feature.participatingBoolOptions.Count == 1 && feature.participatingBoolOptions.ElementAt(0) == infModel.Vm.Root)
                    || basicFeature.participatingNumOptions.Count == 0 && basicFeature.participatingBoolOptions.Count == 1 && basicFeature.participatingBoolOptions.ElementAt(0) == infModel.Vm.Root)
                    continue;
                Feature newCandidate = new Feature(feature.ToString() + " * " + basicFeature.ToString(), basicFeature.getVariabilityModel());
                if (!currentModel.Contains(newCandidate))
                    listOfCandidates.Add(newCandidate);
            }

            //if basic feature represents a numeric option and quadratic function support is activated, then we add a feature representing a quadratic functions of this feature
            if (this.MLsettings.quadraticFunctionSupport && basicFeature.participatingNumOptions.Count > 0)
            {
                Feature newCandidate = new Feature(basicFeature.ToString() + " * " + basicFeature.ToString() + " * " + basicFeature.ToString(), basicFeature.getVariabilityModel());
                if (!currentModel.Contains(newCandidate))
                    listOfCandidates.Add(newCandidate);
                
                foreach (var feature in currentModel)
                {
                    if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingFeatures() == this.MLsettings.featureSizeTrehold))
                        continue;
                    newCandidate = new Feature(feature.ToString() + " * " + basicFeature.ToString() + " * " + basicFeature.ToString(), basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);
                }
            }

            //if basic feature represents a numeric option and logarithmic function support is activated, then we add a feature representing a logarithmic functions of this feature 
            if (this.MLsettings.quadraticFunctionSupport && basicFeature.participatingNumOptions.Count > 0)
            {
                Feature newCandidate = new Feature("log10(" + basicFeature.ToString()+")", basicFeature.getVariabilityModel());
                if (!currentModel.Contains(newCandidate))
                    listOfCandidates.Add(newCandidate);

                foreach (var feature in currentModel)
                {
                    if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingFeatures() == this.MLsettings.featureSizeTrehold))
                        continue;
                    newCandidate = new Feature(feature.ToString() + " * log10(" + basicFeature.ToString()+")", basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);
                }
            }

            return listOfCandidates;
        }
        

        /// <summary>
        /// The backward steps aims at removing already learned features from the model if they have only a small impact on the prediction accuracy. 
        /// This should help keeping the model simple, reducing the danger of overfitting, and leaving local optima.
        /// </summary>
        /// <param name="current">The model learned so far containing the features that might be removed. Strictly mandatory features will not be removed.</param>
        /// <returns>A new model that might be smaller than the original one and might have a slightly worse prediction accuracy.</returns>
        protected LearningRound performBackwardStep(LearningRound current)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region error computation
        /// <summary>
        /// Computes the error of the current model for all configurations in the validation set.
        /// </summary>
        /// <param name="currentModel">The features that have been fitted so far.</param>
        /// <returns>The mean error of the validation set. It depends on the parameters in ML settings which loss function is used.</returns>
        private double computeValidationError(List<Feature> currentModel, out double relativeError)
        {
            return computeError(currentModel, this.validationSet, out relativeError);
        }

        /// <summary>
        /// This mestode produces an estimated for the given configuration based on the given model.
        /// </summary>
        /// <param name="currentModel">The model containing all fitted features.</param>
        /// <param name="c">The configuration for which the estimation should be performed.</param>
        /// <returns>The estimated value.</returns>
        private double estimate(List<Feature> currentModel, Configuration c)
        {
            double prediction = 0;
            for (int i = 0; i < currentModel.Count; i++)
            {
                ////First check whether the configuration options of the current feature are present in the configuration
                if (currentModel[i].validConfig(c))
                {
                    prediction += currentModel[i].eval(c) * currentModel[i].Constant;
                }
            }
            return prediction;
        }

        /// <summary>
        /// This methods computes the error for the given configuration based on the given model. It queries the ML settings to used the configured loss function.
        /// As the actual value, it uses the non-functional property stored in the global model. If this is not available in the configuration, it uses the default NFP of the configuration.
        /// </summary>
        /// <param name="currentModel">The model containing all fitted features.</param>
        /// <param name="configs">The configuration for which the error should be computed. It contains also the actually measured value.</param>
        /// <returns>The error depending on the configured loss function (e.g., relative, least squares, absolute).</returns>
        public double computeError(List<Feature> currentModel, List<Configuration> configs, out double relativeError)
        {
            double error_sum = 0;
            relativeError = 0;
            foreach (Configuration c in configs)
            {
                double estimatedValue = estimate(currentModel, c);
                double realValue = 0;
                try
                {
                    realValue = c.GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    GlobalState.logError.log(argEx.Message);
                    realValue = c.GetNFPValue();
                }
                relativeError += Math.Abs(100 - ((estimatedValue * 100) / realValue));
                double error = 0;
                switch (this.MLsettings.lossFunction)
                {
                    case ML_Settings.LossFunction.RELATIVE:
                        error = Math.Abs(100 - ((estimatedValue * 100) / realValue));
                        break;
                    case ML_Settings.LossFunction.LEASTSQUARES:
                        error = Math.Pow(realValue - estimatedValue, 2);
                        break;
                    case ML_Settings.LossFunction.ABSOLUTE:
                        error = Math.Abs(realValue - estimatedValue);
                        break;
                }
                error_sum += error;
            }
            relativeError = relativeError / configs.Count;
            return error_sum / configs.Count;
        }

        /// <summary>
        /// Computes the error of the current model for all configurations in the learning set.
        /// </summary>
        /// <param name="currentModel">The features that have been fitted so far.</param>
        /// /// <param name="relativeError">This is an out parameter, meaning that it gets assigned the relative error value to be used at the caller side.</param>
        /// <returns>The mean error of the validation set. It depends on the parameters in ML settings which loss function is used.</returns>
        private double computeLearningError(List<Feature> currentModel, out double relativeError)
        {
            return computeError(currentModel, this.learningSet, out relativeError);
        }

        /// <summary>
        /// This method computes the general error for the current model. It depends on the parameters of ML settings which loss function and whether cross validation is used.
        /// </summary>
        /// <param name="currentModel">The model for which the error should be computed.</param>
        /// <param name="relativeError">This is an out parameter, meaning that it gets assigned the relative error value to be used at the caller side.</param>
        /// <returns>The prediction error of the model.</returns>
        private double computeModelError(List<Feature> currentModel, out double relativeError)
        {
            if (!this.MLsettings.crossValidation)
                return computeValidationError(currentModel, out relativeError);
            else
            {
                return (computeLearningError(currentModel, out relativeError) + computeValidationError(currentModel, out relativeError) / 2);
            }
            //todo k-fold
        }

        /// <summary>
        /// Computes the prediction error for the given set of configurations based on the current influence model. The kind of error function depends on parameters in the ML settings.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="relativeError">This is an out parameter, meaning that it gets assigned the relative error value to be used at the caller side.</param>
        /// <returns>The error rate.</returns>
        public double evaluateError(List<Configuration> list, out double relativeError)
        {
            if (this.CurrentRound == null)
            {
                relativeError = Double.MaxValue;
                return -1;
            }
            return computeError(this.CurrentRound.FeatureSet, list, out relativeError);
        }
        #endregion

        #region Check for learning abort
        /// <summary>
        /// This methods checks whether the learning procedure should be aborted. For this decision, it uses parameters of ML settings, such as the number of rounds.
        /// </summary>
        /// <param name="current">The current state of learning (i.e., the current model).</param>
        /// <returns>True if we abort learning, false otherwise</returns>
        protected bool abortLearning(LearningRound current)
        {
            if (current.round >= this.MLsettings.numberOfRounds)
                return true;
            if (abortDueError(current))
                return true;
            return false;
        }

        /// <summary>
        /// This method checks whether we should abort learning due to perfect prediction or worsening prediction.
        /// </summary>
        /// <param name="current">The current model containing the fitted features.</param>
        /// <returns>True if we should abort learning, false otherwise.</returns>
        protected bool abortDueError(LearningRound current)
        {
            if (current.validationError == 0)
                return true;

            double error = 0;
            if (this.MLsettings.crossValidation)
                error = (current.learningError + current.validationError) / 2;
            else
                error = current.validationError;


            if (error < this.MLsettings.abortError)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method checks whether all information is available to start learning.
        /// </summary>
        /// <returns>True if all is set, false otherwise.</returns>
        protected bool allInformationAvailable()
        {
            if (this.learningSet.Count == 0 || this.validationSet.Count == 0 || this.infModel == null || this.MLsettings == null)
            {
                GlobalState.logError.log("Error: you need to specify a learning and validation set.");
                return false;
            }
            return true;
        }

        #endregion

        

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
                    GlobalState.logError.log(argEx.Message);
                    val = measurements[i].GetNFPValue();
                }
                temparryLearn[i] = val;
            }
            Y_learning = temparryLearn;
            Y_learning = Y_learning.T;

            //Now, we genrate for each inidividual option the data column. We also remove options from the initial feature set that occur in all or no variants of the learning set
            List<Feature> featuresToRemove = new List<Feature>();
            foreach (Feature f in this.initialFeatures)
            {
                if (f.participatingNumOptions.Count > 0)
                    continue;
                List<Feature> temp = new List<Feature>();
                temp.Add(f);
                ILArray<double> column = createDataMatrix(temp);
                int nbSelections = 0;
                int nbDeselections = 0;
                for (int i = 0; i < column.Length; i++)
                {
                    if (column[i] == 1)
                        nbSelections++;
                    if (column[i] == 0)
                        nbDeselections++;
                    if (nbSelections > 0 && nbDeselections > 0)
                        break;
                }
                if (nbSelections == this.learningSet.Count)
                    featuresToRemove.Add(f);
                if (nbDeselections == this.learningSet.Count)
                    featuresToRemove.Add(f);
            }
            foreach (var f in featuresToRemove)
                this.initialFeatures.Remove(f);
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
                this.validationSet.Add(measurements[i]);
                double val = 0;
                try
                {
                    val = measurements[i].GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    GlobalState.logError.log(argEx.Message);
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
        /// <summary>
        /// This is an essential method for fitting the constants of features. It constructs the data matrix. 
        /// The matrix has as rows the configurations of the training set and the columns represent the features (i.e., configuration options and interactions) of the model.
        /// A cell contains a value signaling the configured value of the option (or interaction) for the respective configuration. A column representing a binary option can, thus, have only 0 (not in configuration) or 1 (selected).
        /// For numeric options, the cell contains the value parameter of that option. If the feature represents a higher polynomial function, the value paramter is transformed according to the function (e.g., x = 10, we want to fit x^2 -> cell contains 10 * 10).
        /// </summary>
        /// <param name="featureSet">The list of features for which we want to determine the coefficients.</param>
        /// <returns>Returns an array representing the values of each feature for the configurations of the learning set.</returns>
        protected ILArray<double> createDataMatrix(List<Feature> featureSet)
        {
            ILArray<double> DM = ILMath.zeros(featureSet.Count, this.learningSet.Count);
            for (int i = 0; i < featureSet.Count; i++)
            {
                var x = featureSet[i];
                if (DM_columns.ContainsKey(x))
                    DM[i, ILMath.full] = DM_columns[x];
                else
                {
                    generateDM_column(x);
                    DM[i, ":"] = DM_columns[x];
                }
            }

            return DM;
        }

        /// <summary>
        /// For optimization, we generate a column for the data matrix only once. The rows of the columns represent the configurations of the learning set.
        /// The cells contain the value of the feature in the respective configuration. (see comment on createDataMatrix(..) function for more detail).
        /// </summary>
        /// <param name="feature">The feature for which we compute the data column.</param>
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
        /// <summary>
        /// This method creates new list of newly created feature objects and, this, allows for safely changing the features without affecting the original list.
        /// </summary>
        /// <param name="oldList">The list of features that needs to be copied.</param>
        /// <returns>A new list of newly created feature objects.</returns>
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

        /// <summary>
        /// This methids creates a system array based on an ILNumerics array.
        /// </summary>
        /// <typeparam name="T">The type of the objects stored in the array.</typeparam>
        /// <param name="A">The ILNumerics array.</param>
        /// <returns>Returns the newly created system array contain the values of the ILNumerics array.</returns>
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
