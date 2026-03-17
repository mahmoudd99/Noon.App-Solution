using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Noon.Core.Entities.IdentityModule;
using Noon.Core.Services;
using Noon.Repository.Identity;
using Noon.Services;
using System.Text;

namespace Noon.App.Extentions
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdetityServices(this IServiceCollection services ,IConfiguration configuration)
        {

            services.AddScoped<ITokenServices,TokenService>();
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit=true;
                options.Password.RequireUppercase=true;
                options.Password.RequireLowercase=true;
                options.Password.RequireNonAlphanumeric=true;
            })
                     .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddAuthentication(Options=>
            {
                Options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(Options =>
                {
                    Options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Validissure"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

                    };


                }); ;

            return services;
        }
    }
}
