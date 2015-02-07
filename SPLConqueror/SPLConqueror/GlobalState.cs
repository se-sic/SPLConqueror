using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class GlobalState
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
        public static List<NFProperty> nfProperties = null;

        private GlobalState(){ }


        public static void clear()
        {
            varModel = null;
            currentNFP = null;
            allMeasurements = null;
            infModel = null;
            nfProperties = null;
        }

        // TODO: switch from non-normalized configurations to normalized configurations
    }
}
