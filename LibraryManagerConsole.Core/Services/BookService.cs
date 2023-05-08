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
    /// Adds Book to the Database
    /// </summary>
    /// <param name="bookModel">BookModel to add to DB</param>
    /// <returns>Nothing</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task AddBookAsync(BookModel bookModel)
    {
        var allBooks = await repo
            .AllReadonly<Book>()
            .Where(b => b.Title == bookModel.Title)
            .FirstOrDefaultAsync();
        if (allBooks is not null)
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
    /// Returns all books(with their Authors and Genres) in DB as readonly
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
        if(book is null)
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
        var genres = genreNames.Split(' ',StringSplitOptions.RemoveEmptyEntries);

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
    /// Adds Genre to book model
    /// </summary>
    /// <param name="bookModel"></param>
    /// <param name="genreModel"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddGenreToBookModel(BookModel bookModel, GenreModel genreModel)
    {

        if (bookModel.Genres.Any(g => g.Name == genreModel.Name))
        {
            throw new ArgumentException("There's already a genre with this name in the book");
        }
        bookModel.Genres.Add(genreModel);
    }
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

    public BookModel CreateFullBookModel(string bookTitle, string authorFirstName, string authorMiddleName, string authorLastName, string releaseDate, string[] bookGenres)
    {
        if (bookTitle.IsNullOrEmpty() || authorFirstName.IsNullOrEmpty() || authorMiddleName.IsNullOrEmpty() || authorLastName.IsNullOrEmpty() || bookGenres.Length < 1)
        {
            throw new ArgumentException("One or more of the required parameters is null or empty!");
        }
        //DateTime dateOfRelease;
        if (!DateTime.TryParse(releaseDate, out DateTime dateOfRelease))
        {
            dateOfRelease = DateTime.Now.Date;
        }

        var newBookModel = new BookModel
        {
            Title = bookTitle,
            Author = new AuthorModel
            {
                FirstName = authorFirstName,
                MiddleName = authorMiddleName,
                LastName = authorLastName
            },
            DateOfRelease = dateOfRelease.Date,
            //Genres = new List<GenreModel>()
        };
        this.AddGenresToBookModel(newBookModel, bookGenres);
        return newBookModel;
    }
    //Test required!
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
            var authorNames = authorFullname.Split(" ");
            author.FirstName = authorNames[0];
            author.MiddleName = authorNames[1];
            author.LastName = authorNames[2];
        }

        //await repo.SaveChangesAsync();
    }

    public async Task EditAuthorInBookModel(BookModel bookModel, AuthorModel authorModel)
    {
        var authorFullName = authorModel.ToString();
        await this.EditAuthorInBookModel(bookModel, authorFullName);
    }

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

    public void UpdateReleaseDate(BookModel bookModel, string releaseDate)
    {
        if (DateTime.TryParse(releaseDate, out DateTime date))
        {
            UpdateReleaseDate(bookModel, date);
        }
;
    }

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
        //await repo.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookModel>> AllBooksAsync()
    {
        var books = await repo.All<Book>()
            .Include(b => b.Author)
            .Include(b => b.Genres)
            .ToListAsync();

        return BooksToBookModels(books);
    }

    public async Task SaveChangesAsync()
    {
        await repo.SaveChangesAsync();
    }

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
}