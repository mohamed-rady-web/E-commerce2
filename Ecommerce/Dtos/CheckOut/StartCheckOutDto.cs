using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos.CheckOut
{
    public class StartCheckOutDto
    {
        [Required]
        public string UserId { get; set; }
        [Required, MaxLength(20), MinLength(2)]
        public string FirstName { get; set; }
        [Required, MaxLength(20), MinLength(2)]
        public string LastName { get; set; }
        [Required, MaxLength(200), MinLength(20)]
        public string ShippingAddress { get; set; }
        [Required, MaxLength(200), MinLength(20)]
        public string BillingAddress { get; set; }
        [Required]
        public string PaymentMethod { get; set; } = "In Hand";
        public double TotalAmount { get; set; }
        public string Status { get;set; }
    }
}
