using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.GetInTouch_About;
using Ecommerce.Models.User.AboutAndContact;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class GetInTouchService : IGetInTouchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetInTouchService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetInTouchDto> SendMessageAsync(SendMessageDto dto)
        {
            try
            {
                var message = _mapper.Map<GetinTouchModel>(dto);
                message.replied = false;

                _context.ContactUs.Add(message);
                await _context.SaveChangesAsync();

                return _mapper.Map<GetInTouchDto>(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while sending message: {ex.Message}");
            }
        }

        public async Task<List<GetInTouchDto>> GetAllMessagesAsync()
        {
            try
            {
                var messages = await _context.ContactUs
                    .OrderByDescending(m => m.Id)
                    .ToListAsync();

                return _mapper.Map<List<GetInTouchDto>>(messages);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving messages: {ex.Message}");
            }
        }

        public async Task<GetInTouchDto> GetMessageByIdAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactUs
                    .FirstOrDefaultAsync(m => m.Id == messageId);

                if (message == null)
                    throw new Exception("Message not found.");

                return _mapper.Map<GetInTouchDto>(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving message: {ex.Message}");
            }
        }

        public async Task<GetInTouchDto> DeleteMessageAsync(int messageId)
        {
            try
            {
                var message = await _context.ContactUs
                    .FirstOrDefaultAsync(m => m.Id == messageId);

                if (message == null)
                    throw new Exception("Message not found.");

                _context.ContactUs.Remove(message);
                await _context.SaveChangesAsync();

                return _mapper.Map<GetInTouchDto>(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting message: {ex.Message}");
            }
        }
    }
}
