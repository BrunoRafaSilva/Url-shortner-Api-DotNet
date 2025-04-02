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

        [HttpGet("ListAllLinks")]
        [Authorize]
        public async Task<IActionResult> GetUrls()
        {
            var result = await _shortUrlFacade.GetAllRegistredlUrlsAsync();
            return Ok(result);
        }

        [HttpGet("getRedirectLink/{shortUrl}")]
        public async Task<IActionResult> Get(string shortUrl)
        {
            var result = await _shortUrlFacade.GetRedirectUrlFromGuid(shortUrl);
            return Redirect(result);
        }

        [HttpPost("RegisterNewUrl")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] RegisterNewUrlRequest registerUrlObject)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userId, out var userIdInt))
            {
                return BadRequest("Invalid user ID.");
            }
            var urlToShort = registerUrlObject.OriginalUrl;
            var result = await _shortUrlFacade.RegisterShortUrlAsync(urlToShort, userIdInt);
            return result;
        }
    }
}