using LibraryManagerConsole.Core.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(BookModel book);

        Task DeleteBookAsync(BookModel book);

        Task<IEnumerable<BookModel>> AllBooksReadOnlyAsync();
        Task<IEnumerable<BookModel>> AllBooksAsync();

        Task<BookModel> GetBookByIdAsync(int id);

        void AddGenresToBookModel(BookModel book, string[] genreNames);

        void AddGenreToBookModel(BookModel book, GenreModel genre);
        BookModel CreateFullBookModel(string bookTitle, string authorFirstName, string authorMiddleName, string authorLastName, string releaseDate, string[] bookGenres);

        void RemoveGenreFromBook(BookModel book, string genreName);
        Task<GenreModel> CreateGenre(string genreName);

        //private GenreModel FindGenreInBook(BookModel book, string genreName);

        void EditAuthor(BookModel book, string authorName);

        void EditAuthor(BookModel book, AuthorModel author);

        void DeleteAuthor(BookModel book, string authorName);

        AuthorModel FindAuthor(string authorName);

        void UpdateReleaseDate(BookModel book, string releaseDate);



    }
}
