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
        public static Dictionary<SamplingStrategies, Dictionary<String, String>> parametersOfExpDesigns = new Dictionary<SamplingStrategies, Dictionary<string, string>>();

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
                        binaryConfigs.AddRange(fw.generateFeatureWiseConfigsCSP(GlobalState.varModel));
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
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.BOXBEHNKEN]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.CENTRALCOMPOSITE:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.CENTRALCOMPOSITE))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.CENTRALCOMPOSITE]);
                            else
                                design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.CENTRALCOMPOSITE]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.FULLFACTORIAL:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.FULLFACTORIAL))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.FULLFACTORIAL]);
                            else
                                design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.FULLFACTORIAL]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.HYPERSAMPLING:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.HYPERSAMPLING))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.HYPERSAMPLING]);
                                else
                                    design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.HYPERSAMPLING]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.ONEFACTORATATIME:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.ONEFACTORATATIME))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.ONEFACTORATATIME]);
                        else
                            design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.ONEFACTORATATIME]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.KEXCHANGE:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.KEXCHANGE))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.KEXCHANGE]);
                        else
                            design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.KEXCHANGE]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.PLACKETTBURMAN:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.PLACKETTBURMAN))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.PLACKETTBURMAN]);
                        else
                            design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.PLACKETTBURMAN]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
                        break;

                    case SamplingStrategies.RANDOM:
                        if (optionsToConsider.ContainsKey(SamplingStrategies.RANDOM))
                            design = new BoxBehnkenDesign(optionsToConsider[SamplingStrategies.RANDOM]);
                        else
                            design = new BoxBehnkenDesign(vm.NumericOptions);
                        design.computeDesign(parametersOfExpDesigns[SamplingStrategies.RANDOM]);
                        numericConfigs.AddRange(design.SelectedConfigurations);
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
    }
}
