using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.LinearProgramming
{
    public interface ISolverLP
    {
        /// <summary>
        /// Computes the influence of all configuration options and interactions based on the measurements of the given result db. It uses linear programming (simplex) and is an exact algorithm.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing the variability model, all configuration options and interactions.</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <param name="evaluateFeatureInteractionsOnly">Only interactions are learned.</param>
        /// <param name="withDeviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <param name="deviation">(Not used) We can specifiy whether learned influences must be greater than a certain value (e.g., greater than measurement bias).</param>
        /// <returns>Returns the learned infleunces of each option in a map whereas the String (Key) is the name of the option / interaction.</returns>
        Dictionary<String, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db, bool evaluateFeatureInteractionsOnly, bool withDeviation, double deviation);

        /// <summary>
        /// Computes the influence of all configuration options based on the measurements of the given result db. It uses linear programming (simplex) and is an exact algorithm.
        /// </summary>
        /// <param name="nfp">The non-funcitonal property for which the influences of configuration options are to be computed. If null, we use the property of the global model.</param>
        /// <param name="infModel">The influence model containing options and interactions. The state of the model will be changed by the result of the process</param>
        /// <param name="db">The result database containing the measurements.</param>
        /// <returns>A map of binary options to their computed influences.</returns>
        Dictionary<BinaryOption, double> computeOptionInfluences(NFProperty nfp, InfluenceModel infModel, ResultDB db);

        //TODO
        //List<Element> solveForMultipleProperties(FeatureModel fm, bool optimized, List<RuntimeProperty> properties, List<NFPConstraint> constraints, RuntimeProperty goalProp, bool maximize);
    }
}
