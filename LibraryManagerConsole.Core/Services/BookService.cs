using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagerConsole.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository repo = null!;
        public BookService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task AddBookAsync(BookModel bookModel)
        {
            var book = new Book
            {
                Title = bookModel.Title,
                DateOfRelease = bookModel.DateOfRelease,
            };

            //book.Genres.Add(bookModel.Genres.Select(bm => new Genre
            //{
            //    Name = bm.Name
            //}).ToList();
            var allBooks = await repo.AllReadonly<Book>().ToListAsync();
            if (allBooks.Any(b => b.Title == book.Title))
            {
                throw new ArgumentException("There's already a book with that title in the library");
            }

            book = await existingAuthor(book, bookModel);
            book = await existingGenre(book, bookModel);

            await repo.AddAsync(book);
            await repo.SaveChangesAsync();
        }

        private async Task<Book> existingGenre(Book book, BookModel bookModel)
        {
            var existingGenres = await repo.All<Genre>().ToListAsync();
            var genresToAdd = new List<Genre>();
            var existingGenresCounter = 0;
            for (int i = 0; i < existingGenres.Count; i++)
            {
                for (int x = 0; x < bookModel.Genres.Count; x++)
                {
                    if (existingGenres[i].Name == bookModel.Genres[x].Name)
                    {
                        bookModel.Genres.RemoveAt(x);
                        genresToAdd.Add(existingGenres[i]);
                        existingGenresCounter++;
                    }
                }
            }
            var genres = bookModel.Genres.Select(g => new Genre
            {
                Name = g.Name
            }).ToList();
            for (int y = 0; y < genres.Count; y++)
            {
                genresToAdd.Add(genres[y]);
            }
            book.Genres = genresToAdd;
            Console.WriteLine($"Existing Genres added to book : {existingGenresCounter}");
            return book;
        }

        private async Task<Book> existingAuthor(Book book, BookModel bookModel)
        {
            var authors = await repo
                .All<Author>()
                .ToListAsync();
            var existingAuthor = authors
                .Where(a => a.FirstName == bookModel.Author.FirstName && a.MiddleName == bookModel.Author.MiddleName && a.LastName == bookModel.Author.LastName)
                .ToList();

            if (existingAuthor.Any())
            {
                book.Author = existingAuthor.First();
                Console.WriteLine("The author is existing!");
            }
            else
            {
                book.Author = new Author
                {
                    FirstName = bookModel.Author.FirstName,
                    MiddleName = bookModel.Author.MiddleName,
                    LastName = bookModel.Author.LastName,
                };
            }
            return book;
        }

        public async Task<IEnumerable<BookModel>> AllBooksAsync()
        {
            var books = await repo.AllReadonly<Book>().Include(b => b.Author)
                //.Select(b => new BookModel
                //{
                //    Id = b.Id,
                //    Title = b.Title,
                //    Author = new AuthorModel
                //    {
                //        FirstName = b.Author.FirstName,
                //        MiddleName = b.Author.MiddleName,
                //        LastName = b.Author.LastName,
                //    },
                //    DateOfRelease = b.DateOfRelease,
                //    Genres = b.Genres.Select(g => new GenreModel
                //    {
                //        Name = g.Name,
                //        Id = g.Id,
                //        Books = g.Books
                //    }).ToList()
                //})
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
                    Id = g.Id,
                    Books = g.Books
                }).ToList()
            }).ToList();
        }

        public async Task DeleteBookAsync(BookModel book)
        {
            await repo.DeleteAsync<Book>(book.Id);
            await repo.SaveChangesAsync();
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
        public async void RemoveGenre(BookModel book, string genreName)
        {
            try
            {
                var genre = FindGenreInBook(book, genreName);
                book.Genres.Remove(genre);
                await repo.SaveChangesAsync();
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine(aex.Message);
            }
        }

        private GenreModel FindGenreInBook(BookModel book, string genreName)
        {
            GenreModel? genre = book.Genres.Where(g => g.Name == genreName).FirstOrDefault();
            if (genre == null)
            {
                Console.WriteLine($"No such genre in '{book.Title}' book");
                throw new ArgumentException($"No such genre in book - {book.Title}");
            }
            return genre;
        }

        public BookModel CreateFullBookModel(string bookTitle, string authorFirstName, string authorMiddleName, string authorLastName, string[] bookGenres)
        {
            if (bookTitle.IsNullOrEmpty() || authorFirstName.IsNullOrEmpty() || authorMiddleName.IsNullOrEmpty() || authorLastName.IsNullOrEmpty() || bookGenres.Length < 1)
            {
                throw new ArgumentException("One of the required parameters is null or empty!");
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
                    DateOfRelease = DateTime.Now,
                    //Genres = new List<GenreModel>()
                };
            this.AddGenresToBookModel(newBookModel, bookGenres);
            return newBookModel;
        }

        public void AddAuthor(BookModel book, string authorName)
        {
            throw new NotImplementedException();
        }

        public void AddAuthor(BookModel book, AuthorModel author)
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
    }
}
