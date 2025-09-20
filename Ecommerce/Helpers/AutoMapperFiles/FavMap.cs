using AutoMapper;
using Ecommerce.Dtos.User.Fav;
using Ecommerce.Models.User.FavAndBooking;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class FavMap : Profile
    {
        public FavMap()
        {
            CreateMap<AddtoFavDto, FaveoriteItemModel>();

            CreateMap<FaveoriteItemModel, FavItemDto>()
                .ForMember(dest => dest.FavItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrls))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<FaveoriteModel, FavDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
