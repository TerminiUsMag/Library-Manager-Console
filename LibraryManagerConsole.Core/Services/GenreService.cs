using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagerConsole.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository repo = null!;
        private readonly IWriter writer = null!;
        private readonly IReader reader = null!;
        public GenreService(IRepository _repo, IWriter _writer, IReader _reader)
        {
            this.repo = _repo;
            this.writer = _writer;
            this.reader = _reader;
        }

        /// <summary>
        /// Creates a new Genre Model if there's no such genre in Database
        /// </summary>
        /// <param name="genreName">Name of the new genre</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GenreModel> CreateGenreModel(string genreName)
        {
            if (genreName.IsNullOrEmpty())
            {
                throw new ArgumentException("Required argument is null or empty");
            }

            var genre = await repo.All<Genre>().Where(g => g.Name == genreName).FirstOrDefaultAsync();

            if (genre == null)
            {
                return new GenreModel { Name = genreName };
            }
            else
            {
                return new GenreModel { Id = genre.Id, Name = genre.Name };
            }
        }

        /// <summary>
        /// Transfers genres from Book Model to Book (checks for existing genres in DB)
        /// </summary>
        /// <param name="book"></param>
        /// <param name="bookModel"></param>
        /// <returns>Book</returns>
        public async Task<Book> ExistingGenresFromBookModelToBook(Book book, BookModel bookModel)
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
            writer.WriteLine($"Existing Genres added to book : {existingGenresCounter}");
            return book;
        }

        /// <summary>
        /// Returns Genre Model with specific name from Book Model
        /// </summary>
        /// <param name="bookModel">Book Model to search for genre</param>
        /// <param name="genreName">Genre name to find</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        public GenreModel FindGenreInBookModel(BookModel bookModel, string genreName)
        {
            if (genreName.IsNullOrEmpty())
            {
                throw new ArgumentException("A required argument is null or empty");
            }

            GenreModel? genreModel = bookModel.Genres.Where(g => g.Name == genreName).FirstOrDefault();
            if (genreModel == null)
            {
                //writer.WriteLine($"No such genre in '{book.Title}' book");
                throw new ArgumentException($"No such genre in book - '{bookModel.Title}'");
            }
            return genreModel;
        }

        /// <summary>
        /// Find Genre in Database and return it as Genre Model
        /// </summary>
        /// <param name="genreName">Name of Genre to find</param>
        /// <returns>Genre Model</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<GenreModel> FindGenreAsync(string genreName)
        {
            if (genreName.IsNullOrEmpty())
            {
                throw new ArgumentException("A required argument is null or empty");
            }

            var genre = await repo.All<Genre>().Where(g => g.Name == genreName).FirstOrDefaultAsync();

            if (genre is null)
            {
                throw new ArgumentException("There's no genre with this name in the DB");
            }
            return new GenreModel
            {
                Name = genre.Name,
                Id = genre.Id,
            };
        }

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genreName">Name of the new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddGenreAsync(string genreName)
        {
            if (genreName.IsNullOrEmpty())
            {
                throw new ArgumentException("Required argument is null or empty");
            }

            var genreInDb = await repo
                .All<Genre>()
                .Where(g=>g.Name==genreName)
                .FirstOrDefaultAsync();

            if (genreInDb is not null)
            {
                throw new ArgumentException("There's already a genre with the same name in the DB");
            }

            var newGenre = new Genre
            {
                Name = genreName,
            };

            await repo.AddAsync(newGenre);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genreModel">Genre Model of the new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddGenreAsync(GenreModel genreModel)
        {
            await AddGenreAsync(genreModel.Name);
        }

        /// <summary>
        /// Adds a new Genre to DB
        /// </summary>
        /// <param name="genre">new Genre</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddGenreAsync(Genre genre)
        {
            await AddGenreAsync(genre.Name);
        }
    }
}
