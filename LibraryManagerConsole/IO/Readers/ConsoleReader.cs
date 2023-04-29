using LibraryManagerConsole.IO.Contracts;

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
