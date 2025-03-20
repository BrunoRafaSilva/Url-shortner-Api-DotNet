
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Auth.Interfaces;
using UrlShortner.Api.Services.Database.Interfaces;
using UrlShortner.Api.Services.Models.Entities;
using UrlShortner.Api.Services.Models.UI;

namespace UrlShortner.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApiSettings _apisettings;
        private readonly IDatabaseService _databaseService;

        public AuthService(IOptions<ApiSettings> apiSettings, IDatabaseService databaseService)
        {
            _apisettings = apiSettings.Value;
            _databaseService = databaseService;
        }

        public string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apisettings.SecretJwtKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                Issuer = _apisettings.Issuer,
                Audience = _apisettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            var salt = _apisettings.PasswordSalt;
            var saltedPassword = password + salt;
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashedBytes);
        }

        public async Task<Users> Authenticate(LoginFormModel userLoginData)
        {
            var userSearch = await _databaseService.LoginAsync(userLoginData);
            if (userSearch == null)
            {
                throw new InvalidOperationException("Usuário ou senha incorretos");
            }

            var foundUserData = userSearch;

            var userLoginPassword = userLoginData.Password;
            var foundUserPassword = foundUserData.Password;

            var decryptedUserLoginPassword = userLoginPassword;

            if (decryptedUserLoginPassword != foundUserPassword)
            {
                throw new InvalidOperationException("Usuário ou senha incorretos");
            }

            return new Users
            {
                Id = foundUserData.Id,
                Email = foundUserData.Email,
                Role = foundUserData.Email,
                Password = foundUserData.Password
            };
        }

        public async Task<Users> Register(RegisterFormModel userRegisterData)
        {
            var userRegisterEmail = userRegisterData.Email;
            var userRegisterPassword = userRegisterData.Password;

            var userToRegister = new Users
            {
                Email = userRegisterEmail,
                Password = userRegisterPassword
            };
            var userEntity = await _databaseService.RegisterUserAsync(userToRegister);
            return new Users
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Role = userEntity.Email,
                Password = userEntity.Password
            };
        }

    }
}