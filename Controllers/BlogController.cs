using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController(BlogService blogService, UserService userService, CommentService commentService) : ControllerBase
    {
        private readonly BlogService _blogService = blogService;
        private readonly UserService _userService = userService;
        private readonly CommentService _commentService = commentService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var blogs = await _blogService.GetAllAsync();
            blogs.ForEach(async b => b.Comment = await _commentService.GetCommentsByBlogIdAsync(b.Id!));
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            blog.Comment = await _commentService.GetCommentsByBlogIdAsync(blog.Id!);
            return blog is null ? NotFound() : Ok(blog);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Blog blog)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (blog == null || userId == null)
            {
                return BadRequest();
            }
            blog.UserId = userId;
            await _blogService.AddAsync(blog);
            return Ok(blog);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(Blog blog)
        {
            var existingBlog = await _blogService.GetByIdAsync(blog.Id!);
            var existingUser = await _userService.GetByIdAsync(blog.UserId!);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (blog.UserId != userId)
            {
                return BadRequest("You are not authorized to update this blog.");
            }
            if (existingBlog == null || existingUser == null) return BadRequest();
            blog.ModifiedAt = DateTime.UtcNow;
            await _blogService.UpdateAsync(blog);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _blogService.DeleteAsync(id);
            return NoContent();
        }
    }
}
