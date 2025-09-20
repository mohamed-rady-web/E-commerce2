using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.Products;
using Ecommerce.Models.Product;
using Ecommerce.Services.Product.InterFaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Product.Classes
{
    public class SpecialOffersService : ISpecialOffersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialOffersService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SpecialOfferDto> CreateSpecialOfferAsync(AddSpecialOfferDto dto)
        {
            try
            {
                var exitingOffer = await _context.SpecialOffers
                    .FirstOrDefaultAsync(so => so.Title.ToLower() == dto.Title.ToLower());

                if (exitingOffer is not null)
                {
                    var result = _mapper.Map<SpecialOfferDto>(exitingOffer);
                    result.Message = "This Special Offer already exists";
                    return result;
                }

                var specialOffer = _mapper.Map<SpecialOffersModel>(dto);
                _context.SpecialOffers.Add(specialOffer);
                await _context.SaveChangesAsync();

                var createdDto = _mapper.Map<SpecialOfferDto>(specialOffer);
                createdDto.Message = "Special Offer Created Successfully";
                return createdDto;
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
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
                    return new SpecialOfferDto { Message = "This Special Offer not exists" };
                }

                _context.SpecialOffers.Remove(exitingOffer);
                await _context.SaveChangesAsync();

                return new SpecialOfferDto { Message = "Special Offer Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<SpecialOfferDto> DeleteUnActiveSpecialOfferAsync()
        {
            try
            {
                var unActiveOffers = await _context.SpecialOffers
                    .Include(so => so.Products)
                    .Where(so => !so.IsActive || so.EndDate < DateTime.UtcNow)
                    .ToListAsync();

                if (unActiveOffers is null || !unActiveOffers.Any())
                {
                    return new SpecialOfferDto { Message = "There are no inactive special offers to delete." };
                }

                _context.SpecialOffers.RemoveRange(unActiveOffers);
                await _context.SaveChangesAsync();

                return new SpecialOfferDto { Message = $"{unActiveOffers.Count} inactive special offers deleted successfully." };
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<List<SpecialOfferDto>> GetActiveSpecialOffersAsync()
        {
            try
            {
                var offers = await _context.SpecialOffers
                    .Include(so => so.Products).ThenInclude(p => p.Category)
                    .Where(so => so.IsActive && so.StartDate <= DateTime.UtcNow && so.EndDate >= DateTime.UtcNow)
                    .ToListAsync();

                if (offers is null || !offers.Any())
                {
                    return new List<SpecialOfferDto> { new SpecialOfferDto { Message = "No Active Offers" } };
                }

                var mapped = _mapper.Map<List<SpecialOfferDto>>(offers);
                mapped.First().Message = "Active Special Offers Retrieved Successfully";
                return mapped;
            }
            catch (Exception ex)
            {
                return new List<SpecialOfferDto> { new SpecialOfferDto { Message = $"Error: {ex.Message}" } };
            }
        }

        public async Task<List<SpecialOfferDto>> GetAllSpecialOffersAsync()
        {
            try
            {
                var offers = await _context.SpecialOffers
                    .Include(so => so.Products).ThenInclude(p => p.Category)
                    .ToListAsync();

                var mapped = _mapper.Map<List<SpecialOfferDto>>(offers);
                if (mapped.Any())
                    mapped.First().Message = "All Special Offers Retrieved Successfully";

                return mapped;
            }
            catch (Exception ex)
            {
                return new List<SpecialOfferDto> { new SpecialOfferDto { Message = $"Error: {ex.Message}" } };
            }
        }

        public async Task<SpecialOfferDto> GetSpecialOfferByIdAsync(int specialOfferId)
        {
            try
            {
                var offer = await _context.SpecialOffers
                    .Include(so => so.Products).ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(so => so.Id == specialOfferId);

                if (offer is null)
                    return new SpecialOfferDto { Message = "This Special Offer not exists" };

                var mapped = _mapper.Map<SpecialOfferDto>(offer);
                mapped.Message = "Special Offer Retrieved Successfully";
                return mapped;
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<SpecialOfferDto> GetSpecialOfferByNameAsync(string name)
        {
            try
            {
                var offer = await _context.SpecialOffers
                    .Include(so => so.Products).ThenInclude(p => p.Category)
                    .FirstOrDefaultAsync(so => so.Title.ToLower() == name.ToLower());

                if (offer is null)
                    return new SpecialOfferDto { Message = "This Special Offer not exists" };

                var mapped = _mapper.Map<SpecialOfferDto>(offer);
                mapped.Message = "Special Offer Retrieved Successfully";
                return mapped;
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
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
                    return new SpecialOfferDto { Message = "This Special Offer not exists" };

                _mapper.Map(dto, exitingOffer);
                await _context.SaveChangesAsync();

                var mapped = _mapper.Map<SpecialOfferDto>(exitingOffer);
                mapped.Message = "Special Offer Updated Successfully";
                return mapped;
            }
            catch (Exception ex)
            {
                return new SpecialOfferDto { Message = $"Error: {ex.Message}" };
            }
        }
    }
}
