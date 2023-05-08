using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IGenreService
    {

        /// <summary>
        /// Creates a new Genre Model if there's no such genre in Database
        /// </summary>
        /// <param name="genreName">Name of the new genre</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<GenreModel> CreateGenreModel(string genreName);

        /// <summary>
        /// Transfers genres from Book Model to Book (checks for existing genres in DB)
        /// </summary>
        /// <param name="book"></param>
        /// <param name="bookModel"></param>
        /// <returns>Book</returns>
        Task<Book> ExistingGenresFromBookModelToBook(Book book, BookModel bookModel);

        /// <summary>
        /// Returns Genre Model with specific name from Book Model
        /// </summary>
        /// <param name="bookModel">Book Model to search for genre</param>
        /// <param name="genreName">Genre name to find</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        GenreModel FindGenreInBookModel(BookModel bookModel, string genreName);

        /// <summary>
        /// Find Genre in Database and return it as Genre Model
        /// </summary>
        /// <param name="genreName">Name of Genre to find</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<GenreModel> FindGenreAsync(string genreName);

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genreName">Name of the new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task AddGenreAsync(string genreName);

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genreModel">Genre Model of the new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task AddGenreAsync(GenreModel genreModel);

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genre">new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task AddGenreAsync(Genre genre);
    }
}
