using IdentityServer5.Data;

namespace IdentityServer5.Extentions
{
    public static class IdentityServerExtensions
    {
        public static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder app, IConfiguration Configuration)
        {
            app.Services.AddIdentityServer(options => {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;

                options.IssuerUri = Configuration["IdentityUrl"];
            }).AddClientStore<ClientStore>();

            return app;
        }
    }
}