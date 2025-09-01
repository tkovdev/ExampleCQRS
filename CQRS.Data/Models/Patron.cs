using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Data.Models;

public class Patron
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public string Name { get; set; } = string.Empty;
    public IList<CheckedBooks> CheckedBooks { get; set; } = new List<CheckedBooks>();
}

public class CheckedBooks
{
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset CheckoutDate { get; set; }
    public DateTimeOffset? ReturnDate { get; set; }
}