using Microsoft.Extensions.Options;
using PropertyService.Infrastructure.PropertyKafkaProducer;
using PropertyService.Models;
using Shared;

namespace PropertyService.Infrastructure.Consumers;

public class BookingConsumer : BaseKafkaConsumer<BookingEvents>
{
    public BookingConsumer(IOptions<KafkaSettings> kafkaOptions, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory) : 
        base(kafkaOptions, kafkaOptions.Value.Topics.Booking.Created, appLifetime, scopeFactory)
    {
    }

    protected override async Task HandleMessageAsync(IServiceProvider services, BookingEvents message, string key,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(message.User.Name);
        Console.WriteLine(message.User.Email);

        var propertyRepository = services.GetRequiredService<IPropertyRepository>();

        await propertyRepository.AddBookingToProperty(Guid.Parse(key), new Booking
        {
            BookingUserId = Guid.Parse(message.User.Id),
            BookingUserName = message.User.Name,
            StartDate = message.From,
            EndDate = message.To,
            NumberOfPeople = message.NoOfPeople,
            TotalPrice = message.TotalPrice
        });
    }
}