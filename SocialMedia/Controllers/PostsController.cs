using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Migrations;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        public PostsController(AppDbContext _context)
        {
            this._context = _context;
        }
        [HttpGet("AllPosts")]
        public async Task <IActionResult> GetAllPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Posts = _context.Posts.Where(x => x.UserId == userId)
                .Include(y => y.Comment)
                .Include(z=>z.Media)
                .ToList();
            return Ok(Posts);
        }

        [HttpPost("AddPosts")]
        public async Task<IActionResult> AddPosts([FromForm] PostDto post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            byte[] mediaData = null;
            if (post.formFile != null && post.formFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await post.formFile.CopyToAsync(memoryStream);
                    mediaData = memoryStream.ToArray();
                }
            }

            var NewPost = new Posts
            {
                UserId = userId,
                Title = post.Title

            };

                 var newMedia = new Media
                 {
                     Id = Guid.NewGuid().ToString(),
                     PostId = NewPost.Id,
                     Data = mediaData,
                 };
            NewPost.Media.Add(newMedia);
            _context.Posts.Add(NewPost);
            await _context.SaveChangesAsync();


            return Ok("Post Added Successfully");
        }



        [HttpPost("DeletePost")]
        public async Task<IActionResult> DeletePost(string id)
        {
            var post =  _context.Posts.FirstOrDefault(x => x.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok("Post Deleted Successfully");
        }


        [HttpPost("EditPost")]
        public async Task<IActionResult> EditPost( PostDto post)
        {
            var oldPost = await _context.Posts.FindAsync(post.Id);
            if (oldPost == null)
            {
                return NotFound("Post not found");
            }

            byte[] mediaData = null;
            if (post.formFile != null && post.formFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await post.formFile.CopyToAsync(memoryStream);
                    mediaData = memoryStream.ToArray();
                }
            }

            if (mediaData != null)
            {
                var newMedia = new Media
                {
                    Id = Guid.NewGuid().ToString(),
                    PostId = oldPost.Id,
                    Data = mediaData
                };

                oldPost.Media.Clear();
                oldPost.Media.Add(newMedia);
            }

            oldPost.Title = post.Title;

            _context.Posts.Update(oldPost);
            await _context.SaveChangesAsync();

            return Ok("Post Edited Successfully");
        }

    }
}
