using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        [HttpGet("getCategoryById/{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);
            if (category is not null)
            {
                return Ok(category);
            }
            return NotFound(new { Message = "Category not found" });
        }
        [HttpGet("getAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("getCategoryByName/{name}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(categoryId);
            return Ok(category);
        }
    }
}
