using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IAuthorService
    {


        /// <summary>
        /// Transfers Author from Book Model to Book (If exists in DB else creates new Author with Author Model names)
        /// </summary>
        /// <param name="book">Book to tranfer to</param>
        /// <param name="bookModel">Book Model to transfer from</param>
        /// <returns>Book</returns>
        Task<Book> ExistingAuthorFromBookModelToBook(Book book, BookModel bookModel);

        /// <summary>
        /// Update Author of Book In DB
        /// </summary>
        /// <param name="bookModel">Book Model to update</param>
        /// <param name="authorModel">Updated Author Model</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ArgumentException"></exception>
        Task UpdateAuthorOfBookInDbAsync(BookModel bookModel, AuthorModel authorModel);

        /// <summary>
        /// Deletes Author from Database
        /// </summary>
        /// <param name="authorModel">Author Model to delete</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task DeleteAuthorFromDBAsync(AuthorModel authorModel);

        /// <summary>
        /// Returns an Author if exists in DB
        /// </summary>
        /// <param name="id">Id of Author</param>
        /// <returns>Author</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Author> FindAuthorAsync(int id);

        /// <summary>
        /// Returns an Author if exists in DB
        /// </summary>
        /// <param name="fullName">Full name of Author in the format : "'First name' 'Middle name' 'Last name'"</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<Author> FindAuthorAsync(string fullName);

        /// <summary>
        /// Update existing Author
        /// </summary>
        /// <param name="authorModel">Author to find</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task UpdateAuthorAsync(AuthorModel authorModel);

    }
}
