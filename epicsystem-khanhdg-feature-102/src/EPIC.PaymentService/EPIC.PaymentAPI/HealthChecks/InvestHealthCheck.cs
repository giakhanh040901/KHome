using EPIC.DataAccess.Base;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityDomain.Interfaces;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.PaymentDomain.Implements;
using EPIC.PaymentDomain.Interfaces;
using EPIC.Pvcb.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateSharedDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.PaymentAPI.HealthChecks
{
    public class PayemntHealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;

        public PayemntHealthCheck(ILogger<PayemntHealthCheck> logger, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                CheckResolveService();
            }
            catch (Exception ex)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, ex.Message));
            }
            return Task.FromResult(HealthCheckResult.Healthy("A healthy result."));
        }

        private void CheckResolveService()
        {
            _logger.LogInformation($"{nameof(CheckResolveService)}");
            var services = _httpContext.HttpContext.RequestServices;

            var dbContext = services.GetRequiredService<EpicSchemaDbContext>();
            if (!dbContext.Database.CanConnect())
            {
                throw new InvalidOperationException("cannot connect database");
            }

            services.GetRequiredService<PvcbCollectMoneyServices>();
            services.GetRequiredService<IPvcbPaymentServices>();
            services.GetRequiredService<IMsbPaymentServices>();
            services.GetRequiredService<IMsbRequestDetailServices>();
            services.GetRequiredService<IMsbRequestPaymentServices>();
            services.GetRequiredService<IMsbRequestDetailServices>();
            services.GetRequiredService<RocketChatNotificationServices>();
            services.GetRequiredService<IUserServices>();
            services.GetRequiredService<IInvestOrderContractFileServices>();
            services.GetRequiredService<IContractTemplateServices>();
            services.GetRequiredService<IContractDataServices>();
            services.GetRequiredService<IInvestSharedServices>();
            services.GetRequiredService<IGarnerOrderContractFileServices>();
            services.GetRequiredService<IGarnerContractDataServices>();
            services.GetRequiredService<IGarnerContractTemplateServices>();
            services.GetRequiredService<IGarnerFormulaServices>();
            services.GetRequiredService<NotificationServices>();
            services.GetRequiredService<InvestNotificationServices>();
            services.GetRequiredService<GarnerNotificationServices>();
            services.GetRequiredService<RealEstateNotificationServices>();
            services.GetRequiredService<IGarnerContractCodeServices>();
            services.GetRequiredService<IInvestContractCodeServices>();
            services.GetRequiredService<IInterestPaymentServices>();
            services.GetRequiredService<PaymentBackgroundJobServices>();
            services.GetRequiredService<IRstContractCodeServices>();
            services.GetRequiredService<MsbCollectMoneyServices>();
            services.GetRequiredService<IRstSignalRBroadcastServices>();
            services.GetRequiredService<IRstCountServices>();
        }
    }
}
