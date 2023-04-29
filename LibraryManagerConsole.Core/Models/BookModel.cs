using System.Text;

namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Book with Id
    /// </summary>
    public class BookModel : BookViewModel
    {
        public BookModel()
            : base()
        {

        }
        public int Id { get; set; }

        public override string ToString()
        {
            return $@"{this.Title}
Written By : {this.Author.ToString()}
Genre : {string.Join(", ", this.Genres)}
Released on : {this.DateOfRelease.ToString()}";
        }
    }
}
