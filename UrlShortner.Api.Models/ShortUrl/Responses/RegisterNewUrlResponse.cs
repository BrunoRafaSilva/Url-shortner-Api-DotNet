using System.ComponentModel.DataAnnotations;

namespace UrlShortner.Api.Models.ShortUrl.Responses
{
    public class RegisterNewUrlResponse
    {
        [Required]
        public required string LinkReference { get; set; }
        [Required]
        public required string OriginalUrl { get; set; }
    }
}