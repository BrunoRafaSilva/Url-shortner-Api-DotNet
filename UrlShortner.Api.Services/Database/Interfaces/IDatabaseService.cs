
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Models.Entities;
using UrlShortner.Models;

namespace UrlShortner.Api.Services.Database.Interfaces
{
    public interface IDatabaseService
    {
        Task<IActionResult> GetOriginalUrlAsync();
        Task<IActionResult> RegisterShortUrlAsync(ShortUrls shortUrl);
        Task<Users?> LoginAsync(LoginFormModel loginData);
        Task<Users> RegisterUserAsync(Users user);
    }
}