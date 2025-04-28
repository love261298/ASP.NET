using MongoDB.Driver;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class ProductService(MongoDbContext context)
    {
        private readonly IMongoCollection<Product> _product = context.Products;

        public async Task<List<Product>> GetAllAsync()
        {
            return await _product.Find(_ => true).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _product.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _product.InsertOneAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _product.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task DeleteAsync(string id)
        {
            await _product.DeleteOneAsync(p => p.Id == id);
        }
    }
}
