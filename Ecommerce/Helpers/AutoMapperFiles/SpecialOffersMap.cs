using AutoMapper;
using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class SpecialOffersMap: Profile
    {
        public SpecialOffersMap()
        {
            CreateMap<AddSpecialOfferDto, SpecialOffersModel>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));


            CreateMap<UpdateSpecialOfferDto, SpecialOffersModel>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

    
            CreateMap<SpecialOffersModel, SpecialOfferDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            
            CreateMap<ProductModel, ProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SpecialOfferName, opt => opt.MapFrom(src => src.SpecialOffer.Title));

            CreateMap<ProductDto, ProductModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.SpecialOffer, opt => opt.Ignore());
        }
    }
}
