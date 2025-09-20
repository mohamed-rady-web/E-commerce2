using AutoMapper;
using Ecommerce.Dtos.Cart;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Models.Cart;
using Ecommerce.Models.User.FavAndBooking;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class CartMap : Profile
    {
        public CartMap()
        {
            CreateMap<CartModel, CartDto>()
     .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id));
            CreateMap<FaveoriteModel, FavDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
