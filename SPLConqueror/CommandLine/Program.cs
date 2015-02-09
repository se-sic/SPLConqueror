using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.IO;

namespace CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            if (args.Length == 1)
            {
                // MLSettings in ml/sampling module
                //FileInfo fi = new FileInfo(System.Environment.CurrentDirectory + Path.DirectorySeparatorChar + "MLsettings.txt");
                //if (fi.Exists)
                //{
                //    MLsettings.load(fi.OpenText());
                //}
                if (args[0].EndsWith(".txt"))
                {
                
                }
                if (args[0].EndsWith(".a"))
                {
                    Script_A s = new Script_A(args[0]);
                }
            }


        }
    }
}
