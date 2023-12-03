using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServer5.Data;
using IdentityServer5.Extentions;
using IdentityServer5.Service;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IdentityDBSeed));
builder.Services.AddScoped(typeof(UserService));

builder.Services.AddCors(l =>
{
    l.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:7854")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<IdentityDb>(option => {
   option.UseNpgsql(builder.Configuration.GetConnectionString("MyConnection"), l =>
   {
       l.MigrationsAssembly(migrationAssembly);
   });
});

builder.ConfigureIdentityServer(builder.Configuration);

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.None,
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();

app.UseCors();

app.UseStaticFiles("/wwwroot");

app.UseIdentityServer();

app.UseSeedData();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.Run();
