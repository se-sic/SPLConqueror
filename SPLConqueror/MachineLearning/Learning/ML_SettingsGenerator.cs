using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using static MachineLearning.Learning.ML_Settings;

namespace MachineLearning.Learning
{
    public class ML_SettingsGenerator
    {
        private static Tuple<string, string[]> extractSettings(string parameter)
        {
            parameter = parameter.Trim();
            string[] idAndValues = parameter.Split(new char[] { '=' });
            string[] values = idAndValues[1].Substring(1, idAndValues[1].Length - 2).Split(new char[] { ',' });
            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = values[i].Trim();
            }
            return Tuple.Create(idAndValues[0].Trim(), values);
        }

        private static bool isBool(string toTest)
        {
            return toTest.ToLower().Equals("true") || toTest.ToLower().Equals("false");
        }

        //case insensitive conversion of a string to an boolean
        private static bool toBool(string toConv)
        {
            return toConv.ToLower().Equals("true");
        }

        private static bool isLossFunction(string toTest)
        {
            return toTest.Equals("RELATIVE") || toTest.Equals("LEASTSQUARES") || toTest.Equals("ABSOLUTE");
        }

        private static LossFunction toLossFunction(string toConvert)
        {
            if (toConvert.Equals("RELATIVE"))
            {
                return LossFunction.RELATIVE;
            }
            else if (toConvert.Equals("LEASTSQUARES"))
            {
                return LossFunction.LEASTSQUARES;
            }
            else if (toConvert.Equals("ABSOLUTE"))
            {
                return LossFunction.ABSOLUTE;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private static bool isScoreMeasure(string toTest)
        {
            return toTest.Equals("RELERROR") || toTest.Equals("INFLUENCE");
        }

        private static ScoreMeasure toScoreMeasure(string toConvert)
        {
            if (toConvert.Equals("RELERROR"))
            {
                return ScoreMeasure.RELERROR;
            }
            else if (toConvert.Equals("INFLUENCE"))
            {
                return ScoreMeasure.INFLUENCE;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private static void defineParameterSpace(string[] parameters, List<Dictionary<string, List<bool>>> boolSettings,
            List<Dictionary<string, List<int>>> intSettings, List<Dictionary<string, List<double>>> doubleSettings,
            Dictionary<string, List<LossFunction>> lossFuncInterval, Dictionary<string, List<ScoreMeasure>> scoreMeasureInterval,
            Dictionary<string, List<TimeSpan>> learnTimeLimitInterval)
        {
            //update each set of paramters of the user input
            foreach (string parameter in parameters)
            {
                int y;
                double x;
                TimeSpan z;
                Tuple<string, string[]> nameAndValues = extractSettings(parameter);
                if (isBool(nameAndValues.Item2[0]))
                {
                    foreach (Dictionary<string, List<bool>> boolSetting in boolSettings)
                    {
                        if (boolSetting.First().Key.Equals(nameAndValues.Item1))
                        {
                            List<bool> toAdd = new List<bool>();
                            foreach (string value in nameAndValues.Item2)
                            {
                                toAdd.Add(toBool(value));
                            }
                            boolSetting[nameAndValues.Item1] = toAdd;
                        }
                    }
                }
                else if (Int32.TryParse(nameAndValues.Item2[0], out y))
                {
                    foreach (Dictionary<string, List<int>> intSetting in intSettings)
                    {
                        if (intSetting.First().Key.Equals(nameAndValues.Item1))
                        {
                            List<int> toAdd = new List<int>();
                            foreach (string value in nameAndValues.Item2)
                            {
                                toAdd.Add(Int32.Parse(value));
                            }
                            intSetting[nameAndValues.Item1] = toAdd;
                        }
                    }
                }
                else if (Double.TryParse(nameAndValues.Item2[0], out x))
                {
                    foreach (Dictionary<string, List<double>> doubleSetting in doubleSettings)
                    {
                        if (doubleSetting.First().Key.Equals(nameAndValues.Item1))
                        {
                            List<double> toAdd = new List<double>();
                            foreach (string value in nameAndValues.Item2)
                            {
                                toAdd.Add(Double.Parse(value, CultureInfo.InvariantCulture));
                            }
                            doubleSetting[nameAndValues.Item1] = toAdd;
                        }
                    }
                }
                else if (isLossFunction(nameAndValues.Item2[0]))
                {
                    List<LossFunction> toAdd = new List<LossFunction>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(toLossFunction(value));
                    }
                    lossFuncInterval[nameAndValues.Item1] = toAdd;
                }
                else if (isScoreMeasure(nameAndValues.Item2[0]))
                {
                    List<ScoreMeasure> toAdd = new List<ScoreMeasure>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(toScoreMeasure(value));
                    }
                    scoreMeasureInterval[nameAndValues.Item1] = toAdd;
                }
                else if (TimeSpan.TryParse(nameAndValues.Item2[0], out z))
                {
                    List<TimeSpan> toAdd = new List<TimeSpan>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(TimeSpan.Parse(value));
                    }
                    learnTimeLimitInterval[nameAndValues.Item1] = toAdd;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// Method that generates all ML_Settings object within a user defined parameter space
        /// </summary>
        /// <param name="parameters">Definition of the parameter space in String format</param>
        /// <returns>List of all ML_Settings within the parameter space</returns>
        public static List<ML_Settings> generateSettings(string[] parameters)
        {
            //initialize the parameter space
            Dictionary<string, List<LossFunction>> lossFuncInterval = new Dictionary<string, List<LossFunction>>();
            lossFuncInterval.Add("lossFunction", new List<LossFunction> { LossFunction.RELATIVE });
            Dictionary<string, List<bool>> parallelInterval = new Dictionary<string, List<bool>>();
            parallelInterval.Add("parallelization", new List<bool> { true });
            Dictionary<string, List<bool>> baggingInterval = new Dictionary<string, List<bool>>();
            baggingInterval.Add("bagging", new List<bool> { true });
            Dictionary<string, List<int>> baggingNumbersInterval = new Dictionary<string, List<int>>();
            baggingNumbersInterval.Add("baggingNumbers", new List<int> { 3 });
            Dictionary<string, List<int>> baggingTestDataFractionInterval = new Dictionary<string, List<int>>();
            baggingTestDataFractionInterval.Add("baggingTestDataFraction", new List<int> { 50 });
            Dictionary<string, List<bool>> useBackwardInterval = new Dictionary<string, List<bool>>();
            useBackwardInterval.Add("useBackward", new List<bool> { false });
            Dictionary<string, List<double>> abortErrorInterval = new Dictionary<string, List<double>>();
            abortErrorInterval.Add("abortError", new List<double> { 1.0 });
            Dictionary<string, List<bool>> limitFeatureSizeInterval = new Dictionary<string, List<bool>>();
            limitFeatureSizeInterval.Add("limitFeatureSize", new List<bool> { false });
            Dictionary<string, List<int>> featureSizeThresInterval = new Dictionary<string, List<int>>();
            featureSizeThresInterval.Add("featureSizeTreshold", new List<int> { 4 });
            Dictionary<string, List<bool>> quadFuncSupportInterval = new Dictionary<string, List<bool>>();
            quadFuncSupportInterval.Add("quadraticFunctionSupport", new List<bool> { true });
            Dictionary<string, List<bool>> crossValInterval = new Dictionary<string, List<bool>>();
            crossValInterval.Add("crossValidation", new List<bool> { false });
            Dictionary<string, List<int>> crossVal_kInterval = new Dictionary<string, List<int>>();
            crossVal_kInterval.Add("crossValidation_k", new List<int> { 5 });
            Dictionary<string, List<bool>> learn_logFuncInterval = new Dictionary<string, List<bool>>();
            learn_logFuncInterval.Add("learn_logFunction", new List<bool> { false });
            Dictionary<string, List<bool>> learn_asymFuncInterval = new Dictionary<string, List<bool>>();
            learn_asymFuncInterval.Add("learn_asymFunction", new List<bool> { false });
            Dictionary<string, List<bool>> learn_ratioFuncInterval = new Dictionary<string, List<bool>>();
            learn_ratioFuncInterval.Add("learn_ratioFunction", new List<bool> { false });
            Dictionary<string, List<bool>> learn_mirrowedFuncInterval = new Dictionary<string, List<bool>>();
            learn_mirrowedFuncInterval.Add("learn_mirrowedFunction", new List<bool> { false });
            Dictionary<string, List<int>> numberOfRoundsInterval = new Dictionary<string, List<int>>();
            numberOfRoundsInterval.Add("numberOfRounds", new List<int> { 70 });
            Dictionary<string, List<double>> backwardErrorDeltaInterval = new Dictionary<string, List<double>>();
            backwardErrorDeltaInterval.Add("backwardErrorDelta", new List<double> { 1 });
            Dictionary<string, List<double>> minImprovementInterval = new Dictionary<string, List<double>>();
            minImprovementInterval.Add("minImprovementPerRound", new List<double> { 0.1 });
            Dictionary<string, List<bool>> withHierachyInterval = new Dictionary<string, List<bool>>();
            withHierachyInterval.Add("withHierarchy", new List<bool> { true });
            Dictionary<string, List<bool>> bruteForceInterval = new Dictionary<string, List<bool>>();
            bruteForceInterval.Add("bruteForceCandidates", new List<bool> { false });
            Dictionary<string, List<bool>> ignoreBadFeaturesInterval = new Dictionary<string, List<bool>>();
            ignoreBadFeaturesInterval.Add("ignoreBadFeatures", new List<bool> { false });
            Dictionary<string, List<bool>> stopOnLongInterval = new Dictionary<string, List<bool>>();
            stopOnLongInterval.Add("stopOnLongRound", new List<bool> { true });
            Dictionary<string, List<bool>> candidateSizePenaltyInterval = new Dictionary<string, List<bool>>();
            candidateSizePenaltyInterval.Add("candidateSizePenalty", new List<bool> { true });
            Dictionary<string, List<TimeSpan>> learnTimeLimitInterval = new Dictionary<string, List<TimeSpan>>();
            learnTimeLimitInterval.Add("learnTimeLimit", new List<TimeSpan> { new TimeSpan(0) });
            Dictionary<string, List<ScoreMeasure>> scoreMeasureInterval = new Dictionary<string, List<ScoreMeasure>>();
            scoreMeasureInterval.Add("scoreMeasure", new List<ScoreMeasure> { ScoreMeasure.RELERROR });
            Dictionary<string, List<bool>> outputRoundsInterval = new Dictionary<string, List<bool>>();
            outputRoundsInterval.Add("outputRoundsToStdout", new List<bool> { false });

            //group the parameters sets by type
            List<Dictionary<string, List<bool>>> boolSettings = new List<Dictionary<string, List<bool>>> { parallelInterval, baggingInterval, useBackwardInterval, limitFeatureSizeInterval, quadFuncSupportInterval,
                crossValInterval, learn_logFuncInterval, learn_asymFuncInterval, learn_ratioFuncInterval, learn_mirrowedFuncInterval,
                withHierachyInterval, bruteForceInterval, ignoreBadFeaturesInterval, stopOnLongInterval, candidateSizePenaltyInterval,
                outputRoundsInterval};
            List<Dictionary<string, List<int>>> intSettings = new List<Dictionary<string, List<int>>> { baggingNumbersInterval , baggingTestDataFractionInterval,
                featureSizeThresInterval, crossVal_kInterval, numberOfRoundsInterval};
            List<Dictionary<string, List<double>>> doubleSettings = new List<Dictionary<string, List<double>>> { abortErrorInterval , backwardErrorDeltaInterval ,
                minImprovementInterval};

            //expand the parameter space with user input
            defineParameterSpace(parameters, boolSettings, intSettings, doubleSettings, lossFuncInterval, scoreMeasureInterval,
                learnTimeLimitInterval);

            //compute cartesian product over the sets of parameters and parse it into the string form to create ml settings
            var cartProduct = from lossFunc in lossFuncInterval.First().Value
                              from parallel in parallelInterval.First().Value
                              from bagging in baggingInterval.First().Value
                              from baggingNumbers in baggingNumbersInterval.First().Value
                              from baggingData in baggingTestDataFractionInterval.First().Value
                              from useBackward in useBackwardInterval.First().Value
                              from abortError in abortErrorInterval.First().Value
                              from limitFeatureSize in limitFeatureSizeInterval.First().Value
                              from featureSize in featureSizeThresInterval.First().Value
                              from quadFunc in quadFuncSupportInterval.First().Value
                              from crossVal in crossValInterval.First().Value
                              from crossVal_k in crossVal_kInterval.First().Value
                              from learn_log in learn_logFuncInterval.First().Value
                              from learn_asym in learn_asymFuncInterval.First().Value
                              from learn_ratio in learn_ratioFuncInterval.First().Value
                              from learn_mirrowed in learn_mirrowedFuncInterval.First().Value
                              from numberOfRounds in numberOfRoundsInterval.First().Value
                              from backwardError in backwardErrorDeltaInterval.First().Value
                              from minImprovement in minImprovementInterval.First().Value
                              from withHierachy in withHierachyInterval.First().Value
                              from bruteForce in bruteForceInterval.First().Value
                              from ignoreBadFeatures in ignoreBadFeaturesInterval.First().Value
                              from stopOnLong in stopOnLongInterval.First().Value
                              from candidateSizePenalty in candidateSizePenaltyInterval.First().Value
                              from learnTime in learnTimeLimitInterval.First().Value
                              from scoreMeasure in scoreMeasureInterval.First().Value
                              from outputRounds in outputRoundsInterval.First().Value
                              select new List<string> { baggingInterval.First().Key,":",bagging.ToString()," ",
                                     parallelInterval.First().Key,":",parallel.ToString()," ",
                                     lossFuncInterval.First().Key,":",lossFunc.ToString()," ",
                                     baggingNumbersInterval.First().Key,":", baggingNumbers.ToString()," ",
                                     baggingTestDataFractionInterval.First().Key,":",baggingData.ToString()," ",
                                     useBackwardInterval.First().Key,":",useBackward.ToString()," ",
                                     abortErrorInterval.First().Key,":",abortError.ToString()," ",
                                     limitFeatureSizeInterval.First().Key,":",limitFeatureSize.ToString()," ",
                                     featureSizeThresInterval.First().Key,":",featureSize.ToString()," ",
                                     quadFuncSupportInterval.First().Key,":",quadFunc.ToString()," ",
                                     crossValInterval.First().Key,":",crossVal.ToString()," ",
                                     crossVal_kInterval.First().Key,":",crossVal_k.ToString()," ",
                                     learn_logFuncInterval.First().Key,":",learn_log.ToString()," ",
                                     learn_asymFuncInterval.First().Key,":",learn_asym.ToString()," ",
                                     learn_ratioFuncInterval.First().Key,":",learn_ratio.ToString()," ",
                                     learn_mirrowedFuncInterval.First().Key,":",learn_mirrowed.ToString()," ",
                                     numberOfRoundsInterval.First().Key,":",numberOfRounds.ToString()," ",
                                     backwardErrorDeltaInterval.First().Key,":",backwardError.ToString()," ",
                                     minImprovementInterval.First().Key,":",minImprovement.ToString()," ",
                                     withHierachyInterval.First().Key,":",withHierachy.ToString()," ",
                                     bruteForceInterval.First().Key,":",bruteForce.ToString()," ",
                                     ignoreBadFeaturesInterval.First().Key,":",ignoreBadFeatures.ToString()," ",
                                     stopOnLongInterval.First().Key,":",stopOnLong.ToString()," ",
                                     candidateSizePenaltyInterval.First().Key,":",candidateSizePenalty.ToString()," ",
                                     learnTimeLimitInterval.First().Key,":",learnTime.ToString()," ",
                                     scoreMeasureInterval.First().Key,":",scoreMeasure.ToString()," ",
                                     outputRoundsInterval.First().Key,":",outputRounds.ToString()," "};

            List<ML_Settings> settingsInParameterSpace = new List<ML_Settings>();

            foreach (List<string> setting in cartProduct.AsEnumerable<List<string>>())
            {
                StringBuilder sb = new StringBuilder();
                foreach (string value in setting)
                {
                    sb.Append(value);
                }
                settingsInParameterSpace.Add(readSettings(sb.ToString()));
            }
            return settingsInParameterSpace;
        }
    }
}
