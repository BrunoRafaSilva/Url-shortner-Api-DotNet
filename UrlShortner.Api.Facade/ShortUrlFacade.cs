
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.ShortUrl.Responses;
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
            if (registerShortUrl is ObjectResult objectResult && objectResult.Value is List<ShortUrls> value && value.Count > 0)
            {
                var shortUrl = value[0];
                var response = new RegisterNewUrlResponse
                {
                    LinkReference = shortUrl.LinkReference ?? string.Empty,
                    OriginalUrl = shortUrl.OriginalUrl ?? string.Empty,
                };

                return new OkObjectResult(new
                {
                    success = true,
                    message = "Short URL registered successfully.",
                    data = response
                });
            }

            return new BadRequestObjectResult(new
            {
                success = false,
                message = "Failed to register short URL."
            });
        }

        public async Task<IActionResult> GetOriginalUrlAsync()
        {
            var getOriginalLink = await _databaseService.GetOriginalUrlAsync();
            return getOriginalLink;
        }
    }
}