using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    public class LearningRound
    {
        public double learningError = Double.MaxValue;
        public double validationError = Double.MaxValue;
        public double learningError_relative = Double.MaxValue;
        public double validationError_relative = Double.MaxValue;
        private List<Feature> featureSet = new List<Feature>();
        public List<Feature> FeatureSet
        {
            get { return featureSet; }
            set { featureSet = value; }
        }
        public int round = 0;

        public TimeSpan elapsedTime = new TimeSpan(0);
        public double modelComplexity {
            get {
                const double complexityPower = 1.21;
                double complexity = 0;
                foreach (var feature in featureSet)
                {
                    complexity += Math.Pow(feature.getNumberOfParticipatingOptions(), complexityPower);
                }
                return complexity;
            }
        }
        public Feature bestCandidate = null;
        public int bestCandidateSize = 1;
        public double bestCandidateScore = 0;
        public string terminationReason = null;

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
                sb.Append(f.ToString());
                if (i < featureSet.Count - 1)
                    sb.Append(" + ");
            }
            sb.Append(";");

            sb.Append(learningError + ";");
            sb.Append(learningError_relative+";");
            sb.Append(validationError + ";");
            sb.Append(validationError_relative + ";");
            sb.Append(elapsedTime.TotalSeconds + ";");
            sb.Append(modelComplexity + ";");
            sb.Append(bestCandidate + ";");
            sb.Append(bestCandidateSize + ";");
            sb.Append(bestCandidateScore + ";");
            //sb.Append(string.Format("{0};", ));

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
