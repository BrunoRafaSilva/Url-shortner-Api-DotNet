
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UrlShortner.Api.Services.Models.Entities
{
    [Table("users")]
    public class Users : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("active")]
        public bool Active { get; set; }
        [Column("role")]
        public string? Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}