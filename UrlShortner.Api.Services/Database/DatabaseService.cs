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

            // Log temporário para depuração
            Console.WriteLine($"[Login] Password Provided: {loginPassword}");

            var databaseConnection = await ConnectToDatabaseAsync();
            var resultOfSearchAsync = await databaseConnection.From<Users>().Where(u => u.Email == loginEmail).Get();
            var searchUserResult = resultOfSearchAsync.Models.ToList();

            if (searchUserResult != null && searchUserResult.Count > 0)
            {
                var user = searchUserResult[0];
                var passwordHashed = user.Password;

                var saltedPassword = loginPassword + _apisettings.PasswordSalt;

                // Log temporário para depuração
                Console.WriteLine($"[Login] Salted Password: {saltedPassword}");
                Console.WriteLine($"[Login] Stored Password Hash: {passwordHashed}");

                var passwordMatch = BCrypt.Net.BCrypt.Verify(saltedPassword, passwordHashed);
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
            var decryptedPasswordToRegister = DecryptPassword(passwordToRegister);
            Console.WriteLine($"[Register] Decrypted Password: {decryptedPasswordToRegister}");
            var passwordEncrypted = EncryptPassword(passwordToRegister);

            var userToCreate = new Users
            {
                Email = emailToRegister,
                Password = passwordEncrypted,
                Active = true,
                Role = "Default",
                CreatedAt = DateTime.Now
            };

            var databaseConnection = await ConnectToDatabaseAsync();
            var resultOfInsertAsync = await databaseConnection.From<Users>().Insert(userToCreate);
            var result = resultOfInsertAsync.Models.ToList();
            return result[0];
        }

        private string EncryptPassword(string password)
        {
            var salt = _apisettings.PasswordSalt;
            var saltedPassword = password + salt;
            var passwordEncrypted = BCrypt.Net.BCrypt.HashPassword(saltedPassword);

            // Log temporário para depuração
            Console.WriteLine($"[Register] Salted Password: {saltedPassword}");
            Console.WriteLine($"[Register] Encrypted Password: {passwordEncrypted}");

            return passwordEncrypted;
        }

        private string DecryptPassword(string encryptedPassword)
        {
            try
            {
                // Decodifica a chave e o IV de Base64 para bytes
                var key = Convert.FromBase64String(_apisettings.EncryptionKey);
                var iv = Convert.FromBase64String(_apisettings.EncryptionIV);

                // Verifica se o comprimento da chave é válido
                if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                {
                    throw new ArgumentException("EncryptionKey deve ter 16, 24 ou 32 bytes após a decodificação Base64.");
                }

                using (var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    var encryptedBytes = Convert.FromBase64String(encryptedPassword);

                    using (var ms = new MemoryStream(encryptedBytes))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd(); // Retorna a senha descriptografada
                    }
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Erro de descriptografia: {ex.Message}");
                throw new InvalidOperationException("Falha ao descriptografar a senha. Verifique a chave, o IV e os dados criptografados.", ex);
            }
        }
    }
}