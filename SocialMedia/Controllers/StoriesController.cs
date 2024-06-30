using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using System.Net.Http.Headers;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace SocialMedia.Controllers
{
    [Authorize]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public StoriesController(AppDbContext _appDbContext)
        {
            this._appDbContext = _appDbContext;
        }
        [HttpGet("AllStories")]
        
        public IActionResult  AllStories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Allstories = _appDbContext.Stories.Where(x => x.UserId == userId)
                .Include(y => y.Media).ToList();
            return Ok(Allstories);
        }

        [HttpPost("AddStories")]

        public  async Task<IActionResult> AddStory([FromForm] StoryDto story)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            byte[] mediaData = null;
            if (story.formFile != null && story.formFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await story.formFile.CopyToAsync(memoryStream);
                    mediaData = memoryStream.ToArray();
                }
            }

            var newStory = new Story
            {
                UserId= userId,
                CreatedAt = DateTime.UtcNow,
                DeleteAt = DateTime.UtcNow.AddHours(24)
            };

            var newMedia = new Media
            {
                Id = Guid.NewGuid().ToString(),
                storyId = newStory.Id,
                Data = mediaData,
                CreatedAt = DateTime.UtcNow
            };

            newStory.Media.Add(newMedia);
            _appDbContext.Stories.Add(newStory);
            await _appDbContext.SaveChangesAsync();
            return Ok(newStory);
        }


        [HttpPost("DeleteStory")]
        public async Task<IActionResult> DeleteStory(string Id)
        {
            var story = _appDbContext.Stories.Where(x=>x.Id== Id).FirstOrDefault(); 
            if (story != null)
            {
                _appDbContext.Stories.Remove(story);
                await _appDbContext.SaveChangesAsync();

                return Ok("Deleted Successfully");
            }

            return BadRequest();
        }

    }
}
