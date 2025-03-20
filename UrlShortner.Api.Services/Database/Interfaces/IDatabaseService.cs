
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Models.Auth.Models;
using UrlShortner.Api.Services.Models.Entities;

namespace UrlShortner.Api.Services.Database.Interfaces
{
    public interface IDatabaseService
    {
        Task<IActionResult> GetOriginalUrlAsync();
        Task<IActionResult> RegisterShortUrlAsync(string originalUrl);
        Task<Users?> LoginAsync(LoginFormModel loginData);
        Task<Users> RegisterUserAsync(Users user);
    }
}