using Dapper;
using EPIC.BondDomain.Implements;
using EPIC.CoreDomain.Implements;
using EPIC.CoreDomain.Implements.v2;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.MapperProfiles;
using EPIC.CoreSharedServices.Implements;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Services;
using EPIC.FileEntities.Settings;
using EPIC.IdentityDomain.Implements;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.MapperProfiles;
using EPIC.IdentityServer.GrantValidators;
using EPIC.IdentityServer.Models;
using EPIC.IdentityServer.Profiles;
using EPIC.Notification.Services;
using EPIC.RealEstateEntities.MapperProfiles;
using EPIC.RocketchatDomain.Implements;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.CustomException;
using EPIC.Utils.SharedApiService;
using Hangfire;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore.Diagnostics;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace EPIC.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            EntityTypeMapper.InitEntityMapper();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHealthChecks();

            services.Configure<RecognitionApiConfiguration>(Configuration.GetSection("RecognitionApi"));

            //config file
            services.Configure<FileConfig>(Configuration.GetSection("FileConfig:File"));
            services.Configure<ImageConfig>(Configuration.GetSection("FileConfig:Image"));
            services.Configure<IdFileConfig>(Configuration.GetSection("FileConfig:IdFile"));
            services.Configure<SharedApiConfiguration>(Configuration.GetSection("SharedApi"));

            services.AddAutoMapper(typeof(IdentityMapperProfile), typeof(CoreMapperProfile), typeof(RealEstateMapperProfiles));

            services.AddControllers(options =>
                options.Filters.Add(typeof(CustomValidationError))
            );
            services.AddControllersWithViews(options =>
                options.Filters.Add(typeof(CustomValidationError))
            );

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "EPIC.IdentityServer", Version = "v1" });

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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //không cho ghi đè claim type: "sub" thành "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            //if (JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.ContainsKey("sub"))
            //{
            //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap["sub"] = "sub";
            //}

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
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.JsonWebKey(File.ReadAllText(Configuration.GetValue<string>("Jwk"))),
                    ClockSkew = TimeSpan.Zero // remove delay of token when expire
                };
                options.RequireHttpsMetadata = false;
            });

            string connectionString = Configuration.GetConnectionString("EPIC");
            services.AddSingleton(new DatabaseOptions { ConnectionString = connectionString });
            //entity framework
            services.AddDbContext<EpicSchemaDbContext>(options =>
            {
                options.UseOracle(connectionString);
                options.ConfigureWarnings(b => b.Ignore(OracleEventId.DecimalTypeDefaultWarning));
            }, ServiceLifetime.Scoped);

            #region read appsettings for identity server
            List<ApiScope> apiScopesConfig = new();
            foreach (var item in Configuration.GetSection("IdentityServer:ApiScopes").Get<List<ApiScopeConfig>>())
            {
                apiScopesConfig.Add(new ApiScope(item.Name, item.DisplayName));
            }
            Config.ApiScopes = apiScopesConfig;

            List<Client> clientsConfig = new();
            foreach (var item in Configuration.GetSection("IdentityServer:Clients").Get<List<ClientConfig>>())
            {
                var client = new Client
                {
                    ClientId = item.ClientId,
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword, GrantTypeExtend.EmailPhone },
                    RequireClientSecret = true,
                    ClientSecrets = item.ClientSecrets.Select(s => new IdentityServer4.Models.Secret(s.Sha256())).ToList(),
                    AllowedScopes = item.AllowedScopes,
                    AccessTokenLifetime = item.AccessTokenLifetime,
                    AllowOfflineAccess = item.AllowOfflineAccess,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 1296000, //15 days
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RedirectUris = item.RedirectUris,
                };
                if (item.AllowedGrantTypes != null && item.AllowedGrantTypes.Count > 0) //nếu có AllowedGrantTypes thì lấy theo AllowGrantTypes
                {
                    client.AllowedGrantTypes = item.AllowedGrantTypes;
                }
                clientsConfig.Add(client);
            }
            Config.Clients = clientsConfig;
            #endregion

            Console.WriteLine(JsonSerializer.Serialize(Config.Clients));

            services.AddScoped<IProfileService, ProfileService>();
            var identityServerBuilder = services
                .AddIdentityServer()
                .AddSigningCredential(new SigningCredentials(new Microsoft.IdentityModel.Tokens.JsonWebKey(File.ReadAllText(Configuration.GetValue<string>("Jwk"))),
                    SecurityAlgorithms.RsaSha256))
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddExtensionGrantValidator<EmailPhoneGrantValidator>()
                .AddProfileService<ProfileService>()
                .AddJwtBearerClientAuthentication();

            //services.AddLogging(builder =>
            //{
            //    builder.AddFilter("IdentityServer4", LogLevel.Debug);
            //});

            //nếu có cấu hình redis
            string redisConnectionString = Configuration.GetConnectionString("Redis");
            if (!string.IsNullOrWhiteSpace(redisConnectionString))
            {
                identityServerBuilder.AddOperationalStore(options =>
                {
                    options.RedisConnectionString = redisConnectionString;
                    options.Db = 1;
                    options.KeyPrefix = "identity_server";
                });
            }

            // Add Hangfire services.
            services.AddHangfire(configuration =>
            {
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });
                if (!string.IsNullOrWhiteSpace(redisConnectionString))
                {
                    configuration.UseRedisStorage(redisConnectionString);
                }
                else
                {
                    configuration.UseInMemoryStorage();
                    // Add the processing server as IHostedService
                    services.AddHangfireServer((service, options) =>
                    {
                        options.ServerName = $"IDENTITY";
                        options.WorkerCount = 100;
                    });
                }
            });

            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddDataProtection();

            services.AddGrpc();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IInvestorServices, InvestorServices>();
            services.AddScoped<IInvestorV2Services, InvestorV2Services>();
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<RocketChatNotificationServices>();
            services.AddScoped<IRocketChatServices, RocketChatServices>();
            services.AddScoped<NotificationServices, NotificationServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IWhiteListIpServices, WhiteListIpServices>();
            services.AddScoped<SharedNotificationApiUtils>();
            services.AddScoped<IPermissionDataServices, PermissionDataServices>();
            services.AddScoped<IPermissionServices, PermissionServices>();
            services.AddTransient<IdentityBackgroundJobServices>();
            services.AddTransient<BackgroundJobIdentityService>();
            services.AddScoped<IInvestorRegisterLogServices, InvestorRegisterLogServices>();

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
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EPIC.IdentityServer v1");
                    options.DocExpansion(DocExpansion.None);
                });
            }
            loggerFactory.AddLog4Net();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.MapHangfireDashboard("/hangfire");
                //endpoints.MapGrpcService<GreeterService>();
            });
        }
    }
}
