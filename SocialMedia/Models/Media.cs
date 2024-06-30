using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Media
    {
        public string Id { get; set; }

        public string? PostId { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("PostId")]

        public Posts? Post { get; set; }
        [ForeignKey("storyId")]
        public Story? story { get; set; }
       
        public string? storyId { get; set; }
    }

}
