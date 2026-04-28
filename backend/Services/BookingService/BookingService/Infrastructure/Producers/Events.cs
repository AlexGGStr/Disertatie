using BookingService.Models;

namespace BookingService.Infrastructure.Producers;

public class BookingCreatedEvent
{
    public Guid BookingId { get; set; }
    
    public string PropertyName { get; set; }
    
    public User User { get; set; }

    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    public int NoOfPeople { get; set; }

    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }
    public string OwnerEmail { get; set; }
}