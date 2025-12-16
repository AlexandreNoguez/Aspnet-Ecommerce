namespace AspnetEcommerce.Infrastructure.Messaging
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string ExchangeName { get; set; } = "emails";
        public string WelcomeEmailRoutingKey { get; set; } = "emails.welcome";
        public string WelcomeEmailQueueName { get; set; } = "emails.welcome";
    }
}
