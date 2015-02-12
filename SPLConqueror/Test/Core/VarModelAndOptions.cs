using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPLConqueror_Core;

namespace Test.Core
{
    public class VarModelAndOptions
    {

        public void createVarModel()
        {
            VariabilityModel varMod = new VariabilityModel("testModel");


            BinaryOption binOp1 = new BinaryOption(varMod, "binOpt1");
            binOp1.Optional = false;
            binOp1.Prefix = "--";


            BinaryOption binOp2 = new BinaryOption(varMod, "binOpt2");
            binOp2.Optional = true;
            binOp2.Prefix = "-?";
            binOp2.Postfix = "kg";
            binOp2.Parent = binOp1;

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

            varMod.saveXML("D:\SPLConquerorGitHub\SPLConqueror\ExampleFiles\foo.xml");

           
        }

    }
}
