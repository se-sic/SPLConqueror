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

        /// <summary>
        /// Parse sampling strategies to string.
        /// </summary>
        /// <param name="sample">List of sampling strategies that should be parsed.</param>
        /// <returns>String representation of the sampling strategies.</returns>
        public static string dump(List<SamplingStrategies> sample)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<SamplingStrategies>));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, sample);
            return sw.ToString().Replace("utf-16", "utf-8");
        }

        /// <summary>
        /// Parsed saved sampling strategies to SamplingStrategies objects.
        /// </summary>
        /// <param name="path">The path the sampling strategies are saved at.</param>
        /// <returns>List of the recovered sampling strategies.</returns>
        public static List<SamplingStrategies> recoverFromDump(string path)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<SamplingStrategies>));
            StreamReader sr = new StreamReader(path);
            object toSample = xmls.Deserialize(sr);
            return (List<SamplingStrategies>)toSample;
        }
    }
}
