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

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateProductAsync(AddProductDto dto)
        {
            try
            {
                var existingProduct = await _context.Products.AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower());
                if (existingProduct)
                    throw new Exception("Product with the same name already exists.");

                var product = new ProductModel
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ImageUrl = dto.ImageUrl,
                    Stock = dto.Stock,
                    CategoryId = dto.CategoryId,
                    SpecialOfferId = dto.SpecialOfferId
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return new ProductDto
                {
                    Message = "Product created successfully",
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = 0,
                    CategoryId = product.CategoryId,
                    SpecialOfferId = product.SpecialOfferId,
                    Reviews = new List<ReviewsDto>()
                };
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

                return new ProductDto
                {
                    Message = "Product deleted successfully"
                };
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

                return products.Select(product => new ProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList()
                }).ToList();
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

                return products.Select(product => new ProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList()
                }).ToList();
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

                return new ProductDto
                {
                    Message = "Product retrieved successfully",
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList()
                };
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

                return products.Select(product => new ProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList()
                }).ToList();
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

                return products.Select(product => new ProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList()
                }).ToList();
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

                return products.Select(prod => new ProductDto
                {
                    ProductId = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                    ImageUrl = prod.ImageUrl,
                    Stock = prod.Stock,
                    Rating = prod.Reviews.Any() ? prod.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = prod.CategoryId,
                    CategoryName = prod.Category.Name,
                    SpecialOfferId = prod.SpecialOfferId,
                    SpecialOfferName = prod.SpecialOffer?.Title,
                    Reviews = prod.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = prod.Name
                    }).ToList()
                }).ToList();
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

                var dtoProperties = typeof(UpdateProductDto).GetProperties();
                var productProperties = typeof(ProductModel).GetProperties();

                foreach (var dtoProp in dtoProperties)
                {
                    var value = dtoProp.GetValue(dto);
                    if (value != null)
                    {
                        var productProp = productProperties.FirstOrDefault(p => p.Name == dtoProp.Name);
                        if (productProp != null)
                        {
                            productProp.SetValue(product, value);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return new ProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock,
                    Rating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.Name,
                    SpecialOfferId = product.SpecialOfferId,
                    SpecialOfferName = product.SpecialOffer?.Title,
                    Reviews = product.Reviews.Select(r => new ReviewsDto
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Email = r.User.Email,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        ReviewDate = r.ReviewDate,
                        ProductId = r.ProductId,
                        Productname = product.Name
                    }).ToList(),
                    Message = "Product updated successfully"
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product: {ex.Message}");
            }
        }

    }
}