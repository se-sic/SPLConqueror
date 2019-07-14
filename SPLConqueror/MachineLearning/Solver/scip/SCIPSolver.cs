using SPLConqueror_Core;
using System;
using System.Collections.Generic;
// Option 1: Use it like a normal library. However requires at least the C# part of the interface at compile time
//using SCIPExternal;
using System.Reflection;
using System.IO;

namespace MachineLearning.Solver.scip
{
    public class SCIPSolver
    {
        public static Type var;
        public static Type solution;
        public static Type scipEnvType;
        private dynamic solver;
        private dynamic problemVariables;
    //    private SCIP solver;
    //    private Var[] problemVariables;

        private Dictionary<String, int> numericOptionToPseudoOptionID = new Dictionary<string, int>();
        private Dictionary<string, Tuple<ConfigurationOption, int>> nameToOptionID = new Dictionary<string, Tuple<ConfigurationOption, int>>();

        public SCIPSolver(string interfaceLibPath)
        {
            // Option 2: Use it by dynamically loading at runtime, only requires dlls when actually used
            Assembly scipDll = Assembly.LoadFile(interfaceLibPath + "SharpScip.dll");

            foreach (Type t in scipDll.GetExportedTypes())
            {
                if (t.Name == "Var")
                {
                    var = t;
                } else if (t.Name == "SCIP")
                {
                    scipEnvType = t;
                } else if (t.Name == "Sol")
                {
                    solution = t;
                }
            }
        }

        public void createModelForBestConfiguration(InfluenceFunction infMod, VariabilityModel vm, string additionalConstraintFile, bool maximization)
        {
            OsilGenerator generator = new OsilGenerator();
            string osilContent = generator.generateOsil_Syntax(vm, infMod, additionalConstraintFile,
                numericOptionToPseudoOptionID, nameToOptionID, maximization);
            string tmpFileName = Path.GetTempPath() + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".osil";
            File.AppendAllText(tmpFileName, osilContent);
            //solver = new SCIP(tmpFileName, "osil");
            solver = Activator.CreateInstance(scipEnvType, tmpFileName, "osil");
            File.Delete(tmpFileName);
        }

        public void solveProblem()
        {
            problemVariables = solver.getVars();
            solver.solve();
        }

        public Configuration getBestConfiguration(VariabilityModel vm)
        {
            if (solver.getNSols() == 0)
            {
                return null;
            } else
            {
                dynamic bestSolution = solver.getBestSol();
                List<BinaryOption> bestBinOpts = new List<BinaryOption>();
                Dictionary<NumericOption, double> bestNumOpts = new Dictionary<NumericOption, double>();
                foreach (dynamic problemVariable in problemVariables)
                {
                    double value = bestSolution.getSolVal(problemVariable);
                    ConfigurationOption opt = vm.getOption(problemVariable.Name);
                    if (opt is BinaryOption && value == 1)
                    {
                        bestBinOpts.Add((BinaryOption)opt);
                    } else if (opt is NumericOption)
                    {
                        bestNumOpts.Add((NumericOption)opt, value);
                    }
                }
                return new Configuration(bestBinOpts, bestNumOpts);
            }
        }
    }
}
