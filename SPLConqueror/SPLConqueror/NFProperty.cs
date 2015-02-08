using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class NFProperty : IEquatable<NFProperty> 
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }


        public NFProperty(string NFPname)
        {
            this.name = NFPname;
        }

        public override string ToString()
        {
            return name;
        }

        public bool Equals(NFProperty other)
        {
            return this.name.Equals(other.name);
        }



    }
}
