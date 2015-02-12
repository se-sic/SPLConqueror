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
        /// <param name="order"> Order of the configuration option. Currently no partial order is supported.</param>
        public ConfigurationPrinter(string file, string prefix, string postfix, List<ConfigurationOption> order)
        {
            this.file = file;
            this.prefix = prefix;
            this.postfix = postfix;
            this.order = order;
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

        // TODO
        private bool print_order(List<Configuration> configurations)
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



    }
}
