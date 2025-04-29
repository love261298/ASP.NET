using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class BlogService(MongoDbContext context)
    {
        private readonly IMongoCollection<Blog> _blogs = context.Blogs;
        public async Task<List<Blog>> GetAllAsync()
        {
            return await _blogs.Find(_ => true).ToListAsync();
        }
        public async Task<Blog?> GetByIdAsync(string id)
        {
            return await _blogs.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Blog blog)
        {
            await _blogs.InsertOneAsync(blog);
        }
        public async Task UpdateAsync(Blog blog)
        {
            await _blogs.ReplaceOneAsync(p => p.Id == blog.Id, blog);
        }
        public async Task DeleteAsync(string id)
        {
            await _blogs.DeleteOneAsync(p => p.Id == id);
        }
    }
}
