using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class is used to log all normal messages to the given output.
    /// </summary>
    public class InfoLogger : Logger
    {
        readonly object loggerLock = new Object();

        /// <summary>
        /// The default constructor for the <code>InfoLogger</code>.
        /// </summary>
        public InfoLogger()
        {
        }

        /// <summary>
        /// This constructor creates a new <code>InfoLogger</code>, which writes the messages to the given file.
        /// </summary>
        /// <param name="location">The file to write the logs.</param>
        /// <param name="append"><code>true</code> iff the output should be appended to the given file;<code>false</code> otherwise.</param>
        public InfoLogger(String location, bool append=false)
            : base(location, append)
        {
            if(writer!=null)
                Console.SetOut(writer);

        }

        /// <summary>
        /// Logs general information depending on what log mechanism was chosen (console, file, gui).
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
        /// Logs general information depending on what log mechanism was chosen (console, file, gui).
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
