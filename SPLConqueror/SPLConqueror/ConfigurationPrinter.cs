using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class writes a set of configurations to a given file.
    /// </summary>
    public class ConfigurationPrinter
    {
        /// <summary>
        /// The symbol for the element delimiter of the .csv-file.
        /// </summary>
        public const string CSV_ELEMENT_DELIMITER = ";";

        /// <summary>
        /// The symbol for the row delimiter of the .csv-file.
        /// </summary>
        public const string CSV_ROW_DELIMITER = "\n";

        /// <summary>
        /// The file extension of .csv-files
        /// </summary>
        public const string CSV_FILE_EXTENSION = ".csv";

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
        /// <param name="prefix">The prefix of each configuration. Uses 'prefix' as default</param>
        /// <param name="postfix">The postfix of each configuartion. Uses 'postfix' as default</param>
        /// <param name="order"> Order of the configuration option. If only a partial order is given, options not considered in the order are written after all options existing in the order. Information about all configuration options are used from the variability model in the GlobalState.</param>
        public ConfigurationPrinter(string file, List<ConfigurationOption> order, string prefix = "prefix", string postfix = "postfix")
        {
            this.file = file;
            this.prefix = prefix;
            this.postfix = postfix;

            if (GlobalState.varModel.BinaryOptions.Count + GlobalState.varModel.NumericOptions.Count > order.Count)
                this.order = enrichWithAllOptions(order);
            else
                this.order = order;

        }

        /// <summary>
        /// This method appends a textual representation of the set of configurations to a specific file. The file and a specific prefix and postfix can be specified using the constructors of this class. 
        /// </summary>
        /// <param name="configurations">The set of configurations.</param>
        /// <returns>True if the configurations can be printed in the file.</returns>
		public bool print(List<Configuration> configurations, List<NFProperty> nfpPropertiesToPrint = null)
        {
            if (file.Length > 250)
            {
                bool isCSV = file.EndsWith(CSV_FILE_EXTENSION);
                file = file.Substring(0, file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                Random rand = new Random();
                file += "configs" + rand.Next(99);
                if (isCSV)
                {
                    file += ".csv";
                }
                GlobalState.logInfo.logLine("File name for configurations file was too long. Changed to" + file);
            }
            if (!File.Exists(file) && !Path.GetDirectoryName(file).Equals(""))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }

            if (file.EndsWith(CSV_FILE_EXTENSION) && order != null)
            {
                if (nfpPropertiesToPrint == null)
                {
                    nfpPropertiesToPrint = new List<NFProperty>();
                    if (!GlobalState.currentNFP.Equals(NFProperty.DefaultProperty))
                    {
                        nfpPropertiesToPrint.Add(GlobalState.currentNFP);
                    }
                }
                return print_csv(configurations, nfpPropertiesToPrint);
            }

            if (order == null)
                return print_noOrder(configurations);


            return print_order(configurations);

        }

        private bool print_csv(List<Configuration> configurations, List<NFProperty> nfpPropertiesToPrint)
        {
            StringBuilder csvContent = new StringBuilder();

            for (int i = 0; i < this.order.Count; i++)
            {
                ConfigurationOption c = this.order[i];
                if (i != 0)
                {
                    csvContent.Append(CSV_ELEMENT_DELIMITER);
                }
                csvContent.Append(c.Name);
            }

            if (!GlobalState.currentNFP.Equals(NFProperty.DefaultProperty))
            {
                foreach (NFProperty nfpProperty in nfpPropertiesToPrint)
                {
                    csvContent.Append(CSV_ELEMENT_DELIMITER);
                    csvContent.Append(nfpProperty.Name);
                }
            }

            csvContent.Append(CSV_ROW_DELIMITER);

            foreach (Configuration c in configurations)
            {
                csvContent.Append(c.toCsv(this.order, nfpPropertiesToPrint));
            }

            File.WriteAllText(file, csvContent.ToString());

            return true;
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
            File.AppendAllText(file, sb.ToString());

            return true;

        }

        private bool print_order(List<Configuration> configurations)
        {

            foreach (Configuration c in configurations)
            {
                File.AppendAllText(file, prefix + " ");
                File.AppendAllText(file, "\"" + c.ToString(order) + "\"");
                File.AppendAllText(file, c.OutputString(order) + " ");
                File.AppendAllText(file, postfix + " " + System.Environment.NewLine);
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
                if (optionOrder.Contains(nOpt))
                    continue;
                optionOrder.Add(nOpt);
            }
            return optionOrder;
        }
    }
}
