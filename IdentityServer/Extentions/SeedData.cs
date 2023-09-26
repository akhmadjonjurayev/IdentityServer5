using IdentityServer5.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityServer5.Extentions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SeedData
    {
        private readonly RequestDelegate _next;

        public SeedData(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IdentityDBSeed identityDBSeed)
        {
            await identityDBSeed.SeedData();
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SeedDataExtensions
    {
        public static IApplicationBuilder UseSeedData(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedData>();
        }
    }
}
