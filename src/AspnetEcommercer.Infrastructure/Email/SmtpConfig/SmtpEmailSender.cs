using AspnetEcommerce.Application.Contracts.Email;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace AspnetEcommerce.Infrastructure.Email.SmtpConfig
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions smtpOptions;

        public SmtpEmailSender(IOptions<SmtpOptions> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            smtpOptions = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body,
            bool isBodyHtml = true,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrWhiteSpace(body)) throw new ArgumentNullException(nameof(body));

            if (string.IsNullOrWhiteSpace(smtpOptions.Host) ||
                string.IsNullOrWhiteSpace(smtpOptions.UserName) ||
                string.IsNullOrWhiteSpace(smtpOptions.Password))
            {
                throw new InvalidOperationException("SMTP settings are not properly configured.");
            }

            using var message = new MailMessage
            {
                From = new MailAddress(
                    string.IsNullOrWhiteSpace(smtpOptions.From) ? smtpOptions.UserName : smtpOptions.From,
                    smtpOptions.FromDisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            message.To.Add(new MailAddress(to));

            using var client = new SmtpClient(smtpOptions.Host, smtpOptions.Port)
            {
                EnableSsl = smtpOptions.EnableSsl,
                Credentials = new System.Net.NetworkCredential(smtpOptions.UserName, smtpOptions.Password)
            };

            await Task.Run(() => client.Send(message), ct);
        }
    }
}
