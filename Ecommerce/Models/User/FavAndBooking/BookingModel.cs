using Ecommerce.Models.User.ServicesAndTutorials;

namespace Ecommerce.Models.User.FavAndBooking
{
    public class BookingModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ServiceId { get; set; }
        public ApplicationUser User { get; set; }
        public string ServiceName { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public bool IsAvaliable { get; set; }
        public MaintenanceServicesModel Services { get; set; }
        public ICollection <AvaliableDatesModel> AvaliableDates { get; set; }
    }
}
