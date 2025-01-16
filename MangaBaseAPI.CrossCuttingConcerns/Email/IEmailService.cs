namespace MangaBaseAPI.CrossCuttingConcerns.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string body, CancellationToken cancellationToken = default);
        Task SendEmailToMultipleAsync(IEnumerable<string> recipients, string subject, string body, CancellationToken cancellationToken = default);
        Task SendEmailAsync(
            IEnumerable<string> recipients,
            string subject,
            string body,
            IEnumerable<string>? ccRecipients = default,
            IEnumerable<string>? bccRecipients = default,
            CancellationToken cancellationToken = default);
    }
}
