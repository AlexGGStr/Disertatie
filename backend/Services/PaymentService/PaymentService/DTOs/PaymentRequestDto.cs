namespace PaymentService.DTOs;

public class PaymentRequestDto
{
    public decimal Amount { get; set; }
    
    public Guid BookingId { get; set; }
}