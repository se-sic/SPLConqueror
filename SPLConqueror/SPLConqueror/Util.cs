using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class Util
    {

        /// <summary>
        /// Add all configurations of one file to the configurations stored in the GlobalState Object. The variability model stored in the GlobalState is used during the process. 
        /// </summary>
        /// <param name="file">File with the configurations.</param>
        public static void loadConfigurations(string file)
        {
            ConfigurationReader cr = new ConfigurationReader();
            List<Configuration> configurations = cr.readConfigurations(file, GlobalState.varModel);
            foreach (Configuration c in configurations)
                GlobalState.addConfiguration(c);
        }

    }
}
