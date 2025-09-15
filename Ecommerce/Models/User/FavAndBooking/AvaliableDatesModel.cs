namespace Ecommerce.Models.User.FavAndBooking
{
    public class AvaliableDatesModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BookingId { get; set; }
        public BookingModel Booking { get; set; }
        public bool IsBooked { get; set; }
        public int ServiceId { get; set; }
    }
}
