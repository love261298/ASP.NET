using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("comment")]
        public List<Comment>? Comment { get; set; }

        [BsonElement("userIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId>? UserIds { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        [BsonElement("modifiedAt")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
