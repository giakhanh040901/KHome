using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventDomain.SingalR.Hubs;
using EPIC.FileDomain.Implements;
using EPIC.FileDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.Notification.Services;
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

namespace EPIC.EventAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/event/profiler";
            EndpointRouteBuilder = (endpoints) =>
            {
                endpoints.MapHub<EventHub>("/hub/event");
            };
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureSharedApi(services);
            base.ConfigureFile(services);
            base.ConfigureControllerCustomValidation(services);
            base.ConfigureAutoMap(services);
            base.ConfigureMsb(services);

            // thêm hangfire server để chạy
            services.AddHangfireServer((service, options) =>
            {
                options.ServerName = Assembly.GetExecutingAssembly().GetName().Name;
                options.WorkerCount = 200;
                options.Queues = new[] { HangfireQueues.Event };
            });
            services.AddScoped<IEvtManagerEventService, EvtManagerEventService>();
            services.AddScoped<IEvtAppEventService, EvtAppEventService>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
            services.AddScoped<IEvtEventDetailServices, EvtEventDetailServices>();
            services.AddScoped<IEvtTicketService, EvtTicketService>();
            services.AddScoped<IFileExtensionServices, FileExtensionServices>();

            services.AddScoped<IEvtConfigContractCodeServices, EvtConfigContractCodeServices>();
            services.AddScoped<IEvtOrderService, EvtOrderService>();
            services.AddScoped<IEvtOrderValidService, EvtOrderValidService>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IEvtEventMediaServices, EvtEventMediaServices>();
            services.AddScoped<IEvtAppEventMediaService, EvtAppEventMediaService>();
            services.AddScoped<IEvtEventMediaDetailServices, EvtEventMediaDetailServices>();
            services.AddScoped<IFileExtensionServices, FileExtensionServices>();
            services.AddScoped<IEvtOrderPaymentServices, EvtOrderPaymentServices>();
            services.AddScoped<IEvtAppOrderService, EvtAppOrderService>();
            services.AddScoped<IEvtOrderCommonService, EvtOrderCommonService>();
            services.AddScoped<IEvtAppEventHistoryServices, EvtAppEventHistoryServices>();
            services.AddScoped<ILogicInvestorTradingSharedServices, LogicInvestorTradingSharedServices>();
            services.AddScoped<EventNotificationServices>();
            services.AddScoped<EvtBackgroundJobServices>();
            services.AddScoped<IEvtOrderTicketDetailService, EvtOrderTicketDetailService>();
            services.AddScoped<IEvtEventDescriptionMediaServices, EvtEventDescriptionMediaServices>();
            services.AddScoped<IEvtTicketTemplateService, EvtTicketTemplateService>();
            services.AddScoped<IEvtAdminEventService, EvtAdminEventService>();
            services.AddScoped<IEvtSignalRBroadcastService, EvtSignalRBroadcastService>();
            services.AddScoped<IEvtOrderTicketFillService, EvtOrderTicketFillService>();
            services.AddScoped<IEvtDeliveryTicketTemplateService, EvtDeliveryTicketTemplateService>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
