namespace Ecommerce.Models.User.AboutAndContact
{
    public class GetinTouchModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool replied { get; set; }
    }
}
