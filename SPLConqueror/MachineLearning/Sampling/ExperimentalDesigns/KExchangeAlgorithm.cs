using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;
using ILNumerics;

namespace MachineLearning.Sampling.ExperimentalDesigns
{
    public class KExchangeAlgorithm : ExperimentalDesign
    {

        private double[,] matrix;
        private int sampleSize = 100;

        public int SampleSize
        {
            get { return sampleSize; }
            set { sampleSize = value; }
        }
        private Dictionary<NumericOption, int> numberOfLevels;
        private int k = 5;

        public int K
        {
            get { return k; }
            set { k = value; }
        }
        private bool rescale = false;
        private double epsilon = 1E-5;

        private static Dictionary<string, string> parameter = new Dictionary<string, string>();
        static KExchangeAlgorithm()
        {
            parameter.Add("sampleSize", "int");
            parameter.Add("k", "int");
        }
       
        public override Dictionary<string, string> getParameterTypes()
        {
            return KExchangeAlgorithm.parameter;
        }


        public override string getName()
        {
            return "kExchangeAlgorithm";
        }

        public KExchangeAlgorithm(List<NumericOption> options) 
            : base(options){
        }


        public override bool computeDesign(Dictionary<string, string> designOptions)
        {
            foreach (KeyValuePair<string, string> param in designOptions)
            {
                if (param.Key == "k")
                    k = Convert.ToInt32(param.Value);
                if (param.Key == "sampleSize")
                    sampleSize = Convert.ToInt32(param.Value);
            }

            return compute(sampleSize, k);
        }

        public override bool computeDesign() {
            return compute(this.sampleSize, this.k);
        }



        public bool compute(int _sampleSize, int _k)
        {
            if (options.Count == 0)
                return false;
          
            // set number of possible levels for each VariableFeature
            numberOfLevels = new Dictionary<NumericOption, int>();
            foreach (NumericOption vf in options)
            {
                int posLevels = vf.getAllValues().Count;

                if (!rescale || posLevels <= 4) // include all levels if # <= 4
                {
                    numberOfLevels.Add(vf, posLevels);
                }
                else
                {
                    numberOfLevels.Add(vf, Convert.ToInt32(4 + Math.Round(Math.Sqrt(posLevels) / 2.0))); // rescale
                }
            }

            // get full factorial design
            double[,] fullFactorial = getFullFactorial(numberOfLevels);

            // create initial random design
            Random rnd = new Random(1);
            Dictionary<int, int> usedCandidates = new Dictionary<int, int>();
            matrix = new double[_sampleSize, options.Count];

            // chose initial candidates from full factorial
            do
            {
                if (usedCandidates.Count >= _sampleSize)
                    break;

                int candidate = rnd.Next(0, fullFactorial.GetLength(0));

                if (!usedCandidates.Values.Contains(candidate))
                {
                    int tmp = usedCandidates.Count;
                    setRowOfMatrixTo(matrix, tmp, fullFactorial, candidate);
                    usedCandidates.Add(tmp, candidate);
                }
            } while (true); // TODO > improve, not that deterministic

            // Calculate dispersion matrix
            ILArray<double> dispersion = calculateDispersion(matrix);
      
            // start of k-Exchange algorithm
            bool couplesWithPositiveDelta = true;


            while (couplesWithPositiveDelta)
            {

                Dictionary<int, double> variances = new Dictionary<int, double>();
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    variances.Add(i, calcVarianceForRunIL(getRowFromMatrix(matrix, i), dispersion));
                }

                couplesWithPositiveDelta = false;

                foreach (KeyValuePair<int, double> v in variances.OrderByDescending(p => p.Value).Take(_k))
                {

                    // Get deltas for exchange
                    Dictionary<int, double> deltaF = getAllDeltas(usedCandidates[v.Key], fullFactorial);

                    // Get rid of duplicates
                    deltaF = deltaF.Where(c => !usedCandidates.Values.Contains(c.Key)).ToDictionary(dict => dict.Key, dict => dict.Value);

                    // Get exchange candidates
                    KeyValuePair<int, double> eF = deltaF.OrderByDescending(p => p.Value).First();

                    if (eF.Value > epsilon)
                    {
                        setRowOfMatrixTo(matrix, v.Key, fullFactorial, eF.Key);
                        usedCandidates[v.Key] = eF.Key;
                        dispersion = calculateDispersion(matrix);
                        couplesWithPositiveDelta = true;
                    }
                }

            }

            // map configuration to valid values that can be passed to the software
            Dictionary<Tuple<NumericOption, int>, double> values = new Dictionary<Tuple<NumericOption, int>, double>();
            foreach (NumericOption opt in options)
            {
                int maxVal = numberOfLevels[opt] - 1;
                double delta = opt.Max_value - opt.Min_value;

                for (int i = 0; i <= maxVal; i++)
                {
                    double value = opt.Min_value + (i / (double)maxVal) * delta; // TODO > round if parameters have to be integers
                    value = opt.nearestValidValue(value);
                    values.Add(Tuple.Create(opt, i), value);
                }
            }

            List<Dictionary<NumericOption, double>> configs = new List<Dictionary<NumericOption, double>>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Dictionary<NumericOption, double> run = new Dictionary<NumericOption, double>();

                int j = 0;
                foreach (NumericOption vf in options)
                {
                    run.Add(vf, values[Tuple.Create(vf, Convert.ToInt32(matrix[i, j]))]);
                    j++;
                }

                configs.Add(run);
            }

