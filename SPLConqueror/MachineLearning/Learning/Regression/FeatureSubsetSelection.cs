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
using System.Collections.ObjectModel;
using MachineLearning.Solver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace MachineLearning.Learning.Regression
{
    public class FeatureSubsetSelection
    {
        //Information about the state of learning
        public InfluenceModel infModel = null;
        protected ObservableCollection<LearningRound> learningHistory = new ObservableCollection<LearningRound>();
        protected int hierachyLevel = 1;
        protected DateTime startTime;
        protected List<Feature> initialFeatures = new List<Feature>();
        protected List<Feature> strictlyMandatoryFeatures = new List<Feature>();
        protected ML_Settings MLsettings = null;
        protected List<Feature> bruteForceCandidates = new List<Feature>();
        protected IDictionary<Feature, double> bruteForceCandidateRate = new Dictionary<Feature, double>();
        public double finalError = 0.0;

        //Learning and validation data sets
        protected List<Configuration> learningSet = new List<Configuration>();
        protected List<Configuration> validationSet = new List<Configuration>();
        protected ILArray<double> Y_learning, Y_validation = ILMath.empty();
        protected ConcurrentDictionary<Feature, ILArray<double>> DM_columns = new ConcurrentDictionary<Feature, ILArray<double>>();

        //Optimization: Remember candidates with no or only a tiny improvement to test them not in every round, int = nb of remaining rounds to ignore this feature
        private Dictionary<Feature, int> badFeatures = new Dictionary<Feature, int>();

        public ObservableCollection<LearningRound> LearningHistory
        {
            get { return learningHistory; }
        }

        protected LearningRound CurrentRound
        {
            get { if (learningHistory.Count == 0) return null; else return learningHistory[learningHistory.Count - 1]; }
        }

        // TODO: unused method?
        public void clean()
        {
            infModel = null;
            learningHistory = new ObservableCollection<LearningRound>();
            hierachyLevel = 1;
            initialFeatures = new List<Feature>();
            strictlyMandatoryFeatures = new List<Feature>();
            MLsettings = null;
            bruteForceCandidates = new List<Feature>();
            learningSet = new List<Configuration>();
            validationSet = new List<Configuration>();
            Y_validation = ILMath.empty();
            Y_learning = ILMath.empty();
            DM_columns = new ConcurrentDictionary<Feature, ILArray<double>>();
            badFeatures = new Dictionary<Feature, int>();
        }

        public FeatureSubsetSelection()
        {

        }

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
            this.strictlyMandatoryFeatures.Add(new Feature(infModel.Vm.Root.Name, infModel.Vm));
            foreach (var opt in infModel.Vm.NumericOptions)
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
        }

        /// <summary>
        /// Initialize of the learning class. Required only when the empty constructor was used. It reads all configuration options and generates candidates for possible influences (i.e., features).
        /// </summary>
        /// <param name="infModel">The influence model which will later hold all determined influences and contains the variability model from which we derive all configuration options.</param>
        public void init(InfluenceModel infModel, ML_Settings settings)
        {
            this.infModel = infModel;
            this.MLsettings = settings;
            foreach (var opt in infModel.Vm.BinaryOptions)
            {
                if (opt == infModel.Vm.Root)
                    continue;
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
            }
            this.strictlyMandatoryFeatures.Add(new Feature(infModel.Vm.Root.Name, infModel.Vm));
            foreach (var opt in infModel.Vm.NumericOptions)
                initialFeatures.Add(new Feature(opt.Name, infModel.Vm));
        }

        #region parallelization
        class ModelFit
        {
            public List<Feature> newModel = new List<Feature>();
            public bool complete = true;
            public double error = Double.MaxValue;
        }
        

        #endregion

        /// <summary>
        /// If we know the shape of a function or we know the existence of an interaction, we can use this knowledge to learn for exact those functions.
        /// </summary>
        /// <param name="knownInfluences">A list of influence functions, which might be a certain polynomial degree of a option or an interaction between options.</param>
        /// <param name="strict">If true, we always fit with the given influences not matter how good/bad they are. If false, we use these as hints but might not include them in the resulting model.</param>
        public void integrateDomainKnowledge(List<InfluenceFunction> knownInfluences, bool strict)
        {
            foreach (var influence in knownInfluences)
            {
                if (!strict)
                    initialFeatures.Add(new Feature(influence.ToString(), this.infModel.Vm));
                else
                    strictlyMandatoryFeatures.Add(new Feature(infModel.ToString(), this.infModel.Vm));
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
            this.startTime = System.DateTime.Now;
            LearningRound current = new LearningRound();
            if (this.strictlyMandatoryFeatures.Count > 0)
                current.FeatureSet.AddRange(this.strictlyMandatoryFeatures);
            LearningRound previous;
            do
            {
                previous = current;
                current = performForwardStep(previous);
                if (current == null)
                    return;
                learningHistory.Add(current);

                if (this.MLsettings.useBackward)
                {
                    current = performBackwardStep(current);
                    learningHistory.Add(current);
                }
            } while (!abortLearning(current, previous));
            updateInfluenceModel();
            this.finalError = evaluateError(this.validationSet, out this.finalError);
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
                if (f.participatingBoolOptions.Count == 1 && f.participatingNumOptions.Count == 0 && f.getNumberOfParticipatingOptions() == 1)
                {
                    this.infModel.BinaryOptionsInfluence.Add(f.participatingBoolOptions.ElementAt(0), f);
                    continue;
                }
                //single numeric option influence
                if (f.participatingBoolOptions.Count == 0 && f.participatingNumOptions.Count == 1 && f.getNumberOfParticipatingOptions() == 1)
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
        /// <param name="previousRound">State of the learning process untill this forward step.</param>
        /// <returns>Returns a new model (i.e. learning round) with an additional feature.</returns>
        internal LearningRound performForwardStep(LearningRound previousRound)
        {
            //Error in this round (depends on crossvalidation)
            double minimalRoundError = Double.MaxValue;
            double maximalWeightedAbsoluteRoundInfluence = 0.0;
            double maximalRoundScore = Double.MinValue;
            IDictionary<Feature, double> bfCandidateRate = null;
            List<Feature> bestModel = null;

            //Go through each feature of the initial set and combine them with the already present features to build new candidates
            List<Feature> candidates = new List<Feature>();
            if (this.MLsettings.bruteForceCandidates)
            {
                candidates = generateBruteForceCandidates(previousRound.FeatureSet, initialFeatures, out bfCandidateRate);
            }
            else
            {
                candidates = generateCandidates(previousRound.FeatureSet, this.initialFeatures);
            }

            //If we got no candidates and we perform hierachical learning, we go one step further
            if (candidates.Count == 0 && this.MLsettings.withHierarchy)
            {
                if (this.hierachyLevel > 10)
                    return null;
                this.hierachyLevel++;
                return performForwardStep(previousRound);
            }

            ConcurrentDictionary<Feature, double> errorOfFeature = new ConcurrentDictionary<Feature, double>();
            ConcurrentDictionary<Feature, List<Feature>> errorOfFeatureWithModel = new ConcurrentDictionary<Feature, List<Feature>>();
            Feature bestCandidate = null;
            
            List<Task> tasks = new List<Task>();
            //Learn for each candidate a new model and compute the error for each newly learned model
            foreach (Feature candidate in candidates)
            {
                Feature threadCandidate = new Feature(candidate.getPureString(),candidate.getVariabilityModel());
                if (MLsettings.ignoreBadFeatures && this.badFeatures.Keys.Contains(candidate) && this.badFeatures[candidate] > 0)
                {
                    this.badFeatures[candidate]--;
                    continue;
                }
                if (errorOfFeature.Keys.Contains(threadCandidate))
                {
                    continue;
                }
                    
                List<Feature> newModel = copyCombination(previousRound.FeatureSet);
                newModel.Add(threadCandidate);
                if (this.MLsettings.parallelization)
                {//Parallel execution of fitting the model for the current candidate
                    Task task = Task.Factory.StartNew(() =>
                    {
                        ModelFit fi = evaluateCandidate(newModel);
                        if (fi.complete)
                        {
                            errorOfFeature.GetOrAdd(threadCandidate, fi.error);
                            errorOfFeatureWithModel.GetOrAdd(threadCandidate, fi.newModel);
                        }
                    }
                    );
                    tasks.Add(task);
                }
                else
                {//Serial execution of the fitting model for the current candidate
                    ModelFit fi = evaluateCandidate(newModel);
                    if (fi.complete)
                    {
                        errorOfFeature.GetOrAdd(threadCandidate, fi.error);
                        errorOfFeatureWithModel.GetOrAdd(threadCandidate, fi.newModel);
                    }
                }
            }
            if (this.MLsettings.parallelization)
                Task.WaitAll(tasks.ToArray());

            // Evaluation of the candidates
            if (MLsettings.scoreMeasure == ML_Settings.ScoreMeasure.RELERROR)
            {
                foreach (Feature candidate in errorOfFeature.Keys)
                {
                    var candidateError = errorOfFeature[candidate];
                    var candidateScore = previousRound.learningError_relative - candidateError;
                    if (candidateScore > 0)
                    {
                        if (MLsettings.candidateSizePenalty)
                        {
                            candidateScore /= candidate.getNumberOfDistinctParticipatingOptions();
                        }
                        if (candidateScore > maximalRoundScore)
                        {
                            maximalRoundScore = candidateScore;
                            minimalRoundError = errorOfFeature[candidate];
                            bestCandidate = candidate;
                            bestModel = errorOfFeatureWithModel[candidate];
                        } else
                        {
                            candidate.Constant = 1;                        
                        }
                    }
                }
            } else if (MLsettings.scoreMeasure == ML_Settings.ScoreMeasure.INFLUENCE)
            {
                throw new NotImplementedException();
//                foreach (Feature candidate in errorOfFeature.Keys)
//                {
//                    double candidateRate = bfCandidateRate[candidate];
//                    double candidateWeightedAbsoluteInfluence = Math.Abs(candidate.Constant) * candidateRate;
//                    if (candidateWeightedAbsoluteInfluence > maximalWeightedAbsoluteRoundInfluence)
//                    {
//                        maximalWeightedAbsoluteRoundInfluence = candidateWeightedAbsoluteInfluence;
//                        bestCandidate = candidate;
//                        bestModel = errorOfFeatureWithModel[candidate];
//                    } else
//                        candidate.Constant = 1;
//                }
            }

            //error computations and logging stuff
            double relativeErrorEval = 0;
            if (MLsettings.ignoreBadFeatures)
            {
                addFeaturesToIgnore(errorOfFeature);
            }
            if (bestModel == null)
            {
                return null;
            }
            else
            {
                bestModel = copyCombination(bestModel);
                LearningRound newRound = new LearningRound(bestModel, minimalRoundError, computeValidationError(bestModel, out relativeErrorEval), previousRound.round + 1);
                newRound.learningError_relative = minimalRoundError;
                newRound.validationError_relative = relativeErrorEval;
                newRound.elapsedTime = DateTime.Now - startTime;
                newRound.bestCandidate = bestCandidate;
                newRound.bestCandidateSize = bestCandidate.getNumberOfParticipatingOptions();
                newRound.bestCandidateScore = maximalRoundScore;
                return newRound;
            }
        }


        private ModelFit evaluateCandidate(List<Feature> model)
        {
            ModelFit fit = new ModelFit();
            fit.complete = fitModel(model);
            double temp;
            fit.error = computeModelError(model, out temp);
            fit.newModel = model;
            return fit;
        }

        /// <summary>
        /// Optimization: we do not want to consider candidates in the next X rounds that showed no or only a slight improvment in accuracy relative to all other candidates
        /// </summary>
        /// <param name="errorOfCandidates">The Dictionary containing all candidate features with their fitted error rate.</param>
        private void addFeaturesToIgnore(ConcurrentDictionary<Feature, double> errorOfCandidates)
        {
            List<KeyValuePair<Feature, double>> myList = errorOfCandidates.ToList();
            myList.Sort((x, y) => x.Value.CompareTo(y.Value));
            int minNumberToKeep = 5;
            for (int i = myList.Count - 1; i > myList.Count / 2; i--)
            {
                if (i <= (minNumberToKeep))
                    return;
                if (this.badFeatures.Keys.Contains(myList[i].Key))
                    this.badFeatures[myList[i].Key] = 3;
                else
                    this.badFeatures.Add(myList[i].Key, 3);//wait for 3 rounds
            }
        }


        private bool fitModel(List<Feature> newModel)
        {
            ILArray<double> DM = createDataMatrix(newModel);
            if (DM.Size.NumberOfElements == 0)
                return false;
            //   ILArray<double> DMT = DM.T;
            ILArray<double> temparray = null;

            double[,] fixSVDwithACCORD;
            //var exp = toSystemMatrix<double>(DM.T);
            // fixSVDwithACCORD = (double[,])exp;
            fixSVDwithACCORD = ((double[,])toSystemMatrix<double>(DM.T)).PseudoInverse();
            temparray = fixSVDwithACCORD;

            ILArray<double> constants;
            if (temparray.IsEmpty)
                constants = ILMath.multiply(DM, Y_learning.T);
            else
                constants = ILMath.multiply(temparray, Y_learning.T);
            double[] fittedConstant = constants.ToArray<double>();
            for (int i = 0; i < constants.Length; i++)
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
        /// <param name="basicFeatures">The features for which we generate new candidates.</param>
        /// <returns>Returns a list of candidates that can be added to the current model.</returns>
        protected List<Feature> generateCandidates(List<Feature> currentModel, List<Feature> basicFeatures)
        {
            var listOfCandidates = new List<Feature>();
            foreach (Feature basicFeature in basicFeatures)
            {
                //add the feature to the list of candidates if it is not already in the model
                if (!currentModel.Contains(basicFeature))
                    listOfCandidates.Add(basicFeature);

                if (this.MLsettings.withHierarchy && this.hierachyLevel == 1)
                    continue;

                foreach (var feature in currentModel)
                {
                    if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                        continue;
                    //We do not want to generate interactions with the root option
                    if ((feature.participatingNumOptions.Count == 0 && feature.participatingBoolOptions.Count == 1 && feature.participatingBoolOptions.ElementAt(0) == infModel.Vm.Root)
                    || basicFeature.participatingNumOptions.Count == 0 && basicFeature.participatingBoolOptions.Count == 1 && basicFeature.participatingBoolOptions.ElementAt(0) == infModel.Vm.Root)
                        continue;
                    if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                        continue;

                    //Binary times the same binary makes no sense
                    if (basicFeature.participatingBoolOptions.Count > 0)
                    {
                        foreach (var binOption in basicFeature.participatingBoolOptions)
                            if (feature.participatingBoolOptions.Contains(binOption))
                                goto nextRound;
                    }

                    Feature newCandidate = new Feature(feature, basicFeature, basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);
                nextRound:
                    { }
                }

                //if basic feature represents a numeric option and quadratic function support is activated, then we add a feature representing a quadratic functions of this feature
                if (this.MLsettings.quadraticFunctionSupport && basicFeature.participatingNumOptions.Count > 0)
                {
                    Feature newCandidate = new Feature(basicFeature, basicFeature, basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);

                    foreach (var feature in currentModel)
                    {
                        if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                            continue;
                        if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                            continue;
                        newCandidate = new Feature(feature, newCandidate, basicFeature.getVariabilityModel());
                        if (!currentModel.Contains(newCandidate))
                            listOfCandidates.Add(newCandidate);
                    }
                }

                //if basic feature represents a numeric option and logarithmic function support is activated, then we add a feature representing a logarithmic functions of this feature 
                if (this.MLsettings.learn_logFunction && basicFeature.participatingNumOptions.Count > 0)
                {
                    Feature newCandidate = new Feature("log10(" + basicFeature.getPureString() + ")", basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);

                    foreach (var feature in currentModel)
                    {
                        if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                            continue;
                        if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                            continue;
                        newCandidate = new Feature(feature.getPureString() + " * log10(" + basicFeature.getPureString() + ")", basicFeature.getVariabilityModel());
                        if (!currentModel.Contains(newCandidate))
                            listOfCandidates.Add(newCandidate);
                    }
                }

                if (this.MLsettings.learn_asymFunction && basicFeature.participatingNumOptions.Count > 0)
                {
                    Feature newCandidate = new Feature("1 / " + basicFeature.getPureString(), basicFeature.getVariabilityModel());

                    if (basicFeature.participatingBoolOptions.Count == 0 && basicFeature.participatingNumOptions.All(x => x.Min_value > 0))
                    {
                        if (!currentModel.Contains(newCandidate))
                            listOfCandidates.Add(newCandidate);
                    }

                    foreach (var feature in currentModel)
                    {
                        if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                            continue;
                        if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                            continue;
                        newCandidate = new Feature(feature.getPureString() + " * 1 / " + basicFeature.getPureString(), basicFeature.getVariabilityModel());
                        if (newCandidate.participatingBoolOptions.Count == 0 && newCandidate.participatingNumOptions.All(x => x.Min_value > 0))
                        {
                            if (!currentModel.Contains(newCandidate))
                                listOfCandidates.Add(newCandidate);
                        }
                    }
                }

                if (this.MLsettings.learn_ratioFunction && basicFeature.participatingNumOptions.Count > 0)
                {
                    Feature newCandidate = null;
                    foreach (var feature in currentModel)
                    {
                        if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                            continue;
                        if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                            continue;

                        if (basicFeature.participatingBoolOptions.Count == 0 && basicFeature.participatingNumOptions.All(x => x.Min_value > 0))
                        {
                            if (feature.participatingBoolOptions.Count == 0 && feature.participatingNumOptions.All(x => x.Min_value > 0))
                            {
                                newCandidate = new Feature(feature.getPureString() + " / " + basicFeature.getPureString(), basicFeature.getVariabilityModel());
                            }

                        }

                        if (newCandidate != null && !currentModel.Contains(newCandidate))
                            listOfCandidates.Add(newCandidate);
                    }
                }

                // learn mirrowed function
                if (this.MLsettings.learn_mirrowedFunction && basicFeature.participatingNumOptions.Count > 0)
                {
                    
                    Feature newCandidate = new Feature("(" + basicFeature.participatingNumOptions.First().Max_value + " - " + basicFeature.getPureString() + ")", basicFeature.getVariabilityModel());
                    if (!currentModel.Contains(newCandidate))
                        listOfCandidates.Add(newCandidate);

                    foreach (var feature in currentModel)
                    {
                        if (this.MLsettings.withHierarchy && feature.getNumberOfParticipatingOptions() >= this.hierachyLevel)
                            continue;
                        if (this.MLsettings.limitFeatureSize && (feature.getNumberOfParticipatingOptions() == this.MLsettings.featureSizeTreshold))
                            continue;

                   
                        newCandidate = new Feature(feature.getPureString() + "* (" + basicFeature.participatingNumOptions.First().Max_value + " - " + basicFeature.getPureString() + ")", basicFeature.getVariabilityModel());
                       

                        if (newCandidate != null && !currentModel.Contains(newCandidate))
                            listOfCandidates.Add(newCandidate);
                    }
                }


            }
            foreach (Feature f in listOfCandidates)
                f.Constant = 1;
            return listOfCandidates;
        }

        /// <summary>
        /// The method generates a list of candidates to be added to the current model. These candidates are later fitted using regression and rated for their accuracy in estimating the values of the validation set.
        /// The basicFeatures comes from the pool of initial features (e.g., all configuration options of the variability model or predefined combinations of options).
        /// Further candidates are combinations of the basic features. That is, we generate candidates as representatives of interactions or higher polynomial functions.
        /// Which candidates (i.e. their maximum size) and polynomial degrees are generated depends on the parameters given in ML_settings.
        /// Candidates that do not sattisfy the fieature model are removed.
        /// </summary>
        /// <param name="currentModel">The model containing the features found so far. These features are combined with the basic feature.</param>
        /// <param name="basicFeatures">The features for which we generate new candidates.</param>
        /// <param name = "bfCandidateRate">Maps a set of binary configuration options from the feature to the number of configurations in which these options occure.</param>
        /// <returns>Returns a list of candidates that can be added to the current model.</returns>
        protected List<Feature> generateBruteForceCandidates(List<Feature> currentModel, List<Feature> basicFeatures, out IDictionary<Feature, double> bfCandidateRate)
        {
            // Initialize brute force candidates.
            if (!bruteForceCandidates.Any())
            {
                var listOfCombinations = new List<List<Feature>>();
                if (this.MLsettings.withHierarchy)
                {
                    if (hierachyLevel <= MLsettings.featureSizeTreshold)
                    {
                        listOfCombinations = combinations(basicFeatures, hierachyLevel);
                    }
                }
                else
                {
                    listOfCombinations = combinationsUpToN(basicFeatures, MLsettings.featureSizeTreshold);
                }

                // Check feature combinations for validity and create candidates from the valid ones.
                foreach (var combination in listOfCombinations)
                {
                    Feature newCandidate = null;
                    foreach (var feature in combination)
                    {
                        newCandidate = newCandidate == null ? new Feature(feature.getPureString(), feature.getVariabilityModel()) : new Feature(newCandidate.getPureString() + '*' + feature.getPureString(), feature.getVariabilityModel());
                    }

                    if (newCandidate != null)
                    {
                        bool isValid = false;

                        // Use a SAT solver to check if the feature combination in the new candidate is vaild.
                        //var configChecker = new CheckConfigSAT(null);
                        //isValid = configChecker.checkConfigurationSAT(newCandidate.participatingBoolOptions.ToList(), newCandidate.getVariabilityModel(), true);

                        // Search all configurations for the feature combination of the candidate,
                        // if none of the configurations contains the feature combination
                        // the candidate is considered invalid.  If we have a set of all valid configuration,
                        // then we get the same results as using a SAT solver but much faster.
                        foreach (Configuration config in GlobalState.allMeasurements.Configurations)
                        {
                            var candidateBinaryOptions = newCandidate.participatingBoolOptions;
                            var configBinaryOptions = config.getBinaryOptions(BinaryOption.BinaryValue.Selected);
                            if (!candidateBinaryOptions.Except(configBinaryOptions).Any())
                            {
                                isValid = true;
                                break;
                            }
                        }

                        if (isValid)
                        {
                            //bruteForceCandidateRate[newCandidate] = binaryCandidateRate(newCandidate);
                            bruteForceCandidates.Add(newCandidate);
                        }
                    }
                }
            }

            // Remove candidates that are already in the model.
            bruteForceCandidates.RemoveAll(currentModel.Contains);

            bfCandidateRate = bruteForceCandidateRate;
            return bruteForceCandidates;
        }

        /// <summary>
        /// Count the number of configurations in which the candidate (the set of its binary options) occurs.
        /// </summary>
        /// <returns>The candidate incidence.</returns>
        /// <param name="candidate">Candidate feature.</param>
        double binaryCandidateRate(Feature candidate)
        {
            double counter = 0;
            foreach (Configuration config in GlobalState.allMeasurements.Configurations)
            {
                var candidateBinaryOptions = candidate.participatingBoolOptions;
                var configBinaryOptions = config.getBinaryOptions(BinaryOption.BinaryValue.Selected);
                if (!candidateBinaryOptions.Except(configBinaryOptions).Any())
                {
                    ++counter;
                }
            }
            return counter/GlobalState.allMeasurements.Configurations.Count();
        }

        /// <summary>
        /// Generates combinations of length r > 2 from elements in iterable.
        /// </summary>
        /// <param name="pool">Elements for combinations.</param>
        /// <param name="r">Length of combinations.</param>
        /// <returns>An Enumerable containing combinations.</returns>
        /// <typeparam name='T'>Elements' type.</typeparam>
        List<List<T>> combinations<T>(List<T> pool, int r)
        {
            var currentCombinations = new List<List<T>>();
            var n = pool.Count;
            if (r > n)
            {
                return new List<List<T>>();
                //yield break;
            }
            var indicies = Enumerable.Range(0, r).ToList();
            currentCombinations.Add(indicies.Select(x => pool[x]).ToList());
            //yield return indicies.Select(x => pool[x]);
            while (true)
            {
                int i = -1;
                foreach (int ii in Enumerable.Range(0, r).Reverse())
                {
                    if (indicies[ii] != ii + n - r)
                    {
                        i = ii;
                        break;
                    }
                }
                if (i == -1)
                {
                    break;
                }
                indicies[i]++;
                foreach (int j in Enumerable.Range(i + 1, (r - i - 1)))
                {
                    indicies[j] = indicies[j - 1] + 1;
                }
                //yield return indicies.Select(x => pool[x]);
                currentCombinations.Add(indicies.Select(x => pool[x]).ToList());
            }
            return currentCombinations;
        }

        /// <summary>
        /// Given the elements in interable, generate combinations of them of the length up to n 
        /// starting with length 1.
        /// </summary>
        /// <returns>Combinations up to the length n.</returns>
        /// <param name='iterable'>Elements for combinations.</param>
        /// <param name='n'>Maximum length of generated combinations.</param>
        /// <typeparam name='T'>Elements' type.</typeparam>
        List<List<T>> combinationsUpToN<T>(List<T> iterable, int n)
        {
            var allCombinations = new List<List<T>>();
            for (int i = 1; i <= n; ++i)
            {
                var currentCombinations = combinations(iterable, i);
                allCombinations.AddRange(currentCombinations);
            }
            return allCombinations;
        }


        /// <summary>
        /// The backward steps aims at removing already learned features from the model if they have only a small impact on the prediction accuracy. 
        /// This should help keeping the model simple, reducing the danger of overfitting, and leaving local optima.
        /// </summary>
        /// <param name="current">The model learned so far containing the features that might be removed. Strictly mandatory features will not be removed.</param>
        /// <returns>A new model that might be smaller than the original one and might have a slightly worse prediction accuracy.</returns>
        protected LearningRound performBackwardStep(LearningRound current)
        {
            if (current.round < 3 || current.FeatureSet.Count < 2)
                return current;
            bool abort = false;
            List<Feature> featureSet = copyCombination(current.FeatureSet);
            while (!abort)
            {
                double roundError = Double.MaxValue;
                Feature toRemove = null;
                foreach (Feature toDelete in featureSet)
                {
                    List<Feature> tempSet = copyCombination(featureSet);
                    tempSet.Remove(toDelete);
                    double relativeError = 0;
                    double error = computeModelError(tempSet, out relativeError);
                    if (error - this.MLsettings.backwardErrorDelta < current.validationError && error < roundError)
                    {
                        roundError = error;
                        toRemove = toDelete;
                    }
                }
                if (toRemove != null)
                    featureSet.Remove(toRemove);
                if (featureSet.Count <= 2)
                    abort = true;
            }
            current.FeatureSet = featureSet;
            return current;
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
        private static double estimate(List<Feature> currentModel, Configuration c)
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
            int skips = 0;
            foreach (Configuration c in configs)
            {
                double estimatedValue = estimate(currentModel, c);
                double realValue = 0;
                try
                {
                    if (!c.nfpValues.Keys.Contains(GlobalState.currentNFP))
                    {
                        skips++;
                        continue;
                    }
                    realValue = c.GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    GlobalState.logError.logLine(argEx.Message);
                    realValue = c.GetNFPValue();
                }
                //How to handle near-zero values???
                //http://math.stackexchange.com/questions/677852/how-to-calculate-relative-error-when-true-value-is-zero
                //http://stats.stackexchange.com/questions/86708/how-to-calculate-relative-error-when-the-true-value-is-zero

                if (realValue < 1)
                {//((2(true-est) / true+est) - 1 ) * 100
                    //continue;
                    skips++;
                    continue;
                }
                else
                {
                    double er = Math.Abs(100 - ((estimatedValue * 100) / realValue));
                    relativeError += er;
                }
                double error = 0;
                switch (this.MLsettings.lossFunction)
                {
                    case ML_Settings.LossFunction.RELATIVE:
                        if (realValue < 1)
                        {
                            error = Math.Abs(((2 * (realValue - estimatedValue) / (realValue + estimatedValue)) - 1) * 100);
                        }
                        else
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
            relativeError = relativeError / (configs.Count - skips);
            return error_sum / (configs.Count - skips);
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
        /// <param name="previous">The state of learning in the previous round.</param>
        /// <returns>True if we abort learning, false otherwise</returns>
        protected bool abortLearning(LearningRound current, LearningRound previous)
        {
            TimeSpan diff = DateTime.Now - this.startTime;
            if (MLsettings.outputRoundsToStdout)
            {
                //GlobalState.logInfo.logToStdout(current.round.ToString() + ";" + diff.ToString());
                GlobalState.logInfo.logToStdout(current.ToString());
            }
            if (current.round >= this.MLsettings.numberOfRounds)
            {
                current.terminationReason = "numberOfRounds";
                return true;
            }
            if (MLsettings.learnTimeLimit.Ticks > 0 && MLsettings.learnTimeLimit <= diff)
            {
                current.terminationReason = "learnTimeLimit";
                return true;
            }
            if (MLsettings.stopOnLongRound && current.round > 30 && diff.Minutes > 60)
            {
                current.terminationReason = "stopOnLongRound";
                return true;
            }
            if (abortDueError(current))
            {
                current.terminationReason = "abortError";
                return true;
            }
            
            //if (minimalRequiredImprovement(current) + current.validationError_relative > oldRoundRelativeError)
            if (MLsettings.minImprovementPerRound > current.bestCandidateScore)
            {
                if (this.MLsettings.withHierarchy)
                {
                    hierachyLevel++;
                    return false;
                } else
                {
                    current.terminationReason = "minImprovementPerRound";
                    return true;
                }
            }

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
                GlobalState.logError.logLine("Error: you need to specify a learning and validation set.");
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
                }
                catch (ArgumentException argEx)
                {
                    GlobalState.logError.logLine(argEx.Message);
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
                    GlobalState.logError.logLine(argEx.Message);
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
            if (feature.participatingBoolOptions.Count == 1 && feature.participatingNumOptions.Count == 0 && feature.participatingBoolOptions.Contains(this.infModel.Vm.Root))
            {
                for (int r = 0; r < this.learningSet.Count; r++)
                {
                    column[r] = 1;
                }
                this.DM_columns.GetOrAdd(feature, column);
                return;
            }

            int i = 0;
            foreach (Configuration config in this.learningSet)
            {
                if (feature.validConfig(config))
                {
                    column[i] = feature.eval(config);
                }
                i++;
            }
            this.DM_columns.GetOrAdd(feature, column);
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
                Feature temp = new Feature(subset.getPureString(), subset.getVariabilityModel());
                temp.Constant = subset.Constant;
                resultList.Add(temp);
            }
            return resultList;
        }

        /// <summary>
        /// This method creates a system array based on an ILNumerics array.
        /// </summary>
        /// <typeparam name="T">The type of the objects stored in the array.</typeparam>
        /// <param name="A">The ILNumerics array.</param>
        /// <returns>Returns the newly created system array contain the values of the ILNumerics array.</returns>
        public static System.Array toSystemMatrix<T>(ILInArray<T> A)
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


        /// <summary>
        /// The function computes the error of a given influence model for a list of configurations. As the error metric, it uses the given loss function
        /// </summary>
        /// <param name="influenceModel">The influence model containing the already learned influences.</param>
        /// <param name="configs">The configurations for which predictions should be made. The configurations must contain the measured/true value to compute the error.</param>
        /// <param name="loss">The loss functions used to compute the error.</param>
        /// <returns>The error of the model for the given configurations.</returns>
        public static double computeError(InfluenceModel influenceModel, List<Configuration> configs, ML_Settings.LossFunction loss)
        {
            double error_sum = 0;
            int skips = 0;
            List<Feature> currentModel = influenceModel.getListOfFeatures();
            foreach (Configuration c in configs)
            {
                double estimatedValue = estimate(currentModel, c);
                double realValue = 0;
                try
                {
                    if (!c.nfpValues.Keys.Contains(GlobalState.currentNFP))
                    {
                        skips++;
                        continue;
                    }
                    realValue = c.GetNFPValue(GlobalState.currentNFP);
                }
                catch (ArgumentException argEx)
                {
                    GlobalState.logError.logLine(argEx.Message);
                    realValue = c.GetNFPValue();
                }
                //How to handle near-zero values???
                //http://math.stackexchange.com/questions/677852/how-to-calculate-relative-error-when-true-value-is-zero
                //http://stats.stackexchange.com/questions/86708/how-to-calculate-relative-error-when-the-true-value-is-zero

                if (realValue < 1)
                {//((2(true-est) / true+est) - 1 ) * 100
                    //continue;
                    skips++;
                    continue;
                }

                double error = 0;
                switch (loss)
                {
                    case ML_Settings.LossFunction.RELATIVE:
                        if (realValue < 1)
                        {
                            error = Math.Abs(((2 * (realValue - estimatedValue) / (realValue + estimatedValue)) - 1) * 100);
                        }
                        else
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
            return error_sum / (configs.Count - skips);
        }
    }
}
