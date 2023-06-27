using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class stores and provides access to all configurations of the case study. 
    /// </summary>
    public class ResultDB
    {
        private List<Configuration> configurations = new List<Configuration>();
        private IDictionary<string, IDictionary<string, List<Configuration>>> configsMapping =
            new Dictionary<string, IDictionary<string, List<Configuration>>>();

        /// <summary>
        /// This structre provides the maximum of the measured values for each non-functional property. 
        /// </summary>
        public IDictionary<NFProperty, double> maxMeasuredValue = new Dictionary<NFProperty, double>();
        private static int splitFactor = 2;

        /// <summary>
        /// A list containing the blacklisted features.
        /// </summary>
        public List<String> blacklisted;


        /// <summary>
        /// The set of all configurations of the current case study.
        /// </summary>
        public List<Configuration> Configurations
        {
            get { return configurations; }
            set
            {
                configurations = value;
                configsMapping.Clear();
                updateMapping();
            }
        }

        /// <summary>
        /// Set the blacklisted features.
        /// </summary>
        /// <param name="blacklist">The features to ignore/blacklist.</param>
        public void setBlackList(List<String> blacklist)
        {
            this.blacklisted = blacklist;
        }

        /// <summary>
        /// Adds a configuration to the set of all configuration. 
        /// </summary>
        /// <param name="configuration">The configuration to add.</param>
        public void add(Configuration configuration)
        {
            this.configurations.Add(configuration);
            addToMapping(configuration);
        }

        private void addToMapping(Configuration config)
        {
            string currVector = calculateConfigBinVector(config);
            IDictionary<string, List<Configuration>> numMapping = null;

            if (!configsMapping.TryGetValue(currVector, out numMapping))
            {
                numMapping = new Dictionary<string, List<Configuration>>();
                configsMapping.Add(currVector, numMapping);
            }

            currVector = calculateConfigNumVector(config);
            List<Configuration> list = null;

            if (numMapping.TryGetValue(currVector, out list))
                list.Add(config);
            else
            {
                list = new List<Configuration>();
                list.Add(config);
                numMapping.Add(currVector, list);
            }
        }

        /// <summary>
        /// Get configurations that are similar to the given one.
        /// </summary>
        /// <param name="config">The configuration to search a similar configuration for.</param>
        /// <returns>A list of configurations containing similar configurations.</returns>
        public List<Configuration> getSimilarConfigs(Configuration config)
        {
            IDictionary<string, List<Configuration>> numMapping = null;

            if (configsMapping.TryGetValue(calculateConfigBinVector(config), out numMapping))
            {
                List<Configuration> list = null;

                if (numMapping.TryGetValue(calculateConfigNumVector(config), out list))
                    return list;
                else
                {
                    List<Configuration> result = new List<Configuration>();

                    foreach (var val in numMapping.Values)
                        result.AddRange(val);

                    return result;
                }
            }
            else
                return new List<Configuration>();
        }

        private string calculateConfigBinVector(Configuration config)
        {
            string vector = "";

            foreach (BinaryOption opt in GlobalState.varModel.BinaryOptions)
            {
                if (opt != GlobalState.varModel.Root)
                {
                    BinaryOption.BinaryValue val;
                    config.BinaryOptions.TryGetValue(opt, out val);

                    if (val == BinaryOption.BinaryValue.Selected)
                        vector += "1";
                    else
                        vector += "0";
                }
            }

            return vector;
        }

        private string calculateConfigNumVector(Configuration config)
        {
            string vector = "";
            int amountOfParts = (int)Math.Pow(2, splitFactor);

            if (splitFactor > 0)
            {
                foreach (NumericOption opt in GlobalState.varModel.NumericOptions)
                {

                    if (blacklisted != null && blacklisted.Contains(opt.Name.ToLower()))
                    {
                        continue;
                    }

                    List<double> elems = opt.getAllValues();
                    int amountOfElemsInParts = elems.Count >= amountOfParts ? (int)Math.Round((double)elems.Count / amountOfParts, 0) : 1;
                    List<double> currentElems = null;
                    bool found = false;
                    double val = 0;

                    if (!config.NumericOptions.TryGetValue(opt, out val))
                        val = opt.getCenterValue();

                    if (opt.Optional && opt.OptionalFlag == val)
                    {
                        // So for deselected values we a starting 0 with 1 as padding, which can never be 
                        // produced by non-deselected values.
                        vector += new StringBuilder(splitFactor).Insert(0, "0").Insert(1, "1", splitFactor).ToString();
                        continue;
                    }

                    for (int i = 0; i < amountOfParts && !found; i++)
                    {
                        int rest = elems.Count - i * amountOfElemsInParts;
                        currentElems = elems.GetRange(i * amountOfElemsInParts, rest >= amountOfElemsInParts ? amountOfElemsInParts : rest);

                        if (currentElems.Contains(val))
                        {
                            found = true;
                            String bin = Convert.ToString(i, 2);

                            while (bin.Length < splitFactor)
                                bin = "0" + bin;

                            vector += bin;
                        }
                    }
                }
            }

            return vector;
        }

        private void updateMapping()
        {
            foreach (Configuration config in configurations)
                addToMapping(config);
        }
    }
}
