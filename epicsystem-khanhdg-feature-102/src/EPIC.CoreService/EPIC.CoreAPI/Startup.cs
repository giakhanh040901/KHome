using Dapper;
using EPIC.BondDomain.Implements;
using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Implements;
using EPIC.CoreDomain.Implements.v2;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.MapperProfiles;
using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.EventDomain.Implements;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.MapperProfiles;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.MapperProfiles;
using EPIC.InvestSharedDomain.Implements;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.MSB.Services;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.CustomException;
using EPIC.Utils.SharedApiService;
using EPIC.WebAPIBase;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EPIC.CoreAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/core/profiler";
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

            // thêm hangfire server để chạy
            services.AddHangfireServer((service, options) =>
            {
                options.ServerName = Assembly.GetExecutingAssembly().GetName().Name;
                options.WorkerCount = 200;
                options.Queues = new[] { HangfireQueues.Core };
            });

            services.Configure<RecognitionApiConfiguration>(Configuration.GetSection("RecognitionApi"));

            services.AddScoped<IBusinessCustomerServices, BusinessCustomerServices>();
            services.AddScoped<IPartnerServices, PartnerServices>();
            services.AddScoped<IManagerInvestorServices, ManagerInvestorServices>();
            services.AddScoped<ISaleExportCollapContractServices, SaleExportCollapContractServices>();
            services.AddScoped<ISaleServices, SaleServices>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<IApproveServices, ApproveServices>();
            services.AddScoped<IProvinceServices, ProvinceServices>();
            services.AddScoped<ICoreBankServices, CoreBankServices>();
            services.AddScoped<IRocketChatServices, RocketChatServices>();
            services.AddScoped<IAssetManagerServices, AssetManagerServices>();
            services.AddScoped<ISaleInvestorServices, SaleInvestorServices>();
            services.AddScoped<ISaleInvestorV2Services, SaleInvestorV2Services>();
            services.AddTransient<IBondSharedService, BondSharedService>();
            services.AddScoped<ICoreProductNewsServices, CoreProductNewsServices>();
            services.AddTransient<IInvestSharedServices, InvestSharedServices>();
            services.AddTransient<NotificationServices>();
            services.AddTransient<EventNotificationServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddTransient<InvestNotificationServices>();
            services.AddTransient<ICollabContractServices, CollabContractServices>();
            services.AddScoped<ITradingProviderServices, TradingProviderServices>();
            services.AddTransient<ISignPdfServices, SignPdfServices>();

            services.AddScoped<IBusinessLicenseFileServices, BusinessLicenseFileServices>();
            services.AddScoped<IInvestorServices, InvestorServices>();
            services.AddScoped<IInvestorV2Services, InvestorV2Services>();
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddScoped<IInvestOrderShareServices, InvestOrderShareServices>();
            services.AddTransient<InvestDomain.Interfaces.IContractTemplateServices, InvestDomain.Implements.ContractTemplateServices>();
            services.AddTransient<InvestDomain.Interfaces.IContractDataServices, InvestDomain.Implements.ContractDataServices>();
            services.AddScoped<IBondOrderShareService, BondOrderShareService>();
            services.AddScoped<IGarnerFormulaServices, GarnerFormulaServices>();
            services.AddTransient<InvestBackgroundJobService>();
            services.AddTransient<EvtBackgroundJobServices>();
            services.AddTransient<BackgroundJobIdentityService>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ISaleShareServices, SaleShareServices>();
            services.AddScoped<CoreDomain.Interfaces.IExportExcelReportServices, CoreDomain.Implements.ExportExcelReportService>();
            services.AddScoped<ITradingMsbPrefixAccountServices, TradingMsbPrefixAccountServices>();
            services.AddScoped<IInvestContractCodeServices, InvestContractCodeServices>();
            services.AddScoped<IInvestOrderContractFileServices, InvestOrderContractFileServices>();
            services.AddScoped<IPartnerMsbPrefixAccountServices, PartnerMsbPrefixAccountServices>();
            services.AddScoped<ITradingProviderConfigServices, TradingProviderConfigServices>();
            services.AddScoped<CoreDomain.Interfaces.IDashboardServices, CoreDomain.Implements.DashboardServices>();
            services.AddScoped<IInvestorRegisterLogServices, InvestorRegisterLogServices>();
            services.AddScoped<ITradingFirstMessageServices, TradingFirstMessageServices>();
            services.AddScoped<IPartnerBankAccountServices, PartnerBankAccountServices>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
            services.AddScoped<IInvestorSearchService, InvestorSearchService>();
            services.AddScoped<ICallCenterConfigService, CallCenterConfigService>();
            services.AddScoped<IExportDataServices, ExportDataServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var backgroundJobService = scope.ServiceProvider.GetRequiredService<ITradingProviderConfigServices>();
                RecurringJob.AddOrUpdate("trading-provider-delete-order", () => backgroundJobService.DeletedOrderTimeUpByTradingProvider(), Cron.Daily);
            }
        }
    }
}
