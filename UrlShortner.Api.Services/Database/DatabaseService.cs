using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Services.Database.Interfaces;
using Microsoft.Extensions.Options;
using UrlShortner.Api.Services.Models.UI;
using UrlShortner.Models;
using UrlShortner.Api.Services.Models.Entities;
using UrlShortner.Api.Models.Auth.Models;
using System.Text;
using System.Security.Cryptography;

namespace UrlShortner.Api.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ApiSettings _apisettings;

        public DatabaseService(IOptions<ApiSettings> apisettings)
        {
            _apisettings = apisettings.Value;
        }

        private async Task<Supabase.Client> ConnectToDatabaseAsync()
        {
            var databaseUrl = _apisettings.Database_url;
            var databaseKey = _apisettings.Database_key;

            var databaseOptions = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var databaseConnection = new Supabase.Client(databaseUrl, databaseKey, databaseOptions);
            await databaseConnection.InitializeAsync();
            return databaseConnection;
        }

        public async Task<IActionResult> GetOriginalUrlAsync()
        {
            var databaseConnection = await ConnectToDatabaseAsync();
            var resultOfSearchAsync = await databaseConnection.From<City>().Get();
            var result = resultOfSearchAsync.Models.ToList();
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> RegisterShortUrlAsync(string originalUrl)
        {
            var databaseConnection = await ConnectToDatabaseAsync();
            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                VisitCount = 0,
                CreatedAt = DateTime.Now,
                OwnerId = 1
            };

            var resultOfInsertAsync = await databaseConnection.From<ShortUrl>().Insert(shortUrl);
            var result = resultOfInsertAsync.Models.ToList();
            return new OkObjectResult(result);
        }

        public async Task<Users?> LoginAsync(LoginFormModel loginData)
        {
            var loginEmail = loginData.Email;
            var loginPassword = loginData.Password;

            var databaseConnection = await ConnectToDatabaseAsync();
            var resultOfSearchAsync = await databaseConnection.From<Users>().Where(u => u.Email == loginEmail).Get();
            var searchUserResult = resultOfSearchAsync.Models.ToList();

            if (searchUserResult != null && searchUserResult.Count > 0)
            {
                var user = searchUserResult[0];
                var databasePassword = user.Password;

                var saltedLoginPassword = SaltPassword(loginPassword);

                var passwordMatch = BCrypt.Net.BCrypt.Verify(saltedLoginPassword, databasePassword);
                if (passwordMatch)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<Users> RegisterUserAsync(Users user)
        {
            var emailToRegister = user.Email;
            var passwordToRegister = user.Password;

            var saltedPassword = SaltPassword(passwordToRegister);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword);

            var userToCreate = new Users
            {
                Email = emailToRegister,
                Password = hashedPassword,
                Active = true,
                Role = "Default",
                CreatedAt = DateTime.Now
            };

            var databaseConnection = await ConnectToDatabaseAsync();
            var resultOfInsertAsync = await databaseConnection.From<Users>().Insert(userToCreate);
            var result = resultOfInsertAsync.Models.ToList();
            return result[0];
        }

        private string SaltPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("A senha não pode estar vazia.");
            }
            
            var salt = _apisettings.PasswordSalt;
            var saltedPassword = password + salt;

            return saltedPassword;
        }

    }
}