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
            var existingAuthor = await FindAuthorAsync(bookModel.ToString());

            //var authors = await repo
            //.All<Author>()
            //.ToListAsync();

            //var existingAuthor = authors
            //    .Where(a => a.FirstName == bookModel.Author.FirstName && a.MiddleName == bookModel.Author.MiddleName && a.LastName == bookModel.Author.LastName)
            //    .ToList();

            if (CheckIfAuthorIsValid(existingAuthor))
            {
                book.Author = existingAuthor;
                //writer.WriteLine("The author is existing!");
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
        public async Task UpdateAuthorOfBookInDbAsync(BookModel bookModel, AuthorModel authorModel)
        {
            var book = await repo
                .All<Book>()
                .Where(b => b.Title == bookModel.Title && b.DateOfRelease == bookModel.DateOfRelease)
                .FirstOrDefaultAsync();
            if (book is null)
            {
                throw new ArgumentException("There's no such book in the database");
            }

            var author = await FindAuthorAsync(authorModel.ToString());

            if (author is null)
            {
                author = new Author
                {
                    FirstName = authorModel.FirstName,
                    MiddleName = authorModel.MiddleName,
                    LastName = authorModel.LastName,
                };
            }

            book.Author = author;
            await repo.SaveChangesAsync();
        }

        public async Task DeleteAuthorFromDBAsync(AuthorModel authorModel)
        {
            var author = await this.FindAuthorAsync(authorModel.ToString());

            if (CheckIfAuthorIsValid(author!))
            {
                throw new ArgumentException("There's no such author in Database");
            }

            await repo.DeleteAsync<Author>(author!.Id);
            await repo.SaveChangesAsync();
        }

        public async Task<Author> FindAuthorAsync(int id)
        {
            var author = await repo
                .All<Author>()
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            if (CheckIfAuthorIsValid(author!))
            {
                throw new ArgumentException("No author with this ID in Database");
            }

            return author!;

        }

        public async Task<Author> FindAuthorAsync(string fullName)
        {
            var names = fullName.Split(' ');

            var author = await repo
                .All<Author>()
                .Where(a => a.FirstName == names[0] && a.MiddleName == names[1] && a.LastName == names[2])
                .FirstOrDefaultAsync();

            if (CheckIfAuthorIsValid(author!))
            {
                throw new ArgumentException("No author with this name in Database");
            }

            return author!;
        }

        public async Task UpdateAuthorAsync(AuthorModel authorModel)
        {
            if (CheckIfAuthorIsValid(authorModel))
            {
                throw new ArgumentException("Invalid or empty authorModel input");
            }

            var author = await FindAuthorAsync(authorModel.ToString());

            if (CheckIfAuthorIsValid(author))
            {
                throw new ArgumentException("There's no such author in Database");
            }

            writer.WriteLine("Enter new full name for Author with ID:" + author.Id + " (separate with ' ')");
            var newFullName = reader.ReadLine();
            var names = newFullName.Split(" ");
            author.FirstName = names[0];
            author.MiddleName = names[1];
            author.LastName = names[2];
            await repo.SaveChangesAsync();
        }

        private bool CheckIfAuthorIsValid(Object author)
        {
            if (author is null)
            {
                return false;
            }
            return true;
        }
    }
}
