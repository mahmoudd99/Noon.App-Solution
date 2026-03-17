namespace Noon.App.Extentions
{
    public static class ISwaggerExtentions
    {

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {



            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;


        }

        public static WebApplication UseSwaggerMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
