using AutoMapper;
using Noon.App.Dtos;
using Noon.Core.Entities.PruductModule;

namespace Noon.App.Helpers
{
    public class ProductPictureResolverUrl : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureResolverUrl(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
                
            return string.Empty;
        }
    }
}
