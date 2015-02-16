﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class Configuration : IEquatable<Configuration>
    {

        private static String DEFAULT_SEPARATOR = "%;%";

        private Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptions = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

        /// <summary>
        /// Binary options of this configuration. For each option it is stored whether the option is selceted or deselected in this configuration.
        /// </summary>
        public Dictionary<BinaryOption, BinaryOption.BinaryValue> BinaryOptions
        {
            get { return binaryOptions; }
        }

        /// <summary>
        /// Numeric option of this configuration. The selected value for each numeric option is stored in the value. 
        /// </summary>
        public Dictionary<NumericOption, double> NumericOptions
        {
            get { return numericOptions; }
        }

        private Dictionary<NumericOption, double> numericOptions = new Dictionary<NumericOption, double>();

        private Dictionary<NFProperty, double> nfpValues = new Dictionary<NFProperty, double>();

        private string identifier;


        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection">A valid set of binary options.</param>
        /// <param name="numericSelection">A valid selection of values of numeric options.</param>
        /// <param name="measuremements">Measured values of non functional properties, such as performance or footprint.</param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection, Dictionary<NFProperty, double> measuremements)
        {
            binaryOptions = enrichBinarySelection(binarySelection);
            numericOptions = numericSelection;
            nfpValues = measuremements;
            identifier = generateIdentifier(DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection">A valid set of binary options.</param>
        /// <param name="numericSelection">A valid selection of values of numeric options.</param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection)
        {
            binaryOptions = enrichBinarySelection(binarySelection);
            numericOptions = numericSelection;
            identifier = generateIdentifier(DEFAULT_SEPARATOR);
        }


        public void update()
        {
            identifier = generateIdentifier(DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Constructor to create a new configuration based on selected binary options.
        /// </summary>
        /// <param name="binaryConfig">A list of SELECTED binary configuration options</param>
        /// <param name="vm">The variability model containing the remaining binary options. Can be null.</param>
        public Configuration(List<BinaryOption> binaryConfig, VariabilityModel vm)
        {
            if (vm == null)
            {
                foreach (BinaryOption opt in binaryConfig)
                {
                    binaryOptions.Add(opt, BinaryOption.BinaryValue.Selected);
                }
            }
            else
            {
                foreach (var opt in vm.BinaryOptions)
                {
                    if (binaryConfig.Contains(opt))
                        this.binaryOptions.Add(opt, BinaryOption.BinaryValue.Selected);
                    else
                        this.binaryOptions.Add(opt, BinaryOption.BinaryValue.Deselected);
                }
            }
            identifier = generateIdentifier(DEFAULT_SEPARATOR);
        }


        /// <summary>
        /// Returns an identifier describing the choice of binary configuration options and numeric configuration-option values of the configuration. 
        /// The default separator is used in the identifier. 
        /// </summary>
        /// <returns>The identifier for that configuration.</returns>
        public string getIdentifier()
        {
            return identifier;
        }



        /// <summary>
        /// Returns the identifier describing the choice of binary configuration options and numeric configuration-option values of the configuration. 
        /// The separator is used in the identifier. 
        /// </summary>
        /// <param name="separator">The configuration options </param>
        /// <returns>the identifier with the specified separator</returns>
        public string generateIdentifier(String separator)
        {
            // sort binary features by name
            var binarySelection = binaryOptions.Keys.ToList();
            binarySelection.Sort();

            // sort numeric features by name
            var numericSelection = numericOptions.Keys.ToList();
            numericSelection.Sort();

            StringBuilder sb = new StringBuilder();

            foreach (BinaryOption binary in binarySelection)
            {
                if(binaryOptions[binary].Equals(BinaryOption.BinaryValue.Selected))
                    sb.Append(binary.Name+ separator);
            }

            foreach (NumericOption numeric in numericSelection)
            {
                sb.Append(numeric.Name+ numericOptions[numeric] + separator);
            }
            


            return sb.ToString();

        }

        /// <summary>
        /// The list of binary features is enriched with all features existing in the variability model. The value of the features being enriched is the default value of the specific features. 
        /// </summary>
        /// <param name="binarySelection"></param>
        /// <returns></returns>
        private Dictionary<BinaryOption, BinaryOption.BinaryValue> enrichBinarySelection(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection)
        {
            Dictionary<BinaryOption, BinaryOption.BinaryValue> options = new Dictionary<BinaryOption,BinaryOption.BinaryValue>();

            foreach(BinaryOption bOption in GlobalState.varModel.BinaryOptions){
                if (binarySelection.ContainsKey(bOption))
                {
                    options.Add(bOption, binarySelection[bOption]);
                }
                else
                {
                    options.Add(bOption, bOption.DefaultValue);
                }
            }
            return options;
        }

        /// <summary>
        /// This method returns the configuration specific value of the current nf-property or null of the configuration has no value for the current nfp.
        /// </summary>
        /// <returns></returns>
        public double GetNFPValue()
        {
            if (!nfpValues.ContainsKey(GlobalState.currentNFP))
                throw new ArgumentException("NFP not defined for configuration.");

            return nfpValues[GlobalState.currentNFP];
        }

        /// <summary>
        /// This method returns the configuration specific value of the property. 
        /// </summary>
        /// <param name="property">A non functional property.</param>
        /// <returns>The value of the property stored in the configuration.</returns>
        public double GetNFPValue(NFProperty property)
        {
            return nfpValues[property];
        }

        /// <summary>
        /// String representing the configuration. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return identifier;
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public int CompareTo(Configuration other)
        {
            return this.identifier.CompareTo(other.identifier);
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public override bool Equals(Object other)
        {
            if (other == null)
                return false;

            return this.Equals((Configuration)other);
        }

        /// <summary>
        /// Compares one configuration with an other configuration. The identifiers of the configurations are used in the comparison. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public bool Equals(Configuration other)
        {
            return this.identifier.Equals(other.identifier);
        }

        /// <summary>
        /// This method returns a list of binary options that comply to the given configuration value.
        /// </summary>
        /// <param name="binaryValue">The configuration value ((DE-)Selected).</param>
        /// <returns>The list of binary options according to the given value.</returns>
        public List<BinaryOption> getBinaryOptions(BinaryOption.BinaryValue binaryValue)
        {
            List<BinaryOption> result = new List<BinaryOption>();

            foreach(KeyValuePair<BinaryOption, BinaryOption.BinaryValue> bin in this.binaryOptions)
                if(bin.Value.Equals(binaryValue))
                    result.Add(bin.Key);

            return result;
        }

        /// <summary>
        /// Generates a new Configuration based on the given selected binary Options and the set values of the numeric options.
        /// However, it has not yet any measured values.
        /// </summary>
        /// <param name="selectedBinaryOptions">A list of SELECTED binary options.</param>
        /// <param name="numericOptions">A map of numeric option and their corresponding value.</param>
        /// <returns>A new configuration objects with the given selections</returns>
        public static Configuration getConfiguration(List<BinaryOption> selectedBinaryOptions, Dictionary<NumericOption, double> numericOptions)
        {
            Configuration result = new Configuration(selectedBinaryOptions, null);
            result.numericOptions = numericOptions;
            result.update();
            return result;
        }


        /// <summary>
        /// Generates a new Configuration based on the given selected binary Options and the set values of the numeric options.
        /// However, it has not yet any measured values.
        /// </summary>
        /// <param name="selectedBinaryOptions">A dictionary of binary options with the information whether option is selected or deselected.</param>
        /// <param name="numericOptions">A map of numeric option and their corresponding value.</param>
        /// <returns>A new configuration objects with the given selections</returns>
        public static Configuration getConfiguration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptions, Dictionary<NumericOption, double> numericOptions)
        {
            List<BinaryOption> selectedBinaryOptions = new List<BinaryOption>();
            foreach (KeyValuePair<BinaryOption, BinaryOption.BinaryValue> binary in binaryOptions)
            {
                if (binary.Value == BinaryOption.BinaryValue.Selected)
                    selectedBinaryOptions.Add(binary.Key);
            }


            Configuration result = new Configuration(selectedBinaryOptions, null);
            result.numericOptions = numericOptions;
            result.update();
            return result;
        }

        /// <summary>
        /// This method evaluates whether the list of binary selections contains a list of binary configuration options.
        /// </summary>
        /// <param name="setOfBinaryConfigurations">A list of binary selections. Each sublist represents a selection of binary configuration options.</param>
        /// <param name="binaryConfig">A list of binary configuration options.</param>
        /// <returns>True if the list of binary selections contains the list of binary configuration options, false otherwise. If one parameter is null this mehtod retuns false.</returns>
        public static bool containsBinaryConfiguration(List<List<BinaryOption>> setOfBinaryConfigurations, List<BinaryOption> binaryConfig)
        {
            if (setOfBinaryConfigurations == null || binaryConfig == null)
                return false;

            foreach (List<BinaryOption> oneBinary in setOfBinaryConfigurations)
            {
                if (Configuration.equalBinaryConfiguration(oneBinary, binaryConfig))
                    return true;
            }
            return false;
        }

       
        /// <summary>
        /// Compare two lists of binary configuration options. If both lists contains the same binary options, the mehthod retuns true and otherwise false. 
        /// </summary>
        /// <param name="oneConfiguration">A list of binary configuration options.</param>
        /// <param name="otherBinaryConfiguration">A list of binary configuration options.</param>
        /// <returns>True if both configurations contains the same configuration options, false otherwise.</returns>
        public static bool equalBinaryConfiguration(List<BinaryOption> oneConfiguration, List<BinaryOption> otherBinaryConfiguration)
        {
            if (oneConfiguration == null || otherBinaryConfiguration == null)
                return false;
            if (oneConfiguration.Count != otherBinaryConfiguration.Count)
                return false;

            foreach (BinaryOption opt in oneConfiguration)
            {
                if (!otherBinaryConfiguration.Contains(opt))
                    return false;
            }
            return true;
        }


        // TODO oe of these list might be emply, for example it there are no numeric options in the variability model.
        public static List<Configuration> getConfigurations(List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections, List<Dictionary<NumericOption, double>> numericSelections)
        {
            List<Configuration> configurations = new List<Configuration>();

            foreach (Dictionary<NumericOption, double> numeric in numericSelections)
            {
                foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> binary in binarySelections)
                {
                    Configuration config = Configuration.getConfiguration(binary, numeric);
                    if (!configurations.Contains(config))
                    {
                        configurations.Add(config);
                    }
                }
            }

            return configurations;
        }
    }
}
