using LibraryManagerConsole.IO.Contracts;

namespace LibraryManagerConsole.IO.Writters
{
    public class ConsoleWriter : IWriter
    {
        public void EmptyLine()
        {
            Console.WriteLine();
        }

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
