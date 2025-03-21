using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Api.Models.Auth.Models
{
    public class LoginFormModel
    {
        [Required(ErrorMessage = "O campo de e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo de senha é obrigatório.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public required string Password { get; set; }
    }
}