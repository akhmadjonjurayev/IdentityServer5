using IdentityServer5.Models;
using IdentityServer5.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer5.Service
{
    public class UserService
    {
        private readonly string _userRole = "user";
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IdentityDb _identityDbContext;
        public UserService(RoleManager<UserRole> roleManager, UserManager<User> userManager, IdentityDb identityDbContext)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._identityDbContext = identityDbContext;
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

        public async Task<JsonResponse> UpdateUserAsync(UserViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.UserName))
                return JsonResponse.ErrorResponse("error-invalid-data");

            var user = await _userManager.FindByNameAsync(viewModel.UserName);
            if (user == null)
                return JsonResponse.ErrorResponse("error-not-found-data");

            user.UserName = viewModel.UserName;
            user.Email = viewModel.Email;

            var identityResult = await _userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
                return JsonResponse.SuccessResponce("success-update-data");
            return JsonResponse.ErrorResponse("error-update-data");
        }

        public async Task<JsonResponse> DeleteUser(Guid userId)
        {
            var user = await _identityDbContext.Users.FirstOrDefaultAsync(us => us.Id == userId);
            if (user == null)
                return JsonResponse.ErrorResponse("error-not-found-data");

            var identityResult = await _userManager.DeleteAsync(user);
            if (identityResult.Succeeded)
                return JsonResponse.SuccessResponce("success-delete-data");
            return JsonResponse.ErrorResponse("error-delete-data");
        }
    }
}
