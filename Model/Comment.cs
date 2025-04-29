using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("blogId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BlogId { get; set; }

        [BsonElement("chatId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ChatId { get; set; }

        [BsonElement("profile")]
        public User? Profile { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        [BsonElement("modifiedAt")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
