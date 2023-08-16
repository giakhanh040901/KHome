using EPIC.DataAccess.Base;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Utils.ConstantVariables.Identity;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json;
using Oracle.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.UnitTestsBase
{
    public class UnitTestBase
    {
        protected string _baseUrl;

        /// <summary>
        /// Chạy start up của project muốn test
        /// </summary>
        /// <typeparam name="TStartUp"></typeparam>
        /// <returns></returns>
        public IHost GetHost<TStartUp>(IHttpContextAccessor httpContextAccessor = null) where TStartUp : class
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.AddEnvironmentVariables(prefix: "DOTNET_");
                })
                .ConfigureAppConfiguration((hostBuilderContext, configBuilder) =>
                {
                    //hostBuilderContext.HostingEnvironment.EnvironmentName = "Development";
                    var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                    configBuilder.AddJsonFile("appsettings.Test.json");
                    if (env != null)
                    {
                        configBuilder.AddJsonFile($"appsettings.{env}.json");
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartUp>();
                })
                .ConfigureServices(services =>
                {
                    if (httpContextAccessor != null)
                    {
                        services.AddSingleton(httpContextAccessor);
                    }
                });
            return hostBuilder.Build();
        }

        /// <summary>
        /// Tạo httpcontext cho trading provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public virtual IHttpContextAccessor CreateHttpContextTrading(int userId, int tradingProviderId)
        {
            var httpContext = new DefaultHttpContext();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.UserType, UserTypes.TRADING_PROVIDER),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.TradingProviderId, tradingProviderId.ToString()),
            });
            httpContext.User = new ClaimsPrincipal(identity);
            // Fake địa chỉ IP
            httpContext.Connection.RemoteIpAddress = new IPAddress(16885952);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return mockHttpContextAccessor.Object;
        }

        /// <summary>
        /// Tạo httpcontext cho partner
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public virtual IHttpContextAccessor CreateHttpContextPartner(int userId, int partnerId)
        {
            var httpContext = new DefaultHttpContext();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.UserType, UserTypes.PARTNER),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.PartnerId, partnerId.ToString()),
            });
            httpContext.User = new ClaimsPrincipal(identity);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return mockHttpContextAccessor.Object;
        }

        /// <summary>
        /// Tạo httpcontext cho investor
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public virtual IHttpContextAccessor CreateHttpContextInvestor(int userId, int investorId)
        {
            var httpContext = new DefaultHttpContext();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.UserType, UserTypes.INVESTOR),
                new Claim(Utils.ConstantVariables.Shared.ClaimTypes.InvestorId, investorId.ToString()),
            });
            httpContext.User = new ClaimsPrincipal(identity);
            // Fake địa chỉ IP
            httpContext.Connection.RemoteIpAddress = new IPAddress(16885952);
            // Fake Session
            httpContext.Session = new Mock<ISession>().Object;
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return mockHttpContextAccessor.Object;
        }
    }

    public class TokenPhoneEmailResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("is_first_time")]
        public bool IsFirstTime { get; set; }

        [JsonPropertyName("is_temp_password")]
        public bool IsTempPassword { get; set; }

        [JsonPropertyName("is_ekyc")]
        public bool IsEkyc { get; set; }

        [JsonPropertyName("is_have_pin")]
        public bool IsHavePin { get; set; }

        [JsonPropertyName("is_verified_email")]
        public bool IsVerifiedEmail { get; set; }

        [JsonPropertyName("is_temp_pin")]
        public bool IsTempPin { get; set; }

        [JsonPropertyName("is_verified_face")]
        public bool IsVerifiedFace { get; set; }

        [JsonPropertyName("is_temp_user")]
        public bool IsTempUser { get; set; }

        [JsonPropertyName("step")]
        public int Step { get; set; }
    }

    public class TokenPasswordResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }

    public static class UnitTestExtensions
    {
        /// <summary>
        /// Lấy service để test
        /// </summary>
        public static TService GetService<TService>(this IHost host) where TService : class
        {
            return host.Services.GetService(typeof(TService)) as TService;
        }

        public static async Task<TokenPhoneEmailResponse> RequestPhoneEmailTokenAsync(this HttpClient client, string phone, string password)
        {
            // Prepare the form data
            var formData = new Dictionary<string, string>
            {
                { "client_id", "client1" },
                { "client_secret", "secret1" },
                { "grant_type", "email_phone" },
                { "user", phone },
                { "password", password },
                //{ "fcm_token", "examplepass" },
            };

            // Encode the form data
            var content = new FormUrlEncodedContent(formData);

            // Send the HTTP POST request with the form data
            HttpResponseMessage response = await client.PostAsync("/connect/token", content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response
            var responseBody = await response.Content.ReadFromJsonAsync<TokenPhoneEmailResponse>();
            return responseBody;
        }

        public static async Task<TokenPasswordResponse> RequestPasswordTokenAsync(this HttpClient client, string username, string password)
        {
            // Prepare the form data
            var formData = new Dictionary<string, string>
            {
                { "client_id", "client1" },
                { "client_secret", "secret1" },
                { "grant_type", "password" },
                { "username", username },
                { "password", password },
            };

            // Encode the form data
            var content = new FormUrlEncodedContent(formData);

            // Send the HTTP POST request with the form data
            HttpResponseMessage response = await client.PostAsync("/connect/token", content);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response
            var responseBody = await response.Content.ReadFromJsonAsync<TokenPasswordResponse>();
            return responseBody;
        }
    }
}
