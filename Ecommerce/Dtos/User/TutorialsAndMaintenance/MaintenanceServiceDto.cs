namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class MaintenanceServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Message { get; set; }
        public TutorialsDto Tutorial { get; set; }
    }
}
