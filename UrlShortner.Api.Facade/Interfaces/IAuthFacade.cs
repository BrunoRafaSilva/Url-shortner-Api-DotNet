using UrlShortner.Api.Models.Auth.Models;

namespace UrlShortner.Api.Facade.Interfaces
{
    public interface IAuthFacade
    {
        Task<string?> LoginAsync(LoginFormModel loginData);
        Task<bool> RegisterAsync(RegisterFormModel registerData);
    }
}