namespace SocialMedia.Models.DTO
{
    public class CommentDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CommentText { get; set; }
        public string PostId { get; set; }

    }
}
