using IdentityServer5.Models;
using IdentityServer5.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer5.Service
{
    public class UserService
    {
        private readonly string _userRole = "user";
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        public UserService(RoleManager<UserRole> roleManager, UserManager<User> userManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<JsonResponse> CreateUserAsync(UserViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.UserName) || string.IsNullOrEmpty(viewModel.Password))
                return JsonResponse.ErrorResponse("error-invalid-data");

            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user != null)
                return JsonResponse.ErrorResponse("error-duplicate-data");

            var newUser = new User
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email
            };

            var userAddResult = await _userManager.CreateAsync(newUser, viewModel.Password);
            if (!userAddResult.Succeeded)
                return JsonResponse.ErrorResponse("error-add-data");

            var roleAddResult = await _userManager.AddToRoleAsync(newUser, _userRole);
            if (roleAddResult.Succeeded)
                return JsonResponse.DataResponse(newUser.Id);
            return JsonResponse.ErrorResponse("error-add-data");
        }
    }
}
