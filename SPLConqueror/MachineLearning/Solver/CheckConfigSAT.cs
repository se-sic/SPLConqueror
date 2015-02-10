using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using SPLConqueror_Core;
using MachineLearning.Learning.LinearProgramming;

namespace MachineLearning.Solver
{
    internal class CheckConfigSAT : ICheckConfigSAT
    {
        private CompositionContainer _container;
        
        [ImportMany]
        IEnumerable<Lazy<ICheckConfigSAT,ISolverType>> solvers;
        
        public CheckConfigSAT()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Learning.LinearProgramming.SolverLP).Assembly));
            String location = AppDomain.CurrentDomain.BaseDirectory;

            String commandLineOrSingleSPLConfig = "";
            if (location.Contains("CommandLine"))
                commandLineOrSingleSPLConfig = "CommandLine";
            if (location.Contains("SingleSPLConfig"))
                commandLineOrSingleSPLConfig = "SingleSPLConfig";

            if (location.Contains("CommandLine") || location.Contains("SingleSPLConfig"))
            {
#if release
                location = location.Substring(0, (location.Length - ((commandLineOrSingleSPLConfig + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release").Length)));
#else
                location = location.Substring(0, (location.Length - ((commandLineOrSingleSPLConfig + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug").Length)));
#endif
            }
            else
#if release

                location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release").Length)));
#else
                location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug").Length)));
#endif
            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));
#if release
            catalog.Catalogs.Add(new DirectoryCatalog(location + Path.DirectorySeparatorChar + "PLM" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release"));

#else
            String dir = location + Path.DirectorySeparatorChar + "PLM" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug";
            DirectoryInfo dinfo = new DirectoryInfo(dir);
            if(dinfo.Exists)
                catalog.Catalogs.Add(new DirectoryCatalog(dir));
            else
                catalog.Catalogs.Add(new DirectoryCatalog(location));
#endif
            //Create the CompositionContainer with the parts in the catalog

            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="config">The list of binary options that are SELECTED (only selected options must occur in the list).</param>
        /// <param name="vm">The variability model that represents the context of the configuration.</param>
        /// <returns>True if it is a valid selection w.r.t. the VM, false otherwise</returns>
        public bool checkConfigurationSAT(List<BinaryOption> config, VariabilityModel vm)
        {
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return checkConfigurationSAT(config, vm);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                return checkConfigurationSAT(config, vm);
            }

            return false;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="c">The configuration that needs to be checked.</param>
        /// <param name="vm">The variability model that represents the context of the configuration.</param>
        /// <returns>True if it is a valid selection w.r.t. the VM, false otherwise</returns>
        public bool checkConfigurationSAT(Configuration c, VariabilityModel vm)
        {
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return checkConfigurationSAT(c, vm);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                return checkConfigurationSAT(c, vm);
            }

            return false;
        }

        /*public List<Element> determineSetOfInvalidFeatures(int nbOfFeatures, FeatureModel fm, bool withDerivatives, List<Element> forbiddenFeatures, RuntimeProperty rp, NFPConstraint constraint)
        {
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return determineSetOfInvalidFeatures(nbOfFeatures,fm,withDerivatives,forbiddenFeatures,rp,constraint);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                return determineSetOfInvalidFeatures(nbOfFeatures, fm, withDerivatives, forbiddenFeatures, rp, constraint);
            }

            return null;
        }*/

        public static CheckConfigSAT execute()
        {
            return ccs;
        }
        private static CheckConfigSAT ccs = new CheckConfigSAT();
    }
}
