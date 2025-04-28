using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class OrderService(MongoDbContext context)
    {
        private readonly IMongoCollection<Order> _order = context.Orders;
        public async Task<List<Order>> GetAllAsync()
        {
            return await _order.Find(_ => true).ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddAsync(Order order)
        {
            await _order.InsertOneAsync(order);
        }
        public async Task UpdateAsync(Order order)
        {
            await _order.ReplaceOneAsync(o => o.Id == order.Id, order);
        }
        public async Task DeleteAsync(string id)
        {
            await _order.DeleteOneAsync(o => o.Id == id);
        }
        public async Task<List<Order>> GetOrdersByProductIdAsync(string productId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.ProductId, productId);
            return await _order.Find(filter).ToListAsync();
        }
    }
}
