using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.IO.Contracts
{
    public interface IReader
    {
        void Read();
        string ReadLine();
    }
}
