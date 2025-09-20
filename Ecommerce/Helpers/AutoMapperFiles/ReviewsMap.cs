using AutoMapper;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.User.AboutAndContact;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class ReviewsMap : Profile
    {
        public ReviewsMap()
        {
            CreateMap<AddReviewDto, ReviewsModel>();
            CreateMap<UpdateReviewDto, ReviewsModel>();
            CreateMap<ReviewsModel, ReviewsDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Productname, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
