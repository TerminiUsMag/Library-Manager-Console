using LibraryManagerConsole.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(BookViewModel book);

        Task DeleteBookAsync(BookModel book);

        Task<IEnumerable<BookModel>> AllBooksAsync();

        Task GetBookByIdAsync(int id);

    }
}
