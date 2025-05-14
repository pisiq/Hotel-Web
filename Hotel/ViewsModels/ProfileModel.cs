using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewsModels
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Current profile picture path
        public string ProfilePicturePath { get; set; }

        // Property for uploading a new profile picture
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }
    }
}