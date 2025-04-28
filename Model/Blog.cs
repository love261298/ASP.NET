using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.Model
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("coverImage")]
        public string? CoverImage { get; set; }

        [BsonElement("profile")]
        public string? Profile { get; set; }

        [BsonElement("Title")]
        public string? Title { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("comment")]
        public int? Comment { get; set; }

        [BsonElement("share")]
        public int? Share { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        [BsonElement("modifiedAt")]
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
