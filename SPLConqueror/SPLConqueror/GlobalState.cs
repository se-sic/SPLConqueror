using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class GlobalState
    {
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


        public static void clear()
        {
            varModel = null;
            currentNFP = null;
            allMeasurements = null;
            infModel = null;
            nfProperties = null;
        }


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

        internal static void addConfiguration(Configuration config)
        {
            GlobalState.allMeasurements.add(config);
        }


        public static void setDefaultProperty(String propertyName)
        {
            GlobalState.currentNFP = GlobalState.nfProperties[propertyName];
        }

        // TODO: switch from non-normalized configurations to normalized configurations



    }
}
