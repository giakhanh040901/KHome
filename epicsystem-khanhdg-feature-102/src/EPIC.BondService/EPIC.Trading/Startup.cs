using Dapper;
using EPIC.BondDomain.Implements;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.MapperProfiles;
using EPIC.CoreDomain.Implements;
using EPIC.CoreDomain.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConfigModel;
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

namespace EPIC.Trading
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration): base(configuration)
        { 
            MiniProfilerBasePath = "/api/bond/profiler"; 
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

            services.Configure<FileExcelSecondPrice>(Configuration.GetSection("FileConfig:FileExcelSecondPrice"));
            services.Configure<UrlConfirmReceiveContract>(Configuration.GetSection("LinkConfirmReceiveContract"));

            //Service Business
            //services.AddScoped<IInvestorServices, InvestorServices>();
            services.AddScoped<IBondDepositProviderService, BondDepositProviderService>();
            services.AddScoped<ITradingProviderServices, TradingProviderServices>();
            services.AddScoped<IBondIssuerService, BondIssuerService>();
            services.AddScoped<IBondOrderService, BondOrderService>();
            services.AddScoped<IBondProductBondInfoService, BondInfoService>();
            services.AddScoped<IBondCalendarService, BondCalendarService>();
            services.AddScoped<IBondContractTemplateService, BondContractTemplateService>();
            services.AddScoped<IBondPrimaryService, BondPrimaryService>();
            services.AddScoped<IBondSecondaryService, BondSecondaryService>();
            services.AddScoped<IBondDistributionContractService, BondDistributionContractService>();
            services.AddScoped<IBondPolicyTempService, BondPolicyTempService>();
            services.AddScoped<IBondGuaranteeAssetService, BondGuaranteeAssetService>();
            services.AddScoped<IBondContractDataService, BondContractDataService>();
            services.AddScoped<IBondJuridicalFileService, BondJuridicalFileServices>();
            services.AddScoped<IBondPolicyFileService, BondPolicyFileService>();
            services.AddScoped<IBondSharedService, BondSharedService>();
            services.AddScoped<IInvestorAccountService, InvestorAccountServices>();
            services.AddScoped<NotificationServices, NotificationServices>();
            services.AddScoped<IBondJobsService, BondJobsService>();
            services.AddScoped<IBondPartnerCalendarService, BondPartnerCalendarService>();
            services.AddScoped<IBondBlockadeLiberationService, BondBlockadeLiberationService>();
            services.AddScoped<IBondReceiveContractTemplateService, BondReceiveContractTemplateService>();
            services.AddScoped<IBondExportReportService, BondExportReportService>();
            services.AddScoped<IBondDashboardService, BondDashboardServices>();
            services.AddScoped<IBondInterestPaymentService, BondInterestPaymentService>();
            services.AddScoped<IBondRenewalsRequestService, BondRenewalsRequestService>();
            services.AddScoped<BondBackgroundJobService>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IBondOrderShareService, BondOrderShareService>();
            services.AddScoped<IBondInfoOverviewService, BondInfoOverviewService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
