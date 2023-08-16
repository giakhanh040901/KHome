using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.InvestAPI.HealthChecks;
using EPIC.InvestDomain;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestSharedDomain.Implements;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.WebAPIBase;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace EPIC.InvestAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/invest/profiler";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureFile(services);
            base.ConfigureSharedApi(services);
            base.ConfigureControllerCustomValidation(services);
            base.ConfigureAutoMap(services);
            base.ConfigureMsb(services);

            HealthChecksBuilder.AddCheck<InvestHealthCheck>("invest");

            services.Configure<UrlConfirmReceiveContract>(Configuration.GetSection("LinkConfirmReceiveContract"));

            // thêm hangfire server để chạy
            services.AddHangfireServer((service, options) =>
            {
                options.ServerName = Assembly.GetExecutingAssembly().GetName().Name;
                options.WorkerCount = 200;
                options.Queues = new[] { HangfireQueues.Invest };
            });

            services.AddScoped<IProjectServices, ProjectServices>();
            services.AddScoped<IInvestApproveServices, InvestApproveServices>();
            services.AddScoped<IProjectJuridicalFileServices, ProjectJuridicalFileServices>();
            services.AddScoped<IOwnerServices, OwnerServices>();
            services.AddScoped<ICalendarServices, CalendarServices>();
            services.AddScoped<IGeneralContractorServices, GeneralContractorServices>();
            services.AddScoped<IDistriPolicyFileServices, DistriPolicyFileServices>();
            services.AddScoped<IPolicyTempServices, PolicyTempServices>();
            services.AddScoped<IDistributionServices, DistributionServices>();
            services.AddScoped<IContractTemplateServices, ContractTemplateServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IDistributionNewsServices, DistributionNewsServices>();
            services.AddScoped<IDistributionVideoServices, DistributionVideoServices>();
            services.AddScoped<IContractDataServices, ContractDataServices>();
            services.AddScoped<IInvestSharedServices, InvestSharedServices>();
            services.AddScoped<IDistributionFileService, DistributionFileService>();
            services.AddScoped<IRocketChatServices, RocketChatServices>();
            services.AddScoped<IProjectImageServices, ProjectImageServices>();
            services.AddScoped<NotificationServices, NotificationServices>();
            services.AddScoped<InvestNotificationServices, InvestNotificationServices>();
            services.AddScoped<IExportExcelReportService, ExportExcelReportService>();
            services.AddScoped<IReceiveContractTemplateServices, ReceiveContractTemplateServices>();
            services.AddScoped<IBlockadeLiberationServices, BlockadeLiberationServices>();
            services.AddScoped<IInterestPaymentServices, InterestPaymentServices>();
            services.AddScoped<IWithdrawalServices, WithdrawalServices>();
            services.AddScoped<IInvestRenewalsRequestServices, InvestRenewalsRequestServices>();
            services.AddScoped<IProjectOverviewFileServices, ProjectOverviewFileServices>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddScoped<IInvestOrderShareServices, InvestOrderShareServices>();
            services.AddScoped<IProjectTradingProviderServices, ProjectTradingProviderServices>();
            services.AddScoped<IConfigContractCodeServices, ConfigContractCodeServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<InvestBackgroundJobService>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IInvestContractTemplateTempServices, InvestContractTemplateTempServices>();
            services.AddScoped<IInvestOrderContractFileServices, InvestOrderContractFileServices>();
            services.AddScoped<IInvestContractCodeServices, InvestContractCodeServices>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
            services.AddScoped<IInvestRatingServices, InvestRatingServices>();
            services.AddScoped<IInvestProjectInformationShareServices, InvestProjectInformationShareServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
