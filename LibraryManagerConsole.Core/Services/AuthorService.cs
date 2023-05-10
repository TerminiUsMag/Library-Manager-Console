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

        /// <summary>
        /// Transfers Author from Book Model to Book (If exists in DB else creates new Author with Author Model names)
        /// </summary>
        /// <param name="book">Book to tranfer to</param>
        /// <param name="bookModel">Book Model to transfer from</param>
        /// <returns>Book</returns>
        public async Task<Book> ExistingAuthorFromBookModelToBook(Book book, BookModel bookModel)
        {
            var existingAuthor = await FindAuthorAsync(bookModel.Author.ToString());

            if (CheckIfAuthorIsValid(existingAuthor))
            {
                book.Author = existingAuthor;
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

        /// <summary>
        /// Update Author of Book In DB
        /// </summary>
        /// <param name="bookModel">Book Model to update</param>
        /// <param name="authorModel">Updated Author Model</param>
        /// <returns>Nothing</returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Deletes Author from Database
        /// </summary>
        /// <param name="authorModel">Author Model to delete</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Returns an Author if exists in DB
        /// </summary>
        /// <param name="id">Id of Author</param>
        /// <returns>Author</returns>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// Returns an Author if exists in DB
        /// </summary>
        /// <param name="fullName">Full name of Author in the format : "'First name' 'Middle name' 'Last name'"</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Author> FindAuthorAsync(string fullName)
        {
            var names = fullName.Split(' ',StringSplitOptions.RemoveEmptyEntries).Select(a=>a.Trim()).ToList();

            var author = await repo
                .All<Author>()
                .Where(a => a.FirstName == names[0] && a.MiddleName == names[1] && a.LastName == names[2])
                .FirstOrDefaultAsync();

            if (CheckIfAuthorIsValid(author))
            {
                return author;
            }

            return null;
        }

        /// <summary>
        /// Update existing Author
        /// </summary>
        /// <param name="authorModel">Author to find</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
            var names = newFullName.Split(" ",StringSplitOptions.RemoveEmptyEntries);
            author.FirstName = names[0];
            author.MiddleName = names[1];
            author.LastName = names[2];
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Private validation method
        /// </summary>
        /// <param name="author">Author to validate</param>
        /// <returns></returns>
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
