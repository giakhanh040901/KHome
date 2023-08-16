using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPIC.WebAPIBase;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestDomain;
using EPIC.InvestSharedDomain.Implements;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConfigModel;
using EPIC.LoyaltyDomain.Implements;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.FileDomain.Services;

namespace EPIC.LoyaltyAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/loyalty/profiler";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureFile(services);
            base.ConfigureSharedApi(services);
            base.ConfigureControllerCustomValidation(services);
            base.ConfigureAutoMap(services);
            //base.ConfigureMsb(services);

            services.Configure<LinkVoucherConfiguration>(Configuration.GetSection("LinkVoucher"));

            //services.Configure<UrlConfirmReceiveContract>(Configuration.GetSection("LinkConfirmReceiveContract"));
            services.AddScoped<ILoyVoucherServices, LoyVoucherServices>();
            services.AddScoped<IHisAccumulatePointServices, HisAccumulatePointServices>();
            services.AddScoped<ILoyConversionPointServices, LoyConversionPointServices>();
            services.AddScoped<ILoyPointInvestorServices, LoyPointInvestorServices>();
            services.AddScoped<ILoyExportExcelReportServices, LoyExportExcelReportServices>();
            services.AddScoped<ILoyLuckyProgramServices, LoyLuckyProgramServices>();
            services.AddScoped<ILoyLuckyScenarioServices, LoyLuckyScenarioServices>();
            services.AddScoped<ILoyRankServices, LoyRankServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<LoyaltyNotificationServices>();
            services.AddScoped<IFileServices, FileServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
