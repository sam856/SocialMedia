using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Story
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        [Required]
        public DateTime DeleteAt { get; set; } 

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Media> Media { get; set; } = new List<Media>();


        public Story()
        {
            DeleteAt = DateTime.UtcNow.AddHours(24);
        }
    }
}
