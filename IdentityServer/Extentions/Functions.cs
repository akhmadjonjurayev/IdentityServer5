namespace IdentityServer5.Extentions
{
    public static class Functions
    {
        public static Duende.IdentityServer.Models.Client ConvertClientToEntities(Duende.IdentityServer.EntityFramework.Entities.Client client)
        {
            Duende.IdentityServer.Models.Client client_map = new Duende.IdentityServer.Models.Client
            {
                ClientId = client.ClientId,
                ClientUri = client.ClientUri,
                ClientName = client.ClientName,
                RedirectUris = client.RedirectUris.Select(l => l.RedirectUri).ToList(),
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(l => l.PostLogoutRedirectUri).ToList(),
                AllowedCorsOrigins = client.AllowedCorsOrigins.Select(l => l.Origin).ToList(),
                AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
                AllowedScopes = client.AllowedScopes.Select(l => l.Scope).ToList(),
                AllowedGrantTypes = client.AllowedGrantTypes.Select(l => l.GrantType).ToList(),
                RequireClientSecret = client.RequireClientSecret,
                RequireConsent = client.RequireConsent,
                AccessTokenLifetime = client.AccessTokenLifetime
            };

            return client_map;
        }

        public static Duende.IdentityServer.Models.ApiResource ConvertApiResourceToEntities(Duende.IdentityServer.EntityFramework.Entities.ApiResource apiResource)
        {
            Duende.IdentityServer.Models.ApiResource apiResource_map = new Duende.IdentityServer.Models.ApiResource
            {
                ApiSecrets = apiResource.Secrets?.Select(l => new Duende.IdentityServer.Models.Secret
                {
                    Description = l.Description,
                    Expiration = l.Expiration,
                    Type = l.Type,
                    Value = l.Value
                }).ToList(),
                Description = apiResource.Description,
                DisplayName = apiResource.DisplayName,
                Enabled = apiResource.Enabled,
                Name = apiResource.Name,
                Scopes = apiResource.Scopes?.Select(l => l.Scope).ToList()
            };
            if(!string.IsNullOrEmpty(apiResource.AllowedAccessTokenSigningAlgorithms))
                apiResource_map.AllowedAccessTokenSigningAlgorithms = new List<string> { apiResource.AllowedAccessTokenSigningAlgorithms };

            return apiResource_map;
        }

        public static Duende.IdentityServer.Models.ApiScope ConvertApiScopeToEntities(Duende.IdentityServer.EntityFramework.Entities.ApiScope apiScope)
        {
            Duende.IdentityServer.Models.ApiScope apiScope_map = new Duende.IdentityServer.Models.ApiScope
            {
                Description = apiScope.Description,
                DisplayName = apiScope.DisplayName,
                Emphasize = apiScope.Emphasize,
                Enabled = apiScope.Enabled,
                Name = apiScope.Name,
                Required = apiScope.Required,
                ShowInDiscoveryDocument = apiScope.ShowInDiscoveryDocument
            };
            return apiScope_map;
        }

        public static Duende.IdentityServer.Models.IdentityResource ConvertIdentityResourceToEntities(Duende.IdentityServer.EntityFramework.Entities.IdentityResource identityResource)
        {
            Duende.IdentityServer.Models.IdentityResource identityResource_map = new Duende.IdentityServer.Models.IdentityResource
            {
                Description = identityResource.Description,
                DisplayName = identityResource.DisplayName,
                Emphasize = identityResource.Emphasize,
                Enabled = identityResource.Enabled,
                Name = identityResource.Name,
                Required = identityResource.Required,
                ShowInDiscoveryDocument = identityResource.ShowInDiscoveryDocument
            };
            return identityResource_map;
        }
    }
}
