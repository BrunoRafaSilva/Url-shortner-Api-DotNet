using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Models.Entities;

namespace UrlShortner.Api.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(Users user);

        Task<Users> LoginAsync(LoginFormModel userLoginData);

        Task<Users> RegisterAsync(RegisterFormModel userRegisterData);
    }
}