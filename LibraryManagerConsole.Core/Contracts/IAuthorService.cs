using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IAuthorService
    {
        Task<Book> ExistingAuthorFromBookModelToBook(Book book, BookModel bookModel);
        Task AddAuthorAsync();
        Task DeleteAuthorAsync();
        Task FindAuthorAsync(int id);
        Task FindAuthorAsync(string name);
        Task UpdateAuthorAsync(AuthorViewModel author);

    }
}
