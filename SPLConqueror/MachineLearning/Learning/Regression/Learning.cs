using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using System.Threading;
using System.Collections.Concurrent;

namespace MachineLearning.Learning.Regression
{
    public class Learning
    {
        public  ML_Settings mLsettings = null;
        public List<Configuration> testSet, validationSet = null;
        List<System.Threading.Thread> threadPool = new List<Thread>();
        int nbBaggings = 0;
        public List<FeatureSubsetSelection> models = new List<FeatureSubsetSelection>();
        public InfluenceModel metaModel = null;
        public LearningInfo info = new LearningInfo();

        

        public Learning()
        {

        }
        public Learning(List<Configuration> testSet, List<Configuration> validationSet)
        {
            this.testSet = testSet;
            this.validationSet = validationSet;
        }

        public void learn()
        {
            if (!hasNecessaryData())
                return;
            if (this.mLsettings.bagging)
            {
                //Get number of cores
                int coreCount = 0;
                foreach (var item in new System.Management.ManagementObjectSearcher("Select NumberOfCores from Win32_Processor").Get())
                {
                    coreCount += int.Parse(item["NumberOfCores"].ToString());
                }
                createThreadPool(coreCount);

                this.nbBaggings = this.mLsettings.baggingNumbers;
                while (nbBaggings > 0)
                {


                }

            }
            else
            {
                LearningInfo exp = new LearningInfo();
                InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                FeatureSubsetSelection sel = new FeatureSubsetSelection(infMod, this.mLsettings);
                this.models.Add(sel);
                sel.setLearningSet(testSet);
                sel.setValidationSet(this.validationSet);
                sel.learn();
            }
        }

        private void createThreadPool(int coreCount)
        {
            for (int i = 0; i < coreCount; i++)
            {
                Thread t = new Thread(new ThreadStart(LearnModel));
            }
        }

        void LearnModel()
        {

        }

        private bool hasNecessaryData()
        {
            if (this.testSet.Count == 0)
                return false;
            return true;
        }


        public void clearSampling()
        {
            if(this.testSet != null)
                this.testSet.Clear();
            if(this.validationSet != null)
                this.validationSet.Clear();
        }

        public void clear()
        {
            this.nbBaggings = 0;
            this.mLsettings = new ML_Settings();
            this.metaModel = null;
            this.models.Clear();
            clearSampling();
            this.threadPool.Clear();
        }
    }
}
