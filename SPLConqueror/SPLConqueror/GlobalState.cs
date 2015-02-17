using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class stores all information that are needed to perform multiple experiments with one case study. 
    /// </summary>
    public class GlobalState
    {
        public static InfoLog logInfo = new InfoLog(null);

        public static ErrorLog logError = new ErrorLog(null);

        /// <summary>
        /// The variability model of the case study. 
        /// </summary>
        public static VariabilityModel varModel = null;

        /// <summary>
        /// The property being considered. 
        /// </summary>
        public static NFProperty currentNFP = null;
        public static ResultDB allMeasurements = null;
        public static InfluenceModel infModel = null;

        /// <summary>
        /// All properties of the current case study. 
        /// </summary>
        public static Dictionary<string, NFProperty> nfProperties = null;

        private GlobalState(){ }

        /// <summary>
        /// Clears the global state. This mehtod should be used after performing all experiments of one case study. 
        /// </summary>
        public static void clear()
        {
            varModel = null;
            currentNFP = null;
            allMeasurements = null;
            infModel = null;
            nfProperties = null;
        }


        /// <summary>
        /// The mehtod returns non function property with the given name. If there is no property with the name, a new property is created. 
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>A non functional property with the specified name.</returns>
        public static NFProperty getOrCreateProperty(string name)
        {
            if(nfProperties[name] != null)
                return nfProperties[name];
            else{
                NFProperty newProp = new NFProperty(name);
                nfProperties.Add(name, newProp);
                return newProp;
            }
        }

        /// <summary>
        /// Adds a configration to the global state. 
        /// </summary>
        /// <param name="config">An configuration of the variability model.</param>
        public static void addConfiguration(Configuration config)
        {
            GlobalState.allMeasurements.add(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public static void setDefaultProperty(String propertyName)
        {
            GlobalState.currentNFP = GlobalState.nfProperties[propertyName];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public static void setDefaultProperty(NFProperty property)
        {
            GlobalState.currentNFP = property;
        }

        // TODO: switch from non-normalized configurations to normalized configurations



        /// <summary>
        /// This function gets a list of configurations and checks in the global state whether this configuration has a measured value and returns it if so.
        /// </summary>
        /// <param name="list">The list of configurations for which we want a measured value.</param>
        /// <returns>A list of configurations containinga measured value. Might be empty.</returns>
        public static List<Configuration> getMeasuredConfigs(List<Configuration> list)
        {
            List<Configuration> configsWithValues = new List<Configuration>();
            foreach(var config in list) {
                foreach (var configInGS in GlobalState.allMeasurements.Configurations)
                {
                    if (config.Equals(configInGS))
                        configsWithValues.Add(configInGS);
                }
            }
            return configsWithValues;
        }
    }
}
