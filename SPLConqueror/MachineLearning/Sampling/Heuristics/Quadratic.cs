using SPLConqueror_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Sampling.Heuristics
{
    class Quadratic
    {
        private int counter;

        public const string PARAMETER_NUMCONFIGS_AS_OW = "numOW";
        public const string PARAMETER_NUMCONFIGS_AS_PW = "numPW";

        public const string PARAMETER_NUMCONFIGS_NAME = "numConfigs";


        public List<List<BinaryOption>> GenerateRQuadratic(VariabilityModel vm, int treshold, double scale, int timeout)
        {
            Solver.VariantGenerator generator = new Solver.VariantGenerator(null);
            var resultList = new List<List<BinaryOption>>();

            var tasks = new Task[vm.BinaryOptions.Count];
            var mylock = new object();

            for (var i = 1; i <= vm.BinaryOptions.Count; i++)
            {
                var i1 = i;
                tasks[i - 1] = Task.Factory.StartNew(() =>
                {

                    var size = QuadraticSize(vm.BinaryOptions.Count, i1, scale);
                    return generator.generateTilSize(i1, size, timeout, vm);

                }).ContinueWith(task =>
                {
                    if (!task.Status.Equals(TaskStatus.Faulted))
                    {

                        lock (mylock)
                        {
                            resultList.AddRange(task.Result);
                            counter++;
                        }
                    }
                });

            }
            Task.WaitAll(tasks);

            return resultList;
        }

        public int QuadraticSize(int numFeatures, int index, double scale)
        {
            var fcount = numFeatures;
            var yintercept = Math.Pow(scale * fcount / 2.0f, 2);

            var val = scale * (index - fcount / 2.0f);

            return (int)-Math.Pow(val, 2.0) + (int)yintercept;
        }
    }
}
