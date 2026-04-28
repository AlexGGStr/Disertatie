using PaymentService.Models;

namespace PaymentService.Infrastructure.PaymentRepository;

public interface IPaymentRepository
{
    Task<bool> CreatePayment(Guid bookingId, String intentSecret, String intentId, decimal amount);

    Task<bool> UpdatePayment(string paymentId, Status status);

    Task<Payment> GetPaymentByBookingId(Guid bookingId);

}