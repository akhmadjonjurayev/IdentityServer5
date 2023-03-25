using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Models;

public class IdentityDb : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public IdentityDb(DbContextOptions<IdentityDb> option) : base(option)
    {

    }

    public override DbSet<User> Users {get;set;}
}