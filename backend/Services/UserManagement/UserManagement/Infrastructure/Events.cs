namespace UserManagement.Infrastructure;

public interface IUserEvents;

public class UserRegisteredEvent : IUserEvents
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = "";
    public DateTime RegisteredAt { get; set; } = DateTime.Now;
    public string UserName { get; set; } = "";
}

public class UserDeletedEvent : IUserEvents
{
    public Guid UserId { get; set; }
    public DateTime DeletedAt { get; set; } = DateTime.Now;
}

public class PropertyCreatedEvent
{
    public Guid UserId { get; set; }
        
    public string UserName { get; set; }
        
    public string UserEmail { get; set; }
        
    public Guid PropertyId { get; set; }
        
    public string PropertyName { get; set; }
}