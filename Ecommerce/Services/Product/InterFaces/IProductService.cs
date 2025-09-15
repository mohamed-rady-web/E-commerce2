using Ecommerce.Dtos.Products;

namespace Ecommerce.Services.Product.InterFaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetAllProductsAsync();
        public Task<ProductDto> GetProductByIdAsync(int productId);
        public Task<ProductDto> CreateProductAsync(AddProductDto dto);
        public Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto dto);
        public Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        public Task<List<ProductDto>> GetRelatedProductsAsync(int productId);
        public Task<List<ProductDto>> GetProductsBySpecialOfferAsync(int specialOfferId);
        public Task<List<ProductDto>> ExploreOurProductsAsync();
        public Task<ProductDto> DeleteProductAsync(int productId);


    }
}
