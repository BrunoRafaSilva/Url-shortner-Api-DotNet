using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Api.Facade.Interfaces;
using UrlShortner.Api.Models.ShortUrl.Requests;

namespace UrlShortner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortUrlFacade _shortUrlFacade;

        public ShortUrlController(IShortUrlFacade shortUrlFacade)
        {
            _shortUrlFacade = shortUrlFacade;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _shortUrlFacade.GetOriginalUrlAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] RegisterNewUrlRequest registerUrlObject)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var urlToShort = registerUrlObject.OriginalUrl;
            var result = await _shortUrlFacade.RegisterShortUrlAsync(urlToShort);
            return Ok(result);
        }
    }
}