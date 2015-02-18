using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SPLConqueror_Core
{
    public abstract class Logger
    {

        protected StreamWriter writer = null;

        public Logger(String outputLocation)
        {
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
                    ostrm = new FileStream(outputLocation.Trim(), FileMode.OpenOrCreate, FileAccess.Write);
                    ostrm.SetLength(0); // clear the file
                    ostrm.Flush();
                    writer = new StreamWriter(ostrm);
                    writer.AutoFlush = true;
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

        public void close()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
        }

    }
}
