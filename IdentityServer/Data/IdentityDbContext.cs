using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Models;
using IdentityServer5.Models;

public class IdentityDb : IdentityDbContext<User, UserRole, Guid>
{
    public IdentityDb(DbContextOptions<IdentityDb> option) : base(option)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.HasDefaultSchema("duende");
        base.OnModelCreating(builder);
    }

    public override DbSet<User> Users {get;set;}
}