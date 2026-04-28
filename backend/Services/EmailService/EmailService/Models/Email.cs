using EmailService.Models.Events;

namespace EmailService.Models;

public enum EmailTemplate
{
    WelcomeUser,
    PropertyCreated,
    BookingConfirmed,
    BookingConfirmedForOwner
}

public class Email
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public Email(string to, EmailTemplate template, object data)
    {
        To = to;

        switch (template)
        {
            case EmailTemplate.WelcomeUser:
                Subject = "Welcome to Our App!";
                Body = BuildWelcomeEmail((UserCreatedEvent)data);
                break;

            case EmailTemplate.PropertyCreated:
                Subject = "Your Property is Live!";
                Body = BuildPropertyCreatedEmail((PropertyCreatedEvent)data);
                break;

            case EmailTemplate.BookingConfirmed:
                Subject = "Your Booking is Confirmed!";
                Body = BuildBookingEmail((BookingCreatedEvent)data);
                break;
            
            case EmailTemplate.BookingConfirmedForOwner:
                Subject = "New Booking for your property";
                Body = BuildBookingEmailForOwner((BookingCreatedEvent)data);
                break;

            default:
                Subject = "Notification";
                Body = "Hello! This is a generic email.";
                break;
        }
    }

    private string BuildWelcomeEmail(UserCreatedEvent user)
    {
        return $@"
                <h1>Hello {user.UserName},</h1>
                <p>Welcome to our travel platform! We’re excited to have you.</p>";
    }

    private string BuildPropertyCreatedEmail(PropertyCreatedEvent property)
    {
        return $@"
                <h1>Property Created: {property.PropertyName}</h1>
                <p>Location: {property.Location}</p>
                <p>Price Per Night: {property.PricePerNight}</p>
                <p>Your listing is now live!</p>";
    }

    private string BuildBookingEmail(BookingCreatedEvent booking)
    {
        return $@"
                <h1>Booking Confirmed!</h1>
                <p>Hello {booking.User.Name},</br>You booked {booking.PropertyName} from {booking.From:MMM dd} to {booking.To:MMM dd}.</p>
                <p>Total Paid Amount: {booking.TotalPrice}";
    }

    private string BuildBookingEmailForOwner(BookingCreatedEvent booking)
    {
        return $"<h1>New Booking for {booking.PropertyName}</h1>" +
               $"<p1>You have a new booking for {booking.PropertyName} from {booking.From: MMM dd} for {(booking.To.ToDateTime(TimeOnly.MinValue) - booking.From.ToDateTime(TimeOnly.MinValue)).Days}" +
               $"<p1>Check in date: {booking.From: dd MM yyyy}" +
               $"Check out date: {booking.To: dd MM yyyy}" +
               $"Total Payment: {booking.TotalPrice}";
    }
}