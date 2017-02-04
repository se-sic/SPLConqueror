using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MachineLearning.Learning;
using System.IO;
using System.Xml;

namespace Persistence
{
    public class PersistMLSettings
    {
        private PersistMLSettings()
        {

        }

        /// <summary>
        /// String that represents the current mlsettings to save it as persistent file.
        /// </summary>
        /// <returns>ML_Settings in a xml format.</returns>
        public static string dump(ML_Settings mlsettings)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(ML_Settings));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, mlsettings);
            return sw.ToString();
        }


        /// <summary>
        /// Recover a older ml settings from the persistent dump.
        /// </summary>
        /// <param name="persistentDump">Persisten ml settings dump as string</param>
        /// <returns>Previous ML_Settings object</returns>
        public static ML_Settings recoverFromPersistentDump(string persistentDump)
        {
            XmlDocument persistentMLSettings = new System.Xml.XmlDocument();
            persistentMLSettings.Load(persistentDump);
            XmlElement mlsettings = persistentMLSettings.DocumentElement;
            StringBuilder sb = new StringBuilder();
            foreach (XmlElement data in mlsettings)
            {
                if (data.InnerText != "" && data.InnerText.Trim().Length > 0)
                {
                    sb.Append(data.Name + ":" + data.InnerText.Trim() + " ");
                }
            }
            return ML_Settings.readSettings(sb.ToString());

        }

    }
}
