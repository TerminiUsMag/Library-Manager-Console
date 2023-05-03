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
    private readonly IAuthorService authorService= null!;
    public BookService(IRepository _repo, IWriter _writer, IReader _reader, IGenreService genreService, IAuthorService authorService)
    {
        this.repo = _repo;
        this.writer = _writer;
        this.reader = _reader;
        this.genreService = genreService;
        this.authorService = authorService;
    }
    /// <summary>
    /// Adds book to DataBase
    /// </summary>
    /// <param name="bookModel">BookModel to add</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task AddBookAsync(BookModel bookModel)
    {
        var book = new Book
        {
            Title = bookModel.Title,
            DateOfRelease = bookModel.DateOfRelease,
        };

        var allBooks = await repo.AllReadonly<Book>().ToListAsync();
        if (allBooks.Any(b => b.Title == book.Title))
        {
            throw new ArgumentException("There's already a book with that title in the library");
        }

        book = await authorService.ExistingAuthorFromBookModelToBook(book, bookModel);
        book = await genreService.ExistingGenresFromBookModelToBook(book, bookModel);

        await repo.AddAsync(book);
        await repo.SaveChangesAsync();
    }


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

    public async Task DeleteBookAsync(BookModel bookModel)
    {
        var book = await repo.All<Book>().Where(b => b.Title == bookModel.Title).FirstOrDefaultAsync();
        if (book == null)
        {
            return;
        }
        await repo.DeleteAsync<Book>(book.Id);
        await repo.SaveChangesAsync();
    }
    public async Task DeleteBooksAsync(IEnumerable<BookModel> bookModels)
    {
        foreach (var bookModel in bookModels)
        {
            await DeleteBookAsync(bookModel);
        }
    }

    public async Task<BookModel> GetBookByIdAsync(int id)
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
    public void AddGenresToBookModel(BookModel book, string[] genreNames)
    {
        for (int i = 0; i < genreNames.Length; i++)
        {
            if (!book.Genres.Any(g => g.Name == genreNames[i]))
            {
                book.Genres.Add(new GenreModel
                {
                    Name = genreNames[i]
                });
            }
        }
    }
    public void AddGenreToBookModel(BookModel book, GenreModel genre)
    {

        if (book.Genres.Any(g => g.Name == genre.Name))
        {
            throw new ArgumentException("There's already a genre with this name in the book");
        }
        book.Genres.Add(genre);
    }
    public async void RemoveGenreFromBook(BookModel book, string genreName)
    {
        try
        {
            var genre = genreService.FindGenreInBook(book, genreName);
            book.Genres.Remove(genre);
            await repo.SaveChangesAsync();
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

    public void EditAuthor(BookModel book, string authorName)
    {
        throw new NotImplementedException();
    }

    public void EditAuthor(BookModel book, AuthorModel author)
    {
        throw new NotImplementedException();
    }

    public void DeleteAuthor(BookModel book, string authorName)
    {
        throw new NotImplementedException();
    }

    public AuthorModel FindAuthor(string authorName)
    {
        throw new NotImplementedException();
    }

    public void UpdateReleaseDate(BookModel book, string releaseDate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookModel>> AllBooksAsync()
    {
        var books = await repo.All<Book>()
            .Include(b => b.Author)
            .Include(b => b.Genres)
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