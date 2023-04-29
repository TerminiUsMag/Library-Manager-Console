namespace LibraryManagerConsole.Core.IO.Contracts
{
    public interface IWriter
    {
        void Write<T>(T value);
        void WriteLine<T>(T Value);
        void EmptyLine();
    }
}
