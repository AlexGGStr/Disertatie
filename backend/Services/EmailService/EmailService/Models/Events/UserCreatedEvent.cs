namespace EmailService.Models.Events;

public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = "";
    public string UserName { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.Now;
}