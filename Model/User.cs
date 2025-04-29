using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; } = DateTime.Now;

        [BsonElement("modifiedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        [BsonElement("role")]
        public List<string> Role { get; set; } = new List<string> { "user" };
    }
}
