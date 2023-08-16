using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.GarnerAPI.HealthChecks;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerSharedDomain.Implements;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.Notification.Services;
using EPIC.Utils.ConfigModel;
using EPIC.WebAPIBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPIC.GarnerAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/garner/profiler";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureSharedApi(services);
            base.ConfigureFile(services);
            base.ConfigureControllerCustomValidation(services);
            base.ConfigureAutoMap(services);
            base.ConfigureMsb(services);

            HealthChecksBuilder.AddCheck<GarnerHealthCheck>("garner");

            services.Configure<UrlConfirmReceiveContract>(Configuration.GetSection("LinkConfirmReceiveContract"));
            services.AddHostedService<GarnerHostedServices>();

            services.AddScoped<IGarnerProductServices, GarnerProductServices>();
            services.AddScoped<IGarnerApproveServices, GarnerApproveServices>();
            services.AddScoped<IGarnerProductTradingProviderServices, GarnerProductTradingProviderServices>();
            services.AddScoped<IGarnerCalendarServices, GarnerCalendarServices>();
            services.AddScoped<IGarnerDistributionServices, GarnerDistributionServices>();
            services.AddScoped<IGarnerPolicyTempServices, GarnerPolicyTempServices>();
            services.AddScoped<IGarnerPolicyDetailTempServices, GarnerPolicyDetailTempServices>();
            services.AddScoped<IGarnerFormulaServices, GarnerFormulaServices>();
            services.AddScoped<IGarnerOrderServices, GarnerOrderServices>();
            services.AddScoped<IGarnerOrderContractFileServices, GarnerOrderContractFileServices>();
            services.AddScoped<IGarnerOrderPaymentServices, GarnerOrderPaymentServices>();
            services.AddScoped<IGarnerContractTemplateTempServices, GarnerContractTemplateTempServices>();
            services.AddScoped<IGarnerContractTemplateServices, GarnerContractTemplateServices>();
            services.AddScoped<IGarnerPartnerCalendarServices, GarnerPartnerCalendarServices>();
            services.AddScoped<IGarnerContractDataServices, GarnerContractDataServices>();
            services.AddScoped<IGarnerWithdrawalServices, GarnerWithdrawalServices>();
            services.AddScoped<IGarnerDistributionSharedServices, GarnerDistributionSharedServices>();
            services.AddScoped<GarnerBackgroundJobServices>();
            services.AddScoped<IGarnerInterestPaymentServices, GarnerInterestPaymentServices>();
            services.AddScoped<IGarnerBlockadeLiberationServices, GarnerBlockadeLiberationServices>();
            services.AddScoped<GarnerNotificationServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<NotificationServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IGarnerContractCodeServices, GarnerContractCodeServices>();
            services.AddScoped<IGarnerReceiveContractTemplateServices, GarnerReceiveContractTemplateServices>();
            services.AddScoped<IGarnerRatingServices, GarnerRatingServices>();
            services.AddScoped<IGarnerDashboardServices, GarnerDashboardServices>();
            services.AddScoped<IGarnerExportExcelReportServices, GarnerExportExcelReportServices>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
