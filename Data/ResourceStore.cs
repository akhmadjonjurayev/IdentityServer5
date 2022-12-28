using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using IdentityServer5.Extentions;

namespace IdentityServer5.Data
{
    public class ResourceStore : IResourceStore
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        public ResourceStore(ConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }
        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var apiResourceEntities = _configurationDbContext.ApiResources.Where(l => apiResourceNames.Contains(l.Name)).ToList();
            var apiResourceMap = apiResourceEntities.Select(l => Functions.ConvertApiResourceToEntities(l));
            return Task.FromResult(apiResourceMap);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var apiResourceEntities = _configurationDbContext.ApiResources.Where(l => l.Scopes.Any(k => scopeNames.Contains(k.Scope))).ToList();
            var apiResourceMap = apiResourceEntities.Select(l => Functions.ConvertApiResourceToEntities(l));
            return Task.FromResult(apiResourceMap);
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var apiScopesEntities = _configurationDbContext.ApiScopes.Where(l => scopeNames.Contains(l.Name)).ToList();
            var apiScopeMap = apiScopesEntities.Select(l => Functions.ConvertApiScopeToEntities(l));
            return Task.FromResult(apiScopeMap);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResourceEntities = _configurationDbContext.IdentityResources.Where(l => scopeNames.Contains(l.Name)).ToList();
            var identityResourceMap = identityResourceEntities.Select(l => Functions.ConvertIdentityResourceToEntities(l));
            return Task.FromResult(identityResourceMap);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResourceEntities = _configurationDbContext.ApiResources.ToList();
            var apiResourceMap = apiResourceEntities.Select(l => Functions.ConvertApiResourceToEntities(l));

            var apiScopesEntities = _configurationDbContext.ApiScopes.ToList();
            var apiScopeMap = apiScopesEntities.Select(l => Functions.ConvertApiScopeToEntities(l));

            var identityResourceEntities = _configurationDbContext.IdentityResources.ToList();
            var identityResourceMap = identityResourceEntities.Select(l => Functions.ConvertIdentityResourceToEntities(l));

            var resources = new Resources(identityResourceMap, apiResourceMap, apiScopeMap);
            return Task.FromResult(resources);
        }
    }
}
