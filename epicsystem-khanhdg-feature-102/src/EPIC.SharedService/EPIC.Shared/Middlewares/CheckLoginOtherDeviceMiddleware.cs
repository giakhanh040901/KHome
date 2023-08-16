using EPIC.DataAccess.Base;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Shared.Middlewares
{
    /// <summary>
    /// Kiểm tra login trên thiết bị khác loại trừ localhost
    /// </summary>
    public class CheckLoginOtherDeviceMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckLoginOtherDeviceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ILogger logger = context.RequestServices.GetService(typeof(ILogger<CheckLoginOtherDeviceMiddleware>)) as ILogger;
            EpicSchemaDbContext dbContext = context.RequestServices.GetService(typeof(EpicSchemaDbContext)) as EpicSchemaDbContext;
            UsersDevicesEFRepository usersDevicesEFRepository = new UsersDevicesEFRepository(dbContext);

            var request = context.Request;
            if (request.Host.Host == "localhost")
            {
                //CommonUtils.GetCurrentUserId(context)

                await _next(context);
                return;
            }
            
            await _next(context);
        }
    }

    public static class CheckLoginOtherDeviceMiddlewareExtensions
    {
        public static IApplicationBuilder CheckLoginOtherDevice(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckLoginOtherDeviceMiddleware>();
        }
    }
}