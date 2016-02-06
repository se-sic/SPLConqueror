using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class ResultDB
    {
        private List<Configuration> configurations = new List<Configuration>();
        private IDictionary<string, List<Configuration>> configsMapping = new Dictionary<string, List<Configuration>>();
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
            string bins = calculateConfigBinsVector(config);

            List<Configuration> list = null;
            configsMapping.TryGetValue(bins, out list);

            if (list == null)
            {
                list = new List<Configuration>();
                list.Add(config);
                configsMapping.Add(bins, list);
            }
            else
                list.Add(config);
        }

        public List<Configuration> getSimilarConfigs(Configuration config)
        {
            List<Configuration> list;
            configsMapping.TryGetValue(calculateConfigBinsVector(config), out list);
            return list;
        }

        private string calculateConfigBinsVector (Configuration config)
        {
            string bins = "";

            foreach (BinaryOption opt in GlobalState.varModel.BinaryOptions)
            {
                BinaryOption.BinaryValue val;
                config.BinaryOptions.TryGetValue(opt, out val);

                if (val == BinaryOption.BinaryValue.Selected)
                    bins += "1";
                else
                    bins += "0";
            }

            return bins;
        }

        private void updateMapping()
        {
            foreach (Configuration config in configurations)
                addToMapping(config);
        }
    }
}
