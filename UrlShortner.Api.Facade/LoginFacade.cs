using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Database.Interfaces;
using UrlShortner.Api.Services.Models.Entities;

namespace UrlShortner.Api.Facade
{
    public class LoginFacade : ILoginFacade
    {
        private readonly IDatabaseService _databaseService;

        public LoginFacade(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<Users> LoginAsync(LoginFormModel loginData)
        {
            var loginFormData = new LoginFormModel
            {
                Email = loginData.Email,
                Password = loginData.Password
            };
            var login = await _databaseService.LoginAsync(loginFormData);
            return login;
        }
    }
}