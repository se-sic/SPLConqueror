using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    /// <summary>
    /// The abstract logger class that provides the possibility of writing information to a given file.
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// The <code>StreamWriter</code>-object for the output.
        /// </summary>
        protected StreamWriter writer = null;

        /// <summary>
        /// The <code>TextWriter</code>, which prints to the standard output of the console.
        /// </summary>
        protected static TextWriter stdout = new StreamWriter(Console.OpenStandardOutput());

        private string outputLocation;

        /// <summary>
        /// The default constructor of the <code>Logger</code>-class.
        /// </summary>
        public Logger() { }

        /// <summary>
        /// The constructor for the logger to log into the given file.
        /// </summary>
        /// <param name="outputLocation">The location of the file to write the output into.</param>
        /// <param name="append"><code>true</code> iff the output should be appended to the given file;<code>false</code> otherwise.</param>
        public Logger(String outputLocation, bool append)
        {
            this.outputLocation = outputLocation;
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }

            FileStream ostrm;
            TextWriter oldOut = Console.Out;
            if (outputLocation != null)
            {
                try
                {
                    if (!append)
                    {
                        ostrm = new FileStream(outputLocation.Trim(), FileMode.OpenOrCreate, FileAccess.Write);
                        ostrm.SetLength(0); // clear the file
                        ostrm.Flush();
                        writer = new StreamWriter(ostrm);
                        writer.AutoFlush = true;
                    } else
                    {
                        ostrm = new FileStream(outputLocation.Trim(), FileMode.Append, FileAccess.Write);
                        writer = new StreamWriter(ostrm);
                        writer.AutoFlush = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open " + outputLocation.Trim() + " for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// A method to log the given message.
        /// Note: the message is only printed to the specified file.
        /// To print line, use the <code>logLine</code>-method.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        public abstract void log(String msg);

        /// <summary>
        /// Prints the given string in a line.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        public abstract void logLine(String msg);

        /// <summary>
        /// Prints the message to the standard output.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        public void logToStdout(String msg)
        {
            stdout.WriteLine(msg);
            stdout.Flush();
        }

        /// <summary>
        /// Closes the logger instance.
        /// </summary>
        public void close()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Returns the file location of the logger.
        /// </summary>
        /// <returns>File location of the logger.</returns>
        override public string ToString()
        {
            if (outputLocation == null)
            {
                return "null";
            } else
            {
                return outputLocation;
            }
        }

    }
}
