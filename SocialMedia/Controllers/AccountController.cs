using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SocialMedia.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;


        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;


        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterationDto model, IFormFile image)
        {
            byte[] imageUser = null;

            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Handle image upload
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageUser = memoryStream.ToArray();
                }
            }

            // Create user object
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Image = imageUser // Assign the byte array of the image to the user object
            };

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginModel)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    Boolean found = await _userManager.CheckPasswordAsync(user, loginModel.Password);
                    if (found)
                    {
                        var cliams = new List<Claim>();
                        cliams.Add(new Claim(ClaimTypes.Name, user.UserName));
                        cliams.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        cliams.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            cliams.Add(new Claim(ClaimTypes.Role, role));


                        }
                        SecurityKey key = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

                        SigningCredentials signingCredentials = new SigningCredentials(
                            key, SecurityAlgorithms.HmacSha256
                            );

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],

                            audience: _configuration["JWT:Audience"],
                            claims: cliams,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCredentials


                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            exiration = token.ValidTo

                        });



                    }
                }


            }
            return Unauthorized();
        }
    }
}

