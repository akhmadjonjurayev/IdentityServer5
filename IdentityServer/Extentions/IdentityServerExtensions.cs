using IdentityServer5.Data;
using IdentityServer5.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServer5.Extentions
{
    public static class IdentityServerExtensions
    {
        public static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder app, IConfiguration Configuration)
        {
            var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(Configuration["Certificate:Path"], Configuration["Certificate:Password"]);
            var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            app.Services.AddIdentity<User, UserRole>(option =>
            {
                option.Password.RequiredLength = 1;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<IdentityDb>();

            app.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;
                options.KeyManagement.Enabled = false;
                options.IssuerUri = Configuration["IdentityUrl"];
            })
             .AddConfigurationStore(opt =>
            {
            opt.ConfigureDbContext = c => c.UseNpgsql(Configuration.GetConnectionString("MyConnection"),
                sql => sql.MigrationsAssembly(migrationAssembly));
            })
            .AddAspNetIdentity<User>()
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddProfileService<ProfileService>()
            .AddSigningCredential(certificate);

            return app;
        }
    }
}