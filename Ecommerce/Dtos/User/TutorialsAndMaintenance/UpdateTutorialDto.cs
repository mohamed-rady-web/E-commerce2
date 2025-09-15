namespace Ecommerce.Dtos.User.TutorialsAndMaintenance
{
    public class UpdateTutorialDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? coverImageUrl { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
