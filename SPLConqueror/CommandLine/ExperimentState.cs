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

        ML_Settings settings = null;
        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections = null;
        List<Dictionary<NumericOption, double>> numericSelection = null;




        public void clearSampling()
        {
            binarySelections = null;
            numericSelection = null;
        }

        public void clear()
        {
            settings = ML_Settings.getDefaultSettings();
            binarySelections = null;
            numericSelection = null;
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
