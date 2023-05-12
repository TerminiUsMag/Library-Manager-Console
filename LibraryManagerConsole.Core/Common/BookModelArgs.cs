using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerConsole.Core.Common
{
    /// <summary>
    /// Model for creating BookModelArgs which is used to create a BookModel using the bookService function.Once created it cannot be modified, have fun :)
    /// </summary>
    public class BookModelArgs
    {
        /// <summary>
        /// Creating arguments object for BookModel creation
        /// </summary>
        /// <param name="title">Title for the book</param>
        /// <param name="authorName">Book author's full name</param>
        /// <param name="releaseDate">Date of release </param>
        /// <param name="genres">Genres of Book</param>
        public BookModelArgs(string title, string authorName, string releaseDate, string[] genres)
        {
            this.Title = title;
            this.AuthorName = authorName;
            this.ReleaseDate = releaseDate;
            this.Genres = genres;
        }
        //string bookTitle, string authorFullName, string releaseDate, string[] bookGenres
        public string Title { get; init; }
        public string AuthorName { get; init; }
        public string ReleaseDate { get; init; }
        public string[] Genres { get; init; }

    }
}
