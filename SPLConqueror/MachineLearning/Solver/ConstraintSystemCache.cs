using Microsoft.SolverFoundation.Solvers;
using SPLConqueror_Core;
using System.Collections.Generic;

namespace MachineLearning.Solver
{
    /// <summary>
    /// This class contains all information needed for reusing a <see cref="ConstraintSystemCache"/>.
    /// </summary>
    class ConstraintSystemCache
    {
        private ConstraintSystem _cs;
        private List<CspTerm> _variables;
        private Dictionary<BinaryOption, CspTerm> _elemToTerm;
        private Dictionary<CspTerm, BinaryOption> _termToElem;

        /// <summary>
        /// This constructor creates a new <see cref="ConstraintSystemCache"/> with the given parameters.
        /// </summary>
        /// <param name="cs">The <see cref="ConstraintSystem"/> to store.</param>
        /// <param name="variables">The variables used in the <see cref="ConstraintSystem"/>.</param>
        /// <param name="elemToTerm">The mapping from <see cref="BinaryOption"/> to <see cref="CspTerm"/>.</param>
        /// <param name="termToElem">The mapping from <see cref="CspTerm"/> to <see cref="BinaryOption"/>.</param>
        public ConstraintSystemCache(ConstraintSystem cs, List<CspTerm> variables, Dictionary<BinaryOption, CspTerm> elemToTerm, Dictionary<CspTerm, BinaryOption> termToElem)
        {
            this._cs = cs;
            this._variables = variables;
            this._elemToTerm = elemToTerm;
            this._termToElem = termToElem;
        }

        /// <summary>
        /// Returns the stored constraint system.
        /// </summary>
        /// <returns>the stored constraint system.</returns>
        public ConstraintSystem GetConstraintSystem()
        {
            return this._cs;
        }

        /// <summary>
        /// Returns the variables used in the constraint system.
        /// </summary>
        /// <returns>the used variables from the constraint system.</returns>
        public List<CspTerm> GetVariables()
        {
            return this._variables;
        }

        /// <summary>
        /// Returns the mapping from <see cref="BinaryOption"/> to <see cref="CspTerm"/>.
        /// </summary>
        /// <returns>the mapping from <see cref="BinaryOption"/> to <see cref="CspTerm"/>.</returns>
        public Dictionary<BinaryOption, CspTerm> GetElemToTermMapping()
        {
            return this._elemToTerm;
        }

        /// <summary>
        /// Returns the mapping from <see cref="CspTerm"/> to <see cref="BinaryOption"/>.
        /// </summary>
        /// <returns>the mapping from <see cref="CspTerm"/> to <see cref="BinaryOption"/>.</returns>
        public Dictionary<CspTerm, BinaryOption> GetTermToElemMapping()
        {
            return this._termToElem;
        }
    }
}
