using System;
using NUnit.Framework;
using SPLConqueror_Core;
using System.Collections.Generic;

namespace SPLConquerorCoreTest
{
    [TestFixture]
    public class ConfigurationTest
    {
        [Test]
        public void TestConfigurationPositiv()
        {
            VariabilityModel vm = new VariabilityModel("test");
            initVM(vm);
            GlobalState.varModel = vm;
            Dictionary<NFProperty, double> measuredValuesFirst = new Dictionary<NFProperty, double>();
            measuredValuesFirst.Add(GlobalState.getOrCreateProperty("test"), 2);
            Dictionary<NFProperty, double> measuredValuesSecond = new Dictionary<NFProperty, double>();
            measuredValuesSecond.Add(GlobalState.getOrCreateProperty("test"), 3);
            Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptionsFirst =
                new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
            fillUpBinaryOptions(binaryOptionsFirst, vm);
            Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptionsSecond =
                new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
            fillUpBinaryOptions(binaryOptionsSecond, vm);
            Dictionary<NumericOption, double> numericOptionsFirst =
                new Dictionary<NumericOption, double>();
            fillUpNumericOptions(numericOptionsFirst, vm);
            Dictionary<NumericOption, double> numericOptionsSecond =
                new Dictionary<NumericOption, double>();
            fillUpNumericOptions(numericOptionsSecond, vm);
            Configuration configFirst = new Configuration(binaryOptionsFirst,
                numericOptionsFirst, measuredValuesFirst);
            Configuration configSecond = new Configuration(binaryOptionsSecond,
                numericOptionsSecond, measuredValuesSecond);
            Assert.AreEqual(configFirst, configSecond);
        }

        [Test]
        public void TestConfigurationNegativ()
        {
            VariabilityModel vm = new VariabilityModel("test");
            initVM(vm);
            GlobalState.varModel = vm;
            Dictionary<BinaryOption, BinaryOption.BinaryValue> binaryOptionsFirst =
                new Dictionary<BinaryOption, BinaryOption.BinaryValue>();
            fillUpBinaryOptions(binaryOptionsFirst, vm);
            Dictionary<NumericOption, double> numericOptionsFirst =
                new Dictionary<NumericOption, double>();
            fillUpNumericOptions(numericOptionsFirst, vm);
            Configuration firstConfig = new Configuration(binaryOptionsFirst, numericOptionsFirst);
            firstConfig.update();
            Configuration secondConfig = new Configuration(new Dictionary<BinaryOption, BinaryOption.BinaryValue>()
                , new Dictionary<NumericOption, double>());
            Assert.AreNotEqual(firstConfig.ToString(), secondConfig.ToString());
        }

        private static void fillUpBinaryOptions(Dictionary<BinaryOption, BinaryOption.BinaryValue> toFill,
            VariabilityModel vm)
        {
            toFill.Add(new BinaryOption(vm, "testOption1"), BinaryOption.BinaryValue.Selected);
            toFill.Add(new BinaryOption(vm, "testOption2"), BinaryOption.BinaryValue.Deselected);
        }

        private static void fillUpNumericOptions(Dictionary<NumericOption, double> toFill,
            VariabilityModel vm)
        {
            toFill.Add(new NumericOption(vm, "testOption3"), 3);
            toFill.Add(new NumericOption(vm, "testOption4"), 4);
        }

        private static void initVM(VariabilityModel vm)
        {
            vm.addConfigurationOption(new BinaryOption(vm, "testOption1"));
            vm.addConfigurationOption(new BinaryOption(vm, "testOption2"));
            vm.addConfigurationOption(new NumericOption(vm, "testOption3"));
            vm.addConfigurationOption(new NumericOption(vm, "testOption4"));
        }
    }
}
