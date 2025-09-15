namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class TutorialsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string coverImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Message { get; set; }
        public MaintenanceServiceDto RelatedService { get; set; }
    }
}
