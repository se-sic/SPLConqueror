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
        ILArray<double> X, Y, Y2 = ILMath.empty();//ILMath.returnType<double>();
        public ILArray<double> constants;
        VariabilityModel vm;
        Dictionary<Element, int> featureVector = new Dictionary<Element, int>();
        List<int> featureSelection = new List<int>();
        Dictionary<List<Element>, double> measurements = new Dictionary<List<Element>, double>();
        Dictionary<List<Element>, double> evalSet = new Dictionary<List<Element>, double>();
        Dictionary<List<Element>, double> allMeasurements = new Dictionary<List<Element>, double>();
        Element baseFeature = null;

        //Learning and EvalSet with Numerical Features
        List<Configuration> learningSet = new List<Configuration>();
        List<Configuration> evaluationSet = new List<Configuration>();
        List<Configuration> completeEvaluationSet = new List<Configuration>();

        public List<ExpressionTree> influenceModel = new List<ExpressionTree>(); // in combination with constants

        double threshold = 4; //The error rate at which we abort our search (maybe in percent or in absolut)
        double minBenefitValue = 2; //The improvement in accuracy in percent that is needed to consider an element for prediction 
        Dictionary<List<Element>, double> bestFit = new Dictionary<List<Element>, double>();
        int nbComputations = 0;
        StreamWriter sw;
        public FeatureSubsetSelection(VariabilityModel vm)
        {
            bool baseFeatureExist = false;
            this.vm = vm;
            int i = 0;
            
            foreach (Element elem in vm.getFeatures())
            {
                //we add only a single option as a base feature: The rational is that multiple mandatory options introduce ambiguity
                if (elem.isMandatory() && elem.getParent() == -1)
                {
                    if(!baseFeatureExist) {
                        baseFeatureExist = true;
                        this.baseFeature = elem;
                    }
                    continue;
                }
                // we do not add options as candidates that have mandatory child options: the rational is that these features cannot be distinguished from the child options and introduce only ambiguity
                List<Element> childs = elem.getChildElems();
                if (childs.Count == 0)
                {
                    featureVector.Add(elem, i);
                    i++;
                }
                else
                {
             /*       foreach (var child in childs)
                    {
                        if (child.isOptional() == false || child.getCommulativeIDs().Count > 0 ||child.getAlternativeIDs().Count > 0)
                            goto Options;
                    }*/
                    featureVector.Add(elem, i);
                    i++;
                }
                Options:{}
            }
            //There is no base feature. Hence, add it to determine constant values not depending on an input feature
            if (!baseFeatureExist)
            {
                Element baseF = new Element("base",vm.createID(),vm);
                baseF.setOptional(false);
                vm.addElement(baseF);
                this.baseFeature = baseF;
              //  featureVector.Add(baseF, i);
              //  i++;
            }

            foreach (NumericOption vf in vm.VariableFeatures)
            {
                featureVector.Add(vf,i);
                i++;
            }
         //   sw = new StreamWriter("data.csv", true);
          //  sw.WriteLine("================================");
        }

        //a workaround
        ILArray<double> constantsTemp;

        //Converts a List of configurations with its measured value into a validation matrix and a learning matrix
        public void setMeasurementsWithNumericFeatures(List<Configuration> measurements)
        {
            double nbMeasurements = measurements.Count;
            nbMeasurements = nbMeasurements * 0.7;
            int nbM = Convert.ToInt32(nbMeasurements);
            double[] temparryLearn;//measured value

            if (ML_Settings.crossValidation == false)
            {
              //  this.learningSet = measurements;
               // this.evaluationSet = measurements;
                temparryLearn = new double[measurements.Count];
                nbM = measurements.Count;
                //sw.WriteLine("Size learning set: " + measurements.Count + "Size evaluation set: " + measurements.Count);
            }
            else
            {
                temparryLearn = new double[nbM];
               // sw.WriteLine("Size learning set: " + nbM + "Size evaluation set: " + (measurements.Count - nbM));
            }
            int pos = 0;
            foreach (Configuration config in measurements)
            {
                if (pos < nbM)
                {
                    this.learningSet.Add(config);//add configuration
                    temparryLearn[pos] = config.getCurrentValue();//add measured value
                    if (MLsettings.crossValidation == false)
                        this.evaluationSet.Add(config);
                }
                else
                {
                    this.evaluationSet.Add(config);
                    if (MLsettings.crossValidation == false)
                        temparryLearn[pos] = config.getCurrentValue();
                }
                pos++;
            }

            Y2 = temparryLearn;
            Y2 = Y2.T;
        }

        //Used for validating against the whole population
        public void setValidationSet(List<Configuration> measurements)
        {
            double[] temparryLearn = new double[measurements.Count];
            int pos = 0;
            foreach (Configuration config in measurements)
            {
                temparryLearn[pos] = config.getCurrentValue();
                this.completeEvaluationSet.Add(config);
            }
            Y = temparryLearn;
            Y = Y.T;
        }

        //Converts a List of configurations with its measured value into a validation matrix and a learning matrix
        public void setMeasurements(Dictionary<List<Element>, double> measurements)
        {
            double nbMeasurements = measurements.Keys.Count;
            nbMeasurements = nbMeasurements * 0.7;
            int nbM = Convert.ToInt32(nbMeasurements);
            double[] temparryLearn;
            double[] temparryValidate;
            if (MLsettings.crossValidation == false)
            {
                temparryLearn = new double[measurements.Keys.Count];
                temparryValidate = new double[measurements.Keys.Count];
                sw.WriteLine("Size learning set: " + measurements.Keys.Count + "Size evaluation set: " + measurements.Keys.Count);
            }
            else
            {
            temparryLearn= new double[nbM];
            temparryValidate = new double[measurements.Keys.Count - nbM];
            sw.WriteLine("Size learning set: " + nbM + "Size evaluation set: " + (measurements.Keys.Count - nbM));
            }
                int pos = 1;
            foreach (List<Element> config in measurements.Keys)
            {
                if (MLsettings.crossValidation == false)
                {
                    this.measurements.Add(config, measurements[config]);
                    this.evalSet.Add(config, measurements[config]);
                }
                else
                {
                    if (pos <= nbM)
                    {
                        this.measurements.Add(config, measurements[config]);
                    }
                    else
                    {
                        this.evalSet.Add(config, measurements[config]);
                    }
                    pos++;
                }
            }
            //measurements.Values.CopyTo(temparry, 0);
            this.evalSet.Values.CopyTo(temparryValidate, 0);
            Y = temparryValidate;
            Y = Y.T;
            this.measurements.Values.CopyTo(temparryLearn, 0);
            Y2 = temparryLearn;
            Y2 = Y2.T;
            //this.measurements = measurements;
        }

        // configuration -> model with good prediction, constants of the summands of the model, model, new Interactions since the last configuration was predecited with an accuracy less than measurement bias, time to find this model
       // Dictionary<Configuration, Tuple<List<ExpressionTree>, double[], List<ExpressionTree>, long, Tuple<double, double>>> configurationAndRelatedModel = new Dictionary<Configuration, Tuple<List<ExpressionTree>, double[], List<ExpressionTree>, long, Tuple<double, double>>>();

        static Dictionary<Configuration, InfluenceModelContainer> configurationAndRelatedModel = new Dictionary<Configuration, InfluenceModelContainer>();
        static Dictionary<Configuration, InfluenceModelContainer> configurationWithNoFittingModel = new Dictionary<Configuration, InfluenceModelContainer>();

        public double forwardFeatureSelectionWithFunctions()
        {
            if (this.learningSet.Count == 0)
                return -1;
            List<ExpressionTree> treesAddedInForwardStep = new List<ExpressionTree>();
            Process proceso = Process.GetCurrentProcess();
            double procesTimeIntermediat = 0;
            double lifeIntervalIntermediat = 0;
            Stopwatch time = new Stopwatch();
            time.Start();
        //    if (sw == null)
       //         sw = new StreamWriter("data.csv", true);
          //  sw.WriteLine("================================");
            this.bestFit.Clear();
            //Feature Set containing the configuration options with most predictive power
            List<ExpressionTree> featureSetI = new List<ExpressionTree>();//Jedes Element in der Liste, kann aus einer Menge an Features bestehen, um Interaktionen zu realisieren oder aus einem einzelnen Feature
            if (this.baseFeature != null)
            {
                ExpressionTree baseTree = new ExpressionTree(this.baseFeature.getName(), this.vm);
                treesAddedInForwardStep.Add(baseTree);
                featureSetI.Add(baseTree);
            }

            bool abort = false;
            int round = 1;
            double errorRoundBefore = Double.MaxValue;
            while (!abort)
            {
                ExpressionTree addedElementInForwardStep = null;
                double oldError = Double.MaxValue;//Fehler in dieser Runde
                List<ExpressionTree> oldBestCombination = null;//Beste bisher gefundenes FeatureSetI in dieser Runde
               
                foreach (Element feature in this.featureVector.Keys)
                {
                    if (feature == this.baseFeature)
                        continue;
                    ExpressionTree candidate = new ExpressionTree(feature.getName(),this.vm);
                    List < List <ExpressionTree>> testCombination = computeCombinations(featureSetI, candidate);

                    //add squared features to the candidate list
                    if (MLsettings.quadraticOptionAdding && feature is NumericOption)
                    {
                        ExpressionTree squaredCandidate = new ExpressionTree(feature.getName() + " * " + feature.getName(), this.vm);
                        testCombination.AddRange(computeCombinations(featureSetI, squaredCandidate));
                    }

                    if (MLsettings.useLog && feature is NumericOption)
                    {
                        testCombination.AddRange(computeCombinationsLog(featureSetI, candidate));
                    }


                    foreach (List<ExpressionTree> combination in testCombination)
                    {
                        double error = Math.Abs(computeErrorForCombination(combination, MLsettings.ErrorComputationStrategy.Avg));
                        if (error < oldError)
                        {
                            oldBestCombination = combination;
                            oldError = error;
                            constants = this.constantsTemp.ToArray();
                        }
                    }
                }
                Console.WriteLine("Round " + round + " : ended with avg. training error of  " + oldError + "  and a max training error of  " +
                    Math.Abs(computeErrorForCombination(oldBestCombination, MLsettings.ErrorComputationStrategy.Max)));
                //Console.WriteLine("Feature Set: " + printFeatureSet(oldBestCombination, constants));
                round++;
                if (oldError < errorRoundBefore)
                {
                    // compare old and new model to identiy the new part (the new part can be an existing expression tree with an additional feature)
                    addedElementInForwardStep = getDelta(featureSetI, oldBestCombination);
                    treesAddedInForwardStep.Add(addedElementInForwardStep.Copy());
                    featureSetI = copyCombination(oldBestCombination);
                    this.influenceModel = featureSetI;
                    if (MLsettings.storeOneModelForEachConfiguration)
                    {
                        configurationWithNoFittingModel = new Dictionary<Configuration, InfluenceModelContainer>();
                        System.TimeSpan lifeInterval = (DateTime.Now - proceso.StartTime);
                        lifeIntervalIntermediat += lifeInterval.TotalMilliseconds;
                        procesTimeIntermediat += proceso.TotalProcessorTime.TotalMilliseconds;
                        time.Stop();
                        bool oneFound = false;
                        foreach (Configuration c in this.evaluationSet)
                        {
                            if (!configurationAndRelatedModel.ContainsKey(c))
                            {
                                double prediction = predictValue(constants.ToArray(), featureSetI, c);
                                double realValue = c.getCurrentValue();

                                double currError = Math.Abs(100 - ((prediction * 100) / realValue));

                                InfluenceModelContainer imc = new InfluenceModelContainer();
                                imc.influenceModel = featureSetI.Copy();
                                imc.influenceModelFactors = constants.ToArray();
                                imc.newModelComponents = treesAddedInForwardStep.Copy();
                                imc.elapsedTime = time.ElapsedMilliseconds;
                                imc.time_processTime = procesTimeIntermediat;
                                imc.time_lifeTime = lifeIntervalIntermediat;
                                imc.errorWithInfluenceModel = currError;

                                if (currError < MLsettings.minRealtiveError)
                                {
                                    oneFound = true;
                                    configurationAndRelatedModel.Add(c, imc);
                                }
                                else
                                {
                                    configurationWithNoFittingModel.Add(c, imc);
                                }
                                
                            }

                        }
                        Console.WriteLine("Number Of Configs With Model: " + configurationAndRelatedModel.Count);
                        if (oneFound)
                        {
                            treesAddedInForwardStep = new List<ExpressionTree>();
                        }
                        if (configurationAndRelatedModel.Count == this.evaluationSet.Count)
                        {

                            printModelForeachConfig();
                            // stop leaning because nothing more to learn
                            Console.WriteLine("Learned a model for each configuration");
                            return -1.0;
                        }
                        proceso = Process.GetCurrentProcess();
                        time.Start();  
                     }
                }
                else
                {
                    break;
                }

                Console.WriteLine(printResult(featureSetI)+ "  ---> "+oldError);

                double benefit = Math.Abs(errorRoundBefore - oldError);
                
                bool removedFeature = false;
                if (oldError > 0.1 && MLsettings.backwardSelection && featureSetI.Count > 1) //backward feature selection... try to remove elements without sacrifycing accuracy
                {
                    double backwardBenefit = Double.MinValue;
                    //First copy the featureSetI to not overwrite our results
                    ExpressionTree[] shrinkedArray = new ExpressionTree[featureSetI.Count];
                    featureSetI.CopyTo(shrinkedArray);
                    List<ExpressionTree> shrinkedList = shrinkedArray.ToList();
                    double backWardOldError = Double.MaxValue;
                    double[] backWardConstants = null;
                    while (shrinkedList.Count > 1) // use benefit or use complete error rate? unclear
                    {
                        backWardOldError = Double.MaxValue;
                        List<ExpressionTree> backWardOldBestList = new List<ExpressionTree>();
                        

                        for (int i = 0; i < shrinkedList.Count; i++) 
                        {
                            if (shrinkedList[i].participatingBoolFeatures.Contains(this.baseFeature))
                                continue;
                            //copy the list to not remove all elements, but only a single
                            ExpressionTree[] candidateArray = new ExpressionTree[shrinkedList.Count];
                            shrinkedList.CopyTo(candidateArray);
                            List<ExpressionTree> candidateList = candidateArray.ToList();
                            if (candidateList[i] == addedElementInForwardStep)
                                continue;// for optimization and to avoid endless remove and adding: do not remove the element that was added 
                            ExpressionTree candidateToBeRemoved = candidateList[i];
                            if (candidateToBeRemoved.ToString().Equals(addedElementInForwardStep.ToString()))
                                continue;
                            candidateList.RemoveAt(i);
                            double error = Math.Abs(computeErrorForCombination(candidateList, MLsettings.errorAbortion));
                            if (error < backWardOldError)
                            {
                                backWardOldBestList = candidateList;
                                backWardOldError = error;
                                backWardConstants = this.constantsTemp.ToArray();
                            }
                        }
                        //backwardBenefit = Math.Abs(errorRoundBefore - backWardOldError);
                        if (backWardOldError < oldError * MLsettings.backwardFeatureRemovalThreshold || (backWardOldError < oldError + 0.1))
                        {
                            Console.WriteLine("Backward ended with training error of " + backWardOldError);
                            shrinkedList = copyCombination(backWardOldBestList);
                            this.constants = backWardConstants;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (featureSetI.Count > shrinkedList.Count)
                    {
                        featureSetI = copyCombination(shrinkedList);
                        errorRoundBefore = backWardOldError;
                        removedFeature = true;
                    }
                }
               
                bool backward = (MLsettings.backwardSelection && !removedFeature);
                bool benefitTooSmall = (MLsettings.relativeError && (benefit < MLsettings.benefitAbortThreshold));

                //bool maxRound = round > (Math.Max(MLsettings.maxNumberOfSelectionRounds, this.fm.getFeatures().Count ^ 2));
                bool maxRound = featureSetI.Count > MLsettings.maxNumberOfSelectionRounds;


                bool abortionByError = false;

                if (MLsettings.useRealtiveErrorForAbortion)
                {
                    //double errorForAbortion = Math.Abs(computeErrorForCombination(featureSetI, MLsettings.errorAbortion));
                    abortionByError = (benefit < MLsettings.benefitAbortThreshold);//((errorForAbortion - MLsettings.minRealtiveError) <= MLsettings.benefitAbortThreshold);
                }
                if (backward && (benefitTooSmall || (!MLsettings.relativeError && oldError == 0)) || maxRound || oldError == 0)
                    abort = true;
                if (abortionByError)
                { // new condition for learning the model with all configurations
                    abort = true;
                    Console.WriteLine("Abortion because of measurements bias error");
                }

                // todo: stop the computation if MLSettings.errorAbortion == Avg and the average Error is less than the average Error 

                if (MLsettings.minRealtiveError > oldError && MLsettings.errorAbortion == MLsettings.ErrorComputationStrategy.Avg)
                    abort = true;

                errorRoundBefore = oldError;
                
           //     Console.WriteLine("Computing Error for all configurations...");
           //     double completeError = computeError(constants.ToArray<double>(), featureSetI, relativeError, true);
           //     Console.WriteLine("Error: " + completeError);
            }
            Console.WriteLine("No further improvement found. Abort.");
            double completeError = 0;
            this.influenceModel = copyCombination(featureSetI);
            if (completeEvaluationSet.Count > 0)
            {
               // Console.WriteLine("Computing Error for all configurations...");
               // completeError = computeError(constants.ToArray<double>(), featureSetI, false, true);
               // Console.WriteLine("Absolut error: " + completeError);
                /*List<List<Element>> featureSetI_cleared = new List<List<Element>>();
                List<double> clearedConstants = new List<double>();
                for (int i = 0; i < constants.Length; i++)
                {
                    if (Math.Abs(constants.GetValue(i)) < MLsettings.removeOptionWithInfluenceLessThanTreshold)
                        continue;
                    featureSetI_cleared.Add(featureSetI[i]);
                    clearedConstants.Add(constants.GetValue(i));
                }
                completeError = computeError(clearedConstants.ToArray(), featureSetI_cleared, true, true);*/
                if(constants.Length > featureSetI.Count){
                    double[] temp = new double[featureSetI.Count];
                    for (int i = 1; i < featureSetI.Count; i++)
                        constantsTemp[i - 1] = this.constants[i];
                    this.constants = constantsTemp;
                }
                this.influenceModel = featureSetI;
                completeError = computeError(constants.ToArray<double>(), featureSetI, true, MLsettings.ErrorComputationStrategy.Avg);
                //Console.WriteLine("Relative error: " + completeError);
            }
            printModelForeachConfig();
            return completeError;
        }

        public static void printModelForeachConfig()
        {
            printModelForeachConfig(configurationAndRelatedModel);
            printModelForeachConfig(configurationWithNoFittingModel);

        }

        private static void printModelForeachConfig(Dictionary<Configuration, InfluenceModelContainer> set)
        {

            // output of all configurations
            foreach (KeyValuePair<Configuration, InfluenceModelContainer> entry in set)
            {
                Console.WriteLine("Config: " + entry.Key.ToString());
                List<ExpressionTree> newInteractions = new List<ExpressionTree>();
                List<double> constantsNewInteractions = new List<double>();
                List<ExpressionTree> partialInteractions = new List<ExpressionTree>();
                List<double> constantsPartialInteractions = new List<double>();

                for (int i = 0; i < entry.Value.newModelComponents.Count; i++)
                {
                    bool isFullInteraction = false;
                    for (int j = 0; j < entry.Value.influenceModel.Count; j++)
                    {
                        if (entry.Value.influenceModel[j].Equals(entry.Value.newModelComponents[i]))
                        {
                            isFullInteraction = true;
                            newInteractions.Add(entry.Value.influenceModel[j]);
                            constantsNewInteractions.Add(entry.Value.influenceModelFactors[j]);
                        }
                    }
                    if (!isFullInteraction)
                    {
                        partialInteractions.Add(entry.Value.newModelComponents[i]);
                        constantsPartialInteractions.Add(0.0);

                    }
                }

                Console.WriteLine("NewInterations: " + getModel2(newInteractions, constantsNewInteractions.ToArray()));
                Console.WriteLine("NewPartialInterations: " + getModel2(partialInteractions, constantsPartialInteractions.ToArray()));
                Console.WriteLine("Error: " + entry.Value.errorWithInfluenceModel);
                Console.WriteLine("Model: " + getModel2(entry.Value.influenceModel, entry.Value.influenceModelFactors));
                Console.WriteLine("Time: " + entry.Value.elapsedTime + "  ");
                Console.WriteLine("CPULoad: " + (entry.Value.time_processTime / entry.Value.time_lifeTime) * 100 + "  "); // http://www.thecoldsun.com/en/content/06-2008/get-process-and-thread-cpu-time-and-load#toc3
            }
            set = new Dictionary<Configuration, InfluenceModelContainer>();
        }

        private ExpressionTree getDelta(List<ExpressionTree> oldModel, List<ExpressionTree> newModel)
        {
            foreach (ExpressionTree newPart in newModel)
            {
                if (!oldModel.Contains(newPart))
                    return newPart;
            }
            return null;
        }

        private string printResult(List<ExpressionTree> featureSetI)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ExpressionTree exp in featureSetI)
            {
                sb.Append(" {" + exp.ToString() + "} ");

            }
            return sb.ToString();
        }


        private List<ExpressionTree> copyCombination(List<ExpressionTree> oldBestCombination)
        {
            List<ExpressionTree> resultList = new List<ExpressionTree>();
            if (oldBestCombination == null)
                return resultList;
            foreach (ExpressionTree subset in oldBestCombination)
            {
                resultList.Add(new ExpressionTree(subset.ToString(),subset.getFM()));
            }
            return resultList;
        }

        private string printFeatureSet(List<ExpressionTree> oldBestCombination, double[] constants)
        {
            if (oldBestCombination == null)
                return "{}";
            StringBuilder result = new StringBuilder();
            result.Append("{ ");
            for (int i = 0; i < oldBestCombination.Count; i++)
            {
                result.Append("{ ");
                result.Append(oldBestCombination[i].ToString());
                result.Append(" } -> " + constants[i]+ ", ");
            }
            result.Append(" }");
            return result.ToString();
        }

        //Delegates the evaluation process: generating the in
        /** 
         * <para>maxPredictionError = if false -> the average prediction error is computed, if true -> the highest prediction error is returned</para> 
         */
        private double computeErrorForCombination(List<ExpressionTree> combination, MLsettings.ErrorComputationStrategy errorlearning)
        {
            ILArray<double> DM = transformDataSetAccordingToFeatureSetI(combination);
            if (DM.Size.NumberOfElements == 0)
                return Double.MaxValue;
            ILArray<double> DMT = DM.T;
            ILArray<double> temparray = null;
            try
            {
                temparray = ILMath.pinv(DMT);
            }
            catch (Exception e)
            {
                double[,] fixSVDwithACCORD;
                var exp = ToSystemMatrix<double>(DM.T);
                fixSVDwithACCORD = (double[,])exp;
                fixSVDwithACCORD = fixSVDwithACCORD.PseudoInverse();
                temparray = fixSVDwithACCORD;
            }

            ILArray<double> constants;
            if (temparray.IsEmpty)
                constants = ILMath.multiply(DM, Y2.T);
            else
                constants = ILMath.multiply(temparray, Y2.T);
            //ILArray<double> constants22 = ILMath.multiply(ILMath.pinv(ILMath.multiply(DM.T, DM)), ILMath.multiply(DM.T, Y));
            this.constantsTemp = constants;
            return computeError(constants.ToArray<double>(), combination, false, errorlearning);
        }


        /** 
         * <para>maxPredictionError = if false -> the average prediction error is computed, if true -> the highest prediction error is returned</para> 
         */
        private double computeError(double[] constants, List<ExpressionTree> combination, bool completeEvaluation, MLsettings.ErrorComputationStrategy errorlearning)
        {
            StreamWriter sw = null;
            if (completeEvaluation)
                sw = new StreamWriter("relativeError.csv");
            double error = 0;
            if (errorlearning.Equals(MLsettings.ErrorComputationStrategy.Min))
            {
                error = Double.MaxValue;
            }
            List<Configuration> evalSet;
            if (completeEvaluation)
                evalSet = this.completeEvaluationSet;
            else
                evalSet = this.evaluationSet;
            foreach (Configuration config in evalSet)
            {
                double prediction = predictValue(constants, combination, config);
                double realValue = config.getCurrentValue();
                double currentError = 0;
                if (MLsettings.relativeError)
                {
                    if (realValue != 0)
                    {
                        currentError = Math.Abs(100 - ((prediction * 100) / realValue));
                    }
                }
                else
                {
                    currentError += Math.Pow(realValue - prediction, 2);
                }
                if (errorlearning.Equals(MLsettings.ErrorComputationStrategy.Max))
                {
                    error = Math.Max(error, currentError);
                }
                else if (errorlearning.Equals(MLsettings.ErrorComputationStrategy.Min))
                {
                    error = Math.Min(error, currentError);
                }
                else
                {
                    error += Math.Round(currentError, 3);
                }
                if (completeEvaluation)
                    sw.WriteLine(config.ToString() + ";" + prediction + ";" + realValue + ";" + currentError);
            }

            if (completeEvaluation)
                sw.Close();

            if (errorlearning.Equals(MLsettings.ErrorComputationStrategy.Max))
            {
                return error;
            }
            else if (errorlearning.Equals(MLsettings.ErrorComputationStrategy.Min))
            {
                return error;
            }
            else
            {
                return error / evalSet.Count;
            }
        }

       


        private double predictValue(double[] constants, List<ExpressionTree> expression, Configuration config)
        {
            double prediction = 0;
            for (int i = 0; i < expression.Count; i++)
            {
                ////First check whether the current feature or combination of feature is present in the current configuration
                if (expression[i].validConfig(config))
                {
                    prediction += expression[i].eval(config) * constants[i];
                }
            }
            return prediction;
        }

        //Computing the crossproduct of featureSetI and featureToAdd; featureToAdd can be a single element or a combination e.g., x or x*x 
        private List<List<ExpressionTree>> computeCombinations(List<ExpressionTree> featureSetI, ExpressionTree featureToAdd)
        {
            List<List<ExpressionTree>> result = new List<List<ExpressionTree>>();
            
           
           // Now add feature within each List of the featureSetI
            for (int i = 0; i < featureSetI.Count; i++)
            {
                if (featureSetI[i].containsFeature(this.baseFeature))
                    continue;// we do not want interactions with the static base element (i.e., c0)
                List<ExpressionTree> combination = new List<ExpressionTree>();

                //We do not want interactions with our parent
                if (featureSetI[i].containsParent(featureToAdd))
                    continue;

                //Add the new interaction to the current configuration
                ExpressionTree subset = new ExpressionTree(featureSetI[i].ToString() + " * " + featureToAdd.ToString(), vm);

                if ( !MLsettings.allowOverfitting && ((subset.getNumberOfParticipatingFeatures() > subset.getNumberOfDistinctParticipatingFeatures() + 4) || subset.getNumberOfParticipatingFeatures() > 6 || subset.getNumberOfDistinctParticipatingFeatures() > 3)) // avoid overfitting: allowing interactions of only 3 features, only single terms not bigger than 5
                    continue;
                combination.Add(subset);

                //add the remaining feature sets to the configuration
                for (int k = 0; k < featureSetI.Count; k++)
                {
                    combination.Add(featureSetI[k]);
                }

                //add the configuration to the result set
                result.Add(combination);
            }


            //Make sure that always individual features can be added
            if (MLsettings.inidividualOptionAdding || featureSetI.Count == 0)
            {
                //verify of featureSetI has not already the single "feature" as an element
                foreach (ExpressionTree element in featureSetI)
                {
                    if (element.ToString().Equals(featureToAdd.ToString()))
                        return result;//already in the set, we can return
                }
                List<ExpressionTree> firstConfiguration = new List<ExpressionTree>();
                firstConfiguration.AddRange(featureSetI);
                firstConfiguration.Add(featureToAdd);
                result.Add(firstConfiguration);
            }
                return result;
        }


        //Computing the crossproduct of featureSetI and featureToAdd; featureToAdd can be a single element or a combination e.g., x or x*x 
        private List<List<ExpressionTree>> computeCombinationsLog(List<ExpressionTree> featureSetI, ExpressionTree featureToAdd)
        {
            List<List<ExpressionTree>> result = new List<List<ExpressionTree>>();

            ExpressionTree subset = new ExpressionTree("log10(" + featureToAdd.ToString() + ")", vm);

            // Now add feature within each List of the featureSetI
            for (int i = 0; i < featureSetI.Count; i++)
            {
                //for (int l = 0; l < newFeatureToAddVersions.Count; l++)
                //{

                if (featureSetI[i].containsFeature(this.baseFeature))
                    continue;// we do not want interactions with the static base element (i.e., c0)
                    List<ExpressionTree> combination = new List<ExpressionTree>();


                    if ((subset.getNumberOfParticipatingFeatures() > subset.getNumberOfDistinctParticipatingFeatures() + 4) || subset.getNumberOfParticipatingFeatures() > 6 || subset.getNumberOfDistinctParticipatingFeatures() > 3) // avoid overfitting: allowing interactions of only 3 features, only single terms not bigger than 5
                        continue;
                    combination.Add(new ExpressionTree(subset.ToString()+" * "+featureSetI[i].ToString(),vm));



                    //add the remaining feature sets to the configuration
                    for (int k = 0; k < featureSetI.Count; k++)
                    {
                        combination.Add(featureSetI[k]);
                    }

                    //add the configuration to the result set
                    result.Add(combination);
                //}
            }

            

            //Make sure that always individual features can be added
            if (MLsettings.inidividualOptionAdding || featureSetI.Count == 0)
            {

                List<ExpressionTree> combination2 = new List<ExpressionTree>();
                combination2.Add(subset);
                //add the remaining feature sets to the configuration
                for (int k = 0; k < featureSetI.Count; k++)
                {
                    combination2.Add(featureSetI[k]);
                }
                result.Add(combination2); 

            }
            return result;
        }


        //Returns the number of distinct features in the list
        private int distinctFeatures(List<Element> subset)
        {
            Dictionary<Element, int> featureMap = new Dictionary<Element, int>();
            foreach (Element feature in subset)
            {
                if (featureMap.Keys.Contains(feature))
                    featureMap[feature]++;
                else
                    featureMap.Add(feature, 1);
            }
            return featureMap.Keys.Count;
        }


        private double predictErrorForAll(ILArray<double> currentConstants, List<List<Element>> featureSetI)
        {
            double[] constants = currentConstants.ToArray();
            double error = 0;
            foreach (List<Element> config in this.allMeasurements.Keys)
            {
                double predictedValue = 0;
                for (int i = 0; i < featureSetI.Count; i++)
                {

                    bool allFeaturesInConfig = true;
                    foreach (Element feature in featureSetI[i])
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig)
                    {
                        predictedValue += constants[i];
                    }
                }
                double differenceAbsolut = this.allMeasurements[config] - predictedValue;
                //if (predictedValue != 0)
                //    error += Math.Abs((this.allMeasurements[config] * 100 / predictedValue) - 100);
                //else
                //    error += 100;
                
                error += Math.Abs(100 - ((predictedValue * 100) / this.allMeasurements[config]));
            }
            double evalError = 0;
            foreach (List<Element> config in this.evalSet.Keys)
            {
                double predictedValue = 0;
                for (int i = 0; i < featureSetI.Count; i++)
                {

                    bool allFeaturesInConfig = true;
                    foreach (Element feature in featureSetI[i])
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig)
                    {
                        predictedValue += constants[i];
                    }
                }
                evalError += Math.Abs(100 - ((predictedValue * 100) / this.allMeasurements[config]));
            }
            Console.WriteLine("Error for population: " + (error / this.allMeasurements.Keys.Count) + " vs. error of learning set: " + (evalError / this.evalSet.Keys.Count));
            sw.WriteLine((error / this.allMeasurements.Keys.Count) + ";" + evalError / this.evalSet.Keys.Count);
            return (error / this.allMeasurements.Keys.Count);
        }



        public void setAllMeasurements(Dictionary<List<Element>, double> measurements)
        {
            this.allMeasurements = measurements;
        }

        private double computeErrorForNewCandidates(List<List<Element>> featureSetI, List<Element> newCanditates)
        {
            nbComputations++;
            //Console.WriteLine(nbComputations);
            ILArray<double> DM = transformDataSetAccordingToFeatureSetI(featureSetI, newCanditates);
            ILArray<double> temparray = ILMath.pinv(DM.T);
            ILArray<double> constants;
            if (temparray.IsEmpty)
                constants = ILMath.multiply(DM, Y2.T);
            else
                constants = ILMath.multiply(temparray, Y2.T);
            //ILArray<double> constants22 = ILMath.multiply(ILMath.pinv(ILMath.multiply(DM.T, DM)), ILMath.multiply(DM.T, Y));
            this.constantsTemp = constants;
            return computeError(constants.ToArray<double>(), featureSetI, newCanditates, false);
        }

        private double computeError(double[] constants, List<List<Element>> featureSetI, List<Element> newFeature, bool relative)
        {
            double error = 0;
            foreach (List<Element> config in this.evalSet.Keys)
            {
                double predictedValue = 0;
                for (int i = 0; i < featureSetI.Count; i++)
                {

                    bool allFeaturesInConfig = true;
                    foreach (Element feature in featureSetI[i])
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig)
                    {
                        predictedValue += constants[i];
                    }
                }
                bool allFeaturesInConfig2 = true;
                foreach (Element feature in newFeature)
                {
                    if (!config.Contains(feature))
                    {
                        allFeaturesInConfig2 = false;
                        break;
                    }
                }
                if (allFeaturesInConfig2)
                {
                    predictedValue += constants[featureSetI.Count];
                }
                if (!relative)
                    error += Math.Pow(Math.Abs(predictedValue - this.evalSet[config]), 2);
                else//Math.Abs(((result * 100 / estimate) - 100))
                    error += Math.Abs((this.evalSet[config] * 100 / predictedValue) - 100);
            }
            if (!relative)
                return error;
            else
                return error / this.evalSet.Keys.Count;
        }

        private ILArray<double> transformDataSetAccordingToFeatureSetI(List<ExpressionTree> expressions)
        {
            ILArray<double> DM = ILMath.zeros(expressions.Count, this.learningSet.Count);

            int m = 0;
            foreach (Configuration config in this.learningSet)
            {//Row in DM

                for (int i = 0; i < expressions.Count; ++i)
                {

                    //First check whether the current feature or combination of feature is present in the current configuration
                    if (expressions[i].validConfig(config))
                        DM[i, m] = expressions[i].eval(config);
                }
                ++m;
            }

            return DM;
        }

        //Config == Eine Messung des Trainingsets; list == Element der Lösungsmenge des FeatureSetI (i.e., ein Feature bzw. Featureinteraktion (daher Liste von Elementen)) 
        private double getValueForFeature(Configuration config, List<Element> list)
        {
            double value = 1;
            foreach (Element e in list)
            {
                if (e is NumericOption)
                    if (MLsettings.normalize)
                        value = value * config.numericOptions[(NumericOption)e];
                    else
                        value = value * ((NumericOption)e).deNormalize(config.numericOptions[(NumericOption)e]);
                else if (e == this.baseFeature)
                    return 1;
                else if (!config.features.Contains(e))//Dieser Fall sollte eigentlich nicht auftreten
                    return 0;
            }
            return value;
        }

        //Checks whether all features in the checklist are in the configuration
        private bool allFeaturesInConfig(Configuration config, List<Element> checkList)
        {
            foreach (Element e in checkList)
            {
                if (e is NumericOption)
                    continue;
                if (e == this.baseFeature)
                    continue;
                if (!config.features.Contains(e))
                    return false;
            }
            return true;
        }

        private static System.Array ToSystemMatrix<T>(ILInArray<T> A)
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
                Buffer.BlockCopy(workArr, 0, ret, 0, sizeof(double) * A.S.NumberOfElements);
                return ret;
            }
        }

        //The method builds the matrix X according to the current feature set I
        //Example: I= f1 f2 f1#f2 -> X must be length 3 and for the following configurations the following values
        //f1 -> 1 0 0
        //f2 -> 0 1 0
        //f1 f2>1 1 1
        //Extension to numerical features: n1#n1 -> n1^2 
        private ILArray<double> transformDataSetAccordingToFeatureSetI(List<List<Element>> featureSetI, List<Element> newFeature)
        {
            ILArray<double> DM;
            if(newFeature == null)
                DM = ILMath.zeros(featureSetI.Count, this.measurements.Keys.Count);
            else
                DM = ILMath.zeros(featureSetI.Count + 1, this.measurements.Keys.Count);
            int m = 0;
            foreach (List<Element> config in this.measurements.Keys)
            {//Row in DM

                for (int i = 0; i < featureSetI.Count; i++) //List<Element> currentFeatureInI in featureSetI)
                {
                    bool allFeaturesInConfig = true;
                    foreach (Element feature in featureSetI[i])
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig)
                    {
                        DM[i, m] = 1;
                    }
                   // else
                   // {
                   //     DM[i,m] = 0;
                   // }
                }

                if (newFeature != null)
                {
                    bool allFeaturesInConfig2 = true;
                    foreach (Element feature in newFeature)
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig2 = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig2)
                    {
                        DM[featureSetI.Count, m] = 1;
                    }
                 //   else
                 //   {
                 //       DM[featureSetI.Count, m] = 0;
                 //   }
                }
                m++;
            }

            return DM;
        }

        public Dictionary<List<Element>, double> getConstants()
        {
            return this.bestFit;
        }

        public string getModel()
        {
            return printFeatureSet(this.influenceModel, this.constants.ToArray());
        }

        public static string getModel2(List<ExpressionTree> influenceModel, ILArray<double> constants)
        {
            if (influenceModel.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < influenceModel.Count; i++)
            {
                sb.Append(constants.GetValue(i).ToString()+ " * ");
                sb.Append(influenceModel[i].ToString());
                sb.Append(" + ");
                
            }
            sb.Remove(sb.Length - 3, 3);
            return sb.ToString().Trim();
        }
    }
}
