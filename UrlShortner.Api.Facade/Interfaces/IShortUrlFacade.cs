using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Models.ShortUrl.Requests;

namespace UrlShortner.Api.Facade.Interfaces
{
    public interface IShortUrlFacade
    {
        Task<IActionResult> RegisterShortUrlAsync(string originalUrl, int userId);
        Task<IActionResult> GetAllRegistredlUrlsAsync();
        Task<string> GetRedirectUrlFromGuid(string shortUrl);
    }
}