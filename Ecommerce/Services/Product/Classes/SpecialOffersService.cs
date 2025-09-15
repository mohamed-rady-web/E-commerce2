using Ecommerce.Data;
using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.Services.Product.Classes
{
    public class SpecialOffersService : ISpecialOffersService
    {
        private readonly ApplicationDbContext _context;

        public SpecialOffersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SpecialOfferDto> CreateSpecialOfferAsync(AddSpecialOfferDto dto)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .FirstOrDefaultAsync(so => so.Title.ToLower() == dto.Title.ToLower());
                if (exitingOffer is not null)
                {
                    return new SpecialOfferDto
                    {
                        Message = "This Special Offer already exists",
                        Id = exitingOffer.Id,
                        Title = exitingOffer.Title,
                        Description = exitingOffer.Description,
                        DiscountPercentage = exitingOffer.DiscountPercentage,
                        IsActive = exitingOffer.IsActive,
                        StartDate = exitingOffer.StartDate,
                        EndDate = exitingOffer.EndDate,
                        Products = dto.Products.Select(p => new ProductDto
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            ImageUrl = p.ImageUrl,
                            Stock = p.Stock,
                            Rating = p.Rating,
                            CategoryId = p.CategoryId,
                            CategoryName = p.CategoryName,
                            SpecialOfferId = p.SpecialOfferId,
                            SpecialOfferName = p.SpecialOfferName
                        }).ToList()
                    };
                }


                var specialOffer = new SpecialOffersModel
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    DiscountPercentage = dto.DiscountPercentage,
                    IsActive = dto.IsActive,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Products = dto.Products.Select(p => new ProductModel
                    {
                        Id = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        SpecialOfferId = null
                    }).ToList()
                };

                _context.SpecialOffers.Add(specialOffer);
                await _context.SaveChangesAsync();
                return new SpecialOfferDto
                {
                    Message = "Special Offer Created Successfully",
                    Id = specialOffer.Id,
                    Title = specialOffer.Title,
                    Description = specialOffer.Description,
                    DiscountPercentage = specialOffer.DiscountPercentage,
                    IsActive = specialOffer.IsActive,
                    StartDate = specialOffer.StartDate,
                    EndDate = specialOffer.EndDate,
                    Products = specialOffer.Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SpecialOfferId = p.SpecialOfferId,
                        SpecialOfferName = p.SpecialOffer?.Title
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };
            }

        }

        public async Task<SpecialOfferDto> DeleteSpecialOfferAsync(int specialOfferId)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .FirstOrDefaultAsync(so => so.Id == specialOfferId);
                if (exitingOffer is null)
                {
                    return new SpecialOfferDto
                    {
                        Message = "This Special Offer not exists"
                    };
                }
                _context.SpecialOffers.Remove(exitingOffer);
                await _context.SaveChangesAsync();
                return new SpecialOfferDto
                {
                    Message = "Special Offer Deleted Successfully",
                };
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SpecialOfferDto> DeleteUnActiveSpecialOfferAsync()
        {
            try
            {
                var unActiveOffers = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .Where(so => so.IsActive == false || so.EndDate < DateTime.UtcNow)
                    .ToListAsync();
                if (unActiveOffers is null || unActiveOffers.Count == 0)
                {
                    return new SpecialOfferDto
                    {
                        Message = "There are no inactive special offers to delete."
                    };
                }
                _context.SpecialOffers.RemoveRange(unActiveOffers);
                await _context.SaveChangesAsync();
                return new SpecialOfferDto
                {
                    Message = $"{unActiveOffers.Count} inactive special offers deleted successfully."
                };

            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<List<SpecialOfferDto>> GetActiveSpecialOffersAsync()
        {
            try
            {
                var exitingOffers = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .Where(so => so.IsActive == true && so.StartDate <= DateTime.UtcNow && so.EndDate >= DateTime.UtcNow)
                    .ToListAsync();
                if (exitingOffers is null || exitingOffers.Count == 0)
                {
                    return new List<SpecialOfferDto>
                    {
                        new SpecialOfferDto
                        {
                            Message = "No Active Offers"
                        }
                    };
                }
                return new List<SpecialOfferDto>
                    {
                        new SpecialOfferDto
                        {
                            Message = "Active Special Offers Retrieved Successfully",
                            Id = exitingOffers.First().Id,
                            Title = exitingOffers.First().Title,
                            Description = exitingOffers.First().Description,
                            DiscountPercentage = exitingOffers.First().DiscountPercentage,
                            IsActive = exitingOffers.First().IsActive,
                            StartDate = exitingOffers.First().StartDate,
                            EndDate = exitingOffers.First().EndDate,
                            Products = exitingOffers.First().Products.Select(p => new ProductDto
                            {
                                ProductId = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Price = p.Price,
                                ImageUrl = p.ImageUrl,
                                Stock = p.Stock,
                                Rating = p.Rating,
                                CategoryId = p.CategoryId,
                                CategoryName = p.Category.Name,
                                SpecialOfferId = p.SpecialOfferId,
                                SpecialOfferName = p.SpecialOffer?.Title
                            }).ToList()
                        }
                    };
            }
            catch (Exception ex)
            {
                return new List<SpecialOfferDto>
                {
                    new SpecialOfferDto
                    {
                        Message = $"Error: {ex.Message}"
                    }
                };
            }
        }

        public async Task<List<SpecialOfferDto>> GetAllSpecialOffersAsync()
        {
            try
            {
                var exitingOffers = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .ToListAsync();
                return new List<SpecialOfferDto>
                { new SpecialOfferDto{
                    Message = "Active Special Offers Retrieved Successfully",
                    Id = exitingOffers.First().Id,
                    Title = exitingOffers.First().Title,
                    Description = exitingOffers.First().Description,
                    DiscountPercentage = exitingOffers.First().DiscountPercentage,
                    IsActive = exitingOffers.First().IsActive,
                    StartDate = exitingOffers.First().StartDate,
                    EndDate = exitingOffers.First().EndDate,
                    Products = exitingOffers.First().Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SpecialOfferId = p.SpecialOfferId,
                        SpecialOfferName = p.SpecialOffer?.Title

                    }).ToList()
                }
            };
            }
            catch (Exception ex)
            {
                return new List<SpecialOfferDto>
                {
                    new SpecialOfferDto
                    {
                        Message = $"Error: {ex.Message}"
                    }
                };
            }
        }

        public async Task<SpecialOfferDto> GetSpecialOfferByIdAsync(int specialOfferId)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .FirstOrDefaultAsync(so => so.Id == specialOfferId);
                if (exitingOffer is null)
                {
                    return new SpecialOfferDto
                    {
                        Message = "This Special Offer not exists"
                    };
                }
                return new SpecialOfferDto
                {
                    Message = "Special Offer Retrieved Successfully",
                    Id = exitingOffer.Id,
                    Title = exitingOffer.Title,
                    Description = exitingOffer.Description,
                    DiscountPercentage = exitingOffer.DiscountPercentage,
                    IsActive = exitingOffer.IsActive,
                    StartDate = exitingOffer.StartDate,
                    EndDate = exitingOffer.EndDate,
                    Products = exitingOffer.Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SpecialOfferId = p.SpecialOfferId,
                        SpecialOfferName = p.SpecialOffer?.Title
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };

            }
        }

        public async Task<SpecialOfferDto> GetSpecialOfferByNameAsync(string name)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .FirstOrDefaultAsync(so => so.Title.ToLower() == name.ToLower());
                if (exitingOffer is null)
                {
                    return new SpecialOfferDto
                    {
                        Message = "This Special Offer not exists"
                    };
                }
                return new SpecialOfferDto
                {
                    Message = "Special Offer Retrieved Successfully",
                    Id = exitingOffer.Id,
                    Title = exitingOffer.Title,
                    Description = exitingOffer.Description,
                    DiscountPercentage = exitingOffer.DiscountPercentage,
                    IsActive = exitingOffer.IsActive,
                    StartDate = exitingOffer.StartDate,
                    EndDate = exitingOffer.EndDate,
                    Products = exitingOffer.Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SpecialOfferId = p.SpecialOfferId,
                        SpecialOfferName = p.SpecialOffer?.Title
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<SpecialOfferDto> UpdateSpecialOfferAsync(int specialOfferId, UpdateSpecialOfferDto dto)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .FirstOrDefaultAsync(so => so.Id == specialOfferId);
                if (exitingOffer is null)
                {
                    return new SpecialOfferDto
                    {
                        Message = "This Special Offer not exists"
                    };
                }
                var dtoSpecialoffer = typeof(UpdateSpecialOfferDto).GetProperties();
                var SpecialofferModel = typeof(SpecialOffersModel).GetProperties();
                foreach (var dtoProp in dtoSpecialoffer)
                {
                    var modelProp = SpecialofferModel.FirstOrDefault(p => p.Name == dtoProp.Name);
                    if (modelProp is not null)
                    {
                        var dtoValue = dtoProp.GetValue(dto);
                        if (dtoValue is not null)
                        {
                            modelProp.SetValue(exitingOffer, dtoValue);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return new SpecialOfferDto
                {
                    Message = "Special Offer Retrieved Successfully",
                    Id = exitingOffer.Id,
                    Title = exitingOffer.Title,
                    Description = exitingOffer.Description,
                    DiscountPercentage = exitingOffer.DiscountPercentage,
                    IsActive = exitingOffer.IsActive,
                    StartDate = exitingOffer.StartDate,
                    EndDate = exitingOffer.EndDate,
                    Products = exitingOffer.Products.Select(p => new ProductDto
                    {
                        ProductId = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Stock = p.Stock,
                        Rating = p.Rating,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.Name,
                        SpecialOfferId = p.SpecialOfferId,
                        SpecialOfferName = p.SpecialOffer?.Title
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
                return new SpecialOfferDto
                {
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
