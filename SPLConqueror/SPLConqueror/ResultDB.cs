using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    class ResultDB
    {

        List<Configuration> configurations = new List<Configuration>();


        public void add(Configuration configuration)
        {
            this.configurations.Add(configuration);
        }

    }
}
