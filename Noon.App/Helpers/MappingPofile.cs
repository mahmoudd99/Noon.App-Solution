using AutoMapper;
using Noon.App.Dtos;
using Noon.Core.Entities.BasketModule;
using Noon.Core.Entities.IdentityModule;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Entities.PruductModule;

namespace Noon.App.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(D => D.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(D => D.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureResolverUrl>());
            CreateMap<Noon.Core.Entities.IdentityModule.Address, AddressDto>().ReverseMap();

            CreateMap<AddressDto, Noon.Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(O => O.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(O => O.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(O => O.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(O => O.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(O => O.Product.PictureUrl))
                .ForMember(D=>D.PictureUrl,O=>O.MapFrom<OrderItemPictureResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();




        }
    }
}
