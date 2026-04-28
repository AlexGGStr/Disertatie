using Microsoft.Extensions.Options;
using Shared;

namespace UserManagement.Infrastructure.Consumers;

public class PropertyConsumer : BaseKafkaConsumer<PropertyCreatedEvent>
{
    
    public PropertyConsumer(IOptions<KafkaSettings> kafkaOptions, 
        IHostApplicationLifetime applicationLifetime, 
        IServiceScopeFactory scopeFactory) 
        : base(kafkaOptions, kafkaOptions.Value.Topics.Property.Created, applicationLifetime, scopeFactory)
    {
    }

    protected async override Task HandleMessageAsync(IServiceProvider services, PropertyCreatedEvent message, string key, CancellationToken cancellationToken)
    {
        Console.WriteLine(key);
        Console.WriteLine(message.UserId);
        
        var authRepository = services.GetRequiredService<IAuthRepository.IAuthRepository>();

        Console.WriteLine(await authRepository.MakeUserAdmin(Guid.Parse(key)));
    }
}