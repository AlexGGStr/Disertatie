using EmailService.Models;
using EmailService.Models.Events;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EmailService.Services.SendEmailService;

public class SendEmailService(IOptions<EmailSettings> options) : ISendEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;

    public async Task SendEmail(string to, EmailTemplate template, object data)
    {
        try
        {
            var email = new Email(to, template, data);
    
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.Username));
            message.To.Add(MailboxAddress.Parse(email.To));
            message.Subject = email.Subject;
            message.Body = new TextPart(TextFormat.Html) { Text = email.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}