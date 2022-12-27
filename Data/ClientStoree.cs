using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer5.Data
{
    public class ClientStore : IClientStore
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ClientStore(ConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _configurationDbContext.Clients
                .Include(l => l.AllowedCorsOrigins)
                .Include(l => l.AllowedGrantTypes)
                .Include(l => l.AllowedScopes)
                .Include(l => l.RedirectUris)
                .Include(l => l.PostLogoutRedirectUris)
                .FirstOrDefault(l => l.ClientId == clientId);
            if (client is null)
                throw new Exception("error-not-found-data");

            Client client_map = new Client
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

            return Task.FromResult(client_map);
        }
    }
}