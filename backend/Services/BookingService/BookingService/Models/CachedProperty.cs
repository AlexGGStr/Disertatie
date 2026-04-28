using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Models;

public class CachedProperty
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Capacity { get; set; }
    public decimal PricePerNight { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string PropertyName { get; set; }
    public string OwnerEmail { get; set; }
    
    public string Location { get; set; }
}