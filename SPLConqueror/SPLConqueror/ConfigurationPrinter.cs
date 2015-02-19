using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    public class ConfigurationPrinter
    {

        string file = "";

        string prefix = "";
        string postfix = "";

        List<ConfigurationOption> order = null;


        /// <summary>
        /// Creates a new printer object with the specified file, prefix and postfix.
        /// </summary>
        /// <param name="file">Path and name of the file, the configurations have to be printed in. </param>
        /// <param name="prefix">The prefix of each configuration.</param>
        /// <param name="postfix">The postfix of each configuartion.</param>
        public ConfigurationPrinter(string file, string prefix, string postfix)
        {
            this.file = file;
            this.prefix = prefix;
            this.postfix = postfix;
        }

        /// <summary>
        /// Creates a new printer object with the specified file, prefix and postfix.
        /// </summary>
        /// <param name="file">Path and name of the file, the configurations have to be printed in. </param>
        /// <param name="prefix">The prefix of each configuration.</param>
        /// <param name="postfix">The postfix of each configuartion.</param>
        /// <param name="order"> Order of the configuration option. If only a partial order is given, options not considered in the order are written after all options existing in the order. Information about all configuration options are used from the variability model in the GlobalState.</param>
        public ConfigurationPrinter(string file, string prefix, string postfix, List<ConfigurationOption> order)
        {
            this.file = file;
            this.prefix = prefix;
            this.postfix = postfix;

            if (GlobalState.varModel.BinaryOptions.Count + GlobalState.varModel.NumericOptions.Count > order.Count)
                this.order = enrichWithAllOptions(order);

        }

        /// <summary>
        /// This method appends a textual representation of the set of configurations to a specific file. The file and a specific prefix and postfix can be specified using the constructors of this class. 
        /// </summary>
        /// <param name="configurations">The set of configurations.</param>
        /// <returns>True if the configurations can be printed in the file.</returns>
        public bool print(List<Configuration> configurations)
        {
            if (order == null)
                return print_noOrder(configurations);


            return print_order(configurations);

        }

        private bool print_noOrder(List<Configuration> configurations)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Configuration c in configurations)
            {
                sb.Append(prefix + " ");
                sb.Append(c.ToString() + " ");
                sb.Append(postfix + " " + System.Environment.NewLine);

            }

            using (StreamWriter outfile = new StreamWriter(file))
            {
                outfile.Write(sb.ToString());
            }
            return true;

        }

        private bool print_order(List<Configuration> configurations)
        {

            StringBuilder sb = new StringBuilder();

            foreach (Configuration c in configurations)
            {
                sb.Append(prefix + " ");
                sb.Append(c.ToString(order) + " ");
                sb.Append(postfix + " " + System.Environment.NewLine);

            }

            using (StreamWriter outfile = new StreamWriter(file))
            {
                outfile.Write(sb.ToString());
            }
            return true;

        }

        private List<ConfigurationOption> enrichWithAllOptions(List<ConfigurationOption> optionOrder)
        {
            foreach (BinaryOption bopt in GlobalState.varModel.BinaryOptions)
            {
                if (optionOrder.Contains(bopt))
                    continue;
                optionOrder.Add(bopt);
            }

            foreach (NumericOption nOpt in GlobalState.varModel.NumericOptions)
            {
                if(optionOrder.Contains(nOpt))
                    continue;
                optionOrder.Add(nOpt);
            }
            return optionOrder;
        }



    }
}
