using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class UserService(MongoDbContext context)
    {
        private readonly IMongoCollection<User> _users = context.Users;
        public async Task<List<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }
        public async Task UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(p => p.Id == user.Id, user);
        }
        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(p => p.Id == id);
        }
    }
}
