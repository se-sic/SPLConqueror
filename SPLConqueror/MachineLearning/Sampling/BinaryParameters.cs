using System.Collections.Generic;

namespace MachineLearning.Sampling
{
    public class BinaryParameters
    {
        public List<Dictionary<string, string>> tWiseParameters { get; set; }
        public List<Dictionary<string, string>> randomBinaryParameters { get; set; }

        public BinaryParameters()
        {
            tWiseParameters = new List<Dictionary<string, string>>();
            randomBinaryParameters = new List<Dictionary<string, string>>();
        }

    }
}
