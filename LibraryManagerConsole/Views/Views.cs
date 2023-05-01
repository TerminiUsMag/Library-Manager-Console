using LibraryManagerConsole.Contracts;
using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.Views
{
    public static class Views
    {

        public static async void CreateBook(IWriter writer,IReader reader, IBookService bookService)
        {
            writer.WriteLine("Write a title for the book");
            var bookTitle = reader.ReadLine().Trim();
            writer.WriteLine("Write author's first name");
            var authorFirstName = reader.ReadLine().Trim();
            writer.WriteLine("Write author's middle name");
            var authorMiddleName = reader.ReadLine().Trim();
            writer.WriteLine("Write author's last name");
            var authorLastName = reader.ReadLine().Trim();
            writer.WriteLine("Write book's release date in 'DD/MM/YYYY' format (default date is today)");
            var bookReleaseDate = reader.ReadLine().Trim();
            writer.WriteLine("Write the book's Genre/s (split them with semicolon ',') : ");
            var bookGenres = reader.ReadLine().Trim().Split(',');

            try
            {
                var newBook = bookService.CreateFullBookModel(bookTitle, authorFirstName, authorMiddleName, authorLastName, bookReleaseDate, bookGenres);
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
