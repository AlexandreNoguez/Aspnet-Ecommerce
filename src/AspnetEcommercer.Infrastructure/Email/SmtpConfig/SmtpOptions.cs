namespace AspnetEcommerce.Infrastructure.Email.SmtpConfig
{
    public class SmtpOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;
        public string FromDisplayName { get; set; } = string.Empty;
    }
}
