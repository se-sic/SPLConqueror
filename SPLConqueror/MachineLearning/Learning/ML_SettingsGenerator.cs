using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;using System.Globalization;
using SPLConqueror_Core;
using static MachineLearning.Learning.ML_Settings;

namespace MachineLearning.Learning
{
    public class ML_SettingsGenerator
    {
        /// <summary>
        /// Convert string that should contain a setting-values pair, in the form settingName=[v1,v2,v3,...,vn],
        /// to a Tuple with settingName as Item1 and the values as a List.
        /// </summary>
        /// <param name="parameter">String with setting-value.</param>
        /// <returns>Tuple with setting and values.</returns>
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
            try
            {
                toLossFunction(toTest);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

        private static LossFunction toLossFunction(string toConvert)
        {
            bool ignoreCase = true;
            return (LossFunction)Enum.Parse(typeof(LossFunction), toConvert, ignoreCase);
        }

        private static bool isScoreMeasure(string toTest)
        {
            try
            {
                toScoreMeasure(toTest);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

        private static ScoreMeasure toScoreMeasure(string toConvert)
        {
            bool ignoreCase = true;
            return (ScoreMeasure)Enum.Parse(typeof(ScoreMeasure), toConvert, ignoreCase);
        }

        private static void defineParameterSpace(string[] parameters, Dictionary<string, List<bool>> boolSettings,
            Dictionary<string, List<int>> intSettings, Dictionary<string, List<double>> doubleSettings,
            Dictionary<string, List<LossFunction>> lossFuncInterval, Dictionary<string, List<ScoreMeasure>> scoreMeasureInterval,
            Dictionary<string, List<TimeSpan>> learnTimeLimitInterval)
        {
            foreach (string parameter in parameters)
            {
                //dummy
                int y;
                double x;
                TimeSpan z;

                //setting name and values that should be within the parameter space
                Tuple<string, string[]> nameAndValues = extractSettings(parameter);

                ML_Settings referenceSetting = new ML_Settings();
                System.Reflection.FieldInfo fi = referenceSetting.GetType().GetField(nameAndValues.Item1);

                if (fi == null)
                {
                    GlobalState.logInfo.logLine("Invalid variable name: " + nameAndValues.Item1 +
                        ". This setting will be ignored.");
                }
                else if (isBool(nameAndValues.Item2[0]) && fi.FieldType.FullName.Equals("System.Boolean"))
                {
                    List<bool> toAdd = new List<bool>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(toBool(value));
                    }
                    boolSettings.Add(nameAndValues.Item1, toAdd);
                }
                else if (int.TryParse(nameAndValues.Item2[0], out y)
                    && (fi.FieldType.FullName.Equals("System.Int32") || fi.FieldType.FullName.Equals("System.Int64")))
                {
                    List<int> toAdd = new List<int>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(int.Parse(value));
                    }
                    intSettings.Add(nameAndValues.Item1, toAdd);
                }
                else if (Double.TryParse(nameAndValues.Item2[0], out x) && fi.FieldType.FullName.Equals("System.Double"))
                {
                    List<double> toAdd = new List<double>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(Double.Parse(value, CultureInfo.InvariantCulture));
                    }
                    doubleSettings.Add(nameAndValues.Item1, toAdd);
                }
                else if (isLossFunction(nameAndValues.Item2[0])
                      && fi.FieldType.FullName.Equals("MachineLearning.Learning.ML_Settings+LossFunction"))
                {
                    List<LossFunction> toAdd = new List<LossFunction>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(toLossFunction(value));
                    }
                    lossFuncInterval[nameAndValues.Item1] = toAdd;
                }
                else if (isScoreMeasure(nameAndValues.Item2[0])
                    && fi.FieldType.FullName.Equals("MachineLearning.Learning.ML_Settings+ScoreMeasure"))
                {
                    List<ScoreMeasure> toAdd = new List<ScoreMeasure>();
                    foreach (string value in nameAndValues.Item2)
                    {
                        toAdd.Add(toScoreMeasure(value));
                    }
                    scoreMeasureInterval[nameAndValues.Item1] = toAdd;
                }
                else if (TimeSpan.TryParse(nameAndValues.Item2[0], out z)
                    && fi.FieldType.FullName.Equals("System.TimeSpan"))
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
                    GlobalState.logInfo.logLine("Invalid setting-value pair: " + nameAndValues.Item1 + " "
                        + string.Join(",", nameAndValues.Item2) + ". This setting will be ignored.");
                }
            }
        }

        /// <summary>
        /// Method that generates all ML_Settings objects within a user defined parameter space
        /// </summary>
        /// <param name="parameters">Definition of the parameter space in String format</param>
        /// <returns>List of all ML_Settings within the parameter space</returns>
        public static List<ML_Settings> generateSettings(string[] parameters)
        {
            //initialize the Dictionaries for Settings with Enumerations and other objects as values
            Dictionary<string, List<LossFunction>> lossFuncInterval = new Dictionary<string, List<LossFunction>>();
            lossFuncInterval.Add("lossFunction", new List<LossFunction> { LossFunction.RELATIVE });
            Dictionary<string, List<TimeSpan>> learnTimeLimitInterval = new Dictionary<string, List<TimeSpan>>();
            learnTimeLimitInterval.Add("learnTimeLimit", new List<TimeSpan> { new TimeSpan(0) });
            Dictionary<string, List<ScoreMeasure>> scoreMeasureInterval = new Dictionary<string, List<ScoreMeasure>>();
            scoreMeasureInterval.Add("scoreMeasure", new List<ScoreMeasure> { ScoreMeasure.RELERROR });

            //initialize the Dictionaries for the primitive data types
            Dictionary<string, List<bool>> boolSettings = new Dictionary<string, List<bool>>();
            Dictionary<string, List<int>> intSettings = new Dictionary<string, List<int>>();
            Dictionary<string, List<double>> doubleSettings = new Dictionary<string, List<double>>();

            //create the parameter space
            defineParameterSpace(parameters, boolSettings, intSettings, doubleSettings, lossFuncInterval, scoreMeasureInterval,
                learnTimeLimitInterval);

            //initial cartesian product of Enumeratíon and other object Settings
            var cartProduct = from lossFunc in lossFuncInterval.First().Value
                              from learnTime in learnTimeLimitInterval.First().Value
                              from scoreMeasure in scoreMeasureInterval.First().Value
                              select lossFuncInterval.First().Key + ":" + lossFunc.ToString() + " " +
                                     learnTimeLimitInterval.First().Key + ":" + learnTime.ToString() + " " +
                                     scoreMeasureInterval.First().Key + ":" + scoreMeasure.ToString();

            //compute cartesian product of the previous cart product with every int setting
            foreach (KeyValuePair<string, List<int>> intSetting in intSettings)
            {
                cartProduct = from previousSettings in cartProduct
                              from attachedSetting in intSetting.Value
                              select intSetting.Key + ":" + attachedSetting
                              + " " + previousSettings;
            }

            //compute cartesian product of the previous cart product with every bool setting
            foreach (KeyValuePair<string, List<bool>> boolSetting in boolSettings)
            {
                cartProduct = from previousSettings in cartProduct
                              from attachedSetting in boolSetting.Value
                              select boolSetting.Key + ":" + attachedSetting
                              + " " + previousSettings;
            }

            //compute cartesian product of the previous cart product with every double setting
            foreach (KeyValuePair<string, List<double>> doubleSetting in doubleSettings)
            {
                cartProduct = from previousSettings in cartProduct
                              from attachedSetting in doubleSetting.Value
                              select doubleSetting.Key + ":" + attachedSetting
                              + " " + previousSettings;
            }

            List<ML_Settings> settingsInParameterSpace = new List<ML_Settings>();

            //each element in the cartesian product is a ML_Setting
            foreach (string setting in cartProduct)
            {
                settingsInParameterSpace.Add(readSettings(setting));
            }
            return settingsInParameterSpace;
        }
    }
}
