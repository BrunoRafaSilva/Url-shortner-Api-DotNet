
using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Api.Models.ShortUrl.Requests
{
    public class RegisterNewUrlRequest
    {
        [Required]
        [Url]
        public required string OriginalUrl { get; set; }

    }
}