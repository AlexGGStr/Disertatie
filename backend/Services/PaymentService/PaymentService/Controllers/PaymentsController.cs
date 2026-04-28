using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;
using PaymentService.Services;
using Stripe;

namespace PaymentService.Controllers;

[Route("api/[controller]")]
public class PaymentsController (IConfiguration _config, IPaymentService paymentService) : Controller
{
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreateIntent([FromBody] PaymentRequestDto request)
    {
        
        return Ok(new { clientSecret = await paymentService.CreatePaymentIntent(request)});
    }
    
    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeSettings = _config.GetSection("Stripe").Get<StripeSettings>();

        try
        {
            Event stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                stripeSettings.WebhookSecret
            );

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;
                var bookingId = intent.Metadata["bookingId"];

                // ✅ Mark booking as paid
                Console.WriteLine($"✅ Payment succeeded for booking {bookingId}");
                await paymentService.MarkPaymentSuccessful(intent.Id);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            Console.WriteLine($"❌ Stripe error: {e.Message}");
            return BadRequest();
        }
    }
}