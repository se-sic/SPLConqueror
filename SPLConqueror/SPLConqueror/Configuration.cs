using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// A valid configuration option the case study.
    /// </summary>
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

        /// <summary>
        /// All measured values of the non-functional properties. 
        /// </summary>
        public Dictionary<NFProperty, double> nfpValues = new Dictionary<NFProperty, double>();

        private string identifier;

        private double[] optionValues;


        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection">A valid set of binary options.</param>
        /// <param name="numericSelection">A valid selection of values of numeric options.</param>
        /// <param name="measuremements">Measured values of non functional properties, such as performance or footprint.</param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection, Dictionary<NFProperty, double> measuremements)
        {
            binaryOptions = binarySelection;
            if (numericSelection != null)
                numericOptions = numericSelection;
            nfpValues = measuremements;
            identifier = generateIdentifier(DEFAULT_SEPARATOR);

            createIndex();
        }

        /// <summary>
        /// Creates a configuration with the given set an binary and numeric features selected. Binary features existing in the variablity model and not in the given set of binary options are assumed to have
        /// their default value.
        /// </summary>
        /// <param name="binarySelection">A valid set of binary options.</param>
        /// <param name="numericSelection">A valid selection of values of numeric options.</param>
        public Configuration(Dictionary<BinaryOption, BinaryOption.BinaryValue> binarySelection, Dictionary<NumericOption, double> numericSelection)
        {
            binaryOptions = binarySelection;
            if (numericSelection != null)
                numericOptions = numericSelection;
            identifier = generateIdentifier(DEFAULT_SEPARATOR);

            createIndex();
        }


        private void createIndex()
        {
            int diff = 0;
            if (GlobalState.allMeasurements != null && GlobalState.allMeasurements.blacklisted != null)
            {
                diff = GlobalState.allMeasurements.blacklisted.Count;
            }
            optionValues = new double[GlobalState.varModel.indexToOption.Count - diff];

            int shift = 0;
            foreach (KeyValuePair<int, ConfigurationOption> option in GlobalState.varModel.optionToIndex)
            {
                if (option.Value is NumericOption)
                {
                    if (GlobalState.allMeasurements.blacklisted == null || GlobalState.allMeasurements.blacklisted.Count == 0)
                    {
                        try
                        {
                            optionValues[option.Key] = numericOptions[option.Value as NumericOption];
                        }
                        catch (KeyNotFoundException)
                        {
                            GlobalState.logError.logLine(option.Value.Name + "not found in selected numeric options."
                                + "This option is usually mandatory. Unless you removed it from your sampling domain"
                                + ", make sure your measurements contain all numeric options.");
                        }
                    }
                    else if (!GlobalState.allMeasurements.blacklisted.Contains(option.Value.Name.ToLower()))
                    {
                        optionValues[option.Key - shift] = numericOptions[option.Value as NumericOption];
                    }
                    else
                    {
                        shift++;
                    }
                }
                else
                {
                    if (GlobalState.allMeasurements.blacklisted != null &&
                        GlobalState.allMeasurements.blacklisted.Contains(option.Value.Name.ToLower()))
                    {
                        shift++;
                    }
                    else
                    {
                        if (binaryOptions.ContainsKey(option.Value as BinaryOption))
                        {
                            optionValues[option.Key - shift] = 1.0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the identifyer of the configuration, it also adds the configuration to a index structre that is used to access 
        /// the configurations as fast as possible. 
        /// </summary>
        public void update()
        {
            identifier = generateIdentifier(DEFAULT_SEPARATOR);

            createIndex();
        }

        /// <summary>
        /// Constructor to create a new configuration based on selected binary options.
        /// </summary>
        /// <param name="binaryConfig">A set of SELECTED binary configuration options</param>
        public Configuration(List<BinaryOption> binaryConfig)
        {
            foreach (BinaryOption opt in binaryConfig)
            {
                binaryOptions.Add(opt, BinaryOption.BinaryValue.Selected);
            }
            identifier = generateIdentifier(DEFAULT_SEPARATOR);

            createIndex();
        }

        /// <summary>
        /// Creates a new configuration with the given set of binary and numeric configuration options.
        /// </summary>
        /// <param name="binConfig">A set of SELECTED binary configuraiton options.</param>
        /// <param name="numConf">A set numeric configuration options with the values selected in this configuration.</param>
        public Configuration(List<BinaryOption> binConfig, Dictionary<NumericOption, double> numConf)
        {
            foreach (BinaryOption opt in binConfig)
            {
                binaryOptions.Add(opt, BinaryOption.BinaryValue.Selected);
            }
            if (numConf != null)
                numericOptions = numConf;

            identifier = generateIdentifier(DEFAULT_SEPARATOR);

            createIndex();
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
                if (binaryOptions[binary].Equals(BinaryOption.BinaryValue.Selected))
                    sb.Append(binary.Name + separator);
            }

            foreach (NumericOption numeric in numericSelection)
            {
                sb.Append(numeric.Name + "=" + numericOptions[numeric] + separator);
            }



            return sb.ToString();

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
            if (!nfpValues.Keys.Contains(property))
                return -1;
            return nfpValues[property];
        }

        /// <summary>
        /// String representing the configuration. 
        /// </summary>
        /// <returns>The identifier of the configuration.</returns>
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
        /// Compares one configuration with an other configuration. 
        /// </summary>
        /// <param name="other">Configuration to compare</param>
        /// <returns>States whether the two configurations desribes the same configuration option selection.</returns>
        public bool Equals(Configuration other)
        {
            if (other == null)
                return false;

            if (this.optionValues.Count() != other.optionValues.Count())
            {
                return false;
            }

            for (int i = 0; i < optionValues.Count(); i++)
            {
                if (!(this.optionValues[i] == other.optionValues[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// This method retuns the hash code of this configuration. 
        /// </summary>
        /// <returns>The hash code of the configuration based one the identifier.</returns>
        public override int GetHashCode()
        {
            return this.identifier.Replace("root%;%", "").GetHashCode();
        }

        /// <summary>
        /// This method returns a list of binary options that comply to the given configuration value.
        /// </summary>
        /// <param name="binaryValue">The configuration value ((DE-)Selected).</param>
        /// <returns>The list of binary options according to the given value.</returns>
        public List<BinaryOption> getBinaryOptions(BinaryOption.BinaryValue binaryValue)
        {
            List<BinaryOption> result = new List<BinaryOption>();

            foreach (KeyValuePair<BinaryOption, BinaryOption.BinaryValue> bin in this.binaryOptions)
                if (bin.Value.Equals(binaryValue))
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
            Configuration result = new Configuration(selectedBinaryOptions);
            if (numericOptions != null)
                result.numericOptions = numericOptions;
            result.update();
            return result;
        }


        /// <summary>
        /// Generates a new Configuration based on the given selected binary Options and the set values of the numeric options.
        /// However, it has not yet any measured values.
        /// </summary>
        /// <param name="binaryOptions">A dictionary of binary options with the information whether option is selected or deselected.</param>
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


            Configuration result = new Configuration(selectedBinaryOptions);
            if (numericOptions != null)
                result.numericOptions = numericOptions;
            result.update();
            return result;
        }

        /// <summary>
        /// This method evaluates whether the list of binary selections contains a list of binary configuration options.
        /// </summary>
        /// <param name="setOfBinaryConfigurations">A list of binary selections. Each sublist represents a selection of binary configuration options.</param>
        /// <param name="binaryConfig">A list of binary configuration options.</param>
        /// <returns>True if the list of binary selections contains in the list of binary configuration options, false otherwise. If one parameter is null this mehtod retuns false.</returns>
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
            return (oneConfiguration.Count == otherBinaryConfiguration.Count) && !oneConfiguration.Except(otherBinaryConfiguration).Any();
        }

        /// <summary>
        /// This method evaluates whether the list of numeric selections contains a specific numeric selection.
        /// </summary>
        /// <param name="setOfNumericConfigurations">A list of numeric selections. Each sublist represents a selection of numeric configuration options.</param>
        /// <param name="numericConfiguration">A list of numeric configuration options with selected values.</param>
        /// <returns>True if the list of numeric selections contains in the list of numeric configuration options, false otherwise. If one parameter is null this mehtod retuns false.</returns>
        public static bool containsNumericConfiguration(List<Dictionary<NumericOption, double>> setOfNumericConfigurations, Dictionary<NumericOption, double> numericConfiguration)
        {
            if (setOfNumericConfigurations == null || numericConfiguration == null)
                return false;

            foreach (Dictionary<NumericOption, double> oneNumeric in setOfNumericConfigurations)
            {
                if (Configuration.equalNumericalSelection(oneNumeric, numericConfiguration))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Compare two dictionaries of numerical configuration options. If both dictionaries contains the same numerical options with the same values selected, the mehthod retuns true and otherwise false. 
        /// </summary>
        /// <param name="oneConfiguration">A dictionary of numerical configuration options.</param>
        /// <param name="otherNumericalConfiguration">A dictionary of numerical configuration options.</param>
        /// <returns>True if both configurations contains the same configuration options, false otherwise.</returns>
        public static bool equalNumericalSelection(Dictionary<NumericOption, double> oneConfiguration, Dictionary<NumericOption, double> otherNumericalConfiguration)
        {
            if (oneConfiguration.Count != otherNumericalConfiguration.Count)
                return false;

            return oneConfiguration.Keys.All(k => otherNumericalConfiguration.ContainsKey(k) && oneConfiguration[k] == otherNumericalConfiguration[k]);
        }

        /// <summary>
        /// Creates a list of configurations conststing of the cartesian product of the lists of binary and numerical selections. 
        /// </summary>
        /// <param name="binarySelections">A list of binary selections.</param>
        /// <param name="numericSelections">A list of numerical selections.</param>
        /// <returns>A list of configurations.</returns>
        public static List<Configuration> getConfigurations(List<List<BinaryOption>> binarySelections, List<Dictionary<NumericOption, double>> numericSelections)
        {
            HashSet<Configuration> configurations = new HashSet<Configuration>();
            if (binarySelections == null && numericSelections == null)
                return configurations.ToList();
            if (numericSelections != null && numericSelections.Count > 0)
            {
                foreach (Dictionary<NumericOption, double> numeric in numericSelections)
                {
                    if (binarySelections != null && binarySelections.Count > 0)
                    {
                        foreach (List<BinaryOption> binary in binarySelections)
                        {
                            Configuration config = Configuration.getConfiguration(binary, numeric);
                            // if (!configurations.Contains(config))
                            //{
                            configurations.Add(config);
                            //}
                        }
                    }
                    else//We have numeric options, but no binary options
                    {
                        Configuration config = Configuration.getConfiguration(new List<BinaryOption>(), numeric);
                        if (!configurations.Contains(config))
                        {
                            configurations.Add(config);
                        }
                    }
                }
            }
            else
            {//Only binary options are available
                foreach (List<BinaryOption> binary in binarySelections)
                {
                    Configuration config = Configuration.getConfiguration(binary, null);
                    if (!configurations.Contains(config))
                    {
                        configurations.Add(config);
                    }
                }
            }
            return configurations.ToList();
        }

        /// <summary>
        /// Desines a measured value for a non-functional property.
        /// </summary>
        /// <param name="prop">The non-functional property the measuremed value is defined for.</param>
        /// <param name="val">The value for the non-functional property.</param>
        public void setMeasuredValue(NFProperty prop, double val)
        {
            if (prop == null)
            {
                return;
            }
            if (this.nfpValues.Keys.Contains(prop))
                this.nfpValues[prop] = val;
            else
                this.nfpValues.Add(prop, val);
        }

        internal string ToString(List<ConfigurationOption> order)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ConfigurationOption c in order)
            {
                if (c.GetType().Equals(typeof(BinaryOption)))
                {
                    if (this.BinaryOptions.ContainsKey((BinaryOption)c) && this.BinaryOptions[(BinaryOption)c] == BinaryOption.BinaryValue.Selected)
                        sb.Append(c.Name + DEFAULT_SEPARATOR);
                }
                else
                {
                    if (this.numericOptions.ContainsKey((NumericOption)c))
                        sb.Append(c.Name + ";" + this.numericOptions[(NumericOption)c] + DEFAULT_SEPARATOR);
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns an output string formated according to the given order.
        /// </summary>
        /// <param name="order">The order in which the configuration options should be printed.</param>
        /// <returns>A string sorted according to the given order.</returns>
        public string OutputString(List<ConfigurationOption> order)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ConfigurationOption c in order)
            {
                if (c.GetType().Equals(typeof(BinaryOption)))
                {
                    if (this.BinaryOptions.ContainsKey((BinaryOption)c))
                    {
                        String outPutString = c.OutputString;
                        if (outPutString != "noOutput")
                            sb.Append(c.OutputString + " ");
                    }
                }
                else
                {
                    if (this.numericOptions.ContainsKey((NumericOption)c))
                        sb.Append(c.Prefix + this.NumericOptions[(NumericOption)c] + c.Postfix + " ");
                }

            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a configuration based on a hash representation of that configuration.
        /// </summary>
        /// <param name="hashString">The String which from which we can infer the configuration</param>
        /// <param name="vm">The variability model that is required for identifying options in the hash string to instantiate actual configuration options.</param>
        /// <returns>A configuration that maps to the given hash string.</returns>
        internal static Configuration createFromHashString(string hashString, VariabilityModel vm)
        {
            Dictionary<NumericOption, double> numOptions = new Dictionary<NumericOption, double>();
            List<BinaryOption> binaryFeatures = new List<BinaryOption>();
            Configuration c;
            String[] optionList = hashString.Split(new String[] { "%;%" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String option in optionList)
            {
                if (Char.IsDigit(option[option.Length - 1]))//If last char is a digit, then it must be a numeric option
                {
                    //Now remove the digit from the name
                    int index = option.Length - 1;
                    Char last = option[index];

                    while (Char.IsDigit(last) || last == ',' || last == '.' || last == '-')
                    {
                        index--;
                        last = option[index];
                    }
                    Double optionsValue = Math.Round(Double.Parse(option.Substring(index + 1).Replace(',', '.')), 1);
                    NumericOption no = vm.getNumericOption(option.Substring(0, index + 1));
                    if (no == null)
                        continue;
                    numOptions.Add(no, optionsValue);
                }
                else
                {
                    BinaryOption binOpt = vm.getBinaryOption(option);
                    binaryFeatures.Add(binOpt);
                }
            }
            c = new Configuration(binaryFeatures, numOptions);
            return c;
        }

        /// <summary>
        /// Prints a string that respects the variant generation setting of each option. 
        /// This function should be called if we want to get the parameter string to fit it to a program.
        /// </summary>
        /// <returns>The parameter string of the configuration</returns>
        public string printConfigurationForMeasurement()
        {
            StringBuilder sb = new StringBuilder();

            int binaryAndNumericFeatureSize = this.BinaryOptions.Count + this.numericOptions.Count;
            foreach (BinaryOption binary in this.BinaryOptions.Keys)
            {
                if (this.BinaryOptions[binary] == BinaryOption.BinaryValue.Deselected)
                    continue;
                if (binary.OutputString == "noOutput")
                    continue;
                sb.Append(binary.OutputString + " ");
            }

            foreach (NumericOption no in this.numericOptions.Keys)
            {

                sb.Append(no.Prefix + this.numericOptions[no] + " ");
                if (no.Postfix.Length > 0)
                {
                    sb.Append(no.Postfix + " ");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Method to format the configuration into a numeric representation in order to pass it to Python.
        /// </summary>
        /// <returns>Numeric representation of the Configuration.</returns>
        public string toNumeric()
        {
            StringBuilder sb = new StringBuilder();
            var binarySelection = binaryOptions.Keys.ToList();
            binarySelection.Sort();
            var numericSelection = numericOptions.Keys.ToList();
            numericSelection.Sort();

            foreach (double optionValue in optionValues)
            {
                sb.Append(optionValue);
                sb.Append(",");
            }

            sb.Append(GetNFPValue());
            return sb.ToString();
        }

        /// <summary>
        /// Returns the csv-representation of the configuration.
        /// </summary>
        /// <returns>The csv-representation of the configuration.</returns>
        /// <param name="order">The order of the configuration options.</param>
		public string toCsv(List<ConfigurationOption> order, List<NFProperty> nfpProperties)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < order.Count; i++)
            {
                ConfigurationOption c = order[i];

                if (i != 0)
                {
                    result.Append(ConfigurationPrinter.CSV_ELEMENT_DELIMITER);
                }

                if (c.GetType().Equals(typeof(BinaryOption)))
                {
                    if (this.BinaryOptions.ContainsKey((BinaryOption)c))
                    {
                        result.Append(1);
                    }
                    else
                    {
                        result.Append(0);
                    }
                }
                else
                {
                    NumericOption n = (NumericOption)c;
                    if (this.numericOptions.ContainsKey(n))
                    {
                        result.Append(this.NumericOptions[n]);
                    }
                }

            }

            if (nfpProperties != null && nfpProperties.Count != 0 && !GlobalState.currentNFP.Equals(NFProperty.DefaultProperty))
            {
                result.Append(ConfigurationPrinter.CSV_ELEMENT_DELIMITER);
                foreach (NFProperty nfp in this.nfpValues.Keys)
                {
                    if (nfpProperties.Contains(nfp))
                    {
                        result.Append(this.nfpValues[nfp]);
                        result.Append(ConfigurationPrinter.CSV_ELEMENT_DELIMITER);
                    }
                }
                result.Remove(result.Length - ConfigurationPrinter.CSV_ELEMENT_DELIMITER.Length, ConfigurationPrinter.CSV_ELEMENT_DELIMITER.Length);
            }
            result.Append(ConfigurationPrinter.CSV_ROW_DELIMITER);
            return result.ToString();
        }
    }
}
