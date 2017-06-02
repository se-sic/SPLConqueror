using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Sampling.Heuristics
{
    class Linear
    {

        private List<List<BinaryOption>> configurations = new List<List<BinaryOption>>();
        private int counter;

        public const string PARAMETER_NUMCONFIGS_AS_OW = "numOW";
        public const string PARAMETER_NUMCONFIGS_AS_PW = "numPW";

        public const string PARAMETER_NUMCONFIGS_NAME = "numConfigs";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm">The variablity model of the case-study system.</param>
        /// <param name="treshold">Maximal number of configurations selected by the sampling strategy.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns></returns>
        public List<List<BinaryOption>> GenerateRLinear(VariabilityModel vm, int treshold, int timeout)
        {
            //Solver.VariantGenerator generator = new Solver.VariantGenerator(null);

            List<List<BinaryOption>> erglist = new List<List<BinaryOption>>();

            //var tasks = new Task[vm.BinaryOptions.Count];
            //var mylock = new object();

            //for (var i = 1; i <= vm.BinaryOptions.Count; i++)
            //{
            //    var i1 = i;
            //    tasks[i - 1] = Task.Factory.StartNew(() =>
            //    {
            //        var size = LinearSize(vm.BinaryOptions.Count, treshold, i1);
            //        return generator.generateTilSize(i1, size, timeout, vm);

            //    }).ContinueWith(task =>
            //    {
            //        if (!task.Status.Equals(TaskStatus.Faulted))
            //        {
            //            lock (mylock)
            //            {
            //                if (task != null) { 

            //                    erglist.AddRange(task.Result);
            //                    counter++;
            //                }
            //            }

            //        }
                   
            //    });

            //}

            //Task.WaitAll(tasks);

            return erglist;
        }

        public int LinearSize(int numFeatures, int tresh, int index)
        {
            int size;
            float slope = (tresh / (float)numFeatures);
            if (index < numFeatures / 2.0f)
            {
                size = (int)(2 * slope * index) + tresh;
            }
            else
            {
                size = (int)(-index * 2 * slope) + 3 * tresh;
            }
           // Console.WriteLine(size);
            return size;
        }

    }
}
