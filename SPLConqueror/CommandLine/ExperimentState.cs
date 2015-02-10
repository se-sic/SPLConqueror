using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MachineLearning;
using SPLConqueror_Core;

namespace CommandLine
{
    class ExperimentState
    {

        ML_Settings settings = null;
        List<Dictionary<BinaryOption, BinaryOption.BinaryValue>> binarySelections = null;
        List<Dictionary<NumericOption, double>> numericSelection = null;



    }
}
