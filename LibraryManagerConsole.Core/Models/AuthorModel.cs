namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Author with Id
    /// </summary>
    public class AuthorModel : AuthorViewModel
    {
        public AuthorModel()
            : base()
        {

        }
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.MiddleName} {this.LastName}";
        }
    }
}
