using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.IO.Contracts
{
    public interface IWriter
    {
        void Write<T>(T value);
        void WriteLine<T>(T Value);
        void EmptyLine();
    }
}
