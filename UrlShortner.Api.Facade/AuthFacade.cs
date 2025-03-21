using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Auth.Interfaces;

namespace UrlShortner.Api.Services.Auth
{
    public class AuthFacade : IAuthFacade
    {
        private readonly IAuthService _authService;

        public AuthFacade(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string?> LoginAsync(LoginFormModel loginData)
        {
            var user = await _authService.LoginAsync(loginData);
            if (user == null)
                return null;

            return _authService.GenerateJwtToken(user);
        }

        public async Task<bool> RegisterAsync(RegisterFormModel registerData)
        {
            var user = await _authService.RegisterAsync(registerData);
            return user != null;
        }
    }
}