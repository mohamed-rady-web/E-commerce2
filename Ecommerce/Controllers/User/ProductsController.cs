using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getAllProduct")]
        
        public async Task<IActionResult> GetAllProduct()
        {

            var products = await _productService.GetAllProductsAsync();
            return Ok(products);

        }
        
        [HttpGet("getProductById/{productId}")]
        
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            return Ok(product);

        }
        
        [HttpGet("getProductsByCategory/{categoryId}")]
        
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {

            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);

        }
    
        [HttpGet("getRelatedProducts/{productId}")]
        
        public async Task<IActionResult> GetRelatedProducts(int productId)
        {
            var products = await _productService.GetRelatedProductsAsync(productId);
            return Ok(products);
        }

        [HttpGet("getProductsBySpecialOffer/{specialOfferId}")]
        
        public async Task<IActionResult> GetProductsBySpecialOffer(int specialOfferId)
        {
            var products = await _productService.GetProductsBySpecialOfferAsync(specialOfferId);
            return Ok(products);
        }

        [HttpGet("exploreOurProducts")]
        
        public async Task<IActionResult> ExploreOurProducts()
        {
            var products = await _productService.ExploreOurProductsAsync();
            return Ok(products);

        }
        
    }
}
