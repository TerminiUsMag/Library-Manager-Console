using LibraryManagerConsole.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.IO.Readers
{
    public class ConsoleReader : IReader
    {
        public void Read()
        {
            throw new NotImplementedException();
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
