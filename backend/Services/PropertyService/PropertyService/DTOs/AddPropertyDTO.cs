using MongoDB.Bson.Serialization.Attributes;
using PropertyService.Models;

namespace PropertyService.DTOs;

public class AddPropertyDTO
{
    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("location")]
    public string LocationId { get; set; } = null!;
    
    public string LocationAdress { get; set; }

    [BsonElement("pricePerNight")]
    public int PricePerNight { get; set; }

    [BsonElement("rooms")]
    public int Rooms { get; set; }

    [BsonElement("noOfPeople")]
    public int NoOfPeople { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }
    
    public List<IFormFile> Images { get; set; }
}