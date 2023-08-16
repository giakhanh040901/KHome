using Dapper;
using EPIC.CompanySharesDomain.Implements;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.MapperProfiles;
using EPIC.Entities;
using EPIC.FileEntities.Settings;
using EPIC.Notification.Services;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.CustomException;
using EPIC.Utils.SharedApiService;
using EPIC.WebAPIBase;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

namespace EPIC.CompanySharesAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/company-shares/profiler";
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

            services.AddScoped<ICalendarServices, CalendarServices>();
            services.AddScoped<ICpsInfoServices, CpsInfoServices>();
            services.AddScoped<ICpsInfoTradingProviderServices, CpsInfoTradingProviderServices>();
            // Service Company Share
            services.AddScoped<ICpsOrderPaymentServices, CpsOrderPaymentServices>();
            services.AddScoped<ICpsSecondaryServices, CpsSecondaryServices>();
            services.AddScoped<NotificationServices, NotificationServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderSharedServices, OrderSharedServices>();
            services.AddScoped<ICpsSharedServices, CpsSharedServices>();
            services.AddScoped<ICpsIssuerService, CpsIssuerService>();
            services.AddScoped<ICpsPolicyFileServices, CpsPolicyFileServices>();
            services.AddScoped<IPolicyTempServices, PolicyTempServices>();
            services.AddScoped<IContractTemplateServices, ContractTemplateServices>();
            services.AddScoped<IContractDataServices, ContractDataServices>();
            services.AddScoped<CpsBackgroundJobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
