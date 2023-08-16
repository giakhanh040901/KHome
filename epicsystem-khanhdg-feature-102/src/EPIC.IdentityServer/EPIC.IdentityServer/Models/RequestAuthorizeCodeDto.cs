using Microsoft.AspNetCore.Mvc;

namespace EPIC.IdentityServer.Models
{
    public class RequestAuthorizeCodeDto
    {
        [FromQuery(Name = "client_id")]
        public string ClientId { get; set; }

        [FromQuery(Name = "redirect_uri")]
        public string RedirectUri { get; set; }

        [FromQuery(Name = "response_type")]
        public string ResponseType { get; set; }

        [FromQuery(Name = "scope")]
        public string Scope { get; set; }

        [FromQuery(Name = "code")]
        public string Code { get; set; }

        [FromQuery(Name = "code_challenge")]
        public string CodeChallenge { get; set; }
    }
}
