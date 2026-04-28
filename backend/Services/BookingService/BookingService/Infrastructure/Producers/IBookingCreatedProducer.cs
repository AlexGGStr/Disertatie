namespace BookingService.Infrastructure.Producers;

public interface IBookingCreatedProducer
{
    Task SendBooking(string key, BookingCreatedEvent message);
}