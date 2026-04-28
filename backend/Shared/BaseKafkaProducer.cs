using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Shared;

public abstract class BaseKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    protected BaseKafkaProducer(IOptions<KafkaSettings> kafkaOptions)
    {
        var settings = kafkaOptions.Value;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            SaslUsername = settings.Username,
            SaslPassword = settings.Password,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    protected async Task ProduceAsync(string topic, string key, object message)
    {
        var json = JsonSerializer.Serialize(message);

        var kafkaMessage = new Message<string, string>
        {
            Key = key,
            Value = json
        };
        Console.WriteLine($"Key: {kafkaMessage.Key}, Value: {kafkaMessage.Value}");
        await _producer.ProduceAsync(topic, kafkaMessage);
    }
}