using System;
using System.IO;
using NUnit.Framework;
using SPLConqueror_Core;
using Util;

namespace UtilTest
{
    [TestFixture]
    public class ConvertingUtilTest
    {

        private VariabilityModel vm;

        [OneTimeSetUp]
        public void initVariabilityModel()
        {
            VariabilityModel vm = new VariabilityModel("test");
            NumericOption numOpt = new NumericOption(vm, "testNum");
            numOpt.SetValues(new double[] { 0, 5, 10, 15, 20, 25 });
            numOpt.Min_value = 0;
            numOpt.Max_value = 25;
            numOpt.Parent = vm.Root;
            BinaryOption binOpt = new BinaryOption(vm, "testBin");
            binOpt.Optional = true;
            binOpt.Parent = vm.Root;
            vm.addConfigurationOption(numOpt);
            vm.addConfigurationOption(binOpt);
            vm.NonBooleanConstraints.Add(new NonBooleanConstraint("testNum > 0", vm));
            GlobalState.varModel = vm;
            this.vm = vm;
        }

        private bool testContainsAll(string toTest, params string[] values)
        {
            foreach (string val in values)
            {
                if (!toTest.Contains(val))
                    return false;
            }
            return true;
        }

        [Test]
        public void CSVtoBinaryOnlyTest()
        {
            string toConvert = "toConvert.csv";
            string converted = "converted.csv";
            string configStringCSV = "root;testBin;testNum;NFP\n1;0;5;15";
            MachineLearningTest.Util.printToTmpFile(toConvert, configStringCSV);

            ConvertUtil.convertToBinaryCSV(Path.GetTempPath() + toConvert, 
                Path.GetTempPath() + converted, vm);
            
            string result = MachineLearningTest.Util.readTmpFile(converted);
            string header = result.Split(new char[] { '\n' })[0].Trim();
            string config = result.Split(new char[] { '\n' })[1].Trim();
            MachineLearningTest.Util.cleanUpTmpFiles(converted, toConvert);

            Assert.That(
                testContainsAll(result, "testNum_0", "testNum_5", "testNum_10"
                ,"testNum_15", "testNum_20", "testNum_25")
            );

            int idx = 0;
            string[] options = header.Split(new char[] { ';' });
            for (int i = 0; i <  options.Length; i++)
            {
                if (options[i] == "testNum_5")
                {
                    idx = i;
                    break;
                }
            }
            Assert.AreEqual("1", config.Split(new char[] { ';' })[idx]);
        }

        [Test]
        public void testConvertXMLtoBinaryOnly()
        {
            string toConvert = "<results>" +
                                 "<row>" +
                                   "<data columname=\"Configuration\">root, testBin,</data>" +
                                   "<data columname=\"NumericOptions\">testNum;5</data>" +
                                   "<data columname=\"NFP\">15</data>" +
                                 "</row>" +
                               "</results>";
            string toConvertFile = "toConvert.xml";
            string convertedFile = "converted.xml";
            MachineLearningTest.Util.printToTmpFile(toConvertFile, toConvert);
            ConvertUtil.convertToBinaryXml(Path.GetTempPath() + toConvertFile,
                Path.GetTempPath() + convertedFile);
            string result = MachineLearningTest.Util.readTmpFile(convertedFile);
            MachineLearningTest.Util.cleanUpTmpFiles(toConvertFile, convertedFile);

            Assert.That(testContainsAll(result, "testNum_5", "testNum", "testBin"));
            Assert.IsFalse(result.Contains("testNum;"));
            Assert.IsFalse(result.Contains("testNum_10"));
        }

        [Test]
        public void testTransformVMtoAllBinary()
        {
            VariabilityModel transformed = ConvertUtil.transformVarModelAllbinary(vm);
            Assert.AreEqual(0, transformed.NumericOptions.Count);
            Assert.AreEqual(9, transformed.BinaryOptions.Count);
            Assert.That(transformed.getBinaryOption("testNum_5") != null);
            Assert.That(transformed.BinaryConstraints.Contains("!testNum_0"));
            Assert.That(transformed.getBinaryOption("testNum_5")
                .Parent.Equals(transformed.getBinaryOption("testNum")));
        }

        [Test]
        public void testParseDimacs()
        {
            VariabilityModel binOnly = ConvertUtil.transformVarModelAllbinary(vm);
            string parsedToDimacs = ConvertUtil.parseToDimacs(binOnly);
            string[] expressions = parsedToDimacs.Split(new char[] { '\n' });
            Assert.AreEqual(38, parsedToDimacs.Split(new char[] { '\n' }).Length);
            // Test if the general structure was parsed to dimacs properly
            Assert.That(testContainsAll(parsedToDimacs, 
                "c 3 testNum", "c 4 testNum_0", "p cnf 9 27", "1 0", "3 0", "1 -2 0", "5 6 7 8 9 4 -3 0", "-5 -8 0"));
            // Test if the constraint was properly parsed
            Assert.That(parsedToDimacs.Contains("-4 0"));
        }
    }
}
