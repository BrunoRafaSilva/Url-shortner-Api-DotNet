using System;
using System.Collections.Generic;
using System.Linq;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace UrlShortner.Models
{
    [Table("short_urls")]
    public class ShortUrl : BaseModel
    {
        [PrimaryKey("id")]
        public string? Id { get; set; }

        [Column("original_url")]
        public string? OriginalUrl { get; set; }

        [Column("visit_count")]
        public int? VisitCount { get; set; }
        
        [Column("owner_id")]
        public int OwnerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}