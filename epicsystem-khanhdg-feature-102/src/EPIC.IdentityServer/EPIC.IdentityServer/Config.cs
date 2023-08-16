using IdentityServer4.Models;
using System.Collections.Generic;

namespace EPIC.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes { get; set; }

        public static IEnumerable<Client> Clients { get; set; }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
    }
}
