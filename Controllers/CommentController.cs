using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController(CommentService commentService) : ControllerBase
    {
        private readonly CommentService _commentService = commentService;
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() => Ok(await _commentService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            return comment is null ? NotFound() : Ok(comment);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Comment comment)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (comment == null || comment.BlogId == null || userId == null)
            {
                return BadRequest();
            }
            comment.UserId = userId;
            await _commentService.AddAsync(comment);
            return Ok(comment);
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(Comment comment)
        {
            var existingComment = await _commentService.GetByIdAsync(comment.Id!);
            if (existingComment == null) return BadRequest();
            comment.ModifiedAt = DateTime.UtcNow;
            await _commentService.UpdateAsync(comment);
            return Ok(comment);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
