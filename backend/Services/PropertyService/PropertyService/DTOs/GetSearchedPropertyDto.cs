using PropertyService.Models;

namespace PropertyService.DTOs;

public class GetSearchedPropertyDto
{
    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; }
    public Location Location { get; set; }
    public int Capacity { get; set; }
    public int PricePerNight { get; set; }
    public int NoOfRooms { get; set; }
    
    public List<string> Images { get; set; }
}