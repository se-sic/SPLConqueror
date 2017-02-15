using MachineLearning;
using MachineLearning.Learning.Regression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
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
            foreach(FeatureSubsetSelection sel in exp.models)
            {
                sb.Append("<subset>");
                foreach(LearningRound round in sel.LearningHistory)
                {
                    sb.Append("<LearningRound>\n");
                    sb.Append(round.ToString());
                    sb.Append("</LearningRound>\n");
                }
                sb.Append("</subset>");
            }
            return sb.Append("</learning>\n").ToString().Replace(HEADER, "");
        }

        public static List<List<string>> recoverFromPersistentDump(string path)
        {
            XmlDocument persistentLearning = new System.Xml.XmlDocument();
            persistentLearning.Load(path);
            List<List<string>> recoveredLearningRounds = new List<List<string>>();
            XmlElement learning = persistentLearning.DocumentElement;

            foreach (XmlElement learningRound in learning)
            {
                List<string> roundList = new List<string>();
                foreach (XmlElement round in learningRound)
                {
                    roundList.Add(learningRound.InnerText.Trim());
                }
                recoveredLearningRounds.Add(roundList);
            }
            return recoveredLearningRounds;
        }
    }
}
