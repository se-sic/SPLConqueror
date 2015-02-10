using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MachineLearning.Learning
{
    public class ML_Settings
    {
        private static ML_Settings defaultSettings = null;

        public static bool readDefaultSettings(string settingLocation)
        {
            throw new NotImplementedException();

            return true;
        }

        private ML_Settings()
        {
        }

        public static ML_Settings getDefaultSettings()
        {
            ML_Settings newSettings = new ML_Settings();
            newSettings = (ML_Settings) defaultSettings.MemberwiseClone();
            return newSettings;
        }

        public bool setSetting(string name, object value)
        {
            System.Reflection.FieldInfo fi =  this.GetType().GetField(name);
 
            if (fi == null)
                return false;
            
            fi.SetValue(this, value);
            return true;
        }

    }
}
