using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.IdentityDomain.Interfaces;
using EPIC.InvestDomain;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.HealthChecks
{
    public class InvestHealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContext;

        public InvestHealthCheck(ILogger<InvestHealthCheck> logger, IHttpContextAccessor httpContext)
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
            services.GetRequiredService<IProjectServices>();
            services.GetRequiredService<IInvestApproveServices>();
            services.GetRequiredService<IProjectJuridicalFileServices>();
            services.GetRequiredService<IOwnerServices>();
            services.GetRequiredService<ICalendarServices>();
            services.GetRequiredService<IGeneralContractorServices>();
            services.GetRequiredService<IDistriPolicyFileServices>();
            services.GetRequiredService<IPolicyTempServices>();
            services.GetRequiredService<IDistributionServices>();
            services.GetRequiredService<IContractTemplateServices>();
            services.GetRequiredService<IOrderServices>();
            services.GetRequiredService<IDistributionNewsServices>();
            services.GetRequiredService<IDistributionVideoServices>();
            services.GetRequiredService<IContractDataServices>();
            services.GetRequiredService<IInvestSharedServices>();
            services.GetRequiredService<IDistributionFileService>();
            services.GetRequiredService<IRocketChatServices>();
            services.GetRequiredService<IProjectImageServices>();
            services.GetRequiredService<NotificationServices>();
            services.GetRequiredService<InvestNotificationServices>();
            services.GetRequiredService<IExportExcelReportService>();
            services.GetRequiredService<IReceiveContractTemplateServices>();
            services.GetRequiredService<IBlockadeLiberationServices>();
            services.GetRequiredService<IInterestPaymentServices>();
            services.GetRequiredService<IWithdrawalServices>();
            services.GetRequiredService<IInvestRenewalsRequestServices>();
            services.GetRequiredService<IProjectOverviewFileServices>();
            services.GetRequiredService<IDashboardServices>();
            services.GetRequiredService<IInvestOrderShareServices>();
            services.GetRequiredService<IProjectTradingProviderServices>();
            services.GetRequiredService<IConfigContractCodeServices>();
            services.GetRequiredService<IUserServices>();
            services.GetRequiredService<InvestBackgroundJobService>();
            services.GetRequiredService<IInvestContractTemplateTempServices>();
            services.GetRequiredService<IInvestOrderContractFileServices>();
            services.GetRequiredService<IInvestContractCodeServices>();
            services.GetRequiredService<ILogicInvestorTradingSharedServices>();
            services.GetRequiredService<IInvestRatingServices>();
            services.GetRequiredService<IInvestProjectInformationShareServices>();
        }
    }
}
