namespace PropertyService.DTOs;

public class SearchPropertiesDto
{
    public string PlaceId { get; set; } = string.Empty;
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public int NoOfPeople { get; set; }
}