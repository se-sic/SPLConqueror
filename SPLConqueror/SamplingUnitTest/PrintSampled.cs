using CommandLine;
using NUnit.Framework;
using System.IO;

namespace SamplingUnitTest
{
    [TestFixture]
    public class PrintSampled
    {
        [Test]
        public void TestPrintConfigs()
        {
            string modelPath = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..//..//..."))
            + Path.DirectorySeparatorChar + "ExampleFiles"
            + Path.DirectorySeparatorChar + "VariabilityModelSampling.xml";
            if (!File.Exists(modelPath))
            {
                modelPath = "/home/travis/build/se-passau/SPLConqueror/SPLConqueror/Example"
                  + "Files/VariabilityModelSampling.xml";
            }
            Commands cmd = new Commands();
            cmd.performOneCommand(Commands.COMMAND_VARIABILITYMODEL + " " + modelPath);
            cmd.performOneCommand(Commands.COMMAND_SAMPLE_FEATUREWISE);
            cmd.performOneCommand(Commands.COMMAND_EXPERIMENTALDESIGN + " " + Commands.COMMAND_EXPDESIGN_BOXBEHNKEN);
            cmd.performOneCommand(Commands.COMMAND_PRINT_CONFIGURATIONS + " " 
                + System.AppDomain.CurrentDomain.BaseDirectory + "testSampledConfigs.txt");
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory 
                + "testSampledConfigs.txt");
            string line = "";
            int count = 1;

            while (!sr.EndOfStream)
            {
                if (!line.Equals(""))
                {
                    count += 1;
                }

                line = sr.ReadLine();

                if (count == 1)
                {
                    Assert.AreEqual("prefix \"root%;%binOpt1%;%binOpt2%;%group%;"
                        + "%member3%;%group2%;%member23%;%numOpt1;0%;%numOpt2;1%;%\"    " 
                        + "   num1-0-- 1  postfix ", line);
                }
            }
            Assert.AreEqual(50, count);
        }
    }
}
