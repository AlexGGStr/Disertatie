namespace BookingService.Infrastructure.Consumers;

public class PropertyCreatedEvent
{
    public Guid PropertyId { get; set; }
        
    public int PricePerNight { get; set; }

    public string PropertyName { get; set; }
    public int Capacity { get; set; }
    public string UserEmail { get; set; }
    
    public string Location { get; set; }
}