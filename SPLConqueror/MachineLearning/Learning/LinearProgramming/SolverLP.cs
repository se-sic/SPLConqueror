using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using SPLConqueror_Core;
using System.IO;

namespace MachineLearning.Learning.LinearProgramming
{
    internal class SolverLP : ISolverLP
    {
        private CompositionContainer _container;
        
        [ImportMany]
        IEnumerable<Lazy<ISolverLP,ISolverType>> solvers;
        
        public SolverLP(String pathToDll)
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(SolverLP).Assembly));
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

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing options and interactions. The state of the model will be changed by the result of the process</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <returns>A map of binary options to their computed influences.</returns>
        public Dictionary<BinaryOption, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db)
        {
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.computeOptionInfluences(nfp, infModel, db);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                return solver.Value.computeOptionInfluences(nfp, infModel, db);
            }

            return null;
        }

        /// <summary>
        /// This method searches for a corresponding methods in the dynamically loadeda assemblies and calls it if found. It prefers due to performance reasons the Microsoft Solver Foundation implementation.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing the variability model, all configuration options and interactions.</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <param name="evaluateFeatureInteractionsOnly">Only interactions are learned.</param>
        /// <param name="withDeviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <param name="deviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <returns>Returns the learned infleunces of each option in a map whereas the String (Key) is the name of the option / interaction.</returns>
        public Dictionary<string, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db, bool evaluateFeatureInteractionsOnly, bool withDeviation, double deviation)
        {
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.computeOptionInfluences(nfp, infModel, db, evaluateFeatureInteractionsOnly, withDeviation, deviation);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                return solver.Value.computeOptionInfluences(nfp, infModel, db, evaluateFeatureInteractionsOnly, withDeviation, deviation);
            }
            return null;
        }

        /*public List<Element> solveForMultipleProperties(FeatureModel fm, bool optimized, List<RuntimeProperty> properties, List<NFPConstraint> constraints, RuntimeProperty goalProp, bool maximize)
        {
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                if (solver.Metadata.SolverType.Equals("MSSolverFoundation")) return solver.Value.solveForMultipleProperties(fm, optimized, properties, constraints, goalProp, maximize);
            }

            //If not MS Solver, take any solver. Should be changed when supporting more than 2 solvers here
            foreach (Lazy<ISolverLP, ISolverType> solver in solvers)
            {
                return solver.Value.solveForMultipleProperties(fm, optimized, properties, constraints, goalProp, maximize);
            }
            return null;
        }*/
    }
}
