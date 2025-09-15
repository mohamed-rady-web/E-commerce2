using Ecommerce.Models.User.FavAndBooking;

namespace Ecommerce.Models.User.ServicesAndTutorials
{
    public class MaintenanceServicesModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<BookingModel> Bookings { get; set; }
        public int TutorialId { get; set; }
        public TutorialsModel Tutorial { get; set; }
    }
}
