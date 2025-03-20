using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Models.Entities;

namespace UrlShortner.Api.Facade.Interfaces
{
    public interface ILoginFacade
    {
        Task<Users> LoginAsync(LoginFormModel loginData);
    }
}