using Ecommerce.Services.Product.InterFaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesAdminController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesAdminController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory([FromBody] Dtos.Products.AddCategoryDto dto)
        {
            var category = await _categoriesService.AddCategoryAsync(dto);
            return Ok(category);
        }
        [HttpPut("updateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] Dtos.Products.UpdateCategoryDto dto)
        {
            var category = await _categoriesService.UpdateCategoryAsync(categoryId, dto);
            return Ok(category);
        }
        [HttpDelete("deleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _categoriesService.DeleteCategoryAsync(categoryId);
            if (category is not null)
            {
                return Ok(category);
            }
            return NotFound(new { Message = "Category not found" });
        }
    }
}
