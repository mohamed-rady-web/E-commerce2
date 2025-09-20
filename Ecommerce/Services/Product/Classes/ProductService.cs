using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.Products;
using Ecommerce.Dtos.User.Reviews;
using Ecommerce.Models.Product;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Product.Classes
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(AddProductDto dto)
        {
            try
            {
                var existingProduct = await _context.Products.AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower());
                if (existingProduct)
                    throw new Exception("Product with the same name already exists.");

                var product = _mapper.Map<ProductModel>(dto);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<ProductDto>(product);
                result.Message = "Product created successfully";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the product: {ex.Message}");
            }
        }

        public async Task<ProductDto> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                    throw new Exception("Product not found.");

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new ProductDto { Message = "Product deleted successfully" };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the product: {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> ExploreOurProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .OrderBy(p => Guid.NewGuid())
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .Take(8)
                    .ToListAsync();

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products: {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .ToListAsync();

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products: {ex.Message}");
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null) return new ProductDto { Message = "Product not found" };

                var result = _mapper.Map<ProductDto>(product);
                result.Message = "Product retrieved successfully";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product: {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category == null)
                    return new List<ProductDto> { new ProductDto { Message = "Category not found." } };

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .Where(p => p.CategoryId == categoryId)
                    .ToListAsync();

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products by category: {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> GetProductsBySpecialOfferAsync(int specialOfferId)
        {
            try
            {
                var specialOffer = await _context.SpecialOffers.FirstOrDefaultAsync(s => s.Id == specialOfferId);
                if (specialOffer == null)
                    return new List<ProductDto> { new ProductDto { Message = "Special offer not found." } };

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .Where(p => p.SpecialOfferId == specialOfferId)
                    .ToListAsync();

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products by special offer: {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> GetRelatedProductsAsync(int productId)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                    return new List<ProductDto> { new ProductDto { Message = "Product not found." } };

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .Where(p => p.CategoryId == product.CategoryId && p.Id != productId)
                    .Take(5)
                    .ToListAsync();

                return _mapper.Map<List<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving related products: {ex.Message}");
            }
        }

        public async Task<ProductDto> UpdateProductAsync(int productId, UpdateProductDto dto)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.SpecialOffer)
                    .Include(p => p.Reviews).ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                    throw new Exception("Product not found.");

                _mapper.Map(dto, product);

                await _context.SaveChangesAsync();

                var result = _mapper.Map<ProductDto>(product);
                result.Message = "Product updated successfully";
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product: {ex.Message}");
            }
        }
    }
}
