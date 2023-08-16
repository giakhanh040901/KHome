using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.CustomException;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Oracle.EntityFrameworkCore.Diagnostics;
using System.IO;
using System.Linq;
using EPIC.Notification.Services;
using EPIC.CoreEntities.MapperProfiles;
using EPIC.IdentityEntities.MapperProfiles;
using EPIC.BondEntities.MapperProfiles;
using EPIC.Utils.SharedApiService;
using EPIC.WebAPIBase;
using EPIC.Utils.ConfigModel;

namespace EPIC.Rocketchat
{
    public class Startup : StartUpProductBase
    {

        public Startup(IConfiguration configuration) : base(configuration)
        {
            MiniProfilerBasePath = "/api/rocketchat/profiler";
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            base.ConfigureFile(services);
            base.ConfigureSharedApi(services);
            base.ConfigureControllerCustomValidation(services);
            base.ConfigureAutoMap(services);
            //base.ConfigureMsb(services);

            services.AddScoped<SharedNotificationApiUtils>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IRocketChatServices, RocketChatServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}
