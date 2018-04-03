using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using SPLConqueror_Core;

namespace MachineLearning.Learning
{
    public class ML_Settings
    {

        /// <summary>
        /// The epsilon within the error of the loss Function will be 0.
        /// A epsilon of 0 is equal to this feature not being present.
        /// </summary>
        public double epsilon = 0;

        public enum LossFunction {RELATIVE, LEASTSQUARES, ABSOLUTE}

        /// <summary>
        /// The loss function on which bases features are added to the influence model.
        /// </summary>
        public LossFunction lossFunction = LossFunction.RELATIVE;

        /// <summary>
        /// Turns the parallel execution of model candidates on/off.
        /// </summary>
        public bool parallelization = true;

        /// <summary>
        /// Turns the bagging functionality (ensemble learning) on. This functionality relies on parallelization (requires probably larger amount of memory).
        /// </summary>
        public bool bagging = false;

        /// <summary>
        /// Specifies how often an influence model is learned based on a subset of the measurement data
        /// </summary>
        public int baggingNumbers = 3;

        /// <summary>
        /// Lays a tube around the performance-influence function where all errors inside the tube are ignored.
        /// </summary>
        public bool considerEpsilonTube = false;


        /// <summary>
        /// Specifies the percentage of data taken from the test set to be used in one learning run
        /// </summary>
        public int baggingTestDataFraction = 50;

        /// <summary>
        /// Features existing in the model can be removed during the learning procedure if removal leads to a better model.  
        /// </summary>
        public bool useBackward = false;

        /// <summary>
        /// The threshold at which the learning process stops.
        /// </summary>
        public double abortError = 1;

        /// <summary>
        /// Functions created during the learning procedure can not become arbitrary complex. 
        /// </summary>
        public bool limitFeatureSize = false;


        /// <summary>
        /// The maximal number of options participating in one interaction.
        /// </summary>
        public int featureSizeTreshold = 4;

        /// <summary>
        /// The learner can learn quadratic functions of one numeric option, without learning the linear function apriory, if this property is true.
        /// </summary>
        public bool quadraticFunctionSupport = true;

        /// <summary>
        /// Cross validation is used during learning process if this property is true. 
        /// </summary>
        public bool crossValidation = false;

        public int crossValidation_k = 5;


        /// <summary>
        /// If true, the learn algorithm can learn logarithmic functions such as log(soption1). 
        /// </summary>
        public bool learn_logFunction = false;

        /// <summary>
        /// Allows the creation of logarithmic functions with multiple features such as log(soption1 * soption2).
        /// </summary>
        public bool learn_accumulatedLogFunction = false;

        public bool learn_asymFunction = false;

        public bool learn_ratioFunction = false;

        public bool learn_mirrowedFunction = false;

        /// <summary>
        /// Defines the number of rounds the learning process have to be performed. 
        /// </summary>
        public int numberOfRounds = 70;

        /// <summary>
        /// Defines the maximum increase of the error when removing a feature from the model
        /// </summary>
        public double backwardErrorDelta = 1;

        /// <summary>
        /// Defines the minimum error in improved a round must reach before either the learnings is aborted or the hierachy is increased for hierarchy learning
        /// </summary>
        public double minImprovementPerRound = 0.1;

        /// <summary>
        /// Defines whether we learn our model in hierachical steps
        /// </summary>
        public bool withHierarchy = false;

        /// <summary>
        /// Defines how candidate features are generated.  False - candidates 
        /// are generated in each step based on the features in the model and a
        /// set of initial features.  True - all possible combinations of initial
        /// features (up to featureSizeTreshold) are generated and used as
        /// candidates (brute force means, that features alredy present in the
        /// model are not taken inot account).
        /// </summary>
        public bool bruteForceCandidates = false;

        /// <summary>
        /// Enables an optimization: we do not want to consider candidates in the next X rounds that showed no or only a slight improvment in accuracy relative to all other candidates.
        /// </summary>
        public bool ignoreBadFeatures = false;

        /// <summary>
        /// If true, stop learning if the whole process is running longer than 1 hour and the current round runs longer then 30 minutes.
        /// </summary>
        public bool stopOnLongRound = true;

