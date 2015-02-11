using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MachineLearning.Learning;
using MachineLearning;
using SPLConqueror_Core;

namespace CommandLine
{
    class ExperimentState
    {

        public ML_Settings mlSettings = null;

        string binarySamplings = "";
        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections = null;

        string numericSamplings = "";
        List<Dictionary<NumericOption, double>> numericSelection = null;


        public void addBinarySampling(string name)
        {
            binarySamplings += name + " ";
        }

        public void addNumericSampling(string name)
        {
            numericSamplings += name + " ";
        }

        /// <summary>
        /// Clears the binary and numeric selections stored in this object. 
        /// </summary>
        public void clearSampling()
        {
            binarySamplings = "";
            binarySelections = null;

            binarySamplings = "";
            numericSelection = null;
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
        public void addBinarySelection(List<List<BinaryOption>> newSelections)
        {
            if (binarySelections == null)
                binarySelections = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (List<BinaryOption> selection in newSelections)
            {
                Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection = new Dictionary<BinaryOption, BinaryOption.BinaryValue>();

                foreach (BinaryOption bin in selection)
                {
                    newSelection.Add(bin, BinaryOption.BinaryValue.Selected);
                }

                if (binarySelections.Contains(newSelection))
                    continue;
                binarySelections.Add(newSelection);
            }
        
        }


        /// <summary>
        /// Add a selection of binary-configuration options. Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addBinarySelection(Dictionary<BinaryOption, BinaryOption.BinaryValue> newSelection)
        {
            if (binarySelections == null)
                binarySelections = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            if(binarySelections.Contains(newSelection))
                return; 
            binarySelections.Add(newSelection);
        }

        /// <summary>
        /// Add a set of binary-configuration option selections to the ExperimentalState.  Multiple entries of the same binary selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addBinarySelection(List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> newSelections)
        {
            if (binarySelections == null)
                binarySelections = new List<Dictionary<BinaryOption, BinaryOption.BinaryValue>>();

            foreach (Dictionary<BinaryOption, BinaryOption.BinaryValue> selection in newSelections)
            {
                if (binarySelections.Contains(selection))
                    return;
                binarySelections.Add(selection);
            }
        }

        /// <summary>
        /// Add a selection of numeric-configuration options. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelection">A selection of binary configuration options.</param>
        public void addNumericalSelection(Dictionary<NumericOption, double> newSelection)
        {
            if (numericSelection == null)
                numericSelection = new List<Dictionary<NumericOption, double>>();

            if (numericSelection.Contains(newSelection))
                return;
            numericSelection.Add(newSelection);
        }

        /// <summary>
        /// Add a set of numerical-configuration option selections to the ExperimentalState. Multiple entries of the same numerical selections are removed. 
        /// </summary>
        /// <param name="newSelections">A set of binary configuration-option selections.</param>
        public void addNumericalSelection(List<Dictionary<NumericOption, double>> newSelections)
        {
            if (numericSelection == null)
                numericSelection = new List<Dictionary<NumericOption, double>>();

            foreach (Dictionary<NumericOption, double> selection in newSelections)
            {
                if (numericSelection.Contains(selection))
                    return;
                numericSelection.Add(selection);
            }
        }


    }
}
