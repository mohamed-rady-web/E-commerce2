using Ecommerce.Data;
using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Product.Classes
{
    public class CategoryService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> AddCategoryAsync(AddCategoryDto dto)
        {
            try { 
            var exitingCategory= await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());
            if (exitingCategory is not null)
            {
                return new CategoryDto
                {
                    Message = "This Category is already exist",
                    Id = exitingCategory.Id,
                    Name = exitingCategory.Name,
                    Description = exitingCategory.Description,
                    ImageUrl = exitingCategory.ImageUrl
                };
            }
                var category = new CategoryModel
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl
                };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new CategoryDto
            {
                Message = "Category added successfully",
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
        }catch(Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<CategoryDto> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var exitingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (exitingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category is not exist"
                    };
                }
                _context.Categories.Remove(exitingCategory);
                await _context.SaveChangesAsync();
                return new CategoryDto
                {
                    Message = "Category deleted successfully",
                };
            }
            catch (Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                return categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<CategoryDto>
        {
            new CategoryDto
            {
                Message = $"An error occurred: {ex.Message}"
            }
        };
            }
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var exitingCategory = await  _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (exitingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category is not exist"
                    };
                }
                return new CategoryDto
                {
                    Message="This Category is  Found",
                    Id = exitingCategory.Id,
                    Name = exitingCategory.Name,
                    Description = exitingCategory.Description,
                    ImageUrl = exitingCategory.ImageUrl
                };
            }catch(Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<CategoryDto> GetCategoryByNameAsync(string name)
        {
            try
            {
                var exitingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
                if (exitingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category is not exist"
                    };
                }
                return new CategoryDto
                {
                    Message = "This Category is Found",
                    Id = exitingCategory.Id,
                    Name = exitingCategory.Name,
                    Description = exitingCategory.Description,
                    ImageUrl = exitingCategory.ImageUrl
                };

            }
            catch (Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int categoryId, UpdateCategoryDto dto)
        {
            try
            {
                var exitingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (exitingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category is not exist"
                    };
                }
                var dtoPropirtes=typeof(UpdateCategoryDto).GetProperties();
                var modelPropirtes=typeof(CategoryModel).GetProperties();
                foreach (var prop in dtoPropirtes)
                {
                    var dtoValue = prop.GetValue(dto);
                    if (dtoValue is not null)
                    {
                        var modelProp = modelPropirtes.FirstOrDefault(p => p.Name == prop.Name);
                        if (modelProp is not null)
                        {
                            modelProp.SetValue(exitingCategory, dtoValue);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return new CategoryDto
                {
                    Message = "Category updated successfully",
                    Id = exitingCategory.Id,
                    Name = exitingCategory.Name,
                    Description = exitingCategory.Description,
                    ImageUrl = exitingCategory.ImageUrl
                };

            }catch(Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
