using Ecommerce.Models.User.ServicesAndTutorials;

namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class AddMaintenanceDto
    {
       
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public TutorialsModel Tutorial { get; set; }
    }
}
