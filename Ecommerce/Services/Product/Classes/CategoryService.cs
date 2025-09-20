using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> AddCategoryAsync(AddCategoryDto dto)
        {
            try
            {
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == dto.Name.ToLower());

                if (existingCategory is not null)
                {
                    var existingDto = _mapper.Map<CategoryDto>(existingCategory);
                    existingDto.Message = "This Category already exists";
                    return existingDto;
                }

                var category = _mapper.Map<CategoryModel>(dto);

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var categoryDto = _mapper.Map<CategoryDto>(category);
                categoryDto.Message = "Category added successfully";
                return categoryDto;
            }
            catch (Exception ex)
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
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (existingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category does not exist"
                    };
                }

                _context.Categories.Remove(existingCategory);
                await _context.SaveChangesAsync();

                return new CategoryDto
                {
                    Message = "Category deleted successfully"
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
                return _mapper.Map<List<CategoryDto>>(categories);
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
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (existingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category does not exist"
                    };
                }

                var dto = _mapper.Map<CategoryDto>(existingCategory);
                dto.Message = "This Category is found";
                return dto;
            }
            catch (Exception ex)
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
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

                if (existingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category does not exist"
                    };
                }

                var dto = _mapper.Map<CategoryDto>(existingCategory);
                dto.Message = "This Category is found";
                return dto;
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
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (existingCategory is null)
                {
                    return new CategoryDto
                    {
                        Message = "This Category does not exist"
                    };
                }

                _mapper.Map(dto, existingCategory);
                await _context.SaveChangesAsync();

                var categoryDto = _mapper.Map<CategoryDto>(existingCategory);
                categoryDto.Message = "Category updated successfully";
                return categoryDto;
            }
            catch (Exception ex)
            {
                return new CategoryDto
                {
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
