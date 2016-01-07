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
    public class CheckConfigSAT : ICheckConfigSAT
    {
        private CompositionContainer _container;
        
        [ImportMany]
        IEnumerable<Lazy<ICheckConfigSAT,ISolverType>> solvers;
        
        public CheckConfigSAT(String pathToDll)
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(CheckConfigSAT).Assembly));
            String location = AppDomain.CurrentDomain.BaseDirectory;

#if release
            if (pathToDll != null && pathToDll.Length > 0)
                    location = pathToDll;
                else
                    location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release").Length)));

#else
            if (pathToDll != null && pathToDll.Length > 0)
                location = pathToDll;
            else
                location = location.Substring(0, (location.Length - ((Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug").Length)));
#endif

            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//Removing tailing dir sep
            location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//Removing project path
            //location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//Removing project path

#if release
            catalog.Catalogs.Add(new DirectoryCatalog(location));
            location = location + Path.DirectorySeparatorChar + "SolverFoundationWrapper" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Release";
#else
            location = location + Path.DirectorySeparatorChar + "SolverFoundationWrapper" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "Debug";
#endif
            catalog.Catalogs.Add(new DirectoryCatalog(location));
            //Create the CompositionContainer with the parts in the catalog

            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                GlobalState.logError.logLine(compositionException.ToString());
                //Console.WriteLine(compositionException.ToString());
            }
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="config">The list of binary options that are SELECTED (only selected options must occur in the list).</param>
        /// <param name="vm">The variability model that represents the context of the configuration.</param>
        /// <param name="vm">Whether the given list of options represents only a partial configuration. This means that options not in config might be additionally select to obtain a valid configuration.</param>
        /// <returns>True if it is a valid selection w.r.t. the VM, false otherwise</returns>
        public bool checkConfigurationSAT(List<BinaryOption> config, VariabilityModel vm, bool partialConfiguration)
        {
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.checkConfigurationSAT(config, vm, partialConfiguration);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                return solver.Value.checkConfigurationSAT(config, vm, partialConfiguration);
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
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.checkConfigurationSAT(c, vm);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ICheckConfigSAT, ISolverType> solver in solvers)
            {
                return solver.Value.checkConfigurationSAT(c, vm);
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
        private static CheckConfigSAT ccs = new CheckConfigSAT(null);
    }
}
