using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SPLConqueror_Core;

namespace MachineLearning.Solver
{
    public interface IVariantGenerator
    {
        /// <summary>
        /// Generates all valid binary combinations of all binary configurations options in the given model
        /// </summary>
        /// <param name="vm">The variability model containing the binary options and their constraints.</param>
        /// <returns>Returns a list of configurations, in which a configuration is a list of SELECTED binary options (deselected options are not present)</returns>
        List<List<BinaryOption>> GenerateAllVariantsFast(VariabilityModel vm);

        /// <summary>
        /// Generates up to n solutions of the given variability model. 
        /// Note that this method could also generate less than n solutions if the variability model does not contain sufficient solutions.
        /// Moreover, in the case that <code>n &lt; 0</code>, all solutions are generated.
        /// </summary>
        /// <param name="vm">The <see cref="VariabilityModel"/> to obtain solutions for.</param>
        /// <param name="n">The number of solutions to obtain.</param>
        /// <returns>A list of configurations, in which a configuration is a list of SELECTED binary options.</returns>
        List<List<BinaryOption>> GenerateUpToNFast(VariabilityModel vm, int n);


        /// <summary>
        /// Simulates a simple method to get valid configurations of binary options of a variability model. The randomness is simulated by the modulu value.
        /// We take only the modulu'th configuration into the result set based on the CSP solvers output.
        /// </summary>
        /// <param name="vm">The variability model containing the binary options and their constraints.</param>
        /// <param name="treshold">Maximum number of configurations</param>
        /// <param name="modulu">Each configuration that is % modulu == 0 is taken to the result set</param>
        /// <returns>Returns a list of configurations, in which a configuration is a list of SELECTED binary options (deselected options are not present</returns>
        List<List<BinaryOption>> GenerateRandomVariants(VariabilityModel vm, int treshold, int modulu);
        

        //Configuration size
        /// <summary>
        /// Based on a given (partial) configuration and a variability, we aim at finding the smallest (or largest if minimize == false) valid configuration that has all options.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unWantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them.</param>
        /// <returns>The valid configuration (or null if there is none) that satisfies the VM and the goal.</returns>
        List<BinaryOption> MinimizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unWantedOptions);

        /// <summary>
        /// Based on a given (partial) configuration and a variability, we aim at finding all optimally maximal or minimal (in terms of selected binary options) configurations.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unwantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them</param>
        /// <returns>A list of configurations that satisfies the VM and the goal (or null if there is none).</returns>
        List<List<BinaryOption>> MaximizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unwantedOptions);

        /// <summary>
        /// The method aims at finding a configuration which is similar to the given configuration, but does not contain the optionToBeRemoved. If further options need to be removed from the given configuration, they are outputed in removedElements.
        /// </summary>
        /// <param name="optionToBeRemoved">The binary configuration option that must not be part of the new configuration.</param>
        /// <param name="originalConfig">The configuration for which we want to find a similar one.</param>
        /// <param name="removedElements">If further options need to be removed from the given configuration to build a valid configuration, they are outputed in this list.</param>
        /// <param name="vm">The variability model containing all options and their constraints.</param>
        /// <returns>A configuration that is valid, similar to the original configuration and does not contain the optionToBeRemoved.</returns>
        List<BinaryOption> GenerateConfigWithoutOption(BinaryOption optionToBeRemoved, List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm);

        /// <summary>
        /// This method returns a configuration, which is minimal according to the given featureweight.
        /// </summary>
        /// <param name="vm">The variability model containing all options and their constraints.</param>
        /// <param name="numberSelectedFeatures">The number of features that should be selected.</param>
        /// <param name="featureWeight">The weight of every feature.</param>
        /// <param name="lastSampledConfiguration">The last included sampled configuration.</param>
        /// <returns>A list of <see cref="BinaryOption"/>, which should be selected.</returns>
        List<BinaryOption> WeightMinimization(VariabilityModel vm, int numberSelectedFeatures, Dictionary<BinaryOption, int> featureWeight, Configuration lastSampledConfiguration);
    }
}
