using EPIC.Shared.RabbitMQ.Email.Dto;
using EPIC.Utils.ConfigModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.Shared.RabbitMQ.Email
{
    public class RabbitMQEmailReceiver : BackgroundService
    {
        private readonly ILogger _logger;
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly IOptions<SMTPConfiguration> _smtpConfiguration;

        public RabbitMQEmailReceiver(ILogger<RabbitMQEmailReceiver> logger,
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            IOptions<SMTPConfiguration> smtpConfiguration)
        {
            _logger = logger;
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueNames.EmailQueue;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _smtpConfiguration = smtpConfiguration;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _logger.LogInformation($"Initialize RabbitMq Listener successfully: HostName = {_hostname}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var input = JsonSerializer.Deserialize<SendEmailDto>(content);

                HandleMessage(input);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(SendEmailDto input)
        {
            var smtpClient = new SmtpClient(_smtpConfiguration.Value.Host, _smtpConfiguration.Value.Port);
            smtpClient.Credentials = new NetworkCredential(_smtpConfiguration.Value.Username, _smtpConfiguration.Value.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            try
            {
                MailMessage mail = new();
                mail.From = new MailAddress(_smtpConfiguration.Value.From);
                mail.To.Add(new MailAddress(input.To));
                mail.Subject = input.Subject;
                mail.Body = input.Body;
                mail.IsBodyHtml = true;

                smtpClient.Send(mail);
                _logger.LogInformation($"send mail from = {_smtpConfiguration.Value.From} to = {input.To} with subject = {input.Subject}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
