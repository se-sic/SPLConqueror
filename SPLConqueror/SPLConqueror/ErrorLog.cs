using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPLConqueror_Core
{
    public class ErrorLog
    {

        /// <summary>
        /// Logs the error depending on what log mechanism was chosen (console, file, gui). Todo: currently only logging at console
        /// </summary>
        /// <param name="msg">The error message to be printed or logged</param>
        public static void logError(String msg)
        {
            Console.WriteLine(msg);
        }
    }
}
