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
        public async Task<GenreModel> CreateGenre(string genreName)
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
        public GenreModel FindGenreInBook(BookModel book, string genreName)
        {
            GenreModel? genre = book.Genres.Where(g => g.Name == genreName).FirstOrDefault();
            if (genre == null)
            {
                writer.WriteLine($"No such genre in '{book.Title}' book");
                throw new ArgumentException($"No such genre in book - {book.Title}");
            }
            return genre;
        }
        public Task AddGenre()
        {
            throw new NotImplementedException();
        }
    }
}
