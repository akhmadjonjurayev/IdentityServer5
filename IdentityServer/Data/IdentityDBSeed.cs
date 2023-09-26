using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using IdentityServer5.Models;
using Microsoft.AspNetCore.Identity;
using static Duende.IdentityServer.IdentityServerConstants;

namespace IdentityServer5.Data
{
    public class IdentityDBSeed
    {
        private IdentityDb _identityDb;
        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        private ConfigurationDbContext _configurationDbContext;
        public IdentityDBSeed(IdentityDb identityDb, UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            ConfigurationDbContext configurationDbContext)
        {
            this._identityDb = identityDb;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configurationDbContext = configurationDbContext;
        }

        public async Task SeedData()
        {
            if(!_identityDb.Users.Any())
            {
                var role = new UserRole
                {
                    Name = "administrator"
                };
                await _roleManager.CreateAsync(role);

                var user = new User
                {
                    UserName = "kalinus",
                    Email = "kalinus2775@gmail.com"
                };
                await _userManager.CreateAsync(user, "1");

                await _userManager.AddToRoleAsync(user, role.Name);
            }

            if(!_configurationDbContext.Clients.Any())
            {
                var identityResources = new List<Duende.IdentityServer.Models.IdentityResource>
                {
                    new IdentityResources.Address(),
                    new IdentityResources.Email(),
                    new IdentityResources.OpenId(),
                    new IdentityResources.Phone(),
                    new IdentityResources.Profile()
                };

                foreach(var resource in identityResources)
                {
                    var item = new Duende.IdentityServer.EntityFramework.Entities.IdentityResource();
                    item.Name = resource.Name;
                    item.Enabled = resource.Enabled;
                    item.Description = resource.Description;
                    item.ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;
                    item.Required = resource.Required;
                    await _configurationDbContext.IdentityResources.AddAsync(item);
                }

                var apis = new Dictionary<string, string>
                {
                    { "ApiGateway", "Api Gateway" }
                };

                foreach(var api in apis)
                {
                    var apiResource = new Duende.IdentityServer.EntityFramework.Entities.ApiResource()
                    {
                        Name = api.Key,
                        DisplayName = api.Value
                    };
                    _configurationDbContext.ApiResources.Add(apiResource);

                    var apiScope = new Duende.IdentityServer.EntityFramework.Entities.ApiScope
                    {
                        Name = api.Key,
                        DisplayName= api.Value
                    };
                    _configurationDbContext.ApiScopes.Add(apiScope);
                }

                var client = new Duende.IdentityServer.EntityFramework.Entities.Client
                {
                    ClientUri = "http://localhost",
                    ClientId = "Kalinus",
                    ClientName = "Kalinus",
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret
                        {
                            Value = "akhmadjonjurayev_duendeidentityserver"
                        }
                    },
                    RedirectUris = new List<ClientRedirectUri>
                    {
                        new ClientRedirectUri
                        {
                            RedirectUri = "http://localhost:5000/callback"
                        },
                        new ClientRedirectUri
                        {
                            RedirectUri = "http://localhost:5000/silentrenew"
                        }
                    },
                    PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                    {
                        new ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = "http://localhost:5000"
                        },
                        new ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = "http://localhost:5000/logout"
                        }
                    },
                    AllowedCorsOrigins = new List<ClientCorsOrigin>
                    {
                        new ClientCorsOrigin
                        {
                            Origin = "http://localhost:5000"
                        }
                    },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = new List<ClientScope>
                    {
                        new ClientScope
                        {
                            Scope = StandardScopes.OfflineAccess
                        },
                        new ClientScope
                        {
                            Scope = StandardScopes.Profile
                        },
                        new ClientScope
                        {
                            Scope = StandardScopes.Phone
                        },
                        new ClientScope
                        {
                            Scope = StandardScopes.Email
                        },
                        new ClientScope
                        {
                            Scope = StandardScopes.Address
                        },
                        new ClientScope
                        {
                            Scope = "ApiGateway"
                        }
                    },
                    AllowedGrantTypes = new List<ClientGrantType>
                    {
                        new ClientGrantType
                        {
                            GrantType = GrantType.AuthorizationCode
                        },
                        new ClientGrantType
                        {
                            GrantType = GrantType.ClientCredentials
                        },
                        new ClientGrantType
                        {
                            GrantType = GrantType.ResourceOwnerPassword
                        },
                        new ClientGrantType
                        {
                            GrantType = "refresh_token"
                        },
                    },
                    RequireClientSecret = true,
                    RequireConsent = true,
                    RefreshTokenUsage = 1,
                    RefreshTokenExpiration = 3600,
                    AbsoluteRefreshTokenLifetime = 3600,
                    AccessTokenLifetime = 3600
                };
                _configurationDbContext.Clients.Add(client);

                _configurationDbContext.SaveChanges();
            }
        }
    }
}
