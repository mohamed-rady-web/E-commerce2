using Ecommerce.Dtos.Products;

namespace Ecommerce.Services.Product.InterFaces
{
    public interface ISpecialOffersService
    {
        public Task<List<SpecialOfferDto>> GetAllSpecialOffersAsync();
        public Task<SpecialOfferDto> GetSpecialOfferByIdAsync(int specialOfferId);
        public Task<SpecialOfferDto> CreateSpecialOfferAsync(AddSpecialOfferDto dto);
        public Task<SpecialOfferDto> UpdateSpecialOfferAsync(int specialOfferId, UpdateSpecialOfferDto dto);
        public Task<SpecialOfferDto> DeleteSpecialOfferAsync(int specialOfferId);
        public Task<SpecialOfferDto> GetSpecialOfferByNameAsync(string name);
        public Task<List<SpecialOfferDto>> GetActiveSpecialOffersAsync();
        public Task<SpecialOfferDto> DeleteUnActiveSpecialOfferAsync();

    }
}
