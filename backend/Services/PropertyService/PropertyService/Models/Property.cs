using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertyService.Models;

public class Property
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid PropertyId { get; set; } = Guid.NewGuid();

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("location")]
    public Location Location { get; set; } = null!;

    [BsonElement("pricePerNight")]
    public int PricePerNight { get; set; }

    [BsonElement("rooms")]
    public int Rooms { get; set; }

    [BsonElement("noOfPeople")]
    public int NoOfPeople { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("owner")]
    public PropertyOwner Owner { get; set; } = null!;

    [BsonElement("bookings")]
    public List<Booking> Bookings { get; set; } = new();
    
    [BsonElement("images")]
    public List<string> Images { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Location
{
    public string Address { get; set; }
    
    public string City { get; set; }
    
    public string LocationId { get; set; }
    
    public string Country { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}