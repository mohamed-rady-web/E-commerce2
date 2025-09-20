using AutoMapper;
using Ecommerce.Data;
using Ecommerce.Dtos.User.Reservation;
using Ecommerce.Models.User;
using Ecommerce.Models.User.FavAndBooking;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.User.FavAndBooking
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BookService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingDto> addReservation(AddBookingDto dto)
        {
            try
            {
                var exitingBooking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.ServiceId == dto.ServiceId && b.User.Id == b.UserId);

                if (exitingBooking is null)
                {
                    return new BookingDto { Message = "You already have book this Service" };
                }

                var dateExists = await _context.AvaliableDates
                    .AnyAsync(d => d.ServiceId == dto.ServiceId && d.Date == dto.BookingDate && d.IsBooked);

                if (dateExists)
                {
                    return new BookingDto { Message = "This date is not available, please choose another date" };
                }

                var reservation = _mapper.Map<BookingModel>(dto);
                reservation.UserId = exitingBooking.UserId;
                reservation.Status = "Pending";
                reservation.IsAvaliable = true;

                _context.Bookings.Add(reservation);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<BookingDto>(reservation);
                result.Message = "Reservation added successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<BookingDto> AddServiceDate(AddAvailableDatesDto dto)
        {
            try
            {
                var service = await _context.MaintenanceServices.FindAsync(dto.ServiceId);
                if (service is null)
                {
                    return new BookingDto { Message = "Service not found" };
                }

                var dateExists = await _context.AvaliableDates
                    .AnyAsync(d => d.ServiceId == dto.ServiceId && d.Date == dto.Date);

                if (dateExists)
                {
                    return new BookingDto { Message = "This date already exists for this service" };
                }

                var availableDate = _mapper.Map<AvaliableDatesModel>(dto);
                _context.AvaliableDates.Add(availableDate);
                await _context.SaveChangesAsync();

                return new BookingDto
                {
                    Message = "Available date added successfully",
                    Id = availableDate.Id,
                    ServiceId = availableDate.ServiceId,
                    BookingDate = availableDate.Date,
                    IsAvaliable = !availableDate.IsBooked
                };
            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<BookingDto> CancelReservation(int id)
        {
            try
            {
                var reservation = await _context.Bookings.FindAsync(id);
                if (reservation is null)
                {
                    return new BookingDto { Message = "Reservation not found" };
                }

                reservation.Status = "Cancelled";
                _context.Bookings.Update(reservation);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<BookingDto>(reservation);
                result.Message = "Reservation cancelled successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<List<BookingDto>> getAllReservationDates(int reservationId)
        {
            try
            {
                var reservation = await _context.Bookings
                    .Include(b => b.AvaliableDates)
                    .FirstOrDefaultAsync(b => b.Id == reservationId);

                if (reservation is null)
                {
                    return new List<BookingDto> { new BookingDto { Message = "Reservation not found" } };
                }

                var result = _mapper.Map<BookingDto>(reservation);
                result.Message = "Reservation dates retrieved successfully";
                return new List<BookingDto> { result };
            }
            catch (Exception ex)
            {
                return new List<BookingDto> { new BookingDto { Message = $"An error occurred: {ex.Message}" } };
            }
        }

        public async Task<List<BookingDto>> getAllReservations(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                var reservationsQuery = _context.Bookings
                    .Include(b => b.Services)
                    .Include(b => b.User);

                var reservations = isInRole
                    ? await reservationsQuery.ToListAsync()
                    : await reservationsQuery.Where(b => b.UserId == userId).ToListAsync();

                var result = _mapper.Map<List<BookingDto>>(reservations);
                result.ForEach(r => r.Message = "Reservations retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                return new List<BookingDto> { new BookingDto { Message = $"An error occurred: {ex.Message}" } };
            }
        }

        public async Task<BookingDto> getReservationById(string userId, int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                var reservationQuery = _context.Bookings
                    .Include(b => b.Services)
                    .Include(b => b.User);

                var reservation = isInRole
                    ? await reservationQuery.FirstOrDefaultAsync(b => b.Id == id)
                    : await reservationQuery.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

                if (reservation is null)
                {
                    return new BookingDto { Message = "Reservation not found" };
                }

                var result = _mapper.Map<BookingDto>(reservation);
                result.Message = "Reservation retrieved successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<List<BookingDto>> getReservationByName(string firstName)
        {
            try
            {
                var reservations = await _context.Bookings
                    .Include(b => b.Services)
                    .Include(b => b.User)
                    .Where(b => b.User.FirstName == firstName)
                    .Take(8)
                    .ToListAsync();

                if (reservations == null || !reservations.Any())
                {
                    return new List<BookingDto> { new BookingDto { Message = "No reservations found for this name" } };
                }

                var result = _mapper.Map<List<BookingDto>>(reservations);
                result.ForEach(r => r.Message = "Reservations retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                return new List<BookingDto> { new BookingDto { Message = $"An error occurred: {ex.Message}" } };
            }
        }

        public async Task<BookingDto> updateReservation(int id, UpdateBookingDto dto)
        {
            try
            {
                var reservation = await _context.Bookings.FindAsync(id);
                if (reservation == null)
                {
                    return new BookingDto { Message = "Reservation not found" };
                }

                _mapper.Map(dto, reservation);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<BookingDto>(reservation);
                result.Message = "Reservation updated successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }
    }
}
