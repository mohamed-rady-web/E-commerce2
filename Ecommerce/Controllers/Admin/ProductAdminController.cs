using Ecommerce.Dtos.Products;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductAdminController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductAdminController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addProduct")]

        public async Task<IActionResult> AddProduct([FromBody] AddProductDto dto)
        {
            
                var product = await _productService.CreateProductAsync(dto);
                return Ok(product);
            
        }
        
        [HttpPost("updateProduct/{productId}")]

        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto dto)
        {
           
                var product = await _productService.UpdateProductAsync(productId, dto);
                return Ok(product);
           
        }

        [HttpDelete("deleteProduct/{productId}")]

        public async Task<IActionResult> DeleteProduct(int productId)
        {
           
                var result = await _productService.DeleteProductAsync(productId);
                if (result is not null)
                {
                    return Ok(result);
                }
                return NotFound(new { Message = "Product not found" });
            
        }
    }
}
