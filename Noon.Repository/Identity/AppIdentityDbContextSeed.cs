using Microsoft.AspNetCore.Identity;
using Noon.Core.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "MahmoudAmer",
                    Email = "mahmoudamer@gmail.com",
                    UserName = "mahmoud.amer",
                    PhoneNumber = "01033445566" 

                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }

        }
    }
}
