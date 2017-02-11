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

        public bool Equals(CommandHistory other)
        {
            if (other == null)
            {
                throw new ArgumentException();
            }
            string[] thisAsArr = commandHistory.ToArray();
            string[] otherAsArr = other.commandHistory.ToArray();
            if (thisAsArr.Length != otherAsArr.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < thisAsArr.Length; i++)
                {
                    if (!(thisAsArr[i].Equals(otherAsArr[i])) {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
