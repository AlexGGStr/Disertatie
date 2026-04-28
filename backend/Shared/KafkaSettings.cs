namespace Shared;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public TopicGroups Topics { get; set; } = new();

    public class TopicGroups
    {
        public UserTopics User { get; set; } = new();
        public BookingTopics Booking { get; set; } = new();
        public PaymentTopics Payment { get; set; } = new();
        public NotificationTopics Notification { get; set; } = new();
        public PropertyTopics Property { get; set; } = new();
    }

    public class UserTopics
    {
        public string Registered { get; set; } = string.Empty;
        public string Deleted { get; set; } = string.Empty;
    }

    public class BookingTopics
    {
        public string Created { get; set; } = string.Empty;
        public string Cancelled { get; set; } = string.Empty;
    }

    public class PaymentTopics
    {
        public string Succeeded { get; set; } = string.Empty;
        public string Failed { get; set; } = string.Empty;
    }

    public class NotificationTopics
    {
        public string SendEmail { get; set; } = string.Empty;
    }

    public class PropertyTopics
    {
        public string Created { get; set; } = string.Empty;
        public string Updated { get; set; } = string.Empty;
    }
}