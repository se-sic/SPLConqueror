using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using MachineLearning.Solver;
using MachineLearning.Sampling.Heuristics;
using MachineLearning.Sampling.ExperimentalDesigns;

namespace MachineLearning.Sampling
{
    public class ConfigurationBuilder
    {
        public static int binaryThreshold = 0;
        public static int binaryModulu = 0;
        public static Dictionary<SamplingStrategies, List<NumericOption>> optionsToConsider = new Dictionary<SamplingStrategies, List<NumericOption>>();
        public static Dictionary<SamplingStrategies, List<Dictionary<String, String>>> parametersOfExpDesigns = new Dictionary<SamplingStrategies, List<Dictionary<string, string>>>();

        public static List<Configuration> buildConfigs(VariabilityModel vm, List<SamplingStrategies> strategies)
        {
            List<Configuration> result = new List<Configuration>();
            VariantGenerator vg = new VariantGenerator(null);
            ExperimentalDesign design = null;

            List<List<BinaryOption>> binaryConfigs = new List<List<BinaryOption>>();
            List<Dictionary<NumericOption, Double>> numericConfigs = new List<Dictionary<NumericOption, double>>();
            foreach (SamplingStrategies strat in strategies)
            {
                switch (strat)
                {
                    //Binary sampling heuristics
                    case SamplingStrategies.ALLBINARY:
                        binaryConfigs.AddRange(vg.generateAllVariantsFast(vm));
                        break;
                    case SamplingStrategies.BINARY_RANDOM:
                        binaryConfigs.AddRange(vg.generateRandomVariants(GlobalState.varModel, binaryThreshold, binaryModulu));
                        break;
                    case SamplingStrategies.OPTIONWISE:
                        FeatureWise fw = new FeatureWise();
                        binaryConfigs.AddRange(fw.generateFeatureWiseConfigurations(GlobalState.varModel));
                        break;
                    case SamplingStrategies.PAIRWISE:
                        PairWise pw = new PairWise();
                        binaryConfigs.AddRange(pw.generatePairWiseVariants(GlobalState.varModel));
                        break;
                    case SamplingStrategies.NEGATIVE_OPTIONWISE:
                        NegFeatureWise neg = new NegFeatureWise();//2nd option: neg.generateNegativeFWAllCombinations(GlobalState.varModel));
                        binaryConfigs.AddRange(neg.generateNegativeFW(GlobalState.varModel));
                        break;

                    //Experimental designs for numeric options
                    case SamplingStrategies.BOXBEHNKEN:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.BOXBEHNKEN))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.BOXBEHNKEN]);
                        else
                            design = new BoxBehnkenDesign(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.BOXBEHNKEN])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }
                        break;

                    case SamplingStrategies.CENTRALCOMPOSITE:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.CENTRALCOMPOSITE))
                            design = new CentralCompositeInscribedDesign(optionsToConsider[SamplingStrategies.CENTRALCOMPOSITE]);
                        else
                            design = new CentralCompositeInscribedDesign(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.CENTRALCOMPOSITE])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }   
                        break;

                    case SamplingStrategies.FULLFACTORIAL:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.FULLFACTORIAL))
                            design = new FullFactorialDesign(optionsToConsider[SamplingStrategies.FULLFACTORIAL]);
                        else
                            design = new FullFactorialDesign(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.FULLFACTORIAL])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }   

                        break;

                    case SamplingStrategies.HYPERSAMPLING:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.HYPERSAMPLING))
                            design = new HyperSampling(optionsToConsider[SamplingStrategies.HYPERSAMPLING]);
                        else
                            design = new HyperSampling(vm.NumericOptions);
                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.HYPERSAMPLING])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }   
                        break;

                    case SamplingStrategies.ONEFACTORATATIME:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.ONEFACTORATATIME))
                            design = new OneFactorAtATime(optionsToConsider[SamplingStrategies.ONEFACTORATATIME]);
                        else
                            design = new OneFactorAtATime(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.ONEFACTORATATIME])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }
                        break;

                    case SamplingStrategies.KEXCHANGE:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.KEXCHANGE))
                            design = new KExchangeAlgorithm(optionsToConsider[SamplingStrategies.KEXCHANGE]);
                        else
                            design = new KExchangeAlgorithm(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.KEXCHANGE])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }
                        break;

                    case SamplingStrategies.PLACKETTBURMAN:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.PLACKETTBURMAN))
                            design = new PlackettBurmanDesign(optionsToConsider[SamplingStrategies.PLACKETTBURMAN]);
                        else
                            design = new PlackettBurmanDesign(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.PLACKETTBURMAN])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }    
                        break;

                    case SamplingStrategies.RANDOM:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.RANDOM))
                            design = new RandomSampling(optionsToConsider[SamplingStrategies.RANDOM]);
                        else
                            design = new RandomSampling(vm.NumericOptions);

                        foreach (Dictionary<string, string> expDesignParamSet in parametersOfExpDesigns[SamplingStrategies.RANDOM])
                        {
                            design.computeDesign(expDesignParamSet);
                            numericConfigs.AddRange(design.SelectedConfigurations);
                        }    
                        break;
                }
            }

            foreach (List<BinaryOption> binConfig in binaryConfigs)
            {
                if (numericConfigs.Count == 0)
                {
                    Configuration c = new Configuration(binConfig);
                    result.Add(c);
                }
                foreach (Dictionary<NumericOption, double> numConf in numericConfigs)
                {
                    Configuration c = new Configuration(binConfig, numConf);
                    result.Add(c);
                }
            }

            return result.Distinct().ToList();
        }

        public static void printSelectetedConfigurations_expDesign(List<Dictionary<NumericOption, double>> configurations)
        {
            GlobalState.varModel.NumericOptions.ForEach(x => GlobalState.logInfo.log(x.Name+" | "));
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
