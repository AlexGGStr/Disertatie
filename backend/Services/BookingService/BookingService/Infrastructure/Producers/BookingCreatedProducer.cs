using Microsoft.Extensions.Options;
using Shared;

namespace BookingService.Infrastructure.Producers;

public class BookingCreatedProducer : BaseKafkaProducer, IBookingCreatedProducer
{
    private readonly KafkaSettings _settings;

    public BookingCreatedProducer(IOptions<KafkaSettings> kafkaOptions) : base(kafkaOptions)
    {
        _settings = kafkaOptions.Value;
    }

    public async Task SendBooking(string key, BookingCreatedEvent message)
    {
        await this.ProduceAsync(_settings.Topics.Booking.Created, key, message);
    }
}