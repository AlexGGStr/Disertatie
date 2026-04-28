using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyService.Models;

public class Booking
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid BookingUserId { get; set; }

    public string BookingUserName { get; set; } = String.Empty;

    public int NumberOfPeople { get; set; }
    public decimal TotalPrice { get; set; }
}