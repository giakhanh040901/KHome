using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Implements
{
    public class PaymentHostedServices : IHostedService
    {
        private readonly ILogger<PaymentHostedServices> _logger;
        private readonly IBackgroundJobClient _backgroundJobs;

        public PaymentHostedServices(ILogger<PaymentHostedServices> logger,
            IBackgroundJobClient backgroundJobs)
        {
            _logger = logger;
            _backgroundJobs = backgroundJobs;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Garner Hosted Service is running.");
            RecurringJob.AddOrUpdate("Msb update status request payment", () => RecurringUpdateStatusMsbRequestPayment(), Cron.Minutely);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Garner Hosted Service is stopping.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Liên tục cập nhật trạng thái yêu cầu chi hộ
        /// </summary>
        [Queue(HangfireQueues.Payment)]
        [HangfireLogEverything]
        public void RecurringUpdateStatusMsbRequestPayment()
        {
        }
    }
}
