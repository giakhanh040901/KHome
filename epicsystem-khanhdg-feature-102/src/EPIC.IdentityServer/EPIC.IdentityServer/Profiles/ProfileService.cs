using EPIC.IdentityDomain.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPIC.IdentityServer.Profiles
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger _logger;
        private readonly IUserServices _userServices;

        public ProfileService(
            ILogger<ProfileService> logger,
            IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject.FindFirstValue("sub");
            if (subject == null)
            {
                _logger.LogError("Subject claim is null");
                return Task.CompletedTask;
            }
            if (!int.TryParse(subject, out int userId))
            {
                _logger.LogError("Cannot parse userId from subject claim");
                return Task.CompletedTask;
            }
            var claims = _userServices.GetClaims(userId);
            context.IssuedClaims.AddRange(claims);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
