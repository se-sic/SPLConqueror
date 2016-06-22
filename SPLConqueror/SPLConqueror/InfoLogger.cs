using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class InfoLogger : Logger
    {
        readonly object loggerLock = new Object();
        public InfoLogger()
        {
        }

        public InfoLogger(String location)
            : base(location)
        {
            if(writer!=null)
                Console.SetOut(writer);

        }

        /// <summary>
        /// Logs general information depending on what log mechanism was chosen (console, file, gui). Todo: currently only logging at file
        /// </summary>
        /// <param name="msg">The message to be printed or logged</param>
        public override void logLine(String msg)
        {
            lock (loggerLock)
            {
                if (!msg.EndsWith(System.Environment.NewLine))
                    msg += System.Environment.NewLine;

                if (writer != null)
                    writer.Write(msg);
                else
                    Console.Write(msg);
            }
        }

        /// <summary>
        /// Logs general information depending on what log mechanism was chosen (console, file, gui). Todo: currently only logging at file
        /// </summary>
        /// <param name="msg">The message to be printed or logged</param>
        public override void log(String msg)
        {
            lock (loggerLock)
            {
                if (writer != null)
                    writer.Write(msg);
                else
                    Console.Write(msg);
            }
        }

    }
}
