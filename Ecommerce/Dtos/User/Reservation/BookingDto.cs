using Ecommerce.Dtos.User.TutorialsAndMaintenance;
using Ecommerce.Models.User;
using Ecommerce.Models.User.ServicesAndTutorials;

namespace Ecommerce.Dtos.User.Reservation
{
    public class BookingDto
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public bool IsAvaliable { get; set; }
        public int ServiceId { get; set; }
        public MaintenanceServiceDto Service { get; set; }
        public ICollection<AddAvailableDatesDto> AvaliableDates { get; set; }
    }
}
