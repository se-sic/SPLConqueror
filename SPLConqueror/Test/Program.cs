using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Test.Core;
using System.Threading;
using System.Globalization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            VarModelAndOptions vmoTest = new VarModelAndOptions();
            vmoTest.saveVarModel( vmoTest.createVarModel());

        }
    }
}
