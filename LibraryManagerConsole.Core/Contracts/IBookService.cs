using LibraryManagerConsole.Core.Models;

namespace LibraryManagerConsole.Core.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(BookModel book);

        Task DeleteBookAsync(BookModel book);
        Task DeleteBooksAsync(IEnumerable<BookModel> bookModels);

        Task<IEnumerable<BookModel>> AllBooksReadOnlyAsync();
        Task<IEnumerable<BookModel>> AllBooksAsync();

        Task<BookModel> GetBookByIdAsync(int id);

        void AddGenresToBookModel(BookModel book, string[] genreNames);

        void AddGenreToBookModel(BookModel book, GenreModel genre);
        BookModel CreateFullBookModel(string bookTitle, string authorFirstName, string authorMiddleName, string authorLastName, string releaseDate, string[] bookGenres);

        void RemoveGenreFromBook(BookModel book, string genreName);

        //private GenreModel FindGenreInBook(BookModel book, string genreName);

        Task EditAuthorInBookModel(BookModel bookModel, string authorName);

        Task EditAuthorInBookModel(BookModel bookModel, AuthorModel author);

        Task<AuthorModel> FindAuthor(string authorName);

        void UpdateReleaseDate(BookModel bookModel, string releaseDate);

        void UpdateReleaseDate(BookModel bookModel, DateTime releaseDate);



    }
}
