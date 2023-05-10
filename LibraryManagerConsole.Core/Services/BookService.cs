using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagerConsole.Core.Services;

public class BookService : IBookService
{
    private readonly IRepository repo = null!;
    private readonly IWriter writer = null!;
    private readonly IReader reader = null!;
    private readonly IGenreService genreService = null!;
    private readonly IAuthorService authorService = null!;
    public BookService(IRepository _repo, IWriter _writer, IReader _reader, IGenreService genreService, IAuthorService authorService)
    {
        this.repo = _repo;
        this.writer = _writer;
        this.reader = _reader;
        this.genreService = genreService;
        this.authorService = authorService;
    }

    /// <summary>
    /// Adds a Book to the Database
    /// </summary>
    /// <param name="bookModel">BookModel to add to DB</param>
    /// <returns>Nothing</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task AddBookAsync(BookModel bookModel)
    {
        var allBooks = await repo
            .AllReadonly<Book>()
            .ToListAsync();

        var existingBook = allBooks
            .Where(b => b.Title == bookModel.Title && b.Author.ToString()
            == bookModel.Author.ToString())
            .FirstOrDefault();

        if (existingBook is not null)
        {
            throw new ArgumentException("There's already a book with that title in the library");
        }
        var book = new Book
        {
            Title = bookModel.Title,
            DateOfRelease = bookModel.DateOfRelease,
        };

        book = await authorService.ExistingAuthorFromBookModelToBook(book, bookModel);
        book = await genreService.ExistingGenresFromBookModelToBook(book, bookModel);

        await repo.AddAsync(book);
        await repo.SaveChangesAsync();
    }

