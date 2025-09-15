namespace Ecommerce.Dtos.User.Reviews
{
    public class AddReviewDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
    }
}
