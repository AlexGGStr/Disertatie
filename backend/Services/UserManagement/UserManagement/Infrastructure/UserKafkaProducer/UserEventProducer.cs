using Microsoft.Extensions.Options;
using Shared;

namespace UserManagement.Infrastructure.UserKafkaProducer;

public interface IUserEventProducer
{
    Task SendUserRegisteredAsync(UserRegisteredEvent userEvent);
    Task SendUserDeletedAsync(UserDeletedEvent userEvent);
}

public class UserEventProducer(IOptions<KafkaSettings> kafkaOptions) : BaseKafkaProducer(kafkaOptions),
    IUserEventProducer
{
    private readonly KafkaSettings _settings = kafkaOptions.Value;

    public Task SendUserRegisteredAsync(UserRegisteredEvent userEvent)
        => ProduceAsync(_settings.Topics.User.Registered, userEvent.UserId.ToString(), userEvent);

    public Task SendUserDeletedAsync(UserDeletedEvent userEvent)
        => ProduceAsync(_settings.Topics.User.Deleted, userEvent.UserId.ToString(), userEvent);
}