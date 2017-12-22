using System;
using System.Collections.Generic;
using Microsoft.Z3;
using SPLConqueror_Core;

namespace MachineLearning.Solver
{
    class Z3Cache
    {
        private Context _context;
        private Microsoft.Z3.Optimize _optimizer;
        private List<BoolExpr> _variables;
        private Dictionary<BinaryOption, BoolExpr> _optionToTerm;
        private Dictionary<BoolExpr, BinaryOption> _termToOption;

        /// <summary>
        /// This constructor creates a new <see cref="ConstraintSystemCache"/> with the given parameters.
        /// </summary>
        /// <param name="context">The <see cref="Context"/> to store.</param>
        /// <param name="solver">The <see cref="Microsoft.Z3.Optimize"/>-object to use.</param>
        /// <param name="variables">The variables used in the <see cref="Context"/>.</param>
        /// <param name="optionToTerm">The mapping from <see cref="BinaryOption"/> to <see cref="BoolExpr"/>.</param>
        /// <param name="termToOption">The mapping from <see cref="BoolExpr"/> to <see cref="BinaryOption"/>.</param>
        public Z3Cache(Context context, Optimize optimizer, List<BoolExpr> variables, Dictionary<BinaryOption, BoolExpr> optionToTerm, Dictionary<BoolExpr, BinaryOption> termToOption)
        {
            this._context = context;
            this._optimizer = optimizer;
            this._variables = variables;
            this._optionToTerm = optionToTerm;
            this._termToOption = termToOption;
        }

        /// <summary>
        /// Returns the stored constraint system.
        /// </summary>
        /// <returns>the stored constraint system.</returns>
        public Context GetContext()
        {
            return this._context;
        }

        /// <summary>
        /// Returns the solver to add further constraints to it.
        /// </summary>
        /// <returns>the solver to add further constraints (e.g. the already sampled configurations and the weight of them).</returns>
        public Optimize GetOptimizer()
        {
            return this._optimizer;
        }

        /// <summary>
        /// Returns the variables used in the constraint system.
        /// </summary>
        /// <returns>the used variables from the constraint system.</returns>
        public List<BoolExpr> GetVariables()
        {
            return this._variables;
        }

        /// <summary>
        /// Returns the mapping from <see cref="BinaryOption"/> to <see cref="CspTerm"/>.
        /// </summary>
        /// <returns>the mapping from <see cref="BinaryOption"/> to <see cref="CspTerm"/>.</returns>
        public Dictionary<BinaryOption, BoolExpr> GetOptionToTermMapping()
        {
            return this._optionToTerm;
        }

        /// <summary>
        /// Returns the mapping from <see cref="CspTerm"/> to <see cref="BinaryOption"/>.
        /// </summary>
        /// <returns>the mapping from <see cref="CspTerm"/> to <see cref="BinaryOption"/>.</returns>
        public Dictionary<BoolExpr, BinaryOption> GetTermToOptionMapping()
        {
            return this._termToOption;
        }
    
}
}
