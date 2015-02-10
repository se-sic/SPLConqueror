using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SPLConqueror_Core;

namespace CommandLine
{
    class Script_A
    {

        private StreamReader reader = null;

        public Script_A(string file)
        {

            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
                throw new FileNotFoundException(@"Automation script not found.", file);

            reader = fi.OpenText();
            
        }


        public void start()
        {
            
            Commands co = new Commands();

            while (!reader.EndOfStream)
            {
                String line = reader.ReadLine().Trim();
                co.performOneCommand(line);

            }

        }

        
    }
}
