namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class AddTutorialDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string coverImageUrl{ get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int MaintenanceServiceId { get; set; }
    }
}
