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

        public BookService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<BookingDto> addReservation(AddBookingDto dto)
        {
            try
            {
                var exitingBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.ServiceId == dto.ServiceId && b.User.Id == b.UserId);
                if (exitingBooking is null) { return new BookingDto { Message = "You already have book this Service" }; }
                var dateExists = await _context.AvaliableDates.AnyAsync(d => d.ServiceId == dto.ServiceId && d.Date == dto.BookingDate && d.IsBooked == true);
                if (dateExists) { return new BookingDto { Message = "This date is not available, please choose another date" }; }
                var reservation = new BookingModel
                {
                    UserId = exitingBooking.UserId,
                    ServiceId = dto.ServiceId,
                    BookingDate = dto.BookingDate,
                    Status = "Pending",
                    IsAvaliable = true,
                };
                _context.Bookings.Add(reservation);
                await _context.SaveChangesAsync();
                return new BookingDto { Message = "Reservation added successfully", Id = reservation.Id, UserId = reservation.UserId, ServiceId = reservation.ServiceId, BookingDate = reservation.BookingDate, Status = reservation.Status, IsAvaliable = reservation.IsAvaliable };

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
                if (service is null) { return new BookingDto { Message = "Service not found" }; }
                var dateExists = await _context.AvaliableDates.AnyAsync(d => d.ServiceId == dto.ServiceId && d.Date == dto.Date);
                if (dateExists)
                {
                    return new BookingDto
                    {
                        Message = "This date already exists for this service"
                    };
                }
                var availableDate = new AvaliableDatesModel
                {
                    ServiceId = dto.ServiceId,
                    Date = dto.Date,
                    IsBooked = false
                };
                _context.AvaliableDates.Add(availableDate);
                await _context.SaveChangesAsync();
                return new BookingDto {
                    Message = "Available date added successfully", Id = availableDate.Id, ServiceId = availableDate.ServiceId, BookingDate = availableDate.Date, IsAvaliable = !availableDate.IsBooked };
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
                if (reservation is null) { return new BookingDto { Message = "Reservation not found" }; }
                reservation.Status = "Cancelled";
                _context.Bookings.Update(reservation);
                await _context.SaveChangesAsync();
                return new BookingDto { Message = "Reservation cancelled successfully", Id = reservation.Id, UserId = reservation.UserId, ServiceId = reservation.ServiceId, BookingDate = reservation.BookingDate, Status = reservation.Status, IsAvaliable = reservation.IsAvaliable };

            }catch(Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<List<BookingDto>> getAllReservationDates(int reservationId)
        {
            try
            {
                var reservation = await _context.Bookings.Include(b => b.AvaliableDates).FirstOrDefaultAsync(b => b.Id == reservationId);
                if (reservation is null)
                {
                    return new List<BookingDto> { new BookingDto { Message = "Reservation not found" } };
                }
                return new List<BookingDto> {
                    new BookingDto {
                        Message = "Reservation dates retrieved successfully",
                        Id = reservation.Id,
                        UserId = reservation.UserId,
                        ServiceId = reservation.ServiceId,
                        BookingDate = reservation.BookingDate,
                        Status = reservation.Status,
                        IsAvaliable = reservation.IsAvaliable,
                        AvaliableDates = reservation.AvaliableDates
            .Select(d => new AddAvailableDatesDto
            {

                ServiceId = d.ServiceId,
                Date = d.Date,
                IsBooked = d.IsBooked
            })
            .ToList()
                    }
                };

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
                var user= await _context.Users.FirstOrDefaultAsync(u=>u.Id==userId);
                var IsInRole=await _userManager.IsInRoleAsync(user,"Admin");
                if (IsInRole)
                {
                    var reservations = await _context.Bookings.Include(b => b.Services).Include(b => b.User).ToListAsync();
                    return reservations.Select(r => new BookingDto
                    {
                        Message = "Reservations retrieved successfully",
                        Id = r.Id,
                        UserId = r.UserId,
                        User = r.User,
                        ServiceId = r.ServiceId,
                        Service = new Dtos.User.TutorialsAndMaintenance.MaintenanceServiceDto
                        {
                            Id = r.Services.Id,
                            ServiceName = r.Services.ServiceName,
                            Description = r.Services.Description,
                            Price = r.Services.Price
                        },
                        BookingDate = r.BookingDate,
                        Status = r.Status,
                        IsAvaliable = r.IsAvaliable
                    }).ToList();
                }
                else
                {
                    var reservations = await _context.Bookings.Include(b => b.Services).Include(b => b.User).Where(b => b.UserId == userId).ToListAsync();
                    return reservations.Select(r => new BookingDto
                    {
                        Message = "Reservations retrieved successfully",
                        Id = r.Id,
                        UserId = r.UserId,
                        User = r.User,
                        ServiceId = r.ServiceId,
                        Service = new Dtos.User.TutorialsAndMaintenance.MaintenanceServiceDto
                        {
                            Id = r.Services.Id,
                            ServiceName = r.Services.ServiceName,
                            Description = r.Services.Description,
                            Price = r.Services.Price
                        },
                        BookingDate = r.BookingDate,
                        Status = r.Status,
                        IsAvaliable = r.IsAvaliable
                    }).ToList();
                }
        }catch (Exception ex)
            {
                return new List<BookingDto> { new BookingDto { Message = $"An error occurred: {ex.Message}" } };
            }
        }

        public async Task<BookingDto> getReservationById(string userId, int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var IsInRole = await _userManager.IsInRoleAsync(user, "Admin");
                if (IsInRole)
                {
                    var reservation = await _context.Bookings.Include(b => b.Services).Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
                    if (reservation is null) { return new BookingDto { Message = "Reservation not found" }; }
                    return new BookingDto
                    {
                        Message = "Reservation retrieved successfully",
                        BookingDate = reservation.BookingDate,
                        Id = reservation.Id,
                        UserId = reservation.UserId,
                        ServiceId = reservation.ServiceId,
                    };
                }
                var res = await _context.Bookings.Include(b => b.Services).Include(b => b.User).Where(u => u.UserId == userId).FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
                if (res is null) { return new BookingDto { Message = "Reservation not found" }; }
                return new BookingDto
                {
                    Message = "Reservation retrieved successfully",
                    BookingDate = res.BookingDate,
                    Id = res.Id,
                    UserId = res.UserId,
                    ServiceId = res.ServiceId,
                };


            }
            catch (Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<List<BookingDto>> getReservationByName(string FirstName)
        {
            try
            {
                var reservations = await _context.Bookings.Include(b => b.Services).Include(b => b.User).Where(b => b.User.FirstName == FirstName).Take(8).ToListAsync();
                if (reservations is null)
                {
                    return new List<BookingDto> { new BookingDto { Message = "No reservations found for this name" } };

                }
                return reservations.Select(r => new BookingDto
                {
                    Message = "Reservations retrieved successfully",
                    Id = r.Id,
                    UserId = r.UserId,
                    User = r.User,
                    ServiceId = r.ServiceId,
                    Service = new Dtos.User.TutorialsAndMaintenance.MaintenanceServiceDto
                    {
                        Id = r.Services.Id,
                        ServiceName = r.Services.ServiceName,
                        Description = r.Services.Description,
                        Price = r.Services.Price
                    },
                    BookingDate = r.BookingDate,
                    Status = r.Status,
                    IsAvaliable = r.IsAvaliable
                }).ToList();


            }
            catch (Exception ex)
            {
                return new List<BookingDto> { new BookingDto { Message = $"An error occurred: {ex.Message}" } };
            }
        }

        public async Task<BookingDto> updateReservation(int id, UpdateBookingDto dto)
        {
            try { 
            var reservation = await _context.Bookings.FindAsync(id);
            if (reservation == null) { 
            return new BookingDto { Message = "Reservation not found" };
            }
            reservation.BookingDate = dto.BookingDate;
            return new BookingDto { Message = "Reservation updated successfully", Id = reservation.Id, UserId = reservation.UserId, ServiceId = reservation.ServiceId, BookingDate = reservation.BookingDate, Status = reservation.Status, IsAvaliable = reservation.IsAvaliable };
        }catch(Exception ex)
            {
                return new BookingDto { Message = $"An error occurred: {ex.Message}" };
            }
        }

    }
}
