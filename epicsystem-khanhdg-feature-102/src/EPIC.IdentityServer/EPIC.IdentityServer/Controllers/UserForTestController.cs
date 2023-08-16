using EPIC.Entities.Dto.User;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserForTestController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserForTestController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("get-ip")]
        public IActionResult GetIp()
        {
            return Ok(new { yourIp = CommonUtils.GetCurrentXForwardedFor(_httpContextAccessor) });
        }

        [HttpGet("get-headers")]
        public IActionResult GetHeaders()
        {
            return Ok(_httpContextAccessor.HttpContext.Request.Headers);
        }

        [HttpGet("get-ip-vpn")]
        public IActionResult GetIpVpn()
        {
            var network = NetworkInterface.GetAllNetworkInterfaces();
            var vpn = network
                .FirstOrDefault(x => x.Name == "VPNConnection");
            if (vpn != null)
            {
                var ip = vpn.GetIPProperties().UnicastAddresses
                    .FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ?.Address.ToString();
                return Ok(ip);
            }
            return Ok(network);
        }

        [HttpPost("login-for-test")]
        public async Task<object> LoginForTest([FromBody] LoginForTestDto input)
        {
            HttpClient client = new HttpClient();
            var dict = new Dictionary<string, string>();
            dict.Add("client_id", "client1");
            dict.Add("client_secret", "secret1");
            dict.Add("grant_type", input.GrantType);
            dict.Add("username", input.Username);
            dict.Add("password", input.Password);
            dict.Add("refresh_token", input.RefreshToken);

            var content = new FormUrlEncodedContent(dict);
            var response = await client.PostAsync("http://localhost:5001/connect/token", content);
            return new
            {
                StatusCode = response.StatusCode,
                Body = await response.Content.ReadAsStringAsync()
            };
        }
    }
}
