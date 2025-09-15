namespace Ecommerce.Dtos.Auth
{
    public class AuthDto
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime ExpireIn { get; set; }
        public IList<string> Roles { get; set; }
    }
}
