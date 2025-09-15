using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos.Auth
{
    public class RegisterDto
    {
        [Required,MaxLength(50),MinLength(2)]
        public string UserName { get; set; }
        [Required,MaxLength(50),MinLength(6),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required,MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }
        [Required, MaxLength(50), MinLength(2)]
        public string LastName { get; set; }
    }
}
