using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILNumerics;
using System.IO;
using Accord.Math;
using Accord.Math.Decompositions;
using System.Diagnostics;
using SPLConqueror_Core;
using MachineLearning.Learning;

namespace MachineLearning.Learning.Regression
{
    public class FeatureSubsetSelection
    {

        private InfluenceModel infModel = null;
        private ILArray<double> Y_learning, Y_validation = ILMath.empty();
        private Dictionary<Feature, ILArray<double>> DM_columns = new Dictionary<Feature, ILArray<double>>();

        public FeatureSubsetSelection(InfluenceModel infModel)
        {
            this.infModel = infModel;
        }



        //The method builds the matrix X according to the current feature set I
        //Example: I= f1 f2 f1#f2 -> X must be length 3 and for the following configurations the following values
        //f1 -> 1 0 0
        //f2 -> 0 1 0
        //f1 f2>1 1 1
        //Extension to numerical features: n1#n1 -> n1^2 
        /*private ILArray<double> transformDataSetAccordingToFeatureSetI(List<List<Element>> featureSetI, List<Element> newFeature)
        {
            ILArray<double> DM;
            if (newFeature == null)
                DM = ILMath.zeros(featureSetI.Count, this.measurements.Keys.Count);
            else
                DM = ILMath.zeros(featureSetI.Count + 1, this.measurements.Keys.Count);
            int m = 0;
            foreach (List<Element> config in this.measurements.Keys)
            {//Row in DM

                for (int i = 0; i < featureSetI.Count; i++) //List<Element> currentFeatureInI in featureSetI)
                {
                    bool allFeaturesInConfig = true;
                    foreach (Element feature in featureSetI[i])
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig)
                    {
                        DM[i, m] = 1;
                    }
                    // else
                    // {
                    //     DM[i,m] = 0;
                    // }
                }

                if (newFeature != null)
                {
                    bool allFeaturesInConfig2 = true;
                    foreach (Element feature in newFeature)
                    {
                        if (!config.Contains(feature))
                        {
                            allFeaturesInConfig2 = false;
                            break;
                        }
                    }
                    if (allFeaturesInConfig2)
                    {
                        DM[featureSetI.Count, m] = 1;
                    }
                    //   else
                    //   {
                    //       DM[featureSetI.Count, m] = 0;
                    //   }
                }
                m++;
            }

            return DM;
        }*/
    }
}
