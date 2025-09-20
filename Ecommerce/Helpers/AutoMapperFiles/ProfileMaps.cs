using AutoMapper;
using Ecommerce.Dtos.User.Profile;
using Ecommerce.Models.User;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class ProfileMaps : Profile
    {
        public ProfileMaps()
        {
            CreateMap<ApplicationUser, UserProfileDto>()
     .ForMember(dest => dest.FavoriteId, opt => opt.MapFrom(src => src.FaveoriteId))
     .ForMember(dest => dest.Favorite, opt => opt.MapFrom(src => src.Faveorites))
     .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
     .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart))
     .ForMember(dest => dest.CheckOutId, opt => opt.MapFrom(src => src.CheckOutId))
     .ForMember(dest => dest.CheckOuts, opt => opt.MapFrom(src => src.CheckOuts))
     .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.Orders))
     .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
     .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings))
     .ForMember(dest => dest.Message, opt => opt.Ignore());

            CreateMap<UpdateProfileDto, ApplicationUser>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
