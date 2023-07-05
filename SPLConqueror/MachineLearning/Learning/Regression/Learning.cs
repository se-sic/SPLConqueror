﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using MachineLearning.Sampling;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MachineLearning.Learning.Regression
{
    public class Learning : IDisposable
    {
        public ML_Settings mlSettings = null;
        public List<Configuration> testSet, validationSet = null;
        int nbBaggings = 0;
        public ObservableCollection<FeatureSubsetSelection> models = new ObservableCollection<FeatureSubsetSelection>();
        public InfluenceModel metaModel = null;
        public LearningInfo info = new LearningInfo();

        // The event to know that all threads are done
        ManualResetEvent eventX = new ManualResetEvent(false);
        public static int iCount = 0;

        class WorkItem
        {
            public readonly TaskCompletionSource<object> TaskSource;
            public readonly Action Action;
            public readonly CancellationToken? CancelToken;


            public WorkItem(
              TaskCompletionSource<object> taskSource,
              Action action,
              CancellationToken? cancelToken)
            {
                TaskSource = taskSource;
                Action = action;
                CancelToken = cancelToken;
            }
        }
        BlockingCollection<WorkItem> _taskQ = new BlockingCollection<WorkItem>();

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
            if (this.mlSettings.bagging)
            {
                //Get number of cores
                int coreCount = System.Environment.ProcessorCount;
                createThreadPool(coreCount);

                this.nbBaggings = this.mlSettings.baggingNumbers;
                iCount = this.nbBaggings;
                Random rand = new Random(0);
                int nbOfConfigs = (testSet.Count * this.mlSettings.baggingTestDataFraction) / 100;
                for (int i = 0; i < nbBaggings; i++)
                {
                    InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                    FeatureSubsetSelection sel = new FeatureSubsetSelection(infMod, this.mlSettings);
                    this.models.Add(sel);
                    List<int> selection = new List<int>();
                    for (int r = 0; r <= nbOfConfigs; r++)
                    {
                        selection.Add(rand.Next(nbOfConfigs));
                    }
                    List<Configuration> newTestSet = new List<Configuration>();
                    List<Configuration> newValidationSet = new List<Configuration>();
                    for (int r = 0; r <= selection.Count; r++)
                    {
                        if (selection.Contains(r))
                            newTestSet.Add(testSet[r]);
                        else
                            newValidationSet.Add(testSet[r]);
                    }
                    sel.setLearningSet(newTestSet);
                    sel.setValidationSet(newValidationSet);
                    Task task = EnqueueTask(() => sel.learn());
                }
                eventX.WaitOne(Timeout.Infinite, true);
                averageModels();
            }
            else
            {
                GlobalState.logInfo.logLine("Learning progress:");
                InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                FeatureSubsetSelection sel = new FeatureSubsetSelection(infMod, this.mlSettings);
                this.models.Add(sel);
                sel.setLearningSet(testSet);
                sel.setValidationSet(this.validationSet);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                sel.learn();
                sw.Stop();
                Console.WriteLine("Prediction Error: {0} %", sel.finalError);
                Console.WriteLine("Elapsed learning time(seconds): {0}", sw.Elapsed);
            }
        }


        /// <summary>
        /// Continues learning with recovered learning data.
        /// </summary>
        /// <param name="recoveredLr">Learning rounds that were already performed.</param>
        public void continueLearning(List<LearningRound> recoveredLr)
        {
            if (!hasNecessaryData())
                return;
            if (this.mlSettings.bagging)
            {
                throw new NotImplementedException("Recovering with bagging currently dosent work");
            }
            else
            {
                ObservableCollection<LearningRound> learningRounds = new ObservableCollection<LearningRound>();
                GlobalState.logInfo.logLine("Learning progress:");
                foreach (LearningRound lr in recoveredLr)
                {
                    GlobalState.logInfo.logLine(lr.ToString());
                    learningRounds.Add(lr);
                }
                InfluenceModel infMod = new InfluenceModel(GlobalState.varModel, GlobalState.currentNFP);
                FeatureSubsetSelection sel = new FeatureSubsetSelection(infMod, this.mlSettings);
                this.models.Add(sel);
                sel.setLearningSet(testSet);
                sel.setValidationSet(this.validationSet);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                sel.continueLearn(learningRounds);
                sw.Stop();
                Console.WriteLine("Elapsed learning time(seconds): {0}", sw.Elapsed);
            }
        }

        private void averageModels()
        {
            List<FeatureSubsetSelection> sorted = this.models.OrderBy(o => o.finalError).ToList();

            int avg = this.models.Count;
            //int avg = this.models.Count / 2;
            for (int i = 0; i < avg; i++)
            {
                updateInfluenceModel(sorted[i].infModel);
            }
            averageWeights(avg);
        }

        private void averageWeights(int avg)
        {
            foreach (BinaryOption bin in this.metaModel.BinaryOptionsInfluence.Keys)
            {
                ((Feature)this.metaModel.BinaryOptionsInfluence[bin]).Constant /= avg;
            }
            foreach (NumericOption num in this.metaModel.NumericOptionsInfluence.Keys)
            {
                ((Feature)this.metaModel.NumericOptionsInfluence[num]).Constant /= avg;
            }
            foreach (Interaction inter in this.metaModel.InteractionInfluence.Keys)
            {
                ((Feature)this.metaModel.InteractionInfluence[inter]).Constant /= avg;
            }

        }

        private void updateInfluenceModel(InfluenceModel influenceModel)
        {
            foreach (BinaryOption bin in influenceModel.BinaryOptionsInfluence.Keys)
            {
                if (this.metaModel.BinaryOptionsInfluence.Keys.Contains(bin))
                {
                    ((Feature)this.metaModel.BinaryOptionsInfluence[bin]).Constant += ((Feature)influenceModel.BinaryOptionsInfluence[bin]).Constant;
                }
                else
                {
                    this.metaModel.BinaryOptionsInfluence.Add(bin, ((Feature)influenceModel.BinaryOptionsInfluence[bin]));
                }
            }
            foreach (NumericOption num in influenceModel.NumericOptionsInfluence.Keys)
            {
                if (this.metaModel.NumericOptionsInfluence.Keys.Contains(num))
                {
                    ((Feature)this.metaModel.NumericOptionsInfluence[num]).Constant += ((Feature)influenceModel.NumericOptionsInfluence[num]).Constant;
                }
                else
                {
                    this.metaModel.NumericOptionsInfluence.Add(num, ((Feature)influenceModel.NumericOptionsInfluence[num]));
                }
            }
            foreach (Interaction interact in influenceModel.InteractionInfluence.Keys)
            {
                if (this.metaModel.InteractionInfluence.Keys.Contains(interact))
                {
                    ((Feature)this.metaModel.InteractionInfluence[interact]).Constant += ((Feature)influenceModel.InteractionInfluence[interact]).Constant;
                }
                else
                {
                    this.metaModel.InteractionInfluence.Add(interact, ((Feature)influenceModel.InteractionInfluence[interact]));
                }
            }
        }

        private void createThreadPool(int coreCount)
        {
            var customCulture = Thread.CurrentThread.CurrentCulture;
            for (int i = 0; i < coreCount; i++)
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.CurrentCulture = customCulture;
                    this.Consume();
                }
                );
            }
        }

        public void Dispose() { _taskQ.CompleteAdding(); }

        public Task EnqueueTask(Action action)
        {
            return EnqueueTask(action, null);
        }

        public Task EnqueueTask(Action action, CancellationToken? cancelToken)
        {
            var tcs = new TaskCompletionSource<object>();
            _taskQ.Add(new WorkItem(tcs, action, cancelToken));
            return tcs.Task;
        }

        void Consume()
        {
            foreach (WorkItem workItem in _taskQ.GetConsumingEnumerable())
                if (workItem.CancelToken.HasValue &&
                    workItem.CancelToken.Value.IsCancellationRequested)
                {
                    workItem.TaskSource.SetCanceled();
                }
                else
                    try
                    {
                        workItem.Action();
                        workItem.TaskSource.SetResult(null);   // Indicate completion
                        Interlocked.Decrement(ref iCount);
                        if (iCount == 0)
                        {
                            eventX.Set();
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (ex.CancellationToken == workItem.CancelToken)
                            workItem.TaskSource.SetCanceled();
                        else
                            workItem.TaskSource.SetException(ex);
                    }
                    catch (Exception ex)
                    {
                        workItem.TaskSource.SetException(ex);
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
            this.models.Clear();

            if (this.testSet != null)
                this.testSet = new List<Configuration>();
            if (this.validationSet != null)
                this.validationSet = new List<Configuration>();

            this.info.binarySamplings_Learning = "";
            this.info.binarySamplings_Validation = "";

            this.info.numericSamplings_Learning = "";
            this.info.numericSamplings_Validation = "";

            ConfigurationBuilder.binaryParams = new BinaryParameters();
        }

        public void clear()
        {
            this.nbBaggings = 0;
            this.mlSettings = new ML_Settings();
            this.metaModel = null;
            clearSampling();
        }
    }

}
