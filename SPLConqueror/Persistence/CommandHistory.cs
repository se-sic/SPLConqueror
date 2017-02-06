using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class CommandHistory
    {
        private Queue<string> commandHistory = new Queue<string>();

        public CommandHistory()
        {

        }

        public void addCommand(string command)
        {
            commandHistory.Enqueue(command);
            if (commandHistory.Count > 10)
            {
                commandHistory.Dequeue();
            }
        }
    }
}
