using MachineLearning.Sampling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Persistence
{
    public class PersistSampling
    {
        private PersistSampling() { }
        
        public static string dump(List<SamplingStrategies> sample)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<SamplingStrategies>));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, sample);
            return sw.ToString().Replace("utf-16", "utf-8");
        }

        public static List<SamplingStrategies> recoverFromDump(string path)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<SamplingStrategies>));
            StreamReader sr = new StreamReader(path);
            object toSample = xmls.Deserialize(sr);
            return (List<SamplingStrategies>)toSample;
        }

        public static List<SamplingStrategies> recoverToSampleFromLogFiles(string log)
        {
            return new List<SamplingStrategies>();
        }

        public static List<SamplingStrategies> recoverToValidationFromLogFiles(string log)
        {
            return new List<SamplingStrategies>();
        }
    }
}
