using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Api.Models.Auth.Models
{
    public class LoginFormModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }
}