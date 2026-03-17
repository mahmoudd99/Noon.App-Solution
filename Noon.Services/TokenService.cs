using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Noon.Core.Entities.IdentityModule;
using Noon.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Services
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //Generate Token

            // 1. private Claims[User-Defined]

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.DisplayName),
                new Claim(ClaimTypes.Email , user.Email)
            };


            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            // 2. signature [ Key ]

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

            // 3. Register Claims 
            // 4. header

            // 5.   //Generate (object ) Token


            var token = new JwtSecurityToken(

            //Register Claims  : for all user
               issuer: _config["JWT:Validissure"],
               audience: _config["JWT:ValidAudience"],
               expires: DateTime.Now.AddDays(double.Parse(_config["JWT:DurationInDays"])),

                // private Claims : for many users
                // signature[Key]
                claims: authClaims,
                // header
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);




        }
    }
}
