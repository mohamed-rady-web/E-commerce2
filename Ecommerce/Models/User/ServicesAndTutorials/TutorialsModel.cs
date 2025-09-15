namespace Ecommerce.Models.User.ServicesAndTutorials
{
    public class TutorialsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string coverImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public int ServiceId { get; set; }
        public MaintenanceServicesModel RelatedService { get; set; }
    }
}
