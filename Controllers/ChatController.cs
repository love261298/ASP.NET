using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController(ChatService chatService, CommentService commentService) : ControllerBase
    {
        private readonly ChatService _chatService = chatService;
        private readonly CommentService _commentService = commentService;
        [HttpGet]
        [Authorize]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var chats = await _chatService.GetAllAsync();

            var tasks = chats.Select(async c =>
            {
                c.Comment = await _commentService.GetCommentsByChatIdAsync(c.Id!);
            });

            await Task.WhenAll(tasks);

            return Ok(chats);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            var chat = await _chatService.GetByIdAsync(id);
            chat.Comment = await _commentService.GetCommentsByChatIdAsync(chat.Id!);
            return chat is null ? NotFound() : Ok(chat);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string partnerId)
        {
            if (string.IsNullOrEmpty(partnerId))
            {
                return BadRequest("PatternId is required.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userObjectId = new ObjectId(userId);
            var partnerObjectId = new ObjectId(partnerId);
            var existingChat = await _chatService.FindChatBetweenUsersAsync(userObjectId, partnerObjectId);
            if (existingChat != null)
            {
                existingChat.Comment = await _commentService.GetCommentsByChatIdAsync(existingChat.Id!);
                return Ok(existingChat);
            }

            Chat chat = new Chat
            {
                UserIds = new List<ObjectId>
        {
            userObjectId,
            partnerObjectId
        }
            };
            await _chatService.AddAsync(chat);
            return Ok(chat);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Update(Chat chat)
        {
            var existingChat = await _chatService.GetByIdAsync(chat.Id!);
            if (existingChat == null) return BadRequest();
            await _chatService.UpdateAsync(chat);
            return Ok(chat);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var existingChat = await _chatService.GetByIdAsync(id);
            if (existingChat == null) return NotFound();
            await _chatService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetChatsByUserIdAsync()
        {
            var userId = new MongoDB.Bson.ObjectId(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var chats = await _chatService.GetChatsByUserIdAsync(userId);
            return Ok(chats);
        }
    }
}
