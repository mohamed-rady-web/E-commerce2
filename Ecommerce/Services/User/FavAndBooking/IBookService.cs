using Ecommerce.Dtos.User.Reservation;

namespace Ecommerce.Services.User.FavAndBooking
{
    public interface IBookService
    {
        public Task<BookingDto> addReservation(AddBookingDto dto);
        public Task<List<BookingDto>> getAllReservations(string userId);
        public Task<BookingDto> getReservationById(string userId,int id);
        public Task<List<BookingDto>> getReservationByName(string FirstName);
        public Task<List<BookingDto>> getAllReservationDates(int reservationId);
        public Task<BookingDto> updateReservation(int id, UpdateBookingDto dto);
        public Task<BookingDto> CancelReservation(int id);
        public Task<BookingDto> AddServiceDate(AddAvailableDatesDto dto);

    }
}
