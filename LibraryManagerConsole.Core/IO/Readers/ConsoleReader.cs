using LibraryManagerConsole.Core.IO.Contracts;

namespace LibraryManagerConsole.Core.IO.Readers
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
