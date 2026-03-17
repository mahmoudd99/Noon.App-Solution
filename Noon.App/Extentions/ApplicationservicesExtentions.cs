using Noon.App.Helpers;
using Noon.Core;
using Noon.Core.Repositories;
using Noon.Core.Services;
using Noon.Repository;
using Noon.Services;

namespace Noon.App.Extentions
{
    public static class ApplicationservicesExtentions
    {
        public static IServiceCollection AddApplicationServices( this IServiceCollection services)
        {
            
            //services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
           
          
            services.AddAutoMapper(typeof(MappingProfile));


            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
           
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            return services;

        }
    }
}
