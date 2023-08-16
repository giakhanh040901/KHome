using EPIC.GarnerSharedDomain.Implements;
using EPIC.GarnerSharedDomain.Interfaces;
﻿using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.PaymentDomain.Implements;
using EPIC.PaymentDomain.Interfaces;
using EPIC.Pvcb.Configs;
using EPIC.Pvcb.Services;
using EPIC.WebAPIBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.InvestSharedDomain.Implements;
using EPIC.RealEstateSharedDomain.Implements;
using EPIC.RealEstateSharedDomain.Interfaces;
using Hangfire;
using System.Reflection;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.MSB.Services;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.PaymentAPI.HealthChecks;
using EPIC.EventDomain.Interfaces;
using EPIC.EventDomain.Implements;

namespace EPIC.PaymentAPI
{
    public class Startup : StartUpProductBase
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/payment/profiler";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureFile(services);
            base.ConfigureSharedApi(services);
            base.ConfigureAutoMap(services);
            base.ConfigureMsb(services);

            HealthChecksBuilder.AddCheck<PayemntHealthCheck>("payment");

            // thêm hangfire server để chạy
            services.AddHangfireServer((service, options) =>
            {
                options.ServerName = Assembly.GetExecutingAssembly().GetName().Name;
                options.WorkerCount = 200;
                options.Queues = new[] { HangfireQueues.Payment };
            });

            //services.AddHostedService<PaymentHostedServices>();
            services.Configure<PvcbConfiguration>(Configuration.GetSection("Pvcb"));
            services.AddScoped<PvcbCollectMoneyServices>();
            services.AddScoped<IPvcbPaymentServices, PvcbPaymentServices>();

            services.AddScoped<IMsbPaymentServices, MsbPaymentServices>();
            services.AddScoped<IMsbRequestDetailServices, MsbRequestDetailServices>();
            services.AddScoped<IMsbRequestPaymentServices, MsbRequestPaymentServices>();
            services.AddScoped<IMsbRequestDetailServices, MsbRequestDetailServices>();

            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IInvestOrderContractFileServices, InvestOrderContractFileServices>();
            services.AddScoped<IContractTemplateServices, ContractTemplateServices>();
            services.AddScoped<IContractDataServices, ContractDataServices>();
            services.AddScoped<IInvestSharedServices, InvestSharedServices>();
            services.AddScoped<IGarnerOrderContractFileServices, GarnerOrderContractFileServices>();
            services.AddScoped<IGarnerContractDataServices, GarnerContractDataServices>();
            services.AddScoped<IGarnerContractTemplateServices, GarnerContractTemplateServices>();
            services.AddScoped<IGarnerFormulaServices, GarnerFormulaServices>();
            services.AddScoped<NotificationServices>();
            services.AddScoped<InvestNotificationServices>();
            services.AddScoped<GarnerNotificationServices>();
            services.AddScoped<RealEstateNotificationServices>();
            services.AddScoped<EventNotificationServices>();
            services.AddScoped<IGarnerContractCodeServices, GarnerContractCodeServices>();
            services.AddScoped<IInvestContractCodeServices, InvestContractCodeServices>();
            services.AddScoped<IInterestPaymentServices, InterestPaymentServices>();
            services.AddScoped<PaymentBackgroundJobServices>();
            services.AddScoped<IRstContractCodeServices, RstContractCodeServices>();
            services.AddScoped<MsbCollectMoneyServices>();
            services.AddScoped<IRstSignalRBroadcastServices, RstSignalRBroadcastServices>();
            services.AddScoped<IRstCountServices, RstCountServices>();

            services.AddScoped<IEvtSignalRBroadcastService, EvtSignalRBroadcastService>();
            services.AddScoped<IEvtOrderTicketFillService, EvtOrderTicketFillService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
