using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIS.Helpers
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDTO>().ForMember(d => d.Brand, O => O.MapFrom(p => p.Brand.Name))
                                            .ForMember(d => d.Category, O => O.MapFrom(p => p.Category.Name))
                                            .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
        }
    }
}
