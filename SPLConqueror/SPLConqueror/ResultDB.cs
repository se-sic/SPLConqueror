using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class ResultDB
    {

        private List<Configuration> configurations = new List<Configuration>();

        public List<Configuration> Configurations
        {
            get { return configurations; }
            set { configurations = value; }
        }


        public void add(Configuration configuration)
        {
            this.configurations.Add(configuration);
        }

    }
}
