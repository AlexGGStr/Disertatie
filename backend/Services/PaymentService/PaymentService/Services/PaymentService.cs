using PaymentService.DTOs;
using PaymentService.Infrastructure.PaymentRepository;
using PaymentService.Models;
using Stripe;

namespace PaymentService.Services;

public class PaymentService(IPaymentRepository paymentRepository) : IPaymentService
{
    public async Task<string> CreatePaymentIntent(PaymentRequestDto request)
    {
        var payment = await paymentRepository.GetPaymentByBookingId(request.BookingId);
        if (payment != null) return payment.ClientSecret;
        
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(request.Amount * 100), // cents
            Currency = "usd",
            Metadata = new Dictionary<string, string>
            {
                { "bookingId", request.BookingId.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        await paymentRepository.CreatePayment(request.BookingId, intent.ClientSecret, intent.Id, request.Amount);

        return intent.ClientSecret;
    }

    public Task<string> GetPaymentSecret()
    {
        throw new NotImplementedException();
    }

    public async Task MarkPaymentSuccessful(string intentId)
    {
        await paymentRepository.UpdatePayment(intentId, Status.Paid);
    }
}