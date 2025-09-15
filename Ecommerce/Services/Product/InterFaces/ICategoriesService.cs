using Ecommerce.Dtos.Products;


namespace Ecommerce.Services.Product.InterFaces
{
    public interface ICategoriesService
    {
        public Task<List<CategoryDto>> GetAllCategoriesAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
        public Task<CategoryDto> AddCategoryAsync(AddCategoryDto dto);
        public Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryDto dto);
        public Task<CategoryDto> DeleteCategoryAsync(int categoryId);
        public Task<CategoryDto> GetCategoryByNameAsync(string name);

    }
}
