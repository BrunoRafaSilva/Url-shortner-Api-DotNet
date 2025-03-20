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
        public async Task<IActionResult> Get()
        {
            var result = await _shortUrlFacade.GetOriginalUrlAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterNewUrlRequest registerUrlObject)
        {
            var urlToShort = registerUrlObject.OriginalUrl;
            var result = await _shortUrlFacade.RegisterShortUrlAsync(urlToShort);
            return Ok(result);
        }
    }
}