using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortner.Api.Services.Models.UI
{
    public class ApiSettings
    {
        public required string Database_url { get; set; }
        public required string Database_key { get; set; }
        public required string SecretJwtKey { get; set; }
        public required int TokenExpirationHour { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string PasswordSalt { get; set; }
        public required string EncryptionKey { get; set; }
        public required string EncryptionIV { get; set; }
    }
}