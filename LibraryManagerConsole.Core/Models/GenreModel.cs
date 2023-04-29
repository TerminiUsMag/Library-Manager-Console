namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Genre model with Id
    /// </summary>
    public class GenreModel : GenreViewModel
    {
        public GenreModel()
            : base()
        {
        }
        public int Id { get; set; }
        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
