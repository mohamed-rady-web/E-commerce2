using Ecommerce.Dtos.Orders;
using Ecommerce.Models.Order;
using Ecommerce.Models.User;

namespace Ecommerce.Dtos.CheckOut
{
    public class CheckOutDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }

        public string PaymentMethod { get; set; } = "In Hand";

        public double TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public ICollection<OrderDto> Orders { get; set; } 
        public string Message { get;set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
    }


}
