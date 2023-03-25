using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer5.Data
{
    public class ProfileService : IProfileService
    {
        private readonly IdentityDb _db;
        private IUserClaimsPrincipalFactory<User> _claimsFactory;
        public ProfileService(IdentityDb db, IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            this._claimsFactory = claimsFactory;
            this._db = db;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _db.Users.FirstOrDefaultAsync(l => l.Id.ToString() == userId);
            if (user is null)
                throw new Exception("error-not-found-data");
            var factory = await _claimsFactory.CreateAsync(user);
            context.IssuedClaims = factory.Claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _db.Users.FirstOrDefaultAsync(l => l.Id.ToString() == userId);
            if (user is null)
            {
                context.IsActive = false;
                return;
            }
            context.IsActive = true;
        }
    }
}
