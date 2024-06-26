using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Comments
    {
        public string Id { get; set; }
        public string CommentText { get; set; }
        [ForeignKey("UserId")]

        public ApplicationUser User { get; set; }
     
        public string UserId { get; set; }
        [ForeignKey("PostId")]
        public Posts Posts { get; set; }
        public string PostId { get;set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
