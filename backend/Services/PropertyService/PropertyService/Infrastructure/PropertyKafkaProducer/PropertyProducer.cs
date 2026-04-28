using Microsoft.Extensions.Options;
using Shared;

namespace PropertyService.Infrastructure.PropertyKafkaProducer;

public class PropertyProducer(IOptions<KafkaSettings> kafkaOptions) : BaseKafkaProducer(kafkaOptions), IPropertyProducer
{
    private readonly KafkaSettings _settings = kafkaOptions.Value;

    public async Task SendPropertyCreated(string key ,PropertyEvents.PropertyCreatedEvent property)
    {
        Console.WriteLine("Producing...");
        await this.ProduceAsync(_settings.Topics.Property.Created, key, property);
    }
}