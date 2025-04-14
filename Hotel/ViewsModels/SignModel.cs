using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewsModels
{
    public class SignModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Phone { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}