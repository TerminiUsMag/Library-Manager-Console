using LibraryManagerConsole.Core.Contracts;
using LibraryManagerConsole.Core.IO.Contracts;
using LibraryManagerConsole.Core.Models;
using LibraryManagerConsole.Infrastructure.Common;
using LibraryManagerConsole.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerConsole.Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository repo = null!;
        private readonly IWriter writer = null!;
        private readonly IReader reader = null!;
        public AuthorService(IRepository _repo, IWriter _writer, IReader _reader)
        {
            this.repo = _repo;
            this.writer = _writer;
            this.reader = _reader;
        }
        public async Task<Book> ExistingAuthorFromBookModelToBook(Book book, BookModel bookModel)
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
                writer.WriteLine("The author is existing!");
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
        public Task AddAuthorAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAuthorAsync()
        {
            throw new NotImplementedException();
        }

        public Task FindAuthorAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task FindAuthorAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAuthorAsync(AuthorViewModel author)
        {
            throw new NotImplementedException();
        }
    }
}
