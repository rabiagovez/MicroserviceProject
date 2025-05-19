using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NotificationService.Application.Configuration;
using NotificationService.Application.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Options;

namespace NotificationService.Infrastructure.Services
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly NotificationSettings _settings;
        public EmailNotificationSender(IOptions<NotificationSettings> options)
        {
            _settings = options.Value;
            TwilioClient.Init(_settings.TwilioSid, _settings.TwilioToken);
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_settings.FromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SmtpEmail, _settings.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendSmsAsync(string to, string message)
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber(to),
                from: new PhoneNumber(_settings.TwilioFrom),
                body: message
            );

            Console.WriteLine($"📲 SMS gönderildi → {to} → {message}");
        }
    }
}
