using LibraryManagerConsole.Core.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(BookModel book);

        Task DeleteBookAsync(BookModel book);

        Task<IEnumerable<BookModel>> AllBooksAsync();

        Task<BookModel> GetBookByIdAsync(int id);

        void AddGenre(BookModel book, string genreName);

        void AddGenre(BookModel book, GenreModel genre);

        void RemoveGenre(BookModel book, string genreName);

        //private GenreModel FindGenreInBook(BookModel book, string genreName);

        void AddAuthor(BookModel book, string authorName);

        void AddAuthor(BookModel book, AuthorModel author);

        void DeleteAuthor(BookModel book, string authorName);

        AuthorModel FindAuthor(string authorName);

        void UpdateReleaseDate(BookModel book, string releaseDate);



    }
}
