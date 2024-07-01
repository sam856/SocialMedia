namespace SocialMedia.Models.DTO
{
    public class PostDto
    {
        public IFormFile? formFile { get; set; }
        public string? Title { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
