using MachineLearning;
using MachineLearning.Learning.Regression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Persistence
{
    public class PersistLearning
    {
        private PersistLearning() { }

        public const string HEADER = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        public static string dump(MachineLearning.Learning.Regression.Learning exp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<learning>\n");
            XmlSerializer xmls = new XmlSerializer(typeof(LearningInfo));
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, exp.info);
            sb.Append(sw.ToString().Replace(HEADER, ""));
            foreach(FeatureSubsetSelection sel in exp.models)
            {
                sb.Append("<subsetSelection>\n");
                foreach(LearningRound round in sel.LearningHistory)
                {
                    xmls = new XmlSerializer(typeof(LearningRound));
                    sw = new StringWriter();
                    xmls.Serialize(sw, round);
                    sb.Append(sw.ToString().Replace(HEADER, ""));
                }
                sb.Append(sel.getCurrentInfo());
                sb.Append("</subsetSelection>\n");
            }
            return sb.Append("</learning>\n").ToString().Replace(HEADER, "");
        }
    }
}
