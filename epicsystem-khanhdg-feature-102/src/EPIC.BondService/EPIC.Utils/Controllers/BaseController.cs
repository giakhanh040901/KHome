using DocumentFormat.OpenXml.Office2016.Excel;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.Utils.Controllers
{
    /// <summary>
    /// Base controller, xử lý ngoại lệ
    /// </summary>
    public class BaseController : ControllerBase
    {
        protected ILogger _logger;

        [NonAction]
        public APIResponse OkException(Exception ex)
        {
            var request = Request;
            if (request == null)
            {
                return new APIResponse(Utils.StatusCode.Error, null, (int)ErrorCode.InternalServerError, ex.Message);
            }
            string errStr = $"Path = {request.Path}, Query = {JsonSerializer.Serialize(request.Query)}";
            var claims = HttpContext.User.Identity != null ? HttpContext.User.Identity as ClaimsIdentity : null;
            if (claims != null)
            {
                var userId = claims.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                errStr += $"userId = {userId}, ";
            }

            var fex = ex as FaultException;
            if (fex != null)
            {
                if (int.Parse(fex.Code.Name) <= 1000)
                {
                    try
                    {
                        //if (claims != null)
                        //{
                        //    errStr += $"claims = {JsonSerializer.Serialize(claims.Claims.ToList())}";
                        //}
                    }
                    catch (Exception exClaim)
                    {
                        _logger?.LogError(exClaim, $"Lỗi HttpContext.User.Identity as ClaimsIdentity, {exClaim.GetType()}: Message = {exClaim.Message}");
                    }

                    _logger?.LogError(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                }
                else
                {
                    _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                }
                return new APIResponse(Utils.StatusCode.Error, null, int.Parse(fex.Code.Name), fex.Message);
            }

            var httpRequestEx = ex as HttpRequestException;
            if (httpRequestEx != null)
            {
                _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                return new APIResponse(Utils.StatusCode.Error, httpRequestEx.Message, (int)ErrorCode.HttpRequestException, httpRequestEx.Message);
            }

            var dbUpdateException = ex as DbUpdateException;
            if (dbUpdateException != null)
            {
                var innerEx = ex.InnerException;
                if (innerEx != null)
                {
                    _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {innerEx.Message}");
                    return new APIResponse(Utils.StatusCode.Error, null, (int)ErrorCode.DbUpdateException, innerEx.Message);
                }
                else
                {
                    _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                    return new APIResponse(Utils.StatusCode.Error, null, (int)ErrorCode.DbUpdateException, ex.Message);
                }
            }
            _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
            return new APIResponse(Utils.StatusCode.Error, null, (int)ErrorCode.InternalServerError, ex.Message);
        }

        [NonAction]
        public IActionResult OkExceptionStatusCode(Exception ex)
        {
            var request = Request;
            if (request == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
            string errStr = $"Path = {request.Path}, Query = {JsonSerializer.Serialize(request.Query)}, ";
            var claims = HttpContext.User.Identity != null ? HttpContext.User.Identity as ClaimsIdentity : null;
            if (claims != null)
            {
                var userId = claims.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                errStr += $"userId = {userId}, ";
            }

            var fex = ex as FaultException;
            if (fex != null)
            {
                if (int.Parse(fex.Code.Name) <= 1000)
                {
                    _logger?.LogError(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = fex.Message });
                }
                else
                {
                    _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = fex.Message });
                }
            }

            var httpRequestEx = ex as HttpRequestException;
            if (httpRequestEx != null)
            {
                _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new { message = httpRequestEx.Message });
            }

            var jsonEx = ex as JsonException;
            if (jsonEx != null)
            {
                _logger?.LogInformation(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }

            _logger?.LogError(ex, $"{ex.GetType()}: {errStr}, Message = {ex.Message}");
            return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
        }

        [NonAction]
        protected FileContentResult FileByFormat(byte[] fileByte, string fileName)
        {
            string ext = Path.GetExtension(fileName)?.ToLower();

            return ext switch
            {
                ".jpg" or ".jpeg" or ".jfif" => File(fileByte, MimeTypeNames.ImageJpeg),
                ".png" => File(fileByte, MimeTypeNames.ImagePng),
                ".svg" => File(fileByte, MimeTypeNames.ImageSvgXml),
                ".gif" => File(fileByte, MimeTypeNames.ImageGif),
                ".mp4" => File(fileByte, MimeTypeNames.VideoMp4),
                ".pdf" => File(fileByte, MimeTypeNames.ApplicationPdf),
                _ => File(fileByte, MimeTypeNames.ApplicationOctetStream, fileName),
            };
        }
    }
}
