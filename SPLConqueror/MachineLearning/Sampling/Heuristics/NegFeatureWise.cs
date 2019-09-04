using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Sampling.Heuristics
{
    public class NegFeatureWise
    {
        private List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();

        //get one variant per feature multiplied with alternative combinations; the variant tries to maximize the number of selected features, but without the feature in question
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public List<List<BinaryOption>> generateNegativeFW(VariabilityModel vm)
        {
            this.configurations.Clear();
            List<List<BinaryOption>> maxConfigs = new List<List<BinaryOption>>();

            maxConfigs = getMaxConfigurations(vm, false);
            configurations.AddRange(maxConfigs);
            //Idea try to vary only the first maximum configuration by removing only a single feature 
            //If a feature is not present in this maximum configuration, find a maximum configuration in which it is present and then remove the feature
            //Challenges: alternative features or mandatory features cannot be removed
            foreach (BinaryOption binOpt in vm.WithAbstractBinaryOptions)
            {
                if (binOpt.Optional == false || binOpt.hasAlternatives())
                    continue;

                foreach (List<BinaryOption> config in maxConfigs)
                {
                    if (!config.Contains(binOpt))
                        continue;
                    List<BinaryOption> removedElements = null;
                    //Get a configuration without the feature based on the maximum configuration: config
                    List<BinaryOption> configToMeasure = ConfigurationBuilder.vg.GenerateConfigWithoutOption(binOpt, config, out removedElements, vm);

                    if (configToMeasure == null)
                    {//This didn't work, let us try to use another maximum configuration
                        continue;
                    }
                    else
                    {
                        if (!Configuration.containsBinaryConfiguration(configurations, configToMeasure))
                            configurations.Add(configToMeasure);
                        break;
                    }

                }
            }
            return this.configurations;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public List<List<BinaryOption>> generateNegativeFWAllCombinations(VariabilityModel vm)
        {
            this.configurations.Clear();
            List<List<BinaryOption>> maxConfigs = new List<List<BinaryOption>>();

            maxConfigs = getMaxConfigurations(vm, true);
            configurations.AddRange(maxConfigs);

            //Compute negative feature-wise for all maximum configurations
            foreach (List<BinaryOption> config in maxConfigs)
            {
                bool abort = false;
                List<BinaryOption> currentConfig = new List<BinaryOption>();
                foreach (BinaryOption e in config)
                    currentConfig.Add(e);

                List<BinaryOption> removedElements = new List<BinaryOption>();

                while (abort == false)
                {
                    abort = true;
                    BinaryOption currentElementUnderConsdiration = null;
                    foreach (BinaryOption e in currentConfig)
                    {
                        currentElementUnderConsdiration = e;
                        //Constructing new Configuration without the current element
                        List<BinaryOption> configToMeasure = new List<BinaryOption>();
                        configToMeasure = ConfigurationBuilder.vg.GenerateConfigWithoutOption(e, currentConfig, out removedElements, vm);

                        if (configToMeasure == null)
                        {
                            abort = true;
                            continue;
                        }
                        else if (!Configuration.containsBinaryConfiguration(configurations, configToMeasure))
                            configurations.Add(configToMeasure);

                    }
                }
            }
            return this.configurations;
        }

        //compute all configurations that have a maximum number of features (could be multiple, because of alternative groups)
        //allAlternativeCombinations: if false, we compute less configurations, because we do not consider all combinations of alternative features
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="allAlternativeCombinations"></param>
        /// <returns></returns>
        internal List<List<BinaryOption>> getMaxConfigurations(VariabilityModel vm, bool allAlternativeCombinations)
        {
            Dictionary<BinaryOption, List<BinaryOption>> alternatives = new Dictionary<BinaryOption, List<BinaryOption>>();
            List<List<BinaryOption>> maxConfigurations = new List<List<BinaryOption>>();
            maxConfigurations.AddRange(ConfigurationBuilder.vg.MaximizeConfig(null, vm, false, null));
            bool existAlternative = false;

            if (allAlternativeCombinations)
            {
                foreach (BinaryOption elem in vm.WithAbstractBinaryOptions)
                {
                    if (elem.hasAlternatives())
                    {
                        existAlternative = true;
                        if (elem.Parent != null && alternatives.Keys.Contains(elem.Parent))
                            alternatives[(BinaryOption)elem.Parent].Add(elem);
                        else
                        {
                            List<BinaryOption> tempL = new List<BinaryOption>();
                            tempL.Add(elem);
                            alternatives.Add((BinaryOption)elem.Parent, tempL);
                        }
                    }
                }

                //Cleaning... delete all alternatives that are parents from other alternatives
                List<BinaryOption> elemToRemove = new List<BinaryOption>();
                foreach (BinaryOption elem in alternatives.Keys)
                {
                    foreach (BinaryOption elemToCompare in alternatives.Keys)
                    {
                        if (elem == elemToCompare)
                            continue;
                        if (elem.isAncestor(elemToCompare) && elemToRemove.Contains(elem) == false)
                            elemToRemove.Add(elem);
                    }
                }

                foreach (BinaryOption e in elemToRemove)
                    alternatives.Remove(e);

                //Now, create the maximal configuration for each alternative
                List<BinaryOption> alreadyComputedConfigs = new List<BinaryOption>();
                foreach (BinaryOption e in alternatives.Keys)
                {
                    foreach (BinaryOption alter in alternatives[e])
                    {
                        List<List<BinaryOption>> foundConfigs = generateConfig(alter, alternatives, alreadyComputedConfigs, vm);
                        foreach (var conf in foundConfigs)
                        {
                            if (!Configuration.containsBinaryConfiguration(maxConfigurations, conf))
                                maxConfigurations.Add(conf);
                        }
                    }
                    alreadyComputedConfigs.Add(e);
                }
                if (!existAlternative)
                {
                    List<BinaryOption> config = new List<BinaryOption>();
                    maxConfigurations.AddRange(ConfigurationBuilder.vg.MaximizeConfig(config, vm, false, null));
                }
            }

            //Verify whether each option is at least in one maximum configuration
            foreach (BinaryOption elem in vm.WithAbstractBinaryOptions)
            {
                bool isCovered = false;
                foreach (List<BinaryOption> config in maxConfigurations)
                {
                    if (config.Contains(elem))
                    {
                        isCovered = true;
                        break;
                    }
                }
                if (!isCovered)
                {
                    List<BinaryOption> temp = new List<BinaryOption>();
                    temp.Add(elem);
                    if (!allAlternativeCombinations)
                    {
                        maxConfigurations.Add((ConfigurationBuilder.vg.MaximizeConfig(temp, vm, false, null)[0]));
                    }
                    else
                    {
                        maxConfigurations.AddRange((ConfigurationBuilder.vg.MaximizeConfig(temp, vm, false, null)));
                    }
                }
            }

            //Remove empty configs from list
            bool containsEmptyConfigs = true;
            while (containsEmptyConfigs)
            {
                containsEmptyConfigs = false;
                int index = -1;
                for (int i = 0; i < maxConfigurations.Count; i++)
                {
                    if (maxConfigurations[i].Count == 0)
                    {
                        containsEmptyConfigs = true;
                        index = i;
                        break;
                    }
                }
                if (containsEmptyConfigs)
                    maxConfigurations.RemoveAt(index);
            }
            return maxConfigurations;
        }

        private List<List<BinaryOption>> generateConfig(BinaryOption toConfigure, Dictionary<BinaryOption, List<BinaryOption>> alternatives, List<BinaryOption> alreadyComputed, VariabilityModel vm)
        {
            List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
            foreach (BinaryOption next in alternatives.Keys)
            {
                if (alreadyComputed.Contains(next))
                    continue;
                if (toConfigure.Parent == next.Parent)
                    continue;
                foreach (BinaryOption k in alternatives[next])
                {
                    List<BinaryOption> config = new List<BinaryOption>();
                    config.Add(toConfigure);
                    config.Add(k);
                    List<List<BinaryOption>> temp = new List<List<BinaryOption>>();
                    temp = ConfigurationBuilder.vg.MaximizeConfig(config, vm, false, null);
                    if (temp == null || temp.Count == 0)
                        continue;
                    config = temp[0];
                    if (config == null)
                        continue;
                    configurations.Add(config);
                }
            }
            return configurations;
        }

    }
}
