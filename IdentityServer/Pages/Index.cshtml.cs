using System.Reflection;
using IdentityServer5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.Home;

[AllowAnonymous]
public class Index : PageModel
{
    public string Version;
    private readonly SignInManager<User> signInManager;

    public Index(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }    
    public IActionResult OnGet()
    {
        if (!signInManager.IsSignedIn(User))
            return Redirect("~/Account/Login");
        Version = typeof(Duende.IdentityServer.Hosting.IdentityServerMiddleware).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+').First();
        return Page();
    }
}