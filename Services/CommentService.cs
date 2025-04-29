using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class CommentService(MongoDbContext context, UserService userService)
    {
        private readonly IMongoCollection<Comment> _comments = context.Comments;
        private readonly UserService _userService = userService;
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _comments.Find(_ => true).ToListAsync();
        }
        public async Task<Comment?> GetByIdAsync(string id)
        {
            return await _comments.Find(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Comment comment)
        {
            await _comments.InsertOneAsync(comment);
        }
        public async Task UpdateAsync(Comment comment)
        {
            await _comments.ReplaceOneAsync(c => c.Id == comment.Id, comment);
        }
        public async Task DeleteAsync(string id)
        {
            await _comments.DeleteOneAsync(c => c.Id == id);
        }
        public async Task<List<Comment>> GetCommentsByBlogIdAsync(string blogId)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.BlogId, blogId);
            var comments = await _comments.Find(filter).ToListAsync();

            var tasks = comments.Select(async c =>
            {
                c.Profile = await _userService.GetByIdAsync(c.UserId!);
                return c;
            });

            return (await Task.WhenAll(tasks)).ToList();
        }
        public async Task<List<Comment>> GetCommentsByUserIdAsync(string userId)
        {
            var filter = Builders<Comment>.Filter.Eq(c => c.UserId, userId);
            return await _comments.Find(filter).ToListAsync();
        }
    }
}
