using BookingService.Infrastructure.BookingRepository;
using Microsoft.Extensions.Options;
using Shared;

namespace BookingService.Infrastructure.Consumers;

public class PaymentConsumer : BaseKafkaConsumer<bool>
{
    public PaymentConsumer(IOptions<KafkaSettings> kafkaOptions, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory) 
        : base(kafkaOptions, kafkaOptions.Value.Topics.Payment.Succeeded, appLifetime, scopeFactory)
    {
    }

    protected override async Task HandleMessageAsync(IServiceProvider services, bool message, string key, CancellationToken cancellationToken)
    {
        Console.WriteLine(key);
        Console.WriteLine(message);

        if (message)
        {
            var bookingRepository = services.GetRequiredService<IBoookingRepository>();

            Console.WriteLine(await bookingRepository.ConfirmBooking(Guid.Parse(key)));
        }
    }
}