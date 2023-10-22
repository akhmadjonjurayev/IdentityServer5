using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using IdentityServer5.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            if (_configurationDbContext.Database.GetPendingMigrations().Any())
                _configurationDbContext.Database.Migrate();

            if (_identityDb.Database.GetPendingMigrations().Any())
                _identityDb.Database.Migrate();
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
                    ClientUri = "http://localhost:5000/wwwroot/srv",
                    ClientId = "Kalinus",
                    ClientName = "Kalinus",
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret
                        {
                            Value = "akhmadjonjurayev_duendeidentityserver".Sha256(),
                            Type = IdentityServerConstants.SecretTypes.SharedSecret
                        }
                    },
                    RedirectUris = new List<ClientRedirectUri>
                    {
                        new ClientRedirectUri
                        {
                            RedirectUri = "http://localhost:5000/wwwroot/srv/callback"
                        },
                        new ClientRedirectUri
                        {
                            RedirectUri = "http://localhost:5000/wwwroot/srv/silentrenew"
                        }
                    },
                    PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                    {
                        new ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = "http://localhost:5000/wwwroot/srv"
                        },
                        new ClientPostLogoutRedirectUri
                        {
                            PostLogoutRedirectUri = "http://localhost:5000/wwwroot/srv/logout"
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
                    RequireClientSecret = false,
                    RequireConsent = false,
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
