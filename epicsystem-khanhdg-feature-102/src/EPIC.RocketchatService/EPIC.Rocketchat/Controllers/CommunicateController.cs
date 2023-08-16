using EPIC.RocketchatDomain.Interfaces;
using EPIC.RocketchatEntities.Dto.Callback;
using EPIC.RocketchatEntities.Dto.Integration;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.Rocketchat.Controllers
{
    [Route("api/rocketchat")]
    [ApiController]
    public class CommunicateController : BaseController
    {
        private readonly IRocketChatServices _rocketChatServices;
        private readonly IConfiguration _configuration;

        public CommunicateController(ILogger<CommunicateController> logger, IRocketChatServices rocketChatServices, IConfiguration configuration)
        {
            _rocketChatServices = rocketChatServices;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Đăng nhập vào tài khoản rocketchat và trả về token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("login-token")]
        [Authorize]
        public async Task<APIResponse> LoginSSO()
        {
            try
            {
                var token = await _rocketChatServices.processLoginSSO();

                Response.Cookies.Append("XSRF_Auth", token, new CookieOptions
                {
                    Path = "/",
                    //Domain = "localhost",
                    Expires = DateTime.UtcNow.AddHours(6),
                    HttpOnly = false,
                    //SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    Secure = false,
                });

                return new APIResponse(Utils.StatusCode.Success, null, 200, "success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Trả về token cho rocket chat
        /// </summary>
        /// <returns></returns>
        [HttpGet("sso1")]
        [AllowAnonymous]
        public IActionResult PostSsoPage()
        {
            string token = "";
            bool haveToken = Request.Cookies.TryGetValue("XSRF_Auth", out token);

            if (haveToken)
            {
                return Ok(new
                {
                    token = token,
                });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Trả vê trang login để post token
        /// </summary>
        /// <returns></returns>
        [HttpGet("login")]
        [AllowAnonymous]
        public ContentResult GetLoginPage()
        {

            string token = "";
            bool haveValue = Request.Cookies.TryGetValue("XSRF_Auth", out token);

            string rocketchatUrl = _configuration.GetSection("RocketchatUrl")?.Value;

            return new ContentResult()
            {
                ContentType = "text/html",
                Content = "<script>window.parent.postMessage({"
                          + "event: 'login-with-token',"
                          + "loginToken: '" + token + "'"
                          + "}, '" + rocketchatUrl + "');" +
                          "</script>"
            };
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<APIResponse> LogoutSSO()
        {
            try
            {
                string authToken = "";
                Request.Cookies.TryGetValue("XSRF_Auth", out authToken);

                var result = await _rocketChatServices.LogOutSSO(authToken);

                return new APIResponse(Utils.StatusCode.Success, result, 200, "success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("channel-info")]
        [Authorize]
        public APIResponse GetChannelInfo()
        {
            try
            {
                var result = _rocketChatServices.GetChannelName();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("callback")]
        [AllowAnonymous]
        public async Task<APIResponse> CallbackWebhook([FromBody] WebhookRequestDto dto)
        {
            try
            {
                await _rocketChatServices.ProcessCallbackWebhook(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Investor Gửi tin nhắn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="senderPhone"></param>
        /// <param name="receiverPhone"></param>
        /// <returns></returns>
        [HttpPost("integration")]
        public async Task<APIResponse> IntegrationAllMessage([FromBody] IntegrationDirectDto input)
        {
            try
            {
                await _rocketChatServices.SendDirectMessage(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi request integration
        /// </summary>
        /// <param name="input"></param>
        /// <param name="senderPhone"></param>
        /// <param name="receiverPhone"></param>
        /// <returns></returns>
        [HttpPost("process-integration")]
        [AllowAnonymous]
        public async Task<APIResponse> ProcessIntegration([FromBody] ProcessIntegrationDto input)
        {
            try
            {
                var result = await _rocketChatServices.ProcessIntegration(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
