using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;

namespace MachineLearning.Learning.Regression
{
    public class Feature : InfluenceFunction
    {
        public override bool Equals(object obj)
        {
            Feature other = (Feature) obj;
            return base.Equals(obj);
        }
    }
}
