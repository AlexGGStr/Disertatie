using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Infrastructure.PaymentRepository;

public class PaymentRepository(PaymentsDbContext context, IPaymentProducer producer) : IPaymentRepository
{
    public async Task<bool> CreatePayment(Guid bookingId, string intentSecret, string intentId, decimal amount)
    {
        try
        {
            context.Payments.Add(new Payment
            {
                Amount = amount,
                BookingId = bookingId,
                ClientSecret = intentSecret,
                PaymentIntentId = intentId
            });

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> UpdatePayment(string paymentId, Status status)
    {
        try
        {
            Payment payment = await context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentId);

            if (payment != null)
            {
                payment.Status = status;
                payment.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();

                producer.SendPaymendSuccededAsync(payment.BookingId.ToString(), true);
                return true;
            }
            
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<Payment> GetPaymentByBookingId(Guid bookingId)
    {
        return await context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }
}