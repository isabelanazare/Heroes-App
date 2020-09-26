using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task Send(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(from ?? _appSettings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
