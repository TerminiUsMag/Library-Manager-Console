using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerConsole.Core.Models
{
    /// <summary>
    /// Author without Id
    /// </summary>
    public class AuthorViewModel : AuthorModel
    {
        [Display(Name = "Author's first name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Author's middle name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string MiddleName { get; set; } = null!;
        [Display(Name = "Author's last name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "The field '{0}' is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The field '{0}' must be between {2} and {1} characters")]
        public string LastName { get; set; } = null!;
    }
}
