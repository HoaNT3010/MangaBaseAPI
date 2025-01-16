namespace MangaBaseAPI.Infrastructure.Email.Gmail
{
    public class GmailEmailOptions
    {
        public string FromEmail { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
