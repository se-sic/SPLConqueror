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
        public int baggingNumbers = 100;

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


        /// <summary>
        /// If true, the learn algorithm can learn logarithmic functions auch as log(soption1). 
        /// </summary>
        public bool learn_logFunction = false;

        public bool learn_asymFunction = false;

        public bool learn_ratioFunction = false;

        public bool learn_mirrowedFunction = true;

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
        public bool withHierarchy = true;

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
            System.Reflection.FieldInfo fi =  this.GetType().GetField(name);
 
            if (fi == null)
                return false;

            string asa = fi.FieldType.FullName;

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
        /// A textual representation of the machine learning settings. The representation consist of a key value representation of all field of the settings with the dedicated values. 
        /// </summary>
        /// <returns>textual representation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            FieldInfo[] fields = this.GetType().GetFields();

            foreach (FieldInfo field in fields)
            {
                if(!field.IsStatic)
                    sb.Append(field.Name+":"+field.GetValue(this)+" ");
            }
            sb.Append(System.Environment.NewLine);
 	        return sb.ToString();
        }

    }
}
