using Ecommerce.Dtos.User.GetInTouch_About;

namespace Ecommerce.Services.User.AboutAndContact
{
    public interface IGetInTouchService
    {
        public Task<GetInTouchDto> SendMessageAsync(SendMessageDto dto);
        public Task<List<GetInTouchDto>> GetAllMessagesAsync();
        public Task <GetInTouchDto> DeleteMessageAsync(int messageId);
        public Task <GetInTouchDto> GetMessageByIdAsync(int messageId);

    }
}
