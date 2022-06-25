using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SPLConqueror_Core;
using MachineLearning.Solver;
using MachineLearning.Sampling.Heuristics;
using MachineLearning.Sampling.ExperimentalDesigns;
using MachineLearning.Sampling.Hybrid;

namespace MachineLearning.Sampling
{
    public class ConfigurationBuilder
    {
        public static int binaryThreshold = 0;
        public static int binaryModulu = 0;
        public static Dictionary<SamplingStrategies, List<List<BinaryOption>>> optionsToConsider = 
            new Dictionary<SamplingStrategies, List<List<BinaryOption>>>();
        public static BinaryParameters binaryParams = new BinaryParameters();

        // The default variant generator is the one using the CSP solver of the Microsoft solver foundation
        public static IVariantGenerator vg = new VariantGenerator();

        private static List<String> blacklisted;

        public static void setBlacklisted(List<String> blacklist)
        {
            ConfigurationBuilder.blacklisted = blacklist;
        }

        public static void clear()
        {
            binaryModulu = 0;
            binaryThreshold = 0;
            optionsToConsider = new Dictionary<SamplingStrategies, List<List<BinaryOption>>>();
            binaryParams = new BinaryParameters();
        }

        public static List<Configuration> buildConfigs(VariabilityModel vm, List<SamplingStrategies> binaryStrategies,
            List<ExperimentalDesign> experimentalDesigns, List<HybridStrategy> hybridStrategies)
        {
            List<Configuration> result = new List<Configuration>();

            List<List<BinaryOption>> binaryConfigs = new List<List<BinaryOption>>();
            List<List<List<BinaryOption>>> binaryConfigsFromConsider = new List<List<List<BinaryOption>>>();
            List<Dictionary<NumericOption, Double>> numericConfigs = new List<Dictionary<NumericOption, double>>();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (SamplingStrategies strat in binaryStrategies)
            {
                switch (strat)
                {
                    //Binary sampling heuristics
                    case SamplingStrategies.ALLBINARY:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.ALLBINARY))
                        {
                            foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.ALLBINARY])
                            {
                                List<List<BinaryOption>> variants =
                                    vg.GenerateAllVariantsFast(vm.reduce(options));
                                binaryConfigsFromConsider.Add(changeModel(vm, variants));
                            }
                        }
                        else
                        {
                            binaryConfigs.AddRange(vg.GenerateAllVariantsFast(vm));
                        }
                        break;
                    case SamplingStrategies.SAT:
                        int numberSamples = 2;
                        foreach (Dictionary<string, string> parameters in binaryParams.satParameters)
                        {
                            if (parameters.ContainsKey("henard"))
                            {
                                try
                                {
                                    bool b = Boolean.Parse(parameters["henard"]);
                                    ((Z3VariantGenerator)vg).henard = b;
                                }
                                catch (FormatException e)
                                {
                                    Console.Error.WriteLine(e);
                                }
                            }
                            if (parameters.ContainsKey("numConfigs"))
                            {
                                try
                                {
                                    numberSamples = Int32.Parse(parameters["numConfigs"]);
                                }
                                catch (FormatException)
                                {
                                    TWise tw = new TWise();
                                    numberSamples = tw.generateT_WiseVariants_new(GlobalState.varModel, Int32.Parse(parameters["numConfigs"].Remove(0, 4))).Count;
                                }
                            }

                            if (parameters.ContainsKey("seed") && vg is Z3VariantGenerator)
                            {
                                uint seed = 0;
                                seed = UInt32.Parse(parameters["seed"]);
                                ((Z3VariantGenerator)vg).setSeed(seed);
                            }
                            if (optionsToConsider.ContainsKey(SamplingStrategies.SAT))
                            {
                                foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.SAT])
                                {
                                    List<List<BinaryOption>> variants =
                                        vg.GenerateUpToNFast(vm.reduce(options), numberSamples);
                                    binaryConfigs.AddRange(changeModel(vm, variants));
                                }
                            }
                            else
                            {
                                binaryConfigs.AddRange(vg.GenerateUpToNFast(vm, numberSamples));
                            }
                            numberSamples = 2;
                        }
                        break;
                    case SamplingStrategies.BINARY_RANDOM:
                        List<RandomBinary> rb = new List<RandomBinary>();
                        if (optionsToConsider.ContainsKey(SamplingStrategies.BINARY_RANDOM))
                        {
                            foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.BINARY_RANDOM])
                            {
                                rb.Add(new RandomBinary(vm.reduce(options)));
                            }

                        }
                        else
                        {
                            rb.Add(new RandomBinary(vm));
                        }
                        
                        for (int i = 0; i < binaryParams.randomBinaryParameters.Count; i++)
                        {
                            Dictionary<string, string> expDesignParamSet = binaryParams.randomBinaryParameters[i];
                            if (rb.Count > 0)
                            {
                                binaryConfigsFromConsider.Add(changeModel(vm, rb[i].getRandomConfigs(expDesignParamSet)));
                            }
                        }

                        break;
                    case SamplingStrategies.OPTIONWISE:
                        {
                            FeatureWise fw = new FeatureWise();
                            if (optionsToConsider.ContainsKey(SamplingStrategies.OPTIONWISE))
                            {
                                foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.OPTIONWISE])
                                {
                                    List<List<BinaryOption>> variants = fw.generateFeatureWiseConfigurations(
                                        GlobalState.varModel.reduce(options));
                                    binaryConfigsFromConsider.Add(changeModel(vm, variants));
                                }
                            }
                            else
                            {
                                binaryConfigs.AddRange(fw.generateFeatureWiseConfigurations(GlobalState.varModel));
                            }
                        }
                        break;
                    case SamplingStrategies.DISTANCE_BASED:
                        foreach (Dictionary<string, string> parameters in binaryParams.distanceMaxParameters)
                        {
                            DistanceBased distSampling = new DistanceBased(vm);
                            binaryConfigsFromConsider.Add(distSampling.getSample(parameters));
                        }
                        break;

                    //case SamplingStrategies.MINMAX:
                    //    {
                    //        MinMax mm = new MinMax();
                    //        binaryConfigs.AddRange(mm.generateMinMaxConfigurations(GlobalState.varModel));

                    //    }
                    //    break;

                    case SamplingStrategies.PAIRWISE:
                        {
                            PairWise pw = new PairWise();
                            if (optionsToConsider.ContainsKey(SamplingStrategies.PAIRWISE))
                            {
                                foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.PAIRWISE])
                                {
                                    List<List<BinaryOption>> variants = pw.generatePairWiseVariants(
                                        GlobalState.varModel.reduce(options));
                                    binaryConfigsFromConsider.Add(changeModel(vm, variants));
                                }
                            }
                            else
                            {
                                binaryConfigs.AddRange(pw.generatePairWiseVariants(GlobalState.varModel));
                            }
                        }
                        break;
                    case SamplingStrategies.NEGATIVE_OPTIONWISE:
                        {
                            NegFeatureWise neg = new NegFeatureWise();//2nd option: neg.generateNegativeFWAllCombinations(GlobalState.varModel));
                            if (optionsToConsider.ContainsKey(SamplingStrategies.NEGATIVE_OPTIONWISE))
                            {
                                foreach (List<BinaryOption> options in optionsToConsider[SamplingStrategies.NEGATIVE_OPTIONWISE])
                                {
                                    List<List<BinaryOption>> variants = neg.generateNegativeFW(
                                        GlobalState.varModel.reduce(options));
                                    binaryConfigsFromConsider.Add(changeModel(vm, variants));
                                }
                            }
                            else
                            {
                                binaryConfigs.AddRange(neg.generateNegativeFW(GlobalState.varModel));
                            }
                        }
                        break;

                    case SamplingStrategies.T_WISE:
                        foreach (Dictionary<string, string> ParamSet in binaryParams.tWiseParameters)
                        {
                            TWise tw = new TWise();
                            int t = 3;

                            foreach (KeyValuePair<String, String> param in ParamSet)
                            {
                                if (param.Key.Equals(TWise.PARAMETER_T_NAME))
                                {
                                    t = Convert.ToInt16(param.Value);
                                }

                                if (optionsToConsider.ContainsKey(SamplingStrategies.T_WISE))
                                {
                                    List<BinaryOption> options = optionsToConsider[SamplingStrategies.T_WISE][0];
                                    optionsToConsider[SamplingStrategies.T_WISE].RemoveAt(0);
                                    List<List<BinaryOption>> variants = tw.generateT_WiseVariants_new(
                                        vm.reduce(options), t);
                                    binaryConfigsFromConsider.Add(changeModel(vm, variants));
                                }
                                else
                                {
                                    binaryConfigs.AddRange(tw.generateT_WiseVariants_new(vm, t));
                                }
                            }
                        }
                        break;
                }
            }
            sw.Stop();
            
            if (binaryConfigsFromConsider.Count != 0)
            {
                var configurations = computeCartesianProduct(binaryConfigsFromConsider);
                binaryConfigs = configurations;
            }
            
            //Experimental designs for numeric options
            if (experimentalDesigns.Count != 0)
            {
                sw.Start();
                handleDesigns(experimentalDesigns, numericConfigs, vm);
                sw.Stop();
            }

            if (vm.NumericOptions.Any(x => x.Optional))
            {
                createConfigurationsOptNumeric(vm, binaryConfigs, numericConfigs, result);
            } else
            {
                createConfigurations(binaryConfigs, numericConfigs, result);
            }
            
            result = filterNonBoolean(result, vm);
            
            // Create a string -> binary option mapping for the cartesian product
            Dictionary<String, BinaryOption> binOptMap = new Dictionary<string, BinaryOption>();
            foreach (BinaryOption binOpt in GlobalState.varModel.BinaryOptions)
            {
                binOptMap[binOpt.Name] = binOpt;
            }

            // Hybrid designs
            if (hybridStrategies.Count != 0)
            {
                sw.Start();
                List<List<Configuration>> configurations = ExecuteHybridStrategy(hybridStrategies, vm);
                sw.Stop();
                
                // Prepare the previous sample sets
                if (result.Count == 0 && binaryConfigs.Count == 0)
                {
                    if (numericConfigs.Count != 0)
                    {
                        foreach (Dictionary<NumericOption, double> numConf in numericConfigs)
                        {
                            Configuration c =
                                new Configuration(new Dictionary<BinaryOption, BinaryOption.BinaryValue>(), numConf);
                            result.Add(c);
                        }
                    } else
                    {
                        Configuration config = new Configuration(new List<BinaryOption>());
                        foreach (Configuration configuration in configurations[0])
                        {
                            mergeConfigurations(config, configuration, binOptMap, out var binOpts, out var numOpts);
                            result.Add(new Configuration(binOpts, numOpts)); 
                        }
                        configurations.RemoveAt(0);
                    }
                }

                // Build the cartesian product
                List<Configuration> newResult = new List<Configuration>();
                foreach (Configuration config in result)
                {
                    foreach (List<Configuration> hybridConfigurations in configurations)
                    {
                        foreach (Configuration hybridConfiguration in hybridConfigurations)
                        {
                            mergeConfigurations(config, hybridConfiguration, binOptMap, out var binOpts, out var numOpts);
                            newResult.Add(new Configuration(binOpts, numOpts));
                        }
                    }
                    
                    result = newResult;
                }
            }
            
            // Print the time needed for sampling
            GlobalState.logInfo.logLine("Total sampling time=" + sw.Elapsed);

            // Filter the invalid configurations
            List<int> invalidConfigurations = new List<int>();
            // We strictly use z3 solver here
            CheckConfigSATZ3 configurationChecker = new CheckConfigSATZ3();
            result = result.Distinct().ToList();
            for (int i = 0; i < result.Count(); i++)
            {
                Configuration config = result[i];
                if (!configurationChecker.checkConfigurationSAT(config, GlobalState.varModel))
                {
                    invalidConfigurations.Add(i);
                }
            }

            for (int k = invalidConfigurations.Count()-1; k >= 0; k--)
            {
                int i = invalidConfigurations[k];
                result.RemoveAt(i);
            }

            GlobalState.logInfo.logLine("Invalid configurations generated during sampling and filtered out: " 
                                        + invalidConfigurations.Count);

            if (vm.MixedConstraints.Count == 0)
            {
                if (binaryStrategies.Count == 1 && binaryStrategies.Last().Equals(SamplingStrategies.ALLBINARY) && experimentalDesigns.Count == 1 && experimentalDesigns.Last() is FullFactorialDesign)
                {
                    return replaceReference(result.ToList());
                }
                else
                {
                    return replaceReference(result.Distinct().ToList());
                }
            }
            else
            {
                List<Configuration> unfilteredList = result.Distinct().ToList();
                List<Configuration> filteredConfiguration = new List<Configuration>();
                foreach (Configuration toTest in unfilteredList)
                {
                    bool isValid = true;
                    foreach (MixedConstraint constr in vm.MixedConstraints)
                    {
                        if (!constr.requirementsFulfilled(toTest))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        filteredConfiguration.Add(toTest);
                    }
                }
                return replaceReference(filteredConfiguration);
            }
        }

        private static void mergeConfigurations(Configuration config, Configuration hybridConfiguration, Dictionary<string, BinaryOption> binOptMap,
            out Dictionary<BinaryOption, BinaryOption.BinaryValue> binOpts, out Dictionary<NumericOption, double> numOpts)
        {
            binOpts = new Dictionary<BinaryOption, BinaryOption.BinaryValue>(config.BinaryOptions);
            numOpts = new Dictionary<NumericOption, double>(config.NumericOptions);

            Dictionary<BinaryOption, BinaryOption.BinaryValue> hybridBinOpts =
                hybridConfiguration.BinaryOptions;

            foreach (BinaryOption binOpt in hybridConfiguration.BinaryOptions.Keys)
            {
                if (!binOpts.ContainsKey(binOptMap[binOpt.Name]))
                {
                    binOpts.Add(binOptMap[binOpt.Name], hybridBinOpts[binOpt]);
                }
            }

            Dictionary<NumericOption, double> hybridNumOpts = hybridConfiguration.NumericOptions;
            foreach (NumericOption numOpt in hybridConfiguration.NumericOptions.Keys)
            {
                numOpts.Add(numOpt, hybridNumOpts[numOpt]);
            }
        }

        private static List<List<BinaryOption>> computeCartesianProduct(List<List<List<BinaryOption>>> binaryConfigsFromConsider)
        {
            // Compute the cartesian product
            List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
            foreach (List<List<BinaryOption>> sampleSet in binaryConfigsFromConsider)
            {
                if (configurations.Count == 0)
                {
                    configurations.AddRange(sampleSet);
                }
                else
                {
                    List<List<BinaryOption>> tmpConfigurations = configurations;
                    configurations = new List<List<BinaryOption>>();
                    foreach (List<BinaryOption> configurationToExpand in tmpConfigurations)
                    {
                        foreach (List<BinaryOption> expandingConfiguration in sampleSet)
                        {
                            List<BinaryOption> newConfig = new List<BinaryOption>(configurationToExpand);
                            foreach (BinaryOption binOpt in expandingConfiguration)
                            {
                                if (!newConfig.Contains(binOpt))
                                {
                                    newConfig.Add(binOpt);
                                }
                            }

                            configurations.Add(newConfig);
                        }
                    }
                }
            }

            return configurations;
        }

        private static void createConfigurations(List<List<BinaryOption>> binaryParts, List<Dictionary<NumericOption, Double>> numericParts, List<Configuration> results)
        {
            foreach (List<BinaryOption> binConfig in binaryParts)
            {
                if (numericParts.Count == 0)
                {
                    Configuration c = new Configuration(binConfig);
                    results.Add(c);
                }
                foreach (Dictionary<NumericOption, double> numConf in numericParts)
                {
                    Configuration c = new Configuration(binConfig, numConf);
                    results.Add(c);
                }
            }
        }

        private static void createConfigurationsOptNumeric(VariabilityModel vm, List<List<BinaryOption>> binaryParts, List<Dictionary<NumericOption, Double>> numericParts, List<Configuration> results)
        {
            IEnumerable<NumericOption> optionalOptions = vm.NumericOptions.Where(x => x.Optional);
            foreach (List<BinaryOption> binConfig in binaryParts)
            {
                // Get all abstract binary options
                var abstractOptions = binConfig.Where(x => x.IsStrictlyAbstract);

                // get all currently deselected numeric options
                List<NumericOption> currentDeselected = optionalOptions
                    .Where(x => !abstractOptions.Contains(x.abstractEnabledConfigurationOption())).ToList();
                currentDeselected.ForEach(x => binConfig.Add(x.abstractDisabledConfigurationOption()));

                List<String> alreadyAdded = new List<String>();
                foreach (Dictionary<NumericOption, double> numConf in numericParts)
                {
                    Dictionary<NumericOption, double> buff = new Dictionary<NumericOption, double>();
                    numConf.ToList().ForEach(x => { buff[x.Key] = currentDeselected.Contains(x.Key) ? x.Key.OptionalFlag : x.Value; });
                    var selectedPart = String.Join(";", buff.Select(x => x.Value));
                    if (!alreadyAdded.Contains(selectedPart))
                    {
                        alreadyAdded.Add(selectedPart);
                    } else 
                    {
                        continue;
                    }
                    Configuration c = new Configuration(binConfig, buff);
                    results.Add(c);
                }
            }
        }

        private static List<Configuration> filterNonBoolean(List<Configuration> intermediateResults, VariabilityModel vm)
        {
            // Filter configurations based on the NonBooleanConstratins
            List<Configuration> filtered = new List<Configuration>();
            foreach (Configuration conf in intermediateResults)
            {
                bool isValid = true;
                foreach (NonBooleanConstraint nbc in vm.NonBooleanConstraints)
                {
                    if (!nbc.configIsValid(conf))
                    {
                        isValid = false;
                        continue;
                    }
                }

                if (isValid)
                    filtered.Add(conf);
            }
            return filtered;
        }

        private static List<Configuration> replaceReference(List<Configuration> sampled)
        {
            // Replaces the reference of the sampled configuration with the corresponding measured configuration if it exists
            if (GlobalState.varModel.AbrstactOptions.Count > 0)
            {

            } else { }
            var measured = GlobalState.allMeasurements.Configurations.Intersect(sampled);
            var notMeasured = sampled.Except(measured);
            return measured.Concat(notMeasured).ToList();
        }

        private static List<List<Configuration>> ExecuteHybridStrategy(List<HybridStrategy> hybridStrategies, VariabilityModel vm)
        {
            List<List<Configuration>> allSampledConfigurations = new List<List<Configuration>>();
            foreach (HybridStrategy hybrid in hybridStrategies)
            {
                hybrid.ComputeSamplingStrategy();
                allSampledConfigurations.Add(hybrid.selectedConfigurations);
            }
            return allSampledConfigurations;
        }

        private static List<List<BinaryOption>> changeModel(VariabilityModel vm, List<List<BinaryOption>> variants)
        {
            List<List<BinaryOption>> toReturn = new List<List<BinaryOption>>();

            foreach (List<BinaryOption> variant in variants)
            {
                List<BinaryOption> variantInRightModel = new List<BinaryOption>();

                foreach (BinaryOption opt in variant)
                {
                    variantInRightModel.Add(vm.getBinaryOption(opt.Name));
                }

                toReturn.Add(variantInRightModel);
            }

            return toReturn;
        }

        private static void handleDesigns(List<ExperimentalDesign> samplingDesigns, List<Dictionary<NumericOption, Double>> numericOptions,
            VariabilityModel vm)
        {
            foreach (ExperimentalDesign samplingDesign in samplingDesigns)
            {
                if (samplingDesign.getSamplingDomain() == null ||
                    samplingDesign.getSamplingDomain().Count == 0)
                {
                    samplingDesign.setSamplingDomain(vm.getNonBlacklistedNumericOptions(blacklisted));
                }
                else
                {
                    samplingDesign.setSamplingDomain(vm.getNonBlacklistedNumericOptions(blacklisted)
                        .Intersect(samplingDesign.getSamplingDomain()).ToList());
                }

                samplingDesign.computeDesign();
                numericOptions.AddRange(samplingDesign.SelectedConfigurations);
            }
        }

        public static void printSelectetedConfigurations_expDesign(List<Dictionary<NumericOption, double>> configurations)
        {
            GlobalState.varModel.NumericOptions.ForEach(x => GlobalState.logInfo.log(x.Name + " | "));
            GlobalState.logInfo.log("\n");
            foreach (Dictionary<NumericOption, double> configuration in configurations)
            {
                GlobalState.varModel.NumericOptions.ForEach(x =>
                {
                    if (configuration.ContainsKey(x))
                        GlobalState.logInfo.log(configuration[x] + " | ");
                    else
                        GlobalState.logInfo.log("\t | ");
                });
                GlobalState.logInfo.log("\n");
            }
        }
    }
}
