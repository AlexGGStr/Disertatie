namespace EmailService.Models.Events;

public class BookingCreatedEvent
{
    public Guid BookingId { get; set; }
    
    public string PropertyName { get; set; }
    
    public User User { get; set; }
    
    public string OwnerEmail { get; set; }

    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public int NoOfPeople { get; set; }

    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
}