using EmailService.Models;
using EmailService.Models.Events;

namespace EmailService.Services.SendEmailService;

public interface ISendEmailService
{
    Task SendEmail(string to, EmailTemplate template, object data);
}