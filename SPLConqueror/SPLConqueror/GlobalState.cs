using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class GlobalState
    {
        public static VariabilityModel varModel = null;
        public static NFProperty currentNFP = null;
        public static ResultDB allMeasurements = null;
        public static InfluenceModel infModel = null;

        private GlobalState(){ }

    }
}
