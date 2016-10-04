using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWrapper
{
    public class LearningSettings
    {

        public enum LearningStrategies { SVR, DecisionTreeRegression, RandomForestRegressor, BaggingSVR, KNeighborsRegressor, KERNELRIDGE, DecisionTreeRegressor };

        public static LearningStrategies getStrategy(string strategyAsString)
        {
            return (LearningStrategies)Enum.Parse(typeof(LearningStrategies), strategyAsString);
        }

    }
}
