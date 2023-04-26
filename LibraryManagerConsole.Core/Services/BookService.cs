using Eventmi.Infrastructure.Data.Common;
using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository repo;
        public BookService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task AddBookAsync(BookViewModel book)
        {
            await repo.AddAsync(book);
            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookModel>> AllBooksAsync()
        {
            return await repo.AllReadonly<Book>()
                .Select(b => new BookModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    DateOfRelease = b.DateOfRelease,
                    Genres = b.Genres,
                    Rating = b.Rating
                })
                .ToListAsync();
        }

        public Task DeleteBookAsync(BookModel book)
        {
            throw new NotImplementedException();
        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var book = await repo.GetByIdAsync<Book>(id);
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                DateOfRelease = book.DateOfRelease,
                Genres = book.Genres,
                Rating = book.Rating
            };
        }
    }
}