            return true;
        }

        // generate all possible run configurations - plagiarized from FullFactorialDesign
        private double[,] getFullFactorial(Dictionary<NumericOption, int> numberOfLevels)
        {
            double[][] fullFactorial = new double[Convert.ToInt32(numberOfLevels.Values.Aggregate((a, x) => a * x))][];

            Dictionary<NumericOption, List<int>> elementValuePairs = new Dictionary<NumericOption, List<int>>();
            foreach (NumericOption vf in numberOfLevels.Keys)
            {
                elementValuePairs[vf] = Enumerable.Range(0, numberOfLevels[vf]).ToList();
            }

            int[] positions = new int[numberOfLevels.Count];

            int featureToIncrement = 0;
            bool notIncremented = true;

            int iterator = 0;

            do
            {
                notIncremented = true;

                // tests whether all combinations are computed
                if (featureToIncrement == numberOfLevels.Count)
                {
                    double[,] tmp = new double[fullFactorial.Length, fullFactorial[0].Length];
                    for (int i = 0; i < fullFactorial.Length; i++)
                    {
                        for (int j = 0; j < fullFactorial[0].Length; j++)
                        {
                            tmp[i, j] = fullFactorial[i][j];
                        }
                    }

                    return tmp;
                }
                fullFactorial[iterator] = generateRow(elementValuePairs, positions);
                iterator++;

                do
                {
                    if (positions[featureToIncrement] == numberOfLevels.ElementAt(featureToIncrement).Value - 1)
                    {
                        positions[featureToIncrement] = 0;
                        featureToIncrement++;
                    }
                    else
                    {
                        positions[featureToIncrement] = positions[featureToIncrement] + 1;
                        notIncremented = false;
                        featureToIncrement = 0;
                    }
                } while (notIncremented && !(featureToIncrement == numberOfLevels.Count));
            } while (true);
        }

        private double[] generateRow(Dictionary<NumericOption, List<int>> numberOfLevels, int[] positions)
        {
            double[] run = new double[numberOfLevels.Count];

            for (int i = 0; i < numberOfLevels.Count; i++)
            {
                run[i] = numberOfLevels.ElementAt(i).Value[positions[i]];
            }
            return run;
        }

        private ILArray<double> calculateDispersion(double[,] matrix)
        {
            ILArray<double> tmp = matrix;
            return ILMath.pinv(ILMath.multiply(tmp, tmp.T));
        }

        private double[] getRowFromMatrix(double[,] matrix, int row)
        {
            double[] tmp = new double[matrix.GetLength(1)];
            int offset = 8 * matrix.GetLength(1) * row;
            int blocksize = 8 * matrix.GetLength(1);
            System.Buffer.BlockCopy(matrix, offset, tmp, 0, blocksize);

            return tmp;
        }

        private void setRowOfMatrixTo(double[,] matrix1, int row1, double[,] matrix2, int row2)
        {
            int offset1 = 8 * matrix1.GetLength(1) * row1;
            int offset2 = 8 * matrix1.GetLength(1) * row2;
            int blocksize = 8 * matrix1.GetLength(1);
            System.Buffer.BlockCopy(matrix2, offset2, matrix1, offset1, blocksize);
        }

        private double calcVarianceForRun(double[] run, double[,] candidateMatrix)
        {
            ILArray<double> vector = run;
            ILArray<double> matrix = candidateMatrix;
            ILArray<double> inverse = ILMath.pinv(ILMath.multiply(matrix, matrix.T));

            return (double)ILMath.multiply(ILMath.multiply(vector.T, inverse), vector);
        }

        private double calcVarianceForRunIL(double[] run, ILArray<double> dispersion)
        {
            ILArray<double> vector = run;
            return (double)ILMath.multiply(ILMath.multiply(vector.T, dispersion), vector);
        }

        private double calcVarianceFunction(double[] xi, double[] xj, double[,] matrix)
        {
            ILArray<double> xiIL = xi;
            ILArray<double> xjIL = xj;
            ILArray<double> matrixIL = matrix;
            ILArray<double> inverse = ILMath.pinv(ILMath.multiply(matrixIL, matrixIL.T));

            return (double)ILMath.multiply(ILMath.multiply(xiIL.T, inverse), xjIL);
        }

        private double calcVarianceFunctionIL(double[] xi, double[] xj, ILArray<double> dispersion)
        {
            ILArray<double> xiIL = xi;
            ILArray<double> xjIL = xj;
            return (double)ILMath.multiply(ILMath.multiply(xiIL.T, dispersion), xjIL);
        }

        private double calcDelta(double[] xi, double[] xj, double[,] matrix)
        {
            double dxi = calcVarianceForRun(xi, matrix);
            double dxj = calcVarianceForRun(xj, matrix);
            double dxixj = calcVarianceFunction(xi, xj, matrix);
            return dxj - ((dxi * dxj) - Math.Pow(dxixj, 2)) - dxi;
        }

        private double calcDeltaIL(double[] xi, double[] xj, ILArray<double> dispersion)
        {
            double dxi = calcVarianceForRunIL(xi, dispersion);
            double dxj = calcVarianceForRunIL(xj, dispersion);
            double dxixj = calcVarianceFunctionIL(xi, xj, dispersion);
            return dxj - ((dxi * dxj) - Math.Pow(dxixj, 2)) - dxi;
        }

        private Dictionary<int, double> getAllDeltas(int row, double[,] matrix)
        {
            Dictionary<int, double> deltas = new Dictionary<int, double>();
            double[] vector = getRowFromMatrix(matrix, row);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == row)
                    continue;

                deltas.Add(i, calcDelta(vector, getRowFromMatrix(matrix, i), matrix));
            }

            return deltas;
        }
    }
}
