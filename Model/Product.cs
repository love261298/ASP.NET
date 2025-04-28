using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model;
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public string? Code { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("image")]
    public string? Image { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("category")]
    public string? Category { get; set; }

    [BsonElement("quantity")]
    public int? Quantity { get; set; }

    [BsonElement("inventoryStatus")]
    public string? InventoryStatus { get; set; }

    [BsonElement("rating")]
    public int? Rating { get; set; }
    [BsonElement("order")]
    public List<Order>? Order { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    [BsonElement("modifiedAt")]
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}