        /// <summary>
        /// If true, the candidate score (which is an average reduction of the 
        /// prediction error the candidate induces) is made dependent on its size.
        /// See FeatureSubsetSelection.learn() for scroe calculation.
        /// </summary>
        public bool candidateSizePenalty = true;

        /// <summary>
        /// Defines the time limit for the learning process. If 0, no time limit. Format: HH:MM:SS
        /// </summary>
        public TimeSpan learnTimeLimit = new TimeSpan(0);

        public enum ScoreMeasure {RELERROR, INFLUENCE};
        /// <summary>
        /// Defines which mesure is used to select the best candidate and to compute the score of a candidate. See ScoreMeasure enum for the available measures.
        /// </summary>
        public ScoreMeasure scoreMeasure = ScoreMeasure.RELERROR;

        /// <summary>
        /// If true, the info about the rounds is output not only to the 
        /// log file at the end of the learning, but also to the stdout
        /// during the learning after each round completion.
        /// </summary>
        public bool outputRoundsToStdout = false;

        public List<string> blacklisted = new List<string>();

        /// <summary>
        /// Returns a new settings object with the settings specified in the file as key value pair. Settings not beeing specified in this file will have the default value. 
        /// </summary>
        /// <param name="settings">All settings to be changed in a string with whitespaces as separator .</param>
        /// <returns>A settings object with the values specified in the file.</returns>
        public static ML_Settings readSettings(string settings)
        {
            settings = settings.Trim();
            settings = settings.Replace(System.Environment.NewLine, "");
            ML_Settings mls = new ML_Settings();
            String[] settingArray = settings.Split(' ');

            for (int i = 0; i < settingArray.Length; i++)
            {
                string[] nameAndValue = settingArray[i].Split(new char[] { ':' }, 2);
                if (!mls.setSetting(nameAndValue[0], nameAndValue[1]))
                {
                    GlobalState.logError.logLine("MlSetting " + nameAndValue[0] + " not found!");
                }

            }

            if (GlobalState.varModel != null && mls.blacklisted.Count > 0)
            {
                mls.checkAndCleanBlacklisted();
            }

            return mls;
        }

        /// <summary>
        /// Returns a new settings object with the settings specified in the file as key value pair. Settings not beeing specified in this file will have the default value. 
        /// </summary>
        /// <param name="settingLocation">Full qualified name of the settings file.</param>
        /// <returns>A settings object with the values specified in the file.</returns>
        public static ML_Settings readSettingsFromFile(string settingLocation)
        {
            ML_Settings mls = new ML_Settings();
            if (System.IO.File.Exists(settingLocation) == false)
            {
                GlobalState.logError.logLine("Could not load ML settings file! File (" + settingLocation + ") does not exit.");
                return mls;
            }
            System.IO.StreamReader file = new System.IO.StreamReader(settingLocation);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] nameAndValue = line.Split(new char[] { ' ' }, 2);
                if (!mls.setSetting(nameAndValue[0], nameAndValue[1]))
                {
                    GlobalState.logError.logLine("MlSetting " + nameAndValue[0] + " not found!");
                }
            }
            file.Close();

            if (GlobalState.varModel != null && mls.blacklisted.Count > 0)
            {
                mls.checkAndCleanBlacklisted();
            }

