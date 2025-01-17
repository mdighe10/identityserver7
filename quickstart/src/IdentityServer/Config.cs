﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "verification",
                UserClaims =  new List<string> 
                { 
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }  
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { 
                 new ApiScope(name: "Api1", displayName: "My API") 
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            {
        new Client
        {
            ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "Api1" }
        },
        new Client
        {
            ClientId = "web",
            ClientSecrets = { new Secret("secret".Sha256()) },
            //ClientId = "I1h0ohDPHMxTqaJ2l1JK9dXHYsnfYMUKPAZYb6tv_HY",
            //ClientSecrets = { new Secret("L6Db6EIpALIUVBKWNXri56UBRXjyly0hDllfXmahRgc") },

            AllowedGrantTypes = GrantTypes.Code,
            
            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc" },
            
             AllowOfflineAccess = true,

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "verification",
                "Api1",
            }
        }

    };
}