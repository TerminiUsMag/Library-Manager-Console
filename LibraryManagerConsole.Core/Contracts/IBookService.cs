using LibraryManagerConsole.Core.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(BookViewModel book);

        Task DeleteBookAsync(BookModel book);

        Task<IEnumerable<BookModel>> AllBooksAsync();

        Task<BookModel> GetBookByIdAsync(int id);

    }
}
