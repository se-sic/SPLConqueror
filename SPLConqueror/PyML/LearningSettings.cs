using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessWrapper
{
    public class LearningSettings
    {
        public enum LearningStrategies { SVR, LinearSVR, NuSVR, DecisionTreeRegression};

        public enum LearningKernel { linear, standard, poly}
    }
}
