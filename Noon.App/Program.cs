using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noon.App.Errors;
using Noon.App.Extentions;
using Noon.App.Helpers;
using Noon.Core.Entities.IdentityModule;
using Noon.Core.Repositories;
using Noon.Repository;
using Noon.Repository.Data;
using Noon.Repository.Identity;
using StackExchange.Redis;
using System.Xml.Linq;

namespace Noon.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            builder.Services.AddControllers();
            builder.Services.AddSwaggerServices();

            builder.Services.AddApplicationServices();
           
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Default-connection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Identity-Connection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connectionstr = builder.Configuration.GetConnectionString("Redis");
                return  ConnectionMultiplexer.Connect(connectionstr);
            });

            builder.Services.AddIdetityServices(builder.Configuration);


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(E => E.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(P => P.ErrorMessage)
                                                         .ToArray();

                    var validationApiResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationApiResponse);


                };

            });

            builder.Services.AddCors(options =>
            {

                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });

            });




            #endregion

            var app = builder.Build();
             
            #region Migration and Dataseeding
            var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>();
                await dbContext.Database.MigrateAsync(); //Apply Migration
                await StoreContextSeed.SeedAsync(dbContext);

                
                var identityDbContext=Services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync(); //Applay Migration
               
                var userManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                //log Exception
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "error");
            }

            #endregion


            #region Configure KestrelService
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare();
            }


            app.UseHttpsRedirection();
            
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion


            app.Run();



        }
    }
}