using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class ChatService(MongoDbContext context)
    {
        private readonly IMongoCollection<Chat> _chats = context.Chats;
        public async Task<List<Chat>> GetAllAsync()
        {
            return await _chats.Find(_ => true).ToListAsync();
        }
        public async Task<Chat?> GetByIdAsync(string id)
        {
            return await _chats.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Chat chat)
        {
            await _chats.InsertOneAsync(chat);
        }
        public async Task UpdateAsync(Chat chat)
        {
            await _chats.ReplaceOneAsync(p => p.Id == chat.Id, chat);
        }
        public async Task DeleteAsync(string id)
        {
            await _chats.DeleteOneAsync(p => p.Id == id);
        }
        public async Task<List<Chat>> GetChatsByUserIdAsync(ObjectId userId)
        {
            var filter = Builders<Chat>.Filter.AnyEq(c => c.UserIds, userId);
            return await _chats.Find(filter).ToListAsync();
        }
        public async Task<Chat?> FindChatBetweenUsersAsync(ObjectId user1, ObjectId user2)
        {
            var filter = Builders<Chat>.Filter.All(c => c.UserIds, new[] { user1, user2 }) &
                         Builders<Chat>.Filter.Size(c => c.UserIds, 2);
            return await _chats.Find(filter).FirstOrDefaultAsync();
        }
    }
}
