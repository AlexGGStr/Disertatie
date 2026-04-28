using BookingService.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService.DTOs;

public class GetBookingsDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonRepresentation(BsonType.String)]
    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; }
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public string Status { get; set; }
    public int NoOfPeople { get; set; }
    public decimal TotalPrice { get; set; }
    public string Location { get; set; }
}