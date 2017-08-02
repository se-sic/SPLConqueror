using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPLConqueror_Core;

namespace ProcessWrapper
{
    public class LearningSettings
    {
        /// <summary>
        /// Supported learning strategies by the python module.
        /// </summary>
        public enum LearningStrategies { SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE};

        private static LearningStrategies getStrategy(string strategyAsString)
        {
            bool ignoreCase = true;
            return (LearningStrategies)Enum.Parse(typeof(LearningStrategies), strategyAsString, ignoreCase);
        }

        /// <summary>
        /// Test if a command is a supported learning strategy.
        /// </summary>
        /// <param name="toTest">String command to test.</param>
        /// <returns></returns>
        public static bool isLearningStrategy(string toTest)
        {
            try
            {
                getStrategy(toTest);
                return true;
            }
            catch (ArgumentException)
            {
                GlobalState.logError.logLine("Command " + toTest + " is no learning strategy.");
                return false;
            }
        }

    }
}
