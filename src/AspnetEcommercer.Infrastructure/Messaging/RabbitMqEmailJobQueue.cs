using AspnetEcommerce.Application.Contracts.Email.Jobs;
using AspnetEcommerce.Application.Customer.Contracts.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AspnetEcommerce.Infrastructure.Messaging;

public sealed class RabbitMqEmailJobQueue : IEmailJobQueue, IDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqEmailJobQueue> _logger;

    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _syncRoot = new();

    public RabbitMqEmailJobQueue(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqEmailJobQueue> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private bool EnsureConnection()
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
        {
            return true;
        }

        lock (_syncRoot)
        {
            if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
            {
                return true;
            }

            try
            {
                _logger.LogInformation(
                    "Connecting to RabbitMQ at {Host}:{Port}...",
                    _options.HostName,
                    _options.Port);

                var factory = new ConnectionFactory
                {
                    HostName = _options.HostName,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password
                };

                _connection?.Dispose();
                _channel?.Dispose();

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(
                    exchange: _options.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true);

                _channel.QueueDeclare(
                    queue: _options.WelcomeEmailQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _channel.QueueBind(
                    queue: _options.WelcomeEmailQueueName,
                    exchange: _options.ExchangeName,
                    routingKey: _options.WelcomeEmailRoutingKey);

                _logger.LogInformation(
                    "RabbitMQ connection established. Exchange={Exchange}, Queue={Queue}, RoutingKey={RoutingKey}",
                    _options.ExchangeName,
                    _options.WelcomeEmailQueueName,
                    _options.WelcomeEmailRoutingKey);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to connect to RabbitMQ at {Host}:{Port}. Welcome email job will NOT be enqueued.",
                    _options.HostName,
                    _options.Port);

                return false;
            }
        }
    }

    public Task EnqueueWelcomeEmailAsync(WelcomeEmailJob job, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(job);

        if (ct.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        try
        {
            if (!EnsureConnection())
            {
                _logger.LogWarning(
                    "Skipping enqueue of welcome email job for CustomerId={CustomerId} because RabbitMQ connection is unavailable.",
                    job.CustomerId);

                return Task.CompletedTask;
            }

            var payload = JsonSerializer.Serialize(job);
            var body = Encoding.UTF8.GetBytes(payload);

            var props = _channel!.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2; // persistent

            _channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: _options.WelcomeEmailRoutingKey,
                basicProperties: props,
                body: body);

            _logger.LogInformation(
                "Enqueued welcome email job for CustomerId={CustomerId} on Exchange={Exchange}, RoutingKey={RoutingKey}.",
                job.CustomerId,
                _options.ExchangeName,
                _options.WelcomeEmailRoutingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to enqueue welcome email job for CustomerId={CustomerId}. The request will NOT be retried here.",
                job.CustomerId);
            // Do not rethrow: we don't want to break the customer creation flow.
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error while disposing RabbitMQ resources.");
        }
    }
}
