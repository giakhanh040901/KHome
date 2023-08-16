using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Implements;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore.Diagnostics;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EPIC.ImageAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = Int32.MaxValue;
            });
            //config file
            services.Configure<FileConfig>(Configuration.GetSection("FileConfig:File"));
            services.Configure<ImageConfig>(Configuration.GetSection("FileConfig:Image"));
            services.Configure<IdFileConfig>(Configuration.GetSection("FileConfig:IdFile"));

            // ADD
            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "EPIC.File", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.**
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            });

            // CORS
            //string allowOrigins = Configuration.GetSection("AllowedOrigins").Value;
            //string[] origins = allowOrigins.Split(';');

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(_corsPolicy, builder =>
            //        builder.WithOrigins(origins)
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .WithExposedHeaders("Content-Disposition"));
            //});
            // AUTHEN
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new JsonWebKey(File.ReadAllText(Configuration.GetValue<string>("Jwk"))),
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
            });

            services.AddResponseCaching();

            // INJECT SERVICE
            services.AddScoped<IFileServices, FileServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EPIC.File v1");
                    options.DocExpansion(DocExpansion.None);
                });
            }

            app.UseHttpsRedirection();
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            loggerFactory.AddLog4Net();
        }
    }
}
