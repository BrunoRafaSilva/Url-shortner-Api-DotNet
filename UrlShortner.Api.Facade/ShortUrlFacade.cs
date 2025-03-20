
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Services.Database.Interfaces;
using UrlShortner.Api.Services.Models.UI;

namespace UrlShortner.Api.Facade
{
    public class ShortUrlFacade : IShortUrlFacade
    {
        private readonly IDatabaseService _databaseService;


        public ShortUrlFacade(IDatabaseService databaseService, ApiSettings apiSettings)
        {
            _databaseService = databaseService;
        }

        public async Task<IActionResult> RegisterShortUrlAsync(string originalUrl)
        {
            var registerShortUrl = await _databaseService.RegisterShortUrlAsync(originalUrl);
            return registerShortUrl;
        }

        public async Task<IActionResult> GetOriginalUrlAsync()
        {
            var getOriginalLink = await _databaseService.GetOriginalUrlAsync();
            return getOriginalLink;
        }
    }
}