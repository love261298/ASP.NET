
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProductId { get; set; }

        [BsonElement("amount")]
        public int? Amount { get; set; }

        [BsonElement("quantity")]
        public int? Quantity { get; set; }

        [BsonElement("customer")]
        public string? Customer { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        [BsonElement("modifiedAt")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
