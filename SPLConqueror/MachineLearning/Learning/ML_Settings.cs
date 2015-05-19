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

        public LossFunction lossFunction = LossFunction.RELATIVE;

        /// <summary>
        /// Features existing in the model can be removed during the learning procedure if removal leads to a better model.  
        /// </summary>
        public bool useBackward = false;


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
        /// Returns a new settings object with the settings specified in the file as key value pair. Settings not beeing specified in this file will have the default value. 
        /// </summary>
        /// <param name="settings">All settings to be changed in a string with whitespaces as separator .</param>
        /// <returns>A settings object with the values specified in the file.</returns>
        public static ML_Settings readSettings(string settings)
        {
            ML_Settings mls = new ML_Settings();
            String[] settingArray = settings.Split(' ');

            for (int i = 0; i < settingArray.Length; i++)
            {
                string[] nameAndValue = settingArray[i].Split(new char[] { ':' }, 2);
                if (!mls.setSetting(nameAndValue[0], nameAndValue[1]))
                {
                    GlobalState.logError.log("MlSetting " + nameAndValue[0] + " not found!");
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
                GlobalState.logError.log("Could not load ML settings file! File (" + settingLocation + ") does not exit.");
                return mls;
            }
            System.IO.StreamReader file = new System.IO.StreamReader(settingLocation);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] nameAndValue = line.Split(new char[] { ' ' }, 2);
                if (!mls.setSetting(nameAndValue[0], nameAndValue[1]))
                {
                    GlobalState.logError.log("MlSetting " + nameAndValue[0] + " not found!");
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
                fi.SetValue(this, Convert.ToBoolean(value));
                return true;
            }
            if (fi.FieldType.FullName.Equals("System.Int32"))
            {
                fi.SetValue(this, Convert.ToInt32(value));
                return true;
            }
            if (fi.FieldType.FullName.Equals("System.Int64"))
            {
                fi.SetValue(this, Convert.ToInt64(value));
                return true;
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
                fi.SetValue(this, Convert.ToDouble(value));
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
