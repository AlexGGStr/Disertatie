namespace PropertyService.Infrastructure.PropertyKafkaProducer;

public class BookingEvents
{
    public Guid BookingId { get; set; }
    
    public UserEvent User { get; set; }
    
    public DateOnly From { get; set; }
    
    public DateOnly To { get; set; }
    
    public int NoOfPeople { get; set; }

    public decimal PricePerNight { get; set; }
    
    public decimal TotalPrice { get; set; }
}

public class UserEvent
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
}