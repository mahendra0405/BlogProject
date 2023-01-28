using BlogLab.Models.BlogComment;
using BlogLab.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BlogLab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCommentController : ControllerBase
    {
        private readonly IBlogCommentRepository _blogCommentRepository;

        public BlogCommentController(IBlogCommentRepository blogCommentRepository)
        {
            _blogCommentRepository = blogCommentRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BlogComment>> Create(BlogCommentCreate blogCommentCreate)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

            var createBlogComment = await _blogCommentRepository.UpsertAsync(blogCommentCreate, applicationUserId);
            return Ok(createBlogComment);
        }

        [HttpGet("{blogId}")]
        public async Task<ActionResult<List<BlogComment>>> GetAll(int blogId)
        {
            var blogComments = await _blogCommentRepository.GetAllAsync(blogId);

            return Ok(blogComments);
        }

        [Authorize]
        [HttpDelete("blogCommentId")]
        public async Task<ActionResult<int>> Delete(int blogCommnetId)
        {
            int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);
            //var affectedRows = 0;

            var foundBlogComment = await _blogCommentRepository.GetAsync(blogCommnetId);

            if (foundBlogComment == null) return BadRequest("Comment does not exists");

            if (foundBlogComment.ApplicationUserId == applicationUserId)
            {
                var affectedRows = await _blogCommentRepository.DeleteAsync(blogCommnetId);
                return Ok(affectedRows);

            }
            else
            {
                return BadRequest("This comment was not created by the user");
            }
        }
    }
}
