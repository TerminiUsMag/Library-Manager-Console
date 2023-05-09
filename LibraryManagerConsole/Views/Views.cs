using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.Services;
using LibraryManagerConsole.Views.Contracts;

namespace LibraryManagerConsole.Views
{
    public class Views : IViews
    {
        private readonly IBookService bookService;
        private readonly IWriter writer;
        private readonly IReader reader;

        private static Views instance = null;
        public Views(IBookService _bookService, IWriter _writer, IReader _reader)
        {
            Instance = new Views(_bookService, _writer, _reader);
            this.bookService = _bookService;
            this.writer = _writer;
            this.reader = _reader;
        }
        public Views Instance
        {
            set
            {
                if (instance is null)
                {
                    instance = value;
                }
            }
            //set
            //{
            //    if (instance is null)
            //    {
            //        instance = value;
            //    }
            //}
        }
        public async void CreateBook(/*IWriter writer, IReader reader/*, IBookService bookService*/)
        {
            writer.WriteLine("Write a title for the book");
            var bookTitle = reader.ReadLine().Trim();
            writer.WriteLine("Write author's full name in the format : 'First name' 'Second name' 'Last name'");
            var authorFullName = reader.ReadLine().Trim();
            writer.WriteLine("Write book's release date in 'DD/MM/YYYY' format (default date is today)");
            var bookReleaseDate = reader.ReadLine().Trim();
            writer.WriteLine("Write the book's Genre/s (split them with semicolon ',') : ");
            var bookGenres = reader.ReadLine().Trim().Split(',');

            try
            {
                var newBook = bookService.CreateFullBookModel(bookTitle, authorFullName, bookReleaseDate, bookGenres);
                writer.WriteLine(newBook);

                await bookService.AddBookAsync(newBook);
            }
            catch (ArgumentException aex)
            {
                writer.WriteLine(aex.Message);
            }
        }

    }
}
