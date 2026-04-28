using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Shared;

public abstract class BaseKafkaConsumer<TEvent> : BackgroundService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly string _topic;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IServiceScopeFactory _scopeFactory;
    private static readonly TaskCompletionSource _startupCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);

    protected BaseKafkaConsumer(IOptions<KafkaSettings> kafkaOptions, string topic, IHostApplicationLifetime appLifetime, IServiceScopeFactory scopeFactory)
    {
        _kafkaSettings = kafkaOptions.Value;
        _topic = topic;
        _appLifetime = appLifetime;
        _scopeFactory = scopeFactory;
    }

    protected abstract Task HandleMessageAsync(IServiceProvider services, TEvent message, string key, CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait until the application is fully started
        _appLifetime.ApplicationStarted.Register(() => _startupCompletion.TrySetResult());

        await _startupCompletion.Task;
        
        Console.WriteLine($"Starting Consumer for topic {_topic}");
        
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            SaslUsername = _kafkaSettings.Username,
            SaslPassword = _kafkaSettings.Password,
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_topic);
        
        Console.WriteLine($"Subscribed {this.GetType()}");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                
                try
                {
                    var message = JsonSerializer.Deserialize<TEvent>(result.Message.Value);
                    
                    using var scope = _scopeFactory.CreateScope();
                    var services = scope.ServiceProvider;
                    
                    if (message != null)
                    {
                        await HandleMessageAsync(services, message, result.Message.Key, stoppingToken);
                    }
                    else
                    {
                        Console.WriteLine($"Received null or unparseable message on topic {_topic}", _topic);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON deserialization failed for topic {_topic} : {ex.ToString()}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"Consumer for topic {_topic} cancelled");
        }
        finally
        {
            consumer.Close();
        }
    }
}