            return mls;
        }


        public ML_Settings()
        {
        }


        /// <summary>
        /// Set the value of one property of this object.
        /// </summary>
        /// <param name="name">Name of the field to be set.</param>
        /// <param name="value">String representation of the value of the field.</param>
        /// <returns>True of the field could be set with the given value. False if there is no field with the given name.</returns>
        public bool setSetting(string name, string value)
        {
            // Replace '-' in ml settings name with underscore since '-' isnt a valid symbol for names.
            // Now both underscore and '-' are supported for uniform naming.
            if (name.Contains("-"))
            {
                name = name.Replace("-", "_");
            }

            System.Reflection.FieldInfo fi =  this.GetType().GetField(name);
 
            if (fi == null)
                return false;

            string asa = fi.FieldType.FullName;

            // The processing of the blacklist
            if (name.ToLower().Equals("blacklisted"))
            {
                String[] optionsToBlacklist = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (String option in optionsToBlacklist)
                {
                    this.blacklisted.Add(option.ToLower());
                }
                this.blacklisted = this.blacklisted.Distinct().ToList();
                return true;
            }
            if (fi.FieldType.FullName.Equals("System.Boolean"))
            {
                if (value.ToLowerInvariant() == "true" || value.ToLowerInvariant() == "false")
                {
                    fi.SetValue(this, Convert.ToBoolean(value));
                    return true;
                }else
                    return false;

            }
            if (fi.FieldType.FullName.Equals("System.Int32"))
            {
                int n;
                bool isNumeric = int.TryParse(value, out n);

                if (isNumeric)
                {
                    fi.SetValue(this, n);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (fi.FieldType.FullName.Equals("System.Int64"))
            {
                int n;
                bool isNumeric = int.TryParse(value, out n);

                if (isNumeric)
                {
                    fi.SetValue(this, n);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            if (fi.FieldType.FullName.Equals("MachineLearning.Learning.ML_Settings+LossFunction"))
            {
                if(value.Equals("RELATIVE"))
                {
                    fi.SetValue(this, LossFunction.RELATIVE);
                    return true;
                }
                if (value.Equals("LEASTSQUARES"))
                {
                    fi.SetValue(this, LossFunction.LEASTSQUARES);
                    return true;
                }
                if (value.Equals("ABSOLUTE"))
                {
                    fi.SetValue(this, LossFunction.ABSOLUTE); 
                    return true;
                }
            }
            if (fi.FieldType.FullName.Equals("System.Double"))
            {
                double n;
                bool isNumeric = double.TryParse(value, out n);

                if (isNumeric)
                {
                    fi.SetValue(this, n);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            if (fi.FieldType.FullName.Equals("System.TimeSpan"))
            {
                TimeSpan n;
                bool isValid = TimeSpan.TryParse(value, out n);

                if (isValid)
                {
                    fi.SetValue(this, n);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (fi.FieldType.FullName.Equals("MachineLearning.Learning.ML_Settings+ScoreMeasure"))
            {
                ScoreMeasure parsedValue;
                try {
                    parsedValue = (ScoreMeasure) Enum.Parse(typeof(ScoreMeasure), value.ToUpperInvariant());
                }
                catch (ArgumentException) {
                    return false;
                }
                fi.SetValue(this, parsedValue);
                return true;
            }


            return false;
        }

        /// <summary>
        /// Checks if the blacklisted features are valid features in the current variability model.
        /// Removes the features that are not valid.
        /// </summary>
        public void checkAndCleanBlacklisted()
        {
            List<string> toRemove = new List<string>();
            foreach (string blacklistedFeature in blacklisted)
            {
                bool isValidFeature = false;
                foreach (BinaryOption binOpt in GlobalState.varModel.BinaryOptions)
                {
                    if (binOpt.ToString().ToLower().Equals(blacklistedFeature))
                    {
                        GlobalState.logError.logLine(binOpt.ToString() + ": Cannot blacklist binary features.");
                    }
                }

                foreach (NumericOption numOpt in GlobalState.varModel.NumericOptions)
                {
                    if (numOpt.ToString().ToLower().Equals(blacklistedFeature))
                    {
                        isValidFeature = true;
                    }
                }

                if (!isValidFeature)
                {
                    GlobalState.logError.logLine("\"" + blacklistedFeature + "\"" + " is not a valid feature to blacklist.");
                    toRemove.Add(blacklistedFeature);
                }
            }
            blacklisted = blacklisted.Except(toRemove).ToList();
        }


        /// <summary>
        /// A textual representation of the machine learning settings. The representation consist of a key value representation of all field of the settings with the dedicated values. 
        /// </summary>
        /// <returns>textual representation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                if (!field.IsStatic)
                {
                    if (field.Name == "blacklisted")
                    {
                        sb.Append(field.Name + ":");
                        ((List<String>)field.GetValue(this)).ForEach(x => sb.Append(x + ","));
                        sb.Append(" ");
                    } else
                    {
                        // Replace underscore with '-' for uniform naming in string representation.
                        sb.Append(field.Name.Replace("_", "-") + ":" + field.GetValue(this) + " ");
                    }
                }
            }
            sb.Append(System.Environment.NewLine);
 	        return sb.ToString();
        }

    }
}