    /// <summary>
    /// Returns all books(with their Authors and Genres) from DB as readonly Book Models
    /// </summary>
    /// <returns>Collection of Book models</returns>
    public async Task<IEnumerable<BookModel>> AllBooksReadOnlyAsync()
    {
        var books = await repo.AllReadonly<Book>().Include(b => b.Author).Include(b => b.Genres)
            .ToListAsync();
        return books.Select(b => new BookModel
        {
            Id = b.Id,
            Title = b.Title,
            Author = new AuthorModel
            {
                FirstName = b.Author.FirstName,
                MiddleName = b.Author.MiddleName,
                LastName = b.Author.LastName,
            },
            DateOfRelease = b.DateOfRelease,
            Genres = b.Genres.Select(g => new GenreModel
            {
                Name = g.Name,
                //Id = g.Id,
                Books = g.Books
            }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Deletes a Book from Database
    /// </summary>
    /// <param name="bookModel">Book Model to delete from DB</param>
    /// <returns>Nothing</returns>
    public async Task DeleteBookAsync(BookModel bookModel)
    {
        var book = await repo.All<Book>().Where(b => b.Title == bookModel.Title).FirstOrDefaultAsync();
        if (book is null)
        {
            return;
        }
        await repo.DeleteAsync<Book>(book.Id);
        await repo.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a Book from Database
    /// </summary>
    /// <param name="book">Book to delete from DB</param>
    /// <returns>Nothing</returns>
    public async Task DeleteBookAsync(Book book)
    {
        if (book is null)
        {
            return;
        }
        await repo.DeleteAsync<Book>(book.Id);
        await repo.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a Collection of Books from DB
    /// </summary>
    /// <param name="bookModels">Collecton of Book Models to delete from DB</param>
    /// <returns>Nothing</returns>
    public async Task DeleteBooksAsync(IEnumerable<BookModel> bookModels)
    {
        var books = await repo.AllReadonly<Book>().ToListAsync();


        foreach (var bookModel in bookModels)
        {
            var book = books.Where(b => b.Title == bookModel.Title).FirstOrDefault();
            //if (book is null)
            //{
            //    continue;
            //}

            await DeleteBookAsync(book!);
        }
    }

    /// <summary>
    /// Checks the DB for a book with the following ID if it exists return a book model of it.
    /// </summary>
    /// <param name="id">Id to search for in DB</param>
    /// <returns>BookModel</returns>
    public async Task<BookModel> GetBookModelByIdAsync(int id)
    {
        var book = await repo.GetByIdAsync<Book>(id);
        return new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = new AuthorModel
            {
                FirstName = book.Author.FirstName,
                MiddleName = book.Author.MiddleName,
                LastName = book.Author.LastName,
            },
            DateOfRelease = book.DateOfRelease,
            Genres = book.Genres.Select(g => new GenreModel
            {
                Name = g.Name,
                Id = g.Id,
                Books = g.Books
            }).ToList()
        };
    }

    /// <summary>
    /// Adds Genres to book model
    /// </summary>
    /// <param name="bookModel">book model to add genres to</param>
    /// <param name="genreNames">genres to add to book model (strings separated with " ")</param>
    public void AddGenresToBookModel(BookModel bookModel, string genreNames)
    {
        var genres = genreNames.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < genres.Length; i++)
        {
            if (!bookModel.Genres.Any(g => g.Name == genres[i]))
            {
                bookModel.Genres.Add(new GenreModel
                {
                    Name = genres[i]
                });
            }
        }
    }

    /// <summary>
    /// Adds Genres to book model
    /// </summary>
    /// <param name="bookModel">book model to add genres to</param>
    /// <param name="genreNames">genres to add to book model (string array)</param>
    public void AddGenresToBookModel(BookModel bookModel, string[] genreNames)
    {

        for (int i = 0; i < genreNames.Length; i++)
        {
            if (!bookModel.Genres.Any(g => g.Name == genreNames[i]))
            {
                bookModel.Genres.Add(new GenreModel
                {
                    Name = genreNames[i]
                });
            }
        }
    }

    /// <summary>
    /// Adds Genre to book model
    /// </summary>
    /// <param name="bookModel">book model to add genre to</param>
    /// <param name="genreModel">genre model to add to book model</param>
    /// <exception cref="ArgumentException"></exception>
    public void AddGenreToBookModel(BookModel bookModel, GenreModel genreModel)
    {

        if (bookModel.Genres.Any(g => g.Name == genreModel.Name))
        {
            throw new ArgumentException("There's already a genre with this name in the book");
        }
        bookModel.Genres.Add(genreModel);
    }

    /// <summary>
    /// Removes genre from book model
    /// </summary>
    /// <param name="bookModel">book model to remove genre from</param>
    /// <param name="genreName">name of the genre to remove from book model</param>
    public void RemoveGenreFromBookModel(BookModel bookModel, string genreName)
    {
        try
        {
            var genre = genreService.FindGenreInBookModel(bookModel, genreName);
            bookModel.Genres.Remove(genre);
        }
        catch (ArgumentException aex)
        {
            writer.WriteLine(aex.Message);
        }
    }

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
    public BookModel CreateFullBookModel(string bookTitle, string authorFullName, string releaseDate, string[] bookGenres)
    {
        if (authorFullName.IsNullOrEmpty())
        {
            throw new ArgumentException("One of the required parameters is null or empty!");
        }

        var authorNames = authorFullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (bookTitle.IsNullOrEmpty() || authorNames.Length <= 1 || bookGenres.Length < 1)
        {
            throw new ArgumentException("One or more of the required parameters is null or empty!");
        }
        if (!DateTime.TryParse(releaseDate, out DateTime dateOfRelease))
        {
            dateOfRelease = DateTime.Now.Date;
        }

        var newBookModel = new BookModel
        {
            Title = bookTitle,
            Author = new AuthorModel
            {
                FirstName = authorNames[0],
                MiddleName = authorNames[1],
                LastName = authorNames[2]
            },
            DateOfRelease = dateOfRelease.Date,
        };
        this.AddGenresToBookModel(newBookModel, bookGenres);
        return newBookModel;
    }

    //Test required!

    /// <summary>
    /// Edits author of book
    /// </summary>
    /// <param name="bookModel">Book model to edit author of</param>
    /// <param name="authorFullname">Author's new full name in the format : "'First name' 'Middle name' 'Last name'"</param>
    /// <returns>Nothing</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task EditAuthorInBookModel(BookModel bookModel, string authorFullname)
    {
        var book = await repo
            .All<Book>()
            .Where(b => b.Title == bookModel.Title && b.DateOfRelease == bookModel.DateOfRelease)
            .FirstOrDefaultAsync();

        if (book is null)
        {
            throw new ArgumentException("There's no such book in DB");
        }

        try
        {
            var author = await authorService.FindAuthorAsync(authorFullname);
            book.Author = author;
        }
        catch (Exception)
        {
            var author = book.Author;
            var authorNames = authorFullname.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            author.FirstName = authorNames[0];
            author.MiddleName = authorNames[1];
            author.LastName = authorNames[2];
        }
    }

    /// <summary>
    /// Edits author of book
    /// </summary>
    /// <param name="bookModel">Book model to edit author of</param>
    /// <param name="authorModel">Author model with the new author of the book</param>
    /// <returns>Nothing</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task EditAuthorInBookModel(BookModel bookModel, AuthorModel authorModel)
    {
        var authorFullName = authorModel.ToString();
        await this.EditAuthorInBookModel(bookModel, authorFullName);
    }

    /// <summary>
    /// Find author by full name
    /// </summary>
    /// <param name="authorName">Full author name in the format "'First name' 'Middle name' 'Last name'"</param>
    /// <returns>Author model</returns>
    public async Task<AuthorModel> FindAuthor(string authorName)
    {
        var author = await authorService.FindAuthorAsync(authorName);
        return new AuthorModel
        {
            FirstName = author.FirstName,
            MiddleName = author.MiddleName,
            LastName = author.LastName,
            Id = author.Id
        };
    }

    /// <summary>
    /// Update release date of a book model
    /// </summary>
    /// <param name="bookModel">Book model to update releae date of</param>
    /// <param name="releaseDate">Updated release date (string)</param>
    public void UpdateReleaseDate(BookModel bookModel, string releaseDate)
    {
        if (DateTime.TryParse(releaseDate, out DateTime date))
        {
            UpdateReleaseDate(bookModel, date);
        }
;
    }

    /// <summary>
    /// Update release date of a book model
    /// </summary>
    /// <param name="bookModel">Book model to update release date of</param>
    /// <param name="releaseDate">Updated release date</param>
    /// <exception cref="ArgumentException"></exception>
    public async void UpdateReleaseDate(BookModel bookModel, DateTime releaseDate)
    {
        var book = await repo
            .All<Book>()
            .Where(b => b.Title == bookModel.Title && b.DateOfRelease == bookModel.DateOfRelease)
            .FirstOrDefaultAsync();

        if (book is null)
        {
            throw new ArgumentException("There's no such book in DB");
        }

        book.DateOfRelease = releaseDate;
    }

    /// <summary>
    /// Returns all books(with their Authors and Genres) from DB as Book Models
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<BookModel>> AllBooksAsync()
    {
        var books = await repo.All<Book>()
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .ToListAsync();

        return BooksToBookModels(books);
    }

    /// <summary>
    /// Saves all changes to all DB objects in the Database
    /// </summary>
    /// <returns></returns>
    public async Task SaveChangesAsync()
    {
        await repo.SaveChangesAsync();
    }

    /// <summary>
    /// Private conversion method (from Book Collection to Book Model Collection)
    /// </summary>
    /// <param name="books">Books Collection</param>
    /// <returns>Book Model Collection</returns>
    private IEnumerable<BookModel> BooksToBookModels(IEnumerable<Book> books)
    {
        return books.Select(b => new BookModel
        {
            Id = b.Id,
            Title = b.Title,
            Author = new AuthorModel
            {
                FirstName = b.Author.FirstName,
                MiddleName = b.Author.MiddleName,
                LastName = b.Author.LastName,
                Id = b.Author.Id
            },
            DateOfRelease = b.DateOfRelease,
            Genres = b.Genres.Select(g => new GenreModel
            {
                Name = g.Name,
                Id = g.Id
            }).ToList(),
        });
    }

    /// <summary>
    /// Clears the DB
    /// </summary>
    public void ClearDBAsync()
    {
        repo.ClearDBAsync();
    }
}