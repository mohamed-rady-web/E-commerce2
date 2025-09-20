using AutoMapper;
using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;

namespace Ecommerce.Helpers.AutoMapperFiles
{
    public class CategoryMap : Profile
    {
        public CategoryMap()
        {
            CreateMap<AddCategoryDto, CategoryModel>();
            CreateMap<UpdateCategoryDto, CategoryModel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CategoryModel, CategoryDto>();

        }
    }
}
