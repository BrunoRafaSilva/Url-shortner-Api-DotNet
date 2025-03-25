
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Services.Database.Interfaces;
using UrlShortner.Models;

namespace UrlShortner.Api.Facade
{
    public class ShortUrlFacade : IShortUrlFacade
    {
        private readonly IDatabaseService _databaseService;


        public ShortUrlFacade(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<IActionResult> RegisterShortUrlAsync(string originalUrl, int userId)
        {
            var urlToShort = new ShortUrls
            {
                OriginalUrl = originalUrl,
                OwnerId = userId
            };
            var registerShortUrl = await _databaseService.RegisterShortUrlAsync(urlToShort);
            return registerShortUrl;
        }

        public async Task<IActionResult> GetOriginalUrlAsync()
        {
            var getOriginalLink = await _databaseService.GetOriginalUrlAsync();
            return getOriginalLink;
        }
    }
}