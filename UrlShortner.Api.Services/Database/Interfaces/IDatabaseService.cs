
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Models.Entities;
using UrlShortner.Models;

namespace UrlShortner.Api.Services.Database.Interfaces
{
    public interface IDatabaseService
    {
        Task<Users?> LoginAsync(LoginFormModel loginData);
        Task<Users> RegisterUserAsync(Users user);
        Task<IActionResult> GetOriginalUrlAsync();
        Task<string> GetRedirectUrlFromGuid(string shortUrl);
        Task<IActionResult> RegisterShortUrlAsync(ShortUrls shortUrl);
    }
}