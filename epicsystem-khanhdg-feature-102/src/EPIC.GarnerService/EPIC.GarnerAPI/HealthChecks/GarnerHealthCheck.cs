using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.Notification.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.HealthChecks
{
    public class GarnerHealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;

        public GarnerHealthCheck(ILogger<GarnerHealthCheck> logger, IHttpContextAccessor httpContext)
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
                return Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));
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

            services.GetRequiredService<IGarnerProductServices>();
            services.GetRequiredService<IGarnerApproveServices>();
            services.GetRequiredService<IGarnerProductTradingProviderServices>();
            services.GetRequiredService<IGarnerCalendarServices>();
            services.GetRequiredService<IGarnerDistributionServices>();
            services.GetRequiredService<IGarnerPolicyTempServices>();
            services.GetRequiredService<IGarnerPolicyDetailTempServices>();
            services.GetRequiredService<IGarnerFormulaServices>();
            services.GetRequiredService<IGarnerOrderServices>();
            services.GetRequiredService<IGarnerOrderContractFileServices>();
            services.GetRequiredService<IGarnerOrderPaymentServices>();
            services.GetRequiredService<IGarnerContractTemplateTempServices>();
            services.GetRequiredService<IGarnerContractTemplateServices>();
            services.GetRequiredService<IGarnerPartnerCalendarServices>();
            services.GetRequiredService<IGarnerContractDataServices>();
            services.GetRequiredService<IGarnerWithdrawalServices>();
            services.GetRequiredService<IGarnerDistributionSharedServices>();
            services.GetRequiredService<GarnerBackgroundJobServices>();
            services.GetRequiredService<IGarnerInterestPaymentServices>();
            services.GetRequiredService<IGarnerBlockadeLiberationServices>();
            services.GetRequiredService<GarnerNotificationServices>();
            services.GetRequiredService<RocketChatNotificationServices>();
            services.GetRequiredService<NotificationServices>();
            services.GetRequiredService<RocketChatNotificationServices>();
            services.GetRequiredService<IGarnerContractCodeServices>();
            services.GetRequiredService<IGarnerReceiveContractTemplateServices>();
            services.GetRequiredService<IGarnerRatingServices>();
            services.GetRequiredService<IGarnerDashboardServices>();
            services.GetRequiredService<IGarnerExportExcelReportServices>();
            services.GetRequiredService<ILogicInvestorTradingSharedServices>();
        }
    }
}
