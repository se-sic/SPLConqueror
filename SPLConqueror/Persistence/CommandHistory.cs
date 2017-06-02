using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class CommandHistory
    {
        /// <summary>
        /// Queue that represents the command history. No data should be added using the queue directly.
        /// </summary>
        public Queue<string> commandHistory = new Queue<string>();

        /// <summary>
        /// Create a new CommandHistory object. CommandHistory stores the last 10 commands performed.
        /// </summary>
        public CommandHistory()
        {

        }

        /// <summary>
        /// Add a command to the command history.
        /// </summary>
        /// <param name="command">Command that will be added.</param>
        public void addCommand(string command)
        {
            commandHistory.Enqueue(command);
            if (commandHistory.Count > 10)
            {
                commandHistory.Dequeue();
            }
        }

        /// <summary>
        /// Checks whether 2 command history objects are the same.
        /// </summary>
        /// <param name="other">The other CommandHistory object for comparison.</param>
        /// <returns>True if they are equal, else false.</returns>
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
                    if (!(thisAsArr[i].Equals(otherAsArr[i]))) {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
