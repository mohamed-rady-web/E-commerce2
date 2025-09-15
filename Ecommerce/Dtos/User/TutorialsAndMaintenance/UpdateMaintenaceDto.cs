namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class UpdateMaintenaceDto
    {

        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
