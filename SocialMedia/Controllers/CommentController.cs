using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;
using SocialMedia.Models.DTO;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace SocialMedia.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly AppDbContext appContext;

        public CommentController(AppDbContext appContext)
        {
            this.appContext = appContext;
        }


        [HttpPost("AddComment")]
        public IActionResult AddComment(CommentDto comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var NewComment = new Comments
            {
                UserId = userId,
                CommentText = comment.CommentText,
                PostId = comment.PostId
            };
           appContext.Comments.Add(NewComment);
            appContext.SaveChanges();

            return Ok("Added Successfully");


        }


        [HttpDelete("DeleteComment")]
        public IActionResult  DeleteComment(string id)
        {

            var comment = appContext.Comments.Where(Z => Z.Id == id)
               .FirstOrDefault();
            appContext.Remove(comment);
            appContext.SaveChanges();

            return Ok("Deleted Successfully");


        }
    }
}
