using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace IdentityServer5.Data
{
    public class ClientStore : IClientStore
    {
        private IdentityDb _identityDb;
        private readonly ConfigurationDbContext _configurationDbContext;
        public ClientStore(IdentityDb identityDb, ConfigurationDbContext configurationDbContext)
        {
            this._identityDb = identityDb;
            this._configurationDbContext = configurationDbContext;
        }
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _configurationDbContext.Clients.FirstOrDefault(l => l.ClientId == clientId);
            if (client is null)
                throw new Exception("error-not-found-data");
            return Task.FromResult(client as Client);
        }
    }
}