using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    public class LearningRound
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
            StringBuilder sb = new StringBuilder();
            sb.Append(round + ";");
            for (int i = 0; i < featureSet.Count; i++)
            {
                Feature f = featureSet[i];
                sb.Append(f.Constant + " * " + f.ToString());
                if (i < featureSet.Count - 1)
                    sb.Append(" + ");
            }
            sb.Append(";");
            sb.Append(learningError + ";");
            sb.Append(validationError + ";");

            return sb.ToString();
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
