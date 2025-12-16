using AspnetEcommerce.Application.Contracts.Email;
using AspnetEcommerce.Application.Contracts.Email.Jobs;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AspnetEcommerce.Infrastructure.Messaging;

public class WelcomeEmailWorker : BackgroundService
{
    private readonly RabbitMqOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WelcomeEmailWorker> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public WelcomeEmailWorker(
        IOptions<RabbitMqOptions> options,
        IServiceScopeFactory scopeFactory,
        ILogger<WelcomeEmailWorker> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _options.WelcomeEmailQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var job = JsonSerializer.Deserialize<WelcomeEmailJob>(json);

                if (job is null)
                {
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    return;
                }

                // 🔥 Cria um escopo por mensagem
                using var scope = _scopeFactory.CreateScope();

                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var templateProvider = scope.ServiceProvider.GetRequiredService<ICustomerEmailTemplateProvider>();

                var template = templateProvider.BuildWelcomeEmail(
                    job.CustomerName,
                    job.ActivationLink);

                var subject = template.Subject;
                var bodyHtml = template.BodyHtml;

                await emailSender.SendEmailAsync(
                    job.CustomerEmail,
                    subject,
                    bodyHtml,
                    isBodyHtml: true,
                    cancellationToken: stoppingToken);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing welcome email message");

                _channel?.BasicNack(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false,
                    requeue: false);
            }
        };

        _channel.BasicConsume(
            queue: _options.WelcomeEmailQueueName,
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        base.Dispose();
    }
}
