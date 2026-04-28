using Microsoft.Extensions.Options;
using Shared;

namespace PaymentService.Infrastructure;

public interface IPaymentProducer
{
    Task SendPaymendSuccededAsync(string key, bool isSuccessful);
}

public class PaymentProducer(IOptions<KafkaSettings> kafkaOptions) : BaseKafkaProducer(kafkaOptions), IPaymentProducer
{
    private readonly KafkaSettings _settings = kafkaOptions.Value;

    public async Task SendPaymendSuccededAsync(string key, bool isSuccessfull)
    {
        await this.ProduceAsync(_settings.Topics.Payment.Succeeded, key, isSuccessfull);
        Console.WriteLine("Produced for " + key);
    }
}