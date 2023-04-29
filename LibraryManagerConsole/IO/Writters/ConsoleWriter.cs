using LibraryManagerConsole.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.IO.Writters
{
    public class ConsoleWriter : IWritter
    {
        public void Write<T>(T value)
        {
            Console.Write(value);
        }

        public void WriteLine<T>(T value)
        {
            Console.WriteLine(value);
        }
    }
}
