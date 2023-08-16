using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.FileDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateDomain.SignalR.Services;
using EPIC.RealEstateSharedDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.RealEstateAPI.HealthChecks
{
    public class RealEstateHealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;

        public RealEstateHealthCheck(ILogger<RealEstateHealthCheck> logger, IHttpContextAccessor httpContext)
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
            services.GetRequiredService<ProductItemHubServices>();
            services.GetRequiredService<IRstProjectServices>();
            services.GetRequiredService<IRstApproveServices>();
            services.GetRequiredService<IRstOwnerServices>();
            services.GetRequiredService<IRstProjectPolicyServices>();
            services.GetRequiredService<IRstSellingPolicyTempServices>();
            services.GetRequiredService<IRstProjectFileServices>();
            services.GetRequiredService<IRstDistributionPolicyTempServices>();
            services.GetRequiredService<IRstDistributionPolicyServices>();
            services.GetRequiredService<IRstContractTemplateTempServices>();
            services.GetRequiredService<IRstConfigContractCodeServices>();
            services.GetRequiredService<IRstProjectStructureServices>();
            services.GetRequiredService<IRstProjectUtilityServices>();
            services.GetRequiredService<IRstProjectUtilityMediaServices>();
            services.GetRequiredService<IRstProjectUtilityExtendServices>();
            services.GetRequiredService<IRstProductItemServices>();
            services.GetRequiredService<IRstProjectMediaServices>();
            services.GetRequiredService<IRstProjectMediaDetailServices>();
            services.GetRequiredService<IRstDistributionServices>();
            services.GetRequiredService<IRstProductItemMediaDetailServices>();
            services.GetRequiredService<IRstProductItemMediaServices>();
            services.GetRequiredService<IFileExtensionServices>();
            services.GetRequiredService<IRstDistributionContractTemplateServices>();
            services.GetRequiredService<IRstOpenSellServices>();
            services.GetRequiredService<IRstCountServices>();
            services.GetRequiredService<IRstSellingPolicyServices>();
            services.GetRequiredService<IRstOpenSellFileServices>();
            services.GetRequiredService<IRstOpenSellContractTemplateServices>();
            services.GetRequiredService<IRstCartServices>();
            services.GetRequiredService<IRstOrderServices>();
            services.GetRequiredService<IRstOrderSellingPolicyServices>();
            services.GetRequiredService<IRstOrderPaymentServices>();
            services.GetRequiredService<IRstSignalRBroadcastServices>();
            services.GetRequiredService<ILogicInvestorTradingSharedServices>();
            services.GetRequiredService<RstOrderContractFileServices>();
            services.GetRequiredService<RstBackgroundJobServices>();
            services.GetRequiredService<RealEstateNotificationServices>();
            services.GetRequiredService<RocketChatNotificationServices>();
            services.GetRequiredService<IRstProductItemUtilityServices>();
            services.GetRequiredService<IRstContractCodeServices>();
            services.GetRequiredService<IRstRatingServices>();
            services.GetRequiredService<IRstProjectFavouriteServices>();
            services.GetRequiredService<IRstHistoryServices>();
            services.GetRequiredService<IRstExportExcelReportServices>();
            services.GetRequiredService<IRstDashboardServices>();
            services.GetRequiredService<IRstProjectInformationShareServices>();
        }
    }
}
