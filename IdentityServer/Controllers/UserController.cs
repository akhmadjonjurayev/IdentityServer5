using IdentityServer5.Models;
using IdentityServer5.Service;
using IdentityServer5.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer5.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        public async Task<JsonResponse> CreateUser([FromBody] UserViewModel viewModel)
        {
            return await _userService.CreateUserAsync(viewModel);
        }
    }
}
