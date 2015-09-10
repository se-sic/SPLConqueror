using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VariabilitModel_GUI;
using System.Threading;
using System.Globalization;

namespace PerformancePrediction_GUI
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new VariabilityModel_Form());

            //new Thread(() => Application.Run(someForm)).Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PerformancePrediction_Frame());
        }
    }
}
