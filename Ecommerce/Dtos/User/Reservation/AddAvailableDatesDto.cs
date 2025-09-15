namespace Ecommerce.Dtos.User.Reservation
{
    public class AddAvailableDatesDto
    {
        public DateTime Date { get; set; }
        public int ServiceId { get; set; }
        public bool IsBooked { get; set; }=false;
    }
}
