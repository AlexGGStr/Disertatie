namespace EmailService.Models.Events;

public class PropertyCreatedEvent
{
    public string UserName { get; set; }
        
    public string UserEmail { get; set; }
        
    public string PropertyName { get; set; }
        
    public int Capacity { get; set; }
        
    public int PricePerNight { get; set; }
    
    public string Location { get; set; }
}