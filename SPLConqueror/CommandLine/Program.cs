using System.Threading;
using System.Globalization;

namespace CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            if (args.Length == 1 && args[0].EndsWith(".a"))
            {
                Script_A s = new Script_A(args[0]);
            }
        }
    }
}
