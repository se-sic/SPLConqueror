using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    class LearningRound
    {
        internal double learningError = Double.MaxValue;
        internal double validationError = Double.MaxValue;
        internal List<Feature> featureSet = new List<Feature>();
        internal int round = 0;

        /// <summary>
        /// TODO: Prints the information learned in a round.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        internal LearningRound(List<Feature> featureSet, double learningError, double validationError, int round)
        {
            this.featureSet = featureSet;
            this.learningError = learningError;
            this.validationError = validationError;
            this.round = round;
        }

        internal LearningRound() {}
    }
}
