using PaymentService.DTOs;

namespace PaymentService.Services;

public interface IPaymentService
{
    Task<string> CreatePaymentIntent(PaymentRequestDto request);
    Task<string> GetPaymentSecret();
    Task MarkPaymentSuccessful(string intentId);
}