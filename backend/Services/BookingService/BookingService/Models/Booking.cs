using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.Models;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonRepresentation(BsonType.String)]
    public Guid PropertyId { get; set; }

    public PropertySnapshot PropertySnapshot { get; set; }
    
    public User User;

    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.PaymentPending;
    public int NoOfPeople { get; set; }
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum BookingStatus
{
    PaymentPending = 0,
    Confirmed = 1,
    Cancelled = 2
}

public class PropertySnapshot
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string OwnerEmail { get; set; }
    public decimal PricePerNight { get; set; }
    
    public string Location { get; set; }
}