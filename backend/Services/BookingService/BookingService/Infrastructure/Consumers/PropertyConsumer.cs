using BookingService.Infrastructure.PropertyCacheRepository;
using BookingService.Models;
using Microsoft.Extensions.Options;
using Shared;

namespace BookingService.Infrastructure.Consumers;

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
        Console.WriteLine(message.PropertyName);
        
        var propertyCacheRepository = services.GetRequiredService<IPropertyCacheRepository>();

        Console.WriteLine(await propertyCacheRepository.AddPropertyToCache(new CachedProperty
        {
            Capacity = message.Capacity,
            Id = message.PropertyId,
            PropertyName = message.PropertyName,
            PricePerNight = message.PricePerNight,
            UpdatedAt = DateTime.Now,
            OwnerEmail = message.UserEmail,
            Location = message.Location
        }));
    }
}