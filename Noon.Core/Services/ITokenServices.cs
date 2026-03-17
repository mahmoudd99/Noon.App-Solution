using Microsoft.AspNetCore.Identity;
using Noon.Core.Entities.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Services
{
    public interface ITokenServices
    {

        Task<string> CreateTokenAsync(AppUser user ,UserManager<AppUser> userManager);
    }
}
