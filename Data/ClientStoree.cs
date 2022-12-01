using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace IdentityServer5.Data
{
    public class ClientStore : IClientStore
    {
        private IdentityDb _identityDb;
        public ClientStore(IdentityDb identityDb)
        {
            this._identityDb = identityDb;
        }
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _identityDb.Clients.FirstOrDefault(l => l.ClientId == clientId);
            if (client is null)
                throw new Exception("error-not-found-data");
            return Task.FromResult(client);
        }
    }
}