using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Posts
    {
        public string Id { get; set; }  
        public string Title { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public ICollection<Media> Media { get; set; } = new List<Media>();

        public ICollection<Comments> Comment { get; set; } = new List<Comments>();


    }
}
