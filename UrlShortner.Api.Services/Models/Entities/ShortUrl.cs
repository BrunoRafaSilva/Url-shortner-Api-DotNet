using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UrlShortner.Models
{
    [Table("short_urls")]
    public class ShortUrls : BaseModel
    {
        [PrimaryKey("id")]
        public string? Id { get; set; }

        [Column("original_url")]
        public string? OriginalUrl { get; set; }

        [Column("visit_count")]
        public int VisitCount { get; set; } = 0;

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("link_reference")]
        public string? LinkReference { get; set; } = Guid.NewGuid().ToString();

        [Column("is_active")]
        public bool IsActive { get; set; } 
    }
}