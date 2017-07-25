using System;
using NUnit.Framework;
using SPLConqueror_Core;
using System.Collections.Generic;

namespace SPLConquerorCoreTest
{
    [TestFixture]
    public class GlobalStateTest
    {
        [Test, Order(1)]
        public void TestNFProperty()
        {
            NFProperty created = GlobalState.getOrCreateProperty("testProperty");
            Assert.AreEqual(created, GlobalState.getOrCreateProperty(created.Name));
            GlobalState.setDefaultProperty(created.Name);
            Assert.AreEqual(created, GlobalState.currentNFP);
        }

        [Test, Order(2)]
        public void TestMeasurements()
        {
            GlobalState.varModel = new VariabilityModel("testModel");
            Dictionary<NFProperty, double> measuredValues = new Dictionary<NFProperty, double>();
            measuredValues.Add(GlobalState.currentNFP, 2);
            Configuration toAdd = new Configuration(new Dictionary<BinaryOption, BinaryOption.BinaryValue>(),
                new Dictionary<NumericOption, double>(), measuredValues);
            GlobalState.addConfiguration(toAdd);
            List<Configuration> measured = new List<Configuration>();
            measured.Add(toAdd);
            Assert.AreEqual(toAdd.GetNFPValue(), GlobalState.getMeasuredConfigs(measured).ToArray()[0].GetNFPValue());
        }

        [Test, Order(3)]
        public void TestClearGlobalState()
        {
            GlobalState.clear();
            Assert.IsNull(GlobalState.varModel);
            Assert.IsNull(GlobalState.currentNFP);
            Assert.AreEqual(0, GlobalState.allMeasurements.Configurations.Count);
            Assert.AreEqual(0, GlobalState.evaluationSet.Configurations.Count);
            Assert.AreEqual(0, GlobalState.nfProperties.Count);
            Assert.AreEqual(0, GlobalState.optionOrder.Count);
        }
    }
}
