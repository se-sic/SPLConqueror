using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MachineLearning.Learning;
using MachineLearning;
using SPLConqueror_Core;
using MachineLearning.Learning.Regression;

namespace CommandLine
{
    public class ExperimentState
    {

        public FeatureSubsetSelection learning = new FeatureSubsetSelection();

        public ML_Settings mlSettings = new ML_Settings();

        string binarySamplings_Learning = "";
        List<List<BinaryOption>> binarySelections_Learning = new List<List<BinaryOption>>();

        string binarySamplings_Validation = "";
        List<List<BinaryOption>> binarySelections_Validation = new List<List<BinaryOption>>();


        string numericSamplings_Learning = "";
        List<Dictionary<NumericOption, double>> numericSelection_Learning = new List<Dictionary<NumericOption, double>>();

        string numericSamplings_Validation = "";
        List<Dictionary<NumericOption, double>> numericSelection_Validation = new List<Dictionary<NumericOption, double>>();


        public List<List<BinaryOption>> BinarySelections_Learning
        {
            get { return this.binarySelections_Learning; }
        }

        public List<Dictionary<NumericOption, double>> NumericSelection_Learning
        {
            get { return this.numericSelection_Learning; }
        }

        public List<List<BinaryOption>> BinarySelections_Validation
        {
            get { return this.binarySelections_Validation; }
        }

        public List<Dictionary<NumericOption, double>> NumericSelection_Validation
        {
            get { return this.numericSelection_Validation; }
        }

        InfluenceFunction trueModel = null;

        public InfluenceFunction TrueModel
        {
            get { return trueModel; }
            set { trueModel = value; }
        }


        /// <summary>
        /// Returns a textual representation of this object consisting of the names of numerical and binary sampling methods performed for this experimental state and a representation of the mlsettings. 
        /// </summary>
        /// <returns>The textual reprentation.</returns>
        public override string ToString()    
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Learning: " + binarySamplings_Learning + "  " + numericSamplings_Learning);
            sb.Append(mlSettings.ToString());
 	        return sb.ToString();
        }

        /// <summary>
        /// Adds the identifier of a binary sampling method used for the learning set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addBinarySampling_Learning(string name)
        {
            binarySamplings_Learning += name + " ";
        }


        /// <summary>
        /// Adds the identifier of a numeric sampling method used for the learning set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addNumericSampling_Learning(string name)
        {
            numericSamplings_Learning += name + " ";
        }

        /// <summary>
        /// Adds the identifier of a binary sampling method used for the validation set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addBinarySampling_Validation(string name)
        {
            binarySamplings_Validation += name + " ";
        }


        /// <summary>
        /// Adds the identifier of a numeric sampling method used for the validation set to a logging sting. 
        /// </summary>
        /// <param name="name">Name of the sampling method.</param>
        public void addNumericSampling_Validation(string name)
        {
            numericSamplings_Validation += name + " ";
        }



        /// <summary>
        /// Clears the binary and numeric selections stored in this object. 
        /// </summary>
        public void clearSampling()
        {
            binarySamplings_Learning = "";
            binarySelections_Learning = new List<List<BinaryOption>>();

            binarySamplings_Validation = "";
            binarySelections_Validation = new List<List<BinaryOption>>();

            numericSamplings_Learning = "";
            numericSelection_Learning = new List<Dictionary<NumericOption, double>>();

            numericSamplings_Validation = "";
            numericSelection_Validation = new List<Dictionary<NumericOption, double>>();

            this.learning = new FeatureSubsetSelection();
        }

        /// <summary>
        /// Clears the binary and numeric selections and the machine learning settings stored in this object. 
        /// </summary>
        public void clear()
        {
            mlSettings = new ML_Settings();
            clearSampling();
        }

        private void addBinarySelection(List<List<BinaryOption>> selections, List<BinaryOption> newSelection)
        {
         //   if (Configuration.containsBinaryConfiguration(selections,newSelection))
           //     return;
            selections.Add(newSelection);
        }


        private void addNumericSelection(List<Dictionary<NumericOption, double>> selections, Dictionary<NumericOption, double> newSelection)
        {
            if (Configuration.containsNumericConfiguration(selections, newSelection))
                return;
            selections.Add(newSelection);
        }


        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState. All binary configuration options are assumed to be selected. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A set of binary configuration-option selections. All options of are assumed to be selected.</param>
        public void addBinarySelection_Learning(List<List<BinaryOption>> newSelections)
        {
            foreach (List<BinaryOption> selection in newSelections)
                addBinarySelection_Learning(selection);
        }

        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState. All binary configuration options are assumed to be selected. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A set of binary configuration-option selections. All options of are assumed to be selected.</param>
        public void addBinarySelection_Learning(List<BinaryOption> newSelection)
        {
            addBinarySelection(this.binarySelections_Learning, newSelection);
        }


        /// <summary>
        /// Add a selection of numeric-configuration options. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addNumericalSelection_Learning(Dictionary<NumericOption, double> newSelection)
        {
            addNumericSelection(this.numericSelection_Learning, newSelection);
        }

        /// <summary>
        /// Add a set of numerical-configuration option selections to the ExperimentalState. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addNumericalSelection_Learning(List<Dictionary<NumericOption, double>> newSelections)
        {
            foreach (Dictionary<NumericOption, double> oneSelection in newSelections)
                addNumericalSelection_Learning(oneSelection);
        }


        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState. All binary configuration options are assumed to be selected. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A set of binary configuration-option selections. All options of are assumed to be selected.</param>
        public void addBinarySelection_Validation(List<List<BinaryOption>> newSelections)
        {
            foreach (List<BinaryOption> selection in newSelections.Distinct())
                addBinarySelection_Validation(selection);
        }

        /// <summary>
        /// Add a selection of binary-configuration options. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addBinarySelection_Validation(List<BinaryOption> newSelection)
        {
            addBinarySelection(binarySelections_Validation, newSelection);
        }

        /// <summary>
        /// Add a selection of numeric-configuration options. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addNumericalSelection_Validation(Dictionary<NumericOption, double> newSelection)
        {
            addNumericSelection(this.numericSelection_Validation, newSelection);
        }

        /// <summary>
        /// Add a set of numerical-configuration option selections to the ExperimentalState. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addNumericalSelection_Validation(List<Dictionary<NumericOption, double>> newSelections)
        {
            foreach (Dictionary<NumericOption, double> oneSelection in newSelections)
                addNumericalSelection_Validation(oneSelection);
        }
    }
}
