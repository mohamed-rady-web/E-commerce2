using AutoMapper;
using Ecommerce.Dtos.Products;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.Product;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class ProductsMap : Profile
    {
        public ProductsMap()
        {
            CreateMap<AddProductDto, ProductModel>();
            CreateMap<UpdateProductDto, ProductModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProductModel, ProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.SpecialOfferName, opt => opt.MapFrom(src => src.SpecialOffer != null ? src.SpecialOffer.Title : null))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? src.Reviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews.Select(r => new ReviewsDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    Email = r.User.Email,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    Productname = src.Name
                }).ToList()));
        }
    }
    }
