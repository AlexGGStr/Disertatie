namespace PaymentService.Models;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Primary key

    public string PaymentIntentId { get; set; } = null!; // Stripe PaymentIntent ID (e.g. pi_abc123)
    
    public Guid BookingId { get; set; }       // Your internal booking ID

    public decimal Amount { get; set; }                   // Amount in decimal, e.g. 100.00 = $100.00

    public string Currency { get; set; } = "usd";        // Currency code (default usd)

    public Status Status { get; set; } = 0;      // Payment status (pending, succeeded, failed, canceled)

    public string? ClientSecret { get; set; }             // Stripe PaymentIntent client secret (optional)

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum Status
{
    Pending = 0,
    Paid = 1,
    Returned = 2
}