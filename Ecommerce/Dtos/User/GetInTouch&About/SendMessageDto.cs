using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos.User.GetInTouch_About
{
    public class SendMessageDto
    {

        [Required, MaxLength(20), MinLength(2)]
        public string FirstName { get; set; }
        [Required, MaxLength(20), MinLength(2)]
        public string LastName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, MaxLength(200), MinLength(10)]
        public string Subject { get; set; }
        [Required, MaxLength(250), MinLength(15)]
        public string Message { get; set; }
    }
}
