using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPLConqueror_Core;
using System.Windows.Forms;


namespace PerformancePrediction_GUI
{
    class Logger_Gui : Logger
    {
        private PerformancePrediction_Frame outerFrame;

        public Logger_Gui()
        {
        }

        public Logger_Gui(PerformancePrediction_Frame frame)
        {
            outerFrame = frame;

        }


        public override void logLine(string msg)
        {
            outerFrame.LogBox.Invoke((MethodInvoker)(() => this.outerFrame.LogBox.AppendText(msg + System.Environment.NewLine)));
        }

        public override void log(string msg)
        {
            outerFrame.LogBox.Invoke((MethodInvoker)(() => this.outerFrame.LogBox.AppendText(msg)));
        }
    }
}
