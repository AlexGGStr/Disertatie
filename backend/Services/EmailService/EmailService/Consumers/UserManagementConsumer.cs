using EmailService.Models;
using EmailService.Models.Events;
using EmailService.Services.SendEmailService;
using Microsoft.Extensions.Options;
using Shared;

namespace EmailService.Consumers;

public class UserManagementConsumer : BaseKafkaConsumer<UserCreatedEvent>
{
    public UserManagementConsumer(IOptions<KafkaSettings> kafkaOptions, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory) 
        : base(kafkaOptions, kafkaOptions.Value.Topics.User.Registered, appLifetime, scopeFactory)
    {
    }

    protected override async Task HandleMessageAsync(IServiceProvider services, UserCreatedEvent message, string key,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(key);
        Console.WriteLine(message.Email);

        var service = services.GetRequiredService<ISendEmailService>();

        await service.SendEmail(message.Email, EmailTemplate.WelcomeUser, message);
    }
}