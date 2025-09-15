using Ecommerce.Data;
using Ecommerce.Dtos.User.GetInTouch_About;
using Ecommerce.Models.User.AboutAndContact;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.AboutAndContact
{
    public class GetInTouchService : IGetInTouchService
    {
        private readonly ApplicationDbContext _context;

        public GetInTouchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetInTouchDto> SendMessageAsync(SendMessageDto dto)
        {
            try
            {
                var message = new GetinTouchModel
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Subject = dto.Subject,
                    Message = dto.Message,
                    replied = false
                };

                _context.ContactUs.Add(message);
                await _context.SaveChangesAsync();

                return new GetInTouchDto
                {
                    Id = message.Id,
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    Email = message.Email,
                    Subject = message.Subject,
                    Message = message.Message,
                    replied = message.replied
                };
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

                return messages.Select(m => new GetInTouchDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Subject = m.Subject,
                    Message = m.Message,
                    replied = m.replied
                }).ToList();
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

                return new GetInTouchDto
                {
                    Id = message.Id,
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    Email = message.Email,
                    Subject = message.Subject,
                    Message = message.Message,
                    replied = message.replied
                };
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

                return new GetInTouchDto
                {
                    Id = message.Id,
                    FirstName = message.FirstName,
                    LastName = message.LastName,
                    Email = message.Email,
                    Subject = message.Subject,
                    Message = message.Message,
                    replied = message.replied
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting message: {ex.Message}");
            }
        }
    }
}
