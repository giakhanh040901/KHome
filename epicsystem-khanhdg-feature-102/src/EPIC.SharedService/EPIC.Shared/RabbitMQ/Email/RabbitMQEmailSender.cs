using EPIC.Shared.RabbitMQ.Email.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EPIC.Shared.RabbitMQ.Email
{
    public class RabbitMQEmailSender
    {
        private readonly ILogger _logger;
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public RabbitMQEmailSender(ILogger<RabbitMQEmailSender> logger,
            IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _logger = logger;
            _queueName = rabbitMqOptions.Value.QueueNames.EmailQueue;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            CreateConnection();
        }

        public void SendMail(SendEmailDto input)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonSerializer.Serialize(input);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };
            _connection = factory.CreateConnection();
            _logger.LogInformation($"Connected rabbitmq: HostName = {_hostname}");
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }
    }
}
