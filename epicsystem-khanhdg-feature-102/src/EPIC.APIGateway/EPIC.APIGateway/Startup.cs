using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Ocelot.Cache.CacheManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocelot.Administration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http.Features;
using EPIC.Utils.ConstantVariables.Shared;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EPIC.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string CmsAllowOrigins = "_cmsAllowOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            Action<JwtBearerOptions> options = o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
            };
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = Int32.MaxValue;
            });
            services.AddControllers();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EPIC.APIGateway", Version = "v1" });
            });
            services.AddOcelot();
            //.AddEureka()
            //.AddCacheManager(x => x.WithDictionaryHandle())
            //.AddAdministration("/administration", options);

            string allowOrigins = Configuration.GetSection("AllowedOrigins").Value;
            var origins = allowOrigins.Split(';');
            File.WriteAllText("cors.now.txt", $"CORS: {allowOrigins}");
            services.AddCors(options =>
            {
                options.AddPolicy(name: CmsAllowOrigins,
                    builder =>
                    {
                        builder
                            .WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Content-Disposition");
                    });
            });

            services.AddSwaggerForOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();

                app.UseSwaggerForOcelotUI(opt =>
                {
                    //opt.SwaggerEndpoint("");
                    opt.PathToSwaggerGenerator = "/swagger/docs";
                    opt.DocExpansion(DocExpansion.None);
                });
            }

            app.UseForwardedHeaders();
            //app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseCors(CmsAllowOrigins); // allow credentials

            app.UseWebSockets();
            app.UseOcelot().Wait();

            loggerFactory.AddLog4Net();
        }
    }
}
