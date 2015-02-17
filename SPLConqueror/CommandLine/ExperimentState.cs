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

    // TODO use Configuration.equalBinarySelection and equivalent methods. Decide whether the configurations have to be stored as 
    // Dictionary of Binary option and selection or whether a List of binary options is enought. 
    class ExperimentState
    {

        public FeatureSubsetSelection learning = null;

        public ML_Settings mlSettings = new ML_Settings();

        string binarySamplings_Learning = "";
        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections_Learning = null;

        string binarySamplings_Validation = "";
        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections_Validation = null;


        string numericSamplings_Learning = "";
        List<Dictionary<NumericOption, double>> numericSelection_Learning = null;

        string numericSamplings_Validation = "";
        List<Dictionary<NumericOption, double>> numericSelection_Validation = null;


        public List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> BinarySelections_Learning
        {
            get { return this.binarySelections_Learning; }
        }

        public List<Dictionary<NumericOption, double>> NumericSelection_Learning
        {
            get { return this.numericSelection_Learning; }
        }

        public List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> BinarySelections_Validation
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
            binarySelections_Learning = null;

            binarySamplings_Validation = "";
            binarySelections_Validation = null;

            numericSamplings_Learning = "";
            numericSelection_Learning = null;

            numericSamplings_Validation = "";
            numericSelection_Validation = null;
        }

        public void clear()
        {
            mlSettings = new ML_Settings();
            clearSampling();
        }

        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState. All binary configuration options are assumed to be selected. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A set of binary configuration-option selections. All options of are assumed to be selected.</param>
        public void addBinarySelection_Learning(List<List<BinaryOption>> newSelections)
        {
            if (binarySelections_Learning == null)
                binarySelections_Learning = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (List<BinaryOption> selection in newSelections)
            {
                Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

                foreach (BinaryOption bin in selection)
                {
                    newSelection.Add(bin, BinaryOption.BinaryValue.Selected);
                }

                if (binarySelections_Learning.Contains(newSelection))
                    continue;
                binarySelections_Learning.Add(newSelection);
            }
        
        }


        /// <summary>
        /// Add a selection of binary-configuration options. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addBinarySelection_Learning(Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection)
        {
            if (binarySelections_Learning == null)
                binarySelections_Learning = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            if(binarySelections_Learning.Contains(newSelection))
                return; 
            binarySelections_Learning.Add(newSelection);
        }

        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState.  Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addBinarySelection_Learning(List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> newSelections)
        {
            if (binarySelections_Learning == null)
                binarySelections_Learning = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> selection in newSelections)
            {
                if (binarySelections_Learning.Contains(selection))
                    return;
                binarySelections_Learning.Add(selection);
            }
        }

        /// <summary>
        /// Add a selection of numeric-configuration options. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addNumericalSelection_Learning(Dictionary<NumericOption, double> newSelection)
        {
            if (numericSelection_Learning == null)
                numericSelection_Learning = new List<Dictionary<NumericOption, double>>();

            if (numericSelection_Learning.Contains(newSelection))
                return;
            numericSelection_Learning.Add(newSelection);
        }

        /// <summary>
        /// Add a set of numerical-configuration option selections to the ExperimentalState. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addNumericalSelection_Learning(List<Dictionary<NumericOption, double>> newSelections)
        {
            if (numericSelection_Learning == null)
                numericSelection_Learning = new List<Dictionary<NumericOption, double>>();

            foreach (Dictionary<NumericOption, double> selection in newSelections)
            {
                if (numericSelection_Learning.Contains(selection))
                    return;
                numericSelection_Learning.Add(selection);
            }
        }


        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState. All binary configuration options are assumed to be selected. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A set of binary configuration-option selections. All options of are assumed to be selected.</param>
        public void addBinarySelection_Validation(List<List<BinaryOption>> newSelections)
        {
            if (binarySelections_Validation == null)
                binarySelections_Validation = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (List<BinaryOption> selection in newSelections)
            {
                Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

                foreach (BinaryOption bin in selection)
                {
                    newSelection.Add(bin, BinaryOption.BinaryValue.Selected);
                }

                if (binarySelections_Validation.Contains(newSelection))
                    continue;
                binarySelections_Validation.Add(newSelection);
            }

        }


        /// <summary>
        /// Add a selection of binary-configuration options. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addBinarySelection_Validation(Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection)
        {
            if (binarySelections_Validation == null)
                binarySelections_Validation = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            if (binarySelections_Validation.Contains(newSelection))
                return;
            binarySelections_Validation.Add(newSelection);
        }

        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState.  Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addBinarySelection_Validation(List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> newSelections)
        {
            if (binarySelections_Validation == null)
                binarySelections_Validation = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> selection in newSelections)
            {
                if (binarySelections_Validation.Contains(selection))
                    return;
                binarySelections_Validation.Add(selection);
            }
        }

        /// <summary>
        /// Add a selection of numeric-configuration options. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addNumericalSelection_Validation(Dictionary<NumericOption, double> newSelection)
        {
            if (numericSelection_Validation == null)
                numericSelection_Validation = new List<Dictionary<NumericOption, double>>();

            if (numericSelection_Validation.Contains(newSelection))
                return;
            numericSelection_Validation.Add(newSelection);
        }

        /// <summary>
        /// Add a set of numerical-configuration option selections to the ExperimentalState. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addNumericalSelection_Validation(List<Dictionary<NumericOption, double>> newSelections)
        {
            if (numericSelection_Validation == null)
                numericSelection_Validation = new List<Dictionary<NumericOption, double>>();

            foreach (Dictionary<NumericOption, double> selection in newSelections)
            {
                if (numericSelection_Validation.Contains(selection))
                    return;
                numericSelection_Validation.Add(selection);
            }
        }
    }
}
