using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace Test.Core
{
    public class VarModelAndOptions
    {

        public VariabilityModel createVarModel()
        {
            VariabilityModel varMod = new VariabilityModel("testModel_1");

            // -------------------- BINARY OPTIONS ----------------
            BinaryOption binOp1 = new BinaryOption(varMod, "binOpt1");
            binOp1.Optional = false;
            binOp1.Prefix = "--";
            varMod.addConfigurationOption(binOp1);


            BinaryOption binOp2 = new BinaryOption(varMod, "binOpt2");
            binOp2.Optional = true;
            binOp2.Prefix = "-?";
            binOp2.Postfix = "kg";
            binOp2.Parent = binOp1;
            binOp2.OutputString = "binOpt2";
            varMod.addConfigurationOption(binOp2);

            BinaryOption binOp3 = new BinaryOption(varMod, "binOpt3");
            binOp3.Optional = true;
            binOp3.Prefix = "";
            binOp3.Postfix = "";
            binOp3.Parent = binOp1;
            List<List<ConfigurationOption>> exclude = new List<List<ConfigurationOption>>();
            List<ConfigurationOption> subExclude = new List<ConfigurationOption>();
            subExclude.Add(binOp2);
            exclude.Add(subExclude);
            binOp3.Excluded_Options = exclude;
            varMod.addConfigurationOption(binOp3);


            BinaryOption binOp4 = new BinaryOption(varMod, "binOpt4");
            binOp4.Optional = true;
            binOp4.Prefix = "4_";
            binOp4.Postfix = "_4";
            binOp4.Parent = binOp1;
            List<List<ConfigurationOption>> implied = new List<List<ConfigurationOption>>();
            List<ConfigurationOption> subimplied = new List<ConfigurationOption>();
            subimplied.Add(binOp2);
            implied.Add(subimplied);
            binOp4.Implied_Options = implied;
            varMod.addConfigurationOption(binOp4);

            // -------------------- NUMERIC OPTIONS ----------------

            NumericOption numOpt1 = new NumericOption(varMod, "numOpt1");
            numOpt1.Prefix = "num1-";
            numOpt1.Postfix = "--";
            numOpt1.Min_value = 0;
            numOpt1.Max_value = 10;
            numOpt1.StepFunction = new InfluenceFunction("n + 2");
            varMod.addConfigurationOption(numOpt1);

            NumericOption numOpt2 = new NumericOption(varMod, "numOpt2");
            numOpt2.Prefix = "";
            numOpt2.Postfix = "";
            numOpt2.Min_value = 0.1;
            numOpt2.Max_value = 5;
            numOpt2.StepFunction = new InfluenceFunction("n * 2");
            varMod.addConfigurationOption(numOpt2);


            return varMod;

        }


        public void saveVarModel(VariabilityModel varMod)
        {
            varMod.saveXML(@"D:\SPLConquerorGitHub\\SPLConqueror\ExampleFiles\foo.xml");
        }

    }
}
