using IdentityServer5.Data;
using IdentityServer5.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer5.Extentions
{
    public static class IdentityServerExtensions
    {
        public static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder app, IConfiguration Configuration)
        {
            app.Services.AddIdentity<User, UserRole>(option =>
            {
                option.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<IdentityDb>();

            app.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;

                options.IssuerUri = Configuration["IdentityUrl"];
            })
             .AddConfigurationStore(opt =>
            {
            opt.ConfigureDbContext = c => c.UseNpgsql(Configuration.GetConnectionString("MyConnection"),
                sql => sql.MigrationsAssembly("__EFMigrationsHistory"));
            })
            .AddAspNetIdentity<User>()
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddProfileService<ProfileService>();

            return app;
        }
    }
}