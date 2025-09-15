using Ecommerce.Dtos.CheckOut;

namespace Ecommerce.Services.Order
{
    public interface ICheckOutService
    {
        public Task<CheckOutDto> ProcessCheckOutAsync(StartCheckOutDto dto);
        public Task<CheckOutDto> GetCheckOutDetailsAsync(int CheckOutId);
        public Task<CheckOutDto> ConfirmPaymentAsync(int CheckOutId,string newstatus);
        public Task<CheckOutDto> CancelOrderAsync(int CheckOutId);
        public Task<List<CheckOutDto>> GetUserCheckOutOrdersAsync(string userId);
        public Task<List<CheckOutDto>> GetAllOrdersAsync();

    }
}
