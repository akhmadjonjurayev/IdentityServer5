using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using IdentityServer5.Extentions;
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

            return Task.FromResult(Functions.ConvertClientToEntities(client));
        }
    }
}