using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortner.Api.Models.ShortUrl.Responses
{
    public class RegisterNewUrlResponse
    {
        [Required]
        public required string Id { get; set; }
    }
}