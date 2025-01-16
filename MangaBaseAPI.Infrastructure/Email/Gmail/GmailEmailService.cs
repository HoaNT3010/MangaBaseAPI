using MangaBaseAPI.CrossCuttingConcerns.Email.Gmail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace MangaBaseAPI.Infrastructure.Email.Gmail
{
    public class GmailEmailService : IGmailEmailService
    {
        readonly GmailEmailOptions _gmailOptions;
        readonly ILogger<GmailEmailService> _logger;

        public GmailEmailService(
            IOptions<GmailEmailOptions> gmailOptions,
            ILogger<GmailEmailService> logger)
        {
            _gmailOptions = gmailOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(
            string recipient,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            var mailMessage = GetMailMessage(subject, body);
            mailMessage.To.Add(recipient);
            try
            {
                await GetSmtpClient().SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when trying to send email to recipient: {Message}", ex.Message);
            }
        }

        public async Task SendEmailToMultipleAsync(
            IEnumerable<string> recipients,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            if (recipients == null || !recipients.Any())
            {
                throw new ArgumentException("Email must have at least one recipient", nameof(recipients));
            }
            var mailMessage = GetMailMessage(subject, body);
            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(recipient);
            }
            try
            {
                await GetSmtpClient().SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when trying to send email to multiple recipients: {Message}", ex.Message);
            }
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient(_gmailOptions.Host)
            {
                Port = _gmailOptions.Port,
                Credentials = new NetworkCredential(_gmailOptions.UserName, _gmailOptions.Password),
                EnableSsl = true
            };
        }

        private MailMessage GetMailMessage(string subject, string body, bool isBodyHtml = true)
        {
            return new MailMessage
            {
                From = new MailAddress(_gmailOptions.FromEmail, _gmailOptions.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };
        }

        public async Task SendEmailAsync(
            IEnumerable<string> recipients,
            string subject,
            string body,
            IEnumerable<string>? ccRecipients = null,
            IEnumerable<string>? bccRecipients = null,
            CancellationToken cancellationToken = default)
        {
            if (recipients == null || !recipients.Any())
            {
                throw new ArgumentException("Email must have at least one recipient", nameof(recipients));
            }

            var mailMessage = GetMailMessage(subject, body);
            foreach (var toEmail in recipients)
            {
                mailMessage.To.Add(toEmail);
            }

            if (ccRecipients != null && ccRecipients.Any())
            {
                foreach (var ccRecipient in ccRecipients)
                {
                    mailMessage.CC.Add(ccRecipient);
                }
            }
            if (bccRecipients != null && bccRecipients.Any())
            {
                foreach (var bccRecipient in bccRecipients)
                {
                    mailMessage.Bcc.Add(bccRecipient);
                }
            }

            try
            {
                await GetSmtpClient().SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when trying to send email to recipients (CC & BCC included): {Message}", ex.Message);
            }
        }
    }
}
