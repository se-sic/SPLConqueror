using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class MixedConstraint : NonBooleanConstraint
    {
        private const string REQUIRE_ALL = "all";

        private const string REQUIRE_ONE = "one";

        private const string NEGATIVE = "neg";

        private const string POSITIVE = "pos";

        private InfluenceFunction leftHandSide = null;

        private InfluenceFunction rightHandSide = null;

        private VariabilityModel var;

        private string requirement;

        private string negativeOrPositiveExpr;

        public MixedConstraint(String unparsedExpr, VariabilityModel vm, VariabilityModel varMod, string requirement, string exprKind = "pos") : base(unparsedExpr, vm)
        {
            if (requirement.Trim().ToLower().Equals(REQUIRE_ALL))
            {
                this.requirement = REQUIRE_ALL;
            }
            else if (requirement.Trim().ToLower().Equals(REQUIRE_ONE))
            {
                this.requirement = REQUIRE_ONE;
            }
            else
            {
                throw new ArgumentException(String.Format("The tag {0} for mixed requirements is not valid.", requirement));
            }

            if (exprKind.Trim().ToLower().Equals(NEGATIVE))
            {
                this.negativeOrPositiveExpr = NEGATIVE;
            } else if (exprKind.Trim().ToLower().Equals(POSITIVE))
            {
                this.negativeOrPositiveExpr = POSITIVE;
            } else
            {
                throw new ArgumentException(String.Format("The expression kind {0} is not valid. Expression can either be neg or pos.", exprKind));
            }

            String[] parts = base.ToString().Split(new string[] { ">", "<", "=", "<=", ">=" }, StringSplitOptions.None);
            leftHandSide = new InfluenceFunction(parts[0], varMod);
            rightHandSide = new InfluenceFunction(parts[parts.Length - 1], varMod);
            var = varMod;
        }

        public bool requirementsFulfilled(Configuration conf)
        {
            if (negativeOrPositiveExpr.Equals(POSITIVE))
            {
                return evaluatePos(conf);
            }
            else if (negativeOrPositiveExpr.Equals(NEGATIVE))
            {
                return !evaluatePos(conf);
            }
            else
            {
                throw new ArgumentException("Illegal expression kind");
            }
        }

        private bool evaluatePos(Configuration conf)
        {
            if (requirement.Equals(REQUIRE_ALL))
            {
                return base.configIsValid(conf);
            }
            else if (requirement.Equals(REQUIRE_ONE))
            {
                Tuple<bool, bool> preCheckResult = preCheckConfigReqOne(conf);
                bool hasAllConfigs = preCheckResult.Item1;
                bool hasAtLeastOneConfig = preCheckResult.Item2;

                if (!hasAtLeastOneConfig)
                {
                    return true;
                }
                else if (!hasAllConfigs && hasAtLeastOneConfig)
                {
                    return false;
                }
                else
                {
                    return base.configIsValid(conf);
                }
            }
            else
            {
                throw new ArgumentException("Illegal Reuqirement for mixed constraints");
            }
        }

        private Tuple<bool, bool> preCheckConfigReqOne(Configuration config)
        {
            bool hasAllConfigs = true;
            bool hasAtLeastOne = false;
            foreach (BinaryOption bo in leftHandSide.participatingBoolOptions.Union(rightHandSide.participatingBoolOptions))
            {
                if (!config.BinaryOptions.ContainsKey(bo))
                {
                    hasAllConfigs = false;
                }
                else if (config.BinaryOptions.ContainsKey(bo))
                {
                    hasAtLeastOne = true;
                }


            }

            foreach (NumericOption no in leftHandSide.participatingNumOptions.Union(rightHandSide.participatingNumOptions))
            {
                InfluenceFunction func = new InfluenceFunction(no.ToString(), GlobalState.varModel);
                if (!config.NumericOptions.ContainsKey(no))
                {
                    hasAllConfigs = false;

                }
                else if (func.eval(config) == 0)
                {
                    hasAllConfigs = false;
                }
                else if (config.NumericOptions.ContainsKey(no))
                {
                    hasAtLeastOne = true;
                }
            }
            return Tuple.Create(hasAllConfigs, hasAtLeastOne);
        }

        public override string ToString()
        {
            return requirement + ": " + base.ToString();
        }
    }
}
