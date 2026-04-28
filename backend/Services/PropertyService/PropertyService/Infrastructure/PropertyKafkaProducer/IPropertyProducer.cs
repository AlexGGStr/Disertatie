namespace PropertyService.Infrastructure.PropertyKafkaProducer;

public interface IPropertyProducer
{
    Task SendPropertyCreated(string key, PropertyEvents.PropertyCreatedEvent property);
}