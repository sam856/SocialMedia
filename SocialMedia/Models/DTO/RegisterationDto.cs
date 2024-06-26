using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTO
{
    public class RegisterationDto
    {
        public string Username { get; set; }    
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
    }

}
