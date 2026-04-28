using EmailService.Models;
using EmailService.Models.Events;
using EmailService.Services.SendEmailService;
using Microsoft.Extensions.Options;
using Shared;

namespace EmailService.Consumers;

public class PropertyConsumer : BaseKafkaConsumer<PropertyCreatedEvent>
{
    public PropertyConsumer(IOptions<KafkaSettings> kafkaOptions, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory) 
        : base(kafkaOptions, kafkaOptions.Value.Topics.Property.Created, appLifetime, scopeFactory)
    {
    }

    protected override async Task HandleMessageAsync(IServiceProvider services, PropertyCreatedEvent message, string key,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(key);

        var emailService = services.GetRequiredService<ISendEmailService>();
        await emailService.SendEmail(message.UserEmail, EmailTemplate.PropertyCreated, message);
    }
}