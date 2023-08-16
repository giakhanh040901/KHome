using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerHostedServices : IHostedService
    {
        private readonly ILogger<GarnerHostedServices> _logger;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GarnerHostedServices(ILogger<GarnerHostedServices> logger,
            IBackgroundJobClient backgroundJobs,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _backgroundJobs = backgroundJobs;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Garner Hosted Service is running.");
            //RecurringJob.AddOrUpdate("garner_update_msb_request_payment", () => RecurringUpdateStatusMsbRequestPayment(), Cron.Minutely);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Garner Hosted Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
