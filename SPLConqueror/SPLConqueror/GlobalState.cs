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



    }
}
