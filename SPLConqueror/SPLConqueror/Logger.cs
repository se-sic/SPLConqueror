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

        protected StreamWriter writer = null;
        protected static TextWriter stdout = new StreamWriter(Console.OpenStandardOutput());

        private string outputLocation;

        public Logger()
        {
        }

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

        public abstract void log(String msg);

        public abstract void logLine(String msg);

        public void logToStdout(String msg)
        {
            stdout.WriteLine(msg);
            stdout.Flush();
        }

        public void close()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
        }

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
