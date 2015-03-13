using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptGenerator
{
    class Container
    {
        string elementType = "";

        public String Type
        {
            get { return elementType; }
        }
        object element = null;

        public object Content
        {
            get { return element; }
        }

        private string additionalKeyInformation = "";

        public string AdditionalKeyInformation
        {
            get { return additionalKeyInformation; }
            set { additionalKeyInformation = value; }
        }


        List<Container> additionalInformation = null;

        internal List<Container> AdditionalInformation
        {
            get { return additionalInformation; }
        }

        public Container()
        {
        }

        public Container(string type, object content)
        {
            this.elementType = type;
            this.element = content;
        }

        public override string ToString()
        {
            return elementType + additionalKeyInformation;
        }

        public void addAdditionalInformation(Container newInformation)
        {
            if (this.additionalInformation == null)
                this.additionalInformation = new List<Container>();

            this.additionalInformation.Add(newInformation);
        }

    }
}
