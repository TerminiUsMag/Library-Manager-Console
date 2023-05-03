using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IAuthorService
    {
        Task<Book> ExistingAuthorFromBookModelToBook(Book book, BookModel bookModel);
        Task UpdateAuthorOfBookInDbAsync(BookModel bookModel, AuthorModel authorModel);
        Task DeleteAuthorFromDBAsync(AuthorModel authorModel);
        Task<Author> FindAuthorAsync(int id);
        Task<Author> FindAuthorAsync(string fullName);
        Task UpdateAuthorAsync(AuthorViewModel authorModel);

    }
}
