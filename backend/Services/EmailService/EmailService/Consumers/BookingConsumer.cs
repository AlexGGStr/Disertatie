using EmailService.Models;
using EmailService.Models.Events;
using EmailService.Services.SendEmailService;
using Microsoft.Extensions.Options;
using Shared;

namespace EmailService.Consumers;

public class BookingConsumer : BaseKafkaConsumer<BookingCreatedEvent>
{
    public BookingConsumer(IOptions<KafkaSettings> kafkaOptions, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory) 
        : base(kafkaOptions, kafkaOptions.Value.Topics.Booking.Created, appLifetime, scopeFactory)
    {
    }

    protected override async Task HandleMessageAsync(IServiceProvider services, BookingCreatedEvent message, string key,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(key);

        var emailService = services.GetRequiredService<ISendEmailService>();

        await emailService.SendEmail(message.User.Email, EmailTemplate.BookingConfirmed, message);
        await emailService.SendEmail(message.OwnerEmail, EmailTemplate.BookingConfirmedForOwner, message);
    }
}