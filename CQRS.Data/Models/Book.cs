using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Data.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();    
    public string Title { get; set; } = string.Empty;
    public int NumberAvailable { get; set; } = 0;
    public IList<ObjectId> CheckedOutTo { get; set; } = new List<ObjectId>();
}