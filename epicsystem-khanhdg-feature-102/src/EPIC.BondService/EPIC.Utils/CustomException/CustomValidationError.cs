using EPIC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Utils.CustomException
{
    public class CustomValidationError : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            ILogger logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<CustomValidationError>)) as ILogger;
            var request = context.HttpContext.Request;
            string errStr = $"BadRequest: Path = {request.Path}, Query = {JsonSerializer.Serialize(request.Query)}, ";

            if (context.Result is BadRequestObjectResult badRequestObjectResult)
                if (badRequestObjectResult.Value is ValidationProblemDetails)
                {
                    var errorFull = ToDictionary(badRequestObjectResult.Value);
                    errorFull.TryGetValue("Errors", out object errors);
                    logger?.LogWarning(errStr + $" Errors: {JsonSerializer.Serialize(errors != null ? errors : new {})}");
                    context.Result = new OkObjectResult(new APIResponse(Utils.StatusCode.Error, errors, (int)ErrorCode.BadRequest, "Bad Request"));
                }

            base.OnResultExecuting(context);
        }

        private Dictionary<string, object> ToDictionary(object input)
        {
            var s = input.GetType().GetProperties().ToDictionary(
                        propInfo => propInfo.Name,
                        propInfo => propInfo.GetValue(input, null)
                    );
            return s;
        }
    }
}
