namespace Ecommerce.Dtos.User.GetInTouch_About
{
    public class GetInTouchDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool replied { get; set; }= false;
    }
}
