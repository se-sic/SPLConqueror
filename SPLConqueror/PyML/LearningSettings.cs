using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWrapper
{
    public class LearningSettings
    {

        public enum LearningStrategies { SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE};

        private static LearningStrategies getStrategy(string strategyAsString)
        {
            bool ignoreCase = true;
            return (LearningStrategies)Enum.Parse(typeof(LearningStrategies), strategyAsString, ignoreCase);
        }

        public static bool isLearningStrategy(string toTest)
        {
            try
            {
                getStrategy(toTest);
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }

    }
}
