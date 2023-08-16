using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityServer.Models;
using EPIC.Utils.Controllers;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Controllers
{
    [Route("connect")]
    [ApiController]
    public class AuthorizationController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserServices _userServices;
        private readonly IdentityServerTools _identityServerTools;

        public AuthorizationController(ILogger<AuthorizationController> logger,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IUserServices userServices,
            IdentityServerTools identityServerTools)
        {
            _logger = logger;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _userServices = userServices;
            _identityServerTools = identityServerTools;
        }

        [Authorize]
        [HttpGet("authorize1")]
        public async Task AuthorizeAsync([FromQuery] RequestAuthorizeCodeDto input)
        {
            var subjectId = HttpContext.User.Identity.GetSubjectId();

            var request = new TokenCreationRequest
            {
                Subject = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(JwtClaimTypes.Subject, subjectId),
                })),
                ValidatedResources = new IdentityServer4.Validation.ResourceValidationResult(),
            };

            // Create the token using the TokenService class
            //var token = await _tokenService.CreateAccessTokenAsync(request);

            var token = await _identityServerTools.IssueJwtAsync(
                30000,
                new Claim[]
                {
                    new Claim(JwtClaimTypes.Subject, subjectId),
                }
            );

            //var refreshToken = _refreshTokenService.CreateRefreshTokenAsync(,new Token);
        }
    }
}
