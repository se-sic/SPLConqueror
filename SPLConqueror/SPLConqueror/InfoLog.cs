using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class InfoLog
    {
        /// <summary>
        /// Logs general information depending on what log mechanism was chosen (console, file, gui). Todo: currently only logging at console
        /// </summary>
        /// <param name="msg">The message to be printed or logged</param>
        public static void logInfo(String msg)
        {
            Console.WriteLine(msg);
        }
    }
}
