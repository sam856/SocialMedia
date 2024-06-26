using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] Image { get; set; }
    }
}
