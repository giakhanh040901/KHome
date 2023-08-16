using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.FileDomain.Implements;
using EPIC.FileDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.Notification.Services;
using EPIC.RealEstateAPI.HealthChecks;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateDomain.SignalR.Hubs;
using EPIC.RealEstateDomain.SignalR.Services;
using EPIC.RealEstateSharedDomain.Implements;
using EPIC.RealEstateSharedDomain.Interfaces;
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

namespace EPIC.RealEstateAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/real-estate/profiler";
            EndpointRouteBuilder = (endpoints) =>
            {
                endpoints.MapHub<ProductItemHub>("/hub/real-estate/product-item");
            };
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

            HealthChecksBuilder.AddCheck<RealEstateHealthCheck>("real-estate");

            // thêm hangfire server để chạy
            services.AddHangfireServer((service, options) =>
            {
                options.ServerName = Assembly.GetExecutingAssembly().GetName().Name;
                options.WorkerCount = 200;
                options.Queues = new[] { HangfireQueues.RealEstate };
            });

            services.Configure<UrlConfirmReceiveContract>(Configuration.GetSection("LinkConfirmReceiveContract"));

            services.AddSingleton<ProductItemHubServices>(); 
            // AddScoped
            services.AddScoped<IRstProjectServices, RstProjectServices>();
            services.AddScoped<IRstApproveServices, RstApproveServices>();
            services.AddScoped<IRstOwnerServices, RstOwnerServices>();
            services.AddScoped<IRstProjectPolicyServices, RstProjectPolicyServices>();
            services.AddScoped<IRstSellingPolicyTempServices, RstSellingPolicyTempServices>();
            services.AddScoped<IRstProjectFileServices, RstProjectFileServices>();
            services.AddScoped<IRstDistributionPolicyTempServices, RstDistributionPolicyTempServices>();
            services.AddScoped<IRstDistributionPolicyServices, RstDistributionPolicyServices>();
            services.AddScoped<IRstContractTemplateTempServices, RstContractTemplateTempServices>();
            services.AddScoped<IRstConfigContractCodeServices, RstConfigContractCodeServices>();
            services.AddScoped<IRstProjectStructureServices, RstProjectStructureServices>();
            services.AddScoped<IRstProjectUtilityServices, RstProjectUtilityServices>();
            services.AddScoped<IRstProjectUtilityMediaServices, RstProjectUtilityMediaServices>();
            services.AddScoped<IRstProjectUtilityExtendServices, RstProjectUtilityExtendServices>();
            services.AddScoped<IRstProductItemServices, RstProductItemServices>();
            services.AddScoped<IRstProjectMediaServices, RstProjectMediaServices>();
            services.AddScoped<IRstProjectMediaDetailServices, RstProjectMediaDetailServices>();
            services.AddScoped<IRstDistributionServices, RstDistributionServices>();
            services.AddScoped<IRstProductItemMediaDetailServices, RstProductItemMediaDetailServices>();
            services.AddScoped<IRstProductItemMediaServices, RstProductItemMediaServices>();
            services.AddScoped<IFileExtensionServices, FileExtensionServices>();
            services.AddScoped<IRstDistributionContractTemplateServices, RstDistributionContractTemplateServices>();
            services.AddScoped<IRstOpenSellServices, RstOpenSellServices>();
            services.AddScoped<IRstCountServices, RstCountServices>();
            services.AddScoped<IRstSellingPolicyServices, RstSellingPolicyServices>();
            services.AddScoped<IRstOpenSellFileServices, RstOpenSellFileServices>();
            services.AddScoped<IRstOpenSellContractTemplateServices, RstOpenSellContractTemplateServices>();
            services.AddScoped<IRstCartServices, RstCartServices>();
            services.AddScoped<IRstOrderServices, RstOrderServices>();
            services.AddScoped<IRstOrderSellingPolicyServices, RstOrderSellingPolicyServices>();
            services.AddScoped<IRstOrderPaymentServices, RstOrderPaymentServices>();
            services.AddScoped<IRstSignalRBroadcastServices, RstSignalRBroadcastServices>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
            services.AddScoped<RstOrderContractFileServices>();
            services.AddScoped<RstBackgroundJobServices>();
            services.AddScoped<RealEstateNotificationServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IRstProductItemUtilityServices, RstProductItemUtilityServices>();
            services.AddScoped<IRstContractCodeServices, RstContractCodeServices>();
            services.AddScoped<IRstRatingServices, RstRatingServices>();
            services.AddScoped<IRstProjectFavouriteServices, RstProjectFavouriteServices>();
            services.AddScoped<IRstHistoryServices, RstHistoryServices>();
            services.AddScoped<IRstExportExcelReportServices, RstExportExcelReportServices>();
            services.AddScoped<IRstDashboardServices, RstDashboardServices>();
            services.AddScoped<IRstProjectInformationShareServices, RstProjectInformationShareServices>();
            services.AddScoped<IRstProductItemMaterialFileService, RstProductItemMaterialFileService>();
            services.AddScoped<IRstProductItemDesignDiagramFileService, RstProductItemDesignDiagramFileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
