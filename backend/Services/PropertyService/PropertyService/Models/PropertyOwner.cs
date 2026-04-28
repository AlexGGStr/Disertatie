using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyService.Models;

public class PropertyOwner
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = string.Empty;
}