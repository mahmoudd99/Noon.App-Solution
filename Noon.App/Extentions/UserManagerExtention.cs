using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noon.Core.Entities.IdentityModule;
using System.Security.Claims;

namespace Noon.App.Extentions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser?> FindUserWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal Usser)
        {


            var email = Usser.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            return user;
        }
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal Usser)
        {


            var email = Usser.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);

            return user;
        }
    }
}
