using System;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("mango","Mango Server"),
                new ApiScope(name:"read", displayName:"Read your data"),
                new ApiScope(name:"write",displayName:"Write your data"),
                new ApiScope(name:"delete",displayName:"Delete your data")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // Below is how we make generic client
                new Client
                {
                    ClientId="client",
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read","write","profile"}
                },
                // Customized client
                new Client
                {
                    ClientId="mango",
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mango"
                    },
                    //RedirectUris={ "https://localhost:44360/signin-oidc" },
                    //RedirectUris={ "https://localhost:999/signin-oidc", "https://localhost:44360/signin-oidc", "https://localhost:7284/signin-oidc" },
                    RedirectUris = { "https://localhost:7044/signin-oidc", "https://localhost:44360/signin-oidc" },
                    PostLogoutRedirectUris={ "https://localhost:7044/signout-callback-oidc" }
                }
            };
    }
}

