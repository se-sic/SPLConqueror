using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    /// <summary>
    /// This class is used to log all error messages in a given file.
    /// </summary>
    public class ErrorLogger : Logger
    {
        readonly object loggerLock = new Object();

        /// <summary>
        /// Creates an error logger.
        /// </summary>
        /// <param name="location">The file where the errors should be written to. This argument is <code>null</code> if the errors should be printed to the console.</param>
        /// <param name="append"><code>true</code> iff the output should be appended to the given file;<code>false</code> otherwise.</param>
        public ErrorLogger(String location, bool append = false) : base(location, append)
        {
            if (writer != null)
                Console.SetError(writer);
        }

        /// <summary>
        /// Logs the error message and appends a line break to it.
        /// </summary>
        /// <param name="msg">The error message to be logged.</param>
        public override void logLine(String msg)
        {
            lock (loggerLock)
            {
                if (!msg.EndsWith(System.Environment.NewLine))
                    msg += System.Environment.NewLine;
                if (writer != null)
                {
                    writer.Write(msg);
                }
                else
                {
                    Console.Write(msg);
                }
            }
        }

        /// <summary>
        /// Logs the given error message.
        /// </summary>
        /// <param name="msg">The error message to be logged.</param>
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
