using EPIC.BondDomain.Implements;
using EPIC.BondDomain.Interfaces;
using EPIC.CompanySharesDomain.Implements;
using EPIC.CoreSharedServices.Implements;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerSharedDomain.Implements;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.IdentityDomain.Implements;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestSharedDomain.Implements;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.PaymentDomain.Implements;
using EPIC.RealEstateSharedDomain.Implements;
using EPIC.RealEstateSharedDomain.Interfaces;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.SharedDomain.Implements;
using EPIC.SharedDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.WebAPIBase;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Reflection;

namespace EPIC.SharedAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/shared/profiler";
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
                options.Queues = new[] { HangfireQueues.Shared };
            });

            services.AddScoped<NotificationServices>();
            services.AddScoped<GarnerNotificationServices>();
            services.AddScoped<LoyaltyNotificationServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IRocketChatServices, RocketChatServices>();
            services.AddScoped<IInvestorSharedServices, InvestorSharedServices>();

            #region chia sẻ thông tin
            services.AddScoped<IOperationalInfoServices, OperationalInfoServices>();
            services.AddScoped<IInvestorSharedTpsServices, InvestorSharedTpsServices>();
            services.AddScoped<IInvestorSharedTelesaleServices, InvestorSharedTelesaleServices>();
            #endregion

            #region các service phục vụ chạy background identity
            services.AddTransient<IdentityBackgroundJobServices>();
            #endregion

            #region các service phục vụ chạy background bond
            services.AddScoped<EPIC.BondDomain.Interfaces.IBondContractTemplateService, EPIC.BondDomain.Implements.BondContractTemplateService>();
            services.AddScoped<EPIC.BondDomain.Interfaces.IBondContractDataService, EPIC.BondDomain.Implements.BondContractDataService>();
            services.AddScoped<IBondSharedService, BondSharedService>();
            services.AddScoped<IBondOrderShareService, BondOrderShareService>();
            services.AddScoped<BondBackgroundJobService>();
            services.AddScoped<BackgroundJobIdentityService>();
            #endregion

            #region các service phục vụ chạy background invest
            services.AddScoped<EPIC.InvestDomain.Interfaces.IContractTemplateServices, EPIC.InvestDomain.Implements.ContractTemplateServices>();
            services.AddScoped<EPIC.InvestDomain.Interfaces.IContractDataServices, EPIC.InvestDomain.Implements.ContractDataServices>();
            services.AddScoped<IInvestSharedServices, InvestSharedServices>();
            services.AddScoped<IInvestOrderShareServices, InvestOrderShareServices>();
            services.AddScoped<InvestBackgroundJobService>();
            services.AddScoped<IInvestContractCodeServices, InvestContractCodeServices>();
            services.AddScoped<IInvestOrderContractFileServices, InvestOrderContractFileServices>();
            //services.AddScoped<IdentityBackgroundJobServices>();
            //services.AddScoped<BackgroundJobIdentityService>();
            #endregion

            #region các service phục vụ chạy background cps
            services.AddTransient<EPIC.CompanySharesDomain.Interfaces.IContractTemplateServices, EPIC.CompanySharesDomain.Implements.ContractTemplateServices>();
            services.AddTransient<EPIC.CompanySharesDomain.Interfaces.IContractDataServices, EPIC.CompanySharesDomain.Implements.ContractDataServices>();
            services.AddTransient<CpsBackgroundJobService>();
            #endregion

            #region các service phục vụ chạy background real estate
            services.AddScoped<IRstContractCodeServices, RstContractCodeServices>();
            services.AddScoped<RealEstateNotificationServices>();
            #endregion

            #region các service phục vụ chạy background garner
            services.AddScoped<IGarnerContractTemplateServices, GarnerContractTemplateServices>();
            services.AddScoped<IGarnerContractDataServices, GarnerContractDataServices>();
            services.AddScoped<IGarnerFormulaServices, GarnerFormulaServices>();
            services.AddScoped<IGarnerContractCodeServices, GarnerContractCodeServices>();
            services.AddScoped<IGarnerOrderContractFileServices, GarnerOrderContractFileServices>();
            services.AddScoped<GarnerBackgroundJobServices>();
            #endregion

            #region các service phục vụ chạy background payment
            services.AddScoped<InvestNotificationServices>();
            services.AddScoped<GarnerNotificationServices>();
            services.AddScoped<PaymentBackgroundJobServices>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            DashboardOptions dashboardOptions = null;
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1");
                    options.DocExpansion(DocExpansion.None);
                });
            }
            else
            {
                dashboardOptions = new DashboardOptions
                {
                    Authorization = new[]
                    {
                       new HangfireFilter(app.ApplicationServices.GetService(typeof(IConfiguration)) as IConfiguration)
                    }
                };
            }

            app.UseMiniProfiler();

            //app.UseHttpsRedirection();
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseHangfireDashboard(options: dashboardOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapHangfireDashboard("/hangfire");
            });
            loggerFactory.AddLog4Net();
        }
    }
}
