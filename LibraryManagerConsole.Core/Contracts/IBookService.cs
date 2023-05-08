using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        /// <summary>
        /// Adds Book to the Database
        /// </summary>
        /// <param name="bookModel">BookModel to add to DB</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task AddBookAsync(BookModel book);

        /// <summary>
        /// Deletes a Book from Database
        /// </summary>
        /// <param name="book">Book to delete from DB</param>
        /// <returns></returns>
        Task DeleteBookAsync(Book book);

        /// <summary>
        /// Deletes a Book from Database
        /// </summary>
        /// <param name="bookModel">Book Model to delete from DB</param>
        /// <returns></returns>
        Task DeleteBookAsync(BookModel book);

        /// <summary>
        /// Deletes a Collection of Books from DB
        /// </summary>
        /// <param name="bookModels">Collecton of Book Models to delete from DB</param>
        /// <returns></returns>
        Task DeleteBooksAsync(IEnumerable<BookModel> bookModels);

        /// <summary>
        /// Returns all books(with their Authors and Genres) in DB as readonly
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookModel>> AllBooksReadOnlyAsync();

        /// <summary>
        /// Returns all books(with their Authors and Genres) from DB as Book models
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BookModel>> AllBooksAsync();

        /// <summary>
        /// Checks the DB for a book with the following ID if it exists return a book model of it.
        /// </summary>
        /// <param name="id">Id to search for in DB</param>
        /// <returns>BookModel</returns>
        Task<BookModel> GetBookModelByIdAsync(int id);

        /// <summary>
        /// Adds Genres to book model
        /// </summary>
        /// <param name="bookModel">book model to add genres to</param>
        /// <param name="genreNames">genres to add to book model (strings separated with " ")</param>
        void AddGenresToBookModel(BookModel bookModel, string genreNames);

        /// <summary>
        /// Adds Genres to book model
        /// </summary>
        /// <param name="bookModel">book model to add genres to</param>
        /// <param name="genreNames">genres to add to book model (string array)</param>
        void AddGenresToBookModel(BookModel bookModel, string[] genreNames);

        /// <summary>
        /// Adds Genre to book model
        /// </summary>
        /// <param name="bookModel">book model to add genre to</param>
        /// <param name="genreModel">genre model to add to book model</param>
        /// <exception cref="ArgumentException"></exception>
        void AddGenreToBookModel(BookModel bookModel, GenreModel genre);

        /// <summary>
        /// Create a book model
        /// </summary>
        /// <param name="bookTitle">title of book model</param>
        /// <param name="authorFirstName">first name of the author</param>
        /// <param name="authorMiddleName">middle name of the author</param>
        /// <param name="authorLastName">last name of the author</param>
        /// <param name="releaseDate">date of release</param>
        /// <param name="bookGenres">genres of book</param>
        /// <returns>Book Model</returns>
        /// <exception cref="ArgumentException"></exception>
        BookModel CreateFullBookModel(string bookTitle, string authorFirstName, string authorMiddleName, string authorLastName, string releaseDate, string[] bookGenres);

        /// <summary>
        /// Removes genre from book model
        /// </summary>
        /// <param name="bookModel">book model to remove genre from</param>
        /// <param name="genreName">name of the genre to remove from book model</param>
        void RemoveGenreFromBookModel(BookModel bookModel, string genreName);

        /// <summary>
        /// Edits author of book
        /// </summary>
        /// <param name="bookModel">Book model to edit author of</param>
        /// <param name="authorFullName">Author's new full name in the format : "'First name' 'Middle name' 'Last name'"</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ArgumentException"></exception>
        Task EditAuthorInBookModel(BookModel bookModel, string authorFullName);

        /// <summary>
        /// Edits author of book
        /// </summary>
        /// <param name="bookModel">Book model to edit author of</param>
        /// <param name="authorModel">Author model with the new author of the book</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ArgumentException"></exception>
        Task EditAuthorInBookModel(BookModel bookModel, AuthorModel authorModel);

        /// <summary>
        /// Find author by full name
        /// </summary>
        /// <param name="authorName">Full author name in the format "'First name' 'Middle name' 'Last name'"</param>
        /// <returns>Author model</returns>
        Task<AuthorModel> FindAuthor(string authorName);

        /// <summary>
        /// Update release date of a book model
        /// </summary>
        /// <param name="bookModel">Book model to update releae date of</param>
        /// <param name="releaseDate">Updated release date (string)</param>
        void UpdateReleaseDate(BookModel bookModel, string releaseDate);

        /// <summary>
        /// Update release date of a book model
        /// </summary>
        /// <param name="bookModel">Book model to update release date of</param>
        /// <param name="releaseDate">Updated release date</param>
        /// <exception cref="ArgumentException"></exception>
        void UpdateReleaseDate(BookModel bookModel, DateTime releaseDate);

        /// <summary>
        /// Saves all changes to all DB objects in the Database
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();

    }
}
