using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIS.Helpers
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDTO>().ForMember(d => d.Brand, O => O.MapFrom(p => p.Brand.Name))
                                        .ForMember(d => d.Category, O => O.MapFrom(p => p.Category.Name))
                                        .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasket , CustomerBasketDTO>().ReverseMap();

            CreateMap<BasketItem , BasketItemDTO>().ReverseMap();

            CreateMap<AddressDTO , Address>().ReverseMap();

            CreateMap< Talabat.Core.Entities.Identity.Address, AddressIdentityDTO>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                     .ForMember(d => d.DeliveryMethod, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                     .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(o => o.DeliveryMethod.Cost));
                     //.ForMember(d => d.Total, o => o.MapFrom(o => o.GetTotal()));

            CreateMap<OrderItem, OrderItemDTO>()
                     .ForMember(d => d.ProductName, oi => oi.MapFrom(oi => oi.Product.ProductName))
                     .ForMember(d => d.ProductId, oi => oi.MapFrom(oi => oi.Product.ProductId))
                     .ForMember(d => d.ProductURL, oi => oi.MapFrom(oi => oi.Product.ProductURL))
                     .ForMember(d=>d.ProductURL , oi=>oi.MapFrom<OrderItemPictureUrlResolver>());
                     
        }
    }
}
