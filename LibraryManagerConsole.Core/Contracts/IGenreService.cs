using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IGenreService
    {
        Task AddGenre();
        Task<GenreModel> CreateGenreModel(string genreName);
        Task<Book> ExistingGenresFromBookModelToBook(Book book, BookModel bookModel);
        GenreModel FindGenreInBookModel(BookModel bookModel, string genreName);
        Task<GenreModel> FindGenreAsync(string genreName);
    }
}
