using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using MachineLearning.Learning.LinearProgramming;
using SPLConqueror_Core;

namespace MachineLearning.Solver
{
    public class VariantGenerator : IVariantGenerator
    {
        private CompositionContainer _container;
       // CheckConfigSAT ccs = null;

        [ImportMany]
        IEnumerable<Lazy<IVariantGenerator, ISolverType>> solvers;

        public VariantGenerator(String pathToDll)
        {
        //    ccs = new CheckConfigSAT(null);
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(VariantGenerator).Assembly));
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
            //location = location.Substring(0, location.LastIndexOf(Path.DirectorySeparatorChar));//test for PerformancePrediction_GUI

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
                Console.WriteLine(compositionException.ToString());
            }
        }

        #region solver
        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="vm">The variability model containing the binary options and their constraints.</param>
        /// <returns>Returns a list of configurations, in which a configuration is a list of SELECTED binary options (deselected options are not present)</returns>
        public List<List<BinaryOption>> generateAllVariantsFast(VariabilityModel vm)
        {
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.generateAllVariantsFast(vm);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                return solver.Value.generateAllVariantsFast(vm);
            }

            return null;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="vm">The variability model containing the binary options and their constraints.</param>
        /// <param name="treshold">Maximum number of configurations</param>
        /// <param name="modulu">Each configuration that is % modulu == 0 is taken to the result set</param>
        /// <returns>Returns a list of configurations, in which a configuration is a list of SELECTED binary options (deselected options are not present</returns>
        public List<List<BinaryOption>> generateRandomVariants(VariabilityModel vm, int treshold, int modulu)
        {
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.generateRandomVariants(vm, treshold, modulu);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                return solver.Value.generateRandomVariants(vm, treshold, modulu);
            }

            return null;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unWantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them.</param>
        /// <returns>The valid configuration (or null if there is none) that satisfies the VM and the goal.</returns>
        public List<BinaryOption> minimizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unWantedOptions)
        {
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.minimizeConfig(config, vm, minimize, unWantedOptions);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                return solver.Value.minimizeConfig(config, vm, minimize, unWantedOptions);
            }

            return null;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="config">The (partial) configuration which needs to be expaned to be valid.</param>
        /// <param name="vm">Variability model containing all options and their constraints.</param>
        /// <param name="minimize">If true, we search for the smallest (in terms of selected options) valid configuration. If false, we search for the largest one.</param>
        /// <param name="unwantedOptions">Binary options that we do not want to become part of the configuration. Might be part if there is no other valid configuration without them</param>
        /// <returns>A list of configurations that satisfies the VM and the goal (or null if there is none).</returns>
        public List<List<BinaryOption>> maximizeConfig(List<BinaryOption> config, VariabilityModel vm, bool minimize, List<BinaryOption> unwantedOptions)
        {
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.maximizeConfig(config, vm, minimize, unwantedOptions);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                return solver.Value.maximizeConfig(config, vm, minimize, unwantedOptions);
            }

            return null;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="optionToBeRemoved">The binary configuration option that must not be part of the new configuration.</param>
        /// <param name="originalConfig">The configuration for which we want to find a similar one.</param>
        /// <param name="removedElements">If further options need to be removed from the given configuration to build a valid configuration, they are outputed in this list.</param>
        /// <param name="vm">The variability model containing all options and their constraints.</param>
        /// <returns>A configuration that is valid, similar to the original configuration and does not contain the optionToBeRemoved.</returns>
        public List<BinaryOption> generateConfigWithoutOption(BinaryOption optionToBeRemoved, List<BinaryOption> originalConfig, out List<BinaryOption> removedElements, VariabilityModel vm)
        {
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.generateConfigWithoutOption(optionToBeRemoved, originalConfig, out removedElements, vm);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<IVariantGenerator, ISolverType> solver in solvers)
            {
                return solver.Value.generateConfigWithoutOption(optionToBeRemoved, originalConfig, out removedElements, vm);
            }
            removedElements = null;
            return null;
        }
        #endregion

    }
}
