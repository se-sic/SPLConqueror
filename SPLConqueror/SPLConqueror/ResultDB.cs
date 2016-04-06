using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class ResultDB
    {
        private List<Configuration> configurations = new List<Configuration>();
        private IDictionary<string, IDictionary<string, List<Configuration>>> configsMapping =
            new Dictionary<string, IDictionary<string, List<Configuration>>>();
        public IDictionary<NFProperty, double> maxMeasuredValue = new Dictionary<NFProperty, double>();

        public List<Configuration> Configurations
        {
            get { return configurations; }
            set { configurations = value;
                configsMapping.Clear();
                updateMapping();
            }
        }

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

        private string calculateConfigBinVector (Configuration config)
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

            // Aufteilfaktor: Wohin damit?
            int a = 3;
            int amountOfParts = (int)Math.Pow(2, a);

            if (a > 0)
            {
                foreach (NumericOption opt in GlobalState.varModel.NumericOptions)
                {
                    List<double> elems = opt.getAllValues();
                    int amountOfElemsInParts = elems.Count >= amountOfParts ? (int)Math.Round((double)elems.Count / amountOfParts, 0) : 1;
                    List<double> currentElems = null;
                    bool found = false;
                    double val = 0;

                    if (!config.NumericOptions.TryGetValue(opt, out val))
                        val = opt.DefaultValue;

                    for (int i = 0; i < amountOfParts && !found; i++)
                    {
                        int rest = elems.Count - i * amountOfElemsInParts;
                        currentElems = elems.GetRange(i * amountOfElemsInParts, rest >= amountOfElemsInParts ? amountOfElemsInParts : rest);

                        if (currentElems.Contains(val))
                        {
                            found = true;
                            String bin = Convert.ToString(i, 2);

                            while (bin.Length < a)
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
